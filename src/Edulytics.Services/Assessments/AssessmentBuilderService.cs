using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Edulytics.Core.AssessmentIntelligence;
using Edulytics.Core.Assessments;
using Edulytics.Core.Constants;
using Edulytics.Core.Entities;
using Edulytics.Core.Enums;
using Edulytics.Core.Interfaces;
using Edulytics.Core.MathematicsGeneration;
using Edulytics.Core.Users;
using Edulytics.Services.AssessmentIntelligence;
using Edulytics.Services.MathematicsGeneration;

namespace Edulytics.Services.Assessments;

public sealed class AssessmentBuilderService(
    IAssessmentService assessments,
    IAssessmentBuilderRepository repository,
    ISchoolUserRepository users) : IAssessmentBuilderService
{
    public async Task<AssessmentQueryResult<AssessmentBuilderWorkspace>> GetWorkspaceAsync(
        Guid actorUserId,
        Guid assessmentId,
        CancellationToken cancellationToken = default)
    {
        var access = await ResolveAccessAsync(actorUserId, assessmentId, cancellationToken);
        if (access.Error.HasValue)
            return AssessmentQueryResult<AssessmentBuilderWorkspace>.Failure(access.Error.Value);

        var context = await repository.GetContextAsync(access.SchoolId, assessmentId, cancellationToken);
        if (context is null)
            return AssessmentQueryResult<AssessmentBuilderWorkspace>.Failure(AssessmentErrorCode.AssessmentNotFound);

        return AssessmentQueryResult<AssessmentBuilderWorkspace>.Success(BuildWorkspace(access.Details!, context));
    }

    public async Task<AssessmentCommandResult> CreateManualQuestionAsync(
        Guid actorUserId,
        CreateManualBuilderQuestionRequest request,
        CancellationToken cancellationToken = default)
    {
        var access = await ResolveAccessAsync(actorUserId, request.AssessmentId, cancellationToken);
        if (access.Error.HasValue) return Failure(access.Error.Value);
        var details = access.Details!;
        if (details.Assessment.Status != AssessmentStatus.Draft) return Failure(AssessmentErrorCode.AssessmentNotDraft);

        var context = await repository.GetContextAsync(access.SchoolId, request.AssessmentId, cancellationToken);
        if (context is null) return Failure(AssessmentErrorCode.AssessmentNotFound);
        if (context.CurriculumAdoption is null || string.IsNullOrWhiteSpace(context.CurriculumAdoption.CurriculumLevelKey))
            return Failure(AssessmentErrorCode.OutcomeDoesNotMatchAssessment);

        var prompt = request.Prompt?.Trim() ?? string.Empty;
        var answer = request.CorrectAnswer?.Trim() ?? string.Empty;
        var solution = request.Solution?.Trim() ?? string.Empty;
        if (prompt.Length is < 1 or > 1000 || answer.Length is < 1 or > 1000 || solution.Length is < 1 or > 4000)
            return Failure(AssessmentErrorCode.InvalidText);
        if (request.Order <= 0) return Failure(AssessmentErrorCode.InvalidOrder);
        if (request.MaxScore <= 0 || request.MaxScore > 10000) return Failure(AssessmentErrorCode.InvalidQuestionScore);
        if (context.Questions.Any(x => x.Order == request.Order)) return Failure(AssessmentErrorCode.DuplicateQuestionOrder);

        var outcomeIds = NormalizeOutcomes(request.OutcomeIds);
        if (!ValidateOutcomes(details, outcomeIds)) return Failure(AssessmentErrorCode.OutcomeDoesNotMatchAssessment);
        if (context.Questions.Sum(x => x.MaxScore) + request.MaxScore > context.Assessment.MaxScore)
            return Failure(AssessmentErrorCode.AssessmentScoreMismatch);

        var id = Guid.NewGuid();
        var now = DateTime.UtcNow;
        var item = new AssessmentItem
        {
            Id = id,
            SchoolId = access.SchoolId,
            CurriculumAdoptionId = context.CurriculumAdoption.Id,
            CurriculumTopicId = ResolveSingleTopic(context, outcomeIds),
            Source = AssessmentItemSource.TeacherCreated,
            ItemType = AssessmentItemType.ShortAnswer,
            Difficulty = request.Difficulty,
            Prompt = prompt,
            CorrectAnswer = answer,
            Solution = solution,
            CreatedByUserId = actorUserId,
            GenerationMethod = "teacher-created",
            ExposureFingerprint = Fingerprint($"teacher:{id:D}:{prompt}:{answer}"),
            ValidationMetadataJson = SetBuilderStatus(null, AssessmentBuilderQuestionStatus.Draft),
            CreatedAtUtc = now
        };

        var question = new AssessmentQuestion
        {
            Id = id,
            SchoolId = access.SchoolId,
            AssessmentId = request.AssessmentId,
            Prompt = prompt,
            MaxScore = decimal.Round(request.MaxScore, 2, MidpointRounding.AwayFromZero),
            Order = request.Order
        };

        var qMappings = outcomeIds.Select(outcomeId => new QuestionLearningOutcome
        {
            Id = Guid.NewGuid(),
            SchoolId = access.SchoolId,
            AssessmentQuestionId = id,
            LearningOutcomeId = outcomeId
        }).ToArray();
        var itemMappings = outcomeIds.Select(outcomeId => new AssessmentItemOutcome
        {
            Id = Guid.NewGuid(),
            SchoolId = access.SchoolId,
            AssessmentItemId = id,
            LearningOutcomeId = outcomeId
        }).ToArray();

        repository.AddBundle(new AssessmentBuilderQuestionBundle(question, item, qMappings, itemMappings));
        context.Assessment.UpdatedAtUtc = now;
        var saved = await repository.SaveAsync(context.Assessment, request.AssessmentRowVersion, cancellationToken);
        return saved.Succeeded ? AssessmentCommandResult.Success(id) : PersistenceFailure(saved);
    }

    public async Task<AssessmentCommandResult> GenerateQuestionsAsync(
        Guid actorUserId,
        GenerateBuilderQuestionsRequest request,
        CancellationToken cancellationToken = default)
    {
        var access = await ResolveAccessAsync(actorUserId, request.AssessmentId, cancellationToken);
        if (access.Error.HasValue) return Failure(access.Error.Value);
        var details = access.Details!;
        if (details.Assessment.Status != AssessmentStatus.Draft) return Failure(AssessmentErrorCode.AssessmentNotDraft);
        if (request.QuestionCount is < 1 or > 100 || request.MaxScorePerQuestion <= 0)
            return Failure(AssessmentErrorCode.InvalidQuestionScore);

        var context = await repository.GetContextAsync(access.SchoolId, request.AssessmentId, cancellationToken);
        if (context?.CurriculumAdoption is null || string.IsNullOrWhiteSpace(context.CurriculumAdoption.CurriculumLevelKey))
            return Failure(AssessmentErrorCode.OutcomeDoesNotMatchAssessment);

        var outcomeIds = NormalizeOutcomes(request.OutcomeIds);
        if (!ValidateOutcomes(details, outcomeIds)) return Failure(AssessmentErrorCode.OutcomeDoesNotMatchAssessment);
        var requestedMarks = request.QuestionCount * request.MaxScorePerQuestion;
        if (context.Questions.Sum(x => x.MaxScore) + requestedMarks > context.Assessment.MaxScore)
            return Failure(AssessmentErrorCode.AssessmentScoreMismatch);

        var selected = context.LearningOutcomes.Where(x => outcomeIds.Contains(x.Id)).ToArray();
        var profiles = selected.Select(NativeMathematicsOutcomeProfileResolver.Resolve).ToArray();
        if (profiles.Any(x => x is null) || profiles.Length != outcomeIds.Length)
            return Failure(AssessmentErrorCode.OutcomeDoesNotMatchAssessment);

        var policy = request.Difficulty switch
        {
            AssessmentBuilderDifficulty.AtClassLevel => AssessmentDifficultyPolicy.Balanced,
            AssessmentBuilderDifficulty.Stretch => AssessmentDifficultyPolicy.Stretch,
            AssessmentBuilderDifficulty.Challenge => new AssessmentDifficultyPolicy(5, 30, 65),
            _ => AssessmentDifficultyPolicy.Balanced
        };

        var excluded = context.Items
            .Select(x => x.ExposureFingerprint)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToArray();
        var topicId = ResolveSingleTopic(context, outcomeIds);
        var blueprint = new AssessmentBlueprintEngine().Build(new AssessmentBlueprintRequest(
            access.SchoolId,
            context.CurriculumAdoption.Id,
            context.CurriculumAdoption.CurriculumLevelKey!,
            topicId,
            null,
            outcomeIds,
            null,
            AssessmentPurpose.TeacherAssessment,
            request.QuestionCount,
            policy,
            excluded));

        MathematicsGenerationBatch batch;
        try
        {
            batch = new MathematicsQuestionGenerationEngine().Generate(new MathematicsGenerationRequest(
                blueprint,
                profiles.Select(x => x!).ToArray(),
                request.Seed));
        }
        catch (InvalidOperationException)
        {
            return Failure(AssessmentErrorCode.OutcomeDoesNotMatchAssessment);
        }

        var nextOrder = context.Questions.Count == 0 ? 1 : context.Questions.Max(x => x.Order) + 1;
        foreach (var generated in batch.Items)
        {
            var item = generated.Item;
            item.CreatedByUserId = actorUserId;
            item.ValidationMetadataJson = SetBuilderStatus(item.ValidationMetadataJson, AssessmentBuilderQuestionStatus.Draft);
            var question = new AssessmentQuestion
            {
                Id = item.Id,
                SchoolId = access.SchoolId,
                AssessmentId = request.AssessmentId,
                Prompt = item.Prompt,
                MaxScore = decimal.Round(request.MaxScorePerQuestion, 2, MidpointRounding.AwayFromZero),
                Order = nextOrder++
            };
            var qMapping = new QuestionLearningOutcome
            {
                Id = Guid.NewGuid(),
                SchoolId = access.SchoolId,
                AssessmentQuestionId = question.Id,
                LearningOutcomeId = generated.OutcomeLink.LearningOutcomeId
            };
            repository.AddBundle(new AssessmentBuilderQuestionBundle(
                question,
                item,
                [qMapping],
                [generated.OutcomeLink]));
        }

        context.Assessment.UpdatedAtUtc = DateTime.UtcNow;
        var saved = await repository.SaveAsync(context.Assessment, request.AssessmentRowVersion, cancellationToken);
        return saved.Succeeded ? AssessmentCommandResult.Success(request.AssessmentId) : PersistenceFailure(saved);
    }

    public async Task<AssessmentCommandResult> ApproveQuestionAsync(
        Guid actorUserId,
        Guid assessmentId,
        Guid questionId,
        byte[] assessmentRowVersion,
        CancellationToken cancellationToken = default)
    {
        var access = await ResolveAccessAsync(actorUserId, assessmentId, cancellationToken);
        if (access.Error.HasValue) return Failure(access.Error.Value);
        if (access.Details!.Assessment.Status != AssessmentStatus.Draft) return Failure(AssessmentErrorCode.AssessmentNotDraft);
        var context = await repository.GetContextAsync(access.SchoolId, assessmentId, cancellationToken);
        if (context is null) return Failure(AssessmentErrorCode.AssessmentNotFound);
        var item = context.Items.SingleOrDefault(x => x.Id == questionId);
        if (item is null || context.Questions.All(x => x.Id != questionId)) return Failure(AssessmentErrorCode.QuestionNotFound);
        item.ValidationMetadataJson = SetBuilderStatus(item.ValidationMetadataJson, AssessmentBuilderQuestionStatus.Approved);
        context.Assessment.UpdatedAtUtc = DateTime.UtcNow;
        var saved = await repository.SaveAsync(context.Assessment, assessmentRowVersion, cancellationToken);
        return saved.Succeeded ? AssessmentCommandResult.Success(questionId) : PersistenceFailure(saved);
    }

    public async Task<AssessmentCommandResult> DeleteQuestionAsync(
        Guid actorUserId,
        Guid assessmentId,
        Guid questionId,
        byte[] assessmentRowVersion,
        CancellationToken cancellationToken = default)
    {
        var access = await ResolveAccessAsync(actorUserId, assessmentId, cancellationToken);
        if (access.Error.HasValue) return Failure(access.Error.Value);
        if (access.Details!.Assessment.Status != AssessmentStatus.Draft) return Failure(AssessmentErrorCode.AssessmentNotDraft);
        var context = await repository.GetContextAsync(access.SchoolId, assessmentId, cancellationToken);
        if (context is null) return Failure(AssessmentErrorCode.AssessmentNotFound);
        var question = context.Questions.SingleOrDefault(x => x.Id == questionId);
        if (question is null) return Failure(AssessmentErrorCode.QuestionNotFound);
        var item = context.Items.SingleOrDefault(x => x.Id == questionId);
        repository.RemoveQuestionBundle(
            question,
            item,
            context.QuestionOutcomeMappings.Where(x => x.AssessmentQuestionId == questionId).ToArray(),
            context.ItemOutcomeMappings.Where(x => x.AssessmentItemId == questionId).ToArray());
        context.Assessment.UpdatedAtUtc = DateTime.UtcNow;
        var saved = await repository.SaveAsync(context.Assessment, assessmentRowVersion, cancellationToken);
        return saved.Succeeded ? AssessmentCommandResult.Success(questionId) : PersistenceFailure(saved);
    }

    public async Task<AssessmentCommandResult> PublishAsync(
        Guid actorUserId,
        Guid assessmentId,
        byte[] assessmentRowVersion,
        CancellationToken cancellationToken = default)
    {
        var workspace = await GetWorkspaceAsync(actorUserId, assessmentId, cancellationToken);
        if (workspace.Value is null) return Failure(workspace.Error ?? AssessmentErrorCode.AccessDenied);
        if (!workspace.Value.ReadyToPublish) return Failure(AssessmentErrorCode.QuestionMissingOutcome);
        return await assessments.OpenAssessmentAsync(actorUserId, assessmentId, assessmentRowVersion, cancellationToken);
    }

    private async Task<(AssessmentDetails? Details, Guid SchoolId, AssessmentErrorCode? Error)> ResolveAccessAsync(
        Guid actorUserId,
        Guid assessmentId,
        CancellationToken cancellationToken)
    {
        var actor = await users.GetActorAsync(actorUserId, cancellationToken);
        if (actor is null || !actor.IsActive || actor.IsLocked || !actor.SchoolId.HasValue ||
            actor.Roles.Count != 1 || actor.Roles[0] != RoleNames.Teacher)
        {
            return (null, Guid.Empty, AssessmentErrorCode.AccessDenied);
        }
        var details = await assessments.GetDetailsAsync(actorUserId, assessmentId, cancellationToken);
        return details.Value is null
            ? (null, actor.SchoolId.Value, details.Error ?? AssessmentErrorCode.AccessDenied)
            : (details.Value, actor.SchoolId.Value, null);
    }

    private static AssessmentBuilderWorkspace BuildWorkspace(
        AssessmentDetails details,
        AssessmentBuilderPersistenceContext context)
    {
        var itemById = context.Items.ToDictionary(x => x.Id);
        var questions = context.Questions.OrderBy(x => x.Order).Select(question =>
        {
            itemById.TryGetValue(question.Id, out var item);
            var status = item is null ? AssessmentBuilderQuestionStatus.Legacy : ReadBuilderStatus(item.ValidationMetadataJson);
            return new AssessmentBuilderQuestion(
                question.Id,
                question.Order,
                question.Prompt,
                question.MaxScore,
                item?.Source,
                item?.Difficulty,
                status,
                item?.CorrectAnswer ?? string.Empty,
                item?.Solution ?? string.Empty,
                context.QuestionOutcomeMappings
                    .Where(x => x.AssessmentQuestionId == question.Id)
                    .Select(x => x.LearningOutcomeId)
                    .OrderBy(x => x)
                    .ToArray());
        }).ToArray();

        var current = questions.Sum(x => x.MaxScore);
        var mappedOutcomeIds = questions.SelectMany(x => x.OutcomeIds).Distinct().ToArray();
        var masteryRows = context.ClassOutcomeSummaries.Where(x => mappedOutcomeIds.Contains(x.LearningOutcomeId)).ToArray();
        decimal? mastery = masteryRows.Length == 0 ? null : decimal.Round(masteryRows.Average(x => x.AverageMasteryPercentage), 1);
        var allRich = questions.Count > 0 && questions.All(x => x.Status != AssessmentBuilderQuestionStatus.Legacy);
        var allApproved = allRich && questions.All(x => x.Status == AssessmentBuilderQuestionStatus.Approved);
        var allMapped = questions.Count > 0 && questions.All(x => x.OutcomeIds.Count > 0);
        var marksMatch = current == details.Assessment.MaxScore;
        var ready = details.Assessment.Status == AssessmentStatus.Draft && allApproved && allMapped && marksMatch;
        var message = ready
            ? "ReadyToPublish"
            : !allRich ? "BuilderLegacyQuestionsNeedReplacement"
            : !allApproved ? "BuilderQuestionsNeedApproval"
            : !allMapped ? "BuilderQuestionsNeedOutcomes"
            : !marksMatch ? "BuilderMarksMustMatch"
            : "BuilderNotDraft";

        var canGenerate = context.CurriculumAdoption is not null &&
            !string.IsNullOrWhiteSpace(context.CurriculumAdoption.CurriculumLevelKey) &&
            details.EligibleOutcomes.Any(outcome =>
                context.LearningOutcomes.Any(x => x.Id == outcome.Id && NativeMathematicsOutcomeProfileResolver.Resolve(x) is not null));

        return new AssessmentBuilderWorkspace(
            details,
            questions,
            current,
            Math.Max(0m, details.Assessment.MaxScore - current),
            mastery,
            canGenerate,
            ready,
            message);
    }

    private static bool ValidateOutcomes(AssessmentDetails details, IReadOnlyCollection<Guid> outcomeIds)
    {
        if (outcomeIds.Count == 0) return false;
        var eligible = details.EligibleOutcomes.Select(x => x.Id).ToHashSet();
        return outcomeIds.All(eligible.Contains);
    }

    private static Guid[] NormalizeOutcomes(IReadOnlyList<Guid>? ids) =>
        (ids ?? []).Where(x => x != Guid.Empty).Distinct().OrderBy(x => x).ToArray();

    private static Guid? ResolveSingleTopic(AssessmentBuilderPersistenceContext context, IReadOnlyCollection<Guid> outcomeIds)
    {
        var topics = context.LearningOutcomes
            .Where(x => outcomeIds.Contains(x.Id))
            .Select(x => x.TopicId)
            .Distinct()
            .ToArray();
        return topics.Length == 1 ? topics[0] : null;
    }

    private static AssessmentBuilderQuestionStatus ReadBuilderStatus(string? json)
    {
        if (string.IsNullOrWhiteSpace(json)) return AssessmentBuilderQuestionStatus.Draft;
        try
        {
            var node = JsonNode.Parse(json);
            var value = node?["builderStatus"]?.GetValue<string>();
            return Enum.TryParse<AssessmentBuilderQuestionStatus>(value, true, out var status)
                ? status
                : AssessmentBuilderQuestionStatus.Draft;
        }
        catch (JsonException)
        {
            return AssessmentBuilderQuestionStatus.Draft;
        }
    }

    private static string SetBuilderStatus(string? json, AssessmentBuilderQuestionStatus status)
    {
        JsonObject root;
        try
        {
            root = string.IsNullOrWhiteSpace(json)
                ? new JsonObject()
                : JsonNode.Parse(json) as JsonObject ?? new JsonObject();
        }
        catch (JsonException)
        {
            root = new JsonObject();
        }
        root["builderStatus"] = status.ToString();
        return root.ToJsonString(new JsonSerializerOptions { WriteIndented = false });
    }

    private static string Fingerprint(string value) =>
        Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(value))).ToLowerInvariant();

    private static AssessmentCommandResult Failure(AssessmentErrorCode error) =>
        AssessmentCommandResult.Failure(string.Empty, error);

    private static AssessmentCommandResult PersistenceFailure(AssessmentPersistenceResult result) =>
        Failure(result.Error == AssessmentPersistenceError.Conflict
            ? AssessmentErrorCode.ConcurrencyConflict
            : AssessmentErrorCode.PersistenceError);
}

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
        Guid actorUserId, Guid assessmentId, CancellationToken cancellationToken = default)
    {
        var access = await ResolveAccessAsync(actorUserId, assessmentId, cancellationToken);
        if (access.Error.HasValue)
            return AssessmentQueryResult<AssessmentBuilderWorkspace>.Failure(access.Error.Value);
        var context = await repository.GetContextAsync(access.SchoolId, assessmentId, cancellationToken);
        return context is null
            ? AssessmentQueryResult<AssessmentBuilderWorkspace>.Failure(AssessmentErrorCode.AssessmentNotFound)
            : AssessmentQueryResult<AssessmentBuilderWorkspace>.Success(BuildWorkspace(access.Details!, context));
    }

    public async Task<AssessmentCommandResult> CreateManualQuestionAsync(
        Guid actorUserId, CreateManualBuilderQuestionRequest request, CancellationToken cancellationToken = default)
    {
        var resolved = await ResolveEditableAsync(actorUserId, request.AssessmentId, cancellationToken);
        if (resolved.Error.HasValue) return Failure(resolved.Error.Value);
        var context = resolved.Context!;
        if (context.CurriculumAdoption is null || string.IsNullOrWhiteSpace(context.CurriculumAdoption.CurriculumLevelKey))
            return Failure(AssessmentErrorCode.OutcomeDoesNotMatchAssessment);

        var prompt = Clean(request.Prompt);
        var answer = Clean(request.CorrectAnswer);
        var solution = Clean(request.Solution);
        if (!ValidContent(prompt, answer, solution)) return Failure(AssessmentErrorCode.InvalidText);
        if (request.Order <= 0) return Failure(AssessmentErrorCode.InvalidOrder);
        if (!ValidScore(request.MaxScore)) return Failure(AssessmentErrorCode.InvalidQuestionScore);
        if (context.Questions.Any(x => x.Order == request.Order)) return Failure(AssessmentErrorCode.DuplicateQuestionOrder);
        if (context.Questions.Sum(x => x.MaxScore) + request.MaxScore > context.Assessment.MaxScore)
            return Failure(AssessmentErrorCode.AssessmentScoreMismatch);

        var outcomeIds = NormalizeOutcomes(request.OutcomeIds);
        if (!ValidateOutcomes(resolved.Details!, outcomeIds)) return Failure(AssessmentErrorCode.OutcomeDoesNotMatchAssessment);

        var id = Guid.NewGuid();
        var item = new AssessmentItem
        {
            Id = id,
            SchoolId = resolved.SchoolId,
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
            ValidationMetadataJson = SetStatus(null, AssessmentBuilderQuestionStatus.Draft),
            CreatedAtUtc = DateTime.UtcNow
        };
        var question = new AssessmentQuestion
        {
            Id = id,
            SchoolId = resolved.SchoolId,
            AssessmentId = request.AssessmentId,
            Prompt = prompt,
            MaxScore = Round(request.MaxScore),
            Order = request.Order
        };
        repository.AddBundle(new AssessmentBuilderQuestionBundle(
            question,
            item,
            outcomeIds.Select(x => new QuestionLearningOutcome { Id = Guid.NewGuid(), SchoolId = resolved.SchoolId, AssessmentQuestionId = id, LearningOutcomeId = x }).ToArray(),
            outcomeIds.Select(x => new AssessmentItemOutcome { Id = Guid.NewGuid(), SchoolId = resolved.SchoolId, AssessmentItemId = id, LearningOutcomeId = x }).ToArray()));

        return await SaveAsync(context, request.AssessmentRowVersion, id, cancellationToken);
    }

    public async Task<AssessmentCommandResult> EditQuestionAsync(
        Guid actorUserId, EditBuilderQuestionRequest request, CancellationToken cancellationToken = default)
    {
        var resolved = await ResolveEditableAsync(actorUserId, request.AssessmentId, cancellationToken);
        if (resolved.Error.HasValue) return Failure(resolved.Error.Value);
        var context = resolved.Context!;
        var question = context.Questions.SingleOrDefault(x => x.Id == request.QuestionId);
        var item = context.Items.SingleOrDefault(x => x.Id == request.QuestionId);
        if (question is null || item is null) return Failure(AssessmentErrorCode.QuestionNotFound);

        var prompt = Clean(request.Prompt);
        var answer = Clean(request.CorrectAnswer);
        var solution = Clean(request.Solution);
        if (!ValidContent(prompt, answer, solution)) return Failure(AssessmentErrorCode.InvalidText);
        if (!ValidScore(request.MaxScore)) return Failure(AssessmentErrorCode.InvalidQuestionScore);
        if (context.Questions.Where(x => x.Id != question.Id).Sum(x => x.MaxScore) + request.MaxScore > context.Assessment.MaxScore)
            return Failure(AssessmentErrorCode.AssessmentScoreMismatch);

        question.Prompt = prompt;
        question.MaxScore = Round(request.MaxScore);
        item.Prompt = prompt;
        item.CorrectAnswer = answer;
        item.Solution = solution;
        item.Difficulty = request.Difficulty;
        item.ValidationMetadataJson = SetStatus(item.ValidationMetadataJson, AssessmentBuilderQuestionStatus.Edited);
        return await SaveAsync(context, request.AssessmentRowVersion, question.Id, cancellationToken);
    }

    public async Task<AssessmentCommandResult> GenerateQuestionsAsync(
        Guid actorUserId, GenerateBuilderQuestionsRequest request, CancellationToken cancellationToken = default)
    {
        var resolved = await ResolveEditableAsync(actorUserId, request.AssessmentId, cancellationToken);
        if (resolved.Error.HasValue) return Failure(resolved.Error.Value);
        var context = resolved.Context!;
        if (context.CurriculumAdoption is null || string.IsNullOrWhiteSpace(context.CurriculumAdoption.CurriculumLevelKey))
            return Failure(AssessmentErrorCode.OutcomeDoesNotMatchAssessment);
        if (request.QuestionCount is < 1 or > 100 || !ValidScore(request.MaxScorePerQuestion))
            return Failure(AssessmentErrorCode.InvalidQuestionScore);
        if (context.Questions.Sum(x => x.MaxScore) + request.QuestionCount * request.MaxScorePerQuestion > context.Assessment.MaxScore)
            return Failure(AssessmentErrorCode.AssessmentScoreMismatch);

        var outcomeIds = NormalizeOutcomes(request.OutcomeIds);
        if (!ValidateOutcomes(resolved.Details!, outcomeIds)) return Failure(AssessmentErrorCode.OutcomeDoesNotMatchAssessment);
        var batch = TryGenerate(context, resolved.SchoolId, outcomeIds, request.QuestionCount, request.Difficulty, request.Seed);
        if (batch is null) return Failure(AssessmentErrorCode.OutcomeDoesNotMatchAssessment);

        var order = context.Questions.Count == 0 ? 1 : context.Questions.Max(x => x.Order) + 1;
        foreach (var generated in batch.Items)
        {
            generated.Item.CreatedByUserId = actorUserId;
            generated.Item.ValidationMetadataJson = SetStatus(generated.Item.ValidationMetadataJson, AssessmentBuilderQuestionStatus.Draft);
            var question = new AssessmentQuestion
            {
                Id = generated.Item.Id,
                SchoolId = resolved.SchoolId,
                AssessmentId = request.AssessmentId,
                Prompt = generated.Item.Prompt,
                MaxScore = Round(request.MaxScorePerQuestion),
                Order = order++
            };
            repository.AddBundle(new AssessmentBuilderQuestionBundle(
                question,
                generated.Item,
                [new QuestionLearningOutcome { Id = Guid.NewGuid(), SchoolId = resolved.SchoolId, AssessmentQuestionId = question.Id, LearningOutcomeId = generated.OutcomeLink.LearningOutcomeId }],
                [generated.OutcomeLink]));
        }
        return await SaveAsync(context, request.AssessmentRowVersion, request.AssessmentId, cancellationToken);
    }

    public async Task<AssessmentCommandResult> RegenerateQuestionAsync(
        Guid actorUserId, Guid assessmentId, Guid questionId, int seed, byte[] assessmentRowVersion, CancellationToken cancellationToken = default)
    {
        var resolved = await ResolveEditableAsync(actorUserId, assessmentId, cancellationToken);
        if (resolved.Error.HasValue) return Failure(resolved.Error.Value);
        var context = resolved.Context!;
        var question = context.Questions.SingleOrDefault(x => x.Id == questionId);
        var item = context.Items.SingleOrDefault(x => x.Id == questionId);
        if (question is null || item is null || item.Source != AssessmentItemSource.SystemGenerated)
            return Failure(AssessmentErrorCode.QuestionNotFound);
        if (context.CurriculumAdoption is null || string.IsNullOrWhiteSpace(context.CurriculumAdoption.CurriculumLevelKey))
            return Failure(AssessmentErrorCode.OutcomeDoesNotMatchAssessment);

        var outcomeIds = context.QuestionOutcomeMappings
            .Where(x => x.AssessmentQuestionId == questionId)
            .Select(x => x.LearningOutcomeId).Distinct().ToArray();
        if (outcomeIds.Length != 1) return Failure(AssessmentErrorCode.OutcomeDoesNotMatchAssessment);
        var difficulty = item.Difficulty switch
        {
            AssessmentItemDifficulty.Easy => AssessmentBuilderDifficulty.AtClassLevel,
            AssessmentItemDifficulty.Medium => AssessmentBuilderDifficulty.Stretch,
            _ => AssessmentBuilderDifficulty.Challenge
        };
        var batch = TryGenerate(context, resolved.SchoolId, outcomeIds, 1, difficulty, seed);
        var replacement = batch?.Items.SingleOrDefault();
        if (replacement is null) return Failure(AssessmentErrorCode.OutcomeDoesNotMatchAssessment);

        question.Prompt = replacement.Item.Prompt;
        item.Prompt = replacement.Item.Prompt;
        item.CorrectAnswer = replacement.Item.CorrectAnswer;
        item.Solution = replacement.Item.Solution;
        item.ItemType = replacement.Item.ItemType;
        item.Difficulty = replacement.Item.Difficulty;
        item.GenerationMethod = replacement.Item.GenerationMethod;
        item.GenerationFamily = replacement.Item.GenerationFamily;
        item.GenerationParametersJson = replacement.Item.GenerationParametersJson;
        item.ExposureFingerprint = replacement.Item.ExposureFingerprint;
        item.ValidationMetadataJson = SetStatus(replacement.Item.ValidationMetadataJson, AssessmentBuilderQuestionStatus.Draft);
        return await SaveAsync(context, assessmentRowVersion, question.Id, cancellationToken);
    }

    public async Task<AssessmentCommandResult> ApproveQuestionAsync(
        Guid actorUserId, Guid assessmentId, Guid questionId, byte[] assessmentRowVersion, CancellationToken cancellationToken = default)
    {
        var resolved = await ResolveEditableAsync(actorUserId, assessmentId, cancellationToken);
        if (resolved.Error.HasValue) return Failure(resolved.Error.Value);
        var item = resolved.Context!.Items.SingleOrDefault(x => x.Id == questionId);
        if (item is null || resolved.Context.Questions.All(x => x.Id != questionId)) return Failure(AssessmentErrorCode.QuestionNotFound);
        item.ValidationMetadataJson = SetStatus(item.ValidationMetadataJson, AssessmentBuilderQuestionStatus.Approved);
        return await SaveAsync(resolved.Context, assessmentRowVersion, questionId, cancellationToken);
    }

    public async Task<AssessmentCommandResult> DeleteQuestionAsync(
        Guid actorUserId, Guid assessmentId, Guid questionId, byte[] assessmentRowVersion, CancellationToken cancellationToken = default)
    {
        var resolved = await ResolveEditableAsync(actorUserId, assessmentId, cancellationToken);
        if (resolved.Error.HasValue) return Failure(resolved.Error.Value);
        var context = resolved.Context!;
        var question = context.Questions.SingleOrDefault(x => x.Id == questionId);
        if (question is null) return Failure(AssessmentErrorCode.QuestionNotFound);
        var item = context.Items.SingleOrDefault(x => x.Id == questionId);
        repository.RemoveQuestionBundle(
            question,
            item,
            context.QuestionOutcomeMappings.Where(x => x.AssessmentQuestionId == questionId).ToArray(),
            context.ItemOutcomeMappings.Where(x => x.AssessmentItemId == questionId).ToArray());
        return await SaveAsync(context, assessmentRowVersion, questionId, cancellationToken);
    }

    public async Task<AssessmentCommandResult> PublishAsync(
        Guid actorUserId, Guid assessmentId, byte[] assessmentRowVersion, CancellationToken cancellationToken = default)
    {
        var workspace = await GetWorkspaceAsync(actorUserId, assessmentId, cancellationToken);
        if (workspace.Value is null) return Failure(workspace.Error ?? AssessmentErrorCode.AccessDenied);
        if (!workspace.Value.ReadyToPublish) return Failure(AssessmentErrorCode.QuestionMissingOutcome);
        return await assessments.OpenAssessmentAsync(actorUserId, assessmentId, assessmentRowVersion, cancellationToken);
    }

    private async Task<(AssessmentDetails? Details, AssessmentBuilderPersistenceContext? Context, Guid SchoolId, AssessmentErrorCode? Error)> ResolveEditableAsync(
        Guid actorUserId, Guid assessmentId, CancellationToken cancellationToken)
    {
        var access = await ResolveAccessAsync(actorUserId, assessmentId, cancellationToken);
        if (access.Error.HasValue) return (access.Details, null, access.SchoolId, access.Error);
        if (access.Details!.Assessment.Status != AssessmentStatus.Draft)
            return (access.Details, null, access.SchoolId, AssessmentErrorCode.AssessmentNotDraft);
        var context = await repository.GetContextAsync(access.SchoolId, assessmentId, cancellationToken);
        return context is null
            ? (access.Details, null, access.SchoolId, AssessmentErrorCode.AssessmentNotFound)
            : (access.Details, context, access.SchoolId, null);
    }

    private async Task<(AssessmentDetails? Details, Guid SchoolId, AssessmentErrorCode? Error)> ResolveAccessAsync(
        Guid actorUserId, Guid assessmentId, CancellationToken cancellationToken)
    {
        var actor = await users.GetActorAsync(actorUserId, cancellationToken);
        if (actor is null || !actor.IsActive || actor.IsLocked || !actor.SchoolId.HasValue || actor.Roles.Count != 1 || actor.Roles[0] != RoleNames.Teacher)
            return (null, Guid.Empty, AssessmentErrorCode.AccessDenied);
        var details = await assessments.GetDetailsAsync(actorUserId, assessmentId, cancellationToken);
        return details.Value is null
            ? (null, actor.SchoolId.Value, details.Error ?? AssessmentErrorCode.AccessDenied)
            : (details.Value, actor.SchoolId.Value, null);
    }

    private MathematicsGenerationBatch? TryGenerate(
        AssessmentBuilderPersistenceContext context, Guid schoolId, IReadOnlyList<Guid> outcomeIds,
        int count, AssessmentBuilderDifficulty difficulty, int seed)
    {
        if (context.CurriculumAdoption is null || string.IsNullOrWhiteSpace(context.CurriculumAdoption.CurriculumLevelKey)) return null;
        var selected = context.LearningOutcomes.Where(x => outcomeIds.Contains(x.Id)).ToArray();
        var profiles = selected.Select(NativeMathematicsOutcomeProfileResolver.Resolve).ToArray();
        if (profiles.Length != outcomeIds.Count || profiles.Any(x => x is null)) return null;
        var policy = difficulty switch
        {
            AssessmentBuilderDifficulty.AtClassLevel => AssessmentDifficultyPolicy.Balanced,
            AssessmentBuilderDifficulty.Stretch => AssessmentDifficultyPolicy.Stretch,
            AssessmentBuilderDifficulty.Challenge => new AssessmentDifficultyPolicy(5, 30, 65),
            _ => AssessmentDifficultyPolicy.Balanced
        };
        try
        {
            var blueprint = new AssessmentBlueprintEngine().Build(new AssessmentBlueprintRequest(
                schoolId,
                context.CurriculumAdoption.Id,
                context.CurriculumAdoption.CurriculumLevelKey!,
                ResolveSingleTopic(context, outcomeIds),
                null,
                outcomeIds,
                null,
                AssessmentPurpose.TeacherAssessment,
                count,
                policy,
                context.Items.Select(x => x.ExposureFingerprint).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray()));
            return new MathematicsQuestionGenerationEngine().Generate(new MathematicsGenerationRequest(blueprint, profiles.Select(x => x!).ToArray(), seed));
        }
        catch (InvalidOperationException)
        {
            return null;
        }
    }

    private static AssessmentBuilderWorkspace BuildWorkspace(AssessmentDetails details, AssessmentBuilderPersistenceContext context)
    {
        var itemById = context.Items.ToDictionary(x => x.Id);
        var questions = context.Questions.OrderBy(x => x.Order).Select(question =>
        {
            itemById.TryGetValue(question.Id, out var item);
            return new AssessmentBuilderQuestion(
                question.Id,
                question.Order,
                question.Prompt,
                question.MaxScore,
                item?.Source,
                item?.Difficulty,
                item is null ? AssessmentBuilderQuestionStatus.Legacy : ReadStatus(item.ValidationMetadataJson),
                item?.CorrectAnswer ?? string.Empty,
                item?.Solution ?? string.Empty,
                context.QuestionOutcomeMappings.Where(x => x.AssessmentQuestionId == question.Id).Select(x => x.LearningOutcomeId).Distinct().OrderBy(x => x).ToArray());
        }).ToArray();
        var current = questions.Sum(x => x.MaxScore);
        var mappedIds = questions.SelectMany(x => x.OutcomeIds).Distinct().ToArray();
        var masteryRows = context.ClassOutcomeSummaries.Where(x => mappedIds.Contains(x.LearningOutcomeId)).ToArray();
        decimal? mastery = masteryRows.Length == 0 ? null : Round(masteryRows.Average(x => x.AverageMasteryPercentage));
        var allRich = questions.Length > 0 && questions.All(x => x.Status != AssessmentBuilderQuestionStatus.Legacy);
        var allApproved = allRich && questions.All(x => x.Status == AssessmentBuilderQuestionStatus.Approved);
        var allMapped = questions.Length > 0 && questions.All(x => x.OutcomeIds.Count > 0);
        var marksMatch = current == details.Assessment.MaxScore;
        var ready = details.Assessment.Status == AssessmentStatus.Draft && allApproved && allMapped && marksMatch;
        var message = ready ? "ReadyToPublish"
            : !allRich ? "BuilderLegacyQuestionsNeedReplacement"
            : !allApproved ? "BuilderQuestionsNeedApproval"
            : !allMapped ? "BuilderQuestionsNeedOutcomes"
            : !marksMatch ? "BuilderMarksMustMatch"
            : "BuilderNotDraft";
        var canGenerate = context.CurriculumAdoption is not null &&
            !string.IsNullOrWhiteSpace(context.CurriculumAdoption.CurriculumLevelKey) &&
            details.EligibleOutcomes.Any(o => context.LearningOutcomes.Any(x => x.Id == o.Id && NativeMathematicsOutcomeProfileResolver.Resolve(x) is not null));
        return new AssessmentBuilderWorkspace(details, questions, current, Math.Max(0m, details.Assessment.MaxScore - current), mastery, canGenerate, ready, message);
    }

    private async Task<AssessmentCommandResult> SaveAsync(
        AssessmentBuilderPersistenceContext context, byte[] rowVersion, Guid entityId, CancellationToken cancellationToken)
    {
        context.Assessment.UpdatedAtUtc = DateTime.UtcNow;
        var saved = await repository.SaveAsync(context.Assessment, rowVersion, cancellationToken);
        return saved.Succeeded ? AssessmentCommandResult.Success(entityId) : PersistenceFailure(saved);
    }

    private static bool ValidContent(string prompt, string answer, string solution) =>
        prompt.Length is >= 1 and <= 1000 && answer.Length is >= 1 and <= 1000 && solution.Length <= 4000;
    private static bool ValidScore(decimal value) =>
        value > 0m && value <= 10000m && decimal.Truncate(value) == value;
    private static string Clean(string? value) => value?.Trim() ?? string.Empty;
    private static decimal Round(decimal value) => decimal.Round(value, 2, MidpointRounding.AwayFromZero);
    private static bool ValidateOutcomes(AssessmentDetails details, IReadOnlyCollection<Guid> ids)
    {
        if (ids.Count == 0) return false;
        var eligible = details.EligibleOutcomes.Select(x => x.Id).ToHashSet();
        return ids.All(eligible.Contains);
    }
    private static Guid[] NormalizeOutcomes(IReadOnlyList<Guid>? ids) =>
        (ids ?? []).Where(x => x != Guid.Empty).Distinct().OrderBy(x => x).ToArray();
    private static Guid? ResolveSingleTopic(AssessmentBuilderPersistenceContext context, IReadOnlyCollection<Guid> ids)
    {
        var topics = context.LearningOutcomes.Where(x => ids.Contains(x.Id)).Select(x => x.TopicId).Distinct().ToArray();
        return topics.Length == 1 ? topics[0] : null;
    }
    private static AssessmentBuilderQuestionStatus ReadStatus(string? json)
    {
        if (string.IsNullOrWhiteSpace(json)) return AssessmentBuilderQuestionStatus.Draft;
        try
        {
            var value = JsonNode.Parse(json)?["builderStatus"]?.GetValue<string>();
            return Enum.TryParse<AssessmentBuilderQuestionStatus>(value, true, out var status) ? status : AssessmentBuilderQuestionStatus.Draft;
        }
        catch (JsonException) { return AssessmentBuilderQuestionStatus.Draft; }
    }
    private static string SetStatus(string? json, AssessmentBuilderQuestionStatus status)
    {
        JsonObject root;
        try { root = string.IsNullOrWhiteSpace(json) ? new JsonObject() : JsonNode.Parse(json) as JsonObject ?? new JsonObject(); }
        catch (JsonException) { root = new JsonObject(); }
        root["builderStatus"] = status.ToString();
        return root.ToJsonString(new JsonSerializerOptions { WriteIndented = false });
    }
    private static string Fingerprint(string value) => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(value))).ToLowerInvariant();
    private static AssessmentCommandResult Failure(AssessmentErrorCode error) => AssessmentCommandResult.Failure(string.Empty, error);
    private static AssessmentCommandResult PersistenceFailure(AssessmentPersistenceResult result) =>
        Failure(result.Error == AssessmentPersistenceError.Conflict ? AssessmentErrorCode.ConcurrencyConflict : AssessmentErrorCode.PersistenceError);
}
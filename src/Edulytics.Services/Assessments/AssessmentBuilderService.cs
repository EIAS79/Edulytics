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
using Edulytics.Core.Mathematics.Assessment;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Core.MathematicsGeneration;
using Edulytics.Core.Users;
using Edulytics.Services.AssessmentIntelligence;
using Edulytics.Services.Mathematics;
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
        if (request.Order <= 0) return Failure(AssessmentErrorCode.InvalidOrder);
        if (!ValidScore(request.MaxScore)) return Failure(AssessmentErrorCode.InvalidQuestionScore);
        if (context.Questions.Any(x => x.Id != question.Id && x.Order == request.Order))
            return Failure(AssessmentErrorCode.DuplicateQuestionOrder);
        if (context.Questions.Where(x => x.Id != question.Id).Sum(x => x.MaxScore) + request.MaxScore > context.Assessment.MaxScore)
            return Failure(AssessmentErrorCode.AssessmentScoreMismatch);

        var outcomeIds = NormalizeOutcomes(request.OutcomeIds);
        if (!ValidateOutcomes(resolved.Details!, outcomeIds))
            return Failure(AssessmentErrorCode.OutcomeDoesNotMatchAssessment);

        question.Prompt = prompt;
        question.MaxScore = Round(request.MaxScore);
        question.Order = request.Order;
        item.Prompt = prompt;
        item.CorrectAnswer = answer;
        item.Solution = solution;
        item.Difficulty = request.Difficulty;
        item.CurriculumTopicId = ResolveSingleTopic(context, outcomeIds);
        item.ValidationMetadataJson = SetStatus(item.ValidationMetadataJson, AssessmentBuilderQuestionStatus.Edited);

        var currentQuestionMappings = context.QuestionOutcomeMappings
            .Where(x => x.AssessmentQuestionId == question.Id)
            .ToArray();
        var currentItemMappings = context.ItemOutcomeMappings
            .Where(x => x.AssessmentItemId == item.Id)
            .ToArray();
        var replacementQuestionMappings = outcomeIds
            .Select(outcomeId => new QuestionLearningOutcome
            {
                Id = Guid.NewGuid(),
                SchoolId = resolved.SchoolId,
                AssessmentQuestionId = question.Id,
                LearningOutcomeId = outcomeId
            })
            .ToArray();
        var replacementItemMappings = outcomeIds
            .Select(outcomeId => new AssessmentItemOutcome
            {
                Id = Guid.NewGuid(),
                SchoolId = resolved.SchoolId,
                AssessmentItemId = item.Id,
                LearningOutcomeId = outcomeId
            })
            .ToArray();
        repository.ReplaceOutcomeMappings(
            currentQuestionMappings,
            currentItemMappings,
            replacementQuestionMappings,
            replacementItemMappings);

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
        if (request.QuestionCount is < 1 or > 50 ||
            request.MaxScorePerQuestion < 0m ||
            (request.MaxScorePerQuestion > 0m && !ValidScore(request.MaxScorePerQuestion)))
            return Failure(AssessmentErrorCode.InvalidQuestionScore);

        var effectiveDifficulty = AssessmentBuilderGenerationPlanner.ResolveDifficulty(
            request.Difficulty,
            context.Assessment.DifficultyBand);
        if (!effectiveDifficulty.HasValue)
            return Failure(AssessmentErrorCode.Required);

        var currentMarks = context.Questions.Sum(x => x.MaxScore);
        var remainingMarks = context.Assessment.MaxScore - currentMarks;
        if (remainingMarks <= 0m || decimal.Truncate(remainingMarks) != remainingMarks || remainingMarks < request.QuestionCount)
            return Failure(AssessmentErrorCode.AssessmentScoreMismatch);
        if (request.MaxScorePerQuestion > 0m && request.QuestionCount * request.MaxScorePerQuestion > remainingMarks)
            return Failure(AssessmentErrorCode.AssessmentScoreMismatch);

        IReadOnlyList<ScopedGeneratedItem>? generatedItems;
        if (request.ScopeType == AssessmentGenerationScopeType.Outcomes)
        {
            var outcomeIds = NormalizeOutcomes(request.OutcomeIds);
            if (!ValidateOutcomes(resolved.Details!, outcomeIds))
                return Failure(AssessmentErrorCode.OutcomeDoesNotMatchAssessment);

            var batch = TryGenerate(
                context,
                resolved.SchoolId,
                outcomeIds,
                request.QuestionCount,
                effectiveDifficulty.Value,
                request.Seed,
                actorUserId);
            if (batch is null)
                return Failure(AssessmentErrorCode.OutcomeDoesNotMatchAssessment);

            generatedItems = batch.Items
                .Select(item => new ScopedGeneratedItem(
                    item.Item,
                    [item.OutcomeLink.LearningOutcomeId]))
                .ToArray();
        }
        else
        {
            var scopedLessons = ResolveScopedLessons(context, request);
            if (scopedLessons is null || scopedLessons.Count == 0)
                return Failure(AssessmentErrorCode.OutcomeDoesNotMatchAssessment);

            var budgetedLessons = SelectLessonsForQuestionBudget(
                request.ScopeType,
                scopedLessons,
                request.QuestionCount,
                request.Seed);
            if (budgetedLessons.Count == 0)
                return Failure(AssessmentErrorCode.OutcomeDoesNotMatchAssessment);

            generatedItems = TryGenerateLessons(
                context,
                resolved.SchoolId,
                budgetedLessons,
                request.QuestionCount,
                effectiveDifficulty.Value,
                request.Seed,
                actorUserId);
        }

        if (generatedItems is null || generatedItems.Count != request.QuestionCount)
            return Failure(AssessmentErrorCode.PersistenceError);

        var marks = AssessmentBuilderGenerationPlanner.DistributeMarks(
            remainingMarks,
            generatedItems.Select(x => x.Item.Difficulty).ToArray(),
            request.MaxScorePerQuestion);
        if (marks is null || marks.Count != generatedItems.Count)
            return Failure(AssessmentErrorCode.AssessmentScoreMismatch);

        var order = context.Questions.Count == 0 ? 1 : context.Questions.Max(x => x.Order) + 1;
        for (var index = 0; index < generatedItems.Count; index++)
        {
            var generated = generatedItems[index];
            generated.Item.CreatedByUserId = actorUserId;
            generated.Item.ValidationMetadataJson = SetStatus(
                generated.Item.ValidationMetadataJson,
                AssessmentBuilderQuestionStatus.Draft);

            var question = new AssessmentQuestion
            {
                Id = generated.Item.Id,
                SchoolId = resolved.SchoolId,
                AssessmentId = request.AssessmentId,
                Prompt = generated.Item.Prompt,
                MaxScore = Round(marks[index]),
                Order = order++
            };

            var questionMappings = generated.OutcomeIds
                .Select(outcomeId => new QuestionLearningOutcome
                {
                    Id = Guid.NewGuid(),
                    SchoolId = resolved.SchoolId,
                    AssessmentQuestionId = question.Id,
                    LearningOutcomeId = outcomeId
                })
                .ToArray();
            var itemMappings = generated.OutcomeIds
                .Select(outcomeId => new AssessmentItemOutcome
                {
                    Id = Guid.NewGuid(),
                    SchoolId = resolved.SchoolId,
                    AssessmentItemId = generated.Item.Id,
                    LearningOutcomeId = outcomeId
                })
                .ToArray();

            repository.AddBundle(new AssessmentBuilderQuestionBundle(
                question,
                generated.Item,
                questionMappings,
                itemMappings));
        }

        return await SaveAsync(
            context,
            request.AssessmentRowVersion,
            request.AssessmentId,
            cancellationToken);
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

        var difficulty = item.Difficulty switch
        {
            AssessmentItemDifficulty.Easy => AssessmentBuilderDifficulty.AtClassLevel,
            AssessmentItemDifficulty.Medium => AssessmentBuilderDifficulty.Stretch,
            _ => AssessmentBuilderDifficulty.Challenge
        };

        ScopedGeneratedItem? replacement = null;
        if (item.CurriculumPedagogicalLessonId.HasValue)
        {
            var lesson = (context.PedagogicalLessons ?? [])
                .SingleOrDefault(x => x.Id == item.CurriculumPedagogicalLessonId.Value);
            if (lesson is not null)
            {
                replacement = TryGenerateLessons(
                    context,
                    resolved.SchoolId,
                    [lesson],
                    1,
                    difficulty,
                    seed,
                    actorUserId)?.SingleOrDefault();
            }
        }
        else
        {
            var outcomeIds = context.QuestionOutcomeMappings
                .Where(x => x.AssessmentQuestionId == questionId)
                .Select(x => x.LearningOutcomeId)
                .Distinct()
                .ToArray();
            if (outcomeIds.Length == 1)
            {
                var batch = TryGenerate(
                    context,
                    resolved.SchoolId,
                    outcomeIds,
                    1,
                    difficulty,
                    seed,
                    actorUserId);
                var legacyReplacement = batch?.Items.SingleOrDefault();
                if (legacyReplacement is not null)
                {
                    replacement = new ScopedGeneratedItem(
                        legacyReplacement.Item,
                        [legacyReplacement.OutcomeLink.LearningOutcomeId]);
                }
            }
        }

        if (replacement is null)
            return Failure(AssessmentErrorCode.OutcomeDoesNotMatchAssessment);

        question.Prompt = replacement.Item.Prompt;
        item.Prompt = replacement.Item.Prompt;
        item.CorrectAnswer = replacement.Item.CorrectAnswer;
        item.Solution = replacement.Item.Solution;
        item.ItemType = replacement.Item.ItemType;
        item.Difficulty = replacement.Item.Difficulty;
        item.CurriculumPedagogicalLessonId = replacement.Item.CurriculumPedagogicalLessonId;
        item.CurriculumTopicId = replacement.Item.CurriculumTopicId;
        item.GenerationMethod = replacement.Item.GenerationMethod;
        item.GenerationFamily = replacement.Item.GenerationFamily;
        item.GenerationParametersJson = replacement.Item.GenerationParametersJson;
        item.ExposureFingerprint = replacement.Item.ExposureFingerprint;
        item.ValidationMetadataJson = SetStatus(
            replacement.Item.ValidationMetadataJson,
            AssessmentBuilderQuestionStatus.Draft);

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

    private sealed record ScopedGeneratedItem(
        AssessmentItem Item,
        IReadOnlyList<Guid> OutcomeIds);

    private static IReadOnlyList<CurriculumPedagogicalLesson>? ResolveScopedLessons(
        AssessmentBuilderPersistenceContext context,
        GenerateBuilderQuestionsRequest request)
    {
        var lessons = (context.PedagogicalLessons ?? [])
            .OrderBy(x => x.SortOrder)
            .ThenBy(x => x.UnitKey)
            .ThenBy(x => x.Code)
            .ToArray();

        if (lessons.Length == 0)
            return null;

        IReadOnlyList<CurriculumPedagogicalLesson> selected = request.ScopeType switch
        {
            AssessmentGenerationScopeType.Lessons => lessons
                .Where(x => request.LessonIds.Contains(x.Id))
                .ToArray(),

            AssessmentGenerationScopeType.Units => lessons
                .Where(x => request.UnitKeys.Contains(x.UnitKey, StringComparer.Ordinal))
                .ToArray(),

            AssessmentGenerationScopeType.Curriculum => lessons,

            _ => []
        };

        if (selected.Count == 0)
            return null;

        if (request.ScopeType == AssessmentGenerationScopeType.Lessons &&
            selected.Count != request.LessonIds.Where(x => x != Guid.Empty).Distinct().Count())
        {
            return null;
        }

        if (request.ScopeType == AssessmentGenerationScopeType.Units)
        {
            var requestedUnits = request.UnitKeys
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Trim())
                .Distinct(StringComparer.Ordinal)
                .ToArray();
            var resolvedUnits = selected
                .Select(x => x.UnitKey)
                .Distinct(StringComparer.Ordinal)
                .ToHashSet(StringComparer.Ordinal);
            if (requestedUnits.Any(x => !resolvedUnits.Contains(x)))
                return null;
        }

        if (request.ScopeType == AssessmentGenerationScopeType.Lessons &&
            selected.Any(x => !LessonPracticeCapabilityResolver.TryResolve(x.Code, out _)))
        {
            return null;
        }

        return selected
            .Where(x => LessonPracticeCapabilityResolver.TryResolve(x.Code, out _))
            .ToArray();
    }

    private static IReadOnlyList<CurriculumPedagogicalLesson> SelectLessonsForQuestionBudget(
        AssessmentGenerationScopeType scopeType,
        IReadOnlyList<CurriculumPedagogicalLesson> lessons,
        int questionCount,
        int seed)
    {
        if (lessons.Count == 0 || questionCount <= 0)
            return [];

        if (scopeType == AssessmentGenerationScopeType.Lessons)
            return questionCount < lessons.Count ? [] : lessons;

        if (lessons.Count <= questionCount)
            return lessons;

        var groups = lessons
            .GroupBy(x => x.UnitKey, StringComparer.Ordinal)
            .OrderBy(x => x.Min(y => y.SortOrder))
            .ThenBy(x => x.Key, StringComparer.Ordinal)
            .Select(group => new Queue<CurriculumPedagogicalLesson>(
                group
                    .OrderBy(x => x.SortOrder)
                    .ThenBy(x => x.Code)))
            .ToArray();

        var selected = new List<CurriculumPedagogicalLesson>(questionCount);
        var offset = groups.Length == 0 ? 0 : Math.Abs(seed == int.MinValue ? 0 : seed) % groups.Length;
        while (selected.Count < questionCount && groups.Any(x => x.Count > 0))
        {
            for (var step = 0; step < groups.Length && selected.Count < questionCount; step++)
            {
                var group = groups[(offset + step) % groups.Length];
                if (group.Count > 0)
                    selected.Add(group.Dequeue());
            }
        }

        return selected;
    }

    private static Guid[] ResolveLessonOutcomeIds(
        AssessmentBuilderPersistenceContext context,
        CurriculumPedagogicalLesson lesson)
    {
        var officialNodeIds = (context.PedagogicalLessonOutcomes ?? [])
            .Where(x => x.PedagogicalLessonId == lesson.Id)
            .Select(x => x.OutcomeNodeId)
            .Distinct()
            .ToHashSet();

        if (officialNodeIds.Count == 0)
            return [];

        return context.LearningOutcomes
            .Where(x =>
                x.OfficialContentNodeId.HasValue &&
                officialNodeIds.Contains(x.OfficialContentNodeId.Value))
            .Select(x => x.Id)
            .Distinct()
            .OrderBy(x => x)
            .ToArray();
    }

    private static IReadOnlyList<ScopedGeneratedItem>? TryGenerateLessons(
        AssessmentBuilderPersistenceContext context,
        Guid schoolId,
        IReadOnlyList<CurriculumPedagogicalLesson> lessons,
        int count,
        AssessmentBuilderDifficulty difficulty,
        int seed,
        Guid createdByUserId)
    {
        if (context.CurriculumAdoption is null ||
            string.IsNullOrWhiteSpace(context.CurriculumAdoption.CurriculumLevelKey) ||
            lessons.Count == 0 ||
            count < lessons.Count)
        {
            return null;
        }

        var difficultyPlan = AssessmentBuilderGenerationPlanner.PlanItemDifficulties(
            difficulty,
            count,
            seed);
        if (difficultyPlan is null || difficultyPlan.Count != count)
            return null;

        var excluded = context.Items
            .Select(x => x.ExposureFingerprint)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToHashSet(StringComparer.Ordinal);
        var generated = new List<ScopedGeneratedItem>(count);
        var baseCount = count / lessons.Count;
        var remainder = count % lessons.Count;
        var engine = new ExactSkillContractQuestionEngine();
        var globalIndex = 0;

        try
        {
            for (var lessonIndex = 0; lessonIndex < lessons.Count; lessonIndex++)
            {
                var lesson = lessons[lessonIndex];
                if (!LessonPracticeCapabilityResolver.TryResolve(lesson.Code, out var contract) ||
                    contract is null)
                {
                    return null;
                }

                var allocation = baseCount + (lessonIndex < remainder ? 1 : 0);
                if (allocation <= 0)
                    return null;

                var outcomeIds = ResolveLessonOutcomeIds(context, lesson);
                var topicId = ResolveSingleTopic(context, outcomeIds);

                for (var localIndex = 0; localIndex < allocation; localIndex++)
                {
                    var plannedDifficulty = difficultyPlan[globalIndex];
                    var generatedQuestion = engine.Generate(
                        "assessment-lesson",
                        lesson.Code,
                        contract.AllowedQuestionFamilies,
                        ResolveExactDifficulty(plannedDifficulty),
                        1,
                        unchecked(seed + ((lessonIndex + 1) * 104729) + (localIndex * 7919)),
                        excluded,
                        globalIndex).Single();

                    if (!ExactSkillContractQuestionEngine.Verify(
                            generatedQuestion.Family,
                            generatedQuestion.Parameters,
                            generatedQuestion.CorrectAnswer))
                    {
                        return null;
                    }

                    var item = new AssessmentItem
                    {
                        Id = Guid.NewGuid(),
                        SchoolId = schoolId,
                        CurriculumAdoptionId = context.CurriculumAdoption.Id,
                        CurriculumPedagogicalLessonId = lesson.Id,
                        CurriculumTopicId = topicId,
                        Source = AssessmentItemSource.SystemGenerated,
                        ItemType = generatedQuestion.ItemType,
                        Difficulty = AssessmentBuilderGenerationPlanner.ToItemDifficulty(
                            plannedDifficulty),
                        Prompt = generatedQuestion.Prompt,
                        CorrectAnswer = generatedQuestion.CorrectAnswer,
                        Solution = generatedQuestion.Solution,
                        CreatedByUserId = createdByUserId,
                        GenerationMethod = "lesson-skill-contract-assessment-solver-verified-v1",
                        GenerationFamily = generatedQuestion.Family,
                        GenerationParametersJson = JsonSerializer.Serialize(new
                        {
                            lessonId = lesson.Id,
                            lessonCode = lesson.Code,
                            skillId = contract.SkillId,
                            skillIds = contract.SkillIds,
                            questionFamily = generatedQuestion.Family,
                            questionVariant = generatedQuestion.VariantId,
                            parameters = generatedQuestion.Parameters
                        }),
                        ExposureFingerprint = generatedQuestion.ExposureFingerprint,
                        ValidationMetadataJson = JsonSerializer.Serialize(new
                        {
                            alignment = "pedagogical-lesson-skill-contract-verified",
                            lessonId = lesson.Id,
                            lessonCode = lesson.Code,
                            unitKey = lesson.UnitKey,
                            skillContract = contract.SkillId,
                            questionVariant = generatedQuestion.VariantId,
                            officialOutcomeCount = outcomeIds.Length,
                            supportingLesson = outcomeIds.Length == 0,
                            solverVerified = true,
                            broadFallbackUsed = false,
                            teacherReviewRequired = true,
                            workflow = "Select-Generate-Review-Approve-Publish"
                        }),
                        CreatedAtUtc = DateTime.UtcNow,
                        RowVersion = []
                    };

                    if (!excluded.Add(item.ExposureFingerprint))
                        return null;

                    generated.Add(new ScopedGeneratedItem(item, outcomeIds));
                    globalIndex++;
                }
            }
        }
        catch (InvalidOperationException)
        {
            return null;
        }

        return generated.Count == count ? generated : null;
    }

    private static ExactSkillQuestionDifficulty ResolveExactDifficulty(
        AssessmentBuilderDifficulty difficulty) =>
        difficulty switch
        {
            AssessmentBuilderDifficulty.Stretch => ExactSkillQuestionDifficulty.Stretch,
            AssessmentBuilderDifficulty.Challenge => ExactSkillQuestionDifficulty.Challenge,
            _ => ExactSkillQuestionDifficulty.Standard
        };

    private static AssessmentItemDifficulty ResolveItemDifficulty(
        AssessmentBuilderDifficulty difficulty) =>
        difficulty switch
        {
            AssessmentBuilderDifficulty.AtClassLevel => AssessmentItemDifficulty.Easy,
            AssessmentBuilderDifficulty.Stretch => AssessmentItemDifficulty.Medium,
            AssessmentBuilderDifficulty.Challenge => AssessmentItemDifficulty.Challenging,
            _ => AssessmentItemDifficulty.Medium
        };

    private MathematicsGenerationBatch? TryGenerate(
        AssessmentBuilderPersistenceContext context,
        Guid schoolId,
        IReadOnlyList<Guid> outcomeIds,
        int count,
        AssessmentBuilderDifficulty difficulty,
        int seed,
        Guid createdByUserId)
    {
        if (context.CurriculumAdoption is null ||
            string.IsNullOrWhiteSpace(context.CurriculumAdoption.CurriculumLevelKey))
        {
            return null;
        }

        var selected = context.LearningOutcomes
            .Where(x => outcomeIds.Contains(x.Id))
            .OrderBy(x => x.Order)
            .ThenBy(x => x.Code)
            .ToArray();

        if (selected.Length != outcomeIds.Count)
            return null;

        var exactContracts = selected
            .Select(outcome =>
                Stage19AssessmentSkillContracts.TryResolve(outcome.Code, out var contract)
                    ? contract
                    : null)
            .ToArray();

        // READY_VERIFIED official outcomes are authoritative once migrated.
        // Never send an exact outcome through the legacy/native generator.
        if (exactContracts.Any(contract => contract is not null))
        {
            if (exactContracts.Any(contract => contract is null))
                return null;

            if (count < selected.Length)
                return null;

            return TryGenerateExact(
                context,
                schoolId,
                selected,
                exactContracts.Select(contract => contract!).ToArray(),
                count,
                difficulty,
                seed,
                createdByUserId);
        }

        var profiles = selected
            .Select(NativeMathematicsOutcomeProfileResolver.Resolve)
            .ToArray();

        if (profiles.Length != outcomeIds.Count || profiles.Any(x => x is null))
            return null;

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
                context.Items
                    .Select(x => x.ExposureFingerprint)
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .ToArray()));

            return new MathematicsQuestionGenerationEngine().Generate(
                new MathematicsGenerationRequest(
                    blueprint,
                    profiles.Select(x => x!).ToArray(),
                    seed));
        }
        catch (InvalidOperationException)
        {
            return null;
        }
    }

    private static MathematicsGenerationBatch? TryGenerateExact(
        AssessmentBuilderPersistenceContext context,
        Guid schoolId,
        IReadOnlyList<LearningOutcome> outcomes,
        IReadOnlyList<Stage19AssessmentSkillContract> contracts,
        int count,
        AssessmentBuilderDifficulty difficulty,
        int seed,
        Guid createdByUserId)
    {
        if (context.CurriculumAdoption is null ||
            outcomes.Count == 0 ||
            outcomes.Count != contracts.Count)
        {
            return null;
        }

        var difficultyPlan = AssessmentBuilderGenerationPlanner.PlanItemDifficulties(
            difficulty,
            count,
            seed);
        if (difficultyPlan is null || difficultyPlan.Count != count)
            return null;

        var generated = new List<GeneratedMathematicsItem>(count);
        var excluded = context.Items
            .Select(x => x.ExposureFingerprint)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToHashSet(StringComparer.Ordinal);

        var baseCount = count / outcomes.Count;
        var remainder = count % outcomes.Count;
        var engine = new Stage19AssessmentSkillContractEngine();
        var globalIndex = 0;

        try
        {
            for (var index = 0; index < outcomes.Count; index++)
            {
                var allocation = baseCount + (index < remainder ? 1 : 0);
                if (allocation <= 0)
                    return null;

                for (var localIndex = 0; localIndex < allocation; localIndex++)
                {
                    var plannedDifficulty = difficultyPlan[globalIndex];
                    var batch = engine.Generate(
                        schoolId,
                        context.CurriculumAdoption.Id,
                        context.CurriculumAdoption.CurriculumLevelKey!,
                        outcomes[index],
                        contracts[index],
                        plannedDifficulty,
                        1,
                        unchecked(seed + ((index + 1) * 7919) + (localIndex * 104729)),
                        excluded,
                        createdByUserId,
                        globalIndex);

                    var item = batch.Items.Single();
                    if (!Stage19AssessmentSkillContractEngine.VerifyPersistedItem(
                            contracts[index],
                            item.Item))
                    {
                        return null;
                    }

                    if (!excluded.Add(item.Item.ExposureFingerprint))
                        return null;

                    generated.Add(item);
                    globalIndex++;
                }
            }
        }
        catch (InvalidOperationException)
        {
            return null;
        }

        if (generated.Count != count)
            return null;

        return new MathematicsGenerationBatch(
            schoolId,
            context.CurriculumAdoption.Id,
            context.CurriculumAdoption.CurriculumLevelKey!,
            generated,
            Stage19AssessmentSkillContracts.GenerationMethod);
    }

    private static AssessmentBuilderWorkspace BuildWorkspace(
        AssessmentDetails details,
        AssessmentBuilderPersistenceContext context)
    {
        var itemById = context.Items.ToDictionary(x => x.Id);
        var lessonsById = (context.PedagogicalLessons ?? [])
            .ToDictionary(x => x.Id);

        var questions = context.Questions
            .OrderBy(x => x.Order)
            .Select(question =>
            {
                itemById.TryGetValue(question.Id, out var item);
                CurriculumPedagogicalLesson? lesson = null;
                if (item?.CurriculumPedagogicalLessonId is Guid lessonId)
                    lessonsById.TryGetValue(lessonId, out lesson);

                return new AssessmentBuilderQuestion(
                    question.Id,
                    question.Order,
                    question.Prompt,
                    question.MaxScore,
                    item?.Source,
                    item?.Difficulty,
                    item is null
                        ? AssessmentBuilderQuestionStatus.Legacy
                        : ReadStatus(item.ValidationMetadataJson),
                    item?.CorrectAnswer ?? string.Empty,
                    item?.Solution ?? string.Empty,
                    context.QuestionOutcomeMappings
                        .Where(x => x.AssessmentQuestionId == question.Id)
                        .Select(x => x.LearningOutcomeId)
                        .Distinct()
                        .OrderBy(x => x)
                        .ToArray())
                {
                    LessonId = item?.CurriculumPedagogicalLessonId,
                    LessonTitle = lesson?.Title
                };
            })
            .ToArray();

        var current = questions.Sum(x => x.MaxScore);
        var mappedIds = questions.SelectMany(x => x.OutcomeIds).Distinct().ToArray();
        var masteryRows = context.ClassOutcomeSummaries
            .Where(x => mappedIds.Contains(x.LearningOutcomeId))
            .ToArray();
        decimal? mastery = masteryRows.Length == 0
            ? null
            : Round(masteryRows.Average(x => x.AverageMasteryPercentage));

        var allRich = questions.Length > 0 &&
            questions.All(x => x.Status != AssessmentBuilderQuestionStatus.Legacy);
        var allApproved = allRich &&
            questions.All(x => x.Status == AssessmentBuilderQuestionStatus.Approved);
        var allAligned = questions.Length > 0 &&
            questions.All(x => x.OutcomeIds.Count > 0 || x.LessonId.HasValue);
        var marksMatch = current == details.Assessment.MaxScore;
        var ready = details.Assessment.Status == AssessmentStatus.Draft &&
            allApproved &&
            allAligned &&
            marksMatch;
        var message = ready ? "ReadyToPublish"
            : !allRich ? "BuilderLegacyQuestionsNeedReplacement"
            : !allApproved ? "BuilderQuestionsNeedApproval"
            : !allAligned ? "BuilderQuestionsNeedOutcomes"
            : !marksMatch ? "BuilderMarksMustMatch"
            : "BuilderNotDraft";

        var eligibleOutcomeIds = details.EligibleOutcomes
            .Select(x => x.Id)
            .ToHashSet();
        var aiSupportedOutcomeIds = context.LearningOutcomes
            .Where(x =>
                eligibleOutcomeIds.Contains(x.Id) &&
                (Stage19AssessmentSkillContracts.TryResolve(x.Code, out _) ||
                 NativeMathematicsOutcomeProfileResolver.Resolve(x) is not null))
            .Select(x => x.Id)
            .Distinct()
            .OrderBy(x => x)
            .ToArray();

        var lessonOptions = (context.PedagogicalLessons ?? [])
            .OrderBy(x => x.SortOrder)
            .ThenBy(x => x.Code)
            .Select(lesson =>
            {
                var outcomeIds = ResolveLessonOutcomeIds(context, lesson);
                return new AssessmentBuilderLessonOption(
                    lesson.Id,
                    lesson.Code,
                    lesson.UnitKey,
                    lesson.UnitTitle,
                    lesson.Title,
                    outcomeIds.Length > 0,
                    LessonPracticeCapabilityResolver.TryResolve(lesson.Code, out _),
                    outcomeIds);
            })
            .ToArray();

        var unitOptions = lessonOptions
            .GroupBy(x => new { x.UnitKey, x.UnitTitle })
            .OrderBy(x => x.Min(y =>
                (context.PedagogicalLessons ?? [])
                    .FirstOrDefault(z => z.Id == y.Id)?.SortOrder ?? int.MaxValue))
            .ThenBy(x => x.Key.UnitKey, StringComparer.Ordinal)
            .Select(group => new AssessmentBuilderUnitOption(
                group.Key.UnitKey,
                group.Key.UnitTitle,
                group.Count(),
                group.Count(x => x.AiSupported)))
            .ToArray();

        var canGenerate = context.CurriculumAdoption is not null &&
            !string.IsNullOrWhiteSpace(context.CurriculumAdoption.CurriculumLevelKey) &&
            (lessonOptions.Any(x => x.AiSupported) || aiSupportedOutcomeIds.Length > 0);

        return new AssessmentBuilderWorkspace(
            details,
            questions,
            current,
            Math.Max(0m, details.Assessment.MaxScore - current),
            mastery,
            canGenerate,
            ready,
            message)
        {
            AiSupportedOutcomeIds = aiSupportedOutcomeIds,
            Lessons = lessonOptions,
            Units = unitOptions
        };
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

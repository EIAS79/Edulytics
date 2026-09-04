using System.Security.Cryptography;
using System.Text.Json;
using Edulytics.Core.AssessmentIntelligence;
using Edulytics.Core.Entities;
using Edulytics.Core.Enums;
using Edulytics.Core.MathematicsGeneration;
using Edulytics.Core.Practice;
using Edulytics.Services.Assessments;
using Edulytics.Services.AssessmentIntelligence;
using Edulytics.Services.MathematicsGeneration;

namespace Edulytics.Services.Practice;

public sealed class StudentPrivatePracticeService(
    IStudentPrivatePracticeRepository repository) : IStudentPrivatePracticeService
{
    public async Task<StudentPrivatePracticeWorkspace> GetWorkspaceAsync(
        Guid studentUserId,
        Guid? curriculumAdoptionId = null,
        CancellationToken cancellationToken = default)
    {
        var curricula = await repository.ListCurriculaAsync(studentUserId, cancellationToken);
        var selected = curriculumAdoptionId ?? curricula.FirstOrDefault()?.CurriculumAdoptionId;
        IReadOnlyList<StudentPrivatePracticeLessonOption> lessons = [];
        IReadOnlyList<string> units = [];

        if (selected.HasValue)
        {
            var context = await repository.GetContextAsync(studentUserId, selected.Value, cancellationToken);
            if (context is not null)
            {
                lessons = context.Lessons
                    .Select(x => new StudentPrivatePracticeLessonOption(x.Id, x.UnitKey, x.UnitTitle, x.Code, x.Title))
                    .ToArray();
                units = context.Lessons.Select(x => x.UnitKey).Where(x => !string.IsNullOrWhiteSpace(x))
                    .Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
            }
        }

        var attempts = await repository.ListPrivateAttemptsAsync(studentUserId, cancellationToken);
        return new StudentPrivatePracticeWorkspace(curricula, selected, lessons, units, attempts);
    }

    public async Task<StudentPrivatePracticeResult> GenerateAsync(
        Guid studentUserId,
        GenerateStudentPrivatePracticeRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request.QuestionCount is < 1 or > 20)
            return StudentPrivatePracticeResult.Failure(StudentPrivatePracticeError.InvalidQuestionCount);

        var context = await repository.GetContextAsync(studentUserId, request.CurriculumAdoptionId, cancellationToken);
        if (context is null)
            return StudentPrivatePracticeResult.Failure(StudentPrivatePracticeError.CurriculumNotAvailable);

        var scoped = ResolveScope(context, request);
        if (scoped.Error.HasValue)
            return StudentPrivatePracticeResult.Failure(scoped.Error.Value);

        var supported = scoped.Outcomes!
            .Select(x => (Outcome: x, Profile: NativeMathematicsOutcomeProfileResolver.Resolve(x)))
            .Where(x => x.Profile is not null)
            .ToArray();
        if (supported.Length == 0)
            return StudentPrivatePracticeResult.Failure(StudentPrivatePracticeError.NoSupportedOutcomes);

        var masteryByOutcome = context.OfficialMasteries.ToDictionary(x => x.LearningOutcomeId, x => x.MasteryPercentage);
        IEnumerable<(LearningOutcome Outcome, MathematicsOutcomeGenerationProfile? Profile)> ordered = supported;
        if (request.Scope == StudentPrivatePracticeScope.WeakAreas)
        {
            ordered = supported.OrderBy(x => masteryByOutcome.TryGetValue(x.Outcome.Id, out var mastery) ? mastery : 50m)
                .ThenBy(x => x.Outcome.Order);
        }
        var selected = ordered.Take(Math.Min(request.QuestionCount, supported.Length)).ToArray();
        var selectedIds = selected.Select(x => x.Outcome.Id).ToArray();
        var policy = ResolveDifficulty(request.Difficulty, selectedIds, masteryByOutcome);
        var topicIds = selected.Select(x => x.Outcome.TopicId).Distinct().ToArray();
        var topicId = topicIds.Length == 1 ? topicIds[0] : (Guid?)null;
        var excluded = context.Exposures.Select(x => x.ExposureFingerprint)
            .Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().ToArray();

        try
        {
            var blueprint = new AssessmentBlueprintEngine().Build(new AssessmentBlueprintRequest(
                context.Student.SchoolId,
                context.Adoption.Id,
                context.Adoption.CurriculumLevelKey!,
                topicId,
                scoped.LessonId,
                selectedIds,
                null,
                AssessmentPurpose.StudentPersonalTest,
                request.QuestionCount,
                policy,
                excluded));

            var seed = request.Seed != 0
                ? request.Seed
                : RandomNumberGenerator.GetInt32(1, int.MaxValue);
            var batch = new MathematicsQuestionGenerationEngine().Generate(new MathematicsGenerationRequest(
                blueprint,
                selected.Select(x => x.Profile!).ToArray(),
                seed));

            if (batch.Items.Count != request.QuestionCount)
                return StudentPrivatePracticeResult.Failure(StudentPrivatePracticeError.GenerationFailed);

            var now = DateTime.UtcNow;
            var attemptId = Guid.NewGuid();
            var items = batch.Items.Select(x => x.Item).ToArray();
            var itemOutcomes = batch.Items.Select(x => x.OutcomeLink).ToArray();
            foreach (var item in items)
            {
                item.CreatedByUserId = studentUserId;
                item.ValidationMetadataJson = JsonSerializer.Serialize(new
                {
                    purpose = "student-personal-test",
                    privacy = "student-private",
                    scope = request.Scope.ToString(),
                    difficulty = request.Difficulty.ToString()
                });
            }

            var attempt = new PracticeAttempt
            {
                Id = attemptId,
                SchoolId = context.Student.SchoolId,
                StudentProfileId = context.Student.Id,
                CurriculumAdoptionId = context.Adoption.Id,
                CurriculumPedagogicalLessonId = scoped.LessonId,
                IsPrivate = true,
                Status = PracticeAttemptStatus.InProgress,
                StartedAtUtc = now,
                Score = 0m,
                MaxScore = request.QuestionCount,
                Percentage = 0m
            };

            var attemptItems = items.Select((item, index) => new PracticeAttemptItem
            {
                Id = Guid.NewGuid(),
                SchoolId = context.Student.SchoolId,
                PracticeAttemptId = attemptId,
                AssessmentItemId = item.Id,
                Order = index + 1,
                MaxScore = 1m
            }).ToArray();
            var exposures = items.Select(item => new StudentItemExposure
            {
                Id = Guid.NewGuid(),
                SchoolId = context.Student.SchoolId,
                StudentProfileId = context.Student.Id,
                AssessmentItemId = item.Id,
                ExposureFingerprint = item.ExposureFingerprint,
                ExposedAtUtc = now
            }).ToArray();

            await repository.AddGeneratedAttemptAsync(
                items, itemOutcomes, attempt, attemptItems, exposures, cancellationToken);
            return StudentPrivatePracticeResult.Success(attemptId);
        }
        catch (InvalidOperationException)
        {
            return StudentPrivatePracticeResult.Failure(StudentPrivatePracticeError.GenerationFailed);
        }
    }

    private static (IReadOnlyList<LearningOutcome>? Outcomes, Guid? LessonId, StudentPrivatePracticeError? Error) ResolveScope(
        StudentPrivatePracticeContext context,
        GenerateStudentPrivatePracticeRequest request)
    {
        if (request.Scope == StudentPrivatePracticeScope.WholeCurriculum)
            return (context.LearningOutcomes, null, null);

        if (request.Scope == StudentPrivatePracticeScope.WeakAreas)
        {
            var weakIds = context.OfficialMasteries
                .Where(x => x.MasteryPercentage < 70m)
                .OrderBy(x => x.MasteryPercentage)
                .Select(x => x.LearningOutcomeId)
                .ToHashSet();
            var weak = context.LearningOutcomes.Where(x => weakIds.Contains(x.Id)).ToArray();
            return (weak.Length > 0 ? weak : context.LearningOutcomes, null, null);
        }

        CurriculumPedagogicalLesson[] lessons;
        Guid? exactLessonId = null;
        if (request.Scope == StudentPrivatePracticeScope.Lesson)
        {
            if (!request.LessonId.HasValue)
                return (null, null, StudentPrivatePracticeError.InvalidScope);
            lessons = context.Lessons.Where(x => x.Id == request.LessonId.Value).ToArray();
            exactLessonId = lessons.Length == 1 ? lessons[0].Id : null;
        }
        else if (request.Scope == StudentPrivatePracticeScope.Unit)
        {
            if (string.IsNullOrWhiteSpace(request.UnitKey))
                return (null, null, StudentPrivatePracticeError.InvalidScope);
            lessons = context.Lessons.Where(x => string.Equals(x.UnitKey, request.UnitKey.Trim(), StringComparison.OrdinalIgnoreCase)).ToArray();
        }
        else
        {
            return (null, null, StudentPrivatePracticeError.InvalidScope);
        }

        if (lessons.Length == 0)
            return (null, null, StudentPrivatePracticeError.InvalidScope);

        var lessonIds = lessons.Select(x => x.Id).ToHashSet();
        var officialNodeIds = context.LessonOutcomes
            .Where(x => lessonIds.Contains(x.PedagogicalLessonId))
            .Select(x => x.OutcomeNodeId).ToHashSet();
        var outcomes = context.LearningOutcomes
            .Where(x => x.OfficialContentNodeId.HasValue && officialNodeIds.Contains(x.OfficialContentNodeId.Value))
            .ToArray();
        return outcomes.Length == 0
            ? (null, exactLessonId, StudentPrivatePracticeError.NoSupportedOutcomes)
            : (outcomes, exactLessonId, null);
    }

    private static AssessmentDifficultyPolicy ResolveDifficulty(
        StudentPrivatePracticeDifficulty difficulty,
        IReadOnlyList<Guid> outcomeIds,
        IReadOnlyDictionary<Guid, decimal> masteryByOutcome)
    {
        if (difficulty == StudentPrivatePracticeDifficulty.AtClassLevel)
            return AssessmentDifficultyPolicy.Balanced;
        if (difficulty == StudentPrivatePracticeDifficulty.Stretch)
            return AssessmentDifficultyPolicy.Stretch;
        if (difficulty == StudentPrivatePracticeDifficulty.Challenge)
            return new AssessmentDifficultyPolicy(5, 30, 65);

        var values = outcomeIds.Where(masteryByOutcome.ContainsKey).Select(x => masteryByOutcome[x]).ToArray();
        if (values.Length == 0) return AssessmentDifficultyPolicy.Balanced;
        var average = values.Average();
        return average < 50m
            ? AssessmentDifficultyPolicy.Supportive
            : average < 80m
                ? AssessmentDifficultyPolicy.Balanced
                : AssessmentDifficultyPolicy.Stretch;
    }
}

using System.Security.Cryptography;
using System.Text;
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
        IReadOnlyList<StudentPrivatePracticeUnitOption> unitOptions = [];

        if (selected.HasValue)
        {
            var context = await repository.GetContextAsync(studentUserId, selected.Value, cancellationToken);
            if (context is not null)
            {
                var officialNodeIdsByLesson = context.LessonOutcomes
                    .GroupBy(x => x.PedagogicalLessonId)
                    .ToDictionary(
                        group => group.Key,
                        group => (IReadOnlyList<Guid>)group
                            .OrderBy(x => x.SortOrder)
                            .Select(x => x.OutcomeNodeId)
                            .Distinct()
                            .ToArray());

                lessons = context.Lessons
                    .Select(x => new StudentPrivatePracticeLessonOption(
                        x.Id,
                        x.UnitKey,
                        x.UnitTitle,
                        x.Code,
                        x.Title,
                        officialNodeIdsByLesson.TryGetValue(x.Id, out var mappedNodeIds)
                            ? mappedNodeIds
                            : []))
                    .ToArray();

                unitOptions = lessons
                    .Where(x => !string.IsNullOrWhiteSpace(x.UnitKey))
                    .GroupBy(x => x.UnitKey, StringComparer.OrdinalIgnoreCase)
                    .Select(group =>
                    {
                        var first = group.First();
                        return new StudentPrivatePracticeUnitOption(
                            first.UnitKey,
                            string.IsNullOrWhiteSpace(first.UnitTitle)
                                ? "Unit"
                                : first.UnitTitle,
                            group.SelectMany(x => x.OfficialOutcomeNodeIds)
                                .Distinct()
                                .ToArray());
                    })
                    .ToArray();
                units = unitOptions.Select(x => x.UnitKey).ToArray();
            }
        }

        var attempts = await repository.ListPrivateAttemptsAsync(studentUserId, cancellationToken);
        return new StudentPrivatePracticeWorkspace(
            curricula,
            selected,
            lessons,
            units,
            attempts,
            unitOptions);
    }

    public async Task<StudentPrivatePracticeResult> GenerateAsync(
        Guid studentUserId,
        GenerateStudentPrivatePracticeRequest request,
        CancellationToken cancellationToken = default)
    {
        var questionLimit = QuestionLimitForScope(request.Scope);
        if (questionLimit == 0 || request.QuestionCount < 1 || request.QuestionCount > questionLimit)
            return StudentPrivatePracticeResult.Failure(StudentPrivatePracticeError.InvalidQuestionCount);

        var context = await repository.GetContextAsync(studentUserId, request.CurriculumAdoptionId, cancellationToken);
        if (context is null)
            return StudentPrivatePracticeResult.Failure(StudentPrivatePracticeError.CurriculumNotAvailable);

        var scoped = ResolveScope(context, request);
        if (scoped.Error.HasValue)
            return StudentPrivatePracticeResult.Failure(scoped.Error.Value);

        var masteryByOutcome = context.OfficialMasteries
            .ToDictionary(x => x.LearningOutcomeId, x => x.MasteryPercentage);
        var supported = scoped.Outcomes!
            .Select(x => (Outcome: x, Profile: NativeMathematicsOutcomeProfileResolver.Resolve(x)))
            .Where(x => x.Profile is not null)
            .ToArray();

        var contextOnly = false;
        MathematicsOutcomeGenerationProfile[] profiles;
        Guid[] selectedIds;
        Guid? topicId;

        if (supported.Length > 0)
        {
            IEnumerable<(LearningOutcome Outcome, MathematicsOutcomeGenerationProfile? Profile)> ordered = supported;
            if (request.Scope == StudentPrivatePracticeScope.WeakAreas)
            {
                ordered = supported
                    .OrderBy(x => masteryByOutcome.TryGetValue(x.Outcome.Id, out var mastery) ? mastery : 50m)
                    .ThenBy(x => x.Outcome.Order);
            }

            var selected = ordered
                .Take(Math.Min(request.QuestionCount, supported.Length))
                .ToArray();
            profiles = selected.Select(x => x.Profile!).ToArray();
            selectedIds = selected.Select(x => x.Outcome.Id).ToArray();
            var topicIds = selected.Select(x => x.Outcome.TopicId).Distinct().ToArray();
            topicId = topicIds.Length == 1 ? topicIds[0] : (Guid?)null;
        }
        else
        {
            profiles = BuildPedagogicalContextProfiles(context, request);
            if (profiles.Length == 0)
                return StudentPrivatePracticeResult.Failure(StudentPrivatePracticeError.NoSupportedOutcomes);

            // Cambridge reference-only pedagogical blueprints deliberately contain
            // no formal outcome mappings. Private practice may still use the
            // Edulytics-authored lesson/unit context, but must not manufacture an
            // official Cambridge alignment or write fake mastery evidence.
            contextOnly = true;
            selectedIds = profiles.Select(x => x.LearningOutcomeId).ToArray();
            topicId = null;
        }

        var policy = ResolveDifficulty(request.Difficulty, selectedIds, masteryByOutcome);
        var excluded = context.Exposures
            .Select(x => x.ExposureFingerprint)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Distinct()
            .ToArray();

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
                profiles,
                seed));

            if (batch.Items.Count != request.QuestionCount)
                return StudentPrivatePracticeResult.Failure(StudentPrivatePracticeError.GenerationFailed);

            var now = DateTime.UtcNow;
            var attemptId = Guid.NewGuid();
            var items = batch.Items.Select(x => x.Item).ToArray();
            var itemOutcomes = contextOnly
                ? Array.Empty<AssessmentItemOutcome>()
                : batch.Items.Select(x => x.OutcomeLink).ToArray();

            foreach (var item in items)
            {
                item.CreatedByUserId = studentUserId;
                item.ValidationMetadataJson = JsonSerializer.Serialize(new
                {
                    purpose = "student-personal-test",
                    privacy = "student-private",
                    scope = request.Scope.ToString(),
                    difficulty = request.Difficulty.ToString(),
                    alignment = contextOnly
                        ? "pedagogical-context-only"
                        : "official-learning-outcome",
                    officialMasteryEvidence = !contextOnly
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
                items,
                itemOutcomes,
                attempt,
                attemptItems,
                exposures,
                cancellationToken);
            return StudentPrivatePracticeResult.Success(attemptId);
        }
        catch (InvalidOperationException)
        {
            return StudentPrivatePracticeResult.Failure(StudentPrivatePracticeError.GenerationFailed);
        }
    }

    private static int QuestionLimitForScope(StudentPrivatePracticeScope scope) => scope switch
    {
        StudentPrivatePracticeScope.Lesson => 10,
        StudentPrivatePracticeScope.Unit => 15,
        StudentPrivatePracticeScope.WeakAreas => 15,
        StudentPrivatePracticeScope.WholeCurriculum => 30,
        _ => 0
    };

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
            lessons = context.Lessons
                .Where(x => string.Equals(x.UnitKey, request.UnitKey.Trim(), StringComparison.OrdinalIgnoreCase))
                .ToArray();
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
            .Select(x => x.OutcomeNodeId)
            .ToHashSet();
        var outcomes = context.LearningOutcomes
            .Where(x => x.OfficialContentNodeId.HasValue && officialNodeIds.Contains(x.OfficialContentNodeId.Value))
            .ToArray();

        // An empty formal mapping is valid for reference-only pedagogical
        // blueprints (notably Cambridge). Generation can fall back to the
        // independently-authored lesson context without inventing an official
        // curriculum relationship.
        return (outcomes, exactLessonId, null);
    }

    private static MathematicsOutcomeGenerationProfile[] BuildPedagogicalContextProfiles(
        StudentPrivatePracticeContext context,
        GenerateStudentPrivatePracticeRequest request)
    {
        CurriculumPedagogicalLesson[] lessons;
        if (request.Scope == StudentPrivatePracticeScope.Lesson && request.LessonId.HasValue)
        {
            lessons = context.Lessons
                .Where(x => x.Id == request.LessonId.Value)
                .ToArray();
        }
        else if (request.Scope == StudentPrivatePracticeScope.Unit && !string.IsNullOrWhiteSpace(request.UnitKey))
        {
            lessons = context.Lessons
                .Where(x => string.Equals(x.UnitKey, request.UnitKey.Trim(), StringComparison.OrdinalIgnoreCase))
                .OrderBy(x => x.SortOrder)
                .ToArray();
        }
        else
        {
            return [];
        }

        return lessons
            .Select(lesson =>
            {
                var semanticContext = string.Join(
                    ". ",
                    new[]
                    {
                        "Mathematics",
                        lesson.UnitTitle?.Trim(),
                        lesson.Title?.Trim()
                    }.Where(x => !string.IsNullOrWhiteSpace(x)));
                var capability = MathematicsAiCapabilityMatrix.Resolve(null, semanticContext);
                if (!capability.CanGenerate)
                    return null;

                var localId = StableContextId(context.Adoption.Id, lesson.Id);
                return new MathematicsOutcomeGenerationProfile(
                    localId,
                    $"PRIVATE-PED:{lesson.Code}",
                    capability.GenerationFamilies)
                {
                    CanonicalSkills = capability.CanonicalSkills,
                    GenerationContext = semanticContext,
                    IsContextualAssisted = !capability.CanGenerateVerified
                };
            })
            .Where(x => x is not null)
            .Select(x => x!)
            .ToArray();
    }

    private static Guid StableContextId(Guid curriculumAdoptionId, Guid lessonId)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(
            $"student-private-pedagogical-context|{curriculumAdoptionId:D}|{lessonId:D}"));
        var guidBytes = bytes.AsSpan(0, 16).ToArray();
        return new Guid(guidBytes);
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

        var values = outcomeIds
            .Where(masteryByOutcome.ContainsKey)
            .Select(x => masteryByOutcome[x])
            .ToArray();
        if (values.Length == 0)
            return AssessmentDifficultyPolicy.Balanced;

        var average = values.Average();
        return average < 50m
            ? AssessmentDifficultyPolicy.Supportive
            : average < 80m
                ? AssessmentDifficultyPolicy.Balanced
                : AssessmentDifficultyPolicy.Stretch;
    }
}

using Edulytics.Core.Analytics;
using Edulytics.Core.Entities;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Analytics;

internal static class MasteryEvidenceEngine
{
    internal const string FormulaVersion = "phase31-v2";

    internal static StudentOutcomeMastery[] BuildMasteries(
        AnalyticsSourceSnapshot source,
        DateTime calculatedAtUtc)
    {
        var classes = source.ClassGroups.ToDictionary(x => x.Id);
        var students = source.StudentProfiles.ToDictionary(x => x.Id);
        var outcomes = source.LearningOutcomes.ToDictionary(x => x.Id);
        var attempts = (source.PracticeAttempts ?? [])
            .ToDictionary(x => x.Id);
        var evidence = source.LearningEvidence ?? [];

        var accumulator =
            new Dictionary<StudentOutcomeKey, ScoreAccumulator>();

        foreach (var item in evidence
                     .OrderBy(x => x.OccurredAtUtc)
                     .ThenBy(x => x.Id))
        {
            if (!students.TryGetValue(item.StudentProfileId, out var student) ||
                student.SchoolId != item.SchoolId)
            {
                throw new InvalidOperationException(
                    "LearningEvidence StudentProfile is missing or outside school scope.");
            }

            if (!outcomes.TryGetValue(item.LearningOutcomeId, out var outcome) ||
                outcome.SchoolId != item.SchoolId)
            {
                throw new InvalidOperationException(
                    "LearningEvidence LearningOutcome is missing or outside school scope.");
            }

            if (!attempts.TryGetValue(item.PracticeAttemptId, out var attempt) ||
                attempt.SchoolId != item.SchoolId ||
                attempt.StudentProfileId != item.StudentProfileId)
            {
                throw new InvalidOperationException(
                    "LearningEvidence PracticeAttempt is missing or violates student scope.");
            }

            if (!outcome.CurriculumAdoptionId.HasValue ||
                outcome.CurriculumAdoptionId.Value != attempt.CurriculumAdoptionId)
            {
                throw new InvalidOperationException(
                    "LearningEvidence violates curriculum adoption or pathway scope.");
            }

            if (item.EvidenceType != LearningEvidenceType.Practice)
            {
                throw new InvalidOperationException(
                    "Unsupported LearningEvidence type.");
            }

            if (item.MaxScore <= 0m ||
                item.Score < 0m ||
                item.Score > item.MaxScore)
            {
                throw new InvalidOperationException(
                    "LearningEvidence contains an invalid score.");
            }

            var enrolledClasses = source.StudentEnrollments
                .Where(x =>
                    x.SchoolId == item.SchoolId &&
                    x.StudentProfileId == item.StudentProfileId &&
                    classes.TryGetValue(x.ClassGroupId, out var cls) &&
                    cls.SchoolId == item.SchoolId &&
                    cls.AcademicYearId == x.AcademicYearId &&
                    cls.CurriculumAdoptionId == attempt.CurriculumAdoptionId)
                .Select(x => classes[x.ClassGroupId])
                .DistinctBy(x => x.Id)
                .ToArray();

            if (enrolledClasses.Length != 1)
            {
                throw new InvalidOperationException(
                    "LearningEvidence cannot be mapped to exactly one enrolled curriculum class.");
            }

            var classGroup = enrolledClasses[0];
            var effectiveWeight =
                DifficultyWeight(item.Difficulty) *
                RecencyWeight(item.OccurredAtUtc, calculatedAtUtc);
            var normalizedScore = item.Score / item.MaxScore;

            AddEvidence(
                accumulator,
                new StudentOutcomeKey(
                    item.SchoolId,
                    classGroup.AcademicYearId,
                    classGroup.Id,
                    outcome.SubjectId,
                    item.StudentProfileId,
                    outcome.Id),
                normalizedScore,
                effectiveWeight);
        }

        foreach (var item in BuildFormalEvidencePoints(
                     source,
                     calculatedAtUtc))
        {
            AddEvidence(
                accumulator,
                new StudentOutcomeKey(
                    item.SchoolId,
                    item.AcademicYearId,
                    item.ClassGroupId,
                    item.SubjectId,
                    item.StudentProfileId,
                    item.LearningOutcomeId),
                item.NormalizedScore,
                item.EffectiveWeight);
        }

        return accumulator
            .OrderBy(x => x.Key.AcademicYearId)
            .ThenBy(x => x.Key.ClassGroupId)
            .ThenBy(x => x.Key.SubjectId)
            .ThenBy(x => x.Key.StudentProfileId)
            .ThenBy(x => x.Key.LearningOutcomeId)
            .Select(x =>
            {
                var percentage = Percentage(
                    x.Value.WeightedEarned,
                    x.Value.WeightedPossible);

                return new StudentOutcomeMastery
                {
                    Id = Guid.NewGuid(),
                    SchoolId = x.Key.SchoolId,
                    AcademicYearId = x.Key.AcademicYearId,
                    ClassGroupId = x.Key.ClassGroupId,
                    SubjectId = x.Key.SubjectId,
                    StudentProfileId = x.Key.StudentProfileId,
                    LearningOutcomeId = x.Key.LearningOutcomeId,
                    EarnedScore = Round4(x.Value.WeightedEarned),
                    PossibleScore = Round4(x.Value.WeightedPossible),
                    MasteryPercentage = percentage,
                    EvidenceCount = x.Value.EvidenceCount,
                    Band = AnalyticsProjectionBuilder.BandFor(percentage),
                    CalculatedAtUtc = calculatedAtUtc
                };
            })
            .ToArray();
    }

    internal static StudentLearningProfile BuildProfile(
        AnalyticsSourceSnapshot source,
        Guid studentProfileId,
        Guid curriculumAdoptionId,
        DateTime calculatedAtUtc)
    {
        var student = source.StudentProfiles
            .SingleOrDefault(x => x.Id == studentProfileId)
            ?? throw new InvalidOperationException(
                "StudentProfile is missing.");

        var classes = source.ClassGroups.ToDictionary(x => x.Id);
        var enrolledClasses = source.StudentEnrollments
            .Where(x =>
                x.SchoolId == student.SchoolId &&
                x.StudentProfileId == studentProfileId &&
                classes.TryGetValue(x.ClassGroupId, out var cls) &&
                cls.CurriculumAdoptionId == curriculumAdoptionId)
            .Select(x => classes[x.ClassGroupId])
            .DistinctBy(x => x.Id)
            .ToArray();

        if (enrolledClasses.Length != 1)
        {
            throw new InvalidOperationException(
                "Student learning profile requires exactly one enrolled class for the curriculum adoption.");
        }

        var classGroup = enrolledClasses[0];
        var attempts = (source.PracticeAttempts ?? [])
            .Where(x =>
                x.SchoolId == student.SchoolId &&
                x.StudentProfileId == studentProfileId &&
                x.CurriculumAdoptionId == curriculumAdoptionId)
            .ToDictionary(x => x.Id);
        var evidence = (source.LearningEvidence ?? [])
            .Where(x =>
                x.SchoolId == student.SchoolId &&
                x.StudentProfileId == studentProfileId &&
                attempts.ContainsKey(x.PracticeAttemptId))
            .ToArray();
        var formalEvidence = BuildFormalEvidencePoints(source, calculatedAtUtc)
            .Where(x =>
                x.SchoolId == student.SchoolId &&
                x.StudentProfileId == studentProfileId &&
                x.ClassGroupId == classGroup.Id)
            .ToArray();

        var masteries = BuildMasteries(source, calculatedAtUtc)
            .Where(x =>
                x.StudentProfileId == studentProfileId &&
                x.ClassGroupId == classGroup.Id)
            .ToArray();
        var outcomes = source.LearningOutcomes.ToDictionary(x => x.Id);

        var rows = masteries
            .OrderBy(x => outcomes[x.LearningOutcomeId].Order)
            .ThenBy(x => outcomes[x.LearningOutcomeId].Code)
            .Select(mastery =>
            {
                var outcome = outcomes[mastery.LearningOutcomeId];
                var practiceRowsForOutcome = evidence
                    .Where(x => x.LearningOutcomeId == mastery.LearningOutcomeId)
                    .ToArray();
                var formalRowsForOutcome = formalEvidence
                    .Where(x => x.LearningOutcomeId == mastery.LearningOutcomeId)
                    .ToArray();
                var latestForOutcome = practiceRowsForOutcome
                    .Select(x => x.OccurredAtUtc)
                    .Concat(formalRowsForOutcome.Select(x => x.OccurredAtUtc))
                    .Cast<DateTime?>()
                    .DefaultIfEmpty()
                    .Max();

                return new StudentOutcomeLearningProfile(
                    mastery.LearningOutcomeId,
                    outcome.Code,
                    outcome.Description,
                    mastery.MasteryPercentage,
                    mastery.Band,
                    mastery.EvidenceCount,
                    ConfidenceFor(mastery.EvidenceCount),
                    latestForOutcome,
                    practiceRowsForOutcome.Count(x => x.Difficulty == AssessmentItemDifficulty.Easy),
                    practiceRowsForOutcome.Count(x => x.Difficulty == AssessmentItemDifficulty.Medium),
                    practiceRowsForOutcome.Count(x => x.Difficulty == AssessmentItemDifficulty.Challenging),
                    mastery.PossibleScore,
                    FormulaVersion);
            })
            .ToArray();

        var earned = masteries.Sum(x => x.EarnedScore);
        var possible = masteries.Sum(x => x.PossibleScore);
        var overall = Percentage(earned, possible);
        var latest = evidence
            .Select(x => x.OccurredAtUtc)
            .Concat(formalEvidence.Select(x => x.OccurredAtUtc))
            .Cast<DateTime?>()
            .DefaultIfEmpty()
            .Max();
        var confidence = rows.Length == 0
            ? 0m
            : Round2(rows.Average(x => x.ConfidencePercentage));

        return new StudentLearningProfile(
            student.SchoolId,
            studentProfileId,
            classGroup.AcademicYearId,
            classGroup.Id,
            curriculumAdoptionId,
            overall,
            AnalyticsProjectionBuilder.BandFor(overall),
            evidence.Length + formalEvidence.Length,
            confidence,
            latest,
            rows,
            FormulaVersion);
    }

    private static FormalEvidencePoint[] BuildFormalEvidencePoints(
        AnalyticsSourceSnapshot source,
        DateTime calculatedAtUtc)
    {
        var classes = source.ClassGroups.ToDictionary(x => x.Id);
        var students = source.StudentProfiles.ToDictionary(x => x.Id);
        var outcomes = source.LearningOutcomes.ToDictionary(x => x.Id);
        var assessments = source.Assessments
            .Where(x => x.Status != AssessmentStatus.Draft)
            .ToDictionary(x => x.Id);
        var questions = source.AssessmentQuestions.ToDictionary(x => x.Id);
        var results = source.AssessmentResults
            .Where(x => assessments.ContainsKey(x.AssessmentId))
            .ToDictionary(x => x.Id);
        var mappingsByQuestion = source.OutcomeMappings
            .GroupBy(x => x.AssessmentQuestionId)
            .ToDictionary(x => x.Key, x => x.ToArray());
        var enrollments = source.StudentEnrollments
            .Select(x => (x.SchoolId, x.AcademicYearId, x.ClassGroupId, x.StudentProfileId))
            .ToHashSet();
        var points = new List<FormalEvidencePoint>();

        foreach (var answer in source.StudentAnswers
                     .OrderBy(x => x.UpdatedAtUtc)
                     .ThenBy(x => x.Id))
        {
            if (!results.TryGetValue(answer.AssessmentResultId, out var result) ||
                !assessments.TryGetValue(result.AssessmentId, out var assessment))
            {
                continue;
            }

            if (answer.SchoolId != assessment.SchoolId ||
                result.SchoolId != assessment.SchoolId ||
                !students.TryGetValue(result.StudentProfileId, out var student) ||
                student.SchoolId != assessment.SchoolId)
            {
                throw new InvalidOperationException(
                    "Formal assessment result violates school or student scope.");
            }

            if (!classes.TryGetValue(assessment.ClassGroupId, out var classGroup) ||
                classGroup.SchoolId != assessment.SchoolId ||
                classGroup.AcademicYearId != assessment.AcademicYearId)
            {
                throw new InvalidOperationException(
                    "Formal assessment class is missing or outside academic-year scope.");
            }

            if (!enrollments.Contains((
                    assessment.SchoolId,
                    assessment.AcademicYearId,
                    assessment.ClassGroupId,
                    result.StudentProfileId)))
            {
                throw new InvalidOperationException(
                    "Formal assessment result references a student outside the assessment class.");
            }

            if (!questions.TryGetValue(answer.AssessmentQuestionId, out var question) ||
                question.SchoolId != assessment.SchoolId ||
                question.AssessmentId != assessment.Id)
            {
                throw new InvalidOperationException(
                    "Formal assessment answer references an invalid question.");
            }

            if (question.MaxScore <= 0m ||
                answer.Score < 0m ||
                answer.Score > question.MaxScore)
            {
                throw new InvalidOperationException(
                    "Formal assessment answer contains an invalid score.");
            }

            if (!mappingsByQuestion.TryGetValue(question.Id, out var mappings) ||
                mappings.Length == 0)
            {
                throw new InvalidOperationException(
                    "Formal assessment question has no learning-outcome mapping.");
            }

            var normalizedScore = answer.Score / question.MaxScore;
            var effectiveWeight =
                RecencyWeight(answer.UpdatedAtUtc, calculatedAtUtc) /
                mappings.Length;

            foreach (var mapping in mappings.OrderBy(x => x.LearningOutcomeId))
            {
                if (mapping.SchoolId != assessment.SchoolId ||
                    !outcomes.TryGetValue(mapping.LearningOutcomeId, out var outcome) ||
                    outcome.SchoolId != assessment.SchoolId ||
                    outcome.SubjectId != assessment.SubjectId ||
                    outcome.AcademicProgramId != classGroup.AcademicProgramId ||
                    outcome.GradeLevelId != classGroup.GradeLevelId ||
                    (outcome.CurriculumAdoptionId.HasValue &&
                     classGroup.CurriculumAdoptionId.HasValue &&
                     outcome.CurriculumAdoptionId.Value != classGroup.CurriculumAdoptionId.Value))
                {
                    throw new InvalidOperationException(
                        "Formal assessment learning outcome violates assessment scope.");
                }

                points.Add(
                    new FormalEvidencePoint(
                        assessment.SchoolId,
                        assessment.AcademicYearId,
                        assessment.ClassGroupId,
                        assessment.SubjectId,
                        result.StudentProfileId,
                        outcome.Id,
                        normalizedScore,
                        effectiveWeight,
                        answer.UpdatedAtUtc));
            }
        }

        return points.ToArray();
    }

    private static void AddEvidence(
        IDictionary<StudentOutcomeKey, ScoreAccumulator> accumulator,
        StudentOutcomeKey key,
        decimal normalizedScore,
        decimal effectiveWeight)
    {
        if (!accumulator.TryGetValue(key, out var value))
        {
            value = new ScoreAccumulator();
            accumulator[key] = value;
        }

        value.WeightedEarned += normalizedScore * effectiveWeight;
        value.WeightedPossible += effectiveWeight;
        value.EvidenceCount++;
    }

    internal static decimal DifficultyWeight(AssessmentItemDifficulty difficulty) =>
        difficulty switch
        {
            AssessmentItemDifficulty.Easy => 0.85m,
            AssessmentItemDifficulty.Medium => 1.00m,
            AssessmentItemDifficulty.Challenging => 1.15m,
            _ => throw new InvalidOperationException(
                "Unsupported assessment item difficulty.")
        };

    internal static decimal RecencyWeight(
        DateTime occurredAtUtc,
        DateTime calculatedAtUtc)
    {
        if (occurredAtUtc > calculatedAtUtc.AddMinutes(5))
        {
            throw new InvalidOperationException(
                "Learning evidence cannot occur in the future.");
        }

        var age = calculatedAtUtc - occurredAtUtc;
        if (age < TimeSpan.Zero)
            age = TimeSpan.Zero;

        if (age <= TimeSpan.FromDays(14)) return 1.00m;
        if (age <= TimeSpan.FromDays(45)) return 0.90m;
        if (age <= TimeSpan.FromDays(90)) return 0.75m;
        if (age <= TimeSpan.FromDays(180)) return 0.60m;
        return 0.45m;
    }

    internal static decimal ConfidenceFor(int evidenceCount) =>
        Math.Min(100m, Math.Max(0, evidenceCount) * 20m);

    private static decimal Percentage(decimal earned, decimal possible)
    {
        if (possible <= 0m)
            return 0m;

        return Round2(earned / possible * 100m);
    }

    private static decimal Round2(decimal value) =>
        decimal.Round(value, 2, MidpointRounding.AwayFromZero);

    private static decimal Round4(decimal value) =>
        decimal.Round(value, 4, MidpointRounding.AwayFromZero);

    private readonly record struct StudentOutcomeKey(
        Guid SchoolId,
        Guid AcademicYearId,
        Guid ClassGroupId,
        Guid SubjectId,
        Guid StudentProfileId,
        Guid LearningOutcomeId);

    private sealed record FormalEvidencePoint(
        Guid SchoolId,
        Guid AcademicYearId,
        Guid ClassGroupId,
        Guid SubjectId,
        Guid StudentProfileId,
        Guid LearningOutcomeId,
        decimal NormalizedScore,
        decimal EffectiveWeight,
        DateTime OccurredAtUtc);

    private sealed class ScoreAccumulator
    {
        public decimal WeightedEarned { get; set; }
        public decimal WeightedPossible { get; set; }
        public int EvidenceCount { get; set; }
    }
}

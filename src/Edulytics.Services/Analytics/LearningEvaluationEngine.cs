using Edulytics.Core.Analytics;
using Edulytics.Core.Entities;
using Edulytics.Core.Enums;
using Edulytics.Core.Mathematics.Skills;

namespace Edulytics.Services.Analytics;

/// <summary>
/// Deterministic, explainable evaluation layer above the existing mastery
/// projection. It never turns missing evidence into zero mastery and it keeps
/// Assessment and Practice performance separate.
/// </summary>
public sealed class LearningEvaluationEngine
{
    public const string FormulaVersion = "evaluation-v1";

    private readonly EvaluationEvidenceNormalizer _normalizer;

    public LearningEvaluationEngine(
        EvaluationEvidenceNormalizer normalizer)
    {
        _normalizer = normalizer;
    }

    public StudentSubjectEvaluation BuildStudentSubject(
        AnalyticsProjectionSnapshot snapshot,
        Guid studentProfileId,
        Guid academicYearId,
        Guid classGroupId,
        Guid subjectId,
        DateTime calculatedAtUtc)
    {
        ArgumentNullException.ThrowIfNull(snapshot);

        var student = snapshot.StudentProfiles.SingleOrDefault(
            x =>
                x.Id == studentProfileId &&
                !x.IsArchived &&
                x.Status == AcademicStructureStatus.Active)
            ?? throw new InvalidOperationException(
                "Student is missing or inactive.");

        var year = snapshot.AcademicYears.SingleOrDefault(
            x => x.Id == academicYearId)
            ?? throw new InvalidOperationException(
                "Academic year is missing.");

        var classGroup = snapshot.ClassGroups.SingleOrDefault(
            x =>
                x.Id == classGroupId &&
                x.AcademicYearId == academicYearId)
            ?? throw new InvalidOperationException(
                "Class is missing or outside academic-year scope.");

        var subject = snapshot.Subjects.SingleOrDefault(
            x => x.Id == subjectId)
            ?? throw new InvalidOperationException(
                "Subject is missing.");

        if (!snapshot.StudentEnrollments.Any(
                x =>
                    x.StudentProfileId == studentProfileId &&
                    x.AcademicYearId == academicYearId &&
                    x.ClassGroupId == classGroupId))
        {
            throw new InvalidOperationException(
                "Student is not enrolled in the requested class.");
        }

        var evidence = _normalizer.NormalizeOfficial(snapshot)
            .Where(x =>
                x.StudentProfileId == studentProfileId &&
                x.AcademicYearId == academicYearId &&
                x.ClassGroupId == classGroupId &&
                x.SubjectId == subjectId)
            .ToArray();

        var outcomes = snapshot.LearningOutcomes
            .Where(x =>
                x.SubjectId == subjectId &&
                x.AcademicProgramId == classGroup.AcademicProgramId &&
                x.GradeLevelId == classGroup.GradeLevelId &&
                (!x.CurriculumAdoptionId.HasValue ||
                 !classGroup.CurriculumAdoptionId.HasValue ||
                 x.CurriculumAdoptionId.Value ==
                    classGroup.CurriculumAdoptionId.Value))
            .OrderBy(x => x.Order)
            .ThenBy(x => x.Code)
            .ToArray();

        var targetRows =
            new List<(LearningOutcome Outcome, EvaluationSkillDescriptor Skill)>();

        foreach (var outcome in outcomes)
        {
            var expected = EvaluationSkillResolver.ResolveExpected(outcome);
            var observedExact = evidence
                .Where(x =>
                    x.LearningOutcomeId == outcome.Id &&
                    x.SkillResolutionKind !=
                        EvaluationSkillResolutionKind.OutcomeProxy)
                .Select(x =>
                    new EvaluationSkillDescriptor(
                        x.SkillKey,
                        x.SkillName,
                        x.SkillResolutionKind,
                        MathematicsSkillMetadataRegistry.TryResolve(
                            x.SkillKey,
                            out var metadata) &&
                        metadata is not null
                            ? metadata.Prerequisites
                            : []))
                .DistinctBy(x => x.SkillKey, StringComparer.Ordinal)
                .ToArray();

            var effective =
                expected.Count == 1 &&
                expected[0].ResolutionKind ==
                    EvaluationSkillResolutionKind.OutcomeProxy &&
                observedExact.Length > 0
                    ? observedExact
                    : expected;

            foreach (var skill in effective)
                targetRows.Add((outcome, skill));
        }

        foreach (var observed in evidence)
        {
            if (targetRows.Any(
                    x =>
                        x.Outcome.Id == observed.LearningOutcomeId &&
                        string.Equals(
                            x.Skill.SkillKey,
                            observed.SkillKey,
                            StringComparison.Ordinal)))
            {
                continue;
            }

            var outcome = outcomes.FirstOrDefault(
                x => x.Id == observed.LearningOutcomeId);
            if (outcome is null)
                continue;

            var prerequisites =
                MathematicsSkillMetadataRegistry.TryResolve(
                    observed.SkillKey,
                    out var metadata) &&
                metadata is not null
                    ? metadata.Prerequisites
                    : [];

            targetRows.Add(
                (
                    outcome,
                    new EvaluationSkillDescriptor(
                        observed.SkillKey,
                        observed.SkillName,
                        observed.SkillResolutionKind,
                        prerequisites)
                ));
        }

        targetRows = targetRows
            .DistinctBy(
                x => (x.Outcome.Id, x.Skill.SkillKey))
            .ToList();

        var preliminary = targetRows
            .Select(target =>
                BuildPreliminarySkill(
                    target.Outcome,
                    target.Skill,
                    evidence
                        .Where(x =>
                            x.LearningOutcomeId ==
                                target.Outcome.Id &&
                            string.Equals(
                                x.SkillKey,
                                target.Skill.SkillKey,
                                StringComparison.Ordinal))
                        .ToArray(),
                    calculatedAtUtc))
            .ToArray();

        var masteryBySkillKey = preliminary
            .Where(x => x.CurrentMasteryPercentage.HasValue)
            .GroupBy(x => x.SkillKey, StringComparer.Ordinal)
            .ToDictionary(
                x => x.Key,
                x => x
                    .Where(row =>
                        row.CurrentMasteryPercentage.HasValue)
                    .Average(row =>
                        row.CurrentMasteryPercentage!.Value),
                StringComparer.Ordinal);

        var skills = preliminary
            .Select(x =>
            {
                var weakPrerequisites = x.PrerequisiteSkillKeys
                    .Where(key =>
                        masteryBySkillKey.TryGetValue(
                            key,
                            out var mastery) &&
                        mastery < 60m)
                    .Distinct(StringComparer.Ordinal)
                    .ToArray();

                var criticality = CriticalityFor(
                    x,
                    weakPrerequisites);
                var priority = PriorityFor(
                    x.Status,
                    criticality);

                return x with
                {
                    WeakPrerequisiteSkillKeys =
                        weakPrerequisites,
                    GapCriticalityScore =
                        criticality,
                    InterventionPriority =
                        priority
                };
            })
            .OrderByDescending(x => x.InterventionPriority)
            .ThenBy(x => x.CurrentMasteryPercentage ?? 101m)
            .ThenBy(x => x.SkillName, StringComparer.Ordinal)
            .ToArray();

        var evaluatedSkills = skills
            .Where(x =>
                x.Status != EvaluationSkillStatus.NotYetAssessed)
            .ToArray();
        var expectedCount = skills.Length;
        var coverage = expectedCount == 0
            ? 0m
            : Round2(
                evaluatedSkills.Length /
                (decimal)expectedCount *
                100m);

        var confidence = evaluatedSkills.Length == 0
            ? 0m
            : Round2(
                evaluatedSkills.Average(
                    x => x.ConfidencePercentage));

        var allEvidence = evidence.ToArray();
        var assessmentMastery = WeightedMastery(
            allEvidence.Where(
                x =>
                    x.Source ==
                    EvaluationEvidenceSource.Assessment),
            calculatedAtUtc);
        var practiceMastery = WeightedMastery(
            allEvidence.Where(
                x =>
                    x.Source ==
                    EvaluationEvidenceSource.Practice),
            calculatedAtUtc);
        var currentMastery = WeightedMastery(
            allEvidence,
            calculatedAtUtc);

        return new StudentSubjectEvaluation(
            student.SchoolId,
            student.Id,
            student.StudentNumber,
            student.DisplayName,
            year.Id,
            year.Name,
            classGroup.Id,
            classGroup.Name,
            subject.Id,
            subject.Name,
            currentMastery,
            assessmentMastery,
            practiceMastery,
            TransferGap(
                assessmentMastery,
                practiceMastery),
            coverage,
            expectedCount,
            evaluatedSkills.Length,
            confidence,
            ConfidenceBandFor(confidence),
            TrendFor(
                allEvidence,
                shortTerm: true),
            TrendFor(
                allEvidence,
                shortTerm: false),
            skills.Count(
                x =>
                    x.Status is
                        EvaluationSkillStatus.Secure or
                        EvaluationSkillStatus.Strong),
            skills.Count(
                x =>
                    x.Status ==
                    EvaluationSkillStatus.Developing),
            skills.Count(
                x =>
                    x.Status ==
                    EvaluationSkillStatus.NeedsFocus),
            skills.Count(
                x =>
                    x.Status ==
                    EvaluationSkillStatus.Critical),
            skills.Count(
                x =>
                    x.Retention is
                        EvaluationRetentionBand.Concern or
                        EvaluationRetentionBand.SignificantConcern),
            skills,
            FormulaVersion);
    }

    private static StudentSkillEvaluation BuildPreliminarySkill(
        LearningOutcome outcome,
        EvaluationSkillDescriptor skill,
        IReadOnlyList<EvaluationEvidenceRecord> evidence,
        DateTime calculatedAtUtc)
    {
        var current = WeightedMastery(
            evidence,
            calculatedAtUtc);
        var assessment = WeightedMastery(
            evidence.Where(
                x =>
                    x.Source ==
                    EvaluationEvidenceSource.Assessment),
            calculatedAtUtc);
        var practice = WeightedMastery(
            evidence.Where(
                x =>
                    x.Source ==
                    EvaluationEvidenceSource.Practice),
            calculatedAtUtc);
        var confidence = ConfidenceFor(
            evidence,
            calculatedAtUtc);
        var status = StatusFor(
            current,
            evidence.Count);

        return new StudentSkillEvaluation(
            evidence.FirstOrDefault()?.AcademicYearId ??
                Guid.Empty,
            evidence.FirstOrDefault()?.ClassGroupId ??
                Guid.Empty,
            outcome.SubjectId,
            outcome.TopicId,
            outcome.Id,
            outcome.Code,
            outcome.Description,
            skill.SkillKey,
            skill.SkillName,
            skill.ResolutionKind,
            current,
            assessment,
            practice,
            TransferGap(
                assessment,
                practice),
            confidence,
            ConfidenceBandFor(confidence),
            TrendFor(
                evidence,
                shortTerm: true),
            TrendFor(
                evidence,
                shortTerm: false),
            RetentionFor(evidence),
            status,
            0m,
            EvaluationPriority.None,
            skill.PrerequisiteSkillKeys,
            [],
            Summary(evidence),
            FormulaVersion);
    }

    private static EvaluationEvidenceSummary Summary(
        IReadOnlyList<EvaluationEvidenceRecord> evidence)
    {
        var latest = evidence
            .Select(x => (DateTime?)x.OccurredAtUtc)
            .DefaultIfEmpty()
            .Max();

        return new EvaluationEvidenceSummary(
            evidence.Count,
            evidence.Count(
                x =>
                    x.Source ==
                    EvaluationEvidenceSource.Assessment),
            evidence.Count(
                x =>
                    x.Source ==
                    EvaluationEvidenceSource.Practice),
            evidence
                .Where(
                    x =>
                        x.Source ==
                        EvaluationEvidenceSource.Assessment)
                .Select(x => x.SourceId)
                .Distinct()
                .Count(),
            evidence
                .Where(
                    x =>
                        x.Source ==
                        EvaluationEvidenceSource.Practice)
                .Select(x => x.SourceId)
                .Distinct()
                .Count(),
            evidence
                .Where(x => x.Difficulty.HasValue)
                .Select(x => x.Difficulty!.Value)
                .Distinct()
                .Count(),
            latest);
    }

    private static decimal? WeightedMastery(
        IEnumerable<EvaluationEvidenceRecord> source,
        DateTime calculatedAtUtc)
    {
        var rows = source.ToArray();
        if (rows.Length == 0)
            return null;

        decimal weightedScore = 0m;
        decimal totalWeight = 0m;

        foreach (var row in rows)
        {
            if (row.MaxScore <= 0m)
                continue;

            var difficultyWeight =
                row.Difficulty.HasValue
                    ? MasteryEvidenceEngine.DifficultyWeight(
                        row.Difficulty.Value)
                    : 1m;
            var weight =
                row.MappingWeight *
                difficultyWeight *
                MasteryEvidenceEngine.RecencyWeight(
                    row.OccurredAtUtc,
                    calculatedAtUtc);

            if (weight <= 0m)
                continue;

            weightedScore +=
                row.Percentage * weight;
            totalWeight += weight;
        }

        return totalWeight <= 0m
            ? null
            : Round2(
                weightedScore /
                totalWeight);
    }

    private static decimal ConfidenceFor(
        IReadOnlyList<EvaluationEvidenceRecord> evidence,
        DateTime calculatedAtUtc)
    {
        if (evidence.Count == 0)
            return 0m;

        var countScore =
            Math.Min(40m, evidence.Count * 8m);
        var independentSources = evidence
            .Select(x =>
                (
                    x.Source,
                    x.SourceId
                ))
            .Distinct()
            .Count();
        var independenceScore =
            Math.Min(20m, independentSources * 5m);
        var hasAssessment = evidence.Any(
            x =>
                x.Source ==
                EvaluationEvidenceSource.Assessment);
        var hasPractice = evidence.Any(
            x =>
                x.Source ==
                EvaluationEvidenceSource.Practice);
        var sourceDiversityScore =
            hasAssessment && hasPractice
                ? 15m
                : hasAssessment
                    ? 8m
                    : 6m;
        var difficultyScore =
            Math.Min(
                10m,
                evidence
                    .Where(x => x.Difficulty.HasValue)
                    .Select(x => x.Difficulty!.Value)
                    .Distinct()
                    .Count() *
                3m);
        var latest = evidence.Max(
            x => x.OccurredAtUtc);
        var age = calculatedAtUtc - latest;
        var recencyScore =
            age <= TimeSpan.FromDays(30)
                ? 15m
                : age <= TimeSpan.FromDays(90)
                    ? 8m
                    : 3m;

        return Round2(
            Math.Min(
                100m,
                countScore +
                independenceScore +
                sourceDiversityScore +
                difficultyScore +
                recencyScore));
    }

    private static EvaluationConfidenceBand ConfidenceBandFor(
        decimal confidence) =>
        confidence switch
        {
            < 20m => EvaluationConfidenceBand.Insufficient,
            < 40m => EvaluationConfidenceBand.Limited,
            < 65m => EvaluationConfidenceBand.Moderate,
            < 85m => EvaluationConfidenceBand.Strong,
            _ => EvaluationConfidenceBand.VeryStrong
        };

    private static EvaluationSkillStatus StatusFor(
        decimal? mastery,
        int evidenceCount)
    {
        if (!mastery.HasValue)
            return EvaluationSkillStatus.NotYetAssessed;

        if (evidenceCount < 2)
            return EvaluationSkillStatus.InsufficientEvidence;

        return mastery.Value switch
        {
            < 40m => EvaluationSkillStatus.Critical,
            < 60m => EvaluationSkillStatus.NeedsFocus,
            < 75m => EvaluationSkillStatus.Developing,
            < 90m => EvaluationSkillStatus.Secure,
            _ => EvaluationSkillStatus.Strong
        };
    }

    private static EvaluationTrendBand TrendFor(
        IEnumerable<EvaluationEvidenceRecord> source,
        bool shortTerm)
    {
        var rows = source
            .OrderBy(x => x.OccurredAtUtc)
            .ThenBy(x => x.EvidenceKey, StringComparer.Ordinal)
            .ToArray();

        if (rows.Length < 4)
            return EvaluationTrendBand.InsufficientEvidence;

        decimal before;
        decimal after;

        if (shortTerm)
        {
            before = rows
                .Skip(Math.Max(0, rows.Length - 4))
                .Take(2)
                .Average(x => x.Percentage);
            after = rows
                .TakeLast(2)
                .Average(x => x.Percentage);
        }
        else
        {
            var window =
                Math.Max(2, rows.Length / 3);
            before = rows
                .Take(window)
                .Average(x => x.Percentage);
            after = rows
                .TakeLast(window)
                .Average(x => x.Percentage);
        }

        return TrendBandFor(
            after - before);
    }

    private static EvaluationTrendBand TrendBandFor(
        decimal delta) =>
        delta switch
        {
            <= -10m => EvaluationTrendBand.RapidlyDeclining,
            <= -4m => EvaluationTrendBand.Declining,
            >= 10m => EvaluationTrendBand.RapidlyImproving,
            >= 4m => EvaluationTrendBand.Improving,
            _ => EvaluationTrendBand.Stable
        };

    private static EvaluationRetentionBand RetentionFor(
        IReadOnlyList<EvaluationEvidenceRecord> evidence)
    {
        var rows = evidence
            .OrderBy(x => x.OccurredAtUtc)
            .ThenBy(x => x.EvidenceKey, StringComparer.Ordinal)
            .ToArray();

        if (rows.Length < 4 ||
            rows[^1].OccurredAtUtc -
                rows[0].OccurredAtUtc <
            TimeSpan.FromDays(30))
        {
            return EvaluationRetentionBand.InsufficientEvidence;
        }

        var earlierCount =
            Math.Max(2, rows.Length / 2);
        var earlier = rows
            .Take(earlierCount)
            .Average(x => x.Percentage);
        var recent = rows
            .TakeLast(2)
            .Average(x => x.Percentage);

        if (earlier < 75m)
            return EvaluationRetentionBand.Stable;

        var drop = earlier - recent;
        if (drop >= 25m)
            return EvaluationRetentionBand.SignificantConcern;
        if (drop >= 15m)
            return EvaluationRetentionBand.Concern;

        return EvaluationRetentionBand.Stable;
    }

    private static decimal CriticalityFor(
        StudentSkillEvaluation evaluation,
        IReadOnlyList<string> weakPrerequisites)
    {
        if (!evaluation.CurrentMasteryPercentage.HasValue ||
            evaluation.Status is
                EvaluationSkillStatus.NotYetAssessed or
                EvaluationSkillStatus.InsufficientEvidence)
        {
            return 0m;
        }

        var severity =
            Math.Max(
                0m,
                100m -
                evaluation.CurrentMasteryPercentage.Value);
        var confidenceFactor =
            0.5m +
            evaluation.ConfidencePercentage /
            200m;
        var score =
            severity *
            0.55m *
            confidenceFactor;

        score += evaluation.ShortTermTrend switch
        {
            EvaluationTrendBand.RapidlyDeclining => 20m,
            EvaluationTrendBand.Declining => 12m,
            _ => 0m
        };

        score += evaluation.Retention switch
        {
            EvaluationRetentionBand.SignificantConcern => 15m,
            EvaluationRetentionBand.Concern => 10m,
            _ => 0m
        };

        score += Math.Min(
            15m,
            MathematicsSkillMetadataRegistry.DependentCount(
                evaluation.SkillKey) *
            3m);

        score += Math.Min(
            15m,
            weakPrerequisites.Count *
            5m);

        return Round2(
            Math.Min(
                100m,
                Math.Max(
                    0m,
                    score)));
    }

    private static EvaluationPriority PriorityFor(
        EvaluationSkillStatus status,
        decimal criticality)
    {
        if (status is
            EvaluationSkillStatus.NotYetAssessed or
            EvaluationSkillStatus.InsufficientEvidence)
        {
            return EvaluationPriority.None;
        }

        return criticality switch
        {
            >= 75m => EvaluationPriority.Critical,
            >= 55m => EvaluationPriority.High,
            >= 35m => EvaluationPriority.Medium,
            > 0m => EvaluationPriority.Low,
            _ => EvaluationPriority.None
        };
    }

    private static decimal? TransferGap(
        decimal? assessment,
        decimal? practice) =>
        assessment.HasValue &&
        practice.HasValue
            ? Round2(
                assessment.Value -
                practice.Value)
            : null;

    private static decimal Round2(decimal value) =>
        decimal.Round(
            value,
            2,
            MidpointRounding.AwayFromZero);
}

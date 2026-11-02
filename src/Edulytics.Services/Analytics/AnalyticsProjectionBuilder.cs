using Edulytics.Core.Analytics;
using Edulytics.Core.Entities;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Analytics;

public sealed class AnalyticsProjectionBuilder
{
    public AnalyticsProjectionSet Build(
        AnalyticsSourceSnapshot source,
        DateTime calculatedAtUtc)
    {
        var assessments = source.Assessments
            .Where(x => x.Status != AssessmentStatus.Draft)
            .ToDictionary(x => x.Id);
        var outcomes = source.LearningOutcomes.ToDictionary(x => x.Id);
        var topics = source.CurriculumTopics.ToDictionary(x => x.Id);
        var results = source.AssessmentResults
            .Where(x => assessments.ContainsKey(x.AssessmentId))
            .ToDictionary(x => x.Id);

        // Phase 31: mastery is derived only from LearningEvidence. Formal
        // assessment results remain available exclusively for assessment trends.
        var studentMasteries = MasteryEvidenceEngine.BuildMasteries(
            source,
            calculatedAtUtc);

        var classOutcomes = studentMasteries
            .GroupBy(x => new
            {
                x.SchoolId,
                x.AcademicYearId,
                x.ClassGroupId,
                x.SubjectId,
                x.LearningOutcomeId
            })
            .Select(group =>
            {
                var earned = group.Sum(x => x.EarnedScore);
                var possible = group.Sum(x => x.PossibleScore);
                var percentage = Percentage(earned, possible);

                return new ClassOutcomeSummary
                {
                    Id = Guid.NewGuid(),
                    SchoolId = group.Key.SchoolId,
                    AcademicYearId = group.Key.AcademicYearId,
                    ClassGroupId = group.Key.ClassGroupId,
                    SubjectId = group.Key.SubjectId,
                    LearningOutcomeId = group.Key.LearningOutcomeId,
                    EarnedScore = Round4(earned),
                    PossibleScore = Round4(possible),
                    AverageMasteryPercentage = percentage,
                    StudentCount = group.Select(x => x.StudentProfileId).Distinct().Count(),
                    AtRiskStudentCount = group.Count(x => x.MasteryPercentage < 60m),
                    EvidenceCount = group.Sum(x => x.EvidenceCount),
                    CalculatedAtUtc = calculatedAtUtc
                };
            })
            .OrderBy(x => x.AcademicYearId)
            .ThenBy(x => x.ClassGroupId)
            .ThenBy(x => x.SubjectId)
            .ThenBy(x => x.LearningOutcomeId)
            .ToArray();

        var topicInputs = classOutcomes
            .Select(x =>
            {
                if (!outcomes.TryGetValue(x.LearningOutcomeId, out var outcome))
                {
                    throw new InvalidOperationException(
                        "Class outcome references a missing LearningOutcome.");
                }

                if (!topics.ContainsKey(outcome.TopicId))
                {
                    throw new InvalidOperationException(
                        "LearningOutcome references a missing CurriculumTopic.");
                }

                return new { Summary = x, Outcome = outcome };
            })
            .ToArray();

        var classTopics = topicInputs
            .GroupBy(x => new
            {
                x.Summary.SchoolId,
                x.Summary.AcademicYearId,
                x.Summary.ClassGroupId,
                x.Summary.SubjectId,
                TopicId = x.Outcome.TopicId
            })
            .Select(group =>
            {
                var weighted = group
                    .Select(x => new
                    {
                        x.Summary.AverageMasteryPercentage,
                        Weight = x.Outcome.Weight > 0m ? x.Outcome.Weight : 1m
                    })
                    .ToArray();
                var denominator = weighted.Sum(x => x.Weight);
                var mastery = denominator <= 0m
                    ? 0m
                    : Round2(weighted.Sum(x => x.AverageMasteryPercentage * x.Weight) / denominator);
                var topicOutcomeIds = group.Select(x => x.Outcome.Id).ToHashSet();
                var studentCount = studentMasteries
                    .Where(x =>
                        x.AcademicYearId == group.Key.AcademicYearId &&
                        x.ClassGroupId == group.Key.ClassGroupId &&
                        x.SubjectId == group.Key.SubjectId &&
                        topicOutcomeIds.Contains(x.LearningOutcomeId))
                    .Select(x => x.StudentProfileId)
                    .Distinct()
                    .Count();

                return new ClassTopicSummary
                {
                    Id = Guid.NewGuid(),
                    SchoolId = group.Key.SchoolId,
                    AcademicYearId = group.Key.AcademicYearId,
                    ClassGroupId = group.Key.ClassGroupId,
                    SubjectId = group.Key.SubjectId,
                    CurriculumTopicId = group.Key.TopicId,
                    MasteryPercentage = mastery,
                    OutcomeCount = group.Select(x => x.Outcome.Id).Distinct().Count(),
                    WeakOutcomeCount = group.Count(x => x.Summary.AverageMasteryPercentage < 60m),
                    StudentCount = studentCount,
                    CalculatedAtUtc = calculatedAtUtc
                };
            })
            .OrderBy(x => x.AcademicYearId)
            .ThenBy(x => x.ClassGroupId)
            .ThenBy(x => x.SubjectId)
            .ThenBy(x => x.CurriculumTopicId)
            .ToArray();

        foreach (var result in results.Values)
        {
            if (result.Percentage < 0m || result.Percentage > 100m)
            {
                throw new InvalidOperationException(
                    "Analytics source contains an invalid result percentage.");
            }
        }

        var trends = results.Values
            .GroupBy(x => x.AssessmentId)
            .Select(group =>
            {
                var assessment = assessments[group.Key];
                return new ClassAssessmentTrend
                {
                    Id = Guid.NewGuid(),
                    SchoolId = assessment.SchoolId,
                    AcademicYearId = assessment.AcademicYearId,
                    ClassGroupId = assessment.ClassGroupId,
                    SubjectId = assessment.SubjectId,
                    AssessmentId = assessment.Id,
                    AssessmentTitle = assessment.Title,
                    AssessmentDate = assessment.AssessmentDate,
                    AveragePercentage = Round2(group.Average(x => x.Percentage)),
                    StudentCount = group.Select(x => x.StudentProfileId).Distinct().Count(),
                    AtRiskStudentCount = group.Count(x => x.Percentage < 60m),
                    CalculatedAtUtc = calculatedAtUtc
                };
            })
            .OrderBy(x => x.AssessmentDate)
            .ThenBy(x => x.AssessmentTitle)
            .ToArray();

        var schoolSnapshots = studentMasteries
            .GroupBy(x => new { x.SchoolId, x.AcademicYearId })
            .Select(group =>
            {
                var earned = group.Sum(x => x.EarnedScore);
                var possible = group.Sum(x => x.PossibleScore);
                var riskStudents = group
                    .GroupBy(x => x.StudentProfileId)
                    .Count(student => Percentage(
                        student.Sum(x => x.EarnedScore),
                        student.Sum(x => x.PossibleScore)) < 60m);
                var criticalOutcomes = classOutcomes
                    .Where(x => x.SchoolId == group.Key.SchoolId && x.AcademicYearId == group.Key.AcademicYearId)
                    .GroupBy(x => x.LearningOutcomeId)
                    .Count(outcome => Percentage(
                        outcome.Sum(x => x.EarnedScore),
                        outcome.Sum(x => x.PossibleScore)) < 40m);
                var weakTopics = classTopics
                    .Where(x => x.SchoolId == group.Key.SchoolId && x.AcademicYearId == group.Key.AcademicYearId)
                    .GroupBy(x => x.CurriculumTopicId)
                    .Count(topic =>
                    {
                        var rows = topic.ToArray();
                        var denominator = rows.Sum(x => Math.Max(x.StudentCount, 1));
                        if (denominator <= 0) return false;
                        var average = Round2(rows.Sum(x => x.MasteryPercentage * Math.Max(x.StudentCount, 1)) / denominator);
                        return average < 60m;
                    });

                return new SchoolAnalyticsSnapshot
                {
                    Id = Guid.NewGuid(),
                    SchoolId = group.Key.SchoolId,
                    AcademicYearId = group.Key.AcademicYearId,
                    OverallMasteryPercentage = Percentage(earned, possible),
                    StudentsWithEvidence = group.Select(x => x.StudentProfileId).Distinct().Count(),
                    AtRiskStudents = riskStudents,
                    CriticalOutcomeCount = criticalOutcomes,
                    WeakTopicCount = weakTopics,
                    LatestSourceUpdatedAtUtc = LatestSourceForYear(source, assessments, group.Key.AcademicYearId),
                    CalculatedAtUtc = calculatedAtUtc
                };
            })
            .OrderBy(x => x.AcademicYearId)
            .ToArray();

        return new AnalyticsProjectionSet(
            studentMasteries,
            classOutcomes,
            classTopics,
            trends,
            schoolSnapshots);
    }

    public StudentLearningProfile BuildStudentLearningProfile(
        AnalyticsSourceSnapshot source,
        Guid studentProfileId,
        Guid curriculumAdoptionId,
        DateTime calculatedAtUtc) =>
        MasteryEvidenceEngine.BuildProfile(
            source,
            studentProfileId,
            curriculumAdoptionId,
            calculatedAtUtc);

    public static MasteryBand BandFor(decimal percentage) =>
        percentage switch
        {
            < 40m => MasteryBand.CriticalGap,
            < 60m => MasteryBand.Weak,
            < 75m => MasteryBand.Developing,
            < 90m => MasteryBand.Secure,
            _ => MasteryBand.Strong
        };

    private static DateTime? LatestSourceForYear(
        AnalyticsSourceSnapshot source,
        IReadOnlyDictionary<Guid, Assessment> assessments,
        Guid academicYearId)
    {
        var assessmentIds = assessments.Values
            .Where(x => x.AcademicYearId == academicYearId)
            .Select(x => x.Id)
            .ToHashSet();
        var results = source.AssessmentResults
            .Where(x => assessmentIds.Contains(x.AssessmentId))
            .ToArray();
        var resultIds = results.Select(x => x.Id).ToHashSet();
        var candidates = new List<DateTime>();

        candidates.AddRange(results.Select(x => x.UpdatedAtUtc));
        candidates.AddRange(source.StudentAnswers
            .Where(x => resultIds.Contains(x.AssessmentResultId))
            .Select(x => x.UpdatedAtUtc));

        var classes = source.ClassGroups
            .Where(x => x.AcademicYearId == academicYearId && x.CurriculumAdoptionId.HasValue)
            .ToDictionary(x => x.Id);
        var studentAdoptions = source.StudentEnrollments
            .Where(x => x.AcademicYearId == academicYearId && classes.ContainsKey(x.ClassGroupId))
            .Select(x => (x.StudentProfileId, AdoptionId: classes[x.ClassGroupId].CurriculumAdoptionId!.Value))
            .ToHashSet();
        var attemptIds = (source.PracticeAttempts ?? [])
            .Where(x => studentAdoptions.Contains((x.StudentProfileId, x.CurriculumAdoptionId)))
            .Select(x => x.Id)
            .ToHashSet();
        candidates.AddRange((source.LearningEvidence ?? [])
            .Where(x => attemptIds.Contains(x.PracticeAttemptId))
            .Select(x => x.OccurredAtUtc));

        return candidates.Count == 0 ? null : candidates.Max();
    }

    private static decimal Percentage(decimal earned, decimal possible)
    {
        if (possible <= 0m) return 0m;
        return Round2(earned / possible * 100m);
    }

    private static decimal Round2(decimal value) =>
        decimal.Round(value, 2, MidpointRounding.AwayFromZero);

    private static decimal Round4(decimal value) =>
        decimal.Round(value, 4, MidpointRounding.AwayFromZero);
}

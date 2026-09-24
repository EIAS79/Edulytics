using Edulytics.Core.Analytics;
using Edulytics.Core.Entities;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Analytics;

/// <summary>
/// Converts heterogeneous official evidence into one auditable stream.
/// Private student Practice is intentionally absent because
/// AnalyticsProjectionSnapshot preserves the staff-facing privacy boundary.
/// </summary>
public sealed class EvaluationEvidenceNormalizer
{
    public IReadOnlyList<EvaluationEvidenceRecord> NormalizeOfficial(
        AnalyticsProjectionSnapshot snapshot)
    {
        ArgumentNullException.ThrowIfNull(snapshot);

        var result = new List<EvaluationEvidenceRecord>();
        AddAssessmentEvidence(snapshot, result);
        AddPracticeEvidence(snapshot, result);

        return result
            .OrderBy(x => x.OccurredAtUtc)
            .ThenBy(x => x.EvidenceKey, StringComparer.Ordinal)
            .ToArray();
    }

    private static void AddAssessmentEvidence(
        AnalyticsProjectionSnapshot snapshot,
        ICollection<EvaluationEvidenceRecord> result)
    {
        var assessments = snapshot.Assessments
            .Where(x => x.Status != AssessmentStatus.Draft)
            .ToDictionary(x => x.Id);
        var results = snapshot.AssessmentResults
            .Where(x => assessments.ContainsKey(x.AssessmentId))
            .ToDictionary(x => x.Id);
        var questions = snapshot.AssessmentQuestions
            .Where(x => assessments.ContainsKey(x.AssessmentId))
            .ToDictionary(x => x.Id);
        var mappings = snapshot.OutcomeMappings
            .GroupBy(x => x.AssessmentQuestionId)
            .ToDictionary(x => x.Key, x => x.ToArray());
        var outcomes = snapshot.LearningOutcomes.ToDictionary(x => x.Id);
        var classes = snapshot.ClassGroups.ToDictionary(x => x.Id);
        var items = snapshot.AssessmentItems
            .GroupBy(x => x.Id)
            .ToDictionary(x => x.Key, x => x.First());
        var lessons = snapshot.PedagogicalLessons.ToDictionary(x => x.Id);
        var enrollments = snapshot.StudentEnrollments
            .Select(x =>
                (
                    x.StudentProfileId,
                    x.AcademicYearId,
                    x.ClassGroupId
                ))
            .ToHashSet();

        foreach (var answer in snapshot.StudentAnswers)
        {
            if (!results.TryGetValue(
                    answer.AssessmentResultId,
                    out var assessmentResult) ||
                !assessments.TryGetValue(
                    assessmentResult.AssessmentId,
                    out var assessment) ||
                !questions.TryGetValue(
                    answer.AssessmentQuestionId,
                    out var question) ||
                question.AssessmentId != assessment.Id ||
                question.MaxScore <= 0m ||
                answer.Score < 0m ||
                answer.Score > question.MaxScore ||
                !classes.TryGetValue(
                    assessment.ClassGroupId,
                    out var classGroup) ||
                !enrollments.Contains(
                    (
                        assessmentResult.StudentProfileId,
                        assessment.AcademicYearId,
                        assessment.ClassGroupId
                    )) ||
                !mappings.TryGetValue(
                    question.Id,
                    out var questionMappings) ||
                questionMappings.Length == 0)
            {
                continue;
            }

            items.TryGetValue(question.Id, out var item);

            CurriculumPedagogicalLesson? lesson = null;
            if (item?.CurriculumPedagogicalLessonId is Guid lessonId)
                lessons.TryGetValue(lessonId, out lesson);

            var outcomeMappingWeight =
                1m / questionMappings.Length;

            foreach (var mapping in questionMappings)
            {
                if (!outcomes.TryGetValue(
                        mapping.LearningOutcomeId,
                        out var outcome) ||
                    outcome.SubjectId != assessment.SubjectId ||
                    outcome.AcademicProgramId != classGroup.AcademicProgramId ||
                    outcome.GradeLevelId != classGroup.GradeLevelId)
                {
                    continue;
                }

                var skills = EvaluationSkillResolver.ResolveForEvidence(
                    outcome,
                    item,
                    lesson);
                if (skills.Count == 0)
                    continue;

                var skillWeight =
                    outcomeMappingWeight / skills.Count;

                foreach (var skill in skills)
                {
                    result.Add(
                        new EvaluationEvidenceRecord(
                            $"{answer.Id:D}:{outcome.Id:D}:{skill.SkillKey}",
                            assessment.SchoolId,
                            assessmentResult.StudentProfileId,
                            assessment.AcademicYearId,
                            assessment.ClassGroupId,
                            assessment.SubjectId,
                            assessment.TermId == Guid.Empty
                                ? null
                                : assessment.TermId,
                            outcome.TopicId,
                            outcome.Id,
                            outcome.Code,
                            outcome.Description,
                            skill.SkillKey,
                            skill.SkillName,
                            skill.ResolutionKind,
                            item?.CurriculumPedagogicalLessonId,
                            EvaluationEvidenceSource.Assessment,
                            assessment.Id,
                            assessmentResult.Id,
                            question.Id,
                            assessment.Title,
                            item?.GenerationFamily,
                            item?.Difficulty,
                            answer.Score,
                            question.MaxScore,
                            skillWeight,
                            answer.Score >= question.MaxScore,
                            answer.UpdatedAtUtc));
                }
            }
        }
    }

    private static void AddPracticeEvidence(
        AnalyticsProjectionSnapshot snapshot,
        ICollection<EvaluationEvidenceRecord> result)
    {
        var attempts = snapshot.PracticeAttempts
            .Where(x => !x.IsPrivate)
            .ToDictionary(x => x.Id);
        var outcomes = snapshot.LearningOutcomes.ToDictionary(x => x.Id);
        var items = snapshot.AssessmentItems
            .GroupBy(x => x.Id)
            .ToDictionary(x => x.Key, x => x.First());
        var classes = snapshot.ClassGroups.ToDictionary(x => x.Id);
        var years = snapshot.AcademicYears.ToDictionary(x => x.Id);
        var lessons = snapshot.PedagogicalLessons.ToDictionary(x => x.Id);

        foreach (var evidence in snapshot.LearningEvidence)
        {
            if (!attempts.TryGetValue(
                    evidence.PracticeAttemptId,
                    out var attempt) ||
                attempt.IsPrivate ||
                attempt.StudentProfileId != evidence.StudentProfileId ||
                !outcomes.TryGetValue(
                    evidence.LearningOutcomeId,
                    out var outcome) ||
                evidence.MaxScore <= 0m ||
                evidence.Score < 0m ||
                evidence.Score > evidence.MaxScore)
            {
                continue;
            }

            var enrolledClasses = snapshot.StudentEnrollments
                .Where(x =>
                    x.StudentProfileId == evidence.StudentProfileId &&
                    classes.TryGetValue(x.ClassGroupId, out var candidate) &&
                    candidate.CurriculumAdoptionId ==
                        attempt.CurriculumAdoptionId &&
                    years.TryGetValue(
                        x.AcademicYearId,
                        out var year) &&
                    DateOnly.FromDateTime(evidence.OccurredAtUtc) >=
                        year.StartsOn &&
                    DateOnly.FromDateTime(evidence.OccurredAtUtc) <=
                        year.EndsOn)
                .Select(x => classes[x.ClassGroupId])
                .DistinctBy(x => x.Id)
                .ToArray();

            if (enrolledClasses.Length != 1)
                continue;

            var classGroup = enrolledClasses[0];

            items.TryGetValue(
                evidence.AssessmentItemId,
                out var item);

            CurriculumPedagogicalLesson? lesson = null;
            if (item?.CurriculumPedagogicalLessonId is Guid lessonId)
                lessons.TryGetValue(lessonId, out lesson);

            var skills = EvaluationSkillResolver.ResolveForEvidence(
                outcome,
                item,
                lesson);
            if (skills.Count == 0)
                continue;

            var termId = ResolveTermId(
                snapshot.Terms,
                classGroup.AcademicYearId,
                evidence.OccurredAtUtc);
            var skillWeight = 1m / skills.Count;

            foreach (var skill in skills)
            {
                result.Add(
                    new EvaluationEvidenceRecord(
                        $"{evidence.Id:D}:{outcome.Id:D}:{skill.SkillKey}",
                        evidence.SchoolId,
                        evidence.StudentProfileId,
                        classGroup.AcademicYearId,
                        classGroup.Id,
                        outcome.SubjectId,
                        termId,
                        outcome.TopicId,
                        outcome.Id,
                        outcome.Code,
                        outcome.Description,
                        skill.SkillKey,
                        skill.SkillName,
                        skill.ResolutionKind,
                        item?.CurriculumPedagogicalLessonId,
                        EvaluationEvidenceSource.Practice,
                        attempt.Id,
                        attempt.Id,
                        evidence.AssessmentItemId,
                        lesson?.Title ?? "Practice",
                        item?.GenerationFamily,
                        evidence.Difficulty,
                        evidence.Score,
                        evidence.MaxScore,
                        skillWeight,
                        evidence.IsCorrect,
                        evidence.OccurredAtUtc));
            }
        }
    }

    private static Guid? ResolveTermId(
        IReadOnlyList<Term> terms,
        Guid academicYearId,
        DateTime occurredAtUtc)
    {
        var date = DateOnly.FromDateTime(occurredAtUtc);

        return terms
            .Where(x =>
                x.AcademicYearId == academicYearId &&
                date >= x.StartsOn &&
                date <= x.EndsOn)
            .OrderBy(x => x.StartsOn)
            .Select(x => (Guid?)x.Id)
            .FirstOrDefault();
    }
}

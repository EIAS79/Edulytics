using Edulytics.Core.Analytics;
using Edulytics.Core.Constants;
using Edulytics.Core.Entities;
using Edulytics.Core.Enums;
using Edulytics.Core.Interfaces;
using Edulytics.Core.Practice;

namespace Edulytics.Services.Analytics;

/// <summary>
/// Student-only evaluation facade. Official evaluation remains based on the
/// staff-safe evidence stream; private Practice is loaded separately for the
/// owning student and never enters staff analytics or persisted official
/// mastery.
/// </summary>
public sealed class StudentSelfEvaluationService : IStudentSelfEvaluationService
{
    private readonly IAnalyticsRepository _analytics;
    private readonly ISchoolRepository _schools;
    private readonly ISchoolUserRepository _users;
    private readonly IPracticeRepository _practice;
    private readonly IStudentPrivatePracticeRepository _privatePractice;
    private readonly LearningEvaluationEngine _evaluation;

    public StudentSelfEvaluationService(
        IAnalyticsRepository analytics,
        ISchoolRepository schools,
        ISchoolUserRepository users,
        IPracticeRepository practice,
        IStudentPrivatePracticeRepository privatePractice,
        LearningEvaluationEngine evaluation)
    {
        _analytics = analytics;
        _schools = schools;
        _users = users;
        _practice = practice;
        _privatePractice = privatePractice;
        _evaluation = evaluation;
    }

    public async Task<StudentSelfEvaluationResult<StudentSelfEvaluationPage>> GetAsync(
        Guid actorUserId,
        Guid academicYearId,
        Guid classGroupId,
        Guid subjectId,
        CancellationToken cancellationToken = default)
    {
        var actor = await _users.GetActorAsync(
            actorUserId,
            cancellationToken);

        if (actor is null ||
            !actor.IsActive ||
            actor.IsLocked ||
            !actor.SchoolId.HasValue ||
            actor.Roles.Count != 1 ||
            actor.Roles[0] != RoleNames.Student)
        {
            return StudentSelfEvaluationResult<StudentSelfEvaluationPage>
                .Failure(StudentSelfEvaluationErrorCode.AccessDenied);
        }

        var school = await _schools.GetByIdAsync(
            actor.SchoolId.Value,
            cancellationToken);

        if (school is null ||
            school.Status != SchoolStatus.Active)
        {
            return StudentSelfEvaluationResult<StudentSelfEvaluationPage>
                .Failure(StudentSelfEvaluationErrorCode.SchoolNotActive);
        }

        var student = await _practice.FindStudentByUserIdAsync(
            actorUserId,
            cancellationToken);

        if (student is null ||
            student.SchoolId != school.Id ||
            student.IsArchived ||
            student.Status != AcademicStructureStatus.Active)
        {
            return StudentSelfEvaluationResult<StudentSelfEvaluationPage>
                .Failure(StudentSelfEvaluationErrorCode.ProfileNotLinked);
        }

        var projection = await _analytics.GetProjectionSnapshotAsync(
            school.Id,
            cancellationToken);

        var classGroup = projection.ClassGroups.SingleOrDefault(
            x =>
                x.Id == classGroupId &&
                x.AcademicYearId == academicYearId);
        var subject = projection.Subjects.SingleOrDefault(
            x => x.Id == subjectId);
        var enrolled = projection.StudentEnrollments.Any(
            x =>
                x.StudentProfileId == student.Id &&
                x.AcademicYearId == academicYearId &&
                x.ClassGroupId == classGroupId);

        if (classGroup is null ||
            subject is null ||
            !enrolled)
        {
            return StudentSelfEvaluationResult<StudentSelfEvaluationPage>
                .Failure(StudentSelfEvaluationErrorCode.ScopeNotAvailable);
        }

        try
        {
            var now = DateTime.UtcNow;
            var officialEvidence =
                _evaluation.NormalizeOfficialEvidence(projection);
            var official = _evaluation.BuildStudentSubject(
                projection,
                officialEvidence,
                student.Id,
                academicYearId,
                classGroupId,
                subjectId,
                now);

            var privateEvidence = await _privatePractice.ListPrivateEvidenceAsync(
                actorUserId,
                cancellationToken);
            var scopedPrivateEvidence = privateEvidence
                .Where(x =>
                    x.AcademicYearId == academicYearId &&
                    x.ClassGroupId == classGroupId &&
                    x.SubjectId == subjectId)
                .ToArray();

            var privateSkillEvidence = ResolvePrivateSkillEvidence(
                projection,
                scopedPrivateEvidence);
            var privateSkills = BuildPrivateSkillEvaluations(
                privateSkillEvidence,
                now);
            var privateSummary = BuildPrivatePracticeSummary(
                privateSkillEvidence,
                privateSkills);

            var privateBySkill = privateSkills
                .ToDictionary(
                    x => x.SkillKey,
                    StringComparer.Ordinal);

            var officialBySkill = official.Skills
                .GroupBy(x => x.SkillKey, StringComparer.Ordinal)
                .ToDictionary(
                    x => x.Key,
                    x => x.OrderByDescending(
                            row => row.InterventionPriority)
                        .ThenBy(
                            row => row.CurrentMasteryPercentage ?? 101m)
                        .First(),
                    StringComparer.Ordinal);

            var skillKeys = officialBySkill.Keys
                .Union(
                    privateBySkill.Keys,
                    StringComparer.Ordinal)
                .OrderBy(key =>
                    officialBySkill.TryGetValue(
                        key,
                        out var officialSkill)
                        ? -(int)officialSkill.InterventionPriority
                        : 0)
                .ThenBy(key =>
                    officialBySkill.TryGetValue(
                        key,
                        out var officialSkill) &&
                    officialSkill.CurrentMasteryPercentage.HasValue
                        ? officialSkill.CurrentMasteryPercentage.Value
                        : 101m)
                .ThenBy(key => key, StringComparer.Ordinal)
                .ToArray();

            var skills = skillKeys
                .Select(key =>
                {
                    officialBySkill.TryGetValue(
                        key,
                        out var officialSkill);
                    privateBySkill.TryGetValue(
                        key,
                        out var privateSkill);

                    var skillName =
                        officialSkill?.SkillName ??
                        privateSkill?.SkillName ??
                        key;
                    var assessment =
                        officialSkill?.AssessmentMasteryPercentage;
                    var privateMastery =
                        privateSkill?.MasteryPercentage;

                    return new StudentSelfSkillEvaluation(
                        key,
                        skillName,
                        officialSkill?.CurrentMasteryPercentage,
                        assessment,
                        privateMastery,
                        assessment.HasValue &&
                        privateMastery.HasValue
                            ? Round2(
                                assessment.Value -
                                privateMastery.Value)
                            : null,
                        officialSkill?.Status ??
                            EvaluationSkillStatus.NotYetAssessed,
                        officialSkill?.ShortTermTrend ??
                            EvaluationTrendBand.InsufficientEvidence,
                        privateSkill?.Trend ??
                            EvaluationTrendBand.InsufficientEvidence,
                        officialSkill?.ConfidenceBand ??
                            EvaluationConfidenceBand.Insufficient,
                        officialSkill?.Evidence.TotalEvidence ?? 0,
                        privateSkill?.EvidenceCount ?? 0,
                        officialSkill?.InterventionPriority ??
                            EvaluationPriority.None);
                })
                .ToArray();

            var assessmentRows = BuildAssessmentRows(
                projection,
                officialEvidence
                    .Where(x =>
                        x.StudentProfileId == student.Id &&
                        x.AcademicYearId == academicYearId &&
                        x.ClassGroupId == classGroupId &&
                        x.SubjectId == subjectId)
                    .ToArray(),
                student.Id,
                academicYearId,
                classGroupId,
                subjectId);

            var termRows = BuildTermRows(
                projection,
                officialEvidence
                    .Where(x =>
                        x.StudentProfileId == student.Id &&
                        x.AcademicYearId == academicYearId &&
                        x.ClassGroupId == classGroupId &&
                        x.SubjectId == subjectId)
                    .ToArray(),
                academicYearId);

            var nextSteps = BuildNextSteps(
                official,
                privateBySkill);

            decimal? privateTransferGap =
                official.AssessmentMasteryPercentage.HasValue &&
                privateSummary.MasteryPercentage.HasValue
                    ? Round2(
                        official.AssessmentMasteryPercentage.Value -
                        privateSummary.MasteryPercentage.Value)
                    : null;

            return StudentSelfEvaluationResult<StudentSelfEvaluationPage>
                .Success(
                    new StudentSelfEvaluationPage(
                        official,
                        privateSummary,
                        privateTransferGap,
                        skills,
                        nextSteps,
                        assessmentRows,
                        termRows));
        }
        catch (InvalidOperationException)
        {
            return StudentSelfEvaluationResult<StudentSelfEvaluationPage>
                .Failure(StudentSelfEvaluationErrorCode.InvalidSourceData);
        }
    }

    private static IReadOnlyList<PrivateSkillEvidence> ResolvePrivateSkillEvidence(
        AnalyticsProjectionSnapshot projection,
        IReadOnlyList<PrivatePracticeEvidenceItem> source)
    {
        var outcomes = projection.LearningOutcomes.ToDictionary(x => x.Id);
        var lessons = projection.PedagogicalLessons.ToDictionary(x => x.Id);
        var result = new List<PrivateSkillEvidence>();

        foreach (var row in source)
        {
            CurriculumPedagogicalLesson? lesson = null;
            if (row.PedagogicalLessonId is Guid lessonId)
                lessons.TryGetValue(lessonId, out lesson);

            var exact = EvaluationSkillResolver.ResolvePrivatePractice(
                row.GenerationParametersJson,
                row.ValidationMetadataJson,
                lesson);

            IReadOnlyList<EvaluationSkillDescriptor> descriptors;

            if (exact is not null)
            {
                descriptors = [exact];
            }
            else
            {
                descriptors = row.LearningOutcomeIds
                    .Where(outcomes.ContainsKey)
                    .SelectMany(id =>
                        EvaluationSkillResolver.ResolveExpected(
                            outcomes[id]))
                    .GroupBy(
                        x => x.SkillKey,
                        StringComparer.Ordinal)
                    .Select(x => x.First())
                    .ToArray();
            }

            if (descriptors.Count == 0 ||
                row.MaxScore <= 0m)
            {
                continue;
            }

            foreach (var descriptor in descriptors)
            {
                result.Add(
                    new PrivateSkillEvidence(
                        row.AttemptId,
                        descriptor.SkillKey,
                        descriptor.SkillName,
                        row.Difficulty,
                        row.Score,
                        row.MaxScore,
                        row.AnsweredAtUtc));
            }
        }

        return result;
    }

    private static IReadOnlyList<StudentPrivatePracticeSkillEvaluation>
        BuildPrivateSkillEvaluations(
            IReadOnlyList<PrivateSkillEvidence> evidence,
            DateTime now)
    {
        return evidence
            .GroupBy(x => x.SkillKey, StringComparer.Ordinal)
            .Select(group =>
            {
                var rows = group
                    .OrderBy(x => x.OccurredAtUtc)
                    .ToArray();

                return new StudentPrivatePracticeSkillEvaluation(
                    group.Key,
                    rows[0].SkillName,
                    WeightedMastery(rows, now),
                    rows.Length,
                    rows.Select(x => x.AttemptId).Distinct().Count(),
                    Trend(rows),
                    rows.Max(x => (DateTime?)x.OccurredAtUtc));
            })
            .OrderBy(x => x.MasteryPercentage ?? 101m)
            .ThenBy(x => x.SkillName)
            .ToArray();
    }

    private static StudentSelfPracticeSummary BuildPrivatePracticeSummary(
        IReadOnlyList<PrivateSkillEvidence> evidence,
        IReadOnlyList<StudentPrivatePracticeSkillEvaluation> skills)
    {
        if (evidence.Count == 0)
        {
            return new StudentSelfPracticeSummary(
                0,
                0,
                0,
                0,
                null,
                EvaluationTrendBand.InsufficientEvidence,
                null);
        }

        var now = DateTime.UtcNow;

        return new StudentSelfPracticeSummary(
            evidence.Select(x => x.AttemptId).Distinct().Count(),
            evidence.Count,
            evidence.Select(x =>
                    DateOnly.FromDateTime(x.OccurredAtUtc))
                .Distinct()
                .Count(),
            skills.Count,
            WeightedMastery(evidence, now),
            Trend(evidence),
            evidence.Max(x => (DateTime?)x.OccurredAtUtc));
    }

    private static IReadOnlyList<StudentSelfNextStep> BuildNextSteps(
        StudentSubjectEvaluation official,
        IReadOnlyDictionary<string, StudentPrivatePracticeSkillEvaluation>
            privateBySkill)
    {
        return official.Skills
            .Where(x =>
                x.InterventionPriority !=
                EvaluationPriority.None)
            .OrderByDescending(x =>
                x.InterventionPriority)
            .ThenByDescending(x =>
                x.GapCriticalityScore)
            .ThenBy(x =>
                x.CurrentMasteryPercentage ?? 101m)
            .Take(5)
            .Select(skill =>
            {
                privateBySkill.TryGetValue(
                    skill.SkillKey,
                    out var privateSkill);

                string message;

                if (skill.WeakPrerequisiteSkillKeys.Count > 0)
                {
                    message =
                        "Strengthen the prerequisite skill first, then return to this skill.";
                }
                else if (skill.Retention is
                         EvaluationRetentionBand.Concern or
                         EvaluationRetentionBand.SignificantConcern)
                {
                    message =
                        "Review this skill because earlier stronger performance has declined.";
                }
                else if (skill.AssessmentMasteryPercentage.HasValue &&
                         privateSkill?.MasteryPercentage is decimal privateMastery &&
                         skill.AssessmentMasteryPercentage.Value -
                             privateMastery <= -15m)
                {
                    message =
                        "You are stronger in private Practice than in Assessment. Review, then try a fresh checkpoint without support.";
                }
                else
                {
                    message =
                        "Use Learn, then focused Practice, then a short checkpoint to verify progress.";
                }

                return new StudentSelfNextStep(
                    skill.SkillKey,
                    skill.SkillName,
                    skill.InterventionPriority,
                    message);
            })
            .ToArray();
    }

    private static IReadOnlyList<AnalyticsStudentAssessmentEvaluationItem>
        BuildAssessmentRows(
            AnalyticsProjectionSnapshot projection,
            IReadOnlyList<EvaluationEvidenceRecord> evidence,
            Guid studentProfileId,
            Guid academicYearId,
            Guid classGroupId,
            Guid subjectId)
    {
        var termNames = projection.Terms
            .ToDictionary(x => x.Id, x => x.Name);
        var assessments = projection.Assessments
            .Where(x =>
                x.Status != AssessmentStatus.Draft &&
                x.AcademicYearId == academicYearId &&
                x.ClassGroupId == classGroupId &&
                x.SubjectId == subjectId)
            .ToDictionary(x => x.Id);
        var results = projection.AssessmentResults
            .Where(x =>
                x.StudentProfileId == studentProfileId &&
                assessments.ContainsKey(x.AssessmentId))
            .Select(x => (
                Assessment: assessments[x.AssessmentId],
                Result: x))
            .OrderBy(x => x.Assessment.AssessmentDate)
            .ThenBy(x => x.Assessment.Title)
            .ToArray();

        var rows = new List<AnalyticsStudentAssessmentEvaluationItem>();

        for (var i = 0; i < results.Length; i++)
        {
            var current = results[i];
            decimal? overallDelta = null;
            decimal? comparableDelta = null;
            var comparableCount = 0;

            if (i > 0)
            {
                var previous = results[i - 1];
                overallDelta = Round2(
                    current.Result.Percentage -
                    previous.Result.Percentage);
                (comparableDelta, comparableCount) =
                    ComparableSkillDelta(
                        evidence.Where(x =>
                            x.SourceId ==
                            previous.Assessment.Id),
                        evidence.Where(x =>
                            x.SourceId ==
                            current.Assessment.Id));
            }

            rows.Add(
                new AnalyticsStudentAssessmentEvaluationItem(
                    current.Assessment.Id,
                    current.Assessment.Title,
                    current.Assessment.AssessmentDate,
                    termNames.GetValueOrDefault(
                        current.Assessment.TermId),
                    current.Result.Percentage,
                    overallDelta,
                    comparableDelta,
                    comparableCount));
        }

        return rows;
    }

    private static IReadOnlyList<AnalyticsTermEvaluationItem> BuildTermRows(
        AnalyticsProjectionSnapshot projection,
        IReadOnlyList<EvaluationEvidenceRecord> evidence,
        Guid academicYearId)
    {
        var terms = projection.Terms
            .Where(x => x.AcademicYearId == academicYearId)
            .OrderBy(x => x.StartsOn)
            .ToArray();
        var result = new List<AnalyticsTermEvaluationItem>();

        for (var i = 0; i < terms.Length; i++)
        {
            var term = terms[i];
            var rows = evidence
                .Where(x => x.TermId == term.Id)
                .ToArray();

            var assessment = SimpleMastery(
                rows.Where(x =>
                    x.Source ==
                    EvaluationEvidenceSource.Assessment));
            var practice = SimpleMastery(
                rows.Where(x =>
                    x.Source ==
                    EvaluationEvidenceSource.Practice));
            var combined = SimpleMastery(rows);

            decimal? assessmentDelta = null;
            decimal? comparable = null;
            var comparableCount = 0;

            if (i > 0)
            {
                var previous = evidence
                    .Where(x =>
                        x.TermId == terms[i - 1].Id)
                    .ToArray();
                var previousAssessment = SimpleMastery(
                    previous.Where(x =>
                        x.Source ==
                        EvaluationEvidenceSource.Assessment));

                if (assessment.HasValue &&
                    previousAssessment.HasValue)
                {
                    assessmentDelta = Round2(
                        assessment.Value -
                        previousAssessment.Value);
                }

                (comparable, comparableCount) =
                    ComparableSkillDelta(previous, rows);
            }

            result.Add(
                new AnalyticsTermEvaluationItem(
                    term.Id,
                    term.Name,
                    term.StartsOn,
                    term.EndsOn,
                    assessment,
                    practice,
                    combined,
                    assessmentDelta,
                    comparable,
                    comparableCount,
                    rows.Length));
        }

        return result;
    }

    private static (decimal? Delta, int Count) ComparableSkillDelta(
        IEnumerable<EvaluationEvidenceRecord> previous,
        IEnumerable<EvaluationEvidenceRecord> current)
    {
        var p = previous
            .GroupBy(x => x.SkillKey, StringComparer.Ordinal)
            .ToDictionary(
                x => x.Key,
                x => SimpleMastery(x),
                StringComparer.Ordinal);
        var c = current
            .GroupBy(x => x.SkillKey, StringComparer.Ordinal)
            .ToDictionary(
                x => x.Key,
                x => SimpleMastery(x),
                StringComparer.Ordinal);
        var common = p.Keys
            .Intersect(c.Keys, StringComparer.Ordinal)
            .Where(key =>
                p[key].HasValue &&
                c[key].HasValue)
            .ToArray();

        return common.Length == 0
            ? (null, 0)
            : (
                Round2(
                    common.Average(key =>
                        c[key]!.Value -
                        p[key]!.Value)),
                common.Length);
    }

    private static decimal? SimpleMastery(
        IEnumerable<EvaluationEvidenceRecord> source)
    {
        var rows = source.ToArray();
        if (rows.Length == 0)
            return null;

        decimal earned = 0m;
        decimal weight = 0m;

        foreach (var row in rows)
        {
            var w =
                row.MappingWeight *
                (row.Difficulty.HasValue
                    ? MasteryEvidenceEngine.DifficultyWeight(
                        row.Difficulty.Value)
                    : 1m);
            earned += row.Percentage * w;
            weight += w;
        }

        return weight <= 0m
            ? null
            : Round2(earned / weight);
    }

    private static decimal? WeightedMastery(
        IEnumerable<PrivateSkillEvidence> source,
        DateTime now)
    {
        var rows = source.ToArray();
        if (rows.Length == 0)
            return null;

        decimal total = 0m;
        decimal weight = 0m;

        foreach (var row in rows)
        {
            if (row.MaxScore <= 0m)
                continue;

            var percentage =
                row.Score / row.MaxScore * 100m;
            var rowWeight =
                MasteryEvidenceEngine.DifficultyWeight(
                    row.Difficulty) *
                MasteryEvidenceEngine.RecencyWeight(
                    row.OccurredAtUtc,
                    now);

            total += percentage * rowWeight;
            weight += rowWeight;
        }

        return weight <= 0m
            ? null
            : Round2(total / weight);
    }

    private static EvaluationTrendBand Trend(
        IEnumerable<PrivateSkillEvidence> source)
    {
        var rows = source
            .OrderBy(x => x.OccurredAtUtc)
            .ToArray();

        if (rows.Length < 4)
            return EvaluationTrendBand.InsufficientEvidence;

        decimal Percentage(PrivateSkillEvidence row) =>
            row.MaxScore <= 0m
                ? 0m
                : row.Score / row.MaxScore * 100m;

        var before = rows.Take(2)
            .Average(Percentage);
        var after = rows.TakeLast(2)
            .Average(Percentage);
        var delta = after - before;

        return delta switch
        {
            <= -10m => EvaluationTrendBand.RapidlyDeclining,
            <= -4m => EvaluationTrendBand.Declining,
            >= 10m => EvaluationTrendBand.RapidlyImproving,
            >= 4m => EvaluationTrendBand.Improving,
            _ => EvaluationTrendBand.Stable
        };
    }

    private static decimal Round2(decimal value) =>
        decimal.Round(
            value,
            2,
            MidpointRounding.AwayFromZero);

    private sealed record PrivateSkillEvidence(
        Guid AttemptId,
        string SkillKey,
        string SkillName,
        AssessmentItemDifficulty Difficulty,
        decimal Score,
        decimal MaxScore,
        DateTime OccurredAtUtc);
}

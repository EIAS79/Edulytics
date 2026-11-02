using Edulytics.Core.AdaptivePractice;
using Edulytics.Core.Analytics;
using Edulytics.Core.Constants;
using Edulytics.Core.Interfaces;
using Edulytics.Services.Analytics;

namespace Edulytics.Services.AdaptivePractice;

public sealed class AdaptiveProgrammeClosureService(
    AdaptivePracticeV2Policy policy,
    IAdaptiveIntelligenceV2Service intelligence,
    IAdaptivePracticeRepository adaptiveRepository,
    IAnalyticsService analytics,
    ISchoolUserRepository schoolUsers)
    : IAdaptiveProgrammeClosureService
{
    private static readonly TimeSpan PsychometricWindow =
        TimeSpan.FromDays(90);

    private static readonly TimeSpan ResearchWindow =
        TimeSpan.FromDays(180);

    private const int MaximumLiveGroupSize = 6;
    private const int MinimumResearchFamilyResponses = 5;

    public async Task<AdaptiveIntelligenceV2Result<AdaptiveLiveGroupPlan>>
        BuildLiveGroupPlanAsync(
            Guid actorUserId,
            Guid academicYearId,
            Guid classGroupId,
            Guid subjectId,
            CancellationToken cancellationToken = default)
    {
        if (!IsFeatureEnabled(policy.EnableLiveGroupSession))
        {
            return AdaptiveIntelligenceV2Result<AdaptiveLiveGroupPlan>
                .Failure(AdaptiveIntelligenceV2Error.FeatureDisabled);
        }

        // Phase 14 deliberately composes the already-authorized Phase 12
        // classroom snapshot. It never creates a parallel access model.
        var snapshot = await intelligence.GetLiveClassroomAsync(
            actorUserId,
            academicYearId,
            classGroupId,
            subjectId,
            cancellationToken);

        if (snapshot.Value is null)
        {
            return AdaptiveIntelligenceV2Result<AdaptiveLiveGroupPlan>
                .Failure(
                    snapshot.Error ??
                    AdaptiveIntelligenceV2Error.ScopeNotAvailable);
        }

        var buckets = snapshot.Value.Students
            .Select(student =>
            {
                var mode = student.NeedsIntervention
                    ? "Remediation"
                    : student.AdaptiveActive
                        ? "Confirmation"
                        : "Extension";

                var target = string.IsNullOrWhiteSpace(
                    student.CurrentSkillId)
                    ? null
                    : student.CurrentSkillId;

                var key = $"{mode}:{target ?? "class"}";

                return new
                {
                    Key = key,
                    Mode = mode,
                    Target = target,
                    Student = student
                };
            })
            .GroupBy(x => new
            {
                x.Key,
                x.Mode,
                x.Target
            })
            .OrderBy(group =>
                group.Key.Mode == "Remediation"
                    ? 0
                    : group.Key.Mode == "Confirmation"
                        ? 1
                        : 2)
            .ThenBy(
                group => group.Key.Target,
                StringComparer.Ordinal)
            .ToArray();

        var groups = new List<AdaptiveLiveGroup>();

        foreach (var bucket in buckets)
        {
            var members = bucket
                .OrderByDescending(x => x.Student.NeedsIntervention)
                .ThenByDescending(x => x.Student.Priority)
                .ThenBy(
                    x => x.Student.DisplayName,
                    StringComparer.OrdinalIgnoreCase)
                .Select(x => x.Student)
                .ToArray();

            for (var offset = 0;
                 offset < members.Length;
                 offset += MaximumLiveGroupSize)
            {
                var slice = members
                    .Skip(offset)
                    .Take(MaximumLiveGroupSize)
                    .Select(student =>
                        new AdaptiveLiveGroupMember(
                            student.StudentProfileId,
                            student.StudentNumber,
                            student.DisplayName,
                            student.CurrentMasteryPercentage,
                            student.AdaptiveActive,
                            student.ActiveMisconceptionCount))
                    .ToArray();

                var part = offset / MaximumLiveGroupSize + 1;

                groups.Add(
                    new AdaptiveLiveGroup(
                        $"{bucket.Key.Key}:{part}",
                        bucket.Key.Mode,
                        bucket.Key.Target,
                        GroupReason(bucket.Key.Mode),
                        slice));
            }
        }

        var plan = new AdaptiveLiveGroupPlan(
            Guid.NewGuid(),
            academicYearId,
            classGroupId,
            subjectId,
            snapshot.Value.StudentCount,
            groups,
            DateTime.UtcNow);

        Console.WriteLine(
            "ADAPTIVE_V2_PHASE14_GROUP_PLAN " +
            $"classGroupId={classGroupId:D} " +
            $"subjectId={subjectId:D} " +
            $"students={plan.StudentCount} " +
            $"groups={plan.Groups.Count}");

        return AdaptiveIntelligenceV2Result<AdaptiveLiveGroupPlan>
            .Success(plan);
    }

    public async Task<
        AdaptiveIntelligenceV2Result<AdaptivePsychometricReadinessReport>>
        GetPsychometricReadinessAsync(
            Guid actorUserId,
            Guid academicYearId,
            Guid classGroupId,
            Guid subjectId,
            CancellationToken cancellationToken = default)
    {
        if (!IsFeatureEnabled(policy.EnablePsychometricReadiness))
        {
            return AdaptiveIntelligenceV2Result<
                    AdaptivePsychometricReadinessReport>
                .Failure(AdaptiveIntelligenceV2Error.FeatureDisabled);
        }

        var scope = await ResolveStaffScopeAsync(
            actorUserId,
            academicYearId,
            classGroupId,
            subjectId,
            requireResearchRole: false,
            cancellationToken);

        if (scope.Error.HasValue)
        {
            return AdaptiveIntelligenceV2Result<
                    AdaptivePsychometricReadinessReport>
                .Failure(scope.Error.Value);
        }

        var evidence = await LoadEvidenceAsync(
            scope.SchoolId,
            scope.StudentIds,
            DateTime.UtcNow - PsychometricWindow,
            cancellationToken);

        var answered = evidence.Turns
            .Where(x => x.AnsweredAtUtc.HasValue &&
                        x.IsCorrect.HasValue)
            .ToArray();

        var distinctStudents = evidence.Sessions
            .Where(session =>
                answered.Any(turn =>
                    turn.SessionId == session.Id))
            .Select(x => x.StudentProfileId)
            .Distinct()
            .Count();

        var enough =
            answered.Length >= policy.MinimumPsychometricResponses &&
            distinctStudents >= policy.MinimumPsychometricStudents;

        var calibrationReady =
            answered.Length >= Math.Max(
                100,
                policy.MinimumPsychometricResponses * 3) &&
            distinctStudents >= Math.Max(
                30,
                policy.MinimumPsychometricStudents * 3);

        var status = !enough
            ? AdaptivePsychometricReadinessStatus.InsufficientEvidence
            : calibrationReady
                ? AdaptivePsychometricReadinessStatus.CalibrationReady
                : AdaptivePsychometricReadinessStatus.DescriptiveReady;

        var sessionStudent = evidence.Sessions
            .ToDictionary(
                x => x.Id,
                x => x.StudentProfileId);

        var studentOverall = answered
            .Where(x => sessionStudent.ContainsKey(x.SessionId))
            .GroupBy(x => sessionStudent[x.SessionId])
            .ToDictionary(
                group => group.Key,
                group => group.Average(
                    x => x.IsCorrect == true ? 1m : 0m));

        var families = answered
            .GroupBy(
                x => x.QuestionFamily,
                StringComparer.Ordinal)
            .OrderBy(x => x.Key, StringComparer.Ordinal)
            .Select(group =>
            {
                var familyRows = group.ToArray();
                var students = familyRows
                    .Where(x => sessionStudent.ContainsKey(x.SessionId))
                    .Select(x => sessionStudent[x.SessionId])
                    .Distinct()
                    .ToArray();

                return new AdaptivePsychometricFamilyStatistic(
                    group.Key,
                    familyRows.Length,
                    students.Length,
                    Round4(
                        familyRows.Average(
                            x => x.IsCorrect == true ? 1m : 0m)),
                    MedianDuration(familyRows),
                    Discrimination(
                        familyRows,
                        sessionStudent,
                        studentOverall));
            })
            .ToArray();

        var report =
            new AdaptivePsychometricReadinessReport(
                academicYearId,
                classGroupId,
                subjectId,
                distinctStudents,
                answered.Length,
                status,
                calibrationReady,
                families,
                DateTime.UtcNow);

        Console.WriteLine(
            "ADAPTIVE_V2_PHASE15_PSYCHOMETRIC " +
            $"classGroupId={classGroupId:D} " +
            $"subjectId={subjectId:D} " +
            $"students={report.StudentCount} " +
            $"responses={report.ResponseCount} " +
            $"status={report.Status}");

        return AdaptiveIntelligenceV2Result<
                AdaptivePsychometricReadinessReport>
            .Success(report);
    }

    public async Task<AdaptiveIntelligenceV2Result<AdaptiveResearchAggregate>>
        GetResearchAggregateAsync(
            Guid actorUserId,
            Guid academicYearId,
            Guid classGroupId,
            Guid subjectId,
            CancellationToken cancellationToken = default)
    {
        if (!IsFeatureEnabled(policy.EnableResearchProgramme))
        {
            return AdaptiveIntelligenceV2Result<AdaptiveResearchAggregate>
                .Failure(AdaptiveIntelligenceV2Error.FeatureDisabled);
        }

        var scope = await ResolveStaffScopeAsync(
            actorUserId,
            academicYearId,
            classGroupId,
            subjectId,
            requireResearchRole: true,
            cancellationToken);

        if (scope.Error.HasValue)
        {
            return AdaptiveIntelligenceV2Result<AdaptiveResearchAggregate>
                .Failure(scope.Error.Value);
        }

        if (scope.StudentIds.Count <
            policy.MinimumResearchCohortSize)
        {
            return AdaptiveIntelligenceV2Result<AdaptiveResearchAggregate>
                .Failure(AdaptiveIntelligenceV2Error.NotEnoughEvidence);
        }

        var evidence = await LoadEvidenceAsync(
            scope.SchoolId,
            scope.StudentIds,
            DateTime.UtcNow - ResearchWindow,
            cancellationToken);

        var answered = evidence.Turns
            .Where(x => x.AnsweredAtUtc.HasValue &&
                        x.IsCorrect.HasValue)
            .ToArray();

        var misconceptions =
            await adaptiveRepository.GetMisconceptionStatesForStudentsAsync(
                scope.SchoolId,
                scope.StudentIds,
                cancellationToken);

        var families = answered
            .GroupBy(
                x => x.QuestionFamily,
                StringComparer.Ordinal)
            .Where(group =>
                group.Count() >= MinimumResearchFamilyResponses)
            .OrderBy(group => group.Key, StringComparer.Ordinal)
            .Select(group =>
                new AdaptiveResearchFamilyAggregate(
                    group.Key,
                    group.Count(),
                    Round4(
                        group.Average(
                            x => x.IsCorrect == true ? 1m : 0m)),
                    MedianDuration(group)))
            .ToArray();

        var overall = answered.Length == 0
            ? 0m
            : Round4(
                answered.Average(
                    x => x.IsCorrect == true ? 1m : 0m));

        var aggregate = new AdaptiveResearchAggregate(
            academicYearId,
            classGroupId,
            subjectId,
            scope.StudentIds.Count,
            evidence.Sessions.Count,
            answered.Length,
            overall,
            misconceptions.Count(x =>
                x.Status !=
                AdaptiveMisconceptionStatus.Resolved),
            families,
            $"Aggregate only; cohort >= {policy.MinimumResearchCohortSize}; no learner identifiers; family cells require >= {MinimumResearchFamilyResponses} responses.",
            DateTime.UtcNow);

        Console.WriteLine(
            "ADAPTIVE_V2_PHASE16_RESEARCH " +
            $"classGroupId={classGroupId:D} " +
            $"subjectId={subjectId:D} " +
            $"cohort={aggregate.CohortSize} " +
            $"sessions={aggregate.SessionCount} " +
            $"responses={aggregate.AnsweredResponseCount}");

        return AdaptiveIntelligenceV2Result<AdaptiveResearchAggregate>
            .Success(aggregate);
    }

    private async Task<ScopeResolution> ResolveStaffScopeAsync(
        Guid actorUserId,
        Guid academicYearId,
        Guid classGroupId,
        Guid subjectId,
        bool requireResearchRole,
        CancellationToken cancellationToken)
    {
        var actor = await schoolUsers.GetActorAsync(
            actorUserId,
            cancellationToken);

        if (actor?.SchoolId is not Guid schoolId ||
            !actor.IsActive ||
            actor.IsLocked ||
            !policy.AllowsSchool(schoolId))
        {
            return ScopeResolution.Failure(
                AdaptiveIntelligenceV2Error.AccessDenied);
        }

        var allowedRoles = requireResearchRole
            ? new[]
            {
                RoleNames.SchoolAdmin,
                RoleNames.SubjectSupervisor
            }
            : new[]
            {
                RoleNames.SchoolAdmin,
                RoleNames.SubjectSupervisor,
                RoleNames.Teacher
            };

        if (!actor.Roles.Any(role =>
                allowedRoles.Contains(
                    role,
                    StringComparer.Ordinal)))
        {
            return ScopeResolution.Failure(
                AdaptiveIntelligenceV2Error.AccessDenied);
        }

        var page = await analytics.GetStudentsEvaluationAsync(
            actorUserId,
            academicYearId,
            classGroupId,
            subjectId,
            cancellationToken);

        if (page.Value is null)
        {
            return ScopeResolution.Failure(
                page.Error == AnalyticsErrorCode.AccessDenied
                    ? AdaptiveIntelligenceV2Error.AccessDenied
                    : AdaptiveIntelligenceV2Error.ScopeNotAvailable);
        }

        var students = page.Value.Students
            .Select(x => x.StudentProfileId)
            .Distinct()
            .Take(policy.MaximumIntelligenceRows)
            .ToArray();

        return ScopeResolution.Success(
            schoolId,
            students);
    }

    private async Task<EvidenceBundle> LoadEvidenceAsync(
        Guid schoolId,
        IReadOnlyCollection<Guid> studentIds,
        DateTime sinceUtc,
        CancellationToken cancellationToken)
    {
        var sessions = await adaptiveRepository.GetRecentSessionsAsync(
            schoolId,
            studentIds,
            sinceUtc,
            policy.MaximumIntelligenceRows,
            cancellationToken);

        var turns = await adaptiveRepository.GetTurnsForSessionsAsync(
            schoolId,
            sessions.Select(x => x.Id).ToArray(),
            cancellationToken);

        return new EvidenceBundle(
            sessions,
            turns
                .Take(policy.MaximumIntelligenceRows)
                .ToArray());
    }

    private bool IsFeatureEnabled(bool flag) =>
        policy.Enabled &&
        policy.Mode != AdaptivePracticeV2Mode.Off &&
        flag;

    private static string GroupReason(string mode) =>
        mode switch
        {
            "Remediation" =>
                "Students share an active weakness, incorrect response, or unresolved misconception signal.",
            "Confirmation" =>
                "Students are active in Adaptive Practice and are ready for a short independent confirmation.",
            _ =>
                "No urgent intervention signal is active; use extension or transfer work."
        };

    private static long? MedianDuration(
        IEnumerable<Edulytics.Core.Entities.AdaptivePracticeTurn> source)
    {
        var values = source
            .Where(x => x.ResponseDurationMs.HasValue)
            .Select(x => x.ResponseDurationMs!.Value)
            .OrderBy(x => x)
            .ToArray();

        if (values.Length == 0)
            return null;

        var middle = values.Length / 2;

        if (values.Length % 2 == 1)
            return values[middle];

        return (values[middle - 1] + values[middle]) / 2;
    }

    private static decimal? Discrimination(
        IReadOnlyList<Edulytics.Core.Entities.AdaptivePracticeTurn> rows,
        IReadOnlyDictionary<Guid, Guid> sessionStudent,
        IReadOnlyDictionary<Guid, decimal> studentOverall)
    {
        var students = rows
            .Where(row =>
                sessionStudent.TryGetValue(
                    row.SessionId,
                    out var studentId) &&
                studentOverall.ContainsKey(studentId))
            .Select(row => sessionStudent[row.SessionId])
            .Distinct()
            .OrderBy(id => studentOverall[id])
            .ToArray();

        if (students.Length < 6)
            return null;

        var third = Math.Max(2, students.Length / 3);
        var bottom = students.Take(third).ToHashSet();
        var top = students.TakeLast(third).ToHashSet();

        decimal Accuracy(HashSet<Guid> cohort)
        {
            var cohortRows = rows
                .Where(row =>
                    sessionStudent.TryGetValue(
                        row.SessionId,
                        out var studentId) &&
                    cohort.Contains(studentId) &&
                    row.IsCorrect.HasValue)
                .ToArray();

            return cohortRows.Length == 0
                ? 0m
                : cohortRows.Average(
                    row => row.IsCorrect == true ? 1m : 0m);
        }

        return Round4(
            Accuracy(top) -
            Accuracy(bottom));
    }

    private static decimal Round4(decimal value) =>
        decimal.Round(
            value,
            4,
            MidpointRounding.AwayFromZero);

    private sealed record EvidenceBundle(
        IReadOnlyList<Edulytics.Core.Entities.AdaptivePracticeSession> Sessions,
        IReadOnlyList<Edulytics.Core.Entities.AdaptivePracticeTurn> Turns);

    private sealed record ScopeResolution(
        Guid SchoolId,
        IReadOnlyList<Guid> StudentIds,
        AdaptiveIntelligenceV2Error? Error)
    {
        public static ScopeResolution Success(
            Guid schoolId,
            IReadOnlyList<Guid> studentIds) =>
            new(
                schoolId,
                studentIds,
                null);

        public static ScopeResolution Failure(
            AdaptiveIntelligenceV2Error error) =>
            new(
                Guid.Empty,
                [],
                error);
    }
}

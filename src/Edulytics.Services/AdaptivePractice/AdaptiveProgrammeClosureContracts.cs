using Edulytics.Core.Analytics;

namespace Edulytics.Services.AdaptivePractice;

public sealed record AdaptiveLiveGroupMember(
    Guid StudentProfileId,
    string StudentNumber,
    string DisplayName,
    decimal? CurrentMasteryPercentage,
    bool AdaptiveActive,
    int ActiveMisconceptionCount);

public sealed record AdaptiveLiveGroup(
    string GroupKey,
    string Mode,
    string? TargetSkillId,
    string Reason,
    IReadOnlyList<AdaptiveLiveGroupMember> Members);

public sealed record AdaptiveLiveGroupPlan(
    Guid PlanId,
    Guid AcademicYearId,
    Guid ClassGroupId,
    Guid SubjectId,
    int StudentCount,
    IReadOnlyList<AdaptiveLiveGroup> Groups,
    DateTime GeneratedAtUtc);

public enum AdaptivePsychometricReadinessStatus
{
    InsufficientEvidence = 0,
    DescriptiveReady = 1,
    CalibrationReady = 2
}

public sealed record AdaptivePsychometricFamilyStatistic(
    string QuestionFamily,
    int ResponseCount,
    int StudentCount,
    decimal CorrectRate,
    long? MedianResponseDurationMs,
    decimal? DiscriminationIndex);

public sealed record AdaptivePsychometricReadinessReport(
    Guid AcademicYearId,
    Guid ClassGroupId,
    Guid SubjectId,
    int StudentCount,
    int ResponseCount,
    AdaptivePsychometricReadinessStatus Status,
    bool ReliabilityStudyEligible,
    IReadOnlyList<AdaptivePsychometricFamilyStatistic> Families,
    DateTime GeneratedAtUtc);

public sealed record AdaptiveResearchFamilyAggregate(
    string QuestionFamily,
    int ResponseCount,
    decimal CorrectRate,
    long? MedianResponseDurationMs);

public sealed record AdaptiveResearchAggregate(
    Guid AcademicYearId,
    Guid ClassGroupId,
    Guid SubjectId,
    int CohortSize,
    int SessionCount,
    int AnsweredResponseCount,
    decimal OverallCorrectRate,
    int ActiveMisconceptionSignalCount,
    IReadOnlyList<AdaptiveResearchFamilyAggregate> Families,
    string PrivacyRule,
    DateTime GeneratedAtUtc);

public interface IAdaptiveProgrammeClosureService
{
    Task<AdaptiveIntelligenceV2Result<AdaptiveLiveGroupPlan>> BuildLiveGroupPlanAsync(
        Guid actorUserId,
        Guid academicYearId,
        Guid classGroupId,
        Guid subjectId,
        CancellationToken cancellationToken = default);

    Task<AdaptiveIntelligenceV2Result<AdaptivePsychometricReadinessReport>> GetPsychometricReadinessAsync(
        Guid actorUserId,
        Guid academicYearId,
        Guid classGroupId,
        Guid subjectId,
        CancellationToken cancellationToken = default);

    Task<AdaptiveIntelligenceV2Result<AdaptiveResearchAggregate>> GetResearchAggregateAsync(
        Guid actorUserId,
        Guid academicYearId,
        Guid classGroupId,
        Guid subjectId,
        CancellationToken cancellationToken = default);
}

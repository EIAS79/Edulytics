using Edulytics.Core.Analytics;
using Edulytics.Core.Enums;

namespace Edulytics.Core.LearningIntelligence;

public sealed record LearningIntelligenceStudentSnapshot(
    Guid SchoolId,
    Guid StudentProfileId,
    string StudentDisplayName,
    string CurriculumLevelKey,
    string CurriculumLevelLabel,
    Guid ClassGroupId,
    string ClassName,
    Guid? TeacherUserId,
    string? TeacherDisplayName,
    DateTime CapturedAtUtc,
    StudentLearningProfile Profile);

public sealed record RecoveryIntelligenceObservation(
    Guid SchoolId,
    Guid StudentProfileId,
    Guid LearningOutcomeId,
    DateTime OccurredAtUtc,
    decimal BeforeMastery,
    decimal AfterMastery,
    bool RecoveredToSecureOrStrong);

public sealed record LearningIntelligenceRequest(
    Guid SchoolId,
    IReadOnlyCollection<LearningIntelligenceStudentSnapshot> Snapshots,
    IReadOnlyCollection<RecoveryIntelligenceObservation> RecoveryObservations);

public sealed record MasteryTrendPoint(
    DateTime CapturedAtUtc,
    decimal MasteryPercentage,
    int StudentCount);

public sealed record StudentTrendRow(
    Guid StudentProfileId,
    string StudentDisplayName,
    decimal FirstMasteryPercentage,
    decimal LatestMasteryPercentage,
    decimal ChangePercentagePoints,
    MasteryBand LatestBand,
    IReadOnlyList<MasteryTrendPoint> Trend);

public sealed record ClassTrendRow(
    Guid ClassGroupId,
    string ClassName,
    decimal FirstMasteryPercentage,
    decimal LatestMasteryPercentage,
    decimal ChangePercentagePoints,
    int StudentCount,
    IReadOnlyList<MasteryTrendPoint> Trend);

public sealed record OutcomeWeaknessRow(
    Guid LearningOutcomeId,
    string OutcomeCode,
    string OutcomeDescription,
    int StudentsMeasured,
    int WeakStudents,
    decimal WeakStudentPercentage,
    decimal AverageMasteryPercentage);

public sealed record WeaknessConcentrationRow(
    string CurriculumLevelKey,
    string CurriculumLevelLabel,
    Guid ClassGroupId,
    string ClassName,
    int StudentsMeasured,
    int WeakOutcomeInstances,
    decimal WeakOutcomePercentage);

public sealed record RecoveryEffectivenessRow(
    Guid LearningOutcomeId,
    int ReassessmentCount,
    int ImprovedCount,
    int RecoveredCount,
    decimal AverageImprovementPercentagePoints,
    decimal RecoveryRatePercentage);

public sealed record LearningIntelligenceDrilldownRow(
    string CurriculumLevelKey,
    string CurriculumLevelLabel,
    Guid ClassGroupId,
    string ClassName,
    Guid? TeacherUserId,
    string? TeacherDisplayName,
    Guid StudentProfileId,
    string StudentDisplayName,
    Guid LearningOutcomeId,
    string OutcomeCode,
    decimal MasteryPercentage,
    MasteryBand Band,
    decimal ConfidencePercentage,
    int EvidenceCount,
    DateTime? LatestEvidenceAtUtc);

public sealed record LearningIntelligenceDashboard(
    Guid SchoolId,
    decimal SchoolMasteryPercentage,
    decimal ImprovementRatePercentage,
    int StudentCount,
    int OutcomeEvidenceCount,
    IReadOnlyList<StudentTrendRow> StudentTrends,
    IReadOnlyList<ClassTrendRow> ClassTrends,
    IReadOnlyList<OutcomeWeaknessRow> OutcomeWeaknessDistribution,
    IReadOnlyList<WeaknessConcentrationRow> WeaknessConcentration,
    IReadOnlyList<RecoveryEffectivenessRow> RecoveryEffectiveness,
    IReadOnlyList<LearningIntelligenceDrilldownRow> Drilldown,
    string FormulaVersion);

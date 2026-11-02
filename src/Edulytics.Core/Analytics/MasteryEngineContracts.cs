using Edulytics.Core.Enums;

namespace Edulytics.Core.Analytics;

public sealed record StudentOutcomeLearningProfile(
    Guid LearningOutcomeId,
    string OutcomeCode,
    string OutcomeDescription,
    decimal MasteryPercentage,
    MasteryBand Band,
    int EvidenceCount,
    decimal ConfidencePercentage,
    DateTime? LatestEvidenceAtUtc,
    int EasyEvidenceCount,
    int MediumEvidenceCount,
    int ChallengingEvidenceCount,
    decimal WeightedEvidence,
    string FormulaVersion);

public sealed record StudentLearningProfile(
    Guid SchoolId,
    Guid StudentProfileId,
    Guid AcademicYearId,
    Guid ClassGroupId,
    Guid CurriculumAdoptionId,
    decimal OverallMasteryPercentage,
    MasteryBand OverallBand,
    int EvidenceCount,
    decimal ConfidencePercentage,
    DateTime? LatestEvidenceAtUtc,
    IReadOnlyList<StudentOutcomeLearningProfile> Outcomes,
    string FormulaVersion);

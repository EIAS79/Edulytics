using Edulytics.Core.Curriculum;

namespace Edulytics.Core.AdaptivePractice;

public enum AdaptivePracticeV2Mode
{
    Off = 0,
    Shadow = 1,
    Canary = 2,
    On = 3
}

public static class AdaptivePracticeV2Versions
{
    public const string EngineVersion = "adaptive-practice-v2.1";
    public const string PolicyVersion = "adaptive-v2-c0-c5-closure-v1";
}

public static class AdaptivePracticeV2Behavior
{
    public const int MaximumSameItemRetries = 1;
    public const int MaximumSessionItems = 30;
}

public static class AdaptivePracticeEligibilityReasonCodes
{
    public const string Eligible = "ELIGIBLE";
    public const string FeatureDisabled = "FEATURE_DISABLED";
    public const string ModeOff = "MODE_OFF";
    public const string InvalidScope = "INVALID_SCOPE";
    public const string SchoolNotAllowed = "SCHOOL_NOT_ALLOWED";
    public const string CurriculumLevelNotAllowed = "CURRICULUM_LEVEL_NOT_ALLOWED";
    public const string CurriculumLevelUnknown = "CURRICULUM_LEVEL_UNKNOWN";
    public const string OutsidePrimaryRollout = "OUTSIDE_PRIMARY_ROLLOUT";
    public const string LessonNotAllowed = "LESSON_NOT_ALLOWED";
    public const string PracticeCapabilityMissing = "PRACTICE_CAPABILITY_MISSING";
    public const string PracticeCapabilityNotReady = "PRACTICE_CAPABILITY_NOT_READY";
}

public sealed record AdaptivePracticeEligibilityRequest(
    Guid SchoolId,
    string CurriculumLevelKey,
    string LessonCode,
    bool IsMathematics);

public sealed record AdaptivePracticeEligibilityDecision(
    bool IsEligible,
    AdaptivePracticeV2Mode Mode,
    string ReasonCode,
    string? CurriculumLevelKey = null,
    int? LogicalLevel = null,
    string? LessonCode = null,
    string? SkillId = null,
    string? ContractVersion = null)
{
    public bool IsShadow =>
        IsEligible && Mode == AdaptivePracticeV2Mode.Shadow;

    public bool IsLearnerFacing =>
        IsEligible &&
        Mode is AdaptivePracticeV2Mode.Canary or AdaptivePracticeV2Mode.On;
}

public sealed record AdaptivePracticeV2Policy(
    bool Enabled,
    AdaptivePracticeV2Mode Mode,
    IReadOnlySet<string> AllowedCurriculumLevelKeys,
    IReadOnlySet<string> AllowedLessonCodes,
    IReadOnlySet<Guid> AllowedSchoolIds,
    int ShadowSamplingPercentage,
    int MaxLessonQuestions,
    bool EnableMisconceptionLoop,
    bool EnableDirectNextSteps,
    bool EnableQuestionLog,
    bool EnableLiveClassroom,
    bool EnableDiagnosticV2)
{
    public static AdaptivePracticeV2Policy Off { get; } =
        new(
            false,
            AdaptivePracticeV2Mode.Off,
            new HashSet<string>(StringComparer.OrdinalIgnoreCase),
            new HashSet<string>(StringComparer.Ordinal),
            new HashSet<Guid>(),
            100,
            8,
            false,
            false,
            false,
            false,
            false);

    public bool AllowsSchool(Guid schoolId) =>
        Mode == AdaptivePracticeV2Mode.Canary
            ? AllowedSchoolIds.Contains(schoolId)
            : AllowedSchoolIds.Count == 0 || AllowedSchoolIds.Contains(schoolId);

    public bool AllowsCurriculumLevel(string key) =>
        AllowedCurriculumLevelKeys.Contains(key);

    public bool RouteAllReadyVerifiedLessons { get; init; }

    public bool AllowsLesson(string lessonCode) =>
        RouteAllReadyVerifiedLessons ||
        (Mode == AdaptivePracticeV2Mode.Canary
            ? AllowedLessonCodes.Contains(lessonCode)
            : AllowedLessonCodes.Count == 0 ||
              AllowedLessonCodes.Contains(lessonCode));

    public bool EnableLiveGroupSession { get; init; }

    public bool EnablePsychometricReadiness { get; init; }

    public bool EnableResearchProgramme { get; init; }

    public int MinimumPsychometricResponses { get; init; } = 30;

    public int MinimumPsychometricStudents { get; init; } = 10;

    public int MinimumResearchCohortSize { get; init; } = 10;

    public int MaximumIntelligenceRows { get; init; } = 500;
}

public interface IAdaptivePracticeEligibilityResolver
{
    AdaptivePracticeEligibilityDecision Resolve(
        AdaptivePracticeEligibilityRequest request);
}

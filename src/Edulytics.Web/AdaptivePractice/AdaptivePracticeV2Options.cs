using Edulytics.Core.AdaptivePractice;

namespace Edulytics.Web.AdaptivePractice;

public sealed class AdaptivePracticeV2Options
{
    public const string SectionName = "Edulytics:AdaptivePracticeV2";

    public bool Enabled { get; set; }

    public string Mode { get; set; } = nameof(AdaptivePracticeV2Mode.Off);

    public string[] AllowedCurriculumLevelKeys { get; set; } = [];

    public string[] AllowedLessonCodes { get; set; } = [];

    public Guid[] AllowedSchoolIds { get; set; } = [];

    public int ShadowSamplingPercentage { get; set; } = 100;

    public int MaxLessonQuestions { get; set; } = 8;

    public bool EnableMisconceptionLoop { get; set; }

    public bool RouteAllReadyVerifiedLessons { get; set; }

    public bool RouteAllReadyVerifiedCatalogue { get; set; }

    public bool EnableDirectNextSteps { get; set; }

    public bool EnableQuestionLog { get; set; }

    public bool EnableLiveClassroom { get; set; }

    public bool EnableDiagnosticV2 { get; set; }

    public bool EnableLiveGroupSession { get; set; }

    public bool EnablePsychometricReadiness { get; set; }

    public bool EnableResearchProgramme { get; set; }

    public int MinimumPsychometricResponses { get; set; } = 30;

    public int MinimumPsychometricStudents { get; set; } = 10;

    public int MinimumResearchCohortSize { get; set; } = 10;

    public int MaximumIntelligenceRows { get; set; } = 500;

    public AdaptivePracticeV2Policy ToPolicy()
    {
        var mode = Enum.TryParse<AdaptivePracticeV2Mode>(
            Mode,
            ignoreCase: true,
            out var parsed)
            ? parsed
            : AdaptivePracticeV2Mode.Off;

        return new AdaptivePracticeV2Policy(
            Enabled,
            mode,
            AllowedCurriculumLevelKeys
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Trim().ToUpperInvariant())
                .ToHashSet(StringComparer.OrdinalIgnoreCase),
            AllowedLessonCodes
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Trim())
                .ToHashSet(StringComparer.Ordinal),
            AllowedSchoolIds
                .Where(x => x != Guid.Empty)
                .ToHashSet(),
            ShadowSamplingPercentage,
            MaxLessonQuestions,
            EnableMisconceptionLoop,
            EnableDirectNextSteps,
            EnableQuestionLog,
            EnableLiveClassroom,
            EnableDiagnosticV2)
        {
            RouteAllReadyVerifiedLessons = RouteAllReadyVerifiedLessons,
            RouteAllReadyVerifiedCatalogue = RouteAllReadyVerifiedCatalogue,
            EnableLiveGroupSession = EnableLiveGroupSession,
            EnablePsychometricReadiness = EnablePsychometricReadiness,
            EnableResearchProgramme = EnableResearchProgramme,
            MinimumPsychometricResponses = MinimumPsychometricResponses,
            MinimumPsychometricStudents = MinimumPsychometricStudents,
            MinimumResearchCohortSize = MinimumResearchCohortSize,
            MaximumIntelligenceRows = MaximumIntelligenceRows
        };
    }
}

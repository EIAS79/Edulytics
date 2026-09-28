using Edulytics.Core.Mathematics.Rollout;

namespace Edulytics.Web.Mathematics;

public sealed class AdvancedMathematicsV3Options
{
    public const string SectionName =
        "Edulytics:AdvancedMathematicsV3";

    public bool Enabled { get; set; }
    public bool PracticeEnabled { get; set; }
    public bool AssessmentEnabled { get; set; }
    public bool ExamEnabled { get; set; }
    public bool AdaptiveEnabled { get; set; }

    public string[] AllowedCurriculumLevelKeys { get; set; } = [];
    public Guid[] AllowedSchoolIds { get; set; } = [];

    public AdvancedMathematicsV3Policy ToPolicy() =>
        new(
            Enabled,
            PracticeEnabled,
            AssessmentEnabled,
            ExamEnabled,
            AdaptiveEnabled,
            AllowedCurriculumLevelKeys
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Trim().ToUpperInvariant())
                .ToHashSet(StringComparer.OrdinalIgnoreCase),
            AllowedSchoolIds
                .Where(x => x != Guid.Empty)
                .ToHashSet());
}

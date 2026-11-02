using Edulytics.Core.Curriculum;

namespace Edulytics.Data.Seeding;

/// <summary>
/// Exact production-safe learner-content targets discovered by the
/// student-facing forensic audit. The actual learner body is supplied by the
/// reviewed Supporting Practice target rules; this class only constrains the
/// narrow startup persistence scope.
/// </summary>
public static class StudentFacingLessonContentCorrections
{
    public const string Stage3UnitFractionBuild =
        "PED:CAMBRIDGE-INTL-MATH:S3:3F-2:BUILD";
    public const string Stage3UnitFractionApply =
        "PED:CAMBRIDGE-INTL-MATH:S3:3F-2:APPLY";
    public const string Stage5NonUnitFractionBuild =
        "PED:CAMBRIDGE-INTL-MATH:S5:5F-1:BUILD";
    public const string Stage5NonUnitFractionApply =
        "PED:CAMBRIDGE-INTL-MATH:S5:5F-1:APPLY";

    private static readonly HashSet<string> TargetLessonCodes =
        new(StringComparer.Ordinal)
        {
            Stage3UnitFractionBuild,
            Stage3UnitFractionApply,
            Stage5NonUnitFractionBuild,
            Stage5NonUnitFractionApply
        };

    public static bool IsTarget(
        CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson) =>
        string.Equals(
            document.PackCode,
            "CAMBRIDGE-INTL-MATH",
            StringComparison.Ordinal) &&
        TargetLessonCodes.Contains(lesson.LessonCode);

    public static IReadOnlyCollection<string> AllLessonCodes =>
        TargetLessonCodes;
}

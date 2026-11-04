namespace Edulytics.Core.Mathematics.Practice;

/// <summary>
/// IM 2021 Grade 5, U01 L01: digital connected cube count comparisons only.
/// Physical builds, Which One Doesn't Belong reasoning, four-object ordering,
/// explanations and Edulytiks-original Cool-down remain teacher-reviewed.
/// </summary>
internal static class UsCcssG5Unit1Lesson1PracticeContracts
{
    public const string Version = "usccss-g5-u01-l01-connected-compare-v1";

    public static readonly IReadOnlyList<LessonPracticeContract> Reviewed =
    [
        new("PED:US-CCSS-MATH:G5:U01:L01",
            "geometry.volume",
            "CONNECTED_CUBE_COUNT_COMPARISON_TEACHER_LED_BUILD",
            ["usccss.g5.u01.l01.compare_connected_cube_models"],
            "IM2021_G5_U01_L01_LOCAL_ORIGINAL_TEACHER_STUDENT_FACING",
            "READY_VERIFIED",
            Version)
    ];
}

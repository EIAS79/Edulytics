namespace Edulytics.Core.Mathematics.Practice;

/// <summary>
/// Reviewed 2021 first-edition Grade 4 Unit 1 Lesson 1 arithmetic only.
/// Open figure reasoning, tiles, grid drawings and source Cool-down are not
/// electronically scored by these contracts.
/// </summary>
internal static class UsCcssG4Unit1FirstLessonPracticeContracts
{
    public const string Version = "usccss-g4-u01-l01-original-area-multiples-v1";
    public static readonly IReadOnlyList<LessonPracticeContract> Reviewed =
    [
        new("PED:US-CCSS-MATH:G4:U01:L01",
            "geometry.rectangle.area",
            "RECTANGLE_WIDTH_AREA_MULTIPLES",
            [
                "usccss.g4.u01.l01.fixed_width_area",
                "usccss.g4.u01.l01.area_is_multiple",
                "usccss.g4.u01.l01.missing_whole_length",
                "usccss.g4.u01.l01.possible_area_above30"
            ],
            "IM2021_G4_U01_L01_2021_SOURCE_PDF_AND_FIGURES_REVIEWED",
            "READY_VERIFIED",
            Version)
    ];
}

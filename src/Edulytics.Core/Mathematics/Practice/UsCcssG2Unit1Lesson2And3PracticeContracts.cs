namespace Edulytics.Core.Mathematics.Practice;

/// <summary>
/// First-edition IM 2021 G2 U01 L02-03, exact source-goal numerical portions.
/// Physical partner cube games, writing several personally-authored equations,
/// oral strategies, physical tower drawings and community norms remain
/// teacher-assessed. READY_VERIFIED is a family contract capability, not an
/// academic lesson acceptance or executed Stage D result.
/// </summary>
internal static class UsCcssG2Unit1Lesson2And3PracticeContracts
{
    public const string Version = "usccss-g2-u01-l02-l03-source-goal-v1";

    public static readonly IReadOnlyList<LessonPracticeContract> Reviewed =
    [
        new("PED:US-CCSS-MATH:G2:U01:L02",
            "number.whole.add_subtract",
            "HIDDEN_TEN_CUBES_RELATED_EQUATIONS_AND_TOWER_MATCH",
            [
                "usccss.g2.u01.l02.hidden_ten_cubes",
                "usccss.g2.u01.l02.related_ten_equation",
                "usccss.g2.u01.l02.match_ten_cube_tower"
            ],
            "IM2021_G2_U01_L02_TEN_CUBE_PARTNERS_AND_COLOR_TOWERS",
            "READY_VERIFIED", Version),
        new("PED:US-CCSS-MATH:G2:U01:L03",
            "number.whole.add_subtract",
            "HIDDEN_TWENTY_CUBES_AND_EQUATIONS_WITH_20",
            [
                "usccss.g2.u01.l03.hidden_twenty_cubes",
                "usccss.g2.u01.l03.missing_in_twenty_equation",
                "usccss.g2.u01.l03.extension_unknown_minuend"
            ],
            "IM2021_G2_U01_L03_TWO_TENS_TWENTY_CUBES",
            "READY_VERIFIED", Version)
    ];
}

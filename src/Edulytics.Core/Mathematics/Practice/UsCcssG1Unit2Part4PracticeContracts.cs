namespace Edulytics.Core.Mathematics.Practice;

/// <summary>
/// Source-specific Grade 1 U02 L11-L15 comparison and mixed-story Practice.
/// Numeric/choice item grading is not approval of physical cube manipulation,
/// data collection, written equation authoring or whole 500-lesson B001 release.
/// </summary>
internal static class UsCcssG1Unit2Part4PracticeContracts
{
    public const string Version="usccss-g1-u02-part4-practice-v1";
    public static readonly IReadOnlyList<LessonPracticeContract> Reviewed =
    [
        new("PED:US-CCSS-MATH:G1:U02:L11",
            "number.whole.add_subtract","CUBE_TOWER_DIFFERENCE_UNKNOWN",
            ["usccss.g1.u02.towers.difference"],
            "IM2021_G1_U02_L11_CUBE_TOWERS_COMPARE","READY_VERIFIED",Version),
        new("PED:US-CCSS-MATH:G1:U02:L12",
            "number.whole.add_subtract","SCHOOL_SUPPLIES_DIFFERENCE_UNKNOWN",
            ["usccss.g1.u02.supplies.difference"],
            "IM2021_G1_U02_L12_SCHOOL_SUPPLIES_COMPARE","READY_VERIFIED",Version),
        new("PED:US-CCSS-MATH:G1:U02:L13",
            "number.whole.add_subtract","CATEGORY_DATA_DIFFERENCE_UNKNOWN",
            ["usccss.g1.u02.artdata.difference"],
            "IM2021_G1_U02_L13_ART_DATA_COMPARE","READY_VERIFIED",Version),
        new("PED:US-CCSS-MATH:G1:U02:L14",
            "number.whole.add_subtract","MATCH_COMPARE_EQUATION",
            ["usccss.g1.u02.equations.match_difference"],
            "IM2021_G1_U02_L14_MATCH_ADD_SUBTRACT_EQUATIONS","READY_VERIFIED",Version),
        new("PED:US-CCSS-MATH:G1:U02:L15",
            "number.whole.add_subtract","STORY_PROBLEM_UNKNOWN_KIND",
            ["usccss.g1.u02.story.mixed_unknowns"],
            "IM2021_G1_U02_L15_MIXED_STORY_UNKNOWNS","READY_VERIFIED",Version)
    ];
}

namespace Edulytics.Core.Mathematics.Practice;

/// <summary>
/// Exact original IM 2021 G1 U02 L08-L10 source goals, not generic math worksheets.
/// Capture Squares visual board and independent academic signoff remain separate.
/// </summary>
internal static class UsCcssG1Unit2Part3PracticeContracts
{
    public const string Version="usccss-g1-u02-part3-practice-v1";
    public static readonly IReadOnlyList<LessonPracticeContract> Reviewed =
    [
        new("PED:US-CCSS-MATH:G1:U02:L08",
            "number.whole.add_subtract","COVERED_ADDEND_UNKNOWN",
            ["usccss.g1.u02.shake_cover.addend_unknown"],
            "IM2021_G1_U02_L08_COVER_KNOWN_TOTAL_VISIBLE_PART","READY_VERIFIED",Version),
        new("PED:US-CCSS-MATH:G1:U02:L09",
            "number.whole.add_subtract","PART_WHOLE_UNKNOWNS_AND_GALLERY",
            ["usccss.g1.u02.story.result_part_or_both_unknown"],
            "IM2021_G1_U02_L09_RESULT_ONE_OR_BOTH_PARTS","READY_VERIFIED",Version),
        new("PED:US-CCSS-MATH:G1:U02:L10",
            "number.whole.add_subtract","CAPTURE_SQUARES_ADD_WITHIN_TEN",
            ["usccss.g1.u02.center.capture_squares.sum_to10"],
            "IM2021_G1_U02_L10_CAPTURE_SQUARES_CENTER","READY_VERIFIED",Version)
    ];
}

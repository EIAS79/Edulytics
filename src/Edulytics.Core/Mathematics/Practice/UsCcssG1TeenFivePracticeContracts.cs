namespace Edulytics.Core.Mathematics.Practice;

/// <summary>V1 five consecutive original 2021 Grade 1 ordinals 46–50.</summary>
internal static class UsCcssG1TeenFivePracticeContracts
{
    public const string Version="us-ccss-g1-010-u03-l09-l13-original-2021-v1";
    public static readonly IReadOnlyList<LessonPracticeContract> Reviewed=
    [
        new("PED:US-CCSS-MATH:G1:U03:L09","usccss.g1.ten_as_unit",
            "TEN_FRAME_TEEN_COMPOSITION_AND_MISSING_ADDEND",
            ["usccss.g1.u03.l09.ten.add_frame","usccss.g1.u03.l09.ten.missing_equation"],
            "IM2021_U03_L09_TEN_AND_SOME_ONES","READY_VERIFIED",Version),
        new("PED:US-CCSS-MATH:G1:U03:L10","number.whole.add_subtract",
            "TEN_STORY_CHANGE_AND_START_UNKNOWN_RELATED_EQUATIONS",
            ["usccss.g1.u03.l10.teen.story_unknown","usccss.g1.u03.l10.teen.related_equations"],
            "IM2021_U03_L10_TEN_STORIES","READY_VERIFIED",Version),
        new("PED:US-CCSS-MATH:G1:U03:L11","number.whole.add_subtract",
            "ADD_TO_TEEN_STORY_AND_ONE_DIGIT_EQUATION",
            ["usccss.g1.u03.l11.teen.add_story","usccss.g1.u03.l11.teen.add_missing"],
            "IM2021_U03_L11_TEEN_ADD","READY_VERIFIED",Version),
        new("PED:US-CCSS-MATH:G1:U03:L12","number.whole.add_subtract",
            "SUBTRACT_FROM_TEEN_AND_SHAKE_SPILL_COVER",
            ["usccss.g1.u03.l12.teen.take_from","usccss.g1.u03.l12.teen.shake_cover"],
            "IM2021_U03_L12_TEEN_SUBTRACT","READY_VERIFIED",Version),
        new("PED:US-CCSS-MATH:G1:U03:L13","number.whole.add_subtract",
            "TAKE_FROM_RESULT_AND_CHANGE_UNKNOWN_STORIES",
            ["usccss.g1.u03.l13.teen.result_unknown","usccss.g1.u03.l13.teen.change_unknown"],
            "IM2021_U03_L13_TAKE_FROM_STORIES","READY_VERIFIED",Version)
    ];
}

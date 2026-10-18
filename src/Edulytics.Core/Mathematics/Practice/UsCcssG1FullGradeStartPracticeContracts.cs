namespace Edulytics.Core.Mathematics.Practice;

/// <summary>New full-grade Grade 1 ordinals 51–55 (U03 L14-L18) only.</summary>
internal static class UsCcssG1FullGradeStartPracticeContracts
{
    public const string Version="us-ccss-g1-full-grade-u03-l14-l18-first-party-practice-v1";
    public static readonly IReadOnlyList<LessonPracticeContract> Reviewed=
    [
        new("PED:US-CCSS-MATH:G1:U03:L14","number.whole.add_subtract",
            "SOURCE_CENTER_ADD_SUBTRACT_WITHIN20",
            ["usccss.g1.u03.l14.center.add_subtract"],
            "IM2021_G1_U03_L14_CENTER_DIGITAL_PORTION","READY_VERIFIED",Version),
        new("PED:US-CCSS-MATH:G1:U03:L15","number.whole.add_subtract",
            "THREE_GROUP_STORY_TWO_ADDENDS_MAKE_TEN",
            ["usccss.g1.u03.l15.story.three_addends_ten"],
            "IM2021_G1_U03_L15_THREE_ADDENDS","READY_VERIFIED",Version),
        new("PED:US-CCSS-MATH:G1:U03:L16","number.whole.add_subtract",
            "THREE_ADDENDS_TWO_SIDED_EQUALITY",
            ["usccss.g1.u03.l16.three_addends.equivalent"],
            "IM2021_G1_U03_L16_EQUALITY","READY_VERIFIED",Version),
        new("PED:US-CCSS-MATH:G1:U03:L17","number.whole.add_subtract",
            "DECOMPOSE_TO_MAKE_TEN",
            ["usccss.g1.u03.l17.make_ten.decompose"],
            "IM2021_G1_U03_L17_MAKE_TEN","READY_VERIFIED",Version),
        new("PED:US-CCSS-MATH:G1:U03:L18","number.whole.add_subtract",
            "EQUIVALENT_ADDITION_PATTERN",
            ["usccss.g1.u03.l18.addition.pattern"],
            "IM2021_G1_U03_L18_EQUIVALENCE","READY_VERIFIED",Version)
    ];
}

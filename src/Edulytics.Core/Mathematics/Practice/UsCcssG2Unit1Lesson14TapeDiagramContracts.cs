namespace Edulytics.Core.Mathematics.Practice;

/// <summary>
/// Original IM2021 Grade 2 U01 L14: bar-to-tape representation with unknown
/// difference and related inverse equations, not falsely category totals.
/// </summary>
internal static class UsCcssG2Unit1Lesson14TapeDiagramContracts
{
    public const string Version="usccss-g2-u01-l14-source-tape-unknown-difference-v1";
    public static readonly IReadOnlyList<LessonPracticeContract> Reviewed=
    [
        new("PED:US-CCSS-MATH:G2:U01:L14",
            "data.categorical.represent_interpret",
            "BAR_TO_TAPE_KNOWN_LARGER_SMALLER_UNKNOWN_DIFFERENCE",
            [
                "usccss.g2.u01.l14.tape_unknown_gap",
                "usccss.g2.u01.l14.tape_addition_missing_part",
                "usccss.g2.u01.l14.tape_subtraction_missing_part"
            ],
            "IM2021_G2_U01_L14_ORIGINAL_BAR_TO_TAPE_DIFF",
            "READY_VERIFIED",Version)
    ];
}

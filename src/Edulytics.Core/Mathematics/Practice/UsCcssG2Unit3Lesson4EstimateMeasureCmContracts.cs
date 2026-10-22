namespace Edulytics.Core.Mathematics.Practice;
/// <summary>Grade2 U03 L04 ruler zero offset, segment length, iteration and estimates.</summary>
internal static class UsCcssG2Unit3Lesson4EstimateMeasureCmContracts
{
    public const string Version="usccss-g2-u03-l04-estimate-centimeters-v1";
    public static readonly IReadOnlyList<LessonPracticeContract> Reviewed=[
        new("PED:US-CCSS-MATH:G2:U03:L04","measurement.length.centimeter",
            "RULER_MEASURE_AND_ESTIMATE_CENTIMETER_ZERO_TICK",
            [
                "usccss.g2.u03.l04.cm_segment_zero_offset",
                "usccss.g2.u03.l04.cm_segment_shifted_mark",
                "usccss.g2.u03.l04.cm_ruler_iteration",
                "usccss.g2.u03.l04.cm_estimate_about_right"
            ],
            "IM2021_G2_U03_L04_WODB_A_B_D_4CM_C_INVALID_AND_REAL_OBJECTS",
            "READY_VERIFIED",Version)
    ];
}

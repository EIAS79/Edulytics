namespace Edulytics.Core.Mathematics.Practice;
/// <summary>G5 U01 L04 only full-dimension prisms are auto-scored.
/// Incomplete figure inference, estimation and physical models require teacher.</summary>
internal static class UsCcssG5Unit1Lesson4PracticeContracts
{
    public const string Version="usccss-g5-u01-l04-source-layers-v1";
    public static readonly IReadOnlyList<LessonPracticeContract> Reviewed=
    [
        new("PED:US-CCSS-MATH:G5:U01:L04","geometry.volume",
            "FULL_PRISM_LAYERS_COMPLETE_DIMENSIONS_ONLY",
            ["usccss.g5.u01.l04.full_prism_layer_groups"],"IM2021_G5_U01_L04_SOURCE_FULL_LAYER_EXAMPLES",
            "READY_VERIFIED",Version)
    ];
}

namespace Edulytics.Core.Mathematics.Practice;

/// <summary>
/// Source-skill numerical analogues for original IM Grade 2 U01 L08.
/// Category counts/combining/comparing and whether a picture graph can answer
/// a question; graph drawings, student reasoning and teacher responses are
/// separate and never inferred from automated scores.
/// </summary>
internal static class UsCcssG2Unit1Lesson8PictureGraphContracts
{
    public const string Version = "usccss-g2-u01-l08-picture-graphs-v1";
    public static readonly IReadOnlyList<LessonPracticeContract> Reviewed =
    [
        new("PED:US-CCSS-MATH:G2:U01:L08",
            "data.categorical.represent_interpret",
            "SINGLE_UNIT_PICTURE_GRAPH_READ_COMPARE_ANSWERABILITY",
            [
                "usccss.g2.u01.l08.read_pictograph_category",
                "usccss.g2.u01.l08.combine_or_compare_categories",
                "usccss.g2.u01.l08.graph_question_answerable"
            ],
            "IM2021_G2_U01_L08_INTERPRET_PICTURE_GRAPH",
            "READY_VERIFIED",Version)
    ];
}

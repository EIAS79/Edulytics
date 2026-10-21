namespace Edulytics.Core.Mathematics.Practice;

/// <summary>
/// Original 2021 G2 U01 L11 source goals: author graph questions and
/// answer numerical subskills, with actual learner-generated questions
/// teacher-reviewed using companion activity and rubric.
/// </summary>
internal static class UsCcssG2Unit1Lesson11GraphQuestionsContracts
{
    public const string Version="usccss-g2-u01-l11-source-question-writing-v1";
    public static readonly IReadOnlyList<LessonPracticeContract> Reviewed=
    [
        new("PED:US-CCSS-MATH:G2:U01:L11",
            "data.categorical.represent_interpret",
            "TWO_GRAPHS_QUESTION_COUNT_COMBINE_COMPARE",
            [
                "usccss.g2.u01.l11.graph_question_count",
                "usccss.g2.u01.l11.graph_question_combine",
                "usccss.g2.u01.l11.graph_question_compare"
            ],
            "IM2021_G2_U01_L11_STUDENT_AUTHORED_FOUR_GRAPH_QUESTIONS",
            "READY_VERIFIED",Version)
    ];
}

namespace Edulytics.Core.Mathematics.Practice;

/// <summary>2021 original Kendall Hunt Grade 1 ordinals 36–40. The optional
/// source lesson 37 is a real inventory entry and cannot be skipped.</summary>
internal static class UsCcssG1NextSourceFivePracticeContracts
{
    public const string Version="us-ccss-g1-008-original-source-five-practice-v1";
    public static readonly IReadOnlyList<LessonPracticeContract> Reviewed=
    [
        new("PED:US-CCSS-MATH:G1:U02:L21","number.whole.add_subtract",
            "NUMBER_PUZZLES_UNIQUE_DIGITS_AND_RELATED_EQUATIONS",
            ["usccss.g1.u02.l21.number_puzzles.distinct_cards",
             "usccss.g1.u02.l21.related_missing_values"],
            "IM2021_U02_L21_NUMBER_PUZZLES", "READY_VERIFIED",Version),
        new("PED:US-CCSS-MATH:G1:U02:L22","number.whole.add_subtract",
            "SURVEY_DATA_EQUATIONS_AND_STUDENT_QUESTIONS",
            ["usccss.g1.u02.l22.survey.three_equations",
             "usccss.g1.u02.l22.survey.compare_question"],
            "IM2021_U02_L22_OPTIONAL_DATA_MODEL", "READY_VERIFIED",Version),
        new("PED:US-CCSS-MATH:G1:U03:L01","number.whole.add_subtract",
            "ADDITION_FAVORITES_AND_FORMATIVE_FLUENCY_SELF_CHECK",
            ["usccss.g1.u03.l01.sums.fluency_selfcheck",
             "usccss.g1.u03.l01.sums.favorite_and_reason"],
            "IM2021_U03_L01_SUMS_I_KNOW", "READY_VERIFIED",Version),
        new("PED:US-CCSS-MATH:G1:U03:L02","number.whole.add_subtract",
            "COUNT_ON_AND_COMMUTATIVE_ADDEND_COMPARISON",
            ["usccss.g1.u03.l02.count_on.start_steps_total",
             "usccss.g1.u03.l02.commutativity.two_equations"],
            "IM2021_U03_L02_COUNT_ON_SHAKE_SPILL", "READY_VERIFIED",Version),
        new("PED:US-CCSS-MATH:G1:U03:L03","number.whole.add_subtract",
            "EQUAL_EXPRESSIONS_SORT_AND_TRUE_FALSE",
            ["usccss.g1.u03.l03.expressions.compare_both_sides",
             "usccss.g1.u03.l03.expressions.sort_equal_sums"],
            "IM2021_U03_L03_EXPRESSIONS_EQUALITY", "READY_VERIFIED",Version)
    ];
}

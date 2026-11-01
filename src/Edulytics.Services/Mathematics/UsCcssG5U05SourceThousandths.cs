using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Source-specific Grade5 IM2021 U05:L01 thousandths selected choices.
/// Reuses ExactSkillContractQuestionEngine; this is only a bounded lesson adapter.
/// Picture/tiling representations and open statements remain TEACHER_REVIEW_REQUIRED.
/// Written tests are NOT_RUN; not a source picture replacement.
/// </summary>
public static class UsCcssG5U05SourceThousandths
{
    public const string Family="usccss.g5.u05.l01.original_thousandth_decimal_units";
    public sealed record Problem(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    private sealed record SourceCase(string Family,string Context,string Answer,string OtherA,string OtherB);
    private static readonly SourceCase[] Cases =
    [
        new(Family,"Original source: What do you know about one tenth? Choose the decimal for 1/10 of the SAME whole.",
            "0.1","0.01","0.001"),
        new(Family,"Original source: What do you know about one hundredth? Choose the decimal for 1/100 of the SAME whole.",
            "0.01","0.1","0.001"),
        new(Family,"Original source table: 1/1000 = ? as a decimal of the SAME whole.",
            "0.001","0.01","0.1"),
        new(Family,"In the original hundredths grid, ONE of 100 cells is divided into TEN equal pieces. One such part of the ORIGINAL whole is:",
            "0.001","0.01","0.1"),
        new(Family,"Original place value relationship: ten equal thousandths make how much of the original whole?",
            "0.01","0.001","0.1"),
        new(Family,"Original place value relationship: one whole contains how many equal one-thousandth units?",
            "1000","100","10")
        new("usccss.g5.u05.l02.original_grid_thousandth_values", "Original IM2021 U05:L02 Activity1: source A fraction 2/10 represented by a grid. A source figure other than explicitly described D must be physically reviewed by teacher", "0.2", "0.02", "0.002"),
        new("usccss.g5.u05.l02.original_grid_thousandth_values", "Original IM2021 U05:L02 Activity1: source grid B decimal 0.2. A source figure other than explicitly described D must be physically reviewed by teacher", "0.2", "0.02", "0.12"),
        new("usccss.g5.u05.l02.original_grid_thousandth_values", "Original IM2021 U05:L02 Activity1: source grid C fraction 15/100. A source figure other than explicitly described D must be physically reviewed by teacher", "0.15", "0.015", "0.105"),
        new("usccss.g5.u05.l02.original_grid_thousandth_values", "Original IM2021 U05:L02 Activity1: source grid D decimal 0.34. A source figure other than explicitly described D must be physically reviewed by teacher", "0.34", "0.034", "0.304"),
        new("usccss.g5.u05.l02.original_grid_thousandth_values", "Original IM2021 U05:L02 Activity1: source diagram D explicitly describes 62 hundredth cells shaded plus five tenth-strips of one more hundredth cell. A source figure other than explicitly described D must be physically reviewed by teacher", "0.625", "0.62", "0.065"),
        new("usccss.g5.u05.l02.original_grid_thousandth_values", "Original IM2021 U05:L02 Activity1: source instruction shade 0.328 on hundredth grid with thousandth stripes. A source figure other than explicitly described D must be physically reviewed by teacher", "0.328", "0.32", "0.038"),
        new("usccss.g5.u05.l02.original_fifteen_hundredths_critic", "Original source Activity2: Jada says fifteen hundredths. Distinguish correct quantitative content from a pupil's verbal claims; teacher reviews critique", "0.15", "0.015", "0.105"),
        new("usccss.g5.u05.l02.original_fifteen_hundredths_critic", "Original source Activity2: Priya says one hundred fifty thousandths. Distinguish correct quantitative content from a pupil's verbal claims; teacher reviews critique", "0.15", "0.015", "0.105"),
        new("usccss.g5.u05.l02.original_fifteen_hundredths_critic", "Original source Activity2: Tyler says fifteen THOUSANDTHS, a different quantity than the source shaded fifteen hundredths. Distinguish correct quantitative content from a pupil's verbal claims; teacher reviews critique", "0.015", "0.15", "0.150"),
        new("usccss.g5.u05.l02.original_fifteen_hundredths_critic", "Original source Activity2: Diego says one tenth and five hundredths. Distinguish correct quantitative content from a pupil's verbal claims; teacher reviews critique", "0.15", "0.015", "0.105"),
        new("usccss.g5.u05.l02.original_fifteen_hundredths_critic", "Original source Activity2: Mai says one tenth and one half of a tenth. Distinguish correct quantitative content from a pupil's verbal claims; teacher reviews critique", "0.15", "0.015", "0.105"),
        new("usccss.g5.u05.l03.original_decimal_expanded_forms", "Original IM2021 G5 U05:L03 source original Activity1 shaded (4×0.1)+(1×0.01)+(9×0.001). Teacher checks the actual grid and original written explanation", "0.419", "0.491", "0.0419"),
        new("usccss.g5.u05.l03.original_decimal_expanded_forms", "Original IM2021 G5 U05:L03 source original Activity1 shade (8×0.1)+(3×0.01)+(5×0.001). Teacher checks the actual grid and original written explanation", "0.835", "0.853", "0.0835"),
        new("usccss.g5.u05.l03.original_decimal_expanded_forms", "Original IM2021 G5 U05:L03 source Activity2 fraction477/1000. Teacher checks the actual grid and original written explanation", "0.477", "0.0477", "0.4077"),
        new("usccss.g5.u05.l03.original_decimal_expanded_forms", "Original IM2021 G5 U05:L03 source Activity2 one hundred thirty-six thousandths. Teacher checks the actual grid and original written explanation", "0.136", "0.0136", "0.1063"),
        new("usccss.g5.u05.l03.original_decimal_expanded_forms", "Original IM2021 G5 U05:L03 source Activity2 (3×0.1)+(6×0.01)+(8×0.001). Teacher checks the actual grid and original written explanation", "0.368", "0.386", "0.0368"),
        new("usccss.g5.u05.l03.original_decimal_expanded_forms", "Original IM2021 G5 U05:L03 correct source 0.105 in expanded notation (1×0.1)+(0×0.01)+(5×0.001). Teacher checks the actual grid and original written explanation", "0.105", "0.15", "0.015"),
        new("usccss.g5.u05.l03.source_wodb_and_mai_error", "Original source U05:L03 source Which One Doesn't Belong expression 26÷100. WODB choice and Mai's critique require TEACHER_REVIEW_REQUIRED", "0.26", "0.026", "0.0026"),
        new("usccss.g5.u05.l03.source_wodb_and_mai_error", "Original source U05:L03 source Which One Doesn't Belong expression 26×0.001. WODB choice and Mai's critique require TEACHER_REVIEW_REQUIRED", "0.026", "0.26", "0.0026"),
        new("usccss.g5.u05.l03.source_wodb_and_mai_error", "Original source U05:L03 source Which One Doesn't Belong expression (2×0.1)+(6×0.01). WODB choice and Mai's critique require TEACHER_REVIEW_REQUIRED", "0.26", "0.026", "0.206"),
        new("usccss.g5.u05.l03.source_wodb_and_mai_error", "Original source U05:L03 source Mai's INCORRECT claim (1×0.1)+(5×0.01) is equivalent to0.105; calculate her expression instead. WODB choice and Mai's critique require TEACHER_REVIEW_REQUIRED", "0.15", "0.105", "0.015"),
        new("usccss.g5.u05.l04.source_gold_weight_conversions", "Original G5 U05:L04 source gold-weight problem: two 0.1-ounce weights balance the source 0.2-ounce gold nugget. Choice validates selected value, physical balance and other valid configurations require teacher", "0.2", "0.02", "0.002"),
        new("usccss.g5.u05.l04.source_gold_weight_conversions", "Original G5 U05:L04 source gold-weight problem: source 266 units of weight0.001 ounce each. Choice validates selected value, physical balance and other valid configurations require teacher", "0.266", "2.66", "0.0266"),
        new("usccss.g5.u05.l04.source_gold_weight_conversions", "Original G5 U05:L04 source gold-weight problem: source 150 units of0.01 ounces each. Choice validates selected value, physical balance and other valid configurations require teacher", "1.5", "0.15", "15"),
        new("usccss.g5.u05.l04.source_gold_weight_conversions", "Original G5 U05:L04 source gold-weight problem: source 27 units of0.1 ounce each. Choice validates selected value, physical balance and other valid configurations require teacher", "2.7", "0.27", "27"),
        new("usccss.g5.u05.l04.source_gold_weight_conversions", "Original G5 U05:L04 source gold-weight problem: source three tenths, five hundredths, eight thousandths of an ounce. Choice validates selected value, physical balance and other valid configurations require teacher", "0.358", "0.385", "0.0358"),
        new("usccss.g5.u05.l04.source_gold_weight_conversions", "Original G5 U05:L04 source gold-weight problem: source six tenths and two thousandths ounce. Choice validates selected value, physical balance and other valid configurations require teacher", "0.602", "0.62", "0.062"),
        new("usccss.g5.u05.l04.source_gold_weight_conversions", "Original G5 U05:L04 source gold-weight problem: source six tenths and two hundredths ounce. Choice validates selected value, physical balance and other valid configurations require teacher", "0.62", "0.602", "0.062"),
        new("usccss.g5.u05.l04.source_gold_weight_conversions", "Original G5 U05:L04 source gold-weight problem: source two hundred eighty-three thousandths ounce. Choice validates selected value, physical balance and other valid configurations require teacher", "0.283", "0.0283", "2.83"),
        new("usccss.g5.u05.l04.source_gold_weight_conversions", "Original G5 U05:L04 source gold-weight problem: source four hundred nine thousandths ounce. Choice validates selected value, physical balance and other valid configurations require teacher", "0.409", "0.049", "4.09"),
        new("usccss.g5.u05.l04.source_gold_weight_conversions", "Original G5 U05:L04 source gold-weight problem: source original gold0.527 ounce represented as5 tenths+2 hundredths+7 thousandths. Choice validates selected value, physical balance and other valid configurations require teacher", "0.527", "0.572", "0.0527"),
        new("usccss.g5.u05.l04.source_gold_weight_conversions", "Original G5 U05:L04 source gold-weight problem: source0.385 ounce MINIMUM count of available0.1,0.01,0.001 weight pieces. Choice validates selected value, physical balance and other valid configurations require teacher", "16", "385", "25"),
        new("usccss.g5.u05.l04.source_gold_weight_conversions", "Original G5 U05:L04 source gold-weight problem: source0.385 ounce MAXIMUM count using all0.001 ounce pieces. Choice validates selected value, physical balance and other valid configurations require teacher", "385", "16", "38"),
        new("usccss.g5.u05.l04.source_place_value_nugget_ratios", "Original source place-value weights: one 0.1-ounce weight balanced using0.01-ounce pieces", "10", "100", "1"),
        new("usccss.g5.u05.l04.source_place_value_nugget_ratios", "Original source place-value weights: one0.1-ounce weight balanced using0.001-ounce pieces", "100", "10", "1000"),
        new("usccss.g5.u05.l04.source_place_value_nugget_ratios", "Original source place-value weights: source gram-weight A0.6 compared to B0.06, A/B", "10", "100", "0.1"),
        new("usccss.g5.u05.l04.source_place_value_nugget_ratios", "Original source place-value weights: source gram-weight A0.6 compared to C0.006, A/C", "100", "10", "0.01"),
        new("usccss.g5.u05.l04.source_place_value_nugget_ratios", "Original source place-value weights: source gram-weight C0.006 compared to B0.06, C/B", "0.1", "10", "0.01"),
        new("usccss.g5.u05.l04.source_place_value_nugget_ratios", "Original source place-value weights: source gram-weight C0.006 compared to A0.6, C/A", "0.01", "100", "0.1"),
        new("usccss.g5.u05.l05.original_decimal_true_false", "Original source U05:L05 warmup true/false: 7.06 = 7.006. Explain using same place value, not number of printed digits", "false", "true", "not enough information"),
        new("usccss.g5.u05.l05.original_decimal_true_false", "Original source U05:L05 warmup true/false: 7.06 = 7.060. Explain using same place value, not number of printed digits", "true", "false", "not enough information"),
        new("usccss.g5.u05.l05.original_decimal_true_false", "Original source U05:L05 warmup true/false: 7.06 = 7.600. Explain using same place value, not number of printed digits", "false", "true", "not enough information"),
        new("usccss.g5.u05.l05.original_frisbee_swim_comparisons", "Original source Diego threw frisbee 5.10 meters and Jada threw it5.01 meters. Who threw FARTHER? Teacher explains distance comparison.", "Diego", "Jada", "tie"),
        new("usccss.g5.u05.l05.original_frisbee_swim_comparisons", "Original source Tyler swam length in35.15 seconds and Han in35.30 seconds. Who was FASTER? Smaller time is faster.", "Tyler", "Han", "tie"),
        new("usccss.g5.u05.l05.original_frisbee_swim_comparisons", "Original source: which decimal is strictly BETWEEN 5 and 5.01, as Priya's throw requires? EDULYTIKS illustrative choice only, source accepts many answers", "5.005", "5.01", "5.1"),
        new("usccss.g5.u05.l06.original_number_line_comparison", "Original 2021 U05:L06 source number-line comparison 0.2 ? 0.02. Use a consistent scale to thousandths where needed", ">", "<", "="),
        new("usccss.g5.u05.l06.original_number_line_comparison", "Original 2021 U05:L06 source number-line comparison 0.3 ? 0.14. Use a consistent scale to thousandths where needed", ">", "<", "="),
        new("usccss.g5.u05.l06.original_number_line_comparison", "Original 2021 U05:L06 source number-line comparison 0.23 ? 0.216. Use a consistent scale to thousandths where needed", ">", "<", "="),
        new("usccss.g5.u05.l06.original_number_line_comparison", "Original 2021 U05:L06 source number-line comparison 0.534 ? 0.537. Use a consistent scale to thousandths where needed", "<", ">", "="),
        new("usccss.g5.u05.l06.original_nested_line_place_values", "Original source nested number-lines: source one thousandth; exact visual placement and tick labels teacher-reviewed", "0.001", "0.01", "0.1"),
        new("usccss.g5.u05.l06.original_nested_line_place_values", "Original source nested number-lines: source one hundredth; exact visual placement and tick labels teacher-reviewed", "0.01", "0.001", "0.1"),
        new("usccss.g5.u05.l06.original_nested_line_place_values", "Original source nested number-lines: source one tenth; exact visual placement and tick labels teacher-reviewed", "0.1", "0.01", "0.001"),
        new("usccss.g5.u05.l06.original_nested_line_place_values", "Original source nested number-lines: original lower number-line smallest tick interval, thousandth; exact visual placement and tick labels teacher-reviewed", "0.001", "0.01", "0.1"),
        new("usccss.g5.u05.l06.original_nested_line_place_values", "Original source nested number-lines: original middle number-line tick interval, hundredth; exact visual placement and tick labels teacher-reviewed", "0.01", "0.1", "0.001"),
        new("usccss.g5.u05.l06.original_nested_line_place_values", "Original source nested number-lines: original top number-line tick interval, tenth; exact visual placement and tick labels teacher-reviewed", "0.1", "0.01", "0.001"),
        new("usccss.g5.u05.l07.original_doubloon_rounding", "Original G5 U05:L07 historical gold doubloon printed source before1728 weight6.867 grams rounded to nearest TENTH; teacher reviews number-line diagram and which scale can distinguish before versus after", "6.9", "6.8", "6.87"),
        new("usccss.g5.u05.l07.original_doubloon_rounding", "Original G5 U05:L07 historical gold doubloon printed source after1728 weight6.766 grams rounded to nearest TENTH; teacher reviews number-line diagram and which scale can distinguish before versus after", "6.8", "6.7", "6.77"),
        new("usccss.g5.u05.l07.original_doubloon_rounding", "Original G5 U05:L07 historical gold doubloon printed source before1728 weight6.867 grams rounded to nearest HUNDREDTH; teacher reviews number-line diagram and which scale can distinguish before versus after", "6.87", "6.86", "6.8"),
        new("usccss.g5.u05.l07.original_doubloon_rounding", "Original G5 U05:L07 historical gold doubloon printed source after1728 weight6.766 grams rounded to nearest HUNDREDTH; teacher reviews number-line diagram and which scale can distinguish before versus after", "6.77", "6.76", "6.8"),
        new("usccss.g5.u05.l07.original_doubloon_rounding", "Original G5 U05:L07 historical gold doubloon printed source before1728 weight6.867 rounded to nearest WHOLE gram; teacher reviews number-line diagram and which scale can distinguish before versus after", "7", "6", "6.9"),
        new("usccss.g5.u05.l07.original_doubloon_rounding", "Original G5 U05:L07 historical gold doubloon printed source after1728 weight6.766 rounded to nearest WHOLE gram; teacher reviews number-line diagram and which scale can distinguish before versus after", "7", "6", "6.8"),
        new("usccss.g5.u05.l07.original_exact_vs_approximate", "Original source Activity2 classify whether quantity is EXACT or an ESTIMATE/approximate measurement: 14 pencils actually on a desk", "exact", "estimate", "unknown"),
        new("usccss.g5.u05.l07.original_exact_vs_approximate", "Original source Activity2 classify whether quantity is EXACT or an ESTIMATE/approximate measurement: population of Los Angeles stated as 12,400,000", "estimate", "exact", "unknown"),
        new("usccss.g5.u05.l07.original_exact_vs_approximate", "Original source Activity2 classify whether quantity is EXACT or an ESTIMATE/approximate measurement: 2.4-mile stated school-to-park distance", "estimate", "exact", "unknown"),
        new("usccss.g5.u05.l07.original_exact_vs_approximate", "Original source Activity2 classify whether quantity is EXACT or an ESTIMATE/approximate measurement: runner's 19.78-second recorded time", "estimate", "exact", "unknown"),
        new("usccss.g5.u05.l08.source_round_primary_examples", "Original Grade5 U05:L08 Activity2 round printed 6.273 to nearest WHOLE; exact place-value result", "6", "6.3", "6.27"),
        new("usccss.g5.u05.l08.source_round_primary_examples", "Original Grade5 U05:L08 Activity2 round printed 6.273 to nearest TENTH; exact place-value result", "6.3", "6.27", "6"),
        new("usccss.g5.u05.l08.source_round_primary_examples", "Original Grade5 U05:L08 Activity2 round printed 6.273 to nearest HUNDREDTH; exact place-value result", "6.27", "6", "6.3"),
        new("usccss.g5.u05.l08.source_round_primary_examples", "Original Grade5 U05:L08 Activity2 round printed 4.158 to nearest WHOLE; exact place-value result", "4", "4.2", "4.16"),
        new("usccss.g5.u05.l08.source_round_primary_examples", "Original Grade5 U05:L08 Activity2 round printed 4.158 to nearest TENTH; exact place-value result", "4.2", "4.16", "4"),
        new("usccss.g5.u05.l08.source_round_primary_examples", "Original Grade5 U05:L08 Activity2 round printed 4.158 to nearest HUNDREDTH; exact place-value result", "4.16", "4", "4.2"),
        new("usccss.g5.u05.l08.source_optional_rounding_table", "Original Grade5 U05:L08 optional Activity3 round printed 34.482 to nearest WHOLE; exact place-value result", "34", "34.5", "34.48"),
        new("usccss.g5.u05.l08.source_optional_rounding_table", "Original Grade5 U05:L08 optional Activity3 round printed 34.482 to nearest TENTH; exact place-value result", "34.5", "34.48", "34"),
        new("usccss.g5.u05.l08.source_optional_rounding_table", "Original Grade5 U05:L08 optional Activity3 round printed 34.482 to nearest HUNDREDTH; exact place-value result", "34.48", "34", "34.5"),
        new("usccss.g5.u05.l08.source_optional_rounding_table", "Original Grade5 U05:L08 optional Activity3 round printed 99.909 to nearest WHOLE; exact place-value result", "100", "99.9", "99.91"),
        new("usccss.g5.u05.l08.source_optional_rounding_table", "Original Grade5 U05:L08 optional Activity3 round printed 99.909 to nearest TENTH; exact place-value result", "99.9", "99.91", "100"),
        new("usccss.g5.u05.l08.source_optional_rounding_table", "Original Grade5 U05:L08 optional Activity3 round printed 99.909 to nearest HUNDREDTH; exact place-value result", "99.91", "100", "99.9"),
        new("usccss.g5.u05.l08.source_optional_rounding_table", "Original Grade5 U05:L08 optional Activity3 round printed 5.555 to nearest WHOLE; exact place-value result", "6", "5.6", "5.56"),
        new("usccss.g5.u05.l08.source_optional_rounding_table", "Original Grade5 U05:L08 optional Activity3 round printed 5.555 to nearest TENTH; exact place-value result", "5.6", "5.56", "6"),
        new("usccss.g5.u05.l08.source_optional_rounding_table", "Original Grade5 U05:L08 optional Activity3 round printed 5.555 to nearest HUNDREDTH; exact place-value result", "5.56", "6", "5.6"),
        new("usccss.g5.u05.l08.source_optional_rounding_table", "Original Grade5 U05:L08 optional Activity3 round printed 19.509 to nearest WHOLE; exact place-value result", "20", "19.5", "19.51"),
        new("usccss.g5.u05.l08.source_optional_rounding_table", "Original Grade5 U05:L08 optional Activity3 round printed 19.509 to nearest TENTH; exact place-value result", "19.5", "19.51", "20"),
        new("usccss.g5.u05.l08.source_optional_rounding_table", "Original Grade5 U05:L08 optional Activity3 round printed 19.509 to nearest HUNDREDTH; exact place-value result", "19.51", "20", "19.5"),
        new("usccss.g5.u05.l09.source_decimal_inequality_truth", "Original U05:L09 warm-up True or False decimal inequality: 0.909 > 0.91", "false", "true", "not enough information"),
        new("usccss.g5.u05.l09.source_decimal_inequality_truth", "Original U05:L09 warm-up True or False decimal inequality: 4.1 < 4.100", "false", "true", "not enough information"),
        new("usccss.g5.u05.l09.source_decimal_inequality_truth", "Original U05:L09 warm-up True or False decimal inequality: 0.99 < 0.999", "true", "false", "not enough information"),
        new("usccss.g5.u05.l09.source_ordering_and_between", "Original 2021 Grade5 L09 open numerical subskill: Original source: give a decimal strictly between786.2 and786.3. A source-between problem admits many valid numbers; examples are Edulytiks choices only", "786.25", "786.2", "786.3"),
        new("usccss.g5.u05.l09.source_ordering_and_between", "Original 2021 Grade5 L09 open numerical subskill: Original source: between9.99 and10. A source-between problem admits many valid numbers; examples are Edulytiks choices only", "9.995", "9.99", "10"),
        new("usccss.g5.u05.l09.source_ordering_and_between", "Original 2021 Grade5 L09 open numerical subskill: Original source: 0.46 > value >0.45. A source-between problem admits many valid numbers; examples are Edulytiks choices only", "0.456", "0.45", "0.46"),
        new("usccss.g5.u05.l09.source_ordering_and_between", "Original 2021 Grade5 L09 open numerical subskill: Original source: between0.5 and0.51. A source-between problem admits many valid numbers; examples are Edulytiks choices only", "0.505", "0.5", "0.51"),
        new("usccss.g5.u05.l09.source_ordering_and_between", "Original 2021 Grade5 L09 open numerical subskill: Original source: between0.99 and0.999. A source-between problem admits many valid numbers; examples are Edulytiks choices only", "0.995", "0.99", "0.999"),
        new("usccss.g5.u05.l09.source_ordering_and_between", "Original 2021 Grade5 L09 open numerical subskill: Original Kiran claims NO value between1.731 and1.732; choose a source-aligned COUNTEREXAMPLE, NOT unique source answer. A source-between problem admits many valid numbers; examples are Edulytiks choices only", "1.7315", "1.731", "1.732"),
        new("usccss.g5.u05.l09.source_ordering_and_between", "Original 2021 Grade5 L09 open numerical subskill: Original source order67.020,67.200,67.002 least to greatest. A source-between problem admits many valid numbers; examples are Edulytiks choices only", "67.002 < 67.020 < 67.200", "67.020 < 67.002 < 67.200", "67.200 < 67.020 < 67.002"),
        new("usccss.g5.u05.l09.source_ordering_and_between", "Original 2021 Grade5 L09 open numerical subskill: Original source order1.101,1.02,1.1 least to greatest. A source-between problem admits many valid numbers; examples are Edulytiks choices only", "1.02 < 1.1 < 1.101", "1.1 < 1.02 < 1.101", "1.101 < 1.1 < 1.02"),
        new("usccss.g5.u05.l09.source_ordering_and_between", "Original 2021 Grade5 L09 open numerical subskill: Original source order0.333,0.323,0.3 least to greatest. A source-between problem admits many valid numbers; examples are Edulytiks choices only", "0.3 < 0.323 < 0.333", "0.323 < 0.3 < 0.333", "0.333 < 0.323 < 0.3"),
        new("usccss.g5.u05.l09.source_ordering_and_between", "Original 2021 Grade5 L09 open numerical subskill: Original source order99.99,99.09,99.091 least to greatest. A source-between problem admits many valid numbers; examples are Edulytiks choices only", "99.09 < 99.091 < 99.99", "99.091 < 99.09 < 99.99", "99.99 < 99.091 < 99.09"),
        new("usccss.g5.u05.l10.source_luge_time_rounding", "Original 2021 L10 luge athlete1 has time 48.532 seconds. Round to nearest WHOLE SECOND. Teacher reviews effect on source ranking", "49", "48", "50"),
        new("usccss.g5.u05.l10.source_luge_time_rounding", "Original 2021 L10 luge athlete1 has time 48.532 seconds. Round to nearest TENTH SECOND. Teacher reviews effect on source ranking", "48.5", "48.6", "48.7"),
        new("usccss.g5.u05.l10.source_luge_time_rounding", "Original 2021 L10 luge athlete1 has time 48.532 seconds. Round to nearest HUNDREDTH SECOND. Teacher reviews effect on source ranking", "48.53", "48.56", "48.63"),
        new("usccss.g5.u05.l10.source_luge_speeds_and_precision", "Original 2021 L10 luge athlete1 speed 82.13 mph rounded to nearest TENTH MPH", "82.1", "82.7", "82.9"),
        new("usccss.g5.u05.l10.source_luge_speeds_and_precision", "Original 2021 L10 luge athlete1 speed 82.13 mph rounded to nearest WHOLE MPH", "82", "83", "84"),
        new("usccss.g5.u05.l10.source_luge_time_rounding", "Original 2021 L10 luge athlete2 has time 48.561 seconds. Round to nearest WHOLE SECOND. Teacher reviews effect on source ranking", "49", "48", "50"),
        new("usccss.g5.u05.l10.source_luge_time_rounding", "Original 2021 L10 luge athlete2 has time 48.561 seconds. Round to nearest TENTH SECOND. Teacher reviews effect on source ranking", "48.6", "48.5", "48.7"),
        new("usccss.g5.u05.l10.source_luge_time_rounding", "Original 2021 L10 luge athlete2 has time 48.561 seconds. Round to nearest HUNDREDTH SECOND. Teacher reviews effect on source ranking", "48.56", "48.53", "48.63"),
        new("usccss.g5.u05.l10.source_luge_speeds_and_precision", "Original 2021 L10 luge athlete2 speed 82.75 mph rounded to nearest TENTH MPH", "82.8", "82.7", "82.9"),
        new("usccss.g5.u05.l10.source_luge_speeds_and_precision", "Original 2021 L10 luge athlete2 speed 82.75 mph rounded to nearest WHOLE MPH", "83", "82", "84"),
        new("usccss.g5.u05.l10.source_luge_time_rounding", "Original 2021 L10 luge athlete3 has time 48.626 seconds. Round to nearest WHOLE SECOND. Teacher reviews effect on source ranking", "49", "48", "50"),
        new("usccss.g5.u05.l10.source_luge_time_rounding", "Original 2021 L10 luge athlete3 has time 48.626 seconds. Round to nearest TENTH SECOND. Teacher reviews effect on source ranking", "48.6", "48.5", "48.7"),
        new("usccss.g5.u05.l10.source_luge_time_rounding", "Original 2021 L10 luge athlete3 has time 48.626 seconds. Round to nearest HUNDREDTH SECOND. Teacher reviews effect on source ranking", "48.63", "48.53", "48.56"),
        new("usccss.g5.u05.l10.source_luge_speeds_and_precision", "Original 2021 L10 luge athlete3 speed 82.81 mph rounded to nearest TENTH MPH", "82.8", "82.7", "82.9"),
        new("usccss.g5.u05.l10.source_luge_speeds_and_precision", "Original 2021 L10 luge athlete3 speed 82.81 mph rounded to nearest WHOLE MPH", "83", "82", "84"),
        new("usccss.g5.u05.l10.source_luge_time_rounding", "Original 2021 L10 luge athlete4 has time 48.634 seconds. Round to nearest WHOLE SECOND. Teacher reviews effect on source ranking", "49", "48", "50"),
        new("usccss.g5.u05.l10.source_luge_time_rounding", "Original 2021 L10 luge athlete4 has time 48.634 seconds. Round to nearest TENTH SECOND. Teacher reviews effect on source ranking", "48.6", "48.5", "48.7"),
        new("usccss.g5.u05.l10.source_luge_time_rounding", "Original 2021 L10 luge athlete4 has time 48.634 seconds. Round to nearest HUNDREDTH SECOND. Teacher reviews effect on source ranking", "48.63", "48.53", "48.56"),
        new("usccss.g5.u05.l10.source_luge_speeds_and_precision", "Original 2021 L10 luge athlete4 speed 83.07 mph rounded to nearest TENTH MPH", "83.1", "82.7", "82.9"),
        new("usccss.g5.u05.l10.source_luge_speeds_and_precision", "Original 2021 L10 luge athlete4 speed 83.07 mph rounded to nearest WHOLE MPH", "83", "82", "84"),
        new("usccss.g5.u05.l10.source_luge_time_rounding", "Original 2021 L10 luge athlete5 has time 48.708 seconds. Round to nearest WHOLE SECOND. Teacher reviews effect on source ranking", "49", "48", "50"),
        new("usccss.g5.u05.l10.source_luge_time_rounding", "Original 2021 L10 luge athlete5 has time 48.708 seconds. Round to nearest TENTH SECOND. Teacher reviews effect on source ranking", "48.7", "48.5", "48.6"),
        new("usccss.g5.u05.l10.source_luge_time_rounding", "Original 2021 L10 luge athlete5 has time 48.708 seconds. Round to nearest HUNDREDTH SECOND. Teacher reviews effect on source ranking", "48.71", "48.53", "48.56"),
        new("usccss.g5.u05.l10.source_luge_speeds_and_precision", "Original 2021 L10 luge athlete5 speed 82.80 mph rounded to nearest TENTH MPH", "82.8", "82.7", "82.9"),
        new("usccss.g5.u05.l10.source_luge_speeds_and_precision", "Original 2021 L10 luge athlete5 speed 82.80 mph rounded to nearest WHOLE MPH", "83", "82", "84"),
        new("usccss.g5.u05.l10.source_luge_time_rounding", "Original luge source: which athlete is FASTEST by smallest unrounded race TIME?", "Athlete 1", "Athlete 4", "Athlete 5"),
        new("usccss.g5.u05.l10.source_luge_time_rounding", "Original source: 48.85 seconds recorded to nearest hundredth: which set of ten possible true thousandth-place readings rounds to48.85 under nearest halves-up?", "48.845 through 48.854", "48.850 through 48.859", "48.840 through 48.849"),
        new("usccss.g5.u05.l10.source_luge_time_rounding", "Source an athlete timed 48.615 seconds to nearest thousandth; what would this report round to nearest HUNDREDTH?", "48.62", "48.61", "48.60"),
        new("usccss.g5.u05.l10.source_luge_speeds_and_precision", "Original source which athlete has greatest TOP SPEED, not smallest race time?", "Athlete 4", "Athlete 1", "Athlete 3"),
        new("usccss.g5.u05.l10.source_luge_speeds_and_precision", "Original source sixth athlete speed strictly between82.80mph and82.81mph, choose one valid illustrative thousandth-place speed", "82.805", "82.800", "82.810"),
        // END_G5_U05_SOURCE_CASES
    ];
    public static bool Supports(string? family)=>
        !string.IsNullOrWhiteSpace(family) &&
        Cases.Any(c=>string.Equals(c.Family,family,StringComparison.Ordinal));
    private static bool Valid(string? family,IReadOnlyDictionary<string,int>? p,out SourceCase? c)
    {
        c=null;
        if(!Supports(family)||p is null||p.Count!=2||
           !p.TryGetValue("case",out int i)||i<0||i>=Cases.Count(x=>x.Family==family)||
           !p.TryGetValue("slot",out int s)||s is <0 or >2)return false;
        c=Cases.Where(x=>x.Family==family).ElementAt(i);return true;
    }
    public static Problem Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unknown source thousandths family.");
        ArgumentNullException.ThrowIfNull(random);
        var i=random.Next(Cases.Count(x=>x.Family==family));
        int slot=random.Next(3);
        IReadOnlyDictionary<string,int> p=new Dictionary<string,int>(StringComparer.Ordinal)
            {["case"]=i,["slot"]=slot};
        if(!Valid(family,p,out var c)||c is null)throw new InvalidOperationException("Invalid source decimal case.");
        string[] options=slot switch
        {
            0=>[c.Answer,c.OtherA,c.OtherB],
            1=>[c.OtherA,c.Answer,c.OtherB],
            _=>[c.OtherA,c.OtherB,c.Answer]
        };
        string prompt=$"{c.Context} Select 1: {options[0]}; 2: {options[1]}; 3: {options[2]}. "+
            "For drawings, shaded mosaics and original poster explanations: TEACHER_REVIEW_REQUIRED.";
        string solution=$"Correct source-aligned selected answer is {c.Answer}. "+
            "Use the exact original source units, place value, and context; "+
            "teacher reviews the student's diagrams and explanation.";
        return new Problem(family,prompt,solution,AssessmentItemType.Numeric,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p,out _))throw new InvalidOperationException("Malformed source thousandths parameters.");
        return (p["slot"]+1).ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int>? p,string? answer)=>
        Valid(family,p,out _) &&
        string.Equals(answer,(p!["slot"]+1).ToString(CultureInfo.InvariantCulture),StringComparison.Ordinal);
}

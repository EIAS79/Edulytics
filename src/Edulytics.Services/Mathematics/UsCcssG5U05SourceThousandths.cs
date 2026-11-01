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

using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Reusable, STRICTLY source-bounded Grade 5 Unit 03 fraction division question
/// adapter. This is NOT a new shared engine: existing ExactSkillContractQuestionEngine
/// delegates only registered lesson-selected source families to it.
/// Open diagrams, discourse and teacher-scored source tasks remain human reviewed.
/// Test fixtures are authored only (NOT_RUN).
/// </summary>
public static class UsCcssG5U03SourceDivisionQuestions
{
    public sealed record Problem(string Family, string Prompt, string Solution,
        AssessmentItemType ItemType, IReadOnlyDictionary<string, int> Parameters);
    private sealed record SourceCase(string Family, string Context, string Expression,
        int Numerator, int Denominator, string Units);
    private static readonly SourceCase[] Cases =
    [
        new("usccss.g5.u03.l11.mac_cheese_half_pan_equal_shares",
            "Jada and her two sisters equally share HALF the original pan of macaroni and cheese among THREE people",
            "(1/2) / 3", 1,6,"of the whole pan"),
        new("usccss.g5.u03.l11.mac_cheese_half_pan_equal_shares",
            "FOUR people share HALF the original pan equally",
            "(1/2) / 4",1,8,"of the whole pan"),
        new("usccss.g5.u03.l11.mac_cheese_half_pan_equal_shares",
            "FIVE people share HALF the original pan equally",
            "(1/2) / 5",1,10,"of the whole pan"),
        new("usccss.g5.u03.l11.four_divisor_number_talk",
            "Original Grade 5 U03:L11 warm-up, 72 divided by 4","72 / 4",18,1,""),
        new("usccss.g5.u03.l11.four_divisor_number_talk",
            "Original Grade 5 U03:L11 warm-up, 36 divided by 4","36 / 4",9,1,""),
        new("usccss.g5.u03.l11.four_divisor_number_talk",
            "Original Grade 5 U03:L11 warm-up, 4 divided by 4","4 / 4",1,1,""),
        new("usccss.g5.u03.l11.four_divisor_number_talk",
            "Original Grade 5 U03:L11 warm-up, 1 divided by 4","1 / 4",1,4,""),
        new("usccss.g5.u03.l12.third_division_teacher_diagrams", "Priya and Mai's original two diagrams represent one third of the SAME WHOLE shared among four", "(1/3) ÷ 4", 1, 12, "of the original whole"),
        new("usccss.g5.u03.l12.third_division_teacher_diagrams", "Han draws one third of a same original whole divided into three equal parts", "(1/3) ÷ 3", 1, 9, "of the original whole"),
        new("usccss.g5.u03.l12.third_division_teacher_diagrams", "Priya's source mistake says 1/3 ÷ 2 = 1/2; the correct answer is 1/6 of the original WHOLE", "(1/3) ÷ 2", 1, 6, "of the original whole"),
        new("usccss.g5.u03.l12.quarter_division_pattern", "Original Grade 5 L12 source Activity 3 quarter-of-a-whole shared among 2 groups", "(1/4) ÷ 2", 1, 8, "of the original whole"),
        new("usccss.g5.u03.l12.quarter_division_pattern", "Original Grade 5 L12 source Activity 3 quarter-of-a-whole shared among 3 groups", "(1/4) ÷ 3", 1, 12, "of the original whole"),
        new("usccss.g5.u03.l12.quarter_division_pattern", "Original Grade 5 L12 source Activity 3 quarter-of-a-whole shared among 4 groups", "(1/4) ÷ 4", 1, 16, "of the original whole"),
        new("usccss.g5.u03.l13.two_foot_paper_strip_unit_fraction_groups", "Original source paper strip 2 feet long is cut into segments each 1/2 foot. Teacher checks colored paper and physical cut lengths", "2 ÷ (1/2)", 4, 1, "equal strip pieces"),
        new("usccss.g5.u03.l13.two_foot_paper_strip_unit_fraction_groups", "Original source paper strip 2 feet long is cut into segments each 1/3 foot. Teacher checks colored paper and physical cut lengths", "2 ÷ (1/3)", 6, 1, "equal strip pieces"),
        new("usccss.g5.u03.l13.two_foot_paper_strip_unit_fraction_groups", "Original source paper strip 2 feet long is cut into segments each 1/4 foot. Teacher checks colored paper and physical cut lengths", "2 ÷ (1/4)", 8, 1, "equal strip pieces"),
        new("usccss.g5.u03.l13.two_foot_paper_strip_unit_fraction_groups", "Original source paper strip 2 feet long is cut into segments each 1/6 foot. Teacher checks colored paper and physical cut lengths", "2 ÷ (1/6)", 12, 1, "equal strip pieces"),
        new("usccss.g5.u03.l13.two_foot_paper_strip_unit_fraction_groups", "Original source paper strip 3 feet long is cut into segments each 1/6 foot. Teacher checks colored paper and physical cut lengths", "3 ÷ (1/6)", 18, 1, "equal strip pieces"),
        new("usccss.g5.u03.l14.same_divisor_quarter_pattern", "Original Set A 3 whole units divided into quarters", "3 ÷ (1/4)", 12, 1, "quarter-unit groups"),
        new("usccss.g5.u03.l14.same_divisor_quarter_pattern", "Original Set A 4 whole units divided into quarters", "4 ÷ (1/4)", 16, 1, "quarter-unit groups"),
        new("usccss.g5.u03.l14.same_divisor_quarter_pattern", "Original Set A 5 whole units divided into quarters", "5 ÷ (1/4)", 20, 1, "quarter-unit groups"),
        new("usccss.g5.u03.l14.same_divisor_quarter_pattern", "Original Set A 6 whole units divided into quarters", "6 ÷ (1/4)", 24, 1, "quarter-unit groups"),
        new("usccss.g5.u03.l14.same_dividend_three_pattern", "Original Set B 3 whole units divided into 1/2 sized groups", "3 ÷ (1/2)", 6, 1, "groups"),
        new("usccss.g5.u03.l14.same_dividend_three_pattern", "Original Set B 3 whole units divided into 1/3 sized groups", "3 ÷ (1/3)", 9, 1, "groups"),
        new("usccss.g5.u03.l14.same_dividend_three_pattern", "Original Set B 3 whole units divided into 1/4 sized groups", "3 ÷ (1/4)", 12, 1, "groups"),
        new("usccss.g5.u03.l14.same_dividend_three_pattern", "Original Set B 3 whole units divided into 1/5 sized groups", "3 ÷ (1/5)", 15, 1, "groups"),
        new("usccss.g5.u03.l14.snack_servings_story_selection", "Original source popcorn cups: 3 whole units, 1/4 per serving", "3 ÷ (1/4)", 12, 1, "servings"),
        new("usccss.g5.u03.l14.snack_servings_story_selection", "Original source orange juice liters: 2 whole units, 1/4 per serving", "2 ÷ (1/4)", 8, 1, "servings"),
        new("usccss.g5.u03.l14.snack_servings_story_selection", "Original source granola cups: 5 whole units, 1/2 per serving", "5 ÷ (1/2)", 10, 1, "servings"),
        new("usccss.g5.u03.l15.source_four_division_number_talk", "Original source L15 warmup six items shared among three", "6 ÷ 3", 2, 1, "items per group"),
        new("usccss.g5.u03.l15.source_four_division_number_talk", "Original source L15 warmup six wholes divided into third-sized groups", "6 ÷ (1/3)", 18, 1, "groups"),
        new("usccss.g5.u03.l15.source_four_division_number_talk", "Original source L15 warmup one third shared among six people", "(1/3) ÷ 6", 1, 18, "of original whole"),
        new("usccss.g5.u03.l15.source_four_division_number_talk", "Original source L15 warmup one third shared among twelve people", "(1/3) ÷ 12", 1, 36, "of original whole"),
        new("usccss.g5.u03.l15.teacher_authored_card_sort_equivalents", "Edulytiks-authored non-publisher card: three meters strip into quarter-meter equal pieces", "3 ÷ (1/4)", 12, 1, "pieces"),
        new("usccss.g5.u03.l15.teacher_authored_card_sort_equivalents", "Edulytiks-authored non-publisher card: one-quarter of paper shared among three children", "(1/4) ÷ 3", 1, 12, "of original sheet"),
        new("usccss.g5.u03.l15.teacher_authored_card_sort_equivalents", "Edulytiks-authored unmatched-card story: two whole meters into fifth-meter groups", "2 ÷ (1/5)", 10, 1, "pieces"),
        new("usccss.g5.u03.l15.teacher_authored_card_sort_equivalents", "Edulytiks-authored reversed story: one fifth meter shared among two children", "(1/5) ÷ 2", 1, 10, "meter per child"),
        new("usccss.g5.u03.l16.source_twenty_five_fraction_quotients", "Original source greater-than-1: twenty-five whole units divided into fifths", "25 ÷ (1/5)", 125, 1, "groups"),
        new("usccss.g5.u03.l16.source_twenty_five_fraction_quotients", "Original source less-than-1: one seventh divided among twenty-five", "(1/7) ÷ 25", 1, 175, "of original whole"),
        new("usccss.g5.u03.l16.source_twenty_five_fraction_quotients", "Original source less-than-1: one eighth divided among twenty-five", "(1/8) ÷ 25", 1, 200, "of original whole"),
        new("usccss.g5.u03.l16.source_twenty_five_fraction_quotients", "Original source greater-than-1: twenty-five whole units divided into sevenths", "25 ÷ (1/7)", 175, 1, "groups"),
        new("usccss.g5.u03.l16.source_twenty_five_fraction_quotients", "Original source greater-than-1: twenty-five whole units divided into eighths", "25 ÷ (1/8)", 200, 1, "groups"),
        new("usccss.g5.u03.l16.source_twenty_five_fraction_quotients", "Original source less-than-1: one fifth divided among twenty-five", "(1/5) ÷ 25", 1, 125, "of original whole"),
        new("usccss.g5.u03.l16.source_ninetyeight_fifth_estimate", "Original source Estimation Exploration: 98 wholes divided into one-fifth-unit-sized groups, a little less than 100÷1/5=500", "98 ÷ (1/5)", 490, 1, "fifth-size groups"),
        new("usccss.g5.u03.l17.source_multiply_divide_warmup", "Five wholes count how many sixth-sized pieces fit", "5 ÷ (1/6)", 30, 1, "groups"),
        new("usccss.g5.u03.l17.source_multiply_divide_warmup", "Five wholes times one sixth", "5 × (1/6)", 5, 6, "original units"),
        new("usccss.g5.u03.l17.source_multiply_divide_warmup", "One fifth shared among six equal recipients", "(1/5) ÷ 6", 1, 30, "original units per recipient"),
        new("usccss.g5.u03.l17.source_multiply_divide_warmup", "One fifth times one sixth of same whole", "(1/5) × (1/6)", 1, 30, "of the original whole"),
        new("usccss.g5.u03.l17.source_real_world_fraction_contexts", "Eleven grains of rice together weigh one third gram: find weight of ONE grain", "(1/3) ÷ 11", 1, 33, "grams per grain"),
        new("usccss.g5.u03.l17.source_real_world_fraction_contexts", "Mai travels three quarters of a nine-tenths mile long road", "(9/10) × (3/4)", 27, 40, "miles run"),
        new("usccss.g5.u03.l17.source_real_world_fraction_contexts", "Nine tennis balls each weigh 2 1/16 ounces", "(33/16) × 9", 297, 16, "ounces total"),
        new("usccss.g5.u03.l18.original_warmup_and_picnic", "3 times one tenth of original whole", "3 × (1/10)", 3, 10, "of original whole"),
        new("usccss.g5.u03.l18.original_warmup_and_picnic", "one tenth multiplied by three", "(1/10) × 3", 3, 10, "of original whole"),
        new("usccss.g5.u03.l18.original_warmup_and_picnic", "one tenth shared among three people", "(1/10) ÷ 3", 1, 30, "of original whole"),
        new("usccss.g5.u03.l18.original_warmup_and_picnic", "three whole units divided into tenth-sized groups", "3 ÷ (1/10)", 30, 1, "groups"),
        new("usccss.g5.u03.l18.original_warmup_and_picnic", "Diego makes quarter-pound burgers from two pounds beef", "2 ÷ (1/4)", 8, 1, "burgers"),
        new("usccss.g5.u03.l18.original_warmup_and_picnic", "Diego and Clare equally share one-quarter pound potato salad", "(1/4) ÷ 2", 1, 8, "pounds per person"),
        new("usccss.g5.u03.l18.original_partner_contexts", "Partner A: Han walks three fourths of a four-fifths kilometer route", "(4/5) × (3/4)", 3, 5, "kilometers"),
        new("usccss.g5.u03.l18.original_partner_contexts", "Partner A: half gallon water equally divided among ten science groups", "(1/2) ÷ 10", 1, 20, "gallons per group"),
        new("usccss.g5.u03.l18.original_partner_contexts", "Partner A: three kilograms fills one fifth of strawberry container, find full capacity", "3 ÷ (1/5)", 15, 1, "kilograms capacity"),
        new("usccss.g5.u03.l18.original_partner_contexts", "Partner B: Han takes four minutes to walk one-third kilometer, time for whole kilometer", "4 ÷ (1/3)", 12, 1, "minutes"),
        new("usccss.g5.u03.l18.original_partner_contexts", "Partner B: Clare collects two and two thirds times her four-kilogram soil goal", "(8/3) × 4", 32, 3, "kilograms collected"),
        new("usccss.g5.u03.l18.original_partner_contexts", "Partner B: a half-pound strawberry container is three fifths full", "(1/2) × (3/5)", 3, 10, "pounds strawberries"),
        new("usccss.g5.u03.l19.source_estimation_exact", "Original source warm-up 28 times two and eight ninths, estimate between 56 and 84 then compute exactly", "28 × (26/9)", 728, 9, "units"),
        new("usccss.g5.u03.l19.original_digit_optimization_extrema", "Greatest legal product of two fractions using distinct slots from digits 1–6; explain global maximum", "(6/1) × (5/2)", 15, 1, "source-permitted exact fraction value"),
        new("usccss.g5.u03.l19.original_digit_optimization_extrema", "Smallest legal positive product of two fractions using four distinct slots from digits 1–6", "(1/6) × (2/5)", 1, 15, "source-permitted exact fraction value"),
        new("usccss.g5.u03.l19.original_digit_optimization_extrema", "Largest whole ÷ unit fraction from distinct 1–6 choices", "6 ÷ (1/5)", 30, 1, "source-permitted exact fraction value"),
        new("usccss.g5.u03.l19.original_digit_optimization_extrema", "Smallest whole ÷ unit fraction from distinct 1–6 choices", "1 ÷ (1/2)", 2, 1, "source-permitted exact fraction value"),
        new("usccss.g5.u03.l19.original_digit_optimization_extrema", "Largest unit fraction ÷ whole from distinct 1–6 choices", "(1/2) ÷ 1", 1, 2, "source-permitted exact fraction value"),
        new("usccss.g5.u03.l19.original_digit_optimization_extrema", "Smallest unit fraction ÷ whole from distinct 1–6 choices", "(1/6) ÷ 5", 1, 30, "source-permitted exact fraction value"),
        new("usccss.g5.u03.l20.source_unknown_class_whole", "One third of the source class equals nine students. How many students are in the full class?", "9 ÷ (1/3)", 27, 1, "students in class"),
        new("usccss.g5.u03.l20.source_unknown_class_whole", "Source class is 27; one third times unknown whole equals nine", "(1/3) × 27", 9, 1, "students represented by one third"),
        new("usccss.g5.u03.l20.source_unknown_class_whole", "Source class whole contains three groups of nine pupils", "3 × 9", 27, 1, "students in class"),
        new("usccss.g5.u03.l20.source_unknown_class_whole", "Jada's class has 24 pupils, one quarter of grade-five pupils. How many pupils are in grade five?", "24 ÷ (1/4)", 96, 1, "grade-five students"),
        new("usccss.g5.u03.l20.source_unknown_class_whole", "Jada source equal-quarters check: total grade-five pupils 96 divided among four classes", "96 ÷ 4", 24, 1, "students in one class"),
        new("usccss.g5.u03.l20.source_calcium_rocket_total", "Original source 250 mg calcium is one quarter of recommended daily allowance", "250 ÷ (1/4)", 1000, 1, "mg total recommended calcium"),
        new("usccss.g5.u03.l20.source_calcium_rocket_total", "Original source 60 days to travel one fifth of a Mars route, estimate total travel days in a constant-speed model", "60 ÷ (1/5)", 300, 1, "days total travel"),
        // END_G5_U03_AUTHORING_CASES
    ];
    public static bool Supports(string? family) =>
        !string.IsNullOrWhiteSpace(family) &&
        Cases.Any(c => string.Equals(c.Family, family, StringComparison.Ordinal));
    private static int Gcd(int a,int b) {a=Math.Abs(a);b=Math.Abs(b);while(b!=0){int r=a%b;a=b;b=r;}return a;}
    private static string Rational(int a,int b)
    {
        int g=Gcd(a,b);
        return b/g==1 ? (a/g).ToString(CultureInfo.InvariantCulture)
            : $"{a/g}/{b/g}";
    }
    private static bool Valid(string? family,IReadOnlyDictionary<string,int>? p,
        out SourceCase? answer)
    {
        answer=null;
        if(!Supports(family)||p is null||p.Count!=2||
           !p.TryGetValue("case",out int index)||index<0||
           !p.TryGetValue("slot",out int slot)||slot is <0 or >2)return false;
        var cases=Cases.Where(c => c.Family==family).ToArray();
        if(index>=cases.Length) return false;
        answer=cases[index];return true;
    }
    public static Problem Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unregistered G5 U03 source family.");
        ArgumentNullException.ThrowIfNull(random);
        var cases=Cases.Where(c => c.Family==family).ToArray();
        IReadOnlyDictionary<string,int> p=new Dictionary<string,int>(StringComparer.Ordinal)
        {["case"]=random.Next(cases.Length),["slot"]=random.Next(3)};
        if(!Valid(family,p,out var c)||c is null)throw new InvalidOperationException("Invalid source case.");
        string correct=Rational(c.Numerator,c.Denominator);
        string wrongA=Rational(c.Numerator+c.Denominator,c.Denominator);
        string wrongB=Rational(c.Numerator+2*c.Denominator,c.Denominator);
        string[] options=p["slot"] switch
        {
            0=>[correct,wrongA,wrongB],
            1=>[wrongA,correct,wrongB],
            _=>[wrongA,wrongB,correct]
        };
        string prompt=$"{c.Context}. Calculate the exact source expression {c.Expression} " +
            $"relative to the SAME original whole. Select 1: {options[0]}; 2: {options[1]}; 3: {options[2]}. " +
            "Student sketches, original context interpretations and explanations are TEACHER_REVIEW_REQUIRED.";
        string solution=$"{c.Expression} = {correct} {c.Units}. " +
            "The source context and full-whole reference matter; a selected numeric choice cannot score the pupil's diagram.";
        return new Problem(family,prompt,solution,AssessmentItemType.Numeric,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p,out _))throw new InvalidOperationException("Malformed G5 Unit 3 source case.");
        return (p["slot"]+1).ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int>? p,string? answer) =>
        Valid(family,p,out _) &&
        string.Equals(answer,(p!["slot"]+1).ToString(CultureInfo.InvariantCulture),
            StringComparison.Ordinal);
}

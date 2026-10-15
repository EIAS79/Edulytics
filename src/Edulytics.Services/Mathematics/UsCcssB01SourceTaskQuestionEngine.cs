using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// 2021 Illustrative Mathematics K5 B01 lesson-purpose-aligned FIRST PARTY
/// task families. Source-specific tasks are generated from structured numbers,
/// never inferred from the generic title alone. This does not grant academic
/// certification; it supplies a real runtime for subsequent L5/L6 verification.
/// </summary>
internal static class UsCcssB01SourceTaskQuestionEngine
{
    public sealed record Problem(
        string Family, string Prompt, string Solution, AssessmentItemType ItemType,
        IReadOnlyDictionary<string, int> Parameters);

    private static readonly HashSet<string> Families = new(StringComparer.Ordinal)
    {
        // Source-verified optional center / riddle / fractional estimate goals.
        "usccss.b01.g1.place_value_number_riddle",
        "usccss.b01.g2.target_measurement_difference_cm",
        "usccss.b01.g3.quarter_inch_estimate_classify",
        "usccss.b01.g1.match_dot_cubes_expression",
        "usccss.b01.g1.complete_missing_dots",
        "usccss.b01.g1.check_off_add_within10",
        "usccss.b01.g1.add_one_or_two",
        "usccss.b01.g1.subtract_one_or_two",
        "usccss.b01.g1.make_ten_missing_part",
        "usccss.b01.triangle.attributes",
        "usccss.b01.shape.polygon_sides",
        "usccss.b01.shape.specific_subcategory",
        "usccss.b01.area.missing_composite_side",
        "usccss.b01.partition.halves_fourths",
        "usccss.b01.number_line.equal_intervals",
        "usccss.b01.number_line.locate_whole_to100",
        "usccss.b01.number_line.move_left_right_within100",
        "usccss.b01.number_line.estimate_location_within100",
        "usccss.b01.number_line.locate_three_digits",
        "usccss.b01.number_line.locate_large_numbers",
        "usccss.b01.clock.half_quarters",
        "usccss.b01.clock.read_half_hour",
        "usccss.b01.clock.read_on_hour",
        "usccss.b01.clock.read_five_minutes_am_pm",
        "usccss.b01.clock.read_five_minutes",
        "usccss.b01.rectangle.unit_squares",
        "usccss.b01.measure.map_lengths",
        "usccss.b01.g1.measure.same_length_different_units",
        "usccss.b01.g2.measurement.estimate_cm",
        "usccss.b01.g1.measurement.story_result_difference",
        "usccss.b01.g1.measurement.compare_bigger_smaller_unknown",
        "usccss.b01.g2.measurement.inch_ruler",
        "usccss.b01.g2.measurement.feet_inch_same_length",
        "usccss.b01.g2.measure.reptile_meterstick",
        "usccss.b01.g2.measurement.iterate_ruler_cm",
        "usccss.b01.ruler.compare_centimeters",
        "usccss.b01.g2.measurement.read_line_plot",
        "usccss.b01.g2.measurement.pencil_line_plot",
        "usccss.b01.g2.measurement.plot_interpret_nonzero_start",
        "usccss.b01.g3.quarter_inch_line_plot_frequency",
        "usccss.b01.g4.eighth_inch_line_plot_difference",
        "usccss.b01.g3.picture_graph.scaled_compare",
        "usccss.b01.g3.picture_graph.construct",
        "usccss.b01.g2.compare_tape_difference",
        "usccss.b01.g2.compare_tape_unknown_positions",
        "usccss.b01.g2.length_compare.all_unknown_positions100",
        "usccss.b01.ruler.measure_without_zero",
        "usccss.b01.graph.choose_scale",
        "usccss.b01.graph.scaled_compare",
        "usccss.b01.array.equal_groups",
        "usccss.b01.array.columns",
        "usccss.b01.array.equal_addends",
        "usccss.b01.array.equal_equation",
        "usccss.b01.array.unknown_factor",
        "usccss.b01.g3.multiplication_division_inverse_array",
        "usccss.b01.g2.compose_equal_square_shapes",
        "usccss.b01.array.commutativity",
        "usccss.b01.geometry.composite_rectangle_area",
        "usccss.b01.geometry.rectangle_area_grid60",
        "usccss.b01.geometry.rectangle_area_ungridded",
        "usccss.b01.g4.rectangle_area_is_multiple",
        "usccss.b01.g3.distributive_area_partition",
        "usccss.b01.g3.rectangle_area_perimeter_story",
        "usccss.b01.g3.same_perimeter_different_area",
        "usccss.b01.g3.same_area_different_perimeter",
        "usccss.b01.geometry.rectangle_area_real_world_units",
        "usccss.b01.division.unknown_factor",
        "usccss.b01.fraction.tenths_hundredths_within_one",
        "usccss.b01.fraction.tenths_hundredths_over_one",
        "usccss.b01.fraction.name_parts",
        "usccss.b01.fraction.unit_parts",
        "usccss.b01.measure.quarter_inch",
        "usccss.b01.factors.factor_pairs",
        "usccss.b01.g4.factor_pairs_two_rectangles",
        "usccss.b01.factors.prime_composite",
        "usccss.b01.factors.locker_square",
        "usccss.b01.factors.factors_multiples",
        "usccss.b01.fraction.whole_multiply",
        "usccss.b01.fraction.decompose_same_denominator",
        "usccss.b01.place_value.ten_times",
        "usccss.b01.place_value.tens_ones",
        "usccss.b01.g1.add_subtract_10_tens",
        "usccss.b01.g1.teen_no_regroup",
        "usccss.b01.g1.teen_subtract_decompose_ten",
        "usccss.b01.g2.story_without_regroup100",
        "usccss.b01.g2.story_regroup50",
        "usccss.b01.g1.compare_order_tens_ones",
        "usccss.b01.g4.whole_minus_proper_fraction",
        "usccss.b01.g2.order_three_digits_number_line",
        "usccss.b01.g4.decimal_number_line_greater",
        "usccss.b01.g2.add_subtract_1000_no_regroup",
        "usccss.b01.g1.add_within100_no_compose_ten",
        "usccss.b01.g2.add_subtract_without_regroup",
        "usccss.b01.g2.subtract_decompose_ten",
        "usccss.b01.g1.add_subtract_tens_multiples",
        "usccss.b01.place_value.count_to120",
        "usccss.b01.g1.measure.count_cubes_up_to110",
        "usccss.b01.g1.measure.write_cubes_to120",
        "usccss.b01.place_value.three_digits",
        "usccss.b01.market.inventory_100",
        "usccss.b01.g3.add_within1000",
        "usccss.b01.g3.add_regroup1000",
        "usccss.b01.g3.subtract_within1000",
        "usccss.b01.g3.subtract_regroup1000",
        "usccss.b01.g3.subtract_across_zero"
    };

    public static bool Supports(string? family) =>
        !string.IsNullOrWhiteSpace(family) && Families.Contains(family);

    private static Dictionary<string, int> D(params (string Key, int Value)[] pairs) =>
        pairs.ToDictionary(x => x.Key, x => x.Value, StringComparer.Ordinal);

    // Explicit eighth-inch tick labels. 2/8 and 4/8 preserve the actual
    // eighth-inch plotting grid while representing fourths and halves.
    private static string EighthLabel(int whole, int tick) =>
        tick == 0 ? whole.ToString(CultureInfo.InvariantCulture) :
        tick == 8 ? (whole+1).ToString(CultureInfo.InvariantCulture) :
        $"{whole} {tick}/8";

    private static Problem Make(string family, string prompt, string explanation,
        AssessmentItemType type, Dictionary<string, int> p) =>
        new(family, prompt, explanation, type, p);

    public static Problem Build(string f, Random r, ExactSkillQuestionDifficulty difficulty)
    {
        if (!Supports(f)) throw new InvalidOperationException("Unknown B01 source-task family.");
        var level = (int)difficulty;
        int a, b, n, d, m;
        switch (f)
        {
            // Official T001 G1 U01 L03: counting on EXACTLY one or two
            // within ten. This is not unbounded generic addition.
            case "usccss.b01.g1.add_one_or_two":
                a=r.Next(0,9); b=r.Next(1,Math.Min(2,10-a)+1);
                return Make(f,
                    $"Start at {a}. Count forward {b} step(s), one number per step. What is {a} + {b}?",
                    "Use a counter or a number path; count only the additional one or two steps.",
                    AssessmentItemType.Numeric,D(("start",a),("step",b)));
            // Official T001 G1 U01 L04: counting BACK exactly one or two
            // within ten, with no negative totals or addition-only drift.
            case "usccss.b01.g1.subtract_one_or_two":
                a=r.Next(1,11); b=r.Next(1,Math.Min(2,a)+1);
                return Make(f,
                    $"Start with {a} counters. Remove {b} counter(s). Count back {b} step(s). How many remain?",
                    "Counting backward means each step takes away one; check by adding the removed counters.",
                    AssessmentItemType.Numeric,D(("start",a),("step",b)));
            // Official T001 G1 U01 L06: Find the Pair, Make 10.
            // A card plus its complement MUST fill exactly ten spaces.
            case "usccss.b01.g1.make_ten_missing_part":
                a=r.Next(0,11); b=10-a;
                return Make(f,
                    $"In Find the Pair, you have card {a}. Which number card makes exactly 10 with it? Complete {a} + ? = 10.",
                    "Find the empty spots in a ten-frame or subtract the known part from 10.",
                    AssessmentItemType.Numeric,D(("known",a),("missing",b)));
            // IM G1 U08 L09: clues intentionally leave multiple candidates
            // until a tens interval plus exact ones selects one secret number.
            case "usccss.b01.g1.place_value_number_riddle":
                a=r.Next(1,10); // tens, 1..9
                b=r.Next(1,10); // ones, 1..9 for strict inequalities
                return Make(f,
                    $"I have {b} ones. My number is greater than {a*10} " +
                    $"and less than {(a+1)*10}. What is the secret number?",
                    "Ten ones form one ten. Combine the single possible tens " +
                    "digit with the exact ones digit, and verify both clues.",
                    AssessmentItemType.Numeric,
                    D(("tens",a),("ones",b),("number",a*10+b)));
            // IM G2 U03 L13 Target Measurements: a length is chosen, drawn
            // and measured; the game's error is the absolute difference.
            case "usccss.b01.g2.target_measurement_difference_cm":
                a=r.Next(4,21); // target in centimeters
                b=a+r.Next(-3,4); // observed measurement, may be exact
                return Make(f,
                    $"Target Measurements center: draw a line as close as " +
                    $"possible to {a} cm. A ruler shows the drawn line is {b} cm. " +
                    "What is the absolute difference in centimeters?",
                    "Take the positive distance between the target and actual " +
                    "length; an exact match scores zero.",
                    AssessmentItemType.Numeric,
                    D(("target",a),("measured",b)));
            // IM G3 U08 L14: estimate a fractional-inch length and check
            // whether a proposed estimate is low, right or high. A synthetic
            // physical object replaces the source's third-party photograph.
            case "usccss.b01.g3.quarter_inch_estimate_classify":
                a=r.Next(4,12); // true length in quarter-inch marks, 1..2¾ in
                n=r.Next(0,3);  // 0 low, 1 right, 2 high
                b=a+n-1;
                return Make(f,
                    $"A strip measures {a}/4 inches using a ruler marked " +
                    $"in fourths. A student estimates {b}/4 inches. Is that " +
                    "estimate 'too low', 'about right', or 'too high'?",
                    "Compare the proposed and measured lengths using equal " +
                    "quarter-inch intervals. A smaller estimate is low, " +
                    "an equal estimate is about right, and a larger one is high.",
                    AssessmentItemType.ShortAnswer,
                    D(("actualQuarters",a),("estimateQuarters",b),("mode",n)));

            // Grade 1 U01 L02, official IM Activity 1: match paired dot
            // cube pictures with their written expression, then find total.
            // We explicitly provide visual dot groups rather than a word-only
            // addition problem and bound all sums to ten.
            case "usccss.b01.g1.match_dot_cubes_expression":
                a = r.Next(1,7);
                b = r.Next(1, Math.Min(7,11-a));
                return Make(f,
                    $"First dot cube: {string.Join(" ",Enumerable.Repeat("●",a))}. " +
                    $"Second dot cube: {string.Join(" ",Enumerable.Repeat("●",b))}. " +
                    "Write the matching addition expression as a+b (without a total).",
                    "Count the two pictures separately, then write the two addends " +
                    "in the same order using a plus sign.",
                    AssessmentItemType.ShortAnswer,D(("first",a),("second",b)));

            // Original student-facing Activity 1 asks students to draw the
            // missing dots for a given expression. Numeric count is checked
            // here; Rich V2 displays the pictured first group separately.
            case "usccss.b01.g1.complete_missing_dots":
                a = r.Next(1,7);
                b = r.Next(1,Math.Min(7,11-a));
                return Make(f,
                    $"Draw the missing dot group so two groups represent {a} + {b}. " +
                    $"One group already shows {a} dots: " +
                    string.Join(" ",Enumerable.Repeat("●",a)) +
                    ". How many dots must the second group contain?",
                    $"Add exactly {b} more dots to match the second addend, then " +
                    $"check that the combined total is {a+b}.",
                    AssessmentItemType.Numeric,D(("first",a),("second",b)));

            // Official IM Activity 2: draw two number cards at most five,
            // find the sum, and check it off while recording the expression.
            case "usccss.b01.g1.check_off_add_within10":
                a = r.Next(0,6); b = r.Next(0,6);
                return Make(f,
                    $"In Check It Off, your two number cards are {a} and {b}. " +
                    $"What sum do you check off? Record {a} + {b} beside that sum.",
                    "The sum is the total after combining the two number cards; " +
                    "both cards are at most five.",
                    AssessmentItemType.Numeric,D(("first",a),("second",b)));

            // Original IM Grade 3 U03: add and subtract within 1,000,
            // represent hundreds/tens/ones, regroup and analyze across-zero
            // decomposition. A tiny generic arithmetic question is not a
            // substitute for these source goals.
            case "usccss.b01.g3.add_within1000":
            {
                a=r.Next(100,700);
                b=r.Next(100,1000-a);
                return Make(f,
                    $"Add {a} + {b} within 1,000. Show how hundreds, tens and ones combine.",
                    "Combine equal place-value units; trade ten ones for one ten " +
                    "or ten tens for one hundred only when needed, then verify by subtraction.",
                    AssessmentItemType.Numeric,
                    D(("left",a),("right",b),("operation",0),("expected",a+b)));
            }
            case "usccss.b01.g3.add_regroup1000":
            {
                for(var attempt=0;attempt<128;attempt++)
                {
                    a=r.Next(120,700);b=r.Next(100,1000-a);
                    var onesCarry=a%10+b%10>=10;
                    var tensCarry=(a/10)%10+(b/10)%10+(onesCarry?1:0)>=10;
                    if (!onesCarry && !tensCarry)continue;
                    return Make(f,
                        $"Use a written place-value addition algorithm for {a} + {b}. " +
                        "Explain a newly composed ten or hundred, then find the sum.",
                        "Regroup ten ones as one ten or ten tens as one hundred. " +
                        "Record the carried unit above the corresponding column.",
                        AssessmentItemType.Numeric,
                        D(("left",a),("right",b),("operation",0),("expected",a+b)));
                }
                throw new InvalidOperationException("Could not generate regrouping addition.");
            }
            case "usccss.b01.g3.subtract_within1000":
            {
                a=r.Next(200,1000);b=r.Next(100,a);
                return Make(f,
                    $"Subtract {a} − {b} within 1,000. Choose a place-value " +
                    "algorithm or an equivalent strategy, and explain your method.",
                    "Compare the hundreds, tens and ones before choosing a method. " +
                    "Check the difference by adding it back to the subtrahend.",
                    AssessmentItemType.Numeric,
                    D(("left",a),("right",b),("operation",1),("expected",a-b)));
            }
            case "usccss.b01.g3.subtract_regroup1000":
            {
                for(var attempt=0;attempt<128;attempt++)
                {
                    a=r.Next(250,1000);b=r.Next(101,a);
                    var borrowOnes=a%10<b%10;
                    var borrowTens=(a/10)%10<(b/10)%10+(borrowOnes?1:0);
                    if(!borrowOnes && !borrowTens)continue;
                    return Make(f,
                        $"Subtract {a} − {b} using an expanded-form " +
                        "or written place-value algorithm. Show the necessary decomposition.",
                        "Rename one higher-place unit as ten lower-place units " +
                        "before subtracting. Check by adding the result back.",
                        AssessmentItemType.Numeric,
                        D(("left",a),("right",b),("operation",1),("expected",a-b)));
                }
                throw new InvalidOperationException("Could not generate regrouping subtraction.");
            }
            case "usccss.b01.g3.subtract_across_zero":
            {
                a=r.Next(3,10)*100;
                b=r.Next(1,a/100)*100+r.Next(1,10)*10+r.Next(1,10);
                return Make(f,
                    $"Analyze the subtraction algorithm for {a} − {b}. " +
                    "How do you decompose a hundred across the zero tens and zero ones?",
                    "Exchange one hundred for ten tens, then exchange one ten " +
                    "for ten ones; finish the subtraction and verify by addition.",
                    AssessmentItemType.Numeric,
                    D(("left",a),("right",b),("operation",1),("expected",a-b)));
            }

            case "usccss.b01.market.inventory_100":
                // 2021 IM G2 U02 L16 optional market: one stock category <10,
                // two categories >10, total exactly 100; transactions subtract
                // sales from the real inventory.
                a = r.Next(2, 10);
                b = r.Next(20, 61);
                n = 100 - a - b;
                d = r.Next(1, Math.Min(b, 12));
                m = r.Next(0, 2);
                return Make(f,
                    $"Your market starts with {a} apples, {b} notebooks and {n} pencils " +
                    $"(100 items altogether). A customer buys {d} notebooks. " +
                    (m == 0 ? "How many notebooks remain in stock?" :
                              "How many pencils were in stock before the sale?"),
                    "Model three inventory quantities totaling 100; subtract sales " +
                    "from the relevant item, and do not subtract unrelated stock.",
                    AssessmentItemType.Numeric,
                    D(("small", a), ("second", b), ("third", n), ("sold", d), ("mode", m)));

            // Official Grade 1 U04/U08: two-digit numerals are tens and ones.
            // Original IM Grade 1 U04 L03: add or subtract EXACTLY TEN
            // from a multiple of ten. There are no ones-domain tasks here.
            // IM G2 U02 L01: two-digit arithmetic without regrouping.
            case "usccss.b01.g2.add_subtract_without_regroup":
            {
                m=r.Next(0,2);
                if(m==0)
                {
                    var lt=r.Next(1,8);var lo=r.Next(0,10);
                    var rt=r.Next(1,10-lt);var ro=r.Next(0,10-lo);
                    a=10*lt+lo;b=10*rt+ro;
                }
                else
                {
                    var lt=r.Next(2,10);var lo=r.Next(0,10);
                    var rt=r.Next(1,lt+1);var ro=r.Next(0,lo+1);
                    a=10*lt+lo;b=10*rt+ro;
                }
                return Make(f,
                    $"Find {a} {(m==0?"+":"−")} {b} using tens and ones. " +
                    "Do not compose or decompose a ten.",
                    "Operate independently on tens and ones, without carrying or borrowing.",
                    AssessmentItemType.Numeric,
                    D(("left",a),("right",b),("operation",m),
                      ("expected",m==0?a+b:a-b)));
            }
            // IM G2 U02 L07: subtract using a mandatory ten decomposition.
            case "usccss.b01.g2.subtract_decompose_ten":
            {
                var lt=r.Next(2,10);var lo=r.Next(0,9);
                var rt=r.Next(1,lt);var ro=r.Next(lo+1,10);
                a=lt*10+lo;b=rt*10+ro;
                return Make(f,
                    $"Subtract {a} − {b} with tens and ones. " +
                    "Show how to decompose one ten into ten ones.",
                    "Rename one ten as ten ones before subtracting the ones column.",
                    AssessmentItemType.Numeric,
                    D(("left",a),("right",b),("operation",1),
                      ("expected",a-b)));
            }

            // Original IM G1 U04 L17: compare and ORDER three 2-digit
            // numbers by tens, breaking ties using the ones digit.
            // Original G4 U03 L10: subtract a proper fraction from a whole.
            // Original G2 U05 L09: compare actual three-digit positions
            // using the SAME equal-interval number line.
            // IM G2 U07 L04: add/subtract within 1,000 with NO regrouping.
            // IM G1 U05 L04: two-digit addition within 100 without
            // ever composing a new ten from the ones.
            case "usccss.b01.g1.add_within100_no_compose_ten":
            {
                var ah=r.Next(1,9);var bh=r.Next(1,10-ah);
                var ao=r.Next(0,10);var bo=r.Next(0,10-ao);
                a=ah*10+ao;b=bh*10+bo;
                return Make(f,
                    $"Add {a} + {b} within 100 using tens and ones. " +
                    "Do not compose a new ten.",
                    "Combine tens with tens and ones with ones; their ones " +
                    "total stays below 10, so no regrouping is necessary.",
                    AssessmentItemType.Numeric,
                    D(("left",a),("right",b),("expected",a+b)));
            }

            case "usccss.b01.g2.add_subtract_1000_no_regroup":
            {
                m=r.Next(0,2);
                if(m==0)
                {
                    var leftH=r.Next(1,9);var rightH=r.Next(1,10-leftH);
                    var leftT=r.Next(0,10);var rightT=r.Next(0,10-leftT);
                    var leftO=r.Next(0,10);var rightO=r.Next(0,10-leftO);
                    a=leftH*100+leftT*10+leftO;
                    b=rightH*100+rightT*10+rightO;
                }
                else
                {
                    var leftH=r.Next(2,10);var rightH=r.Next(1,leftH);
                    var leftT=r.Next(0,10);var rightT=r.Next(0,leftT+1);
                    var leftO=r.Next(0,10);var rightO=r.Next(0,leftO+1);
                    a=leftH*100+leftT*10+leftO;
                    b=rightH*100+rightT*10+rightO;
                }
                return Make(f,
                    $"Compute {a} {(m==0?"+":"−")} {b} with place-value " +
                    "strategies, without composing or decomposing tens or hundreds.",
                    "Every place is handled independently; check each ones, " +
                    "tens and hundreds column against its own digit bounds.",
                    AssessmentItemType.Numeric,
                    D(("left",a),("right",b),("operation",m),
                      ("expected",m==0?a+b:a-b)));
            }

            // IM G4 U04 L03: compare hundredths by relative numeric
            // positions on an equal-interval number line.
            case "usccss.b01.g4.decimal_number_line_greater":
            {
                a=r.Next(0,9)*10;
                b=r.Next(0,11);n=r.Next(0,11);
                while(n==b)n=r.Next(0,11);
                var first=a+b;var second=a+n;
                return Make(f,
                    "A number line uses equal 0.01 intervals. Two marked " +
                    $"points are {(first/100m).ToString("0.00",CultureInfo.InvariantCulture)} " +
                    $"and {(second/100m).ToString("0.00",CultureInfo.InvariantCulture)}. " +
                    "Which decimal is farther RIGHT (greater)?",
                    "Greater decimals are farther right. Each equal tick adds 0.01.",
                    AssessmentItemType.ShortAnswer,
                    D(("start",a),("tickA",b),("tickB",n),
                      ("first",first),("second",second)));
            }

            case "usccss.b01.g2.order_three_digits_number_line":
            {
                var startHundred=r.Next(1,9)*100;
                a=r.Next(0,11);b=r.Next(0,11);
                while(b==a)b=r.Next(0,11);
                return Make(f,
                    $"The numbers {startHundred+a*10} and {startHundred+b*10} " +
                    "are placed on the same number line, with 10 units " +
                    "between adjacent marks. Which number is farther right?",
                    "Greater whole numbers lie farther right on the same " +
                    "graduated line; do not add the two values.",
                    AssessmentItemType.Numeric,
                    D(("start",startHundred),("tickA",a),("tickB",b),
                      ("first",startHundred+a*10),("second",startHundred+b*10)));
            }

            case "usccss.b01.g4.whole_minus_proper_fraction":
            {
                a=r.Next(1,5);d=r.Next(3,11);n=r.Next(1,d);
                return Make(f,
                    $"Calculate {a} − {n}/{d}. Express the whole as " +
                    $"{a*d}/{d} and subtract equal denominator-sized parts. " +
                    "Give a simplified fraction.",
                    "Whole numbers can be renamed as fractions with the same denominator. " +
                    "Subtract numerators after expressing equivalent units.",
                    AssessmentItemType.ShortAnswer,
                    D(("whole",a),("numerator",n),("denominator",d)));
            }

            case "usccss.b01.g1.compare_order_tens_ones":
            {
                a=r.Next(11,90);
                b=(a/10)*10+r.Next(0,10);
                if(b==a)b=(a/10)*10+(a%10+1)%10;
                n=r.Next(10,100);
                while(n==a||n==b)n=r.Next(10,100);
                m=r.Next(0,3);
                return Make(f,
                    $"Order these two-digit numbers by tens, then ones: " +
                    $"{a}, {b}, {n}. " +
                    (m==0?"Which is greatest?":
                     m==1?"Which is smallest?":
                         "Which is in the middle after sorting from least to greatest?"),
                    "Compare tens digits first. If the tens match, compare ones. " +
                    "Write the three numbers in order before choosing the answer.",
                    AssessmentItemType.Numeric,
                    D(("first",a),("second",b),("third",n),("position",m)));
            }

            // Teacher IM G1 U03 L12: operate on teen ± one-digit
            // without composing/decomposing the existing one ten.
            case "usccss.b01.g1.teen_no_regroup":
            {
                m=r.Next(0,2);
                a=m==0?r.Next(11,19):r.Next(11,20);
                var ones=a%10;
                b=r.Next(1,(m==0?9-ones:ones)+1);
                return Make(f,
                    $"Start with {a}. {(m==0?"Add":"Subtract")} {b} ones. " +
                    "How many now? Keep the one ten intact.",
                    "Keep the ten bundled and operate on the loose ones. " +
                    "No new ten is made or broken.",
                    AssessmentItemType.Numeric,
                    D(("left",a),("right",b),("operation",m),
                      ("expected",m==0?a+b:a-b)));
            }
            // IM G1 U03 L22: subtract from teen numbers by exchanging
            // a ten for ten ones before removing more than the loose ones.
            case "usccss.b01.g1.teen_subtract_decompose_ten":
            {
                a=r.Next(10,19);
                b=r.Next(a%10+1,10);
                return Make(f,
                    $"There are {a} cubes: one full ten and {a%10} loose ones. " +
                    $"Take away {b} cubes. Decompose the ten into ten ones " +
                    "before calculating the remainder.",
                    "Rename one ten as ten ones, so that there are " +
                    $"{10+a%10} loose ones. Then subtract {b}.",
                    AssessmentItemType.Numeric,
                    D(("left",a),("right",b),("operation",1),
                      ("expected",a-b)));
            }
            // IM G2 U02 L03: story problems within 100, NO regrouping.
            case "usccss.b01.g2.story_without_regroup100":
            {
                m=r.Next(0,2);
                if(m==0)
                {
                    var lt=r.Next(1,8);var lo=r.Next(0,10);
                    var rt=r.Next(1,10-lt);var ro=r.Next(0,10-lo);
                    a=10*lt+lo;b=10*rt+ro;
                }
                else
                {
                    var lt=r.Next(2,10);var lo=r.Next(0,10);
                    var rt=r.Next(1,lt+1);var ro=r.Next(0,lo+1);
                    a=10*lt+lo;b=10*rt+ro;
                }
                return Make(f,
                    m==0
                        ?$"A class has {a} stickers and receives {b} more. " +
                         "How many stickers are there altogether? " +
                         "Explain with tens and ones without regrouping."
                        :$"A class has {a} stickers and gives away {b}. " +
                         "How many remain? Explain with tens and ones without regrouping.",
                    "Add or subtract tens and ones separately; no carrying " +
                    "or borrowing is needed in this story.",
                    AssessmentItemType.Numeric,
                    D(("left",a),("right",b),("operation",m),
                      ("expected",m==0?a+b:a-b)));
            }
            // IM G2 U02 L11: story problems within 50 that REQUIRE
            // composing a ten in addition or decomposing a ten in subtraction.
            case "usccss.b01.g2.story_regroup50":
            {
                m=r.Next(0,2);
                if(m==0)
                {
                    var lt=r.Next(1,3);var lo=r.Next(1,10);
                    var rt=r.Next(1,4-lt);var ro=r.Next(10-lo,10);
                    a=lt*10+lo;b=rt*10+ro;
                }
                else
                {
                    var lt=r.Next(2,5);var lo=r.Next(0,9);
                    var rt=r.Next(1,lt);var ro=r.Next(lo+1,10);
                    a=lt*10+lo;b=rt*10+ro;
                }
                return Make(f,
                    m==0
                        ?$"At a school event, one group counted {a} tickets and " +
                         $"another counted {b}. How many tickets altogether? " +
                         "Show how ten ones combine into one ten."
                        :$"A school has {a} pencils and gives away {b}. " +
                         "How many pencils remain? Show how one ten is decomposed.",
                    "Use bundles of tens and single ones. " +
                    "A composition or decomposition of ten is necessary.",
                    AssessmentItemType.Numeric,
                    D(("left",a),("right",b),("operation",m),
                      ("expected",m==0?a+b:a-b)));
            }

            case "usccss.b01.g1.add_subtract_10_tens":
            {
                a=r.Next(1,9)*10;
                m=r.Next(0,2);
                return Make(f,
                    $"Start with {a}. {(m==0?"Add":"Subtract")} 10. " +
                    "How many do you have? Show how tens and ones change.",
                    "Every ten is one complete bundle with zero loose ones. " +
                    (m==0?"Join one ten to the bundles.":"Remove one ten from the bundles."),
                    AssessmentItemType.Numeric,
                    D(("left",a),("right",10),("operation",m),
                      ("expected",m==0?a+10:a-10)));
            }
            // Original IM Grade 1 U04 L04: BOTH operands are multiples
            // of ten; sum or difference always within the G1 tens domain.
            case "usccss.b01.g1.add_subtract_tens_multiples":
            {
                a=r.Next(2,9)*10;
                b=r.Next(1,Math.Min(a/10,9-a/10)+1)*10;
                m=r.Next(0,2);
                return Make(f,
                    $"Calculate {a} {(m==0?"+":"−")} {b} using bundles of ten. " +
                    "What is the sum or difference?",
                    "Work with whole tens: each bundle represents 10 and " +
                    "no ones are added or removed.",
                    AssessmentItemType.Numeric,
                    D(("left",a),("right",b),("operation",m),
                      ("expected",m==0?a+b:a-b)));
            }

            case "usccss.b01.place_value.tens_ones":
                a = r.Next(10, 100);
                m = r.Next(0, 3);
                return Make(f,
                    m switch
                    {
                        0 => $"A base-ten model shows {a/10} bundles of TEN and " +
                             $"{a%10} single ones. Write the numeral.",
                        1 => $"A two-digit number is shown by the expression " +
                             $"{(a/10)*10} + {a%10}. Write the numeral.",
                        _ => $"A drawing groups {(a/10)*10} dots into tens with " +
                             $"{a%10} loose dots. How many dots are there altogether?"
                    },
                    "Each bundle of ten is worth 10; single pieces are worth 1. " +
                    "Combine tens and ones without changing the place values.",
                    AssessmentItemType.Numeric,
                    D(("number",a),("tens",a/10),("ones",a%10),("mode",m)));

            // Official Grade 1 U08 L07: organize a counted collection up to 120.
            // IM G1 U06 L08 and L09: source-bound LONG length iteration
            // and reading/writing the counted total: L08 100..110, L09
            // 95..120 unit cubes, rather than a generic 40..120 collection.
            case "usccss.b01.g1.measure.count_cubes_up_to110":
            case "usccss.b01.g1.measure.write_cubes_to120":
            {
                var limit110=f=="usccss.b01.g1.measure.count_cubes_up_to110";
                var cubes=limit110?r.Next(100,111):r.Next(95,121);
                var mode=r.Next(0,2);
                return Make(f,
                    $"A long length is measured end-to-end using equal " +
                    $"small unit cubes with NO gaps or overlaps. The cubes " +
                    $"are organized into {cubes/10} groups of ten and " +
                    $"{cubes%10} single cubes. " +
                    (mode==0
                        ?"How many unit cubes cover the entire length?"
                        :"Write the complete measured length as a number of unit cubes."),
                    "Every complete group contributes 10 equal cube units; " +
                    "add all groups and remaining cubes to find the measured length.",
                    AssessmentItemType.Numeric,
                    D(("number",cubes),("tens",cubes/10),
                      ("ones",cubes%10),("mode",mode)));
            }

            case "usccss.b01.place_value.count_to120":
                a = r.Next(40, 121);
                return Make(f,
                    $"A large collection has been organized into {a/100} group(s) " +
                    $"of one hundred, {(a%100)/10} bundle(s) of ten, and {a%10} " +
                    "remaining individual objects. How many objects are there?",
                    "Count each complete hundred and bundle of ten once, " +
                    "then add the remaining ones. The total must not exceed 120.",
                    AssessmentItemType.Numeric,
                    D(("number",a),("hundreds",a/100),
                      ("tens",(a%100)/10),("ones",a%10)));

            // Official Grade 2 U05 L04: base-ten numerals for 3-digit numbers.
            case "usccss.b01.place_value.three_digits":
                a = r.Next(100, 1000);
                m = r.Next(0,2);
                return Make(f,
                    m == 0
                        ? $"A base-ten drawing has {a/100} hundreds, " +
                          $"{(a%100)/10} tens, and {a%10} ones. Write the three-digit numeral."
                        : $"Write the numeral represented by {(a/100)*100} + " +
                          $"{((a%100)/10)*10} + {a%10}.",
                    "Value = 100 × hundreds + 10 × tens + ones. " +
                    "Zero in a place keeps that place but adds no units.",
                    AssessmentItemType.Numeric,
                    D(("number",a),("hundreds",a/100),
                      ("tens",(a%100)/10),("ones",a%10),("mode",m)));

            // G2 U06 L01: identify polygons by their defining side/corner count.
            case "usccss.b01.shape.polygon_sides":
                n = r.Next(3,7);
                // Four conceptually genuine classroom representations per
                // side-count, so a 10-question session is not exhausted.
                m = r.Next(0,4);
                return Make(f,
                    m switch
                    {
                        0 => $"A closed polygon has {n} straight sides and {n} " +
                             "corners. Name its category.",
                        1 => $"On dot paper, connect {n} corners with {n} straight " +
                             "edges into a closed figure. Name the polygon.",
                        2 => $"A shape card has {n} straight edges; its shape " +
                             $"has exactly {n} vertices. Which polygon card is it?",
                        _ => $"Rotate a polygon with {n} sides so it points " +
                             $"in a new direction. It still has {n} corners. " +
                             "What is its shape category?"
                    } + " (triangle, quadrilateral, pentagon or hexagon)",
                    "A triangle has 3 sides, a quadrilateral 4, a pentagon 5, " +
                    "and a hexagon 6. Orientation and side lengths do not change the category.",
                    AssessmentItemType.ShortAnswer, D(("sides",n),("variant",m)));

            // G3 U07 L02: sort triangular and quadrilateral subcategories
            // by their *specific* side and angle attributes.
            case "usccss.b01.shape.specific_subcategory":
                m = r.Next(0,4);
                n = r.Next(0,3);
                return Make(f,
                    m switch
                    {
                      0 => "A triangle has three sides of equal length. " +
                           "Name the most specific triangle category.",
                      1 => "A triangle has one 90-degree angle. " +
                           "Name the most specific triangle category given.",
                      2 => "A four-sided polygon has four 90-degree angles, " +
                           "but adjacent sides have different lengths. Name it.",
                      _ => "A quadrilateral has four equal sides and four right " +
                           "angles. Name its most specific category."
                    } + (n==0 ? "" : n==1 ? " Ignore its rotation on paper." :
                        " The sides form a closed polygon."),
                    "Sort by shared defining attributes: three equal sides make an " +
                    "equilateral triangle; a 90-degree angle makes a right triangle; " +
                    "four right angles make a rectangle; four equal sides plus " +
                    "four right angles make a square.",
                    AssessmentItemType.ShortAnswer,D(("subcategory",m),("variant",n)));

            // G3 U02 L14: composite area with a truly missing side.
            case "usccss.b01.area.missing_composite_side":
                a=r.Next(2,8); b=r.Next(2,8); n=r.Next(2,7);
                m=r.Next(2,7);
                if (m==n) m = m==6 ? 5 : m+1;
                return Make(f,
                    $"An ungridded L-shaped figure consists of two adjoining " +
                    $"non-overlapping rectangles. The left rectangle is {n} units " +
                    $"tall and {a} units wide. The right rectangle is {m} units tall. " +
                    $"The full base is {a+b} units wide, but the right width " +
                    $"is missing. Find the total area in square units.",
                    $"Missing right width: {a+b} − {a} = {b}. Sum the " +
                    $"non-overlapping areas: {n}×{a} + {m}×{b} = {n*a+m*b}.",
                    AssessmentItemType.Numeric,
                    D(("widthTotal",a+b),("widthKnown",a),
                      ("widthMissing",b),("heightLeft",n),
                      ("heightRight",m)));

            case "usccss.b01.triangle.attributes":
                m = r.Next(1, 4);
                n = r.Next(0, 4);
                return Make(f,
                    "Imagine " + (n == 0 ? "a narrow triangle" :
                        n == 1 ? "a tilted triangle" :
                        n == 2 ? "a triangle with unequal side lengths" :
                                 "a right-angled triangle") +
                    ". Which property MUST every triangle have? " +
                    (m == 1 ? "1) Three straight sides; 2) Three equal sides; 3) Four sides." :
                     m == 2 ? "1) Three equal sides; 2) Three straight sides; 3) Four sides." :
                              "1) Four sides; 2) Three equal sides; 3) Three straight sides.") +
                    " Enter the number of the correct choice.",
                    "A triangle is a closed shape with exactly three straight sides. Equal side lengths are not required.",
                    AssessmentItemType.Numeric, D(("position", m), ("style", n)));

            case "usccss.b01.partition.halves_fourths":
                d = r.Next(0, 2) == 0 ? 2 : 4;
                // Five distinct equal-part shapes give ten meaningful
                // shape/partition contexts without fake wording variants.
                m = r.Next(0, 5);
                var partitionShape = new[] { "circle", "rectangle", "square",
                    "triangle", "paper strip" }[m];
                return Make(f,
                    $"A {partitionShape} is partitioned into {d} EQUAL pieces. " +
                    "What is one piece called: half or fourth?",
                    "A half is one of two equal parts; a fourth is one of four equal parts.",
                    AssessmentItemType.ShortAnswer, D(("denominator", d), ("shape", m)));

            // Verified IM source targets: G2 U04 L01, G2 U05 L08
            // and G4 U04 L11. Exact grade-scoped scales prevent
            // teaching an irrelevant or impossible numeric range.
            // Original IM G2 U04 L04: increasing to the right and
            // decreasing to the left on a graduated whole-number line.
            case "usccss.b01.number_line.move_left_right_within100":
            {
                var startTick=r.Next(2,9);
                var jumps=r.Next(1,Math.Min(4,Math.Min(startTick,10-startTick))+1);
                var direction=r.Next(0,2)==0?-1:1;
                a=startTick*10;b=jumps*10;n=(startTick+direction*jumps)*10;
                return Make(f,
                    $"Start at {a} on a number line with marks every 10. " +
                    $"Move {jumps} equal jump(s) to the {(direction==1?"right":"left")}. " +
                    "Which whole number is at the ending point?",
                    "Moving right increases a number and moving left decreases it. " +
                    "Each labelled tick is ten units apart.",
                    AssessmentItemType.Numeric,
                    D(("start",a),("distance",b),("direction",direction),
                      ("jumps",jumps),("end",n)));
            }
            // Original IM G2 U04 L05: estimate the relative location of a
            // whole number on a line using bounding decades and distance.
            case "usccss.b01.number_line.estimate_location_within100":
            {
                a=r.Next(0,10)*10;
                b=new[]{1,2,3,4,6,7,8,9}[r.Next(0,8)];
                n=a+b;
                m=a+(b>=6?10:0);
                return Make(f,
                    $"A number line marks 0, 10, 20, ..., 100 equally. " +
                    $"Place the number {n} between {a} and {a+10}. " +
                    "Which labelled ten is closer to the point?",
                    "Compare the distance from the point to both adjacent tens. " +
                    "The closer labelled tick is its best estimate.",
                    AssessmentItemType.Numeric,
                    D(("target",n),("lower",a),("upper",a+10),("nearest",m)));
            }

            case "usccss.b01.number_line.locate_whole_to100":
            case "usccss.b01.number_line.locate_three_digits":
            case "usccss.b01.number_line.locate_large_numbers":
                a = f switch
                {
                    "usccss.b01.number_line.locate_whole_to100" =>
                        new[]{1,2,5,10}[r.Next(0,4)],
                    "usccss.b01.number_line.locate_three_digits" =>
                        new[]{50,100}[r.Next(0,2)],
                    _ => new[]{1000,10000,100000}[r.Next(0,3)]
                };
                n=r.Next(f=="usccss.b01.number_line.locate_three_digits"?2:1,10);
                m=r.Next(0,3);
                return Make(f,
                    $"A number line begins at 0 and has ten equal intervals " +
                    $"of {a:N0} units each. " +
                    (m==0 ? $"What whole number is at tick {n}?" :
                    m==1 ? $"The marked point is {n} equal jumps from zero. " +
                           "Write its exact number." :
                           $"Skip-count by {a:N0} to tick {n}. What is its label?"),
                    "Multiply the number of equal intervals by the interval " +
                    "size to locate the number exactly.",
                    AssessmentItemType.Numeric,
                    D(("step",a),("tick",n),("mode",m)));

            case "usccss.b01.number_line.equal_intervals":
                a = r.Next(1, 4 + level);
                b = r.Next(1, 7);
                return Make(f,
                    $"A number line has equally spaced ticks at 0, {a * b}, __, {a * b * 3}. " +
                    "What is the missing tick value?",
                    "Consecutive ticks have equal differences: add the constant step again.",
                    AssessmentItemType.Numeric, D(("step", a * b)));

            // Publisher G1 U07 L14-L15: hour/half-hour hand positions.
            // Original IM G1 U07 L13: tell and write exact whole hours.
            // This lesson does not teach elapsed time or half-hour answers.
            case "usccss.b01.clock.read_on_hour":
            {
                a=r.Next(1,13);
                return Make(f,
                    $"On an analog clock the short hour hand points directly " +
                    $"to {a} and the long minute hand points to 12. " +
                    "Write the time as h:mm.",
                    "The minute hand at 12 means zero minutes past the hour.",
                    AssessmentItemType.ShortAnswer,D(("hour",a),("minutes",0)));
            }
            // Original IM G2 U06 L13: analog/digital time to five minutes,
            // explicitly distinguished between a.m. and p.m.
            case "usccss.b01.clock.read_five_minutes_am_pm":
            {
                a=r.Next(1,13);
                b=r.Next(0,12)*5;
                m=r.Next(0,2);
                return Make(f,
                    $"The analog clock's minute hand points to " +
                    $"{(b==0?12:b/5)}, and the hour hand " +
                    (b==0?$"points directly to {a}. ":
                        $"is between {a} and {(a%12)+1}. ") +
                    $"It is {(m==0?"before noon (morning)":"after noon (afternoon/evening)")}. " +
                    "Write the digital time including a.m. or p.m.",
                    "Read each number as five minutes, then use the described " +
                    "part of the day to choose a.m. or p.m.",
                    AssessmentItemType.ShortAnswer,
                    D(("hour",a),("minutes",b),("period",m)));
            }
            case "usccss.b01.clock.read_half_hour":
                a = r.Next(1, 13);
                m = r.Next(0, 2);
                return Make(f,
                    m == 0
                        ? $"On an analog clock the hour hand points to {a} and " +
                          "the minute hand points to 12. Write the time (h:mm)."
                        : $"On an analog clock the hour hand is halfway between {a} " +
                          $"and {(a%12)+1}, and the minute hand points to 6. " +
                          "Write the time (h:mm).",
                    "On the hour the minute hand points to 12; at half past " +
                    "it points to 6 and the hour hand is halfway between numbers.",
                    AssessmentItemType.ShortAnswer, D(("hour", a), ("minutes", 30*m)));

            // Publisher G2 U06 L12: numbered clock ticks are five minutes apart.
            case "usccss.b01.clock.read_five_minutes":
                a = r.Next(1, 13);
                m = r.Next(0, 12);
                return Make(f,
                    $"The analog clock's minute hand points to " +
                    $"{(m==0 ? 12 : m)}, and the hour hand has reached or passed {a}. " +
                    "Write the time (h:mm).",
                    "Each number on an analog clock represents five minutes. " +
                    "Count the minute-hand position in groups of five.",
                    AssessmentItemType.ShortAnswer, D(("hour", a), ("minutes", m*5)));

            case "usccss.b01.clock.half_quarters":
                m = r.Next(0, 3);
                n = r.Next(1, 13);
                return Make(f,
                    $"On an analog clock, the hour hand is near {n} and the minute hand points to " +
                    (m == 0 ? "3" : m == 1 ? "6" : "9") +
                    ". How many minutes past the hour is that?",
                    "Each number on the minute scale represents five minutes.",
                    AssessmentItemType.Numeric, D(("position", m), ("hour", n)));

            case "usccss.b01.rectangle.unit_squares":
                a = r.Next(2, 4 + level);
                b = r.Next(2, Math.Max(3, 25 / a) + 1);
                return Make(f,
                    $"A rectangle is partitioned into {a} rows of {b} EQUAL unit squares " +
                    "with no gaps or overlaps. How many unit squares cover it?",
                    "Count the same number of unit squares in every row, then multiply rows by columns.",
                    AssessmentItemType.Numeric, D(("rows", a), ("columns", b)));

            // IM 2021 G2 U03 L03: centimeter ruler comparison.
            // Teacher source G2 U03 L14: interpret an X-mark line plot.
            // Original IM G2 U01 L14: two aligned tape bars show
            // a comparison with the difference unknown, within 20.
            // IM G2 U01 L15 and L16: compare problems with the unknown
            // in ALL three positions, not merely difference-unknown.
            // IM G2 U03 L06: comparisons of reptile lengths WITHIN 100 cm
            // where the unknown may be longer length, shorter length, or
            // difference. Wording must not choose the operation by itself.
            case "usccss.b01.g2.length_compare.all_unknown_positions100":
            {
                a=r.Next(41,100);
                var difference=r.Next(5,Math.Min(a-1,31));
                b=a-difference;
                m=r.Next(0,3);
                var question=m switch
                {
                    0=>$"A long reptile measures {a} cm. A shorter reptile " +
                      $"measures {b} cm. How much longer is the first reptile?",
                    1=>$"A smaller reptile measures {b} cm. The larger one " +
                      $"is {difference} cm longer. What is the larger length?",
                    _=>$"A large reptile measures {a} cm. That is " +
                      $"{difference} cm longer than a second reptile. " +
                      "What is the shorter length?"
                };
                return Make(f,question+" Use an aligned tape diagram.",
                    "The longer measurement equals the shorter measurement " +
                    "plus their difference, regardless of the story wording.",
                    AssessmentItemType.Numeric,
                    D(("longCount",a),("shortCount",b),
                      ("difference",difference),("unknownPosition",m)));
            }

            case "usccss.b01.g2.compare_tape_unknown_positions":
            {
                a=r.Next(8,20);b=r.Next(2,a);m=r.Next(0,3);
                var diff=a-b;
                var story=m switch
                {
                    0=>$"A group has {a} counters and another has {b}. " +
                       "How many more counters does the first group have?",
                    1=>$"One group has {b} counters. The other group has " +
                       $"{diff} more counters. How many does the larger group have?",
                    _=>$"A group has {a} counters, which is {diff} more " +
                       "than a second group. How many does the second group have?"
                };
                return Make(f,
                    $"{story} Use an aligned tape diagram to explain.",
                    "The longer bar equals the shorter bar plus the difference. " +
                    "Identify which of these three quantities is missing.",
                    AssessmentItemType.Numeric,
                    D(("longCount",a),("shortCount",b),
                      ("difference",diff),("unknownPosition",m)));
            }

            case "usccss.b01.g2.compare_tape_difference":
            {
                a=r.Next(8,20);b=r.Next(2,a);
                return Make(f,
                    $"One group has {a} blocks while a second group has {b} " +
                    "blocks. Show two aligned tape-diagram bars and find " +
                    "how many more blocks the first group has.",
                    "Align the bars at the same zero. The unmatched segment " +
                    "equals the longer count minus the shorter count.",
                    AssessmentItemType.Numeric,
                    D(("longCount",a),("shortCount",b),("difference",a-b)));
            }

            // Original IM 2021 G3 U01 L03: scaled picture-graph
            // interpretation. One printed symbol represents 2 or 5 people.
            case "usccss.b01.g3.picture_graph.scaled_compare":
            {
                a=r.Next(3,7);b=r.Next(1,a);m=r.Next(0,2)==0?2:5;
                return Make(f,
                    $"On a scaled picture graph each ● stands for {m} responses. " +
                    $"Category A has {a} symbols, while category B has {b}. " +
                    "How many more responses does category A represent?",
                    "Multiply each category's symbol count by the scale before " +
                    "comparing the represented frequencies.",
                    AssessmentItemType.Numeric,
                    D(("scale",m),("aIcons",a),("bIcons",b)));
            }
            // Original IM 2021 G3 U01 L04: construct a SCALED
            // picture graph from categorical frequencies.
            case "usccss.b01.g3.picture_graph.construct":
            {
                a=r.Next(2,6);b=r.Next(1,7);m=r.Next(0,2)==0?2:5;
                return Make(f,
                    $"Make a scaled picture graph. One ● represents {m} votes. " +
                    $"Category A received {a*m} votes and category B received {b*m}. " +
                    "How many ● symbols must you draw for category B?",
                    "Divide the B frequency by the value of one picture symbol. " +
                    "Use the same key for both categories.",
                    AssessmentItemType.Numeric,
                    D(("scale",m),("aIcons",a),("bIcons",b)));
            }

            // Official G4 U03 L13: the source requires eighth-inch data,
            // not merely the earlier Grade 3 quarter-inch frequency task.
            // Compare two actually plotted measurements using eighths.
            case "usccss.b01.g4.eighth_inch_line_plot_difference":
            {
                var whole=r.Next(7,10);
                var counts=Enumerable.Range(0,9).Select(_=>r.Next(1,4)).ToArray();
                var firstTick=r.Next(1,8);
                var lastTick=r.Next(firstTick+1,9);
                var data=Enumerable.Range(0,9)
                    .SelectMany(i=>Enumerable.Repeat(EighthLabel(whole,i),counts[i]))
                    .OrderBy(_=>r.Next()).ToArray();
                var parameters=Enumerable.Range(0,9)
                    .Select(i=>($"count{i}",counts[i])).ToList();
                parameters.Add(("whole",whole));
                parameters.Add(("firstTick",firstTick));
                parameters.Add(("lastTick",lastTick));
                return Make(f,
                    "The following shoe lengths in inches were measured to the nearest " +
                    "eighth: " + string.Join(", ",data) + ". Make a line plot " +
                    "with one X per observation and marks 1/8 inch apart. " +
                    $"Find the difference between {EighthLabel(whole,lastTick)} " +
                    $"and {EighthLabel(whole,firstTick)} inches. " +
                    "Give the answer in eighths of an inch (denominator 8).",
                    "Use the equally spaced eighth-inch ticks on the line plot: " +
                    $"({lastTick}-{firstTick})/8 = {lastTick-firstTick}/8 inch.",
                    AssessmentItemType.ShortAnswer,D(parameters.ToArray()));
            }

            // Source G3 U06 L04–L05: read/construct a line plot with
            // quarter- and half-inch measurements, not just whole units.
            case "usccss.b01.g3.quarter_inch_line_plot_frequency":
            {
                a=r.Next(1,5);b=r.Next(1,5);n=r.Next(1,5);
                d=r.Next(1,5);m=r.Next(1,5);
                var index=r.Next(0,5);
                var labels=new[]{"1","1 1/4","1 1/2","1 3/4","2"};
                var observations=new[]{a,b,n,d,m};
                var data=Enumerable.Range(0,5)
                    .SelectMany(i=>Enumerable.Repeat(labels[i],observations[i]))
                    .OrderBy(_=>r.Next()).ToArray();
                return Make(f,
                    "Lengths in inches measured to the nearest quarter-inch: " +
                    $"{string.Join(", ",data)}. Make or read a line plot with " +
                    "one X per item and marks 1/4 inch apart. " +
                    $"How many observations measure {labels[index]} inches?",
                    "Use five equal-spacing quarter-inch marks; count the X " +
                    "above the requested fraction or whole-inch tick.",
                    AssessmentItemType.Numeric,
                    D(("count1",a),("count2",b),("count3",n),
                      ("count4",d),("count5",m),("targetTick",index)));
            }

            // Original IM G2 U03 L16: ten actual measurements, nonzero
            // minimum tick, equal 1cm intervals, interpretable X counts.
            case "usccss.b01.g2.measurement.plot_interpret_nonzero_start":
            {
                var start=r.Next(21,29);
                var lastTick=r.Next(7,11);
                var counts=new int[11];
                counts[0]=1;
                counts[lastTick]=1;
                for(var j=0;j<8;j++)counts[r.Next(0,lastTick+1)]++;
                var targetTick=r.Next(0,lastTick);
                var mode=r.Next(0,3);
                var data=Enumerable.Range(0,lastTick+1)
                    .SelectMany(i=>Enumerable.Repeat(start+i,counts[i]))
                    .OrderBy(_=>r.Next()).ToArray();
                var parameters=new List<(string Key,int Value)>
                {
                    ("start",start),("lastTick",lastTick),
                    ("targetTick",targetTick),("mode",mode)
                };
                parameters.AddRange(Enumerable.Range(0,11)
                    .Select(i=>($"count{i}",counts[i])));
                var question=mode switch
                {
                    0=>$"How many plants are taller than {start+targetTick} cm?",
                    1=>"What is the difference in centimeters between the tallest and shortest plant?",
                    _=>$"How many plants are exactly {start+targetTick} cm tall?"
                };
                return Make(f,
                    "Ten measured plant heights in centimeters: " +
                    string.Join(", ",data) + ". Make a line plot " +
                    $"with one X per height and 1 cm marks beginning at {start}. " +
                    question,
                    "Begin at the smallest measurement, not at zero. Plot " +
                    "one X for each plant and count or subtract based on " +
                    "equally spaced centimeters.",
                    AssessmentItemType.Numeric,D(parameters.ToArray()));
            }

            // IM Grade 2 U09 L04: represent ten REAL (simulated) measured
            // pencil lengths, one X per centimeter observation, then
            // add/subtract the count of observations within 20.
            // IM G2 U03 L04: estimate, then measure with a 1cm ruler.
            // Longer (50–90cm) ruler iteration remains a separate source task.
            // Official IM G2 U03 L04: a long 50–90cm object requires
            // iterating a standard 30cm ruler end-to-end (no gaps/overlap).
            // Verified endpoints: 0, 30, 60, ... final remainder.
            case "usccss.b01.g2.measurement.iterate_ruler_cm":
            {
                var total=r.Next(50,91);
                var fullSpans=total/30;
                var remainder=total%30;
                return Make(f,
                    $"A classroom object is longer than a 30 cm ruler. " +
                    $"Move the ruler end-to-end without gaps or overlaps: " +
                    $"{fullSpans} full passes of 30 cm and a final " +
                    $"{remainder} cm. What is the object's total length?",
                    "Multiply the full 30cm ruler passes, then add the " +
                    "measured remainder without double-counting endpoints.",
                    AssessmentItemType.Numeric,
                    D(("fullSpans",fullSpans),("remainder",remainder),
                      ("total",total)));
            }

            // IM G2 U03 L05 reptile tape preparations, source measurements:
            // A 58, B 72, C 44, D 180, E 336, F 190, G 282 cm.
            // One meter is exactly 100 cm; no approximate conversion.
            case "usccss.b01.g2.measure.reptile_meterstick":
            {
                var idx=r.Next(0,7);
                var lengths=new[]{58,72,44,180,336,190,282};
                var names=new[]{"gila monster","baby alligator","baby cobra",
                    "komodo dragon","adult alligator","adult cobra","ribbon snake"};
                var lengthCm=lengths[idx];
                var fullMeters=lengthCm/100;
                var remainingCm=lengthCm%100;
                var mode=r.Next(0,3);
                return Make(f,
                    $"The {names[idx]} tape (label {(char)('A'+idx)}) " +
                    $"is {lengthCm} cm long. Use a meter stick (100 cm) " +
                    "without gaps or overlaps. " +
                    (mode switch
                    {
                        0=>"How many COMPLETE 1-meter sticks fit?",
                        1=>"How many centimeters remain after the complete meter sticks?",
                        _=>$"It measures {fullMeters} meter(s) and " +
                          $"{remainingCm} centimeter(s). How many centimeters in total?"
                    }),
                    "One complete meter stick is 100 cm. Count complete " +
                    "placements, then measure the remaining centimeters.",
                    AssessmentItemType.Numeric,
                    D(("tapeIndex",idx),("lengthCm",lengthCm),
                      ("fullMeters",fullMeters),("remainingCm",remainingCm),
                      ("mode",mode)));
            }

            // Official G2 U03 L08: 1–11 inch ruler measurement and estimation.
            // G1 U06 L11: Put Together result unknown, Take Apart
            // result unknown, Compare difference unknown — all <=20 cubes.
            case "usccss.b01.g1.measurement.story_result_difference":
            {
                var first=r.Next(4,12);
                var other=r.Next(2,Math.Min(10,21-first));
                var total=first+other;
                var mode=r.Next(0,3);
                var question=mode switch
                {
                    0=>$"One ribbon measures {first} cube units. Another " +
                       $"measures {other}. How long are they together?",
                    1=>$"A ribbon measures {total} cube units and one " +
                       $"part is {first}. How long is the remaining part?",
                    _=>$"A longer shoe measures {total} cube units and a " +
                      $"shorter shoe measures {first}. How much longer is the first?"
                };
                return Make(f,question+" Explain with a measured tape diagram.",
                    "Two lengths join to make the measured whole; a missing " +
                    "part or difference is found by subtracting.",
                    AssessmentItemType.Numeric,
                    D(("first",first),("other",other),("total",total),("mode",mode)));
            }

            // G1 U06 L12: bigger unknown using MORE, smaller using FEWER.
            case "usccss.b01.g1.measurement.compare_bigger_smaller_unknown":
            {
                var shorter=r.Next(4,12);
                var difference=r.Next(2,Math.Min(9,21-shorter));
                var longer=shorter+difference;
                var mode=r.Next(0,2);
                var question=mode==0
                    ?$"A shoe measures {shorter} unit cubes. A second is " +
                     $"{difference} cubes MORE. How long is the bigger shoe?"
                    :$"A ribbon measures {longer} cube units. Another is " +
                     $"{difference} cubes FEWER. How long is the shorter ribbon?";
                return Make(f,question+" Show two aligned length bars.",
                    "The longer measured bar is the shorter bar plus " +
                    "the unmatched difference.",
                    AssessmentItemType.Numeric,
                    D(("shorter",shorter),("difference",difference),
                      ("longer",longer),("mode",mode)));
            }

            case "usccss.b01.g2.measurement.inch_ruler":
            {
                var actual=r.Next(1,12);
                var offset=r.Next(1,Math.Min(4,actual+1));
                var estimate=Math.Max(1,actual+(r.Next(0,2)==0?-offset:offset));
                if(estimate==actual)estimate=actual+1;
                return Make(f,
                    $"An object is estimated to be {estimate} inches long. " +
                    $"On an inch ruler starting at 0, the object's endpoint is {actual}. " +
                    "How many inches different are the estimate and measurement?",
                    "Count one-inch intervals from the zero endpoint, then " +
                    "compare the original estimate to the actual length.",
                    AssessmentItemType.Numeric,
                    D(("actual",actual),("estimate",estimate)));
            }

            // Original G2 U03 L09 teacher strips: bass 2ft, shark 4ft,
            // catfish 3ft. Children compare physically counted units,
            // NOT perform calculated feet-to-inches conversions.
            case "usccss.b01.g2.measurement.feet_inch_same_length":
            {
                var fish=r.Next(0,3);
                var inches=fish switch {0=>24,1=>48,_=>36};
                var rulerUnits=fish switch {0=>2,1=>4,_=>3};
                // Four genuinely distinct source-aligned requests per fish:
                // compare larger/smaller UNIT COUNTS, or read each concrete
                // physical count. Three original tape lengths x four prompts
                // provide 12 semantic cases (>= 10 unique session tasks).
                var mode=r.Next(0,4);
                var species=fish switch
                {
                    0=>"largemouth bass",1=>"spiny dogfish shark",_=>"catfish"
                };
                return Make(f,
                    $"A {species} tape strip has been physically measured with " +
                    $"{inches} one-inch tiles and also {rulerUnits} complete " +
                    "foot-length rulers. The strip itself does not change. " +
                    (mode switch
                    {
                        0=>"Which measurement counts MORE units? Enter 1 for inch tiles, 2 for foot-length rulers.",
                        1=>"Which measurement counts FEWER units? Enter 1 for inch tiles, 2 for foot-length rulers.",
                        2=>"How many individual ONE-INCH tile units measured this strip?",
                        _=>"How many complete FOOT-LENGTH ruler units measured this strip?"
                    }),
                    "The same length can be counted in different units. " +
                    "Small inch tiles have the larger count, while longer foot " +
                    "rulers have the smaller count. Read each given count exactly.",
                    AssessmentItemType.Numeric,
                    D(("fish",fish),("inchTiles",inches),
                      ("footRulers",rulerUnits),("mode",mode)));
            }

            case "usccss.b01.g2.measurement.estimate_cm":
            {
                var actual=r.Next(5,31);
                var error=r.Next(1,Math.Min(6,actual));
                var estimate=r.Next(0,2)==0 ? actual-error : actual+error;
                return Make(f,
                    $"A classroom object was estimated to be {estimate} cm long. " +
                    $"A ruler placed at zero shows its endpoint at {actual} cm. " +
                    "How many centimeters off was the estimate?",
                    "Measure with equally spaced 1 cm marks starting at zero, " +
                    "then compare the estimate with the measured length.",
                    AssessmentItemType.Numeric,
                    D(("actual",actual),("estimate",estimate),("error",error)));
            }

            case "usccss.b01.g2.measurement.pencil_line_plot":
            {
                var startCm=r.Next(8,12);
                var counts=new int[7];
                counts[0]=1;
                counts[6]=1;
                for(var i=0;i<8;i++)counts[r.Next(0,7)]++;
                var targetTick=r.Next(0,7);
                var queryMode=r.Next(0,3);
                var observed=Enumerable.Range(0,7)
                    .SelectMany(i=>Enumerable.Repeat(startCm+i,counts[i]))
                    .OrderBy(_=>r.Next()).ToArray();
                var question=queryMode switch
                {
                    0=>$"How many pencils are at most {startCm+targetTick} cm long?",
                    1=>$"How many pencils are longer than {startCm+targetTick} cm?",
                    _=>$"Find the difference between the number of pencils longer than " +
                       $"{startCm+targetTick} cm and the number at most that long. " +
                       "How many more pencils are in the larger group?"
                };
                var parameters=new List<(string Key,int Value)>
                {
                    ("start",startCm),("targetTick",targetTick),("mode",queryMode)
                };
                parameters.AddRange(Enumerable.Range(0,7)
                    .Select(i=>($"count{i}",counts[i])));
                return Make(f,
                    $"Ten measured pencil lengths (cm): {string.Join(", ",observed)}. " +
                    "Plot one X for each pencil over the correct centimeter mark. " +
                    question,
                    "Each pencil contributes exactly one X. Use the actual " +
                    "1 cm scale and count both groups, then add or subtract " +
                    "the measured frequencies to answer.",
                    AssessmentItemType.Numeric,D(parameters.ToArray()));
            }

            case "usccss.b01.g2.measurement.read_line_plot":
            {
                a=r.Next(1,5);b=r.Next(1,5);m=r.Next(1,5);n=r.Next(1,5);
                var target=r.Next(2,6);
                var data=Enumerable.Repeat(2,a).Concat(Enumerable.Repeat(3,b))
                    .Concat(Enumerable.Repeat(4,m)).Concat(Enumerable.Repeat(5,n))
                    .OrderBy(_=>r.Next()).ToArray();
                return Make(f,
                    $"A group measured the following objects in centimeters: " +
                    $"{string.Join(", ",data)}. Draw or read an X-mark line plot. " +
                    $"How many objects are exactly {target} cm long?",
                    "Plot one X above every measured length, then count the X marks " +
                    "stacked above the requested tick.",
                    AssessmentItemType.Numeric,
                    D(("count2",a),("count3",b),("count4",m),("count5",n),
                      ("targetLength",target)));
            }

            case "usccss.b01.ruler.compare_centimeters":
            {
                a=r.Next(4,16);b=r.Next(2,a);
                return Make(f,
                    $"One object measures {a} cm from zero on a ruler. " +
                    $"A second object measures {b} cm with the same 1 cm ticks. " +
                    "How many centimeters longer is the first?",
                    "Compare the two lengths using equal centimeter marks. " +
                    "Subtract the shorter length from the longer length.",
                    AssessmentItemType.Numeric,
                    D(("longLength",a),("shortLength",b),("difference",a-b)));
            }
            // IM 2021 G2 U03 L10: measure despite a nonzero starting tick.
            case "usccss.b01.ruler.measure_without_zero":
            {
                a=r.Next(2,10);b=r.Next(3,14);n=a+b;
                return Make(f,
                    $"An object starts at the {a} cm mark and ends at " +
                    $"the {n} cm mark on a ruler. It does NOT begin at zero. " +
                    "What is the object's actual length in centimeters?",
                    "Count the 1 cm intervals between the two endpoints " +
                    "or subtract the start mark from the end mark.",
                    AssessmentItemType.Numeric,
                    D(("start",a),("end",n),("length",b)));
            }

            // Same physical tape measured with original teacher's 1in and
            // 2in paper clips. Changing unit size changes the count.
            case "usccss.b01.g1.measure.same_length_different_units":
            {
                var inches=new[]{12,16,18,20,24}[r.Next(0,5)];
                var mode=r.Next(0,3);
                return Make(f,
                    $"A tape is {inches} inches long. Measure the SAME " +
                    "tape with 1-inch paper clips and 2-inch paper clips, " +
                    "without gaps or overlaps. " +
                    (mode switch
                    {
                        0=>"How many 1-inch clips are needed?",
                        1=>"How many 2-inch clips are needed?",
                        _=>"How many MORE 1-inch clips than 2-inch clips are needed?"
                    }),
                    "The physical length remains unchanged. Larger units " +
                    "need fewer clips for the same object.",
                    AssessmentItemType.Numeric,
                    D(("tapeInches",inches),("oneInchCount",inches),
                      ("twoInchCount",inches/2),("mode",mode)));
            }
            case "usccss.b01.measure.map_lengths":
                a = r.Next(2, 11);
                b = r.Next(2, Math.Min(21 - a, 11));
                m = r.Next(0, 2);
                return Make(f,
                    m == 0
                        ? $"Two adjoining paths on a map measure {a} cm and {b} cm. " +
                          "What is their combined measured length in centimeters?"
                        : $"Two route lengths measured on a map are {a+b} cm and {a} cm. " +
                          "How many centimeters longer is the first?",
                    "Add adjoining lengths, or subtract to compare the measured paths.",
                    AssessmentItemType.Numeric, D(("a", a), ("b", b), ("mode", m)));

            case "usccss.b01.graph.choose_scale":
                a = new[] { 2, 5, 10 }[r.Next(0, 3)];
                n = r.Next(0, 5);
                return Make(f,
                    $"A bar graph should show counts {a}, {a*(n%3+1)}, {a*(3+(n%2))}, and {4*a}. " +
                    $"Which convenient vertical-axis scale fits the data in exactly four equal steps: " +
                    $"1, {a}, or {a*2}? Enter the chosen step size.",
                    "Choose equal intervals so the largest bar reaches the fourth tick.",
                    AssessmentItemType.Numeric, D(("step", a), ("dataVariant", n)));

            // Publisher G3 U01 L07: within-100 "how many more/fewer"
            // comparisons grounded in the units of a scaled bar chart.
            case "usccss.b01.graph.scaled_compare":
                m = new[] { 2, 5, 10 }[r.Next(0,3)];
                a = r.Next(2, 9);
                b = r.Next(1, 9);
                if (a == b) b = b == 8 ? 7 : b+1;
                n = r.Next(0,2);
                return Make(f,
                    $"A bar graph uses a scale of {m} items per tick. " +
                    $"Category A reaches {a} ticks ({a*m} items) and category B " +
                    $"reaches {b} ticks ({b*m} items). " +
                    (n==0 ? "How many more items does the larger category have?"
                          : "How many fewer items does the smaller category have?"),
                    $"Each bar represents {m} items per marked tick. " +
                    "Find both category counts, then subtract the smaller from the larger.",
                    AssessmentItemType.Numeric,
                    D(("unit",m),("ticksA",a),("ticksB",b),
                      ("countA",a*m),("countB",b*m),("mode",n)));
            case "usccss.b01.array.equal_groups":
                a = r.Next(2, 5 + level);
                b = r.Next(2, Math.Min(11, 101 / a));
                return Make(f,
                    $"Arrange objects into {a} equal rows with {b} objects in each row. " +
                    "How many objects are in the rectangular array?",
                    "An array contains rows × objects per row; its total can also be counted by equal groups.",
                    AssessmentItemType.Numeric, D(("rows", a), ("columns", b)));

            case "usccss.b01.array.columns":
                a = r.Next(2, 6 + level); b = r.Next(2, 7 + level);
                return Make(f,
                    $"An array has {a} equal rows with {b} objects in each row. " +
                    "How many columns does this array have?",
                    "Each column has one item from every row; the number of " +
                    "columns equals the objects in each row.",
                    AssessmentItemType.Numeric, D(("rows", a), ("columns", b)));
            case "usccss.b01.array.equal_addends":
                a = r.Next(2, 6 + level); b = r.Next(2, 7 + level);
                return Make(f,
                    $"An array has {a} rows of {b} objects. " +
                    "Write the equal-addends expression representing all rows (use +).",
                    "Each row contributes the same addend; repeat it once per row.",
                    AssessmentItemType.ShortAnswer, D(("rows", a), ("columns", b)));
            case "usccss.b01.array.equal_equation":
                a = r.Next(2, 6 + level); b = r.Next(2, 7 + level);
                return Make(f,
                    $"An array has {a} equal rows of {b} objects. " +
                    "Write an addition equation that represents the array " +
                    "(equal addends on the left, total on the right).",
                    $"Add the same addend {b} for every row, then write the " +
                    $"total equal to {a*b}.",
                    AssessmentItemType.ShortAnswer, D(("rows", a), ("columns", b)));
            // Original IM G3 U08 L10: fluency within 100 by reasoning
            // from the SAME rectangular array in both × and ÷ directions.
            // Original IM G2 U06 L06: compose square or non-square
            // rectangle from congruent small squares, with no gaps.
            case "usccss.b01.g2.compose_equal_square_shapes":
            {
                a=r.Next(2,8);m=r.Next(0,2);
                // 6 square sizes and 30 distinct non-square row/column
                // pairs; enough source-valid diversity for 10 unique items.
                b=m==0?a:r.Next(2,7);
                if(m==1 && b>=a)b++;
                return Make(f,
                    $"Place {a*b} equal-size square tiles in {a} rows " +
                    $"and {b} columns. Do they form a square or a non-square rectangle?",
                    "Squares have equal side lengths. A grid with unequal " +
                    "numbers of rows and columns is a non-square rectangle.",
                    AssessmentItemType.ShortAnswer,
                    D(("rows",a),("columns",b),("mode",m),("tiles",a*b)));
            }
            case "usccss.b01.g3.multiplication_division_inverse_array":
            {
                a=r.Next(2,11);b=r.Next(2,11);m=r.Next(0,2);
                return Make(f,
                    m==0
                        ?$"An array has {a} rows of {b} equal objects. " +
                         "How many objects are there altogether? " +
                         $"Use {a} × {b} and the inverse division fact."
                        :$"There are {a*b} objects arranged equally into " +
                         $"{a} rows. How many are in each row? " +
                         $"Use {a*b} ÷ {a} and its inverse multiplication fact.",
                    $"The same {a} by {b} array proves both " +
                    $"{a} × {b} = {a*b} and {a*b} ÷ {a} = {b}.",
                    AssessmentItemType.Numeric,
                    D(("rows",a),("columns",b),("total",a*b),("operation",m)));
            }
            case "usccss.b01.array.unknown_factor":
                a = r.Next(2, 6 + level); b = r.Next(2, 7 + level);
                return Make(f,
                    $"An array contains {a} equal rows and {a*b} objects overall. " +
                    "How many objects are in each row?",
                    "Find the unknown factor by dividing the total by the number of rows.",
                    AssessmentItemType.Numeric, D(("rows", a), ("columns", b)));
            case "usccss.b01.array.commutativity":
                a = r.Next(2, 6 + level); b = r.Next(2, 7 + level);
                return Make(f,
                    $"An array has {a} rows and {b} columns. " +
                    "Rotate it a quarter-turn. How many rows does it have after rotation?",
                    $"A quarter-turn exchanges rows and columns: {a} × {b} = {b} × {a}.",
                    AssessmentItemType.Numeric, D(("rows", a), ("columns", b)));
            // Publisher G3 U02 L04: count square tiles, area <= 60.
            case "usccss.b01.geometry.rectangle_area_grid60":
                a = r.Next(2,9);
                b = r.Next(2,61/a);
                return Make(f,
                    $"A rectangle is tiled with {a} rows of {b} " +
                    "equal unit squares and no gaps. How many square units is its area?",
                    "One unit square in every grid cell; area = rows × columns.",
                    AssessmentItemType.Numeric,D(("rows",a),("columns",b)));

            // Publisher G3 U02 L08: area without full grid.
            // IM G3 U02 L07/L10: measured rectangle coverage in
            // square feet or square meters and concrete real-life contexts.
            case "usccss.b01.geometry.rectangle_area_real_world_units":
            {
                a=r.Next(3,11);b=r.Next(2,10);m=r.Next(0,2);
                var measure=m==0?"meters":"feet";
                var place=m==0?"a rectangular school garden":"a reading rug";
                return Make(f,
                    $"The dimensions of {place} are {a} {measure} by " +
                    $"{b} {measure}. How many SQUARE {measure} cover its area?",
                    "The unit square has side length one linear unit; " +
                    "multiply the measured side lengths to count the unit squares.",
                    AssessmentItemType.Numeric,
                    D(("length",a),("width",b),("unitMode",m)));
            }

            // IM 2021 G3 U07 L11: equal perimeter does NOT force equal area.
            case "usccss.b01.g3.same_perimeter_different_area":
            {
                var sideSum=r.Next(7,14);
                a=r.Next(2,Math.Min(6,(sideSum-2)/2)+1);
                b=sideSum-a;n=a+1;m=sideSum-n;
                return Make(f,
                    $"Rectangle A has width {a} and height {b}. Rectangle B " +
                    $"has width {n} and height {m}. Their perimeters are equal. " +
                    "How many more square units does rectangle B cover?",
                    "Both perimeters equal twice the sum of adjacent sides. " +
                    "Their areas differ: multiply length by width separately.",
                    AssessmentItemType.Numeric,
                    D(("widthA",a),("heightA",b),("widthB",n),("heightB",m),
                      ("expected",n*m-a*b)));
            }
            // IM 2021 G3 U07 L12: equal area does NOT force equal perimeter.
            case "usccss.b01.g3.same_area_different_perimeter":
            {
                var pairs=(
                    from aw in Enumerable.Range(2,7)
                    from ah in Enumerable.Range(3,10)
                    from bw in Enumerable.Range(3,7)
                    from bh in Enumerable.Range(2,11)
                    where aw<bw && aw*ah==bw*bh &&
                          aw+ah>bw+bh
                    select (aw,ah,bw,bh)).ToArray();
                var selected=pairs[r.Next(pairs.Length)];
                a=selected.aw;b=selected.ah;
                n=selected.bw;m=selected.bh;
                return Make(f,
                    $"Rectangle A has width {a} and height {b}. Rectangle B " +
                    $"has width {n} and height {m}. Their areas are equal. " +
                    "By how many units is perimeter A greater than perimeter B?",
                    "The same number of unit squares can have a different " +
                    "boundary length. Calculate each perimeter independently.",
                    AssessmentItemType.Numeric,
                    D(("widthA",a),("heightA",b),("widthB",n),("heightB",m),
                      ("expected",2*(a+b-n-m))));
            }

            // IM Grade 4 U01 L01: an area is a multiple of either
            // integer side length of a rectangle.
            case "usccss.b01.g4.rectangle_area_is_multiple":
            {
                a=r.Next(2,12);b=r.Next(2,11);
                return Make(f,
                    $"An area model has {a} equal rows with {b} unit squares " +
                    $"per row. What is the area in unit squares? " +
                    $"Explain why this result is a multiple of BOTH {a} and {b}.",
                    "The total area equals rows × units per row; either " +
                    "side length is a factor, so the product is a multiple of both.",
                    AssessmentItemType.Numeric,
                    D(("length",a),("width",b),("expected",a*b)));
            }
            // IM Grade 3 U04 L10: multiplication distributive property
            // proved by two adjacent, non-overlapping area-model parts.
            case "usccss.b01.g3.distributive_area_partition":
            {
                a=r.Next(2,9);b=r.Next(1,7);n=r.Next(1,7);
                return Make(f,
                    $"A rectangular array has {a} rows. A vertical partition " +
                    $"splits its columns into {b} and {n}. " +
                    $"Use {a}×({b}+{n}) = {a}×{b} + {a}×{n}. " +
                    "What is the total area in unit squares?",
                    "Count the two disjoint rectangular regions. " +
                    "The distributive property recombines their areas.",
                    AssessmentItemType.Numeric,
                    D(("height",a),("leftWidth",b),("rightWidth",n),
                      ("expected",a*(b+n))));
            }
            // IM Grade 3 U07 L10: both area AND perimeter in a
            // contextual rectangle; never conflate square and linear units.
            case "usccss.b01.g3.rectangle_area_perimeter_story":
            {
                a=r.Next(3,11);b=r.Next(2,10);m=r.Next(0,2);
                return Make(f,
                    $"A rectangular playground measures {a} meters by {b} meters. " +
                    (m==0
                       ?"How many square meters of surface does it cover?"
                       :"How many meters of boundary fencing are needed?"),
                    m==0
                       ?"Area counts unit squares: multiply the two side lengths."
                       :"Perimeter counts boundary lengths: add all four sides.",
                    AssessmentItemType.Numeric,
                    D(("length",a),("width",b),("mode",m),
                      ("expected",m==0?a*b:2*(a+b))));
            }

            case "usccss.b01.geometry.rectangle_area_ungridded":
                a = r.Next(2,11);b = r.Next(2,11);
                return Make(f,
                    $"An ungridded rectangle is {a} units long and " +
                    $"{b} units wide. Find its area in square units.",
                    "Rectangle area is length × width, even when no square grid is drawn.",
                    AssessmentItemType.Numeric,D(("length",a),("width",b)));

            case "usccss.b01.geometry.composite_rectangle_area":
                a = r.Next(2, 6); b = r.Next(2, 7); n = r.Next(1, 5);
                return Make(f,
                    $"A floor consists of two non-overlapping rectangles, both {a} units tall, " +
                    $"with widths {b} units and {n} units. What is their COMBINED area in square units?",
                    "Find each rectangular area, add them, and check by multiplying the combined width by height.",
                    AssessmentItemType.Numeric, D(("height", a), ("width1", b), ("width2", n)));

            case "usccss.b01.division.unknown_factor":
                a = r.Next(2, 11); b = r.Next(2, 11);
                return Make(f,
                    $"An unknown number of equal groups each has {a} objects. " +
                    $"Altogether there are {a*b} objects. What is the unknown number of groups?",
                    "Use the missing-factor equation: groups × objects in each group = total.",
                    AssessmentItemType.Numeric, D(("objectsPerGroup", a), ("groups", b)));

            case "usccss.b01.fraction.unit_parts":
                d = new[] { 2, 3, 4, 6, 8 }[r.Next(0,5)];
                m = r.Next(0,3);
                return Make(f,
                    $"A {(m==0 ? "circle" : m==1 ? "square" : "rectangle")} " +
                    $"is divided into {d} EQUAL parts. Exactly one part is shaded. " +
                    "Write the unit fraction represented by that single part.",
                    "A unit fraction has numerator 1 because exactly one " +
                    "out of the equal parts is chosen; the denominator counts all equal parts.",
                    AssessmentItemType.ShortAnswer,
                    D(("denominator",d),("shape",m)));
            // Official IM 2021 G4 U03 L16 and L17: bridge tenths
            // and hundredths through denominator 100, preserving each
            // lesson's below/above-one boundary without rounding.
            case "usccss.b01.fraction.tenths_hundredths_within_one":
                a = r.Next(1,10);
                b = r.Next(1,101-10*a);
                n = 10*a+b;
                return Make(f,
                    $"Use equivalent fractions to add {a}/10 + {b}/100. " +
                    "Write your answer in hundredths (denominator 100, do not simplify).",
                    $"{a}/10 = {10*a}/100, so {10*a}/100 + {b}/100 = {n}/100. " +
                    "The total is at most one whole.",
                    AssessmentItemType.ShortAnswer,
                    D(("tenths",a),("hundredths",b),("totalHundredths",n)));
            case "usccss.b01.fraction.tenths_hundredths_over_one":
                a = r.Next(2,10);
                b = r.Next(101-10*a,100);
                n = 10*a+b;
                return Make(f,
                    $"Use equivalent fractions to add {a}/10 + {b}/100. " +
                    "Write your answer in hundredths (denominator 100, do not simplify).",
                    $"{a}/10 = {10*a}/100, so {10*a}/100 + {b}/100 = {n}/100. " +
                    $"That is 1 and {n-100}/100, greater than one whole.",
                    AssessmentItemType.ShortAnswer,
                    D(("tenths",a),("hundredths",b),("totalHundredths",n)));
            case "usccss.b01.fraction.name_parts":
                d = new[] { 2, 3, 4, 6, 8 }[r.Next(0, 5)];
                n = r.Next(1, d);
                return Make(f,
                    $"One whole is partitioned into {d} EQUAL parts and {n} of those parts are shaded. " +
                    "Write the shaded fraction in numerator/denominator form (do not simplify).",
                    "The numerator counts shaded parts; the denominator counts equal pieces in the whole.",
                    AssessmentItemType.ShortAnswer, D(("numerator", n), ("denominator", d)));

            case "usccss.b01.measure.quarter_inch":
                // 1/4, 2/4 and 3/4 are all valid ruler marks. The middle
                // half-inch mark adds a genuinely different measured length.
                n = r.Next(1, 4);
                a = r.Next(0, 4);
                return Make(f,
                    $"An inch ruler is marked in FOURTHS. A segment stops at {a} whole inch(es) " +
                    $"plus {n} quarter-inch mark(s). Write its length as a fraction " +
                    "of an inch (keep denominator 4).",
                    "Count whole inches as 4 quarter-inch parts each, then add the remaining parts.",
                    AssessmentItemType.ShortAnswer, D(("quarters", n), ("wholeInches", a)));

            // IM Grade 4 U01 L02: MULTIPLE different factor pairs for
            // the same numerical area, shown by different rectangles.
            case "usccss.b01.g4.factor_pairs_two_rectangles":
            {
                var pairs=(
                    from area in Enumerable.Range(12,61)
                    from wa in Enumerable.Range(2,11)
                    from wb in Enumerable.Range(2,11)
                    where wa<wb && area%wa==0 && area%wb==0
                    let ha=area/wa
                    let hb=area/wb
                    where wa<=ha && wb<=hb && ha<=12 && hb<=12
                    select (wa,ha,wb,hb,area)).ToArray();
                var chosen=pairs[r.Next(pairs.Length)];
                return Make(f,
                    $"A rectangle of {chosen.wa} by {chosen.ha} square tiles " +
                    $"has area {chosen.area}. Construct a DIFFERENT rectangle " +
                    $"with the SAME area and width {chosen.wb}. " +
                    "What is its height in unit tiles?",
                    "A pair of side lengths is a factor pair when their " +
                    "product equals the number of unit squares. " +
                    "The two different factor pairs must multiply to the same area.",
                    AssessmentItemType.Numeric,
                    D(("widthA",chosen.wa),("heightA",chosen.ha),
                      ("widthB",chosen.wb),("heightB",chosen.hb),
                      ("area",chosen.area)));
            }

            case "usccss.b01.factors.factor_pairs":
                a = r.Next(2, 9); b = r.Next(2, 10);
                return Make(f,
                    $"A rectangular tile design has an area of {a*b} unit squares. " +
                    $"One side is {a} unit squares long. What is its other factor/side length?",
                    "A factor pair makes the area: first side × second side = area.",
                    AssessmentItemType.Numeric, D(("firstFactor", a), ("secondFactor", b)));

            case "usccss.b01.factors.prime_composite":
                n = r.Next(2, 101);
                return Make(f,
                    $"Classify {n}: prime or composite? " +
                    "A prime number has exactly two positive factors.",
                    "Check whether any whole number from 2 through the square root divides the number.",
                    AssessmentItemType.ShortAnswer, D(("number", n)));

            case "usccss.b01.factors.locker_square":
                n = r.Next(1, 21);
                return Make(f,
                    $"In the locker problem, locker {n} is toggled once for every positive factor of {n}. " +
                    "It starts closed. Is it open or closed after all toggles?",
                    "Only a perfect square has an odd number of positive factors, so it ends open.",
                    AssessmentItemType.ShortAnswer, D(("locker", n)));

            case "usccss.b01.factors.factors_multiples":
                a = r.Next(2, 11); b = r.Next(2, 11);
                m = r.Next(0, 2);
                n = r.Next(0, 2);
                return Make(f,
                    m == 0
                        ? $"Is {a} a factor of {a*b+(n==0 ? 0 : 1)}? Answer yes or no."
                        : $"Is {a*b+(n==0 ? 0 : 1)} a multiple of {a}? Answer yes or no.",
                    "A factor divides the number without a remainder; a multiple is a product with a whole number.",
                    AssessmentItemType.ShortAnswer, D(("a", a), ("b", b), ("mode", m), ("offset", n)));

            case "usccss.b01.fraction.whole_multiply":
                d = r.Next(3, 10); n = r.Next(2, d); a = r.Next(2, 6);
                return Make(f,
                    $"Find {a} groups of the non-unit fraction {n}/{d}. " +
                    "Write the result as a fraction, simplifying if possible.",
                    "Add equal fractions or multiply the numerator by the number of groups, and simplify.",
                    AssessmentItemType.ShortAnswer, D(("numerator", n), ("denominator", d), ("whole", a)));

            case "usccss.b01.fraction.decompose_same_denominator":
                d = r.Next(4, 11); n = r.Next(2, d+1); a = r.Next(1, n);
                return Make(f,
                    $"Decompose {n}/{d} as {a}/{d} + __/{d}. " +
                    "What fraction completes this equality (keep the same denominator)?",
                    "With identical denominators, the missing numerator is the difference of the numerators.",
                    AssessmentItemType.ShortAnswer, D(("wholeN", n), ("firstN", a), ("denominator", d)));

            case "usccss.b01.place_value.ten_times":
                a = r.Next(1, 10); b = r.Next(1, 4);
                var power = b == 1 ? 10 : b == 2 ? 100 : 1000;
                return Make(f,
                    $"The digit {a} has value {a*power} in one number and value {a*power*10} " +
                    "in another number. How many times as large is its place value in the second?",
                    "Moving a digit one place to the left multiplies its value by ten.",
                    AssessmentItemType.Numeric, D(("digit", a), ("place", power)));
            default: throw new InvalidOperationException("Unknown source-target problem family.");
        }
    }

    private static int Gcd(int a, int b)
    {
        while (b != 0) { (a, b) = (b, a % b); }
        return a;
    }

    private static string ReducedFraction(int n, int d)
    {
        var g = Gcd(n, d);
        return (n / g).ToString(CultureInfo.InvariantCulture) + "/" +
            (d / g).ToString(CultureInfo.InvariantCulture);
    }

    public static string Solve(string f, IReadOnlyDictionary<string, int> p)
    {
        int P(string key) => p[key];
        return f switch
        {
            "usccss.b01.g1.add_one_or_two" =>
                (P("start")+P("step")).ToString(CultureInfo.InvariantCulture),
            "usccss.b01.g1.subtract_one_or_two" =>
                (P("start")-P("step")).ToString(CultureInfo.InvariantCulture),
            "usccss.b01.g1.make_ten_missing_part" =>
                (10-P("known")).ToString(CultureInfo.InvariantCulture),
            "usccss.b01.g3.add_within1000" or
            "usccss.b01.g3.add_regroup1000" =>
                (P("left")+P("right")).ToString(CultureInfo.InvariantCulture),
            "usccss.b01.g3.subtract_within1000" or
            "usccss.b01.g3.subtract_regroup1000" or
            "usccss.b01.g3.subtract_across_zero" =>
                (P("left")-P("right")).ToString(CultureInfo.InvariantCulture),
            "usccss.b01.market.inventory_100" =>
                (P("mode") == 0 ? P("second") - P("sold") : P("third"))
                    .ToString(CultureInfo.InvariantCulture),
            "usccss.b01.g1.teen_no_regroup" or "usccss.b01.g1.teen_subtract_decompose_ten" or "usccss.b01.g2.story_without_regroup100" or "usccss.b01.g2.story_regroup50" or
            "usccss.b01.g2.add_subtract_without_regroup" or
            "usccss.b01.g2.subtract_decompose_ten" =>
                P("expected").ToString(CultureInfo.InvariantCulture),
            "usccss.b01.g1.place_value_number_riddle" =>
                P("number").ToString(CultureInfo.InvariantCulture),
            "usccss.b01.g2.target_measurement_difference_cm" =>
                Math.Abs(P("target")-P("measured")).ToString(CultureInfo.InvariantCulture),
            "usccss.b01.g3.quarter_inch_estimate_classify" =>
                P("mode") switch { 0 => "too low", 1 => "about right",
                    2 => "too high", _ => throw new InvalidOperationException() },
            "usccss.b01.g1.add_within100_no_compose_ten" =>
                (P("left")+P("right")).ToString(CultureInfo.InvariantCulture),
            "usccss.b01.g2.add_subtract_1000_no_regroup" =>
                P("expected").ToString(CultureInfo.InvariantCulture),
            "usccss.b01.g4.decimal_number_line_greater" =>
                (Math.Max(P("first"),P("second"))/100m)
                    .ToString("0.00",CultureInfo.InvariantCulture),
            "usccss.b01.g2.order_three_digits_number_line" =>
                Math.Max(P("first"),P("second"))
                    .ToString(CultureInfo.InvariantCulture),
            "usccss.b01.g4.whole_minus_proper_fraction" =>
                ReducedFraction(P("whole")*P("denominator")-P("numerator"),
                    P("denominator")),
            "usccss.b01.g1.compare_order_tens_ones" =>
                (P("position")==0?Math.Max(P("first"),Math.Max(P("second"),P("third"))):
                 P("position")==1?Math.Min(P("first"),Math.Min(P("second"),P("third"))):
                 new[]{P("first"),P("second"),P("third")}.OrderBy(x=>x).ElementAt(1))
                 .ToString(CultureInfo.InvariantCulture),
            "usccss.b01.g1.add_subtract_10_tens" or "usccss.b01.g1.add_subtract_tens_multiples" =>
                P("expected").ToString(CultureInfo.InvariantCulture),
            "usccss.b01.place_value.tens_ones" or
            "usccss.b01.place_value.count_to120" or
            "usccss.b01.g1.measure.count_cubes_up_to110" or
            "usccss.b01.g1.measure.write_cubes_to120" or
            "usccss.b01.place_value.three_digits" =>
                P("number").ToString(CultureInfo.InvariantCulture),
            "usccss.b01.g1.match_dot_cubes_expression" =>
                $"{P("first")}+{P("second")}",
            "usccss.b01.g1.complete_missing_dots" =>
                P("second").ToString(CultureInfo.InvariantCulture),
            "usccss.b01.g1.check_off_add_within10" =>
                (P("first")+P("second")).ToString(CultureInfo.InvariantCulture),
            "usccss.b01.shape.polygon_sides" =>
                P("sides") switch
                {
                    3=>"triangle",4=>"quadrilateral",5=>"pentagon",
                    6=>"hexagon",_=>throw new InvalidOperationException()
                },
            "usccss.b01.shape.specific_subcategory" =>
                P("subcategory") switch
                {
                    0=>"equilateral triangle",1=>"right triangle",
                    2=>"rectangle",3=>"square",_=>throw new InvalidOperationException()
                },
            "usccss.b01.area.missing_composite_side" =>
                (P("heightLeft")*P("widthKnown")+
                 P("heightRight")*P("widthMissing")).ToString(CultureInfo.InvariantCulture),
            "usccss.b01.triangle.attributes" => P("position").ToString(CultureInfo.InvariantCulture),
            "usccss.b01.partition.halves_fourths" => P("denominator") == 2 ? "half" : "fourth",
            "usccss.b01.number_line.move_left_right_within100" =>
                P("end").ToString(CultureInfo.InvariantCulture),
            "usccss.b01.number_line.estimate_location_within100" =>
                P("nearest").ToString(CultureInfo.InvariantCulture),
            "usccss.b01.number_line.locate_whole_to100" or
            "usccss.b01.number_line.locate_three_digits" or
            "usccss.b01.number_line.locate_large_numbers" =>
                (P("step")*P("tick")).ToString(CultureInfo.InvariantCulture),
            "usccss.b01.number_line.equal_intervals" =>
                (2 * P("step")).ToString(CultureInfo.InvariantCulture),
            "usccss.b01.clock.read_on_hour" =>
                $"{P("hour")}:00",
            "usccss.b01.clock.read_five_minutes_am_pm" =>
                $"{P("hour")}:{P("minutes"):00} {(P("period")==0?"a.m.":"p.m.")}",
            "usccss.b01.clock.read_half_hour" or
            "usccss.b01.clock.read_five_minutes" =>
                $"{P("hour")}:{P("minutes"):00}",
            "usccss.b01.clock.half_quarters" =>
                (P("position") + 1).ToString(CultureInfo.InvariantCulture) switch
                {
                    "1" => "15", "2" => "30", "3" => "45", _ => throw new InvalidOperationException()
                },
            "usccss.b01.rectangle.unit_squares" or
            "usccss.b01.array.equal_groups" =>
                (P("rows") * P("columns")).ToString(CultureInfo.InvariantCulture),
            "usccss.b01.g2.compose_equal_square_shapes" => P("mode")==0?"square":"rectangle",
            "usccss.b01.g3.multiplication_division_inverse_array" =>
                (P("operation")==0?P("total"):P("columns"))
                    .ToString(CultureInfo.InvariantCulture),
            "usccss.b01.array.columns" or
            "usccss.b01.array.unknown_factor" or
            "usccss.b01.array.commutativity" =>
                P("columns").ToString(CultureInfo.InvariantCulture),
            "usccss.b01.array.equal_addends" =>
                string.Join("+", Enumerable.Repeat(
                    P("columns").ToString(CultureInfo.InvariantCulture), P("rows"))),
            "usccss.b01.array.equal_equation" =>
                string.Join("+", Enumerable.Repeat(
                    P("columns").ToString(CultureInfo.InvariantCulture), P("rows"))) +
                "=" + (P("rows")*P("columns")).ToString(CultureInfo.InvariantCulture),
            "usccss.b01.g2.length_compare.all_unknown_positions100" =>
                (P("unknownPosition") switch
                {
                    0=>P("difference"),
                    1=>P("longCount"),
                    2=>P("shortCount"),
                    _=>throw new InvalidOperationException()
                }).ToString(CultureInfo.InvariantCulture),
            "usccss.b01.g2.compare_tape_unknown_positions" =>
                (P("unknownPosition") switch
                {
                    0=>P("difference"),
                    1=>P("longCount"),
                    2=>P("shortCount"),
                    _=>throw new InvalidOperationException()
                }).ToString(CultureInfo.InvariantCulture),
            "usccss.b01.g2.compare_tape_difference" =>
                (P("longCount")-P("shortCount"))
                    .ToString(CultureInfo.InvariantCulture),
            "usccss.b01.g3.picture_graph.scaled_compare" =>
                ((P("aIcons")-P("bIcons"))*P("scale"))
                    .ToString(CultureInfo.InvariantCulture),
            "usccss.b01.g3.picture_graph.construct" =>
                P("bIcons").ToString(CultureInfo.InvariantCulture),
            "usccss.b01.g4.eighth_inch_line_plot_difference" =>
                $"{P("lastTick")-P("firstTick")}/8",
            "usccss.b01.g3.quarter_inch_line_plot_frequency" =>
                P("targetTick") switch
                {
                    0=>P("count1").ToString(CultureInfo.InvariantCulture),
                    1=>P("count2").ToString(CultureInfo.InvariantCulture),
                    2=>P("count3").ToString(CultureInfo.InvariantCulture),
                    3=>P("count4").ToString(CultureInfo.InvariantCulture),
                    4=>P("count5").ToString(CultureInfo.InvariantCulture),
                    _=>throw new InvalidOperationException()
                },
            "usccss.b01.g2.measurement.plot_interpret_nonzero_start" =>
                P("mode") switch
                {
                    0=>Enumerable.Range(P("targetTick")+1,
                        P("lastTick")-P("targetTick"))
                        .Sum(i=>P($"count{i}")).ToString(CultureInfo.InvariantCulture),
                    1=>P("lastTick").ToString(CultureInfo.InvariantCulture),
                    2=>P($"count{P("targetTick")}").ToString(CultureInfo.InvariantCulture),
                    _=>throw new InvalidOperationException()
                },
            "usccss.b01.g2.measurement.iterate_ruler_cm" =>
                (30*P("fullSpans")+P("remainder")).ToString(CultureInfo.InvariantCulture),
            "usccss.b01.g2.measure.reptile_meterstick" =>
                (P("mode") switch
                {
                    0=>P("fullMeters"),
                    1=>P("remainingCm"),
                    2=>P("lengthCm"),
                    _=>throw new InvalidOperationException()
                }).ToString(CultureInfo.InvariantCulture),
            "usccss.b01.g1.measurement.story_result_difference" =>
                (P("mode")==0?P("total"):P("other"))
                    .ToString(CultureInfo.InvariantCulture),
            "usccss.b01.g1.measurement.compare_bigger_smaller_unknown" =>
                (P("mode")==0?P("longer"):P("shorter"))
                    .ToString(CultureInfo.InvariantCulture),
            "usccss.b01.g2.measurement.inch_ruler" =>
                Math.Abs(P("actual")-P("estimate")).ToString(CultureInfo.InvariantCulture),
            "usccss.b01.g2.measurement.feet_inch_same_length" =>
                (P("mode") switch
                {
                    0=>1,
                    1=>2,
                    2=>P("inchTiles"),
                    3=>P("footRulers"),
                    _=>throw new InvalidOperationException()
                }).ToString(CultureInfo.InvariantCulture),
            "usccss.b01.g2.measurement.estimate_cm" =>
                Math.Abs(P("actual")-P("estimate")).ToString(CultureInfo.InvariantCulture),
            "usccss.b01.g2.measurement.pencil_line_plot" =>
                P("mode") switch
                {
                    0=>Enumerable.Range(0,P("targetTick")+1)
                        .Sum(i=>P($"count{i}")).ToString(CultureInfo.InvariantCulture),
                    1=>Enumerable.Range(P("targetTick")+1,6-P("targetTick"))
                        .Sum(i=>P($"count{i}")).ToString(CultureInfo.InvariantCulture),
                    2=>Math.Abs(Enumerable.Range(P("targetTick")+1,6-P("targetTick"))
                        .Sum(i=>P($"count{i}")) -
                        Enumerable.Range(0,P("targetTick")+1)
                        .Sum(i=>P($"count{i}"))).ToString(CultureInfo.InvariantCulture),
                    _=>throw new InvalidOperationException()
                },
            "usccss.b01.g2.measurement.read_line_plot" =>
                P("targetLength") switch
                {
                    2=>P("count2").ToString(CultureInfo.InvariantCulture),
                    3=>P("count3").ToString(CultureInfo.InvariantCulture),
                    4=>P("count4").ToString(CultureInfo.InvariantCulture),
                    5=>P("count5").ToString(CultureInfo.InvariantCulture),
                    _=>throw new InvalidOperationException()
                },
            "usccss.b01.ruler.compare_centimeters" =>
                (P("longLength")-P("shortLength")).ToString(CultureInfo.InvariantCulture),
            "usccss.b01.ruler.measure_without_zero" =>
                (P("end")-P("start")).ToString(CultureInfo.InvariantCulture),
            "usccss.b01.g1.measure.same_length_different_units" =>
                (P("mode") switch
                {
                    0=>P("oneInchCount"),
                    1=>P("twoInchCount"),
                    2=>P("oneInchCount")-P("twoInchCount"),
                    _=>throw new InvalidOperationException()
                }).ToString(CultureInfo.InvariantCulture),
            "usccss.b01.measure.map_lengths" =>
                (P("mode") == 0 ? P("a") + P("b") : P("b"))
                    .ToString(CultureInfo.InvariantCulture),
            "usccss.b01.graph.scaled_compare" =>
                Math.Abs(P("countA")-P("countB")).ToString(CultureInfo.InvariantCulture),
            "usccss.b01.graph.choose_scale" =>
                P("step").ToString(CultureInfo.InvariantCulture),
            "usccss.b01.geometry.rectangle_area_grid60" =>
                (P("rows")*P("columns")).ToString(CultureInfo.InvariantCulture),
            "usccss.b01.geometry.rectangle_area_real_world_units" =>
                (P("length")*P("width")).ToString(CultureInfo.InvariantCulture),
            "usccss.b01.g3.same_perimeter_different_area" or
            "usccss.b01.g3.same_area_different_perimeter" =>
                P("expected").ToString(CultureInfo.InvariantCulture),
            "usccss.b01.g4.rectangle_area_is_multiple" or "usccss.b01.g3.distributive_area_partition" or "usccss.b01.g3.rectangle_area_perimeter_story" =>
                P("expected").ToString(CultureInfo.InvariantCulture),
            "usccss.b01.geometry.rectangle_area_ungridded" =>
                (P("length")*P("width")).ToString(CultureInfo.InvariantCulture),
            "usccss.b01.geometry.composite_rectangle_area" =>
                (P("height") * (P("width1") + P("width2"))).ToString(CultureInfo.InvariantCulture),
            "usccss.b01.division.unknown_factor" =>
                P("groups").ToString(CultureInfo.InvariantCulture),
            "usccss.b01.fraction.unit_parts" =>
                $"1/{P("denominator")}",
            "usccss.b01.fraction.tenths_hundredths_within_one" or
            "usccss.b01.fraction.tenths_hundredths_over_one" =>
                $"{P("totalHundredths")}/100",
            "usccss.b01.fraction.name_parts" =>
                $"{P("numerator")}/{P("denominator")}",
            "usccss.b01.measure.quarter_inch" =>
                $"{P("wholeInches") * 4 + P("quarters")}/4",
            "usccss.b01.g4.factor_pairs_two_rectangles" =>
                P("heightB").ToString(CultureInfo.InvariantCulture),
            "usccss.b01.factors.factor_pairs" =>
                P("secondFactor").ToString(CultureInfo.InvariantCulture),
            "usccss.b01.factors.prime_composite" =>
                IsPrime(P("number")) ? "prime" : "composite",
            "usccss.b01.factors.locker_square" =>
                IsPerfectSquare(P("locker")) ? "open" : "closed",
            "usccss.b01.factors.factors_multiples" =>
                P("offset") == 0 ? "yes" : "no",
            "usccss.b01.fraction.whole_multiply" =>
                ReducedFraction(P("numerator") * P("whole"), P("denominator")),
            "usccss.b01.fraction.decompose_same_denominator" =>
                $"{P("wholeN") - P("firstN")}/{P("denominator")}",
            "usccss.b01.place_value.ten_times" => "10",
            _ => throw new InvalidOperationException("Unknown B01 family")
        };
    }

    private static bool IsPrime(int number)
    {
        if (number < 2) return false;
        for (var i = 2; i * i <= number; i++)
            if (number % i == 0) return false;
        return true;
    }

    private static bool IsPerfectSquare(int x)
    {
        if (x < 1) return false;
        var r = (int)Math.Sqrt(x);
        return r * r == x;
    }

    // The verifier must reject a modified parameter payload even if someone
    // supplies an answer consistent with those modified parameters.
    private static bool ValidDomain(string family, IReadOnlyDictionary<string, int> p)
    {
        bool Range(string key,int lo,int hi) =>
            p.TryGetValue(key,out var v) && v>=lo && v<=hi;
        int N(string key) => p[key];
        return family switch
        {
            "usccss.b01.g1.add_one_or_two" =>
                Range("start",0,8) && Range("step",1,2) &&
                N("start")+N("step")<=10,
            "usccss.b01.g1.subtract_one_or_two" =>
                Range("start",1,10) && Range("step",1,2) &&
                N("start")>=N("step"),
            "usccss.b01.g1.make_ten_missing_part" =>
                Range("known",0,10) && Range("missing",0,10) &&
                N("known")+N("missing")==10,
            "usccss.b01.g3.add_within1000" or
            "usccss.b01.g3.add_regroup1000" =>
                Range("left",100,699) && Range("right",100,899) &&
                Range("operation",0,0) && Range("expected",200,999) &&
                N("left")+N("right")==N("expected") &&
                (family=="usccss.b01.g3.add_within1000" ||
                    (N("left")%10+N("right")%10>=10 ||
                     N("left")/10%10+N("right")/10%10+
                     (N("left")%10+N("right")%10>=10?1:0)>=10)),
            "usccss.b01.g3.subtract_within1000" or
            "usccss.b01.g3.subtract_regroup1000" or
            "usccss.b01.g3.subtract_across_zero" =>
                Range("left",200,999) && Range("right",100,998) &&
                N("left")>N("right") && Range("operation",1,1) &&
                Range("expected",1,899) &&
                N("left")-N("right")==N("expected") &&
                (family=="usccss.b01.g3.subtract_within1000" ||
                 family=="usccss.b01.g3.subtract_regroup1000" &&
                    (N("left")%10<N("right")%10 ||
                     N("left")/10%10<N("right")/10%10+
                       (N("left")%10<N("right")%10?1:0)) ||
                 family=="usccss.b01.g3.subtract_across_zero" &&
                    N("left")%100==0 && N("right")%100!=0),
            "usccss.b01.market.inventory_100" =>
                Range("small",2,9) && Range("second",20,60) &&
                Range("third",11,78) &&
                N("small") + N("second") + N("third") == 100 &&
                Range("sold",1,11) && N("sold") < N("second") &&
                Range("mode",0,1),
            "usccss.b01.g1.teen_no_regroup" =>
                Range("left",11,19) && Range("right",1,9) &&
                Range("operation",0,1) && Range("expected",10,19) &&
                (N("operation")==0
                    ?N("left")%10+N("right")<10
                    :N("left")%10>=N("right")) &&
                N("expected")==(N("operation")==0
                    ?N("left")+N("right"):N("left")-N("right")),
            "usccss.b01.g1.teen_subtract_decompose_ten" =>
                Range("left",10,18) && Range("right",1,9) &&
                Range("operation",1,1) && Range("expected",1,17) &&
                N("left")%10<N("right") &&
                N("expected")==N("left")-N("right"),
            "usccss.b01.g2.story_without_regroup100" =>
                Range("left",10,99) && Range("right",10,99) &&
                Range("operation",0,1) && Range("expected",0,99) &&
                N("expected")==(N("operation")==0
                    ?N("left")+N("right"):N("left")-N("right")) &&
                (N("operation")==0
                    ?N("left")%10+N("right")%10<10 &&
                     N("left")/10+N("right")/10<=9
                    :N("left")%10>=N("right")%10 &&
                     N("left")/10>=N("right")/10),
            "usccss.b01.g2.story_regroup50" =>
                Range("left",10,49) && Range("right",10,49) &&
                Range("operation",0,1) && Range("expected",1,49) &&
                N("expected")==(N("operation")==0
                    ?N("left")+N("right"):N("left")-N("right")) &&
                (N("operation")==0
                    ?N("left")%10+N("right")%10>=10
                    :N("left")%10<N("right")%10),
            "usccss.b01.g2.add_subtract_without_regroup" =>
                Range("left",10,99) && Range("right",10,99) &&
                Range("operation",0,1) && Range("expected",0,99) &&
                N("expected")==(N("operation")==0
                    ?N("left")+N("right"):N("left")-N("right")) &&
                (N("operation")==0
                    ?N("left")%10+N("right")%10<10 &&
                     N("left")/10+N("right")/10<=9
                    :N("left")%10>=N("right")%10 &&
                     N("left")/10>=N("right")/10),
            "usccss.b01.g2.subtract_decompose_ten" =>
                Range("left",20,99) && Range("right",11,89) &&
                Range("operation",1,1) && Range("expected",1,88) &&
                N("left")/10>N("right")/10 &&
                N("left")%10<N("right")%10 &&
                N("expected")==N("left")-N("right"),
            "usccss.b01.g1.add_within100_no_compose_ten" =>
                Range("left",10,89) && Range("right",10,89) &&
                Range("expected",20,99) &&
                N("left")+N("right")==N("expected") &&
                N("left")%10+N("right")%10<10 &&
                N("left")/10+N("right")/10<=9,
            "usccss.b01.g2.add_subtract_1000_no_regroup" =>
                Range("left",100,999) && Range("right",100,899) &&
                Range("operation",0,1) && Range("expected",0,999) &&
                N("expected")==(N("operation")==0
                    ?N("left")+N("right"):N("left")-N("right")) &&
                (N("operation")==0
                    ?N("left")/100+N("right")/100<10 &&
                     N("left")/10%10+N("right")/10%10<10 &&
                     N("left")%10+N("right")%10<10
                    :N("left")/100>N("right")/100 &&
                     N("left")/10%10>=N("right")/10%10 &&
                     N("left")%10>=N("right")%10),
            "usccss.b01.g4.decimal_number_line_greater" =>
                Range("start",0,80) && N("start")%10==0 &&
                Range("tickA",0,10) && Range("tickB",0,10) &&
                N("tickA")!=N("tickB") &&
                Range("first",0,90) && Range("second",0,90) &&
                N("first")==N("start")+N("tickA") &&
                N("second")==N("start")+N("tickB"),
            "usccss.b01.g2.order_three_digits_number_line" =>
                Range("start",100,800) && N("start")%100==0 &&
                Range("tickA",0,10) && Range("tickB",0,10) &&
                N("tickA")!=N("tickB") &&
                Range("first",100,900) && Range("second",100,900) &&
                N("first")==N("start")+10*N("tickA") &&
                N("second")==N("start")+10*N("tickB"),
            "usccss.b01.g4.whole_minus_proper_fraction" =>
                Range("whole",1,4) && Range("denominator",3,10) &&
                Range("numerator",1,N("denominator")-1),
            "usccss.b01.g1.compare_order_tens_ones" =>
                Range("first",10,99) && Range("second",10,99) &&
                Range("third",10,99) && Range("position",0,2) &&
                N("first")!=N("second") && N("first")!=N("third") &&
                N("second")!=N("third") &&
                N("first")/10==N("second")/10,
            "usccss.b01.g1.add_subtract_10_tens" or "usccss.b01.g1.add_subtract_tens_multiples" =>
                Range("left",10,80) && Range("right",10,80) &&
                Range("operation",0,1) && Range("expected",0,90) &&
                N("left")%10==0 && N("right")%10==0 &&
                N("expected")==(N("operation")==0
                    ?N("left")+N("right"):N("left")-N("right")) &&
                (family=="usccss.b01.g1.add_subtract_tens_multiples" ||
                    N("right")==10) &&
                (family=="usccss.b01.g1.add_subtract_10_tens" ||
                    (N("left")>=20 && N("left")<=80 &&
                     N("right")<=Math.Min(N("left"),90-N("left")))),
            "usccss.b01.g1.place_value_number_riddle" =>
                Range("tens",1,9) && Range("ones",1,9) &&
                Range("number",11,99) &&
                N("number")==N("tens")*10+N("ones"),
            "usccss.b01.g2.target_measurement_difference_cm" =>
                Range("target",4,20) && Range("measured",1,23) &&
                Math.Abs(N("target")-N("measured"))<=3,
            "usccss.b01.g3.quarter_inch_estimate_classify" =>
                Range("actualQuarters",4,11) && Range("mode",0,2) &&
                Range("estimateQuarters",3,12) &&
                N("estimateQuarters")==N("actualQuarters")+N("mode")-1,
            "usccss.b01.place_value.tens_ones" =>
                Range("number",10,99) && Range("tens",1,9) &&
                Range("ones",0,9) && Range("mode",0,2) &&
                N("number") == N("tens")*10 + N("ones"),
            "usccss.b01.g1.measure.count_cubes_up_to110" =>
                Range("number",100,110) && Range("tens",10,11) &&
                Range("ones",0,9) && Range("mode",0,1) &&
                N("number")==N("tens")*10+N("ones"),
            "usccss.b01.g1.measure.write_cubes_to120" =>
                Range("number",95,120) && Range("tens",9,12) &&
                Range("ones",0,9) && Range("mode",0,1) &&
                N("number")==N("tens")*10+N("ones"),
            "usccss.b01.place_value.count_to120" =>
                Range("number",40,120) && Range("hundreds",0,1) &&
                Range("tens",0,9) && Range("ones",0,9) &&
                N("number") == N("hundreds")*100 + N("tens")*10 + N("ones"),
            "usccss.b01.place_value.three_digits" =>
                Range("number",100,999) && Range("hundreds",1,9) &&
                Range("tens",0,9) && Range("ones",0,9) && Range("mode",0,1) &&
                N("number") == N("hundreds")*100 + N("tens")*10 + N("ones"),
            "usccss.b01.g1.match_dot_cubes_expression" or
            "usccss.b01.g1.complete_missing_dots" =>
                Range("first",1,6) && Range("second",1,6) &&
                N("first")+N("second")<=10,
            "usccss.b01.g1.check_off_add_within10" =>
                Range("first",0,5) && Range("second",0,5),
            "usccss.b01.shape.polygon_sides" =>
                Range("sides",3,6) && Range("variant",0,3),
            "usccss.b01.shape.specific_subcategory" =>
                Range("subcategory",0,3) && Range("variant",0,2),
            "usccss.b01.area.missing_composite_side" =>
                Range("widthKnown",2,7) && Range("widthMissing",2,7) &&
                Range("widthTotal",4,14) &&
                N("widthTotal")==N("widthKnown")+N("widthMissing") &&
                Range("heightLeft",2,6) && Range("heightRight",2,6) &&
                N("heightLeft")!=N("heightRight"),
            "usccss.b01.triangle.attributes" =>
                Range("position",1,3) && Range("style",0,3),
            "usccss.b01.partition.halves_fourths" =>
                (N("denominator") is 2 or 4) && Range("shape",0,4),
            "usccss.b01.number_line.move_left_right_within100" =>
                Range("start",20,80) && N("start")%10==0 &&
                Range("distance",10,40) && N("distance")%10==0 &&
                Range("jumps",1,4) && N("distance")==N("jumps")*10 &&
                (N("direction") is -1 or 1) && Range("end",0,100) &&
                N("end")==N("start")+N("direction")*N("distance"),
            "usccss.b01.number_line.estimate_location_within100" =>
                Range("target",1,99) &&
                Range("lower",0,90) && N("lower")%10==0 &&
                Range("upper",10,100) && N("upper")==N("lower")+10 &&
                N("target")>N("lower") && N("target")<N("upper") &&
                N("target")%10!=5 &&
                Range("nearest",0,100) &&
                N("nearest")==(N("target")-N("lower")<5
                    ?N("lower"):N("upper")),
            "usccss.b01.number_line.locate_whole_to100" =>
                (N("step") is 1 or 2 or 5 or 10) &&
                Range("tick",1,9) && Range("mode",0,2),
            "usccss.b01.number_line.locate_three_digits" =>
                (N("step") is 50 or 100) &&
                Range("tick",2,9) && Range("mode",0,2),
            "usccss.b01.number_line.locate_large_numbers" =>
                (N("step") is 1000 or 10000 or 100000) &&
                Range("tick",1,9) && Range("mode",0,2),
            "usccss.b01.number_line.equal_intervals" => Range("step",1,40),
            "usccss.b01.clock.read_on_hour" =>
                Range("hour",1,12) && Range("minutes",0,0),
            "usccss.b01.clock.read_five_minutes_am_pm" =>
                Range("hour",1,12) && Range("minutes",0,55) &&
                N("minutes")%5==0 && Range("period",0,1),
            "usccss.b01.clock.read_half_hour" =>
                Range("hour",1,12) && (N("minutes") is 0 or 30),
            "usccss.b01.clock.read_five_minutes" =>
                Range("hour",1,12) && Range("minutes",0,55) &&
                N("minutes")%5==0,
            "usccss.b01.clock.half_quarters" =>
                Range("position",0,2) && Range("hour",1,12),
            "usccss.b01.rectangle.unit_squares" =>
                Range("rows",2,7) && Range("columns",2,13) &&
                N("rows")*N("columns")<=35,
            "usccss.b01.g2.length_compare.all_unknown_positions100" =>
                Range("longCount",41,99) &&
                Range("shortCount",11,94) &&
                Range("difference",5,30) && Range("unknownPosition",0,2) &&
                N("longCount")>N("shortCount") &&
                N("difference")==N("longCount")-N("shortCount"),
            "usccss.b01.g2.compare_tape_unknown_positions" =>
                Range("longCount",8,19) && Range("shortCount",2,18) &&
                Range("difference",1,17) && Range("unknownPosition",0,2) &&
                N("longCount")>N("shortCount") &&
                N("difference")==N("longCount")-N("shortCount"),
            "usccss.b01.g2.compare_tape_difference" =>
                Range("longCount",8,19) && Range("shortCount",2,18) &&
                N("longCount")>N("shortCount") &&
                Range("difference",1,17) &&
                N("difference")==N("longCount")-N("shortCount"),
            "usccss.b01.g3.picture_graph.scaled_compare" =>
                (N("scale") is 2 or 5) &&
                Range("aIcons",3,6) && Range("bIcons",1,5) &&
                N("aIcons")>N("bIcons"),
            "usccss.b01.g3.picture_graph.construct" =>
                (N("scale") is 2 or 5) &&
                Range("aIcons",2,5) && Range("bIcons",1,6),
            "usccss.b01.g4.eighth_inch_line_plot_difference" =>
                Range("whole",7,9) &&
                Range("firstTick",1,7) &&
                Range("lastTick",2,8) &&
                N("lastTick")>N("firstTick") &&
                Enumerable.Range(0,9).All(i=>Range($"count{i}",1,3)),
            "usccss.b01.g3.quarter_inch_line_plot_frequency" =>
                Range("count1",1,4) && Range("count2",1,4) &&
                Range("count3",1,4) && Range("count4",1,4) &&
                Range("count5",1,4) && Range("targetTick",0,4),
            "usccss.b01.g2.measurement.plot_interpret_nonzero_start" =>
                Range("start",21,28) && Range("lastTick",7,10) &&
                Range("targetTick",0,N("lastTick")-1) &&
                Range("mode",0,2) &&
                Enumerable.Range(0,11).All(i=>Range($"count{i}",0,10)) &&
                N("count0")>0 && N($"count{N("lastTick")}")>0 &&
                Enumerable.Range(0,11).Sum(i=>N($"count{i}"))==10 &&
                Enumerable.Range(N("lastTick")+1,10-N("lastTick"))
                    .All(i=>N($"count{i}")==0),
            "usccss.b01.g2.measurement.iterate_ruler_cm" =>
                Range("fullSpans",1,3) && Range("remainder",0,29) &&
                Range("total",50,90) &&
                30*N("fullSpans")+N("remainder")==N("total"),
            "usccss.b01.g2.measure.reptile_meterstick" =>
                Range("tapeIndex",0,6) &&
                N("lengthCm")==new[]{58,72,44,180,336,190,282}[N("tapeIndex")] &&
                N("fullMeters")==N("lengthCm")/100 &&
                N("remainingCm")==N("lengthCm")%100 &&
                Range("mode",0,2),
            "usccss.b01.g1.measurement.story_result_difference" =>
                Range("first",4,11) && Range("other",2,9) &&
                Range("total",6,20) && Range("mode",0,2) &&
                N("total")==N("first")+N("other"),
            "usccss.b01.g1.measurement.compare_bigger_smaller_unknown" =>
                Range("shorter",4,11) && Range("difference",2,8) &&
                Range("longer",6,20) && Range("mode",0,1) &&
                N("longer")==N("shorter")+N("difference"),
            "usccss.b01.g2.measurement.inch_ruler" =>
                Range("actual",1,11) && Range("estimate",1,14) &&
                N("actual")!=N("estimate"),
            "usccss.b01.g2.measurement.feet_inch_same_length" =>
                Range("fish",0,2) && Range("mode",0,3) &&
                N("inchTiles")==(N("fish")==0?24:N("fish")==1?48:36) &&
                N("footRulers")==(N("fish")==0?2:N("fish")==1?4:3) &&
                N("inchTiles")>N("footRulers"),
            "usccss.b01.g2.measurement.estimate_cm" =>
                Range("actual",5,30) && Range("estimate",1,35) &&
                Range("error",1,5) &&
                Math.Abs(N("actual")-N("estimate"))==N("error"),
            "usccss.b01.g2.measurement.pencil_line_plot" =>
                Range("start",8,11) && Range("targetTick",0,6) &&
                Range("mode",0,2) &&
                Enumerable.Range(0,7).All(i=>Range($"count{i}",0,10)) &&
                N("count0")>0 && N("count6")>0 &&
                Enumerable.Range(0,7).Sum(i=>N($"count{i}"))==10,
            "usccss.b01.g2.measurement.read_line_plot" =>
                Range("count2",1,4) && Range("count3",1,4) &&
                Range("count4",1,4) && Range("count5",1,4) &&
                Range("targetLength",2,5),
            "usccss.b01.ruler.compare_centimeters" =>
                Range("longLength",4,15) && Range("shortLength",2,14) &&
                Range("difference",1,13) &&
                N("longLength")>N("shortLength") &&
                N("difference")==N("longLength")-N("shortLength"),
            "usccss.b01.ruler.measure_without_zero" =>
                Range("start",2,9) && Range("end",5,22) &&
                Range("length",3,13) &&
                N("length")==N("end")-N("start"),
            "usccss.b01.g1.measure.same_length_different_units" =>
                N("tapeInches") is 12 or 16 or 18 or 20 or 24 &&
                N("oneInchCount")==N("tapeInches") &&
                N("twoInchCount")*2==N("tapeInches") &&
                Range("mode",0,2),
            "usccss.b01.measure.map_lengths" =>
                Range("a",2,10) && Range("b",2,10) &&
                N("a")+N("b")<=20 && Range("mode",0,1),
            "usccss.b01.graph.scaled_compare" =>
                (N("unit") is 2 or 5 or 10) &&
                Range("ticksA",2,8) && Range("ticksB",1,8) &&
                N("ticksA")!=N("ticksB") && Range("mode",0,1) &&
                N("countA")==N("ticksA")*N("unit") &&
                N("countB")==N("ticksB")*N("unit") &&
                N("countA")<=100 && N("countB")<=100,
            "usccss.b01.graph.choose_scale" =>
                (N("step") is 2 or 5 or 10) && Range("dataVariant",0,4),
            "usccss.b01.array.equal_groups" =>
                Range("rows",2,7) && Range("columns",2,11) &&
                N("rows")*N("columns")<=100,
            "usccss.b01.g2.compose_equal_square_shapes" =>
                Range("rows",2,7) && Range("columns",2,7) &&
                Range("mode",0,1) && Range("tiles",4,49) &&
                N("tiles")==N("rows")*N("columns") &&
                (N("mode")==0?N("rows")==N("columns"):
                    N("rows")!=N("columns")),
            "usccss.b01.g3.multiplication_division_inverse_array" =>
                Range("rows",2,10) && Range("columns",2,10) &&
                Range("total",4,100) && Range("operation",0,1) &&
                N("total")==N("rows")*N("columns"),
            "usccss.b01.array.columns" or
            "usccss.b01.array.equal_addends" or
            "usccss.b01.array.equal_equation" or
            "usccss.b01.array.unknown_factor" or
            "usccss.b01.array.commutativity" =>
                Range("rows",2,8) && Range("columns",2,9),
            "usccss.b01.geometry.rectangle_area_grid60" =>
                Range("rows",2,8) && Range("columns",2,30) &&
                N("rows")*N("columns")<=60,
            "usccss.b01.geometry.rectangle_area_real_world_units" =>
                Range("length",3,10) && Range("width",2,9) &&
                Range("unitMode",0,1),
            "usccss.b01.g3.same_perimeter_different_area" =>
                Range("widthA",2,6) && Range("widthB",3,7) &&
                Range("heightA",3,11) && Range("heightB",2,10) &&
                N("widthB")==N("widthA")+1 &&
                N("heightA")==N("heightB")+1 &&
                Range("expected",1,9) &&
                N("widthA")+N("heightA")==N("widthB")+N("heightB") &&
                N("widthB")*N("heightB")>N("widthA")*N("heightA") &&
                N("expected")==N("widthB")*N("heightB")-
                    N("widthA")*N("heightA"),
            "usccss.b01.g3.same_area_different_perimeter" =>
                Range("widthA",2,8) && Range("widthB",3,9) &&
                Range("heightA",3,12) && Range("heightB",2,12) &&
                N("widthA")<N("widthB") &&
                Range("expected",1,22) &&
                N("widthA")*N("heightA")==N("widthB")*N("heightB") &&
                2*(N("widthA")+N("heightA"))>
                    2*(N("widthB")+N("heightB")) &&
                N("expected")==2*(N("widthA")+N("heightA")-
                    N("widthB")-N("heightB")),
            "usccss.b01.g4.rectangle_area_is_multiple" =>
                Range("length",2,11) && Range("width",2,10) &&
                Range("expected",4,110) &&
                N("length")*N("width")==N("expected"),
            "usccss.b01.g3.distributive_area_partition" =>
                Range("height",2,8) && Range("leftWidth",1,6) &&
                Range("rightWidth",1,6) && Range("expected",4,96) &&
                N("expected")==N("height")*
                    (N("leftWidth")+N("rightWidth")),
            "usccss.b01.g3.rectangle_area_perimeter_story" =>
                Range("length",3,10) && Range("width",2,9) &&
                Range("mode",0,1) && Range("expected",10,90) &&
                N("expected")==(N("mode")==0
                    ?N("length")*N("width")
                    :2*(N("length")+N("width"))),
            "usccss.b01.geometry.rectangle_area_ungridded" =>
                Range("length",2,10) && Range("width",2,10),
            "usccss.b01.geometry.composite_rectangle_area" =>
                Range("height",2,5) && Range("width1",2,6) &&
                Range("width2",1,4),
            "usccss.b01.division.unknown_factor" =>
                Range("objectsPerGroup",2,10) && Range("groups",2,10),
            "usccss.b01.fraction.tenths_hundredths_within_one" =>
                Range("tenths",1,9) && Range("hundredths",1,99) &&
                N("totalHundredths") == 10*N("tenths")+N("hundredths") &&
                N("totalHundredths")<=100,
            "usccss.b01.fraction.tenths_hundredths_over_one" =>
                Range("tenths",2,9) && Range("hundredths",1,99) &&
                N("totalHundredths") == 10*N("tenths")+N("hundredths") &&
                N("totalHundredths")>100 && N("totalHundredths")<200,
            "usccss.b01.fraction.unit_parts" =>
                (N("denominator") is 2 or 3 or 4 or 6 or 8) &&
                Range("shape",0,2),
            "usccss.b01.fraction.name_parts" =>
                (N("denominator") is 2 or 3 or 4 or 6 or 8) &&
                Range("numerator",1,N("denominator")-1),
            "usccss.b01.measure.quarter_inch" =>
                Range("quarters",1,3) && Range("wholeInches",0,3),
            "usccss.b01.g4.factor_pairs_two_rectangles" =>
                Range("widthA",2,12) && Range("heightA",2,12) &&
                Range("widthB",2,12) && Range("heightB",2,12) &&
                Range("area",12,72) && N("widthA")<N("widthB") &&
                N("widthA")<=N("heightA") &&
                N("widthB")<=N("heightB") &&
                N("widthA")*N("heightA")==N("area") &&
                N("widthB")*N("heightB")==N("area"),
            "usccss.b01.factors.factor_pairs" =>
                Range("firstFactor",2,8) && Range("secondFactor",2,9),
            "usccss.b01.factors.prime_composite" => Range("number",2,100),
            "usccss.b01.factors.locker_square" => Range("locker",1,20),
            "usccss.b01.factors.factors_multiples" =>
                Range("a",2,10) && Range("b",2,10) &&
                Range("mode",0,1) && Range("offset",0,1),
            "usccss.b01.fraction.whole_multiply" =>
                Range("denominator",3,9) &&
                Range("numerator",2,N("denominator")-1) && Range("whole",2,5),
            "usccss.b01.fraction.decompose_same_denominator" =>
                Range("denominator",4,10) &&
                Range("wholeN",2,N("denominator")) &&
                Range("firstN",1,N("wholeN")-1),
            "usccss.b01.place_value.ten_times" =>
                Range("digit",1,9) && (N("place") is 10 or 100 or 1000),
            _ => false
        };
    }

    public static bool Verify(string family, IReadOnlyDictionary<string, int> p, string answer)
    {
        if (!Supports(family) || answer is null) return false;
        try
        {
            if (!ValidDomain(family,p)) return false;
            // Independent CI oracles cross-check the mathematical result from
            // the structured generation parameters across all 19 families.
            // Numeric decimal answers such as 0.30 and 0.3 are equivalent.
            // Compare the exact decimal, with no rounding or float tolerance.
            if(family=="usccss.b01.g4.decimal_number_line_greater")
                return decimal.TryParse(answer.Trim(),
                    NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture,out var supplied) &&
                    supplied==Math.Max(p["first"],p["second"])/100m;
            var checkedAnswer = family is "usccss.b01.array.equal_addends"
                or "usccss.b01.array.equal_equation"
                ? new string(answer.Where(c=>!char.IsWhiteSpace(c)).ToArray())
                : answer.Trim();
            return string.Equals(checkedAnswer, Solve(family,p),
                StringComparison.OrdinalIgnoreCase);
        }
        catch (Exception e) when (e is KeyNotFoundException or
            InvalidOperationException or DivideByZeroException or OverflowException)
        {
            return false;
        }
    }
}

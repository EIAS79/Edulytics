using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Grade 6 U02:L01 source-aligned ratio language. Short-answer ratio comparisons
/// and independently authored square-unit color partitions reuse the existing
/// ExactSkillContractQuestionEngine. Student collections, category decisions,
/// oral justification and drawn non-rectangles are TEACHER_REVIEW_REQUIRED.
/// </summary>
internal static class UsCcssG6Unit2RatioQuestions
{
    public const string L01Collections = "usccss.g6.u02.l01.ordered_collection_ratios";
    public const string L01Shade = "usccss.g6.u02.l01.shaded_area_ratio_counts";
    public const string L02Diagrams = "usccss.g6.u02.l02.labeled_two_row_ratios";
    public const string L02Cards = "usccss.g6.u02.l02.three_ingredient_ratio_cards";
    public const string L03RecipeBatches = "usccss.g6.u02.l03.recipe_batch_scale";
    public const string L03EqualTaste = "usccss.g6.u02.l03.same_taste_equivalent_ratios";
    public const string L04ColorAmounts = "usccss.g6.u02.l04.color_mixture_quantities";
    public const string L04EqualShade = "usccss.g6.u02.l04.same_shade_proportions";
    public const string L05Equivalence = "usccss.g6.u02.l05.equivalent_ratio_definition";
    public const string L05ScaleMissing = "usccss.g6.u02.l05.scaled_ratio_missing_term";

    public const string L06DoubleLineAmounts = "usccss.g6.u02.l06.double_number_line_amounts";
    public const string L06DoubleLineLogic = "usccss.g6.u02.l06.double_number_line_structure";

    public const string L07LineValues = "usccss.g6.u02.l07.created_double_number_line_values";
    public const string L07LineLogic = "usccss.g6.u02.l07.double_number_line_units_per";

    public const string L08UnitPrices = "usccss.g6.u02.l08.unit_prices_and_counts";
    public const string L08PriceLogic = "usccss.g6.u02.l08.unit_price_interpretation";

    public const string L09SpeedDistances = "usccss.g6.u02.l09.constant_speed_quantities";
    public const string L09SpeedComparisons = "usccss.g6.u02.l09.constant_speed_equivalence";

    public const string L10CompareAmounts = "usccss.g6.u02.l10.rate_comparison_quantities";
    public const string L10EquivalentContexts = "usccss.g6.u02.l10.compare_rates_reasoning";

    public const string L11TableAmounts = "usccss.g6.u02.l11.table_equivalent_ratio_values";
    public const string L11TableLogic = "usccss.g6.u02.l11.ratio_table_reasoning";

    public const string L12TableCalculations = "usccss.g6.u02.l12.table_missing_quantity";
    public const string L12TableStrategies = "usccss.g6.u02.l12.table_equivalence_methods";

    public const string L13RepresentationValues = "usccss.g6.u02.l13.table_line_ratio_values";
    public const string L13RepresentationLogic = "usccss.g6.u02.l13.representation_selection";

    public const string L14InfoGapNumbers = "usccss.g6.u02.l14.info_gap_ratio_quantities";
    public const string L14InfoGapReasoning = "usccss.g6.u02.l14.rate_information_requests";

    public const string L15TapeParts = "usccss.g6.u02.l15.tape_diagram_part_whole";
    public const string L15TapeLogic = "usccss.g6.u02.l15.part_to_whole_reasoning";

    public const string L16MultistepRatio = "usccss.g6.u02.l16.multistep_ratio_totals";
    public const string L16RepresentationChoice = "usccss.g6.u02.l16.ratio_tool_selection";

    public const string L17FermiAssumptions = "usccss.g6.u02.l17.first_party_fermi_assumption_arithmetic";
    public const string L17FermiSensitivity = "usccss.g6.u02.l17.first_party_fermi_sensitivity";

    public const string L0301PerOne = "usccss.g6.u03.l01.per_one_source_rate";
    public const string L0301RateMeaning = "usccss.g6.u03.l01.source_rate_meaning";

    public const string L0302UnitMagnitudes = "usccss.g6.u03.l02.unit_magnitude_and_conversion";
    public const string L0302UnitCategories = "usccss.g6.u03.l02.choose_appropriate_unit_category";

    public const string L0303SourceAmounts = "usccss.g6.u03.l03.measurement_source_quantity";
    public const string L0303AttributeClassification = "usccss.g6.u03.l03.measurement_attribute_identification";

    public const string L0304Conversions = "usccss.g6.u03.l04.source_ratio_unit_conversions";
    public const string L0304DirectionChecks = "usccss.g6.u03.l04.conversion_direction_and_units";

    public const string L0305Rates = "usccss.g6.u03.l05.source_speed_price_values";
    public const string L0305Comparisons = "usccss.g6.u03.l05.same_rate_price_speed_reasoning";

    public const string L0306Rates = "usccss.g6.u03.l06.source_reciprocal_rate_values";
    public const string L0306InterpretRate = "usccss.g6.u03.l06.reciprocal_rate_meaning";

    public const string L0307UnitRateValues = "usccss.g6.u03.l07.unit_rate_table_quantities";
    public const string L0307RateInvariance = "usccss.g6.u03.l07.unit_rate_equivalence";

    public const string L0308SpeedAmounts = "usccss.g6.u03.l08.speed_distance_time_quantities";
    public const string L0308PaceMeaning = "usccss.g6.u03.l08.speed_pace_comparison";

    public const string L0309RateValues = "usccss.g6.u03.l09.source_unit_rate_calculations";
    public const string L0309ComparisonLogic = "usccss.g6.u03.l09.solve_rate_comparison";

    public const string L0310PercentNumbers = "usccss.g6.u03.l10.percent_coin_values";
    public const string L0310PercentLogic = "usccss.g6.u03.l10.percent_comparison_logic";

    public const string L0311PercentValues = "usccss.g6.u03.l11.percent_scaled_amounts";
    public const string L0311PercentReasoning = "usccss.g6.u03.l11.percent_of_quantity_reasoning";

    public const string L0312TapePercent = "usccss.g6.u03.l12.tape_diagram_percent_amounts";
    public const string L0312TapeStructure = "usccss.g6.u03.l12.percent_tape_reasoning";

    public const string L0313BenchmarkNumbers = "usccss.g6.u03.l13.benchmark_percent_values";
    public const string L0313BenchmarkLogic = "usccss.g6.u03.l13.benchmark_percent_reasoning";

    public const string L0314PercentComputations = "usccss.g6.u03.l14.percent_unknowns";
    public const string L0314PercentInterpretation = "usccss.g6.u03.l14.percent_compare_and_inverse";

    public const string L0315PercentAmounts = "usccss.g6.u03.l15.percent_of_quantity_exact";
    public const string L0315PercentProperties = "usccss.g6.u03.l15.percent_of_quantity_reasoning";

    public const string L0316FindPercent = "usccss.g6.u03.l16.find_percent_from_two_amounts";
    public const string L0316FindPercentLogic = "usccss.g6.u03.l16.percent_division_reasoning";

    public const string L0317PaintCalculations = "usccss.g6.u03.l17.first_party_bounded_room_paint_math";
    public const string L0317PaintReasoning = "usccss.g6.u03.l17.first_party_paint_model_reasoning";

    public const string L0401SourceNumerics = "usccss.g6.u04.l01.division_and_source_quantities";
    public const string L0401QuotientScale = "usccss.g6.u04.l01.predict_quotient_size";

    public const string L0402DivisionNumbers = "usccss.g6.u04.l02.division_groups_and_size";
    public const string L0402DivisionMeaningLogic = "usccss.g6.u04.l02.division_interpretations";

    public const string L0403ContextualDivision = "usccss.g6.u04.l03.division_contextual_results";
    public const string L0403DivisionInterpretation = "usccss.g6.u04.l03.division_explanation_checks";

    public const string L0404FractionGroups = "usccss.g6.u04.l04.fraction_groups_exact";
    public const string L0404FractionModels = "usccss.g6.u04.l04.fraction_division_reasoning";

    public const string L0405FractionValues = "usccss.g6.u04.l05.fraction_group_quotients";
    public const string L0405FractionMeaning = "usccss.g6.u04.l05.fractional_group_interpretation";

    public const string L0406TapeValues = "usccss.g6.u04.l06.fraction_division_values";
    public const string L0406TapeLogic = "usccss.g6.u04.l06.fraction_tape_interpretation";

    public const string L0407FractionGroups = "usccss.g6.u04.l07.fraction_of_group_numeric";
    public const string L0407FractionLogic = "usccss.g6.u04.l07.fractional_group_reasoning";

    public const string L0408GroupSizeNumbers = "usccss.g6.u04.l08.fraction_group_size";
    public const string L0408GroupSizeLogic = "usccss.g6.u04.l08.fraction_group_size_reasoning";

    public const string L0409SourceWordValues = "usccss.g6.u04.l09.contextual_fraction_division";
    public const string L0409ModelReasoning = "usccss.g6.u04.l09.division_word_model_logic";

    public const string L0410AlgorithmValues = "usccss.g6.u04.l10.fraction_division_steps";
    public const string L0410AlgorithmLogic = "usccss.g6.u04.l10.division_reciprocal_reasoning";

    public const string L0411DivisionFluency = "usccss.g6.u04.l11.divide_fractions_exact";
    public const string L0411DivisionLogic = "usccss.g6.u04.l11.fraction_algorithm_reasoning";

    public const string L0412LengthValues = "usccss.g6.u04.l12.fractional_length_comparisons";
    public const string L0412LengthReasoning = "usccss.g6.u04.l12.fractional_comparison_reasoning";

    public const string L0413AreaValues = "usccss.g6.u04.l13.rectangle_area_fractional_sides";
    public const string L0413AreaLogic = "usccss.g6.u04.l13.rectangle_area_diagram_reasoning";

    public const string L0414GeometryNumbers = "usccss.g6.u04.l14.triangle_height_and_cube_counts";
    public const string L0414GeometryLogic = "usccss.g6.u04.l14.cube_and_triangle_reasoning";

    public const string L0415Volumes = "usccss.g6.u04.l15.fractional_prism_volume";
    public const string L0415VolumeLogic = "usccss.g6.u04.l15.prism_unit_cube_reasoning";

    public const string L0416AllOperations = "usccss.g6.u04.l16.fraction_operations_context";
    public const string L0416OperationsLogic = "usccss.g6.u04.l16.fraction_ops_reasoning";

    public const string L0417BoundedPacking = "usccss.g6.u04.l17.first_party_fixed_orientation_packing_math";
    public const string L0417PackingModelLogic = "usccss.g6.u04.l17.first_party_orientation_model_reasoning";

    public const string L0501DecimalArithmetic = "usccss.g6.u05.l01.shopping_decimal_arithmetic";
    public const string L0501DecimalEstimation = "usccss.g6.u05.l01.shopping_reasonableness";

    public const string L0502PlaceValueArithmetic = "usccss.g6.u05.l02.base_ten_decimal_arithmetic";
    public const string L0502PlaceValueLogic = "usccss.g6.u05.l02.align_decimal_place_values";

    public const string L0503DecimalCalculations = "usccss.g6.u05.l03.decimal_sum_difference_results";
    public const string L0503DecimalPrinciples = "usccss.g6.u05.l03.placevalue_borrowing_correctness";

    public const string L0504PreciseArithmetic = "usccss.g6.u05.l04.precise_decimal_add_subtract";
    public const string L0504DecimalReasoning = "usccss.g6.u05.l04.placevalue_high_precision_logic";

    public const string L0505DecimalProducts = "usccss.g6.u05.l05.decimal_products_base_ten";
    public const string L0505DecimalLogic = "usccss.g6.u05.l05.decimal_product_placevalue_reasoning";

    public const string L0506Products = "usccss.g6.u05.l06.decimal_multiplication_methods";
    public const string L0506MethodsLogic = "usccss.g6.u05.l06.decimal_product_scale_reasoning";

    public const string L0507ProductValues = "usccss.g6.u05.l07.decimal_partial_product_values";
    public const string L0507PartialLogic = "usccss.g6.u05.l07.partial_product_reasoning";

    public const string L0508ProductArithmetic = "usccss.g6.u05.l08.multiplication_calculation_values";
    public const string L0508ProductLogic = "usccss.g6.u05.l08.decimal_product_reasoning";

    public const string L0509WholeDivision = "usccss.g6.u05.l09.partial_quotient_results";
    public const string L0509DivisionLogic = "usccss.g6.u05.l09.partial_quotient_principles";

    public const string L0510LongDivisionNumbers = "usccss.g6.u05.l10.long_division_quotient_results";
    public const string L0510LongDivisionLogic = "usccss.g6.u05.l10.long_division_placevalue_interpretation";

    public const string L0511DecimalQuotients = "usccss.g6.u05.l11.whole_division_decimal_results";
    public const string L0511DivisionReasoning = "usccss.g6.u05.l11.decimal_quotient_placevalue_logic";

    public const string L0512DecimalDivision = "usccss.g6.u05.l12.decimal_divided_by_whole";
    public const string L0512DecimalScaleLogic = "usccss.g6.u05.l12.division_scale_equivalence";

    public const string L0513DecimalQuotients = "usccss.g6.u05.l13.decimal_divisor_quotients";
    public const string L0513DecimalLogic = "usccss.g6.u05.l13.decimal_divisor_reasoning";

    internal sealed record Problem(string Family, string Prompt, string Explanation,
        AssessmentItemType ItemType, IReadOnlyDictionary<string,int> Parameters);

    private static readonly (string First,int A,string Second,int B)[] Pairs =
    [
        ("bananas",9,"apples",4),       // source practice P1
        ("plums",3,"apples",4),         // source practice P1
        ("apples",4,"plums",3),         // reverse of P1
        ("bananas",9,"plums",3),        // source practice P1
        ("squares",6,"triangles",3),
        ("triangles",3,"squares",6),
        ("circles",8,"stars",12),
        ("stars",12,"circles",8),
        ("red counters",6,"blue counters",9),
        ("green counters",5,"yellow counters",2),
        ("cards",10,"coins",5),
        ("shells",7,"rocks",3)
    ];

    // (total equal square units, first-color ratio term, second-color term,
    // number of units for the named first color?)
    private static readonly (int Total,int First,int Second,bool FirstColor)[] Shades =
    [
        (24,2,1,true),(24,2,1,false),
        (18,2,1,true),(18,2,1,false),
        (30,2,1,true),(30,2,1,false),
        (27,2,1,true),(27,2,1,false),
        (24,3,1,true),(24,3,1,false),
        (20,3,2,true),(20,3,2,false)
    ];

    // First-party text-accessible discrete diagrams. The source 4-green/2-white
    // practice diagram and 2-cup/6-tablespoon classroom setup are represented,
    // without using the publisher's original artwork.
    private static readonly (string First,int A,string UnitA,string Second,int B,string UnitB)[] Diagrams =
    [
        ("green paint",4,"cups","white paint",2,"cups"),
        ("white paint",2,"cups","green paint",4,"cups"),
        ("white paint",2,"cups","blue paint",6,"tablespoons"),
        ("blue paint",6,"tablespoons","white paint",2,"cups"),
        ("flour",8,"cups","water",2,"pints"),
        ("juice",6,"cups","soda water",3,"cups"),
        ("blue cubes",6,"blocks","red cubes",4,"blocks"),
        ("red blocks",3,"cubes","yellow blocks",5,"cubes"),
        ("paper tiles",5,"pieces","wood tiles",2,"pieces"),
        ("paint",7,"cups","paste",4,"tablespoons"),
        ("squares",4,"tiles","triangles",6,"tiles"),
        ("short bars",9,"pieces","long bars",3,"pieces")
    ];

    private static readonly (string A,int CountA,string UnitA,string B,int CountB,string UnitB,
        string C,int CountC,string UnitC)[] Cards =
    [
        ("raisins",2,"cups","pretzels",4,"cups","almonds",6,"cups"),
        ("tomato sauce",4,"cups","oil",2,"tablespoons","spice",1,"teaspoon"),
        ("flour",3,"cups","milk",6,"cups","eggs",2,"eggs"),
        ("water",1,"cup","fruit",3,"cups","ice",5,"cubes"),
        ("red beads",4,"beads","blue beads",6,"beads","green beads",2,"beads"),
        ("yellow markers",5,"markers","blue markers",2,"markers","red markers",3,"markers"),
        ("orange paint",2,"cups","white paint",3,"cups","black paint",4,"tablespoons"),
        ("rice",6,"cups","beans",2,"cups","herbs",4,"teaspoons"),
        ("paper squares",3,"squares","paper circles",5,"circles","paper triangles",1,"triangle"),
        ("raisins",2,"cups","nuts",4,"cups","seeds",3,"cups"),
        ("juice",7,"cups","water",3,"cups","lemon syrup",2,"tablespoons"),
        ("cardboard",4,"sheets","glue",1,"bottle","paint",6,"tubes")
    ];

    // Original source numerical skills, independently worded and keyed.
    private static readonly (string Prompt,int Answer,string Explanation)[] RecipeBatches =
    [
        ("Base recipe: 3 tsp mustard seeds, 5 tsp chili powder and 1 tsp salt. Diagram shows 9, 15 and 3 tsp respectively. How many complete batches?",3,"Every ingredient is multiplied by 3."),
        ("A batch of chocolate milk has 2 cups milk and 5 tablespoons cocoa. Find milk cups for TWO batches.",4,"2 × 2 = 4 cups milk."),
        ("A batch of chocolate milk has 2 cups milk and 5 tablespoons cocoa. Find cocoa tablespoons for TWO batches.",10,"5 × 2 = 10 tablespoons."),
        ("Sparkling-water:concentrate recipe is 3:1. How many cups sparkling water for TWO batches?",6,"3 × 2 = 6."),
        ("Sparkling-water:concentrate recipe is 3:1. How many cups concentrate for THREE batches?",3,"1 × 3 = 3."),
        ("Cookies use 5 cups flour and 2 tsp vanilla per batch. Flour for TWO batches?",10,"5 × 2 = 10 cups."),
        ("Cookies use 5 cups flour and 2 tsp vanilla per batch. Vanilla for TWO batches?",4,"2 × 2 = 4 tsp."),
        ("Cookies use 5 cups flour and 2 tsp vanilla per batch. Flour for THREE batches?",15,"5 × 3 = 15 cups."),
        ("Cookies use 5 cups flour and 2 tsp vanilla per batch. Vanilla for THREE batches?",6,"2 × 3 = 6 tsp."),
        ("Cookies use 5 cups flour and 2 tsp vanilla per batch. Flour for FIVE batches?",25,"5 × 5 = 25 cups."),
        ("Cookies use 5 cups flour and 2 tsp vanilla per batch. Vanilla for FIVE batches?",10,"2 × 5 = 10 tsp."),
        ("A drink recipe uses 1 cup water and 4 tsp powder. How many tsp powder for TWO batches?",8,"4 × 2 = 8 tsp."),
        ("A drink recipe uses 1 cup water and 4 tsp powder. How many cups water for TWO batches?",2,"1 × 2 = 2 cups."),
        ("Sparkling water:juice concentrate is 3:1. How many cups sparkling water for FIVE batches?",15,"3 × 5 = 15 cups."),
        ("Sparkling water:juice concentrate is 3:1. How many cups concentrate for FIVE batches?",5,"1 × 5 = 5 cups."),
        ("Chocolate milk is 2 cups milk and 5 tbsp cocoa per batch. Milk cups for FOUR batches?",8,"2 × 4 = 8 cups."),
        ("Chocolate milk is 2 cups milk and 5 tbsp cocoa per batch. Cocoa tablespoons for FOUR batches?",20,"5 × 4 = 20 tablespoons."),
        ("Cumulative cube review: from 80 unit cubes, what is the maximum whole-number cube EDGE length?",4,"4 cubed=64, 5 cubed=125 exceeds 80."),
        ("Cumulative cube review: from 80 unit cubes, what is the VOLUME of the largest cube you can build?",64,"The largest perfect cube no more than 80 is 4 cubed=64."),
        ("Cumulative ratio language: 6 dogs for every 5 cats. How many cats in three such groups?",15,"5 × 3 = 15 cats.")
    ];

    private static readonly (string Prompt,int Answer,string Explanation)[] EqualTaste =
    [
        ("Are 1 cup water + 4 tsp powder and 2 cups water + 8 tsp powder equal-strength batches? 1=yes,0=no.",1,"Both quantities doubled."),
        ("Are 1 cup water + 4 tsp powder and 2 cups water + 4 tsp powder equal-strength batches? 1=yes,0=no.",0,"Only water doubled."),
        ("Do sparkling water:grape concentrate 3:1 and 6:2 preserve the recipe? 1=yes,0=no.",1,"Both values doubled."),
        ("Do sparkling water:grape concentrate 3:1 and 6:3 preserve the recipe? 1=yes,0=no.",0,"Different scaling of the two quantities."),
        ("Do cookie flour:vanilla 5:2 and 10:4 preserve the recipe? 1=yes,0=no.",1,"Both doubled."),
        ("Do cookie flour:vanilla 5:2 and 10:3 preserve the recipe? 1=yes,0=no.",0,"The vanilla quantity was not doubled."),
        ("Do chocolate-milk milk:cocoa 2:5 and 4:10 preserve the recipe? 1=yes,0=no.",1,"Both doubled."),
        ("Do chocolate-milk milk:cocoa 2:5 and 4:9 preserve the recipe? 1=yes,0=no.",0,"Ratio changed."),
        ("Do spice-mix mustard:chili:salt 3:5:1 and 9:15:3 preserve the batch ratio? 1=yes,0=no.",1,"All three ingredients tripled."),
        ("Do spice-mix mustard:chili:salt 3:5:1 and 9:15:2 preserve the batch ratio? 1=yes,0=no.",0,"Salt was doubled rather than tripled.")
    ];

    // Color-mixing ratios from original teacher/student/practice tasks, recast
    // as authored single-number selected Practice. Physical color matching and
    // accessibility/tactile dough experiments are teacher-assessed.
    private static readonly (string Prompt,int Answer,string Explanation)[] ColorAmounts =
    [
        ("For a mixture 5 mL blue and 15 mL yellow, how many mL BLUE for TWO identical batches?",10,"Both ingredient quantities double."),
        ("For a mixture 5 mL blue and 15 mL yellow, how many mL YELLOW for TWO identical batches?",30,"15 × 2 = 30 mL."),
        ("Diego uses 10 tbsp yellow and 2 tbsp blue. How many tbsp yellow would go with 3 tbsp blue to preserve shade?",15,"Yellow-to-blue is 5:1; 3 blue needs 15 yellow."),
        ("Diego uses 10 tbsp yellow and 2 tbsp blue. How many tbsp blue with 15 tbsp yellow to preserve shade?",3,"15:3 = 10:2."),
        ("A 1:3 blue:yellow formula has 25 mL blue. How many total mL yellow does it need?",75,"25 × 3 = 75 mL yellow."),
        ("An altered 5:15 mL blue:yellow mixture received +20 mL of EACH and now is 25:35. How many MORE mL yellow to restore 1:3?",40,"Target yellow75; 75-35=40 mL."),
        ("A 17 mL blue and 13 mL yellow mixture needs 1:3 blue:yellow. How much EXTRA yellow must be added?",38,"Yellow target51; 51-13=38 mL."),
        ("Clare's sky blue uses 2 cups blue for 1 gallon white. For TWO batches, how many cups BLUE?",4,"2 × 2 = 4 cups blue."),
        ("Clare's sky blue uses 2 cups blue for 1 gallon white. For TWO batches, how many gallons WHITE?",2,"1 × 2 = 2 gallons white."),
        ("Diego has 10 tbsp yellow for 2 tbsp blue. If BOTH double, how many tbsp yellow?",20,"10 × 2 = 20 tbsp."),
        ("Diego has 10 tbsp yellow for 2 tbsp blue. If BOTH double, how many tbsp blue?",4,"2 × 2 = 4 tbsp."),
        ("Purple formula blue:red=8:3. Jada has 24 mL blue. How many mL red preserve the ratio?",9,"Scale both by3: 24:9."),
        ("Diego's original yellow:blue=10:2. How many tbsp yellow for ONE tbsp blue?",5,"10:2 equivalent to 5:1."),
        ("Diego's original yellow:blue=10:2. How many tbsp blue for 30 tbsp yellow?",6,"Triple every component:30:6."),
        ("An independent orange mix uses3 tbsp red and6 tbsp yellow. Four batches: how many tbsp RED?",12,"3 ×4=12 tbsp red."),
        ("Diego's original green mix has10 tbsp yellow and2 tbsp blue. How many tbsp BLUE in one batch?",2,"Given count is 2 tablespoons blue."),
        ("A purple mix uses 8 mL blue and 3 mL red. How many mL BLUE in the source-size batch?",8,"The first named ingredient quantity is 8 mL."),
        ("Cumulative square area: a square of side5ft occupies how many square feet?",25,"5×5=25 square feet.")
    ];

    private static readonly (string Prompt,int Answer,string Explanation)[] EqualShades =
    [
        ("Will 5:15 blue:yellow and10:30 blue:yellow predict the same shade under consistent ingredients? 1=yes,0=no.",1,"Both double."),
        ("Will 5:15 blue:yellow and25:35 blue:yellow predict the same shade? 1=yes,0=no.",0,"Adding 20 to each does not preserve the ratio."),
        ("Will 10:2 yellow:blue and15:3 yellow:blue preserve Diego's shade? 1=yes,0=no.",1,"Both yellow:blue ratios are 5:1."),
        ("Will 10:2 yellow:blue and11:3 yellow:blue preserve Diego's shade? 1=yes,0=no.",0,"11:3 is not 5:1."),
        ("Will 8:3 blue:red and24:9 blue:red preserve the purple recipe? 1=yes,0=no.",1,"Both scaled by3."),
        ("Will 8:3 blue:red and16:9 blue:red preserve the purple recipe? 1=yes,0=no.",0,"Scale factors are inconsistent."),
        ("Will 2 cups blue:1 gallon white and4 cups blue:2 gallons white preserve the two-batch recipe? 1=yes,0=no.",1,"Both ingredient amounts double in their own units."),
        ("Will 2 cups blue:1 gallon white and4 cups blue:1 gallon white preserve the recipe? 1=yes,0=no.",0,"Only blue increases."),
        ("Will 5:15 blue:yellow and17:51 blue:yellow preserve the numeric ratio? 1=yes,0=no.",1,"Both simplify to1:3."),
        ("Will 5:15 blue:yellow and17:13 blue:yellow preserve the ratio? 1=yes,0=no.",0,"17:13 differs from1:3."),
        ("Does 1 cup flour:5 tbsp water and2 cups flour:10 tbsp water preserve proportions? 1=yes,0=no.",1,"Equal scaling can preserve dough texture under consistent mixing."),
        ("Does 1 cup flour:5 tbsp water and2 cups flour:6 tbsp water preserve proportions? 1=yes,0=no.",0,"Scaling is not common for both quantities.")
    ];

    // Extensible per-lesson numerical selected Practice bank: avoid creating a
    // new engine or modifying the shared exact dispatcher for each Unit 2 lesson.
    // Every bank question is source-scoped; diagrams/explanations are teacher reviewed.
    private sealed record LaterRatioProblem(string Prompt,int Answer,string Explanation);
    private static readonly Dictionary<string,LaterRatioProblem[]> AdditionalLessonFamilies = new(StringComparer.Ordinal)
    {
        [L05Equivalence] =
        [
            new("Are 4:5 and 8:10 equivalent? Enter 1=yes,0=no.",1,"Scale both terms by2."),
            new("Are 18:3 and 6:1 equivalent? 1=yes,0=no.",1,"Divide both by3."),
            new("Are 2:7 and10000:35000 equivalent? 1=yes,0=no.",1,"Both multiply by5000."),
            new("Are 6:4 and18:8 equivalent? 1=yes,0=no.",0,"6-to18 scales ×3,4-to8 ×2."),
            new("Are 3:6 and6:3 equivalent? 1=yes,0=no.",0,"Reversing terms changes the ratio."),
            new("Are 8:12 and12:18 equivalent? 1=yes,0=no.",1,"Both scale ×1.5."),
            new("Are 2:3 and4:5 equivalent? 1=yes,0=no.",0,"Adding2 to each does not preserve a ratio."),
            new("Are 3:4 and15:20 equivalent? 1=yes,0=no.",1,"Scale both by5."),
            new("Are 3:4 and9:10 equivalent? 1=yes,0=no.",0,"Scaling factors3 and2.5 differ."),
            new("Are 3:6:10 and15:30:50 equivalent ordered ingredient ratios? 1=yes,0=no.",1,"All ingredients scale ×5."),
            new("Are 3:6:10 and15:30:40 equivalent? 1=yes,0=no.",0,"Soup changes by different factor."),
            new("Are 4:6 and2:3 equivalent? 1=yes,0=no.",1,"Divide both by2."),
            new("Are 6:4 and18:12 equivalent? 1=yes,0=no.",1,"Both scale ×3."),
            new("Are 6:4 and3:2 equivalent? 1=yes,0=no.",1,"Both divide by2."),
            new("Are 6:4 and18:8 equivalent? 1=yes,0=no.",0,"18/6 differs from8/4.")
        ],
        [L05ScaleMissing] =
        [
            new("One original ratio4:5 becomes ?:10 when both terms double. Find missing first term.",8,"4×2=8."),
            new("One original ratio4:5 becomes 8:? when both terms double. Find missing second term.",10,"5×2=10."),
            new("18:3 simplifies to ?:1. Find first term.",6,"Divide both by3."),
            new("3 cups pasta,6 ounces tuna,10 ounces soup per batch. Soup ounces for HALF a batch?",5,"10×1/2=5 ounces."),
            new("3 cups pasta,6 ounces tuna,10 ounces soup. Pasta cups for FIVE batches?",15,"3×5=15 cups."),
            new("3 cups pasta,6 ounces tuna,10 ounces soup. Tuna ounces for FIVE batches?",30,"6×5=30 ounces."),
            new("3 cups pasta,6 ounces tuna,10 ounces soup. Soup ounces for FIVE batches?",50,"10×5=50 ounces."),
            new("A fixed-height 9 by18 inch casserole tray has base area in square inches?",162,"9×18=162 square inches."),
            new("Same-height casserole double batch needs double the 162 sq-in original base area. New base area?",324,"162×2=324 square inches."),
            new("Same-height casserole half batch needs half the 162 sq-in original base area. New base area?",81,"162/2=81 square inches."),
            new("Source fruit bowl:6 bananas,4 apples,3 oranges. If bananas scale by2, new banana count?",12,"6×2=12."),
            new("Source fruit bowl:6 bananas,4 apples,3 oranges. If oranges scale by6, new orange count?",18,"3×6=18."),
            new("In a4:6 ratio, multiply both terms by3. What is the NEW second term?",18,"6×3=18."),
            new("In a4:5 ratio, multiply both terms by4. What is the NEW second term?",20,"5×4=20."),
            new("Source fruit bowl:6 bananas and3 oranges. How many bananas for EACH orange?",2,"6/3=2."),
            new("In a 6:4 pair, simplifying both by2 gives3:?. Find missing second term.",2,"4/2=2.")
        ],
        [L06DoubleLineAmounts] =
        [
            new("Paint has 2 cups yellow for every 3 cups red. YELLOW cups in 3 batches?",6,"2×3=6 yellow."),
            new("Paint has 2 cups yellow for every 3 cups red. RED cups in 3 batches?",9,"3×3=9 red."),
            new("Cookies use 5 cups flour per 3 eggs. FLOUR cups in 2 batches?",10,"5×2=10 flour."),
            new("Cookies use 5 cups flour per 3 eggs. EGGS in 2 batches?",6,"3×2=6 eggs."),
            new("Cookies use 5 cups flour per 3 eggs. FLOUR cups in 3 batches?",15,"5×3=15 cups."),
            new("Cookies use 5 cups flour per 3 eggs. EGGS in 3 batches?",9,"3×3=9."),
            new("Cookies use 5 cups flour per 3 eggs. FLOUR cups in 4 batches?",20,"5×4=20."),
            new("Cookies use 5 cups flour per 3 eggs. EGGS in 4 batches?",12,"3×4=12."),
            new("Cookies use 5 cups flour per 3 eggs. FLOUR cups with 6 eggs?",10,"Two batches need10 cups flour."),
            new("Cookies use 5 cups flour per 3 eggs. EGGS with 15 cups flour?",9,"Three batches need9 eggs."),
            new("Purple paint uses 3 cups red to 2 cups blue per batch. Number of BATCHES for 12 cups red?",4,"12÷3=4 batches."),
            new("Purple paint uses 3 cups red to 2 cups blue per batch. Number of BATCHES for 6 cups blue?",3,"6÷2=3 batches."),
            new("A pizza plan uses 3 pizzas for every 7 children. How many pizzas for 21 children?",9,"Three sets of 7 children need9 pizzas."),
            new("A parallelogram has base 6 units and perpendicular height4 units. Find its area.",24,"6×4=24 square units."),
            new("A triangle has base8 units and perpendicular height6 units. Find its area.",24,"8×6÷2=24 square units."),
            new("Purple paint uses 3 red cups for every 2 blue cups. How many red cups in 2 batches?",6,"3×2=6 red cups.")
        ],
        [L06DoubleLineLogic] =
        [
            new("Both axes of a double number line share aligned zero marks for their quantities. 1=yes,0=no.",1,"The axes show the same number of batches at aligned ticks."),
            new("For a 2-yellow:3-red mix, 6 yellow and 9 red represent3 batches. 1=yes,0=no.",1,"Both quantities ×3."),
            new("For a 2-yellow:3-red mix, 6 yellow and 6 red preserve the shade. 1=yes,0=no.",0,"6:6 differs from2:3."),
            new("For cookies 5 flour:3 eggs, four batches are 20 cups flour and12 eggs. 1=yes,0=no.",1,"Both quantities ×4."),
            new("For cookies 5 flour:3 eggs, four batches are15 cups flour and12 eggs. 1=yes,0=no.",0,"Flour scales×3, eggs×4."),
            new("Purple ratio3red:2blue,12red:8blue is the same shade. 1=yes,0=no.",1,"Both ×4."),
            new("Purple ratio3red:2blue,12red:6blue is the same shade. 1=yes,0=no.",0,"Factors4 and3 differ."),
            new("Pizza ratio7 kids:3 pizzas,21 kids:9 pizzas is equivalent. 1=yes,0=no.",1,"Both triple."),
            new("Pizza ratio7 kids:3 pizzas,21 kids:3 pizzas is equivalent. 1=yes,0=no.",0,"Only children triple."),
            new("A double number line can use different numerical scales on the two parallel axes while ticks align as batches. 1=yes,0=no.",1,"Same batch positions but different units/scales."),
            new("The ratio 5 cups flour:3 eggs equals 6 cups flour:4 eggs. 1=yes,0=no.",0,"5×4 differs from6×3."),
            new("The ratio2 yellow:3 red equals4 yellow:6 red. 1=yes,0=no.",1,"Both double.")
        ],
        [L07LineValues] =
        [
            new("One cinnamon-roll batch uses2 tablespoons sugar per1 teaspoon cinnamon. Sugar tbsp for3 batches?",6,"2×3=6 tablespoons."),
            new("One cinnamon-roll batch uses2 tbsp sugar per1 tsp cinnamon. Cinnamon tsp for3 batches?",3,"1×3=3 teaspoons."),
            new("One cinnamon-roll batch uses2 tbsp sugar per1 tsp cinnamon. Sugar tbsp for4 batches?",8,"2×4=8 tablespoons."),
            new("One cinnamon-roll batch uses2 tbsp sugar per1 tsp cinnamon. Cinnamon tsp for4 batches?",4,"4 teaspoons."),
            new("One cinnamon-roll batch uses2 tbsp sugar per1 tsp cinnamon. Sugar tbsp for5 batches?",10,"2×5=10 tablespoons."),
            new("One cinnamon-roll batch uses2 tbsp sugar per1 tsp cinnamon. Cinnamon tsp for5 batches?",5,"5 teaspoons."),
            new("Meatloaf uses2 pounds beef per half cup of bread crumbs. Beef pounds for4 batches?",8,"2×4=8 pounds."),
            new("Meatloaf uses2 pounds beef per half cup bread crumbs. Breadcrumb HALF-CUP UNITS for4 batches?",4,"Four halves make2 actual cups."),
            new("Fruit punch has4 cups pineapple juice and5 cups orange juice. Pineapple cups for3 batches?",12,"4×3=12 cups."),
            new("Fruit punch has4 cups pineapple juice and5 cups orange juice. Orange cups for3 batches?",15,"5×3=15 cups."),
            new("Fruit punch has4 cups pineapple juice and5 cups orange juice. Orange cups for5 batches?",25,"5×5=25 cups."),
            new("Pink paint has2 cups red per7 cups white. Red14 cups means how many batches?",7,"14÷2=7."),
            new("Pink paint has2 cups red per7 cups white. White cups for7 batches?",49,"7×7=49."),
            new("A triple batch of a fruit punch with one-third cup lime per batch uses how many THIRD-CUP portions of lime?",3,"3×1/3 cup=1 cup, or3 third-cup portions."),
            new("Find a ratio equivalent to3:11 scaled by4. What is the first term?",12,"3×4=12."),
            new("Find a ratio equivalent to3:11 scaled by4. What is the second term?",44,"11×4=44.")
        ],
        [L07LineLogic] =
        [
            new("For one cinnamon roll filling, 2 tablespoons sugar PER teaspoon cinnamon means for each teaspoon. 1=yes,0=no.",1,"Per means for each."),
            new("A double number line may use uneven tick spacing while claiming equal consecutive batches. 1=yes,0=no.",0,"Equal batches need equally spaced ticks along each line."),
            new("For cinnamon2 sugar:1 cinnamon, three batches6:3 preserve the amounts per batch. 1=yes,0=no.",1,"Both triple."),
            new("For cinnamon2 sugar:1 cinnamon, three batches6:4 preserve amounts per batch. 1=yes,0=no.",0,"Cinnamon quadrupled."),
            new("For meatloaf2lb beef:1/2cup crumbs, 4lb beef:1cup crumbs is equivalent. 1=yes,0=no.",1,"Both double."),
            new("For meatloaf2lb beef:1/2cup crumbs,4lb beef:2cups crumbs is equivalent. 1=yes,0=no.",0,"Different multipliers."),
            new("Fruit punch4 pineapple:5 orange,12 pineapple:15 orange has the same ratio. 1=yes,0=no.",1,"Both triple."),
            new("Fruit punch4 pineapple:5 orange,12 pineapple:20 orange has the same ratio. 1=yes,0=no.",0,"Scaling ×3 vs×4."),
            new("A label of cups cannot be silently replaced with pounds in a double number line. 1=yes,0=no.",1,"Unit semantics matter."),
            new("14 cups red and49 cups white represent seven 2:7 pink-paint batches. 1=yes,0=no.",1,"Both ×7."),
            new("For a double number line, aligned positions show the same batch count across two labeled scales. 1=yes,0=no.",1,"Parallel lines connect paired quantities."),
            new("In 6pints red:2pints yellow, there are3pints red per1pint yellow. 1=yes,0=no.",1,"6:2=3:1.")
        ],
        [L08UnitPrices] =
        [
            new("Source model: 2oz gold cost2640 dollars. Price for1oz in DOLLARS?",1320,"2640÷2=1320 dollars per ounce."),
            new("Source model: 2oz gold cost2640 dollars. Price for3oz in DOLLARS?",3960,"3×1320=3960 dollars."),
            new("Source model: 2oz gold cost2640 dollars. Price for4oz in DOLLARS?",5280,"4×1320=5280 dollars."),
            new("Tomatoes:4lb cost14 dollars. Price for1lb in CENTS?",350,"1400 cents÷4=350 cents per pound."),
            new("Tomatoes:4lb cost14 dollars. Price for2lb in CENTS?",700,"350×2=700 cents."),
            new("Tomatoes:4lb cost14 dollars. Price for3lb in CENTS?",1050,"350×3=1050 cents."),
            new("Four movie tickets cost48 dollars. Price for1 ticket in DOLLARS?",12,"48÷4=12 dollars per ticket."),
            new("Four movie tickets cost48 dollars. Price for5 tickets in DOLLARS?",60,"12×5=60 dollars."),
            new("Four movie tickets cost48 dollars. Price for11 tickets in DOLLARS?",132,"12×11=132 dollars."),
            new("Twelve eggs cost3 dollars. Price for1 egg in CENTS?",25,"300÷12=25 cents per egg."),
            new("Three pounds of peanuts cost7 dollars50 cents. Price per pound in CENTS?",250,"750÷3=250 cents."),
            new("Four rolls of toilet paper cost2 dollars. Price per roll in CENTS?",50,"200÷4=50 cents."),
            new("Ten apples cost3 dollars50 cents. Price per apple in CENTS?",35,"350÷10=35 cents."),
            new("Clare smoothie uses2 tsp chocolate syrup; doubled recipe requires how many tsp syrup?",4,"Double2 to4 teaspoons, not5."),
            new("Clare smoothie doubled should use4 tsp syrup but Kiran added5 tsp. How many tsp EXTRA used?",1,"5-4=1 tsp extra."),
            new("Trapezoidal stage base20ft/10ft with height12ft. What is the area of one trapezoid in ft squared?",180,"(20+10)×12/2=180."),
            new("Trapezoidal stage horizontal top area180ft², perimeter56ft, vertical depth2ft. Area including top and all side panels, excluding bottom?",292,"180+56×2=292 square feet.")
        ],
        [L08PriceLogic] =
        [
            new("If4 tickets cost48 dollars, a ticket costs12 dollars. 1=yes,0=no.",1,"48÷4=12."),
            new("If4 tickets cost48 dollars, 11 tickets cost120 dollars at the same rate. 1=yes,0=no.",0,"11×12=132 dollars."),
            new("If4lb tomatoes cost14 dollars, 1lb costs3 dollars50 cents. 1=yes,0=no.",1,"1400cents÷4=350."),
            new("If4lb tomatoes cost14 dollars, 3lb cost12 dollars. 1=yes,0=no.",0,"3lb=10.50 dollars."),
            new("If12 eggs cost3 dollars, each egg costs25 cents. 1=yes,0=no.",1,"300÷12=25 cents."),
            new("If12 eggs cost3 dollars, 6 eggs cost2 dollars at the same rate. 1=yes,0=no.",0,"6×25cents=150cents."),
            new("If10 apples cost3 dollars50 cents, one apple costs35 cents. 1=yes,0=no.",1,"350÷10=35."),
            new("A price per item compares price and number of items, with units attached. 1=yes,0=no.",1,"Unit price expresses currency for one item."),
            new("At a constant price, doubling the number of items without increasing cost preserves price per item. 1=yes,0=no.",0,"Cost must scale too."),
            new("Two ounces gold2640 dollars makes 3 ounces3960 dollars at this rate. 1=yes,0=no.",1,"1320 dollars per ounce."),
            new("Doubling 1cup yogurt3tbsp peanut butter2tsp syrup requires2cups6tbsp4tsp. 1=yes,0=no.",1,"All quantities double."),
            new("Source smoother with2cups yogurt6tbsp peanut butter5tsp syrup matches twice the original recipe exactly. 1=yes,0=no.",0,"Syrup should4tsp.")
        ],
        [L09SpeedDistances] =
        [
            new("A scooter travels30 feet in2 seconds. Speed in feet per SECOND?",15,"30÷2=15 feet per second."),
            new("A scooter travels30 feet in2 seconds. Feet traveled after1 second at constant speed?",15,"15×1=15 feet."),
            new("A scooter travels30 feet in2 seconds. Feet after3 seconds?",45,"15×3=45 feet."),
            new("A scooter travels30 feet in2 seconds. Feet after4 seconds?",60,"15×4=60 feet."),
            new("A scooter travels30 feet in2 seconds. Feet after5 seconds?",75,"15×5=75 feet."),
            new("Cargo ship travels150 nautical miles in6 hours. Nautical miles in ONE hour?",25,"150÷6=25 nautical miles per hour."),
            new("Cargo ship travels150 nautical miles in6 hours. Nautical miles in3 hours?",75,"25×3=75 nautical miles."),
            new("Han runs10m in2.7sec. At the same speed, time to run50m in TENTHS OF A SECOND?",135,"5×2.7=13.5 seconds or135 tenths."),
            new("Priya runs10m in2.4sec. At the same speed, time to run50m in TENTHS OF A SECOND?",120,"5×2.4=12 seconds or120 tenths."),
            new("Pasta dough uses150 grams flour per egg. Grams for6 eggs?",900,"150×6=900g."),
            new("Pasta dough uses150 grams flour per egg. Eggs for450g flour?",3,"450÷150=3 eggs."),
            new("Four frozen vegetable bags cost11 dollars96 cents. Price for one bag in CENTS?",299,"1196÷4=299 cents."),
            new("Four frozen vegetable bags cost11 dollars96 cents. Nine bags cost how many CENTS?",2691,"299×9=2691 cents."),
            new("Five cats have4 paws per cat. Total number of PAWS?",20,"5×4=20 paws."),
            new("Five cats have2 ears per cat. Total number of EARS?",10,"5×2=10 ears."),
            new("Five cats have4 paws and2 ears each. How many MORE paws than ears in total?",10,"20-10=10."),
            new("At constant speed2 meters every4 seconds, how many meters in10 seconds?",5,"2/4=1/2m/s;10×1/2=5m."),
            new("At constant speed5m every10s, how many seconds to travel20m?",40,"4×10=40 seconds.")
        ],
        [L09SpeedComparisons] =
        [
            new("Han runs10m in2.7 seconds while Priya runs10m in2.4 seconds. Is Priya faster? 1=yes,0=no.",1,"Same distance in less time means faster."),
            new("Han runs10m in2.7sec while Priya runs10m in2.4sec. Is Han faster? 1=yes,0=no.",0,"Priya takes less time."),
            new("Scooter30ft in2sec versus skateboard55ft in4sec: is scooter faster? 1=yes,0=no.",1,"15ft/s exceeds13.75ft/s."),
            new("Scooter30ft in2sec versus skateboard55ft in4sec: is skateboard faster? 1=yes,0=no.",0,"13.75ft/s below15ft/s."),
            new("A scooter travels30ft in2sec. Can it travel75ft in5sec at constant speed? 1=yes,0=no.",1,"15ft/s×5=75ft."),
            new("A scooter travels30ft in2sec. Can it travel55ft in5sec at constant speed? 1=yes,0=no.",0,"At15ft/s,5sec makes75ft."),
            new("A ship at150 nautical miles per6hours travels25 nautical miles per hour. 1=yes,0=no.",1,"150÷6=25."),
            new("A ship at150 nautical miles per6hours travels30 nautical miles per hour. 1=yes,0=no.",0,"Rate is25."),
            new("On a constant-speed double number line, equal time steps yield equal distance steps. 1=yes,0=no.",1,"Each step repeats the same distance/time ratio."),
            new("Five cats have paws-to-ears ratio20:10=2:1. 1=yes,0=no.",1,"20/10=2."),
            new("The ratios198:1287 and2:13 are equivalent. 1=yes,0=no.",1,"198×13=1287×2."),
            new("At the same distance, the runner taking MORE seconds has greater average speed. 1=yes,0=no.",0,"More time for same distance means lower speed.")
        ],
        [L10CompareAmounts] =
        [
            new("Slug travels3 cm in3 sec. Speed in centimeters per second?",1,"3/3=1cm per second."),
            new("Snail travels6 cm in6 sec. Speed in centimeters per second?",1,"6/6=1cm per second."),
            new("Plane travels3360 miles in6 hours. Speed in miles per hour?",560,"3360/6=560mph."),
            new("Plane travels3360 miles in6 hours. Miles in10 hours at same speed?",5600,"560×10=5600 miles."),
            new("Ground beef costs5 dollars per pound. Price for3 pounds in CENTS?",1500,"3×500=1500cents."),
            new("Ground beef costs5 dollars per pound. Price for one-half pound in CENTS?",250,"500/2=250cents."),
            new("Ground beef costs5 dollars per pound. Price for one-quarter pound in CENTS?",125,"500/4=125cents."),
            new("Ground beef costs5 dollars per pound. Price for three-quarters of a pound in CENTS?",375,"500×3/4=375cents."),
            new("Ground beef costs5 dollars per pound. Price for3¾ pounds in CENTS?",1875,"500×15/4=1875cents."),
            new("Tulips cost11 dollars for5 bulbs at store A. Price per bulb in CENTS?",220,"1100/5=220 cents."),
            new("Tulips cost13 dollars for6 bulbs at store B. Price for12 bulbs at that rate in CENTS?",2600,"Two six-bulb bundles cost26 dollars."),
            new("Purple paint model has5 cups purple and2cups white. For 15cups purple, white cups at same ratio?",6,"Scale both×3:15purple6white."),
            new("Purple paint model has5 cups purple and2cups white. For 3 batches, purple cups?",15,"5×3=15."),
            new("Spice mix6tsp garlic to15tsp salt. Garlic tsp for5tsp salt?",2,"6:15=2:5."),
            new("Spice mix6tsp garlic to15tsp salt. Salt tsp for8tsp garlic?",20,"8×5/2=20."),
            new("Spice mix6tsp garlic to15tsp salt. If total14tsp at same ratio, salt tsp?",10,"2:5 ratio, 14total gives4garlic10salt."),
            new("Spice mix6tsp garlic to15tsp salt. How much MORE salt than garlic in original recipe?",9,"15-6=9tsp."),
            new("Slugs3cm in3sec and snails6cm in6sec: for a10 second period at1cm/sec, distance in cm?",10,"Constant rate1cm/sec×10=10cm.")
        ],
        [L10EquivalentContexts] =
        [
            new("Slug3cm in3sec and snail6cm in6sec travel at the same speed. 1=yes,0=no.",1,"Both1cm per second."),
            new("Snail6cm in6sec is faster than slug3cm in3sec because its distance is larger. 1=yes,0=no.",0,"Rate is the same."),
            new("Milkshake2 chocolate scoops:1cup milk is more chocolate-heavy than3 scoops:2cups milk. 1=yes,0=no.",1,"2 per cup exceeds1.5 per cup."),
            new("Milkshake3 chocolate scoops:2cups milk is more chocolate-heavy than2 scoops:1cup milk. 1=yes,0=no.",0,"3/2<2."),
            new("Purple mix B15 purple:8 white has MORE white per purple than A5 purple:2 white. 1=yes,0=no.",1,"8/15>2/5."),
            new("Purple mix B15 purple:8white preserves A5 purple:2white ratio exactly. 1=yes,0=no.",0,"3×5=15 but3×2=6, not8."),
            new("Store A5 tulips for11dollars and store B6 for13dollars have exactly equal unit prices. 1=yes,0=no.",0,"A=2.20 dollars each, B≈2.17."),
            new("Store B6 tulips for13dollars is cheaper per bulb than A5 for11dollars. 1=yes,0=no.",1,"13/6<11/5."),
            new("Plane3360 miles in6hours moves at560 miles per hour. 1=yes,0=no.",1,"3360/6=560."),
            new("One-quarter pound beef at5 dollars per pound costs one-quarter of5 dollars. 1=yes,0=no.",1,"125cents."),
            new("Spice garlic6:salt15 is equivalent to garlic2:salt5. 1=yes,0=no.",1,"Both÷3."),
            new("With equal travel TIME, the runner covering more DISTANCE has greater average speed. 1=yes,0=no.",1,"Distance per same time comparison.")
        ],
        [L11TableAmounts] =
        [
            new("A car travels70km every hour. Distance in km after14 hours?",980,"70Ã—14=980km."),
            new("A car travels70km every hour. Distance in km after3 hours?",210,"70Ã—3=210km."),
            new("Olives20 pounds make3 liters olive oil. How many liters from3000 pounds?",450,"3000/20Ã—3=450 liters."),
            new("Olives20 pounds make3 liters. How many liters from100 pounds?",15,"100/20Ã—3=15 liters."),
            new("A recess needs2 adults for24 children. Adults needed for72 children?",6,"72/24Ã—2=6."),
            new("A recess needs2 adults for24 children. Children for8 adults?",96,"8/2Ã—24=96."),
            new("A recess needs2 adults for24 children. Children for4 adults?",48,"24Ã—2=48."),
            new("A recess needs2 adults for24 children. Children for6 adults?",72,"24Ã—3=72."),
            new("Jogging at25 beats every10s, beats in60s?",150,"25Ã—6=150beats per minute."),
            new("Six ears corn cost180cents. Price per ear in CENTS?",30,"180/6=30."),
            new("Twelve apples cost288cents. Price per apple in CENTS?",24,"288/12=24."),
            new("Five tomatoes cost310cents. Price per tomato in CENTS?",62,"310/5=62."),
            new("Orange paint source first row5 quarts yellow for6 quarts red. Yellow quarts for3 batches?",15,"5Ã—3=15."),
            new("Orange paint source first row5 quarts yellow for6 quarts red. Red quarts for3 batches?",18,"6Ã—3=18."),
            new("Olive oil20lb for3L. Pounds olives for15L?",100,"15/3Ã—20=100."),
            new("A car goes70km per hour. How many hours to travel210km?",3,"210/70=3hours."),
            new("For2 adults and24 children per group, total people in3 equal groups?",78,"3Ã—(2+24)=78.")
        ],
        [L11TableLogic] =
        [
            new("Car1hour70km and14hours980km are a constant-rate ratio. 1=yes,0=no.",1,"Scale both quantitiesÃ—14."),
            new("Car1hour70km and14hours900km keep the same speed. 1=yes,0=no.",0,"14Ã—70=980km."),
            new("20lb olives3L and100lb olives15L are equivalent. 1=yes,0=no.",1,"BothÃ—5."),
            new("20lb olives3L and100lb olives12L are equivalent. 1=yes,0=no.",0,"Liters should15."),
            new("2 adults24children and6 adults72children are equivalent ratios. 1=yes,0=no.",1,"BothÃ—3."),
            new("2 adults24children and6 adults48children are equivalent. 1=yes,0=no.",0,"AdultsÃ—3 but childrenÃ—2."),
            new("A ratio table can list equivalent pairs in any row order, unlike an ordered number line. 1=yes,0=no.",1,"Rows need not follow geometric ordering."),
            new("Table entries must preserve correct labeled units for both columns. 1=yes,0=no.",1,"Column units give ratios meaning."),
            new("Jogging25 beats in10seconds is150bpm, which is lower than160bpm. 1=yes,0=no.",1,"25Ã—6=150."),
            new("Jogging25 beats in10seconds exceeds160bpm. 1=yes,0=no.",0,"It is150bpm."),
            new("Six corn ears for180cents means30cents per ear. 1=yes,0=no.",1,"Divide180 by6."),
            new("Two ratio table rows can be equivalent if only one column is multiplied by2. 1=yes,0=no.",0,"Both corresponding values scale equally.")
        ],
        [L12TableCalculations] =
        [
            new("Each penny has mass2.5g. Priya collected2400g. Number of pennies?",960,"2400 divided by2.5 equals960 pennies."),
            new("Each penny weighs2.5g. Priya collected2400g. Total money in CENTS?",960,"960 pennies are960 cents."),
            new("Kiran reads5 pages in20 minutes. Minutes for11 pages at same pace?",44,"20 divided by5 is4minutes per page; 11 takes44minutes."),
            new("Kiran reads5 pages in20 minutes. Minutes per page?",4,"20 divided by5 gives4minutes."),
            new("Mai makes4 pizzas with10 ounces of cheese. Ounces per pizza in HALF-OUNCE units?",5,"10/4=2.5oz=5 half-ounce units."),
            new("Mai makes4 pizzas with10 ounces cheese. Ounces for15 pizzas in HALF-OUNCE units?",75,"15×2.5=37.5oz=75 half-ounce units."),
            new("Clare earns90 dollars for5 hours. Dollars per hour?",18,"90 divided by5 gives18dollars/hour."),
            new("Clare earns90 dollars for5 hours. Seconds needed to earn25 cents at same rate?",50,"18dollars/hour is1800 cents/3600 sec=0.5 cents/sec, so25 cents needs50sec."),
            new("Car travels20miles in half hour. Miles per hour?",40,"20 divided by0.5=40mph."),
            new("Car travels30miles in three-quarter hour. Miles per hour?",40,"30 divided by0.75=40mph."),
            new("A fruit mixture has9fl oz cranberry and21fl oz apple. Cranberry fl oz for2 groups of3:7?",6,"One scaled3:7 pair repeats twice to6:14."),
            new("A fruit mixture has9fl oz cranberry and21fl oz apple. Apple fl oz for2 groups of3:7?",14,"2×7=14."),
            new("600:450 simplifies to4:3 by dividing first term by what positive whole number?",150,"600/150=4."),
            new("From a ratio4:3, multiplying both terms by15 gives what FIRST term?",60,"4×15=60."),
            new("Mai uses10oz cheese for4 pizzas. Cheese ounces for8 pizzas?",20,"Double both quantities."),
            new("A table has5pages per20min. Pages read in60minutes?",15,"60/20×5=15pages.")
        ],
        [L12TableStrategies] =
        [
            new("600:450 and60:45 are equivalent ratios. 1=yes,0=no.",1,"Both divide by10."),
            new("60:45 and4:3 are equivalent ratios. 1=yes,0=no.",1,"Both divide by15."),
            new("600:450 and4:3 are equivalent ratios. 1=yes,0=no.",1,"Both divide by150."),
            new("600:450 and60:40 are equivalent ratios. 1=yes,0=no.",0,"Second must45."),
            new("5 pages in20min means reading each page at an average4 minutes. 1=yes,0=no.",1,"20/5=4."),
            new("5 pages in20min means11 pages take40 minutes at same pace. 1=yes,0=no.",0,"11×4=44."),
            new("4 pizzas need10oz cheese, so one pizza uses2.5oz. 1=yes,0=no.",1,"10/4=2.5."),
            new("4 pizzas need10oz cheese, so15 pizzas need30oz. 1=yes,0=no.",0,"15×2.5=37.5."),
            new("20miles in half hour and30miles in three-quarter hour represent same speed. 1=yes,0=no.",1,"Both40mph."),
            new("20miles in half hour and30miles in one full hour represent same speed. 1=yes,0=no.",0,"40 vs30mph."),
            new("A ratio table allows multiplying both quantities by a common nonzero positive factor to reach a new row. 1=yes,0=no.",1,"Same multiplicative relationship."),
            new("Clare earns90 dollars in5hours; she earns25 cents in50sec at that constant rate. 1=yes,0=no.",1,"18 dollars/hour=0.5 cents/sec.")
        ],
        [L13RepresentationValues] =
        [
            new("Bread recipe3 tablespoons olive oil to2 cloves garlic. Oil tablespoons for4 batches?",12,"3 times4=12."),
            new("Bread recipe3 tablespoons oil to2 cloves garlic. Garlic cloves for4 batches?",8,"2 times4=8."),
            new("Bread recipe3 tablespoons oil to2 cloves garlic. Oil tablespoons for2 batches?",6,"3 times2=6."),
            new("Bread recipe3 tablespoons oil to2 cloves garlic. Garlic cloves for5 batches?",10,"2 times5=10."),
            new("Clare travels72 miles in2hours. Miles per hour?",36,"72 divided by2=36mph."),
            new("Clare travels72 miles in2hours. Miles in3hours?",108,"36 times3=108."),
            new("Clare travels72 miles in2hours. Miles in6hours?",216,"36 times6=216."),
            new("Clare travels72 miles in2hours. Miles in13 HALF-HOURS?",234,"13 times18=234 miles in6.5h."),
            new("Clare travels72 miles in2hours. Miles in1hour?",36,"One half of72 is36."),
            new("Lin travels23mi in half hour. Miles in1hour at same rate?",46,"23 times2=46mph."),
            new("Diego travels25mi in half hour. Miles in1hour at same rate?",50,"25 times2=50mph."),
            new("Lin takes65 minutes to travel60miles, Diego70minutes. Who is faster? Enter1 for Lin,2 for Diego.",1,"For same60miles Lin takes fewer minutes."),
            new("Sports drink uses5/3 tbsp powder per12oz water. How many batches with5 tbsp powder?",3,"5 divided by5/3 is3."),
            new("Sports drink uses5/3 tbsp powder per12oz water. How many batches with36oz water?",3,"36/12=3."),
            new("Three tbsp olive oil per two cloves garlic. How many tbsp oil for10 cloves?",15,"10/2 times3=15."),
            new("A car at36mph travels how many miles in HALF an hour?",18,"36/2=18.")
        ],
        [L13RepresentationLogic] =
        [
            new("A ratio table can list equivalent rows in non-increasing order. 1=yes,0=no.",1,"Table row ordering is independent of geometric distance."),
            new("The physical spacing on a double number line encodes differences in the two quantities. 1=yes,0=no.",1,"Both axes map numerical distances."),
            new("A table is always invalid for very small fractions. 1=yes,0=no.",0,"Tables can show fractional values."),
            new("A ratio table is often easier than a physically drawn long number line for very large values. 1=yes,0=no.",1,"Geometry can become impractical."),
            new("3tbsp oil:2 garlic cloves and6tbsp oil:4 cloves preserve recipe ratio. 1=yes,0=no.",1,"Both double."),
            new("3tbsp oil:2 garlic cloves and6tbsp oil:5 cloves preserve ratio. 1=yes,0=no.",0,"Factors differ."),
            new("Clare72miles in2h travels at36 miles per hour. 1=yes,0=no.",1,"72/2=36."),
            new("Lin23mi in30min is faster than Diego25mi in30min. 1=yes,0=no.",0,"Same time, Diego covers more distance."),
            new("For same60miles, Lin65min is faster than Diego70min. 1=yes,0=no.",1,"Less time for same distance."),
            new("A drink with5/3 tablespoons powder and12oz water is exactly3 batches when using5 tbsp and36oz. 1=yes,0=no.",1,"Scale both by3."),
            new("For bread 3 tbsp oil:2 cloves garlic,5 tablespoons oil corresponds to10 cloves at the same rate. 1=yes,0=no.",0,"10 cloves needs15 tbsp."),
            new("A table and a double number line may describe the same equivalent ratio family. 1=yes,0=no.",1,"Both preserve corresponding pair values.")
        ],
        [L14InfoGapNumbers] =
        [
            new("Vinegar costs3 dollars for2 gallons. Cost of1 gallon in CENTS?",150,"300cents divided by2=150cents."),
            new("Vinegar costs3 dollars for2 gallons. Cost of15 gallons in CENTS?",2250,"15 times150cents=2250cents."),
            new("Vinegar costs3 dollars for2 gallons. Cost of30 gallons in CENTS?",4500,"30 times150=4500cents."),
            new("Eight pounds pasta cost12 dollars. Price for1 pound in CENTS?",150,"1200 divided by8=150cents."),
            new("Eight pounds pasta cost12 dollars. Price for21 pounds in CENTS?",3150,"21 times150=3150cents."),
            new("Lin reads20pages in35minutes. At same rate, time for47 pages in QUARTER-MINUTES?",329,"47 times35 divided by20=82.25minutes=329quarter-minute units."),
            new("Lin reads20pages in35minutes. Additional time beyond1hour for47pages, in SECONDS?",1335,"82.25minutes minus60=22.25minutes=1335seconds."),
            new("Diego types140words in4minutes. Words per minute?",35,"140 divided by4=35words/min."),
            new("Diego types140words in4minutes. Minutes for385words?",11,"385/35=11minutes."),
            new("Diego types140words in4minutes. Words in15minutes?",525,"35 times15=525words."),
            new("Train travels30miles in one-third hour. Miles per hour?",90,"30 divided by1/3=90mph."),
            new("Train travels20miles in half hour. Miles per hour?",40,"20 divided by1/2=40mph."),
            new("Triangular-prism net has three rectangle faces length12,10,10 and height4 inches. Total area of rectangles in square inches?",128,"4 times(12+10+10)=128."),
            new("Two congruent triangles each base12in and height8in. Combined triangle area in square inches?",96,"Two triangles each12 times8 divided by2=48, combined96."),
            new("Prism source net combines rectangular area128 and triangular ends96 square inches. Total surface area?",224,"128+96=224square inches."),
            new("A rate of140words per4minutes, words in2minutes?",70,"35 times2=70words.")
        ],
        [L14InfoGapReasoning] =
        [
            new("At a constant cost of3 dollars per2gallons,1gallon costs1.50 dollars. 1=yes,0=no.",1,"Divide both amounts by2."),
            new("At3 dollars per2 gallons,4 gallons cost3 dollars. 1=yes,0=no.",0,"Four gallons cost6 dollars."),
            new("At3 dollars per2 gallons,30 gallons cost45 dollars. 1=yes,0=no.",1,"15 times the source pair."),
            new("At3 dollars per2 gallons,2 dollars buys30 gallons. 1=yes,0=no.",0,"2 dollars buys4/3 gallons."),
            new("At3 dollars per2 gallons,1 dollar buys2/3 gallon. 1=yes,0=no.",1,"Divide both by3."),
            new("For21lb pasta at8lb per12dollars, knowing the unit price helps. 1=yes,0=no.",1,"1.50 dollars per pound."),
            new("Lin47pages at20pages/35minutes will finish within60minutes. 1=yes,0=no.",0,"It takes82.25minutes."),
            new("Diego140words in4minutes has a constant rate35words/minute. 1=yes,0=no.",1,"140/4=35."),
            new("A train30miles in1/3hour is faster than another20miles in1/2hour. 1=yes,0=no.",1,"90mph exceeds40mph."),
            new("An info-gap solver should ask specifically for the missing relevant quantity and explain why. 1=yes,0=no.",1,"Ask precise what/why questions."),
            new("A learner must guess unknown source information rather than ask their partner in an info-gap. 1=yes,0=no.",0,"The activity expects explicit requests."),
            new("For a price-to-weight problem, the unit price can be used to scale the required weight. 1=yes,0=no.",1,"Multiply dollars/pound by pounds.")
        ],
        [L15TapeParts] =
        [
            new("Kennel has cats:dogs=4:5, 27 animals total. Animals per ratio PART?",3,"27 divided by9 parts=3."),
            new("Kennel has cats:dogs=4:5, 27 total. How many cats?",12,"4 parts times3=12."),
            new("Kennel has cats:dogs=4:5, 27 total. How many dogs?",15,"5 parts times3=15."),
            new("Kennel has cats:dogs=4:5, 27 total. Number of ratio parts?",9,"4+5=9."),
            new("Weather has sunny:rainy=4:1, 30 total days. How many rainy days?",6,"30 divided by5parts=6."),
            new("Weather has sunny:rainy=4:1,30 total days. How many sunny days?",24,"4 times6=24."),
            new("Bike ride covers32miles in160minutes. Minutes per mile?",5,"160/32=5min per mile."),
            new("Bike ride covers32miles in160minutes. Minutes for100miles?",500,"100 times5=500minutes."),
            new("Cashier earns58 dollars in8hours. Hourly pay in CENTS?",725,"5800cents/8=725 cents."),
            new("Cashier earns58 dollars in8hours. Earnings for3hours in CENTS?",2175,"725 times3=2175cents."),
            new("Three-pound orange bags cost4 dollars. Price per pound in CENTS rounded to nearest whole cent?",133,"400/3=133.333 cents, rounded 133."),
            new("Eight-pound orange bags cost9 dollars. Price per pound in THOUSANDTHS of a DOLLAR?",1125,"9/8=1.125dollars per pound."),
            new("First-party mixture3 cups yellow and2 cups blue. Total green mixture volume in CUPS under additive liquids?",5,"3+2=5cups."),
            new("First-party mixture3 cups yellow and2 cups blue. Total cups for4 identical batches?",20,"4 times(3+2)=20."),
            new("A ratio3 red to2 yellow has5 total parts. If there are25 total cups, red cups?",15,"25/5 times3=15."),
            new("A ratio3 red to2 yellow has5 total parts. If there are25 total cups, yellow cups?",10,"25/5 times2=10.")
        ],
        [L15TapeLogic] =
        [
            new("For cats:dogs4:5, part-to-whole cats:total is4:9. 1=yes,0=no.",1,"Total parts4+5=9."),
            new("For cats:dogs4:5, part-to-whole dogs:total is5:4. 1=yes,0=no.",0,"Dogs to whole=5:9."),
            new("At cats:dogs4:5 with27 animals, 12 cats and15 dogs fits. 1=yes,0=no.",1,"Both are3 times ratio terms."),
            new("At cats:dogs4:5 with27 animals, 15 cats and12 dogs fits. 1=yes,0=no.",0,"Order is reversed."),
            new("At sunny:rainy4:1 over30 days, there are6 rainy days. 1=yes,0=no.",1,"One part6."),
            new("At sunny:rainy4:1 over30 days, there are10 rainy days. 1=yes,0=no.",0,"Total5parts, each6."),
            new("When two paint liquids have compatible volume units, 3cups yellow+2cups blue models5cups of mixture, assuming additive volumes. 1=yes,0=no.",1,"Sums compatible liquid volumes under model assumptions."),
            new("You may always add measured cups of dry oatmeal to water and claim the same total fluid volume. 1=yes,0=no.",0,"Volume can be nonadditive."),
            new("For a ratio a:b, the whole consists of a+b equal ratio parts in the part-part-whole model. 1=yes,0=no.",1,"Sum ratio units for two categories."),
            new("Four sunny days for every rainy day means rainy:total=1:5. 1=yes,0=no.",1,"Four sunny and one rainy part."),
            new("At32miles per160minutes,100miles takes500minutes at same pace. 1=yes,0=no.",1,"5 minutes/mile."),
            new("A 3-pound orange bag4 dollars has lower cost per pound than an8-pound bag9 dollars. 1=yes,0=no.",0,"4/3=1.333 vs9/8=1.125.")
        ],
        [L16MultistepRatio] =
        [
            new("Piggy bank ratios nickels:dimes=2:3 and dimes:quarters=2:5. Combined smallest integer ratio for NICKELS?",4,"Bring dime ratio to6: nickels4,dimes6,quarters15."),
            new("Piggy bank ratios nickels:dimes=2:3 and dimes:quarters=2:5. Combined smallest ratio for DIMES?",6,"Nickels4:dimes6:quarters15."),
            new("Piggy bank ratios nickels:dimes=2:3 and dimes:quarters=2:5. Combined smallest ratio for QUARTERS?",15,"Nickels4:dimes6:quarters15."),
            new("Piggy bank has500 coins with nickels:dimes:quarters=4:6:15. How many NICKELS?",80,"500 divided by25=20;4×20=80."),
            new("Piggy bank has500 coins with nickels:dimes:quarters=4:6:15. How many DIMES?",120,"6×20=120."),
            new("Piggy bank has500 coins with nickels:dimes:quarters=4:6:15. How many QUARTERS?",300,"15×20=300."),
            new("Piggy bank has80nickels120dimes300quarters. Total VALUE in CENTS?",9100,"80×5+120×10+300×25=9100."),
            new("Horse A gallops32ft/sec and horse B28ft/sec. How many feet farther after5sec?",20,"(32-28)×5=20 feet."),
            new("Horse A gallops32ft/sec. Distance in feet after5sec?",160,"32×5=160feet."),
            new("Horse B gallops28ft/sec. Distance in feet after5sec?",140,"28×5=140feet."),
            new("Andre paid13 dollars for3 books. At the same rate Diego pays how many DOLLARS for12 books?",52,"12/3×13=52 dollars."),
            new("Andre paid13 dollars for3 books. At same rate, how many dollars for6 books?",26,"6/3×13=26."),
            new("Nickels:dimes:quarters=4:6:15 has how many total RATIO PARTS?",25,"4+6+15=25."),
            new("A ratio2:3 scales to 6:9. If total objects are15, how many FIRST category?",6,"2/5 of15=6."),
            new("A rectangular area model needs 3 equal parts width and5 height. Area in square units?",15,"3×5=15."),
            new("Horse A and B rates differ by how many FEET PER SECOND?",4,"32-28=4.")
        ],
        [L16RepresentationChoice] =
        [
            new("Nickels:dimes2:3 and dimes:quarters2:5 together give4:6:15. 1=yes,0=no.",1,"Common dime amount6."),
            new("Nickels:dimes2:3 and dimes:quarters2:5 together give2:3:5. 1=yes,0=no.",0,"The dime count must agree across ratios."),
            new("A bank with nickels80,dimes120,quarters300 has500 total coins. 1=yes,0=no.",1,"80+120+300=500."),
            new("The value of80 nickels,120 dimes and300 quarters is91 dollars. 1=yes,0=no.",1,"400+1200+7500=9100cents."),
            new("32ft/s and28ft/s differ by20ft after5seconds. 1=yes,0=no.",1,"Difference4ft/s times5=20ft."),
            new("If two horses have speeds32 and28 feet/sec, their distance difference shrinks over time. 1=yes,0=no.",0,"It grows4feet each second."),
            new("Books priced3 for13 dollars cost52 dollars for12 at same rate. 1=yes,0=no.",1,"Four groups of3 books cost4×13."),
            new("For a ratio-total question, a tape diagram can help count the number of equal parts. 1=yes,0=no.",1,"Equal blocks depict the whole."),
            new("For constant speeds over time, a double number line can show aligned elapsed time and distance. 1=yes,0=no.",1,"Same time steps align rates."),
            new("One model is required for every ratio problem even when another representation is valid. 1=yes,0=no.",0,"Students may choose a fitting tool."),
            new("The nickels:dimes ratio4:6 simplifies to2:3. 1=yes,0=no.",1,"Divide both by2."),
            new("The dimes:quarters ratio6:15 simplifies to2:5. 1=yes,0=no.",1,"Divide both by3.")
        ],
        [L17FermiAssumptions] =
        [
            new("FIRST-PARTY Fermi assumption: a school has24 classrooms with25 students each. Estimated TOTAL students?",600,"24×25=600 under these explicit assumptions."),
            new("FIRST-PARTY Fermi assumption:600 students use3 pages each daily. Daily pages?",1800,"600×3=1800 pages daily."),
            new("FIRST-PARTY Fermi assumption: a class has25 students, each uses4 pencils in a term. Class pencils?",100,"25×4=100 pencils."),
            new("FIRST-PARTY Fermi assumption:40 classes each use100 pencils per term. School pencils?",4000,"40×100=4000 pencils."),
            new("FIRST-PARTY Fermi assumption: 5 schooldays per week,40 weeks. Total schooldays?",200,"5×40=200 days."),
            new("FIRST-PARTY Fermi assumption:600 students make2 journeys per schoolday over200 schooldays. Total one-way journeys?",240000,"600×2×200=240000."),
            new("FIRST-PARTY Fermi assumption: a bus holds40 riders. Buses needed to carry600 riders in one trip?",15,"600/40=15 buses."),
            new("FIRST-PARTY Fermi assumption: bookshelves each hold30 books, library has45 shelves. Estimated books?",1350,"45×30=1350 books."),
            new("FIRST-PARTY Fermi assumption: 1200 bottles discarded over5 days. Bottles per day under a uniform model?",240,"1200/5=240 bottles/day."),
            new("FIRST-PARTY Fermi assumption: a hallway has80 floor tiles per row and6 rows. Total tiles?",480,"80×6=480 tiles."),
            new("FIRST-PARTY Fermi assumption: school cafeteria serves250 lunches a day for20 days. Monthly lunches?",5000,"250×20=5000 lunches."),
            new("FIRST-PARTY Fermi assumption: 300 pupils each walk2000 meters per day. Total meters walked?",600000,"300×2000=600000 meters.")
        ],
        [L17FermiSensitivity] =
        [
            new("Assuming24 classrooms each contain25 students gives600 students, not an exact census. 1=yes,0=no.",1,"600 is an estimate under assumptions."),
            new("Assuming24 classrooms have25 students each proves every actual class has25 students. 1=yes,0=no.",0,"Model assumptions are not observations."),
            new("If assumed students per class double and number of classes stays fixed, predicted total doubles. 1=yes,0=no.",1,"Multiplicative scaling."),
            new("If number of classes doubles but student count per class stays fixed, predicted total stays unchanged. 1=yes,0=no.",0,"Predicted total doubles."),
            new("Two different plausible assumptions can produce different legitimate Fermi estimates. 1=yes,0=no.",1,"Open-ended estimates depend on premises."),
            new("The exact numeric answer from a fixed-assumptions exercise automatically proves a student's open Fermi research project correct. 1=yes,0=no.",0,"Teacher must assess assumptions, evidence and process."),
            new("Choosing upper and lower bounds is useful for evaluating sensitivity of an estimate. 1=yes,0=no.",1,"Bounds communicate uncertainty."),
            new("A Fermi estimate should always omit units to make arithmetic easier. 1=yes,0=no.",0,"Units are essential."),
            new("Breaking a broad question into smaller measurable questions is appropriate Fermi modeling. 1=yes,0=no.",1,"Subquestions make estimates manageable."),
            new("One can multiply estimated riders per bus by buses to estimate passenger capacity. 1=yes,0=no.",1,"Capacity scales multiplicatively."),
            new("A rough estimate should be presented as an exact observed count solely because arithmetic is correct. 1=yes,0=no.",0,"Assumptions remain uncertain."),
            new("Recording an assumption's source and how changing it affects the result strengthens the estimate. 1=yes,0=no.",1,"Transparent model assumptions support review.")
        ],
        [L0301PerOne] =
        [
            new("Source elevator moves310 feet in10 seconds. Feet per second?",31,"310/10=31 feet per second."),
            new("Source elevator moves310 feet in10 seconds. Feet traveled in12 seconds at constant speed?",372,"31×12=372 feet."),
            new("Source elevator moves310 feet in10 seconds. Feet traveled in5 seconds?",155,"31×5=155 feet."),
            new("Han earns33 dollars for4 hours babysitting. Hourly pay in CENTS?",825,"3300/4=825 cents."),
            new("Han earns33 dollars for4 hours babysitting. Pay for7 hours in CENTS?",5775,"7×825=5775 cents."),
            new("Han earns33 dollars for4 hours babysitting. Pay for8 hours in DOLLARS?",66,"Two batches of4 hours."),
            new("Five dog-food cans cost435 cents. Price for one can in CENTS?",87,"435/5=87 cents."),
            new("Five dog-food cans cost435 cents. Cost of11 cans in CENTS?",957,"87×11=957 cents."),
            new("A restaurant clears4 tables in10minutes. Minutes to clear26 tables at same rate?",65,"26×10/4=65 minutes."),
            new("A restaurant clears4 tables in10minutes. How many tables in30minutes at this rate?",12,"4×3=12 tables."),
            new("Sandwich uses4 ounces meat and3 ounces cheese. Combined ounces per sandwich?",7,"4+3=7 ounces."),
            new("Sandwich shop used91 total ounces meat and cheese at7oz per sandwich. How many sandwiches?",13,"91/7=13."),
            new("Sandwich shop made13 sandwiches using4 ounces meat each. Meat used in ounces?",52,"13×4=52."),
            new("Sandwich shop made13 sandwiches using3 ounces cheese each. Cheese used in ounces?",39,"13×3=39."),
            new("The modeled building climb is828meters in6hours. Meters per hour?",138,"828/6=138 m/h."),
            new("At modeled138meters/hour, meters climbed in2 hours?",276,"138×2=276m."),
            new("At modeled138meters/hour, meters climbed in5 hours?",690,"138×5=690m."),
            new("Window washing:15 windows in18minutes. Minutes for10 windows at same active pace?",12,"18/15×10=12min."),
            new("Window washing:15 windows per18minutes. Total active washing time for24348 windows in TENTHS OF MINUTES?",292176,"24348×18/15=29217.6 min or292176 tenths.")
        ],
        [L0301RateMeaning] =
        [
            new("An elevator going310feet in10seconds has a modeled constant speed31feet per second. 1=yes,0=no.",1,"310÷10=31."),
            new("An elevator going310feet in10seconds covers372feet in12seconds at the same rate. 1=yes,0=no.",1,"31×12=372."),
            new("Han's pay33dollars in4hours predicts7hours pays57.75dollars at constant rate. 1=yes,0=no.",1,"8.25dollars/hour×7."),
            new("Han's pay33dollars in4hours means11dollars per hour. 1=yes,0=no.",0,"Correct rate8.25."),
            new("Five cans for435cents means 11 cans cost957cents. 1=yes,0=no.",1,"87cents per can."),
            new("Five cans for435cents means 11 cans cost870cents. 1=yes,0=no.",0,"11×87=957cents."),
            new("Four tables in10minutes means26 tables require65minutes at same pace. 1=yes,0=no.",1,"2.5 minutes per table."),
            new("One sandwich with4oz meat and3oz cheese uses7oz total. 1=yes,0=no.",1,"4+3=7oz."),
            new("A modeling assumption that window washing remains constant guarantees real workers never take breaks. 1=yes,0=no.",0,"A simplified prediction cannot guarantee conditions."),
            new("Reciprocal units minutes/window and windows/minute describe related but different rate questions. 1=yes,0=no.",1,"Swap numerator and denominator."),
            new("A mathematical prediction of a building's window-washing time proves the current real window count. 1=yes,0=no.",0,"The window count and rate are historical classroom assumptions."),
            new("A per-one rate is expressed using named units, such as feet per second or dollars per hour. 1=yes,0=no.",1,"Units determine the interpretation.")
        ],
        [L0302UnitMagnitudes] =
        [
            new("A 2-yard piece of string equals how many FEET?",6,"2 yards×3feet/yard=6 feet."),
            new("A 2-yard piece of string equals how many INCHES?",72,"2×36inches=72 inches."),
            new("One yard of string equals how many INCHES?",36,"3 feet×12inches/foot=36."),
            new("One meter equals how many CENTIMETERS?",100,"One meter=100cm."),
            new("One liter equals how many MILLILITERS?",1000,"One liter=1000mL."),
            new("A 200mL volume is what whole-number FIFTH of one liter? Enter numerator when denominator5.",1,"200/1000=1/5."),
            new("A recipe uses5 cups apple juice and2 cups sparkling water per7 total cups. For35 cups total, APPLE cups?",25,"35/7×5=25."),
            new("A recipe uses5 cups apple juice and2 cups sparkling water per7 total cups. For35 cups total, SPARKLING cups?",10,"35/7×2=10."),
            new("Three hats cost2250 cents. Cost per hat in CENTS?",750,"2250/3=750cents."),
            new("Three hats cost2250 cents. Number of hats purchasable with6000 cents at same price?",8,"6000/750=8."),
            new("Light travels180 million km in10minutes in model. Millions of km per MINUTE?",18,"180/10=18 million."),
            new("Light travels180 million km in10minutes in model. Kilometers per SECOND?",300000,"180000000/600=300000 km/s."),
            new("A yard equals3feet. A string of4yards is how many feet?",12,"4×3=12."),
            new("A string is72inches long. How many yards?",2,"72/36=2 yards."),
            new("A milliliter is one THOUSANDTH of a liter. How many milliliters in3liters?",3000,"3×1000=3000mL."),
            new("Source benchmark: two meters of string correspond to how many CENTIMETERS?",200,"2×100=200cm.")
        ],
        [L0302UnitCategories] =
        [
            new("A centimeter is a unit of length, not of volume. 1=yes,0=no.",1,"cm measures length."),
            new("A square centimeter is an appropriate unit of linear length. 1=yes,0=no.",0,"Square cm measures area."),
            new("A cubic centimeter measures volume. 1=yes,0=no.",1,"Three-dimensional extent."),
            new("A kilogram is an appropriate unit of mass for a hippopotamus. 1=yes,0=no.",1,"Large animal mass."),
            new("A millimeter is larger than a meter. 1=yes,0=no.",0,"One meter=1000mm."),
            new("A kilometer is larger than a meter. 1=yes,0=no.",1,"1km=1000m."),
            new("A yard is longer than a foot. 1=yes,0=no.",1,"1yard=3feet."),
            new("A quart is smaller than a cup in customary volume measurement. 1=yes,0=no.",0,"1quart=4cups."),
            new("A pound is larger than an ounce in customary weight measurement. 1=yes,0=no.",1,"1pound=16ounces."),
            new("A liter is larger than a milliliter. 1=yes,0=no.",1,"1liter=1000mL."),
            new("A gram is larger than a kilogram. 1=yes,0=no.",0,"1kg=1000g."),
            new("It is sensible to measure a large lake volume in cubic kilometers. 1=yes,0=no.",1,"Large-scale volume."),
            new("It is sensible to measure a spaghetti noodle length in centimeters. 1=yes,0=no.",1,"Small length."),
            new("A tissue box surface area can be measured in square inches. 1=yes,0=no.",1,"Surface area uses square units.")
        ],
        [L0303SourceAmounts] =
        [
            new("A height of57 inches is approximately how many CENTIMETERS rounded to nearest whole centimeter?",145,"57×2.54=144.78cm≈145cm."),
            new("The original multiple-choice height57in in cm gives options A22.4,B57,C144.8,D3551. Enter the correct OPTION NUMBER 1–4.",3,"C is144.8cm≈57×2.54."),
            new("A pot holding20 US quarts is about how many LITERS rounded to nearest whole liter?",19,"20×0.94635≈18.93L≈19L."),
            new("The original pot20quarts choices A7.57,B19,C21,D75.7 liters. Enter correct OPTION NUMBER 1–4.",2,"B≈19L."),
            new("An object weighing4.5 pounds is about how many kilograms rounded to nearest whole kg?",2,"4.5×0.4536≈2.04kg, about2kg."),
            new("Original choice4.5lb in kg offers A2.04,B4.5,C9.92,D4500. Enter correct OPTION NUMBER 1–4.",1,"A≈2.04kg."),
            new("Noah buys15 baseball cards for900cents. Cost per card in CENTS?",60,"900÷15=60 cents."),
            new("Noah buys15 baseball cards for900cents. Cost of30 cards in CENTS?",1800,"30×60=1800cents."),
            new("Noah buys15 baseball cards for900cents. Cost of12 cards in CENTS?",720,"12×60=720cents."),
            new("Jada travels135miles in3hours. Speed in miles per hour?",45,"135÷3=45mph."),
            new("Andre travels228miles in6hours. Speed in miles per hour?",38,"228÷6=38mph."),
            new("Jada45mph and Andre38mph. Enter1 for Jada faster,2 for Andre faster.",1,"45>38mph."),
            new("Classroom length11 meters: centimeters in that length?",1100,"11×100=1100 centimeters."),
            new("One yard is3 feet. A6-foot rope is how many yards?",2,"6÷3=2 yards."),
            new("If a room is11meters long, will its yard measure be MORE than11? Enter1=yes,0=no.",1,"A yard is shorter than a meter, so it takes more yards."),
            new("A box with a geometric volume of3 liters is how many MILLILITERS?",3000,"3×1000=3000mL.")
        ],
        [L0303AttributeClassification] =
        [
            new("Identify the attribute for centimeters across a handprint. Codes1=length,2=area,3=volume,4=mass. Enter code.",1,"Centimeters measure length."),
            new("Identify attribute for square inches needed to wrap a box. Codes1=length,2=area,3=volume,4=mass.",2,"Square inches measure surface area."),
            new("Identify attribute for gallons in a fish tank. Codes1=length,2=area,3=volume,4=mass.",3,"Gallons measure volume/capacity."),
            new("Identify attribute for pounds in a bag of potatoes. Codes1=length,2=area,3=volume,4=mass.",4,"Pounds measure weight."),
            new("Identify attribute for feet across a swimming pool. Codes1=length,2=area,3=volume,4=mass.",1,"Feet measure length."),
            new("Identify attribute for ounces of grapes as weight. Codes1=length,2=area,3=volume,4=mass.",4,"Ounces as labeled weight measure mass/weight."),
            new("Identify attribute for liters in a punch bowl. Codes1=length,2=area,3=volume,4=mass.",3,"Liters measure volume."),
            new("Identify attribute for square feet of grass in a lawn. Codes1=length,2=area,3=volume,4=mass.",2,"Square feet measure area."),
            new("Identify attribute for cubic centimeters inside a box. Codes1=length,2=area,3=volume,4=mass.",3,"Cubic centimeters measure volume."),
            new("Identify attribute for kilograms of animal mass. Codes1=length,2=area,3=volume,4=mass.",4,"Kilograms measure mass."),
            new("Identify attribute for meters of string. Codes1=length,2=area,3=volume,4=mass.",1,"Meters measure length."),
            new("Identify attribute for square kilometers of land. Codes1=length,2=area,3=volume,4=mass.",2,"Square kilometers measure area.")
        ],
        [L0304Conversions] =
        [
            new("Currency example:4250pesos for250dollars. Pesos per dollar?",17,"4250/250=17pesos per dollar."),
            new("Currency example:4250pesos for250dollars. Sweater510pesos costs how many DOLLARS?",30,"510/17=30dollars."),
            new("Currency example:4250pesos for250dollars. Pesos for3 dollars?",51,"3×17=51pesos."),
            new("One gallon is approximated as3785milliliters. How many mL in3gallons?",11355,"3785×3=11355mL."),
            new("One gallon contains4quarts. How many quarts in3gallons?",12,"4×3=12quarts."),
            new("One gallon approx3785mL. About how many mL in1quart rounded to WHOLE mL?",946,"3785/4=946.25mL, rounded946."),
            new("Six quarts makes how many HALF-GALLONS?",3,"6 quarts=1.5 gallons=3 half-gallons."),
            new("One kilogram weighs approximately35ounces. A28ounce bat weighs how many TENTHS of kilogram?",8,"28/35=0.8kg=8 tenths."),
            new("One kilogram is approximately35ounces. A28ounce bat weighs about how many GRAMS?",800,"0.8kg=800grams under stated approximation."),
            new("Trail mix7oz almonds5oz raisins. Ounces total in one batch?",12,"7+5=12."),
            new("Trail mix7 almonds5 raisins for12 total. In16oz mix, almond quantity in THIRD-OUNCE UNITS?",28,"16×7/12=28/3oz, or28 third-ounce units."),
            new("An ant moves980inches in5minutes. Distance in inches in1minute?",196,"980/5=196inches."),
            new("An ant moves980inches in5minutes. Distance in inches in7minutes?",1372,"196×7=1372inches."),
            new("A quarter cup contains4tablespoons. How many TABLESPOONS in1cup?",16,"4quarters×4tablespoons=16."),
            new("Two cups make1pint. How many TABLESPOONS per PINT?",32,"16×2=32."),
            new("One gallon contains4quarts. How many quarts in6 gallons?",24,"4×6=24quarts.")
        ],
        [L0304DirectionChecks] =
        [
            new("To convert6 quarts to gallons, divide by4. 1=yes,0=no.",1,"6/4=1.5gal."),
            new("To convert6 quarts to gallons, multiply by4 giving24gallons. 1=yes,0=no.",0,"Quarts are smaller than gallons."),
            new("One gallon contains4quarts;3gallons contains12quarts. 1=yes,0=no.",1,"Multiply3×4."),
            new("One quart is approximately946.25mL when one gallon is approximated as3785mL. 1=yes,0=no.",1,"3785/4."),
            new("One kg≈35ounces;28ounces≈0.8kg. 1=yes,0=no.",1,"28/35=0.8."),
            new("One kg≈35ounces;28ounces≈35kg. 1=yes,0=no.",0,"Wrong direction/size."),
            new("A mile is a length unit and a cup is a volume unit. 1=yes,0=no.",1,"Different measurement attributes."),
            new("A gram is a volume unit equivalent to a milliliter for every material. 1=yes,0=no.",0,"Mass/volume depend on density."),
            new("Converting a distance from feet to centimeters changes the physical distance. 1=yes,0=no.",0,"Only the numerical representation changes."),
            new("An ant's980 inches in5 minutes implies1372 inches in7 minutes at constant speed. 1=yes,0=no.",1,"Both quantities scale by7/5."),
            new("A currency rate17 pesos per dollar means510pesos is30dollars. 1=yes,0=no.",1,"510/17=30."),
            new("For trail mix7oz almonds5oz raisins, a16oz bag needs16oz almonds to retain composition. 1=yes,0=no.",0,"Almonds are7/12 of total."),
            new("There are16tablespoons per cup and2cups per pint, so32tablespoons per pint. 1=yes,0=no.",1,"16×2=32."),
            new("A ratio table can help carry measurement units through cross-system conversions. 1=yes,0=no.",1,"Matching labels prevent reciprocal errors.")
        ],
        [L0305Rates] =
        [
            new("Mai scoots15 meters in6 seconds. Speed in TENTHS of a meter per second?",25,"15/6=2.5m/s=25tenths."),
            new("Priya scoots22 meters in10 seconds. Speed in TENTHS of meter per second?",22,"22/10=2.2m/s=22tenths."),
            new("Mai travels15m in6s and Priya22m in10s. Enter1 for Mai faster,2 for Priya faster.",1,"Mai2.5m/s exceeds Priya2.2m/s."),
            new("Store X sells4 same-size juice cans for248cents. Unit price in CENTS?",62,"248/4=62."),
            new("Store Y sells5 same-size juice cans for300cents. Unit price in CENTS?",60,"300/5=60."),
            new("Store Z sells one juice can for59cents. Unit price in CENTS?",59,"Given price59."),
            new("Stores X62c/can,Y60c/can,Z59c/can. Enter1 for X,2 for Y,3 for Z as cheapest.",3,"Z at59cents."),
            new("A 450-square-foot apartment costs540000 dollars. Dollars per SQUARE FOOT?",1200,"540000/450=1200 dollars/sqft."),
            new("A 2100-square-foot home costs110 dollars per square foot. Total cost in DOLLARS?",231000,"2100×110=231000 dollars."),
            new("A liter contains33.8 US fluid ounces and a gallon128 US fluid ounces. Approximately how many WHOLE liters per gallon?",4,"128/33.8≈3.787≈4 liters."),
            new("A unit rate comparison has128 fl oz per gallon and33.8 fl oz per liter. Which listed number (2,3,4,5) estimates liters/gallon best?",4,"128/33.8≈3.79, nearest4."),
            new("Diego is165 centimeters tall. Andre is1.7meters tall. Enter1 for Diego taller,2 for Andre taller.",2,"1.7m=170cm, Andre taller."),
            new("A 1.7-meter height is how many CENTIMETERS?",170,"1.7×100=170cm."),
            new("Two scooter rates15m/6s and22m/10s: difference in TENTHS m/s?",3,"2.5-2.2=0.3m/s=3tenths."),
            new("Store X unit price62c/can and Store Z59c/can. Savings in CENTS per can from choosing Z?",3,"62-59=3cents."),
            new("A home with1200 dollars/sqft for450sqft has total price in DOLLARS?",540000,"1200×450=540000 dollars."),
            new("A unit rate of 110dollars/sqft applies to100sqft. Cost in DOLLARS?",11000,"110×100=11000 dollars.")
        ],
        [L0305Comparisons] =
        [
            new("Mai15m in6s has faster speed than Priya22m in10s. 1=yes,0=no.",1,"2.5m/s >2.2m/s."),
            new("Mai15m in6s is slower than Priya22m in10s because Mai covered less distance. 1=yes,0=no.",0,"Time durations differ; compare per second."),
            new("Store X4cans for248cents means62cents per can. 1=yes,0=no.",1,"248/4=62."),
            new("Store Y5cans for300cents means59cents per can. 1=yes,0=no.",0,"300/5=60."),
            new("Store Z at59cents per can is cheaper than Y at60cents. 1=yes,0=no.",1,"59<60."),
            new("An apartment costing540000 dollars for450sqft has unit price1200 dollars/sqft. 1=yes,0=no.",1,"540000/450=1200."),
            new("A 2100sqft home at110 dollars/sqft costs231000 dollars. 1=yes,0=no.",1,"2100×110=231000."),
            new("Given gallon128fl oz and liter33.8fl oz, rounding to4liters slightly OVERestimates actual liters per gallon. 1=yes,0=no.",1,"128/33.8≈3.787."),
            new("One meter equals100 centimeters, so1.7m is170cm. 1=yes,0=no.",1,"1.7×100=170."),
            new("165cm is taller than1.7m. 1=yes,0=no.",0,"1.7m=170cm."),
            new("Comparing prices of packages of different sizes is best done after finding per-item price. 1=yes,0=no.",1,"Common unit price."),
            new("When comparing two moving rates, distance alone proves who is faster even when times differ. 1=yes,0=no.",0,"Use distance/time for each."),
            new("A price per square foot must include dollars and square-foot units. 1=yes,0=no.",1,"Units explain the quotient.")
        ],
        [L0306Rates] =
        [
            new("Raspberries cost12 dollars for3 pints. Cost per pint in DOLLARS?",4,"12/3=4dollars/pint."),
            new("Raspberries cost12 dollars for3 pints. Number of QUARTER-PINT portions per dollar?",1,"3/12=1/4pint per dollar, one quarter-pint portion."),
            new("Raspberries cost12 dollars for3 pints. Pints purchasable with20 dollars?",5,"20/4=5pints."),
            new("Raspberries cost12 dollars for3 pints. Cost for8 pints in DOLLARS?",32,"8×4=32."),
            new("Pink paint uses4 cups WHITE for3 cups RED. White cups for6 cups red?",8,"Double both."),
            new("Pink paint4 white:3 red. Red cups for8 white?",6,"Double both."),
            new("Pink paint4 white:3 red. For1 cup WHITE, red amount in QUARTER-CUP portions?",3,"3/4cup is3 quarter-cup units."),
            new("Pink paint4 white:3 red. For1 cup RED, white amount in THIRD-CUP portions?",4,"4/3cups white is4 third-cup units."),
            new("Polenta uses5 cups water for2 cups cornmeal. For3 cups water, cornmeal in FIFTH-CUP portions?",6,"3×2/5=6/5cups, six fifth-cup portions."),
            new("Polenta uses5 cups water for2 cups cornmeal. For3 cups cornmeal, water in HALF-CUP portions?",15,"3×5/2=7.5cups, fifteen half-cup portions."),
            new("Paint covers350square feet per gallon. Gallons for1750square feet?",5,"1750/350=5gallons."),
            new("Andre types208 words in4 minutes. Words per minute?",52,"208/4=52."),
            new("Noah types342 words in6 minutes. Words per minute?",57,"342/6=57."),
            new("Andre52words/min and Noah57words/min. Enter1 for Andre faster,2 for Noah faster.",2,"Noah types5words/minute more."),
            new("Eight ears corn cost256 cents. Cost per ear in CENTS?",32,"256/8=32cents."),
            new("Twelve ears corn cost432cents. Cost per ear in CENTS?",36,"432/12=36cents."),
            new("Which corn package is lower cost per ear? Enter1=8ears256cents,2=12ears432cents.",1,"32cents is less than36cents."),
            new("100meters is approximately109yards. Which original choice A33.3 B91 C100 D109 is nearest? Enter OPTION NUMBER.",4,"D≈109yards.")
        ],
        [L0306InterpretRate] =
        [
            new("At3pints for12 dollars, the price is4 dollars per pint. 1=yes,0=no.",1,"12/3=4."),
            new("At3pints for12 dollars, each dollar buys4 pints. 1=yes,0=no.",0,"One dollar buys1/4pint."),
            new("The reciprocal unit rate of4 dollars/pint is1/4 pint per dollar. 1=yes,0=no.",1,"Reciprocal when both quantities positive."),
            new("At3pints for12 dollars,20 dollars buys5 pints. 1=yes,0=no.",1,"20/4=5."),
            new("For polenta5 water:2 cornmeal,3cups water needs1.2cups cornmeal. 1=yes,0=no.",1,"3×2/5=1.2."),
            new("For polenta5 water:2 cornmeal,3cups cornmeal needs7.5cups water. 1=yes,0=no.",1,"3×5/2=7.5."),
            new("For paint350sqft per gallon,1750sqft requires5gallons. 1=yes,0=no.",1,"1750/350=5."),
            new("For paint350sqft per gallon, each square foot uses350 gallons. 1=yes,0=no.",0,"Each square foot uses1/350 gallon."),
            new("Typing Andre208/4 gives52wpm and Noah342/6 gives57wpm. Noah is faster. 1=yes,0=no.",1,"57>52."),
            new("Corn8 for256cents is a better unit price than12 for432cents. 1=yes,0=no.",1,"32cents per ear <36."),
            new("A rate a/b answers amount of first quantity per one second quantity. 1=yes,0=no.",1,"Units determine interpretation."),
            new("The reciprocal b/a gives a different unit interpretation of the same nonzero quantity relationship. 1=yes,0=no.",1,"E.g. dollars/pint and pints/dollar."),
            new("A rate per one should be reported without units because only the numeral matters. 1=yes,0=no.",0,"Units are essential.")
        ],
        [L0307UnitRateValues] =
        [
            new("A car travels55 miles per hour. Distance in miles after2 hours?",110,"55×2=110 miles."),
            new("A car travels55mph. Distance in HALF-MILE UNITS after half an hour?",55,"0.5h×55=27.5miles=55 half-miles."),
            new("A car travels55mph. Distance in HALF-MILE UNITS after1.5 hours?",165,"1.5×55=82.5miles=165 half-miles."),
            new("Salsa recipe:2oz onions for16oz tomatoes. Tomatoes ounces per1 onion ounce?",8,"16/2=8."),
            new("Salsa recipe:6oz onions for48oz tomatoes. Tomatoes per onion ounce?",8,"48/6=8."),
            new("Ten burritos for12 dollars. Price per burrito in CENTS?",120,"1200/10=120cents."),
            new("Six burritos for7.50 dollars. Price per burrito in CENTS?",125,"750/6=125cents."),
            new("Party needs60 burritos. Cost of6 ten-burrito specials in DOLLARS?",72,"6×12=72 dollars."),
            new("Party needs60 burritos. Cost of10 six-burrito specials in DOLLARS?",75,"10×7.50=75 dollars."),
            new("A banana costs50 cents. Price for4 bananas in CENTS?",200,"50×4=200 cents."),
            new("A banana costs50 cents. Price for6 bananas in CENTS?",300,"50×6=300 cents."),
            new("A banana costs50 cents. Price for7 bananas in CENTS?",350,"50×7=350 cents."),
            new("A banana costs50 cents. Price for10 bananas in CENTS?",500,"50×10=500 cents."),
            new("Bananas cost50 cents each. Number purchasable for10 dollars?",20,"1000cents/50=20 bananas."),
            new("Bananas cost50 cents each. Number purchasable for16.50 dollars?",33,"1650cents/50=33."),
            new("Plane A travels2800 miles in5 hours. Miles per hour?",560,"2800/5=560."),
            new("Plane B travels3885 miles in7 hours. Miles per hour?",555,"3885/7=555."),
            new("A vehicle has15 gallons gas at35 miles/gallon. Total range in miles?",525,"15×35=525 miles."),
            new("Gas used for100 miles at35miles/gallon, expressed in SEVENTHS OF A GALLON: how many sevenths?",20,"100/35=20/7 gallons."),
            new("Cereal weighs600grams,1kg≈2.2lb. Weight in HUNDREDTHS OF A POUND?",132,"600g=0.6kg≈1.32lb=132hundredths.")
        ],
        [L0307RateInvariance] =
        [
            new("Onions2oz tomatoes16oz and onions6oz tomatoes48oz have the same tomatoes per onion ounce. 1=yes,0=no.",1,"Both8 tomato ounces per1 onion ounce."),
            new("Onions2oz tomatoes16oz and onions6oz tomatoes40oz have the same rate. 1=yes,0=no.",0,"40/6≠8."),
            new("Car55miles per hour travels110miles in2hours. 1=yes,0=no.",1,"Both same unit rate55mph."),
            new("Car55mph travels100miles in2hours at that rate. 1=yes,0=no.",0,"It travels110."),
            new("10 burritos/$12 has a lower price per burrito than6 burritos/$7.50. 1=yes,0=no.",1,"1.20<1.25 per burrito."),
            new("Six orders of10 burritos for12dollars each cost less than ten orders of6 for7.50dollars each. 1=yes,0=no.",1,"72<75dollars."),
            new("A banana at50cents each,4 for200cents and7 for350cents are equivalent ratios. 1=yes,0=no.",1,"Same50cents per banana."),
            new("A banana at50cents each,4 for200cents and7 for300cents are equivalent ratios. 1=yes,0=no.",0,"300/7 differs from50."),
            new("Plane A2800mi/5h=560mph and plane B3885mi/7h=555mph, so A is faster. 1=yes,0=no.",1,"560>555."),
            new("Plane B3885mi/7h is faster than A2800mi/5h because total distance is longer. 1=yes,0=no.",0,"Compare speed not total distance."),
            new("35miles per gallon and1/35 gallon per mile are reciprocal rates of a modeled constant fuel efficiency. 1=yes,0=no.",1,"Units reverse."),
            new("At35miles/gallon,15gallons is525 miles of modeled range. 1=yes,0=no.",1,"15×35=525."),
            new("If two pairs represent equivalent ratios, their unit rates must match for the same quantity order and units. 1=yes,0=no.",1,"Equal quotient."),
            new("A table's unit-rate column should change if quantities scale by the same positive factor. 1=yes,0=no.",0,"Unit rate is invariant.")
        ],
        [L0308SpeedAmounts] =
        [
            new("Kangaroo hops2km in3minutes. Time for5km at constant speed in SECONDS?",450,"5×3/2=7.5min=450seconds."),
            new("Kangaroo hops2km in3minutes. Distance in2minutes in THIRD-KILOMETER units?",4,"2×(2/3)=4/3km, four thirds."),
            new("Kangaroo hops2km in3minutes. Time in MINUTES to travel4km?",6,"Twice the distance, twice the time."),
            new("Mai runs4 laps of400m track. Total distance in METERS?",1600,"4×400=1600m."),
            new("Mai runs250meters/minute. Time for4 laps totaling1600meters, in TENTHS OF MINUTE?",64,"1600/250=6.4minutes=64tenths."),
            new("Mai runs250m/min. Time for4 laps of400m each in SECONDS?",384,"1600/250×60=384seconds."),
            new("Han runs at12min/mile. Distance in HALF-MILE units after30minutes?",5,"30/12=2.5miles=five half-miles."),
            new("Han runs at12min/mile. Distance in MILES after60minutes?",5,"60/12=5miles."),
            new("Tyler runs at15min/mile. Distance in MILES after60minutes?",4,"60/15=4miles."),
            new("Han and Tyler start opposite ends of10miles: Han5mi and Tyler4mi after one hour. Gap in MILES after1hour?",1,"10-(5+4)=1mile."),
            new("Han12min/mile and Tyler15min/mile start10mi apart opposite. Time to meet in SECONDS assuming constant speeds?",4000,"Their closing rate=1/12+1/15=3/20mi/min,10/(3/20)=200/3min=4000sec."),
            new("Skateboarder A moves15ft/s, B22ft/s. Speed DIFFERENCE feet per second?",7,"22-15=7ft/s."),
            new("Skateboarder B22ft/s vs A15ft/s over4minutes. Distance difference in FEET?",1680,"4min=240s;7×240=1680ft."),
            new("Four tablespoons equal1/4cup. Tablespoons in1 cup?",16,"4×4=16tbsp."),
            new("Four tbsp equal1/4cup,2cups per pint. Tablespoons in1PINT?",32,"16×2=32tbsp."),
            new("Cube A side2 units, cube B side4 units. Surface area of A in SQUARE UNITS?",24,"6×2²=24."),
            new("Cube B side4 units. Surface area in SQUARE UNITS?",96,"6×4²=96."),
            new("Cube A side2 units, cube B side4 units. B-to-A volume multiplication FACTOR?",8,"4³/2³=64/8=8.")
        ],
        [L0308PaceMeaning] =
        [
            new("A kangaroo hopping2km in3min needs7.5min for5km at constant speed. 1=yes,0=no.",1,"Multiply3min by5/2."),
            new("A kangaroo hopping2km in3min covers5km in5min at same speed. 1=yes,0=no.",0,"It takes7.5min."),
            new("Mai at250m/min completes4×400m laps in6.4min. 1=yes,0=no.",1,"1600/250=6.4min."),
            new("Han12min/mile runs faster than Tyler15min/mile. 1=yes,0=no.",1,"Fewer minutes per mile means faster."),
            new("Han12min/mile and Tyler15min/mile run toward each other10mi apart, they meet within1hour. 1=yes,0=no.",0,"They cover5+4=9miles in an hour."),
            new("Han12min/mile and Tyler15min/mile cover a combined9miles in1hour. 1=yes,0=no.",1,"5+4=9."),
            new("Two skateboarders22ft/s and15ft/s differ by1680feet after4minutes. 1=yes,0=no.",1,"240sec×7ft/s."),
            new("A 4minute period contains4 seconds. 1=yes,0=no.",0,"Four minutes=240 seconds."),
            new("A cube of side4 has double the surface area of cube side2. 1=yes,0=no.",0,"Surface area scales by4."),
            new("A cube of side4 has eight times the volume of a cube side2. 1=yes,0=no.",1,"Volume scales by2 cubed=8."),
            new("A pace measured in minutes per mile is the reciprocal of speed in miles per minute for a constant positive rate. 1=yes,0=no.",1,"Reciprocal quantities/units."),
            new("Time to travel a fixed distance at constant speed halves if speed doubles. 1=yes,0=no.",1,"time=distance/speed."),
            new("On a constant-speed table, doubling time doubles distance traveled from the same origin. 1=yes,0=no.",1,"Proportional movement.")
        ],
        [L0309RateValues] =
        [
            new("Copier prints480 pages in4minutes. Copies per MINUTE?",120,"480/4=120 copies/minute."),
            new("Copier prints480 pages in4minutes. Copies in10minutes?",1200,"120×10=1200copies."),
            new("Copier prints480 pages in4minutes. Minutes to print720copies?",6,"720/120=6minutes."),
            new("School bus mass9tons. Pounds assuming2000lb per ton?",18000,"9×2000=18000lbs."),
            new("Elephant weighs5500kilograms. Approx pounds using2.2lbs/kg?",12100,"5500×2.2=12100lbs."),
            new("Grand piano weighs15840ounces. Pounds given16ounces/lb?",990,"15840/16=990lbs."),
            new("Horse weighs1100lbs. Equivalent ounces at16oz/lb?",17600,"1100×16=17600ounces."),
            new("Rank from heaviest to lightest bus9tons, elephant5500kg, horse1100lb, piano15840oz. Enter1 for bus,2 elephant,3 horse,4 piano as HEAVIEST.",1,"School bus18000lb is heaviest."),
            new("A worker earns10dollars for half hour. Dollars per hour?",20,"10/0.5=20dollars/hour."),
            new("A worker earns30dollars for1.5hours. Dollars per hour?",20,"30/1.5=20."),
            new("A worker earns30dollars for2hours. Dollars per hour?",15,"30/2=15."),
            new("A worker earns20dollars per hour. Pay in DOLLARS for3hours?",60,"20×3=60."),
            new("Compute one-half times17, expressed in TENTHS of a unit.",85,"17/2=8.5, or85tenths."),
            new("Compute three-quarters of200.",150,"200×3/4=150."),
            new("Compute0.2 times40.",8,"0.2×40=8."),
            new("Compute0.25 times60.",15,"0.25×60=15."),
            new("A fast printer makes120 copies per minute. Time for360 copies in MINUTES?",3,"360/120=3."),
            new("At20 dollars/hour, pay for half an hour in DOLLARS?",10,"20×1/2=10.")
        ],
        [L0309ComparisonLogic] =
        [
            new("A machine prints480copies in4minutes or120copies/minute. 1=yes,0=no.",1,"480÷4=120."),
            new("The copier at120copies/min prints720copies in6min. 1=yes,0=no.",1,"720/120=6."),
            new("The bus9tons weighs18000pounds under2000lb/ton. 1=yes,0=no.",1,"9×2000=18000."),
            new("The elephant5500kg weighs about12100lb using2.2lb/kg. 1=yes,0=no.",1,"Approximate conversion."),
            new("The piano15840oz weighs more than a horse1100lb. 1=yes,0=no.",0,"Piano990lb, horse1100lb."),
            new("Job A half hour10dollars and job B1.5hours30dollars have equal hourly pay. 1=yes,0=no.",1,"Both20dollars/hour."),
            new("A2hour job for30dollars pays more per hour than half hour for10dollars. 1=yes,0=no.",0,"15dollars/hour vs20."),
            new("The three jobs 10dollars/half hour,30/1.5h,30/2h all pay the same rate. 1=yes,0=no.",0,"Third pays less."),
            new("Unit-rate reasoning may require converting measurements to the same unit before comparing. 1=yes,0=no.",1,"Incompatible unit comparisons mislead."),
            new("A whole-package price is enough to determine per-slice cheese price without knowing slice count. 1=yes,0=no.",0,"The source image count must be known."),
            new("The heart-shaped polygon's exact area can be confirmed from an unlabeled text summary alone without inspecting the drawing. 1=yes,0=no.",0,"Teacher/source image dimensions matter."),
            new("An exact arithmetic result for a source problem does not automatically grade a student's open-ended visual justification. 1=yes,0=no.",1,"Human review remains needed."),
            new("A percentage can be interpreted as a rate per100 in next lessons. 1=yes,0=no.",1,"100-based ratio.")
        ],
        [L0310PercentNumbers] =
        [
            new("Four US dimes are40 cents. Percent of one dollar?",40,"40/100=40%."),
            new("One nickel and three pennies total8 cents. Percent of one dollar?",8,"5+3=8cents=8%."),
            new("Five quarters and one dime total135cents. Percent of one dollar?",135,"5×25+10=135cents=135%."),
            new("Three dimes total30cents. Percent of one dollar?",30,"30%."),
            new("One quarter plus one nickel total30cents. Percent of one dollar?",30,"25+5=30%."),
            new("Five quarters, one dime and one nickel total140cents. Percent of one dollar?",140,"125+10+5=140%."),
            new("A historical half-cent coin is what TENTHS of a PERCENT of a dollar?",5,"0.5cent/100cents=0.5%=5tenths of a percent."),
            new("A 3-cent coin is what percent of one dollar?",3,"3cents=3%."),
            new("A 20-cent coin is what percent of one dollar?",20,"20%."),
            new("A 2.5-dollar historical coin is what percent of a dollar?",250,"2.5×100%=250%."),
            new("A 5-dollar coin is what percent of a dollar?",500,"5×100%=500%."),
            new("Elena buys8 tokens for440cents. Cents per token?",55,"440/8=55cents."),
            new("Elena buys8 tokens for440cents. Tokens purchasable with605cents?",11,"605/55=11."),
            new("Elena buys8 tokens for440cents. Cost of19 tokens in CENTS?",1045,"19×55=1045cents."),
            new("Snail travels10cm in4minutes. Seconds needed for24cm at constant speed?",576,"24×4/10=9.6min=576sec."),
            new("Snail travels10cm in4minutes. Cm in6minutes?",15,"10/4×6=15cm."),
            new("Three tacos cost18 dollars. Dollars per taco?",6,"18/3=6."),
            new("Three tacos cost18 dollars. Total DOLLARS for4 tacos?",24,"6×4=24."),
            new("Three tacos cost18 dollars. Total DOLLARS for5 tacos?",30,"6×5=30."),
            new("Three tacos cost18 dollars. Total DOLLARS for6 tacos?",36,"6×6=36.")
        ],
        [L0310PercentLogic] =
        [
            new("40cents is40% of one dollar. 1=yes,0=no.",1,"One dollar=100cents."),
            new("135 cents is135% of one dollar. 1=yes,0=no.",1,"Percentage can exceed100 when comparing quantities."),
            new("A percentage can never be more than100% for any type of comparison. 1=yes,0=no.",0,"Comparison quantities can exceed the reference."),
            new("If 25% of a water bottle is consumed, the consumed portion cannot exceed the whole bottle in that part-whole context. 1=yes,0=no.",1,"Part of the reference whole."),
            new("Two and a half dollars equals250% of a dollar. 1=yes,0=no.",1,"2.5 times the reference."),
            new("A 5dollar amount equals5% of a dollar. 1=yes,0=no.",0,"It equals500%."),
            new("Four dimes and one quarter total65% of a dollar. 1=yes,0=no.",1,"40+25=65cents."),
            new("Two quarters equal50% of one dollar. 1=yes,0=no.",1,"50/100=50%."),
            new("A 30-cent coin collection is30% of a dollar. 1=yes,0=no.",1,"30 cents per100cents."),
            new("Percent means per hundred, so100% represents the full reference amount. 1=yes,0=no.",1,"Rate per100."),
            new("At55 cents per token, 11tokens cost605cents. 1=yes,0=no.",1,"11×55=605."),
            new("A snail10cm in4min travels24cm in6min at the same rate. 1=yes,0=no.",0,"6min yields15cm."),
            new("Three tacos for18 dollars gives each taco6dollars. 1=yes,0=no.",1,"18/3=6.")
        ],
        [L0311PercentValues] =
        [
            new("Mai attempted40 free throws and made25%. Successful throws?",10,"40×25/100=10."),
            new("Priya previously made12 free throws and today made150% as many. Successful throws today?",18,"12×1.5=18."),
            new("200mg vitamin C is250% of daily recommended amount. Daily100% in MILLIGRAMS?",80,"200/2.5=80mg."),
            new("At school100 students represent40% who prefer hip hop. Total sixth graders?",250,"100/0.4=250."),
            new("20% of150 apples is how many apples?",30,"150×0.2=30."),
            new("75% of80 students is how many?",60,"80×3/4=60."),
            new("The scooter traveled1020inches in5seconds. Distance in TENTHS OF A CENTIMETER?",25908,"1020×2.54=2590.8cm=25908tenths."),
            new("Skateboard traveled90feet in5seconds. Distance in TENTHS OF A CENTIMETER?",27432,"90×30.48=2743.2cm=27432tenths."),
            new("Bike traveled4800centimeters in5seconds. Distance in CENTIMETERS?",4800,"Given4800cm."),
            new("Go-cart traveled0.03kilometers in5seconds. Distance in CENTIMETERS?",3000,"0.03km=30meters=3000cm."),
            new("Fastest of skateboard90ft, scooter1020in, bike4800cm, go-cart0.03km in same5s. Enter1=skateboard,2=scooter,3=bike,4=go-cart.",3,"Bike4800cm in5s is farthest."),
            new("A batch uses10lb potatoes for15lb mashed potatoes. Mash in HALF-POUND units from15lb raw potatoes?",45,"15×1.5=22.5lb=45half-pound units."),
            new("A recipe uses10lb potatoes to make15lb mash. Raw pounds needed for50lb mash in THIRD-POUND units?",100,"50×10/15=100/3lb=100 third-pound units."),
            new("A medium bottle has320mL reference,150% of reference is how many mL?",480,"320×1.5=480mL."),
            new("Forty free throws correspond to100%. Number corresponding to50%?",20,"Half of40=20."),
            new("Twelve successful throws represent100%. Number at125%?",15,"12×1.25=15."),
            new("200mg is250% of reference. Quantity for50% of reference, in mg?",40,"Reference80mg, half=40.")
        ],
        [L0311PercentReasoning] =
        [
            new("25% of40 attempts is10 successful throws. 1=yes,0=no.",1,"40/4=10."),
            new("150% of12 is18, which is larger than the reference12. 1=yes,0=no.",1,"150% is one and a half times."),
            new("150% of12 must be less than12 because percentages cannot exceed100. 1=yes,0=no.",0,"Comparison percentages may exceed100."),
            new("200mg as250% of allowance means allowance80mg. 1=yes,0=no.",1,"200/2.5=80."),
            new("If100 pupils are40% of a group, the whole group consists of250 pupils. 1=yes,0=no.",1,"100/0.4=250."),
            new("If100 pupils are40% of a group, total pupils=140. 1=yes,0=no.",0,"40% of140=56."),
            new("On a percent double number line the100% reference aligns with the entire reference quantity. 1=yes,0=no.",1,"Corresponding marks."),
            new("Skateboard90ft equals2743.2cm using30.48cm/foot. 1=yes,0=no.",1,"90×30.48."),
            new("Go-cart0.03km equals3000cm. 1=yes,0=no.",1,"0.03km=30m."),
            new("Bike4800cm in5seconds is faster than go-cart3000cm in5seconds. 1=yes,0=no.",1,"Same time, greater distance."),
            new("Potatoes10lbs produce15lbs mash, so15lbs potatoes make22.5lbs mash at constant rate. 1=yes,0=no.",1,"Both×1.5."),
            new("Percentage comparisons should identify which amount is100%. 1=yes,0=no.",1,"Without reference the percentage is ambiguous."),
            new("A 200mg quantity can be250% of a smaller80mg reference. 1=yes,0=no.",1,"200/80=2.5.")
        ],
        [L0312TapePercent] =
        [
            new("Bakery has40 muffin flavors;25% contain chocolate. Number with chocolate?",10,"40×1/4=10."),
            new("Bakery has40 flavors;25% chocolate. Number without chocolate?",30,"40-10=30."),
            new("School band70 students:40% sixth graders. Count sixth graders?",28,"70×0.4=28."),
            new("School band70 students:20% seventh graders. Count seventh graders?",14,"70×0.2=14."),
            new("Band70 with40% sixth and20% seventh. Percentage eighth graders?",40,"100-40-20=40%."),
            new("Band70 with40% sixth and20% seventh. Count eighth graders?",28,"70×40%=28."),
            new("Jada spent60dollars, which is120% of budget. Monthly budget in DOLLARS?",50,"60/1.2=50."),
            new("Jada budget50dollars. Spending120% of budget in DOLLARS?",60,"50×1.2=60."),
            new("Five tickets cost12dollars50cents. Price per ticket in CENTS?",250,"1250/5=250cents."),
            new("Eight tickets cost20dollars16cents. Price per ticket in CENTS?",252,"2016/8=252cents."),
            new("Buying5 tickets for12.50 dollars or8 for20.16 dollars, which is cheaper per ticket? Enter1 for five-pack,2 for eight-pack.",1,"250cents vs252cents."),
            new("An athlete runs8miles in50minutes. Time for9miles in SECONDS at same rate?",3375,"9×50/8=56.25min=3375sec."),
            new("An athlete runs8miles in50minutes. Distance traveled in60minutes in TENTHS OF A MILE?",96,"8×60/50=9.6miles=96tenths."),
            new("Under a tape model, one quarter of80 students is how many?",20,"80/4=20."),
            new("Under a tape model, three quarters of80 students is how many?",60,"3×20=60."),
            new("A bill of60dollars is120% of a monthly50dollar budget. Amount OVER budget in DOLLARS?",10,"60-50=10."),
            new("Forty muffin flavors with ten chocolate flavors: percent chocolate?",25,"10/40×100%=25.")
        ],
        [L0312TapeStructure] =
        [
            new("25% of40 muffin flavors is10 flavors. 1=yes,0=no.",1,"One of four equal quarters."),
            new("25% of40 muffin flavors is25 flavors. 1=yes,0=no.",0,"It is10."),
            new("Band40% sixth and20% seventh leaves40% eighth. 1=yes,0=no.",1,"100-40-20=40%."),
            new("Band70 members with40% sixth has28 sixth graders. 1=yes,0=no.",1,"0.4×70=28."),
            new("Band70 members with20% seventh has20 seventh graders. 1=yes,0=no.",0,"20% of70=14."),
            new("A tape with four equal blocks can model25% as one block. 1=yes,0=no.",1,"One-quarter equals25% of amount."),
            new("Jada60dollars spent is120% of a50dollar budget. 1=yes,0=no.",1,"60/50=120%."),
            new("If60dollars is120% of budget, budget is72dollars. 1=yes,0=no.",0,"Budget is50."),
            new("Five tickets cost1250cents, so250cents per ticket. 1=yes,0=no.",1,"1250/5=250."),
            new("Eight tickets cost2016cents, so252cents per ticket. 1=yes,0=no.",1,"2016/8=252."),
            new("One can determine exact walking percentages from an original image without knowing the picture's measured quantities. 1=yes,0=no.",0,"Diagram data needed."),
            new("Tapes can represent both percentages below100 and comparisons larger than100 by extending beyond the reference whole. 1=yes,0=no.",1,"Extend tape beyond100%."),
            new("At8miles per50minutes,9miles takes56.25minutes. 1=yes,0=no.",1,"50×9/8=56.25.")
        ],
        [L0313BenchmarkNumbers] =
        [
            new("Andre lives1.6km from school. Fifty percent of that distance in METERS?",800,"Half of1.6km=0.8km=800meters."),
            new("Diego lives half a mile from school. Fifty percent of that in QUARTER-MILE UNITS?",1,"Half of1/2mile is1/4mile."),
            new("Diego lives half a mile from school. Fifty percent in YARDS?",440,"One quarter mile=440yards."),
            new("Laptop sale saves35 dollars, which is10% of original price. Original price in DOLLARS?",350,"35/0.10=350dollars."),
            new("If15 is out of30, what PERCENT is15 of30?",50,"15/30×100=50%."),
            new("If3 is out of12, what PERCENT is3 of12?",25,"3/12×100=25%."),
            new("If6 is out of10, what PERCENT is6 of10?",60,"6/10×100=60%."),
            new("20% of60 is how many?",12,"One fifth of60=12."),
            new("20% of125 is how many?",25,"125/5=25."),
            new("Diego has75% of10dollars. Amount in CENTS?",750,"0.75×1000=750cents."),
            new("Noah has25% of30dollars. Amount in CENTS?",750,"0.25×3000=750cents."),
            new("Nine is what percent of36?",25,"9/36=1/4=25%."),
            new("Nine is what percent of18?",50,"9/18=1/2=50%."),
            new("Nine is what percent of90?",10,"9/90=1/10=10%."),
            new("Nine is what percent of12?",75,"9/12=3/4=75%."),
            new("Lin walks2.5miles/hour, Andre3miles/hour from opposite ends22mile trail. Meeting time in HOURS?",4,"22/(2.5+3)=4 hours."),
            new("Lin walks2.5mph, Andre3mph toward each other22mi apart. Combined miles covered after2hours?",11,"(2.5+3)×2=11miles."),
            new("One hour has60minutes. Twenty-five percent of hour in MINUTES?",15,"60/4=15minutes."),
            new("One hour has60minutes. Seventy-five percent of hour in MINUTES?",45,"3×15=45minutes.")
        ],
        [L0313BenchmarkLogic] =
        [
            new("50% of any positive quantity is one-half of that quantity. 1=yes,0=no.",1,"50/100=1/2."),
            new("25% of a quantity is one-fourth of it. 1=yes,0=no.",1,"25/100=1/4."),
            new("10% of a quantity is one-tenth of it. 1=yes,0=no.",1,"10/100=1/10."),
            new("75% of a quantity is three-fourths of it. 1=yes,0=no.",1,"75/100=3/4."),
            new("For any real number,20% of it equals that number divided by5. 1=yes,0=no.",1,"20/100=1/5."),
            new("Twenty percent of60 is20, because percent means subtract20. 1=yes,0=no.",0,"20%=12."),
            new("Saving35 dollars at10% discount means original price350 dollars. 1=yes,0=no.",1,"35×10=350."),
            new("Fifteen is50% of30. 1=yes,0=no.",1,"15 is half30."),
            new("Three is25% of12. 1=yes,0=no.",1,"3 is quarter12."),
            new("Six is60% of10. 1=yes,0=no.",1,"6/10=60%."),
            new("75% of10dollars and25% of30dollars are equal. 1=yes,0=no.",1,"Both7.50dollars."),
            new("Lin2.5mph and Andre3mph start22miles apart and move toward each other, meeting after4hours. 1=yes,0=no.",1,"Combined speed5.5mph."),
            new("A 10% reduction of350dollars saves35 dollars. 1=yes,0=no.",1,"350×0.1=35.")
        ],
        [L0314PercentComputations] =
        [
            new("160 is what percentage of40?",400,"160/40×100=400%."),
            new("40 is160% of what number?",25,"40/1.6=25."),
            new("What number is40% of160?",64,"0.4×160=64."),
            new("An item is20% off and Mai saves13 dollars. Original price in DOLLARS?",65,"13/0.2=65."),
            new("A scarf originally costs16 dollars and a shopper saves12 dollars. Percent discount?",75,"12/16×100=75%."),
            new("A scarf originally costs16 dollars and shopper saves12. Sale price in DOLLARS?",4,"16-12=4."),
            new("Evaluate120% of100.",120,"1.2×100=120."),
            new("Evaluate50% of150.",75,"0.5×150=75."),
            new("Evaluate150% of50.",75,"1.5×50=75."),
            new("Evaluate20% of800.",160,"0.2×800=160."),
            new("Evaluate200% of30.",60,"2×30=60."),
            new("Evaluate500% of400.",2000,"5×400=2000."),
            new("Evaluate1% of1000.",10,"0.01×1000=10."),
            new("An ant travels30cm in2minutes. Speed in centimeters per minute?",15,"30/2=15cm/min."),
            new("An ant travels30cm in2minutes. SECONDS per centimeter?",4,"120sec/30cm=4sec/cm."),
            new("Three and a half US cups at236.6mL per cup: total in TENTHS OF MILLILITER?",8281,"3.5×236.6=828.1mL."),
            new("One meter is approximately100cm. 75% of a meter in CENTIMETERS?",75,"0.75×100=75cm."),
            new("A coupon saves15 dollars on an original75-dollar price. What discount percent?",20,"15/75×100=20%.")
        ],
        [L0314PercentInterpretation] =
        [
            new("160 is400% of40. Enter1=yes,0=no.",1,"Four times the reference."),
            new("40 is160% of25. Enter1=yes,0=no.",1,"1.6×25=40."),
            new("40% of160 is64. Enter1=yes,0=no.",1,"0.4×160=64."),
            new("A20% sale saving13 dollars implies original65 dollars. Enter1=yes,0=no.",1,"13/0.2=65."),
            new("A12-dollar saving from16 dollars is a75% discount. Enter1=yes,0=no.",1,"12/16=0.75."),
            new("120% of100 exceeds100. Enter1=yes,0=no.",1,"120>100."),
            new("50% of150 exceeds100. Enter1=yes,0=no.",0,"75<100."),
            new("150% of50 exceeds100. Enter1=yes,0=no.",0,"75<100."),
            new("20% of800 exceeds100. Enter1=yes,0=no.",1,"160>100."),
            new("200% of30 exceeds100. Enter1=yes,0=no.",0,"60<100."),
            new("500% of400 exceeds100. Enter1=yes,0=no.",1,"2000>100."),
            new("1% of1000 exceeds100. Enter1=yes,0=no.",0,"10<100."),
            new("3.5 cups at236.6mL each contains less than1liter. Enter1=yes,0=no.",1,"828.1mL<1000mL."),
            new("An ant that travels30cm in2minutes travels15cm per minute. Enter1=yes,0=no.",1,"30/2=15."),
            new("Solving an information-gap percentage problem requires identifying the100% reference. Enter1=yes,0=no.",1,"Percent comparisons require a known or inferred whole."),
            new("A discount of20% is always20 dollars regardless of original price. Enter1=yes,0=no.",0,"Discount depends on the reference price.")
        ],
        [L0315PercentAmounts] =
        [
            new("Forty percent of75?",30,"0.4×75=30."),
            new("Priya practices152% as long as Han's75 minutes. How many minutes?",114,"1.52×75=114."),
            new("Tyler practices64% as long as Han's75 minutes. How many minutes?",48,"0.64×75=48."),
            new("Amusement park has1575 visitors.56% adults. How many adults?",882,"1575×0.56=882."),
            new("Amusement park has1575 visitors.16% teens. How many teenagers?",252,"1575×0.16=252."),
            new("Amusement park has1575 visitors.28% children. How many children?",441,"1575×0.28=441."),
            new("Find55% of180.",99,"180×0.55=99."),
            new("Find300% of26.",78,"26×3=78."),
            new("Find12% of700.",84,"700×0.12=84."),
            new("Find20% of60.",12,"60÷5=12."),
            new("25% of which number equals6?",24,"6÷0.25=24."),
            new("What percent of100 is14?",14,"14/100=14%."),
            new("Find50% of90.",45,"90÷2=45."),
            new("10% of which number is7?",70,"7÷0.1=70."),
            new("Find30% of70.",21,"70×0.3=21."),
            new("Six sandwich rolls cost528 cents. Per roll price in CENTS?",88,"528/6=88."),
            new("Four sandwich rolls cost340 cents. Per roll price in CENTS?",85,"340/4=85."),
            new("For24 rolls four six-packs at528cents each cost how many CENTS?",2112,"4×528=2112."),
            new("For24 rolls six four-packs at340cents each cost how many CENTS?",2040,"6×340=2040."),
            new("Trip chaperone:student ratio3:20,92 total people. How many chaperones?",12,"92÷23=4 parts groups;3×4=12."),
            new("Trip chaperone:student ratio3:20,92 total. How many students?",80,"20×4=80.")
        ],
        [L0315PercentProperties] =
        [
            new("Two fifths of75 equals40% of75. 1=yes,0=no.",1,"2/5=0.4."),
            new("Two fifths of any number equals40% of that number. 1=yes,0=no.",1,"Equivalent factors."),
            new("152% of75 minutes is114minutes. 1=yes,0=no.",1,"75×1.52=114."),
            new("64% of75 minutes is64minutes. 1=yes,0=no.",0,"It is48min."),
            new("Amusement park categories56%,16%,28% sum to100%. 1=yes,0=no.",1,"56+16+28=100."),
            new("Of1575 people56% adults equals882. 1=yes,0=no.",1,"1575×0.56."),
            new("55% of180 is greater than300% of26. 1=yes,0=no.",1,"99>78."),
            new("12% of700 is greater than55% of180. 1=yes,0=no.",0,"84<99."),
            new("300% of26 equals78. 1=yes,0=no.",1,"Triple26."),
            new("A six-pack of rolls for528cents is a better unit price than four-pack for340cents. 1=yes,0=no.",0,"88c versus85c each."),
            new("Four six-packs cost2112cents and six four-packs cost2040cents for24 rolls. 1=yes,0=no.",1,"Four-packs save72 cents."),
            new("A 3:20 chaperone:student ratio with92 total people gives12 chaperones. 1=yes,0=no.",1,"23 parts, each4."),
            new("A percent multiplication factor over1 can produce a value greater than the reference. 1=yes,0=no.",1,"Such as152%."),
            new("When finding A% of B, multiplying B by A/100 gives the amount. 1=yes,0=no.",1,"Definition of percent per100.")
        ],
        [L0316FindPercent] =
        [
            new("A ride minimum height is40 inches.34in is what percent of40in?",85,"34/40×100=85%."),
            new("A ride minimum height is40 inches.54in is what percent of40in?",135,"54/40×100=135%."),
            new("Tool normally costs80dollars, savings12dollars. Savings percent?",15,"12/80×100=15%."),
            new("A tub capacity80gallons and fills4gal/minute for6minutes. Gallons filled?",24,"4×6=24."),
            new("A tub capacity80gallons and fills4gal/minute for6minutes. Percent filled?",30,"24/80×100=30%."),
            new("Backpack30dollars, sale price85% of usual. Sale price in CENTS?",2550,"30×0.85=25.50dollars."),
            new("Sweatshirt18dollars, sale85% of usual. Sale price in CENTS?",1530,"18×0.85=15.30dollars."),
            new("Soccer ball24dollars80cents, sale85% of usual. Sale price in CENTS?",2108,"2480×0.85=2108cents."),
            new("Eight six-packs of hot dogs at210cents each cost how many CENTS?",1680,"8×210=1680."),
            new("Six eight-packs of hot dogs at312cents each cost how many CENTS?",1872,"6×312=1872."),
            new("Both hotdog package offers supply48 hotdogs. Cheaper choice? Enter1 for six-packs,2 for eight-packs.",1,"1680cents <1872cents."),
            new("Elena56in tall; centimeters using2.54cm per inch, answer in HUNDREDTHS OF CM?",14224,"56×2.54=142.24cm."),
            new("Elena56in tall; meters in TEN-THOUSANDTHS of meter?",14224,"142.24cm=1.4224meters."),
            new("Usual price80 dollars reduced12. Sale price in DOLLARS?",68,"80-12=68."),
            new("85% of80 is how many DOLLARS?",68,"0.85×80=68."),
            new("40in minimum; 34in is how many INCHES below minimum?",6,"40-34=6."),
            new("40in minimum;54in is how many INCHES above minimum?",14,"54-40=14."),
            new("Tub80gal fills24gal. Gallons still unfilled?",56,"80-24=56.")
        ],
        [L0316FindPercentLogic] =
        [
            new("To find the percent C is of B (B nonzero), compute C/B×100. 1=yes,0=no.",1,"Definition of percent relative to reference."),
            new("34inches is85% of40inches. 1=yes,0=no.",1,"34/40=.85."),
            new("54inches is135% of40inches. 1=yes,0=no.",1,"54/40=1.35."),
            new("54inches cannot be135% of40inches because percentages never exceed100. 1=yes,0=no.",0,"Comparison may exceed100."),
            new("Saving12dollars off80 represents15% of usual price. 1=yes,0=no.",1,"12/80=.15."),
            new("Saving12dollars off80 represents85% of usual price. 1=yes,0=no.",0,"85% is the price remaining."),
            new("A tub80gallons with24gallons is30% full. 1=yes,0=no.",1,"24/80=.3."),
            new("Sale price85% of30dollars is25.50 dollars. 1=yes,0=no.",1,"0.85×30=25.5."),
            new("Sale price85% of24.80dollars is21.08 dollars. 1=yes,0=no.",1,"0.85×24.8=21.08."),
            new("For48 hotdogs, eight six-packs at2.10 each are cheaper than six eight-packs at3.12. 1=yes,0=no.",1,"16.80<18.72."),
            new("56 inches equals142.24cm given2.54cm per inch. 1=yes,0=no.",1,"56×2.54."),
            new("The reference quantity B is the100% amount when asking what percent C is of B. 1=yes,0=no.",1,"C/B×100%."),
            new("40inches is100% of the ride minimum40inches. 1=yes,0=no.",1,"Equal amounts."),
            new("A percentage based on one reference can be transferred to another unrelated reference without recalculating the amount. 1=yes,0=no.",0,"Amounts depend on base.")
        ],
        [L0317PaintCalculations] =
        [
            new("Given east wall9ft long and8ft high, its area in SQUARE FEET?",72,"9×8=72sqft."),
            new("Given total paintable wall area300sqft and two coats, total paint coverage in SQUARE FEET?",600,"300×2=600 square feet of coverage."),
            new("Source paint covers175sqft with2quarts. Coverage per QUART in HALF-SQUARE-FOOT UNITS?",175,"175/2=87.5sqft per quart, or175 half-sqft units."),
            new("Two coats require600sqft coverage and each2quarts covers175sqft. Minimum WHOLE QUARTS to buy?",7,"600/(175/2)=6.857... quarts, so7 whole quarts."),
            new("Paint costs1090cents per quart. Cost of7quarts WITHOUT discount in CENTS?",7630,"7×1090=7630cents."),
            new("Paint costs1090cents per quart with20% off. Discount per QUART in CENTS?",218,"0.2×1090=218."),
            new("Paint costs1090cents per quart with20% off. Sale price per QUART in CENTS?",872,"1090-218=872cents."),
            new("Seven quart paint cans with20% discount cost how many CENTS?",6104,"7×872=6104cents."),
            new("One gallon costs3490cents and3quarts each1090cents before discount. Total in CENTS?",6760,"3490+3×1090=6760cents."),
            new("One gallon3490cents plus3discounted quarts872cents each. Total in CENTS?",6106,"3490+3×872=6106cents."),
            new("Two gallons cost3490cents each. Total in CENTS?",6980,"2×3490=6980."),
            new("Compare seven discounted quart cans6104cents with one gallon plus3 discounted quarts6106cents. Savings in CENTS by choosing seven quarts?",2,"6106-6104=2cents."),
            new("Source east wall72sqft of total300sqft. Percent of wall area represented by east wall?",24,"72/300×100=24%."),
            new("Painting east72sqft with two coats takes96minutes. Minutes per SQUARE FOOT of wall area, in THIRD-MINUTE UNITS?",4,"96/72=4/3min/sqft."),
            new("At same solo pace as east wall96min/72sqft, solo minutes for entire300sqft?",400,"300×96/72=400min."),
            new("At same solo pace, remaining minutes after east wall96 of solo total400?",304,"400-96=304."),
            new("With friend, remaining painting takes150minutes instead of304solo. Minutes saved?",154,"304-150=154minutes."),
            new("Room's south window5ft by3ft not painted. Area excluded in SQFT?",15,"5×3=15 square feet."),
            new("A narrow paintable wall face measures0.5ft wide by8ft tall. Area in SQUARE FEET?",4,"0.5×8=4sqft."),
            new("Paint purchase: seven quarts supply how many HALF-SQUARE-FOOT COVERAGE UNITS given each quart87.5 sqft?",1225,"7×87.5=612.5sqft, or1225 half-square-foot units.")
        ],
        [L0317PaintReasoning] =
        [
            new("Two coats on300sqft of walls require covering600sqft in total. 1=yes,0=no.",1,"Two complete coats double coverage needed."),
            new("If each quart covers87.5sqft and600sqft must be coated,6 whole quarts are sufficient. 1=yes,0=no.",0,"6×87.5=525, need7."),
            new("Seven quarts can cover612.5sqft at87.5sqft per quart, enough for600sqft. 1=yes,0=no.",1,"Capacity exceeds need."),
            new("The east wall72sqft is25% of300sqft total. 1=yes,0=no.",0,"72/300=24%."),
            new("The east wall72sqft is24% of300sqft total. 1=yes,0=no.",1,"72/300×100=24%."),
            new("A painter's quoted rate96min for72sqft can estimate solo time for300sqft as400min under unchanged conditions. 1=yes,0=no.",1,"96/72×300=400."),
            new("If the rest takes304min alone but150min with a friend, saved painting time is154min. 1=yes,0=no.",1,"304-150=154."),
            new("Quarts sold with20% discount have an effective price872cents when original is1090cents. 1=yes,0=no.",1,"1090×.8=872."),
            new("One gallon and3 discounted quarts cost6106cents given gallon3490 and quart872. 1=yes,0=no.",1,"3490+2616=6106."),
            new("Seven discounted quarts cost6104cents, two cents less than one gallon plus3 discounted quarts. 1=yes,0=no.",1,"6106-6104=2."),
            new("Drying breaks must be added to painting minutes to answer a question restricted to active painting time. 1=yes,0=no.",0,"Prompt excludes drying/lunch pauses."),
            new("Open-ended project supplies costs have one guaranteed identical total for every valid purchase plan. 1=yes,0=no.",0,"Container and extra supplies decisions vary."),
            new("Areas of doors and windows should be excluded if those surfaces are not painted. 1=yes,0=no.",1,"Subtract nonpainted surfaces."),
            new("A numerically correct practice answer automatically verifies a student's floor-plan diagram and supply rationale. 1=yes,0=no.",0,"Teacher review is required."),
            new("The original lesson has numbered work on preparing, budgeting and painting time, even though it has no separate practice.html. 1=yes,0=no.",1,"Activities17.1–17.3 form the original capstone.")
        ],
        [L0401SourceNumerics] =
        [
            new("To obtain a quotient CLOSEST to1 with divisor40.01, choose dividend4,40,4000. Enter chosen number.",40,"40/40.01≈0.99975."),
            new("To obtain a quotient MUCH LESS than1 with divisor40.01, choose dividend4,40,4000. Enter chosen number.",4,"4/40.01≈0.1."),
            new("To obtain a quotient MUCH GREATER than1 with divisor40.01, choose dividend4,40,4000. Enter chosen number.",4000,"4000/40.01≈100."),
            new("100 divided by1/1000 equals?",100000,"100×1000=100000."),
            new("Rocking horse limit60 pounds;33 pounds is what PERCENT of limit?",55,"33/60×100=55%."),
            new("Rocking horse limit60 pounds;114 pounds is what PERCENT of limit?",190,"114/60×100=190%."),
            new("Rocking horse limit60 pounds;95% of weight limit in POUNDS?",57,"0.95×60=57."),
            new("Playlist has90 songs.40% rock. Number of rock songs?",36,"0.4×90=36."),
            new("Playlist has90 songs.10% country. Number of country songs?",9,"0.1×90=9."),
            new("Playlist has90 songs.30% hip hop. Number of hip-hop songs?",27,"0.3×90=27."),
            new("Playlist has90 songs. Rest electronica after40% rock10% country30% hip hop. Count?",18,"20%×90=18."),
            new("Garden hose emits9quarts in6seconds. Seconds to emit12quarts at same rate?",8,"9/6=1.5qt/s;12/1.5=8sec."),
            new("Garden hose emits9quarts in6seconds. Quarts in10seconds?",15,"1.5×10=15quarts."),
            new("Compare decimals0.7 and0.70. Enter1 if first is larger,0 if equal,2 if smaller.",0,"Trailing zero does not change value."),
            new("Compare0.03+6/10 with0.30+6/100. Enter1 if first larger,0 equal,2 smaller.",1,"0.63>0.36."),
            new("Compare0.9 vs0.12. Enter1 if first larger,0 equal,2 smaller.",1,"0.9>0.12.")
        ],
        [L0401QuotientScale] =
        [
            new("100 divided by1/1000 is much greater than1. 1=yes,0=no.",1,"Dividing by a tiny positive number produces large quotient."),
            new("50 and1/3 divided by50 and1/4 is close to1. 1=yes,0=no.",1,"Dividend and divisor almost equal."),
            new("4.7 divided by5.2 is close to1. 1=yes,0=no.",1,"Quotient≈0.904."),
            new("2 divided by7335 is much smaller than1. 1=yes,0=no.",1,"Divisor greatly exceeds dividend."),
            new("2000001 divided by9 is much greater than1. 1=yes,0=no.",1,"Dividend greatly exceeds divisor."),
            new("0.002 divided by2000 is much smaller than1. 1=yes,0=no.",1,"One millionth."),
            new("40 divided by40.01 is close to1. 1=yes,0=no.",1,"Quotient≈0.99975."),
            new("4 divided by40.01 is much greater than1. 1=yes,0=no.",0,"Quotient≈0.10."),
            new("4000 divided by40.01 is much greater than1. 1=yes,0=no.",1,"Quotient≈99.975."),
            new("For positive a, dividing a by a positive number smaller than1 may produce a quotient larger than a. 1=yes,0=no.",1,"E.g.2/0.5=4."),
            new("Dividing by a positive number much larger than a positive dividend gives quotient less than1. 1=yes,0=no.",1,"Ratio dividend/divisor<1."),
            new("A thicker object generally needs MORE copies than thin object to fill same stack height. 1=yes,0=no.",0,"Thicker means fewer copies."),
            new("For a positive dividend, a larger positive divisor produces a larger quotient. 1=yes,0=no.",0,"Monotonic inverse relationship."),
            new("Two numerically close positive numbers yield a quotient close to1. 1=yes,0=no.",1,"Provided one is not zero and difference small relative to magnitude.")
        ],
        [L0402DivisionNumbers] =
        [
            new("20 pounds strawberries shared by5 people. Pounds PER PERSON?",4,"20÷5=4lb per person."),
            new("20 pounds strawberries shared5lb per person. Number of PEOPLE?",4,"20÷5=4people."),
            new("A museum needs180 dollars. Tickets cost15 dollars EACH. Number of tickets?",12,"180÷15=12 tickets."),
            new("A museum needs180 dollars for15 tickets. Price per ticket in DOLLARS?",12,"180÷15=12 dollars."),
            new("Ten divided by5 equals?",2,"5×2=10."),
            new("Four and a half divided by3, answer in HALVES?",3,"4.5÷3=1.5=3halves."),
            new("One half divided by4, answer in EIGHTHS?",1,"1/2÷4=1/8."),
            new("Two and a half gallons divided equally among5 bottles. Fill per bottle in HALF-GALLONS?",1,"2.5÷5=0.5gallon."),
            new("200 golf balls divided into4 buckets. Balls per bucket?",50,"200÷4=50."),
            new("Sixteen socks in pairs. Number of pairs?",8,"16÷2=8."),
            new("a divided by6 equals1. What is a?",6,"6/6=1."),
            new("a divided by6 is greater than1, and a is a positive integer. Smallest a?",7,"Any a>6 works; smallest integer7."),
            new("a divided by6 is less than1 and positive a is a whole number. Largest positive a?",5,"Largest positive whole a<6 is5."),
            new("0.25 is what PERCENT of1?",25,"0.25×100=25."),
            new("0.1 is what PERCENT of1?",10,"0.1×100=10."),
            new("0.75 is what PERCENT of1?",75,"0.75×100=75."),
            new("1.5 is what PERCENT of1?",150,"1.5×100=150."),
            new("1.4 is what PERCENT of1?",140,"1.4×100=140."),
            new("Jada walks3mph, Elena2.8mph, both for3hours. Difference in TENTHS OF MILE?",6,"(3-2.8)×3=0.6mile=6tenths."),
            new("Two and a half gallons split five equal bottles. Number of QUARTS each, assuming4quarts per gallon?",2,"0.5gallon=2quarts.")
        ],
        [L0402DivisionMeaningLogic] =
        [
            new("20÷5=4 can mean20lb shared among5 people gives4lb per person. 1=yes,0=no.",1,"Number of groups known."),
            new("20÷5=4 can mean20lb split into5lb portions yields4 people. 1=yes,0=no.",1,"Size of each group known."),
            new("20÷5=4 has one and only one possible contextual interpretation. 1=yes,0=no.",0,"Unknown groups or unknown group size."),
            new("180÷15=12 can represent12 tickets at15 dollars each. 1=yes,0=no.",1,"12×15=180."),
            new("180÷15=12 can represent15 tickets at12 dollars each. 1=yes,0=no.",1,"15×12=180."),
            new("4.5÷3=1.5 can be checked by multiplying1.5×3=4.5. 1=yes,0=no.",1,"Inverse operation."),
            new("1/2 divided by4 equals1/8. 1=yes,0=no.",1,"Four equal parts of a half."),
            new("Two and a half gallons among5 bottles means each gets2 gallons. 1=yes,0=no.",0,"Each gets half a gallon."),
            new("200 balls among4 equal buckets means50 per bucket. 1=yes,0=no.",1,"200/4=50."),
            new("Sixteen socks in pairs make8 pairs. 1=yes,0=no.",1,"16/2=8."),
            new("If a/6 is greater than1 for positive a, then a>6. 1=yes,0=no.",1,"Divisor6."),
            new("If a/6=1 then a=6. 1=yes,0=no.",1,"Equal factors."),
            new("If a/6 is less than1 for positive a, a must be greater than6. 1=yes,0=no.",0,"a<6."),
            new("The quotient can represent either number of groups or amount per group depending on units. 1=yes,0=no.",1,"Context determines unit of unknown."),
            new("At3mph versus2.8mph for3h, distance difference is0.6mile. 1=yes,0=no.",1,"9 vs8.4 miles.")
        ],
        [L0403ContextualDivision] =
        [
            new("Mai has36 dollars and movie tickets cost4.50dollars each. Number of tickets?",8,"36÷4.50=8."),
            new("Sixteen items arranged in groups of8. Number of groups?",2,"16÷8=2."),
            new("Sixteen items arranged in2 groups. Items per group?",8,"16÷2=8."),
            new("Four divided by one and one-third equals how many groups?",3,"4÷(4/3)=3."),
            new("Mini muffins cost300cents per12. Price per muffin in CENTS?",25,"300÷12=25."),
            new("At25cents per muffin, how many muffins can be bought for200cents?",8,"200÷25=8."),
            new("At25cents per muffin, price of16 muffins in CENTS?",400,"16×25=400."),
            new("Family monthly budget2400 dollars,44% housing. Dollars on housing?",1056,"2400×0.44=1056."),
            new("Family monthly budget2400 dollars,23% food. Dollars on food?",552,"2400×0.23=552."),
            new("Family monthly budget2400 dollars,6% clothing. Dollars on clothing?",144,"2400×0.06=144."),
            new("Family monthly budget2400 dollars,17% transportation. Dollars on transport?",408,"2400×0.17=408."),
            new("Family budget2400 with44% housing23% food6% clothing17% transport. Percent savings?",10,"100-44-23-6-17=10%."),
            new("Family budget2400 dollars with10% remaining. Savings in DOLLARS?",240,"2400×0.1=240."),
            new("A whole4 is divided into groups of4/3. How many groups?",3,"Three groups of4/3 make4."),
            new("Noah says division always reduces positive values. Counterexample2 divided by0.5 equals?",4,"2÷0.5=4."),
            new("Movie tickets cost450cents each. Cost for8 tickets in CENTS?",3600,"450×8=3600."),
            new("Twelve muffins cost300cents, cost of24 muffins in CENTS?",600,"2×300=600.")
        ],
        [L0403DivisionInterpretation] =
        [
            new("36 dollars at4.50 dollars per ticket purchases8 tickets. 1=yes,0=no.",1,"8×4.50=36."),
            new("For16÷8=2, 2 can represent number of equal groups with8 in each. 1=yes,0=no.",1,"16=2×8."),
            new("For16÷2=8, 8 can represent group size if there are2 equal groups. 1=yes,0=no.",1,"16=2×8."),
            new("Both16÷8=2 and16÷2=8 can be represented by the same group arrangement with differing unknowns. 1=yes,0=no.",1,"Groups and items per group swap."),
            new("4÷(1⅓)=3 because3 groups of1⅓ make4. 1=yes,0=no.",1,"3×4/3=4."),
            new("Dividing by a positive fraction less than1 always gives a smaller positive result than the dividend. 1=yes,0=no.",0,"E.g.2÷0.5=4."),
            new("Mini muffins are25cents each if a dozen costs300cents. 1=yes,0=no.",1,"300÷12=25."),
            new("Two dollars buys8 muffins at25cents each. 1=yes,0=no.",1,"200÷25=8."),
            new("Sixteen mini muffins at25cents each cost4 dollars. 1=yes,0=no.",1,"16×25=400cents."),
            new("In a budget44%+23%+6%+17%=90%, leaving10% savings. 1=yes,0=no.",1,"Total allocated90%."),
            new("Housing44% of2400dollars equals1056dollars. 1=yes,0=no.",1,"2400×.44=1056."),
            new("A correct multiplication equation can check a division quotient. 1=yes,0=no.",1,"Inverse relationship."),
            new("The division quotient always has the same real-world units as the dividend. 1=yes,0=no.",0,"Depends on grouping interpretation."),
            new("Teacher review is needed for a student-created visual diagram even when arithmetic is automatically checked. 1=yes,0=no.",1,"Diagram correctness needs a separate rubric.")
        ],
        [L0404FractionGroups] =
        [
            new("A bag holds3lb cat food and cat eats3/4lb each week. Weeks per bag?",4,"3÷(3/4)=4weeks."),
            new("How many one-third units are in one and two-thirds?",5,"5/3 divided by1/3=5."),
            new("Unknown ? multiplied by1/8 equals3. What is??",24,"24×1/8=3."),
            new("Fifteen multiplied by2/5 yields what whole number?",6,"15×2/5=6."),
            new("Six divided by2/5 yields what whole number?",15,"6×5/2=15."),
            new("Six divided by15, expressed in FIFTHS?",2,"6/15=2/5, two fifths."),
            new("Six multiplied by4/3 yields what whole number?",8,"6×4/3=8."),
            new("Eight divided by4/3 yields what whole number?",6,"8×3/4=6."),
            new("Eight divided by6, expressed in THIRDS?",4,"8/6=4/3, four thirds."),
            new("Sixteen multiplied by7/8 yields what whole number?",14,"16×7/8=14."),
            new("Fourteen divided by7/8 yields what whole number?",16,"14×8/7=16."),
            new("Fourteen divided by16, expressed in EIGHTHS?",7,"14/16=7/8."),
            new("Eight amusement park tickets total100 dollars. Price per ticket in CENTS?",1250,"10000/8=1250cents."),
            new("Noah brings13 dollars, ticket costs12.50. How many CENTS remain if he buys one?",50,"1300-1250=50cents."),
            new("Eight divided by0.001 equals?",8000,"8×1000=8000."),
            new("Twelve is150% of what number?",8,"12/1.5=8."),
            new("Five is50% of what number?",10,"5/0.5=10."),
            new("10% of what number is300?",3000,"300/0.10=3000."),
            new("5% of what number is72?",1440,"72/0.05=1440."),
            new("Twenty is80% of what number?",25,"20/0.8=25.")
        ],
        [L0404FractionModels] =
        [
            new("Three pounds divided by3/4 pound per week lasts4weeks. 1=yes,0=no.",1,"4×3/4=3."),
            new("Three pounds divided by3/4 pound per week lasts3weeks. 1=yes,0=no.",0,"3×3/4=2.25."),
            new("One and two-thirds contains5 one-third groups. 1=yes,0=no.",1,"5/3÷1/3=5."),
            new("The equation?×1/8=3 asks how many eighths fit in3. 1=yes,0=no.",1,"Answer24."),
            new("The equation?×1/8=3 asks for one eighth of3. 1=yes,0=no.",0,"It asks count of1/8 groups."),
            new("Fifteen times2/5 equals6. 1=yes,0=no.",1,"30/5=6."),
            new("Six times4/3 equals8. 1=yes,0=no.",1,"6×4/3=8."),
            new("Sixteen times7/8 equals14. 1=yes,0=no.",1,"16×7/8=14."),
            new("One whole divided into thirds contains3 one-third parts. 1=yes,0=no.",1,"1÷1/3=3."),
            new("Two wholes contain6 one-third parts. 1=yes,0=no.",1,"2×3=6."),
            new("The number of groups can increase when dividing by a positive fraction less than1. 1=yes,0=no.",1,"3÷3/4=4."),
            new("An8-person ticket charge totaling100dollars means12.50 dollars per student, so13dollars is sufficient. 1=yes,0=no.",1,"13>12.50."),
            new("When a yellow pattern-block hexagon represents1 whole, a one-third block covers one of3 equal area parts. 1=yes,0=no.",1,"Unit whole fixed by hexagon."),
            new("Fraction models must clearly label which shape represents the unit whole. 1=yes,0=no.",1,"Reference matters.")
        ],
        [L0405FractionValues] =
        [
            new("How many1/3 groups are in1/2? Answer in HALF-GROUP UNITS.",3,"1/2÷1/3=3/2 groups, three half-groups."),
            new("Seven divided by1/2 gives how many groups?",14,"7×2=14."),
            new("Six divided by3/8 gives how many groups?",16,"6×8/3=16."),
            new("One and7/8 divided by5/16 equals how many groups?",6,"15/8×16/5=6."),
            new("One and1/2 divided by2/5 equals how many QUARTER-GROUPS?",15,"3/2×5/2=15/4, fifteen quarter-groups."),
            new("Three wholes contain how many ONE-FOURTH pieces?",12,"3÷1/4=12."),
            new("Six wholes contain how many TWO-THIRDS pieces?",9,"6÷2/3=9."),
            new("Milk vendor A sells2L for380cents. Unit price in CENTS per liter?",190,"380/2=190cents/L."),
            new("Milk vendor B sells1.5L for270cents. Unit price in CENTS per liter?",180,"270/1.5=180cents/L."),
            new("For milk, vendor A190c/L, vendor B180c/L. Cheaper vendor? Enter1 A or2 B.",2,"B is10cents/L cheaper."),
            new("Flour:sugar recipe5cups:2cups. Sugar per cup flour in FIFTH-CUP portions?",2,"2/5cup sugar per cup flour."),
            new("Flour:sugar recipe5:2. Flour per cup sugar in HALF-CUP portions?",5,"5/2cup flour, five half-cup portions."),
            new("Flour:sugar recipe5:2. With7 cups sugar, flour cups in HALF-CUP units?",35,"7×5/2=17.5cups,35 half-cups."),
            new("Flour:sugar recipe5:2. With6 cups flour, sugar in FIFTH-CUP units?",12,"6×2/5=12/5cups."),
            new("A third of one whole fits in one half how many HALF-COUNTS?",3,"1.5 groups=3 halves."),
            new("One whole is divided into thirds. How many thirds?",3,"1÷1/3=3."),
            new("Three dollars80 cents buys2L. Price for4L in CENTS?",760,"Double."),
            new("The first milk vendor sells2L for380cents. Cost per HALF-LITER in CENTS?",95,"190c/L×0.5=95.")
        ],
        [L0405FractionMeaning] =
        [
            new("One half contains one and a half one-third groups. 1=yes,0=no.",1,"1/2÷1/3=1.5."),
            new("Half divided by third equals two-thirds. 1=yes,0=no.",0,"Correct3/2."),
            new("Seven has fourteen one-half groups. 1=yes,0=no.",1,"7×2=14."),
            new("Six has sixteen groups of3/8. 1=yes,0=no.",1,"6÷3/8=16."),
            new("1⅞ contains6 groups of5/16. 1=yes,0=no.",1,"30/16÷5/16=6."),
            new("1½ contains3¾ groups of2/5. 1=yes,0=no.",1,"1.5÷0.4=3.75."),
            new("The number of equal groups may be a non-integer fraction or mixed number. 1=yes,0=no.",1,"Partial group fits."),
            new("The yellow hexagon, rather than the red trapezoid, must be1 whole in every task. 1=yes,0=no.",0,"Source lesson compares different whole references."),
            new("One-half divided by one-third can be shown using equivalent sixths:3 sixths versus2 sixths. 1=yes,0=no.",1,"Three sixths contains1.5 pairs of sixths."),
            new("Milk vendor B at180cents per liter is cheaper than vendor A at190cents per liter. 1=yes,0=no.",1,"Compare same units."),
            new("Six whole cups of flour requires12/5 cups sugar at ratio5:2. 1=yes,0=no.",1,"6×2/5."),
            new("Seven cups sugar needs17.5 cups flour at ratio5:2. 1=yes,0=no.",1,"7×5/2."),
            new("A division equation can be verified by multiplying quotient times divisor. 1=yes,0=no.",1,"Unknown factor."),
            new("A drawing of a half group requires identifying the entire reference group first. 1=yes,0=no.",1,"Whole reference matters.")
        ],
        [L0406TapeValues] =
        [
            new("Three divided by1/4 equals how many groups?",12,"3×4=12."),
            new("Three divided by3/5 equals how many groups?",5,"3×5/3=5."),
            new("Seven days split into1/2-day periods. Number of periods?",14,"7×2=14."),
            new("One divided by5/6 gives how many FIFTH-GROUP UNITS?",6,"1÷5/6=6/5groups."),
            new("One divided by4/5 gives how many QUARTER-GROUP UNITS?",5,"1÷4/5=5/4 groups."),
            new("Ten percent of70?",7,"70/10=7."),
            new("Ten percent of110?",11,"110/10=11."),
            new("Twenty-five percent of160?",40,"160/4=40."),
            new("Twenty-five percent of48?",12,"48/4=12."),
            new("Fifty percent of90?",45,"90/2=45."),
            new("Fifty percent of350?",175,"350/2=175."),
            new("Seventy-five percent of300?",225,"300×3/4=225."),
            new("Seventy-five percent of48?",36,"48×3/4=36."),
            new("How many1/2 groups fit into2 wholes?",4,"2×2=4."),
            new("How many3/4 groups fit into3 wholes?",4,"3÷.75=4."),
            new("How many2/5 groups fit into4 wholes?",10,"4÷.4=10."),
            new("An unknown number of4/5 groups makes1 whole. Answer in QUARTER-GROUP UNITS?",5,"Unknown=5/4 groups."),
            new("How many1/3 groups fit into2 wholes?",6,"2×3=6.")
        ],
        [L0406TapeLogic] =
        [
            new("3÷1/4=12. Enter1=yes,0=no.",1,"Twelve quarter groups fit3."),
            new("3÷3/5=5. Enter1=yes,0=no.",1,"Five groups of3/5 make3."),
            new("A week has14 half-day periods. Enter1=yes,0=no.",1,"7÷1/2=14."),
            new("There are6/5 groups of5/6 in1 whole. Enter1=yes,0=no.",1,"(6/5)×(5/6)=1."),
            new("1÷4/5=5/4. Enter1=yes,0=no.",1,"Reciprocal factor."),
            new("The equation ?×4/5=1 represents how many4/5-sized groups fit in1. Enter1=yes,0=no.",1,"Original choice D."),
            new("The equation 1÷4/5=? represents how many4/5-sized groups fit in1. Enter1=yes,0=no.",1,"Original choice E."),
            new("The equation 4/5÷1=? represents how many4/5-sized groups fit in1. Enter1=yes,0=no.",0,"It asks about division by1."),
            new("Tape intervals may be unequal even when representing equal-size fractional groups. Enter1=yes,0=no.",0,"Group-size intervals should match."),
            new("75% of48 is36. Enter1=yes,0=no.",1,"Three quarters."),
            new("10% of70 is7. Enter1=yes,0=no.",1,"One tenth."),
            new("25% of160 is40. Enter1=yes,0=no.",1,"One quarter."),
            new("50% of350 is175. Enter1=yes,0=no.",1,"One half."),
            new("A diagram must label the reference whole before counting fractional groups. Enter1=yes,0=no.",1,"Fraction definition depends on whole.")
        ],
        [L0407FractionGroups] =
        [
            new("One batch needs1/2lb flour. How many batches with1lb?",2,"1÷1/2=2."),
            new("One batch needs1/2lb flour. With3/4lb, batches in HALF-BATCH UNITS?",3,"3/4÷1/2=3/2 batches."),
            new("One batch needs1/2lb flour. With1/4lb, batches in HALF-BATCH UNITS?",1,"1/4÷1/2=1/2 batch."),
            new("Whiskers weighs2⅔kg and Piglio4kg. Whiskers as fraction of Piglio, in THIRDS?",2,"(8/3)/4=2/3."),
            new("Piglio4kg to Whiskers2⅔kg. Piglio as fraction in HALF-UNITS?",3,"4/(8/3)=3/2."),
            new("Andre's trip is1⅝km, walked1/3km. Fraction completed in THIRTY-NINTHS?",8,"(1/3)/(13/8)=8/39."),
            new("What fraction of2½ is4/5? Answer in TWENTY-FIFTHS?",8,"(4/5)/(5/2)=8/25."),
            new("How many groups of3/4 are in11/4? Answer in THIRDS?",11,"(11/4)/(3/4)=11/3 groups."),
            new("How many groups of3/4 are in6½? Answer in THIRDS?",26,"(13/2)×(4/3)=26/3."),
            new("Four divided by2/7 gives how many groups?",14,"4×7/2=14."),
            new("One-fourth divided by one-half, answer in HALF-GROUPS?",1,"1/4÷1/2=1/2 group."),
            new("One-third divided by one-half, answer in THIRD-GROUP UNITS?",2,"1/3÷1/2=2/3 group."),
            new("Two-thirds divided by four-thirds, answer in HALF-GROUPS?",1,"(2/3)/(4/3)=1/2."),
            new("One-sixth divided by one-third, answer in HALF-GROUPS?",1,"1/6÷1/3=1/2."),
            new("A full group is1/2kg, available1/8kg. Fraction of full group in QUARTERS?",1,"1/8÷1/2=1/4."),
            new("Two whole kilograms split into half-kilogram groups. Number of groups?",4,"2÷1/2=4.")
        ],
        [L0407FractionLogic] =
        [
            new("One-quarter pound flour makes half of a batch needing one-half pound. 1=yes,0=no.",1,"1/4÷1/2=1/2."),
            new("Three-quarters pound makes one and one-half batches needing half-pound each. 1=yes,0=no.",1,"3/4÷1/2=1.5."),
            new("One pound flour makes only half a batch needing half-pound each. 1=yes,0=no.",0,"Makes2 batches."),
            new("Whiskers2⅔kg is2/3 as heavy as Piglio4kg. 1=yes,0=no.",1,"8/3÷4=2/3."),
            new("Piglio4kg is1½ as heavy as Whiskers2⅔kg. 1=yes,0=no.",1,"4÷8/3=1.5."),
            new("The equation ?×1⅝=1/3 asks what fraction of Andre's trip has been walked. 1=yes,0=no.",1,"Part divided by total."),
            new("The equation ?×1⅝=1/3 asks what trip distance remains in kilometers. 1=yes,0=no.",0,"It asks fractional portion."),
            new("Four divided by2/7 asks how many two-seventh-sized groups fit into four. 1=yes,0=no.",1,"Original choice B."),
            new("Four divided by2/7 is equal to4×2/7. 1=yes,0=no.",0,"Division uses reciprocal."),
            new("If an amount is smaller than one whole group, the number of groups can be between0 and1. 1=yes,0=no.",1,"Fractional group."),
            new("A group-count model uses group count×group size=total amount. 1=yes,0=no.",1,"Multiplicative structure."),
            new("11/4 divided by3/4 gives11/3 groups. 1=yes,0=no.",1,"Cancel quarters."),
            new("The original unit whole matters when describing a fraction of a group. 1=yes,0=no.",1,"Reference and scale must be defined."),
            new("A small positive dividend divided by a larger positive divisor yields a quotient greater than1. 1=yes,0=no.",0,"Quotient below1.")
        ],
        [L0408GroupSizeNumbers] =
        [
            new("One cup strawberries makes3/4 cake. Cups for full cake in THIRD-CUP units?",4,"1÷3/4=4/3cups."),
            new("1½cups raspberries make3/4 cake. Cups for full cake?",2,"1.5÷0.75=2."),
            new("A painter covers9/2 square yards with3 gallons. Gallons per square yard in THIRD-GALLON units?",2,"3/(9/2)=2/3gallons per sqyd."),
            new("A painter covers9/2 square yards with3 gallons. Square yards per gallon in HALF-SQUARE-YARD units?",3,"(9/2)/3=3/2sqyd per gallon."),
            new("Han walks1/4mile and is1/3 of way to school. Full distance in QUARTER-MILE units?",3,"1/4÷1/3=3/4mile,3quarters."),
            new("Four fifths divided by two thirds: answer in FIFTH-GROUP UNITS?",6,"(4/5)/(2/3)=6/5groups."),
            new("Books1.5in wide fill a36in shelf. How many books?",24,"36÷1.5=24."),
            new("Fifty-six divided by8 equals?",7,"56/8=7."),
            new("56 divided by8000000 in MILLIONTHS?",7,"56/8000000=7/1000000."),
            new("56 divided by0.000008 equals?",7000000,"56×125000=7000000."),
            new("Choose a divisor16 for56÷16. Answer in HALF-UNITS?",7,"56/16=3.5, or7 halves."),
            new("One cup enough for3/4 cake. How many cups for half cake in THIRD-CUP units?",2,"Full4/3 cups; half2/3 cups."),
            new("A wall covers4.5 square yards with3 gallons. Gallons per1.5square yards?",1,"Divide into3 sections1.5sqyd each."),
            new("A cake requires4/3 cups strawberries. 3 whole cakes need how many cups?",4,"3×4/3=4."),
            new("Raspberry cake needs2 cups per cake. 5 cakes need how many cups?",10,"2×5=10."),
            new("If Han's full journey3/4mile and he walked1/4mile, remaining distance in QUARTER-MILE units?",2,"3/4-1/4=2/4 mile.")
        ],
        [L0408GroupSizeLogic] =
        [
            new("One cup for3/4 cake means4/3 cups for full cake. 1=yes,0=no.",1,"1÷3/4=4/3."),
            new("One and one-half cups for3/4 cake means2 cups per full cake. 1=yes,0=no.",1,"1.5÷.75=2."),
            new("Three gallons for4.5square yards means2/3 gallon per square yard. 1=yes,0=no.",1,"3/4.5=2/3."),
            new("One-quarter mile is1/3 of3/4mile. 1=yes,0=no.",1,"(1/4)/(3/4)=1/3."),
            new("Four-fifths divided by two-thirds equals6/5. 1=yes,0=no.",1,"Multiply by reciprocal."),
            new("Twenty-four books of1.5in each fit a36in shelf. 1=yes,0=no.",1,"24×1.5=36."),
            new("56÷8million is much less than1. 1=yes,0=no.",1,"Seven millionths."),
            new("56÷0.000008 is far greater than1. 1=yes,0=no.",1,"7million."),
            new("56÷8 equals7, so it is between1 and7. 1=yes,0=no.",0,"Equals7, not strictly less."),
            new("56÷16 equals3.5, strictly between1 and7. 1=yes,0=no.",1,"1<3.5<7."),
            new("If size of each group is unknown, total÷number of groups finds the group size. 1=yes,0=no.",1,"Partition interpretation."),
            new("For fractions, an amount can be less than the size of a whole group, requiring quotient below1. 1=yes,0=no.",1,"Fraction of group."),
            new("A picture-based tape diagram should identify which section is one whole. 1=yes,0=no.",1,"Reference needed."),
            new("A numeric quotient alone proves a student's original drawn source diagram accurate. 1=yes,0=no.",0,"Teacher verifies diagram.")
        ],
        [L0409SourceWordValues] =
        [
            new("Friends share2½lb berries, each gets5/4lb. Number of friends?",2,"(5/2)/(5/4)=2."),
            new("Friends share2½lb berries equally among5. Pounds per person in HALF-POUND UNITS?",1,"2.5/5=0.5lb."),
            new("Two-fifths kg soil is one-third of container. Full container kg in FIFTHS?",6,"2/5÷1/3=6/5kg."),
            new("A container holds6/5kg. Can1kg fit? Enter1 yes,0 no.",1,"1<6/5."),
            new("Three quarters hour rain fills2/5 gauge. Minutes elapsed?",45,"3/4hour=45min."),
            new("Rain fills2/5 gauge in45min. Filled fraction after60min in FIFTEENTHS?",8,"2/5×60/45=8/15."),
            new("Three museum tickets cost1275cents. Price per ticket in CENTS?",425,"1275/3=425."),
            new("Three museum tickets cost1275cents. Five tickets cost in CENTS?",2125,"425×5=2125."),
            new("Elena walks60m in15sec. Meters per second?",4,"60/15=4."),
            new("Noah walks50m in10sec. Meters per second?",5,"50/10=5."),
            new("Who faster at4m/s Elena or5m/s Noah? Enter1 Elena,2 Noah.",2,"Noah5>4."),
            new("Trail mix1 batch uses2cups cereal. Four batches use cereal CUPS?",8,"2×4=8."),
            new("Trail mix1 batch uses1/3cup almonds. Three batches use almonds CUPS?",1,"3×1/3=1."),
            new("Trail mix1 batch uses1/4cup raisins. Four batches use raisins CUPS?",1,"4×1/4=1."),
            new("Trail mix1 batch uses1/3cup almonds. Two batches in THIRD-CUP portions?",2,"2/3cups."),
            new("Trail mix1 batch uses1/4cup raisins. Three batches in QUARTER-CUP portions?",3,"3/4cup."),
            new("Trail mix1 batch uses2cups cereal. Three batches use cereal CUPS?",6,"3×2=6."),
            new("Rain fills2/5 gauge in3/4hour. Fraction filled in15minutes in FIFTEENTHS?",2,"2/5×15/45=2/15.")
        ],
        [L0409ModelReasoning] =
        [
            new("2½lb split into5/4lb groups gives2groups. 1=yes,0=no.",1,"2.5/1.25=2."),
            new("2½lb split among5 people gives½lb each. 1=yes,0=no.",1,"2.5/5=.5."),
            new("2/5kg fills1/3 of a container, so full holds6/5kg. 1=yes,0=no.",1,"2/5÷1/3=6/5."),
            new("One kg fits in a6/5kg capacity container. 1=yes,0=no.",1,"1<1.2."),
            new("Rain gauge2/5 full at45min will be8/15 full after60min at steady rainfall. 1=yes,0=no.",1,"2/5×4/3=8/15."),
            new("The equation3/4÷2/5 directly gives fraction filled at60minutes. 1=yes,0=no.",0,"It gives time per full-gauge scale, not filled fraction at60min."),
            new("Three tickets cost12.75dollars, so five cost21.25 at constant rate. 1=yes,0=no.",1,"4.25 each."),
            new("Noah5m/sec is faster than Elena4m/sec. 1=yes,0=no.",1,"Speed per equal second."),
            new("Trail mix cereal amount for4 batches is8cups at2cups/batch. 1=yes,0=no.",1,"2×4=8."),
            new("Three batches of trail mix at1/3cup almonds per batch require1cup almonds. 1=yes,0=no.",1,"3×1/3=1."),
            new("Four batches each1/4cup raisins require1cup. 1=yes,0=no.",1,"4×1/4=1."),
            new("An unscripted fraction word problem may require deciding which quantity represents groups. 1=yes,0=no.",1,"Contextual interpretation."),
            new("A tape diagram can help check whether a model treats rate as constant. 1=yes,0=no.",1,"Corresponding segments."),
            new("Rainfall must remain steady to infer future gauge fill from a short observation. 1=yes,0=no.",1,"Constant-rate assumption.")
        ],
        [L0410AlgorithmValues] =
        [
            new("24 apples divided into half-apple shares. Number of shares?",48,"24÷1/2=48."),
            new("One divided by1/10 equals?",10,"1×10=10."),
            new("Four divided by1/10 equals?",40,"4×10=40."),
            new("Eighteen divided by1/10 equals?",180,"18×10=180."),
            new("Four divided by2/10 equals?",20,"4×10/2=20."),
            new("Four divided by8/10 equals?",5,"4×10/8=5."),
            new("Five divided by1/10 equals?",50,"5×10=50."),
            new("Five divided by3/10, answer in THIRDS?",50,"5×10/3=50/3."),
            new("Five divided by9/10, answer in NINTHS?",50,"5×10/9=50/9."),
            new("Two and a half divided by1/8 equals?",20,"5/2×8=20."),
            new("Two and a half divided by5/8 equals?",4,"5/2×8/5=4."),
            new("Road paving3/5km per week. Time to pave1km in THIRD-WEEK UNITS?",5,"1÷3/5=5/3weeks."),
            new("Pancake mix1¾lb; using7/8lb. Fraction used in HALF-UNIT FRACTIONS?",1,"7/8÷7/4=1/2."),
            new("Twenty-five percent of400?",100,"One-quarter400."),
            new("Fifty percent of90?",45,"One-half90."),
            new("Seventy-five percent of200?",150,"Three quarters200."),
            new("Ten percent of8000?",800,"One tenth."),
            new("Five percent of20?",1,"One twentieth."),
            new("4÷0.2 gives?",20,"4÷1/5=20."),
            new("4÷0.8 gives?",5,"4÷4/5=5.")
        ],
        [L0410AlgorithmLogic] =
        [
            new("Dividing a positive number by1/10 is the same as multiplying by10. 1=yes,0=no.",1,"Reciprocal of1/10."),
            new("24÷1/2=12. 1=yes,0=no.",0,"24 half-apple shares double to48."),
            new("4÷2/10 equals20. 1=yes,0=no.",1,"Multiply by10 and divide by2."),
            new("4÷8/10 equals5. 1=yes,0=no.",1,"4×10/8=5."),
            new("To divide by2/5, multiply by5 and then divide by2. 1=yes,0=no.",1,"Reciprocal5/2."),
            new("5÷3/10 equals50/3. 1=yes,0=no.",1,"5×10/3."),
            new("5÷9/10 equals50/9. 1=yes,0=no.",1,"5×10/9."),
            new("2½÷5/8 equals4. 1=yes,0=no.",1,"20÷5=4."),
            new("Paving3/5km per week takes5/3 weeks per kilometer at a constant rate. 1=yes,0=no.",1,"1/(3/5)=5/3."),
            new("A box1¾lb of pancake mix with7/8lb used has half used. 1=yes,0=no.",1,"7/8÷14/8=1/2."),
            new("The numerator and denominator of a nonzero divisor fraction can be swapped to multiply by the reciprocal. 1=yes,0=no.",1,"a÷(b/c)=a×(c/b)."),
            new("Dividing by a fraction less than1 always makes the quotient smaller for positive dividend. 1=yes,0=no.",0,"It gets larger."),
            new("A tape diagram and reciprocal algorithm should produce the same correct quotient. 1=yes,0=no.",1,"Equivalent reasoning."),
            new("One must avoid using0 as the divisor of a fraction-division expression. 1=yes,0=no.",1,"Division by zero undefined.")
        ],
        [L0411DivisionFluency] =
        [
            new("14/15 divided by7/5, answer in THIRDS?",2,"14/15×5/7=2/3."),
            new("4/3 divided by5/2, answer in FIFTEENTHS?",8,"4/3×2/5=8/15."),
            new("15/4 divided by5/8 equals?",6,"15/4×8/5=6."),
            new("Kiran has2¾lb flour and fills4⅛ equal bags. Pounds per bag in THIRDS?",2,"(11/4)/(33/8)=2/3lb."),
            new("Four and a half divided by1/8 equals?",36,"9/2×8=36."),
            new("Four and a half divided by1/4 equals?",18,"9/2×4=18."),
            new("Four and a half divided by1/6 equals?",27,"9/2×6=27."),
            new("A phone reaches2/5 charged in1/3hour at constant rate. Time to full in MINUTES?",50,"(1/3)/(2/5)=5/6h=50min."),
            new("A bucket2/5 full holds2½lb water. Weight when full in QUARTER-POUND units?",25,"5/2÷2/5=25/4lb."),
            new("14/15 divided by7/5: multiplying first by5 and then dividing by7 gives numerator in THIRDS?",2,"14/15×5÷7=2/3."),
            new("4/3 divided by5/2: reciprocal multiplier denominator?",5,"Reciprocal of5/2 is2/5."),
            new("4/3 divided by5/2: reciprocal multiplier numerator?",2,"Reciprocal is2/5."),
            new("A phone needs50min per charge. Time for3 charges at same constant model in MINUTES?",150,"3×50=150."),
            new("A bucket full holds25/4lb. Half-full water in EIGHTH-POUND units?",25,"Half of25/4=25/8lb."),
            new("Two and three-quarters pounds is how many QUARTER-POUND portions?",11,"11/4 divided by1/4=11."),
            new("Four and one-eighth bags expressed in EIGHTH-BAG portions?",33,"4⅛=33/8."),
            new("A fraction6/5 is what number of FIFTHS?",6,"Six fifths.")
        ],
        [L0411DivisionLogic] =
        [
            new("14/15÷7/5=2/3. 1=yes,0=no.",1,"14/15×5/7=2/3."),
            new("For14/15÷7/5, multiply14/15 by5 and then by1/7; valid. 1=yes,0=no.",1,"Original option A."),
            new("For14/15÷7/5, multiply14/15 by5 then divide7; valid. 1=yes,0=no.",1,"Original option D."),
            new("For14/15÷7/5, multiply14/15 by7 then divide5; valid. 1=yes,0=no.",0,"Incorrect reciprocal."),
            new("Clare4/3÷5/2=10/3 is correct. 1=yes,0=no.",0,"Correct8/15."),
            new("Dividing4/3 by5/2 means multiply4/3 by2/5. 1=yes,0=no.",1,"Reciprocal algorithm."),
            new("15/4 divided by5/8 equals6. 1=yes,0=no.",1,"15/4×8/5=6."),
            new("2¾lb divided into4⅛ equal bags yields2/3lb per bag. 1=yes,0=no.",1,"11/4÷33/8=2/3."),
            new("4½ divided by1/8 equals36. 1=yes,0=no.",1,"9/2×8=36."),
            new("At2/5 charge in1/3hour, constant rate full charge time is5/6hour. 1=yes,0=no.",1,"1/3÷2/5=5/6."),
            new("For phone fraction charged2/5 in1/3hour, 2/5×?=1/3 is a valid equation for full time. 1=yes,0=no.",1,"Unknown time5/6."),
            new("For phone fraction charged2/5 in1/3hour, 2/5÷1/3 directly gives full time. 1=yes,0=no.",0,"It gives fraction per hour6/5."),
            new("2/5 of a full bucket weighs2.5lb, so full bucket water weighs6.25lb. 1=yes,0=no.",1,"2.5÷.4=6.25."),
            new("Multiplying by a fraction's reciprocal works only when the divisor is nonzero. 1=yes,0=no.",1,"Zero denominator not allowed.")
        ],
        [L0412LengthValues] =
        [
            new("One inch is about2 11/20cm. Three inches in TWENTIETHS of a centimeter?",153,"3×51/20=153/20cm."),
            new("One inch is about51/20cm. One centimeter as fraction of inch in FIFTY-FIRSTS?",20,"1÷51/20=20/51in."),
            new("Ten cm divided by2 11/20cm per inch: inches in FIFTY-FIRSTS?",200,"10×20/51=200/51in."),
            new("Zookeeper6¼ft, giraffe9⅜ft. Giraffe height compared with zookeeper in HALF-UNITS?",3,"(75/8)/(25/4)=3/2."),
            new("Zookeeper6¼ft, giraffe9⅜ft. Zookeeper height as fraction of giraffe in THIRDS?",2,"(25/4)/(75/8)=2/3."),
            new("A rectangular tile is1½ft long, bathroom length10½ft. Number of tiles along length?",7,"10.5/1.5=7."),
            new("Tile span1½ft, floor width6½ft. Number of TILE SPANS in THIRDS?",13,"6.5/1.5=13/3 tile lengths."),
            new("Floor width6½ft and tile side1½ft. Minimum WHOLE tiles across row if pieces may not combine?",5,"Need cover4⅓ tile spans, round up5 tiles."),
            new("In3/4cup oatmeal there is1/10 daily iron. Daily iron fraction per1cup in FIFTEENTHS?",2,"(1/10)/(3/4)=2/15."),
            new("What fraction of1/2 is1/3? Answer in THIRDS?",2,"(1/3)/(1/2)=2/3."),
            new("How many groups of4/5 are in2? Answer in HALF-GROUP UNITS?",5,"2÷4/5=5/2 groups."),
            new("The zookeeper6¼ft tall expressed in QUARTER-FOOT portions?",25,"25/4feet."),
            new("Giraffe9⅜ft expressed in EIGHTH-FOOT portions?",75,"75/8feet."),
            new("3 inches at2.55cm per inch, in HUNDREDTHS of centimeter?",765,"3×2.55=7.65cm."),
            new("A floor is10½ft long with tiles1½ft each. Total feet of7 tile lengths in HALVES?",21,"7×1.5=10.5ft=21 halves."),
            new("One cup iron fraction2/15. Three cups iron fraction in FIFTHS?",2,"3×2/15=2/5."),
            new("A box6 feet high compared with a1½-foot item. Multiplicative ratio?",4,"6/1.5=4.")
        ],
        [L0412LengthReasoning] =
        [
            new("A giraffe9⅜ft is one and a half times6¼ft. 1=yes,0=no.",1,"9.375/6.25=1.5."),
            new("The zookeeper6¼ft is two-thirds as tall as the giraffe9⅜ft. 1=yes,0=no.",1,"6.25/9.375=2/3."),
            new("Three inches at2.55cm per inch are7.65cm. 1=yes,0=no.",1,"3×2.55."),
            new("One centimeter is20/51 of an inch given51/20cm per inch. 1=yes,0=no.",1,"Reciprocal."),
            new("Seven tiles of1.5ft span exactly10.5ft. 1=yes,0=no.",1,"7×1.5=10.5."),
            new("Six and a half feet contains exactly4 whole1.5ft spans with no remainder. 1=yes,0=no.",0,"There are4⅓ spans."),
            new("Across6.5ft using1.5ft squares, at least5 tile-length positions are required if no reused cuts. 1=yes,0=no.",1,"Round up from4⅓."),
            new("Three quarters cup containing1/10 daily iron implies1cup contains2/15. 1=yes,0=no.",1,"1/10÷3/4=2/15."),
            new("One third is two thirds of one half. 1=yes,0=no.",1,"1/3÷1/2=2/3."),
            new("Two contains two and a half groups of4/5. 1=yes,0=no.",1,"2÷4/5=2.5."),
            new("Comparison questions 'how many times as tall' can be modeled by dividing heights in same units. 1=yes,0=no.",1,"Multiplicative comparison."),
            new("A number of tile-span lengths always equals the integer number of physical uncut tiles purchased. 1=yes,0=no.",0,"Fractional spans and cuts differ."),
            new("A source ruler with approximate inch-centimeter factor should be marked approximate, not an exact standard. 1=yes,0=no.",1,"2.55 is approximate."),
            new("Original source diagrams can be evaluated using a numeric quotient only without checking lengths. 1=yes,0=no.",0,"Teacher checks representations.")
        ],
        [L0413AreaValues] =
        [
            new("Rectangle area11square meters, known side3⅔meters. Other side in METERS?",3,"11÷11/3=3."),
            new("TV area462square inches and length16½in. Width in INCHES?",28,"462÷16.5=28."),
            new("Rectangle area17½square inches, shorter side3½in. Longer side INCHES?",5,"17.5÷3.5=5."),
            new("A floor is12ft by15ft. Total area in SQUARE FEET?",180,"12×15=180."),
            new("Square tile side1⅓ft. Its area in NINTHS OF SQUARE FOOT?",16,"(4/3)²=16/9."),
            new("Floor length12ft with tile edge1⅓ft. Number of complete tile lengths?",9,"12÷4/3=9."),
            new("Floor width15ft with tile edge1⅓ft. Number of tile-edge spans in QUARTERS?",45,"15÷4/3=45/4."),
            new("42inch bookshelf with1½inch books fits how many BOOKS?",28,"42÷1.5=28."),
            new("Five shelves each42in long. Total shelf space in INCHES?",210,"5×42=210."),
            new("Five shelves each42in long. Total shelf space in HALF-FEET?",35,"210in=17.5feet=35 half-feet."),
            new("5/32 divided by25/4, quotient in FORTIETHS?",1,"(5/32)×(4/25)=1/40."),
            new("How many1⅔ groups in1⅚? Answer in TENTHS?",11,"11/6÷5/3=11/10."),
            new("How many1⅔ groups in4⅓? Answer in FIFTHS?",13,"13/3÷5/3=13/5."),
            new("How many1⅔ groups in5/6? Answer in HALVES?",1,"5/6÷5/3=1/2."),
            new("A hose fills3gallons in1¼minutes. Seconds to fill50gallons at same rate?",1250,"50×(5/4)/3×60=1250 seconds."),
            new("A rectangle3m by3⅔m has area in SQUARE METERS?",11,"3×11/3=11."),
            new("TV width28in and length16½in. Area in SQUARE INCHES?",462,"28×16.5=462."),
            new("Original area11m² with one side11/3m. Unknown side in THIRD-METER units?",9,"3meters=9 thirds.")
        ],
        [L0413AreaLogic] =
        [
            new("A rectangle of area11m² and side3⅔m has other side3m. 1=yes,0=no.",1,"11÷11/3=3."),
            new("TV equation16½×w=462 can represent its area. 1=yes,0=no.",1,"Original option B."),
            new("TV equation462÷16½=w can represent its missing width. 1=yes,0=no.",1,"Original option C."),
            new("TV equation462÷w=16½ can represent its length. 1=yes,0=no.",1,"Original option D."),
            new("TV equation w×462=16½ correctly relates original dimensions. 1=yes,0=no.",0,"Original option A invalid."),
            new("A rectangular area17½in² divided by side3½in gives other side5in. 1=yes,0=no.",1,"17.5/3.5=5."),
            new("A square with side1⅓ft has area16/9 square feet. 1=yes,0=no.",1,"4/3 squared."),
            new("A12ft by15ft floor has180sqft area. 1=yes,0=no.",1,"12×15=180."),
            new("The fraction101¼ tile-areas can itself be purchased as101¼ whole uncut physical tiles without cutting. 1=yes,0=no.",0,"Geometric area units differ from purchasing constraints."),
            new("A floor width15ft requires11¼ tile-width spans of1⅓ft each. 1=yes,0=no.",1,"15÷4/3=45/4."),
            new("Five shelves each42in contain17½feet of total length. 1=yes,0=no.",1,"210in/12=17.5ft."),
            new("The dimensions of a rectangle should share units before calculating the product area. 1=yes,0=no.",1,"Convert lengths consistently."),
            new("A diagram with a fractional side can be verified by multiply side lengths to get area. 1=yes,0=no.",1,"Inverse operations."),
            new("Teacher inspection of the actual tile layout is needed before claiming an exact purchase count. 1=yes,0=no.",1,"Cuts and reuse assumptions matter.")
        ],
        [L0414GeometryNumbers] =
        [
            new("A larger cube has4inch sides and is built of1/2inch cubes. Number of cubes along one edge?",8,"4/(1/2)=8."),
            new("A larger cube has4inch sides and is built of1/2inch cubes. Total small cubes?",512,"8³=512."),
            new("Triangle area7⅞cm² and base5¼cm. Height in CENTIMETERS?",3,"2×(63/8)/(21/4)=3."),
            new("Prism1½in by2in by2½in. Volume in HALF-CUBIC-INCH UNITS?",15,"1.5×2×2.5=7.5in³=15 halves."),
            new("Prism1½in by2in by2½in, packed with 1/2inch cubes. Number of small cubes?",60,"3×4×5=60 cubes."),
            new("One cube of side1/2inch. Volume in EIGHTH-CUBIC-INCH UNITS?",1,"(1/2)³=1/8in³."),
            new("A prism5×5×8 units. Its volume in CUBIC UNITS?",200,"5×5×8=200."),
            new("Prism5×5×8 packed with1/3-unit cubes. Number of little cubes?",5400,"15×15×24=5400."),
            new("Fence boards6¼in wide for150in fence with no gaps. Number of boards?",24,"150÷25/4=24."),
            new("Two and1/7 divided by2/7, answer in HALF-GROUPS?",15,"15/7÷2/7=15/2."),
            new("17/20 divided by1/4, answer in FIFTHS?",17,"17/20×4=17/5."),
            new("Bucket holds11⅔ gallons and is5/6 full. Full capacity in GALLONS?",14,"35/3÷5/6=14."),
            new("Gym has80 children,75% wearing socks. How many wearing socks?",60,"80×.75=60."),
            new("Gym has80 children,75% wearing socks. How many NOT wearing socks?",20,"80×.25=20."),
            new("Lin wants75 dollars and saved37.50. Percent saved?",50,"37.5/75=50%."),
            new("Noah wants60dollars and saved45. Percent saved?",75,"45/60=75%."),
            new("Lin saved50% of trip goal. Percent remaining?",50,"100-50=50%."),
            new("Noah saved75% of concert ticket goal. Percent remaining?",25,"100-75=25%."),
            new("Triangle with base5¼cm and height3cm has area in EIGHTH-SQUARE-CM units?",63,"5.25×3÷2=7.875=63/8."),
            new("A4-inch cube volume in CUBIC INCHES?",64,"4³=64.")
        ],
        [L0414GeometryLogic] =
        [
            new("A cube of side4in contains8 half-inch edges along each side. 1=yes,0=no.",1,"4÷0.5=8."),
            new("A cube of side4in contains512 smaller half-inch cubes. 1=yes,0=no.",1,"8³=512."),
            new("Triangle area7⅞cm² base5¼cm has perpendicular height3cm. 1=yes,0=no.",1,"A=bh/2."),
            new("A slanted triangle side can automatically be used as height even when not perpendicular to base. 1=yes,0=no.",0,"Height is perpendicular."),
            new("A prism1.5×2×2.5in has volume7.5 cubic inches. 1=yes,0=no.",1,"Multiply three edges."),
            new("A half-inch cube volume equals1/2 cubic inch. 1=yes,0=no.",0,"Cube volume1/8."),
            new("A1.5×2×2.5in prism contains60 half-inch cubes. 1=yes,0=no.",1,"3×4×5."),
            new("Five by five by eight prism can contain5400 cubes with edge1/3unit. 1=yes,0=no.",1,"15×15×24."),
            new("Multiplying whole-prism volume200 by27 gives count of1/3-unit cubes. 1=yes,0=no.",1,"Each tiny cube volume1/27."),
            new("A150inch fence with6.25inch boards and no gaps needs24 boards. 1=yes,0=no.",1,"150/6.25=24."),
            new("A bucket11⅔gallons at5/6 full has capacity14 gallons. 1=yes,0=no.",1,"11⅔÷5/6=14."),
            new("75% of80 kids are wearing socks so20 are not. 1=yes,0=no.",1,"25%×80=20."),
            new("Volume uses cubic units, not squared units. 1=yes,0=no.",1,"Three dimensions."),
            new("The small cube count equals the ratio of large volume to small cube volume. 1=yes,0=no.",1,"Volume quotient.")
        ],
        [L0415Volumes] =
        [
            new("Pool24ft by15ft, water height1⅓ft. Volume in CUBIC FEET?",480,"24×15×4/3=480."),
            new("Prism2⅖in×3⅕in×2in. Volume in TWENTY-FIFTHS of a cubic inch?",384,"12/5×16/5×2=384/25."),
            new("Prism2⅖in×3⅕in×2in. Count of1/5inch cubes along first edge?",12,"(12/5)/(1/5)=12."),
            new("Prism2⅖in×3⅕in×2in. Count1/5in cubes along second edge?",16,"(16/5)/(1/5)=16."),
            new("Prism2⅖in×3⅕in×2in. Count1/5in cubes along third edge?",10,"2/(1/5)=10."),
            new("Prism2⅖in×3⅕in×2in. Total count1/5inch unit cubes?",1920,"12×16×10=1920."),
            new("Prism2⅖in×3⅕in×2in. Total count2/5inch cubes?",240,"6×8×5=240."),
            new("A1/5inch cube has volume in ONE-HUNDRED-TWENTY-FIFTH cubic-inch units?",1,"1/5³=1/125."),
            new("A right triangle legs3/4 and1 unit. Area in EIGHTHS of sq unit?",3,"3/4×1÷2=3/8."),
            new("Right triangle area3/8 and hypotenuse base5/4, perpendicular height in FIFTHS?",3,"2×3/8÷5/4=3/5."),
            new("Salt lick cube edge5/12ft. Volume in 1728THS cubic feet?",125,"5³/12³=125/1728."),
            new("Salt lick packing box1¼ft×1⅔ft×5/6ft. Cubes along first edge of5/12ft?",3,"5/4÷5/12=3."),
            new("Salt lick packing box1¼ft×1⅔ft×5/6ft. Cubes along second edge?",4,"5/3÷5/12=4."),
            new("Salt lick packing box1¼ft×1⅔ft×5/6ft. Cubes along third edge?",2,"5/6÷5/12=2."),
            new("Salt lick packing box1¼ft×1⅔ft×5/6ft. Total count of5/12ft cubes?",24,"3×4×2=24."),
            new("How many groups of1/3inch fit in3/4inch? Answer in QUARTER-GROUP UNITS?",9,"3/4÷1/3=9/4."),
            new("One and2/5 groups of1⅔in. Length in THIRDS of inch?",7,"7/5×5/3=7/3in."),
            new("Art paste uses1cup flour per1/2cup water. Water cups for4flour?",2,"4×1/2=2."),
            new("Art paste1cup flour per1/2cup water. Flour cups with3cups water?",6,"3×2=6.")
        ],
        [L0415VolumeLogic] =
        [
            new("Pool24×15×1⅓feet contains480 cubic feet at given water height. 1=yes,0=no.",1,"24×15×4/3=480."),
            new("A prism2⅖×3⅕×2in has volume384/25in³. 1=yes,0=no.",1,"Multiply edge lengths."),
            new("A prism packed with1/5in cubes has1920 of them under stated dimensions. 1=yes,0=no.",1,"12×16×10."),
            new("A prism packed with2/5in cubes has240 of them under stated dimensions. 1=yes,0=no.",1,"6×8×5."),
            new("Larger2/5in cubes require MORE cubes than smaller1/5in cubes to fill same prism. 1=yes,0=no.",0,"Larger cubes take more volume each."),
            new("A1/5in unit cube has volume1/5 cubic inch. 1=yes,0=no.",0,"Volume1/125."),
            new("A right triangle with perpendicular legs3/4 and1 has area3/8 sq unit. 1=yes,0=no.",1,"Multiply legs÷2."),
            new("If triangle area3/8 and new base5/4, its perpendicular altitude is3/5. 1=yes,0=no.",1,"2A/b=3/5."),
            new("Salt lick cube side5/12ft has volume smaller than1 cubic foot. 1=yes,0=no.",1,"125/1728<1."),
            new("Salt lick shipping box with3×4×2 cube counts can hold24 equal cubes. 1=yes,0=no.",1,"Multiply fit counts."),
            new("A fractional prism volume requires expressing lengths in a common measurement unit. 1=yes,0=no.",1,"Unit consistency."),
            new("Cubic inches and square inches are interchangeable units when finding a prism's volume. 1=yes,0=no.",0,"Different dimensions."),
            new("One and two-fifths groups of1⅔in is7/3in. 1=yes,0=no.",1,"7/5×5/3=7/3."),
            new("1cup flour to1/2cup water means4cups flour require2cups water. 1=yes,0=no.",1,"Scale both×4.")
        ],
        [L0416AllOperations] =
        [
            new("Each orange yields1/4cup juice. Number of oranges needed for2½cups?",10,"2.5÷.25=10."),
            new("Hiking sign shows3/4mile back to parking and1½miles ahead to summit. Total hike in QUARTER-MILE UNITS?",9,"3/4+6/4=9/4=2.25mi."),
            new("Hiking sign3/4mi back1½mi ahead. Fraction of full hike already traveled in THIRDS?",1,"(3/4)/(9/4)=1/3."),
            new("Hiking sign3/4mi back1½mi ahead. Remaining as a MULTIPLE of distance walked?",2,"1.5÷0.75=2."),
            new("Hiking total2¼mi compared with3/4 already walked. Whole as multiple of walked?",3,"2.25÷.75=3."),
            new("Priya cat5½lb, dog8¼lb. Cat as fraction of dog in THIRDS?",2,"(11/2)/(33/4)=2/3."),
            new("Priya cat5½lb, dog8¼lb. Combined weight in QUARTER-POUND UNITS?",55,"11/2+33/4=55/4."),
            new("Priya cat5½lb, dog8¼lb. Dog heavier in QUARTER-POUND UNITS?",11,"33/4-22/4=11/4."),
            new("Ice box7½ft×6ft×6ft with cubes1½ft per edge. Cubes along first edge?",5,"7.5÷1.5=5."),
            new("Ice box7½ft×6ft×6ft with cubes1½ft per edge. Cubes along second edge?",4,"6÷1.5=4."),
            new("Ice box7½ft×6ft×6ft with cubes1½ft edge. Total cubes?",80,"5×4×4=80."),
            new("School club sold300shirts,31% to fifth graders. Number?",93,"300×.31=93."),
            new("School club sold300shirts,52% to sixth graders. Number?",156,"300×.52=156."),
            new("School club sold300shirts, remaining17% to teachers. Number?",51,"300×.17=51."),
            new("Jada pennies:dimes2:3, total55coins. Pennies?",22,"55÷5=11 per part;2×11=22."),
            new("Jada pennies:dimes2:3, total55coins. Dimes?",33,"3×11=33."),
            new("Jada22pennies33dimes. Total VALUE in CENTS?",352,"22×1+33×10=352."),
            new("Ratio of pennies:dimes2:3 with no total. Can exact coin counts be determined? Enter1 yes,0 no.",0,"Scale factor unknown."),
            new("Ten oranges supply how many QUARTER-CUP portions of juice?",10,"10×1/4cup=2.5cups."),
            new("Cubic ice blocks of edge1½ft have volume in EIGHTH-CUBIC-FT UNITS?",27,"(3/2)³=27/8ft³.")
        ],
        [L0416OperationsLogic] =
        [
            new("2½cups orange juice at1/4cup per orange requires10 oranges. 1=yes,0=no.",1,"Divide target by per-orange amount."),
            new("The equation?×1/4=2½ models required oranges. 1=yes,0=no.",1,"Original option A."),
            new("The equation2½÷1/4=? models required oranges. 1=yes,0=no.",1,"Original option D."),
            new("The equation1/4÷2½=? models required oranges. 1=yes,0=no.",0,"Wrong direction."),
            new("At hiking sign3/4mi back to lot and1½mi ahead to summit, distance already walked is1/3 of whole. 1=yes,0=no.",1,"3/4 of9/4 total."),
            new("At hiking sign, distance remaining is twice the already walked distance. 1=yes,0=no.",1,"1.5/.75=2."),
            new("At hiking sign, total hike is triple distance already walked. 1=yes,0=no.",1,"2.25/.75=3."),
            new("Cat5½lb is2/3 as heavy as dog8¼lb. 1=yes,0=no.",1,"5.5/8.25=2/3."),
            new("Dog8¼lb is2¾lb heavier than cat5½lb. 1=yes,0=no.",1,"8.25-5.5=2.75."),
            new("An ice box7½×6×6ft can hold80 cubes of1½ft edges with no gaps. 1=yes,0=no.",1,"5×4×4."),
            new("300shirts 31% fifth+52% sixth leaves17% teachers. 1=yes,0=no.",1,"100-83=17%."),
            new("From pennies:dimes2:3 alone, exact coin counts can be determined without a total. 1=yes,0=no.",0,"Only proportion, not scale."),
            new("With55coins at ratio2:3, Jada has22pennies and33dimes. 1=yes,0=no.",1,"11 per ratio unit."),
            new("The value of22pennies and33dimes is3 dollars52 cents. 1=yes,0=no.",1,"22+330=352cents."),
            new("Choice of fraction operation depends on whether total, difference, rate or number of groups is unknown. 1=yes,0=no.",1,"Meaning drives operation.")
        ],
        [L0417BoundedPacking] =
        [
            new("An artist must ship270 jewelry boxes. SMALL shipping box can fit3×3×2 boxes under specified orientation. Capacity per carton?",18,"3×3×2=18."),
            new("A SMALL carton fits18 jewelry boxes. Cartons needed for270 jewelry boxes?",15,"270/18=15."),
            new("SMALL cartons each cost680cents. Shipping270 items using15 cartons costs how many CENTS?",10200,"15×680=10200cents."),
            new("MEDIUM 1 orientation fits6×11×2 jewelry boxes. Capacity?",132,"6×11×2=132."),
            new("MEDIUM 1 cartons fit132 jewelry boxes each. How many cartons for270?",3,"Ceiling270/132=3."),
            new("Three MEDIUM 1 shipping cartons at1345cents each cost CENTS?",4035,"3×1345=4035."),
            new("MEDIUM 2 orientation fits6×4×6 jewelry boxes. Capacity?",144,"6×4×6=144."),
            new("MEDIUM 2 cartons fit144 per carton. How many needed for270?",2,"Ceiling270/144=2."),
            new("Two MEDIUM 2 cartons at1345cents each cost CENTS?",2690,"2×1345=2690."),
            new("LARGE carton chosen orientation fits16×5×3 jewelry boxes. Capacity?",240,"16×5×3=240."),
            new("LARGE cartons fit240 each. How many needed for270?",2,"Ceiling270/240=2."),
            new("Two LARGE shipping cartons at1875cents each cost CENTS?",3750,"2×1875=3750."),
            new("Two MEDIUM 2 cartons can hold how many jewelry boxes with144-per-carton packing?",288,"2×144=288."),
            new("If two MEDIUM 2 cartons hold288 in total but only270 ordered, unused item SLOTS?",18,"288-270=18 slots."),
            new("One MEDIUM 2 carton fits144 and one MEDIUM 1 fits132. Combined capacity?",276,"144+132=276."),
            new("One MEDIUM 2 and one MEDIUM 1 at1345cents each cost CENTS?",2690,"2×1345=2690."),
            new("One LARGE carton fits240, one MEDIUM 1 fits132. Combined capacity?",372,"240+132=372."),
            new("One LARGE and one MEDIUM 1 cost CENTS at1875 and1345cents?",3220,"1875+1345=3220."),
            new("A jewelry box has edge lengths1¾in,2¼in,¾in. Volume in SIXTY-FOURTHS OF CUBIC INCH?",189,"7/4×9/4×3/4=189/64in³."),
            new("Small box5⅜in along a1¾in jewelry-box edge. Maximum full boxes along this axis?",3,"5.375/1.75=3.071, floor3."),
            new("Small box8⅝in along a2¼in jewelry-box edge. Maximum full boxes along this axis?",3,"8.625/2.25=3.833, floor3."),
            new("Small box1⅝in along a¾in jewelry-box edge. Maximum full boxes along this axis?",2,"1.625/.75=2.166, floor2."),
            new("Large box12in edge along¾in jewelry-box edge. Count along that axis?",16,"12/.75=16."),
            new("Large box12in edge along2¼in jewelry-box edge. Count along that axis?",5,"12/2.25=5.333, floor5."),
            new("Large box5½in edge along1¾in jewelry-box edge. Count along that axis?",3,"5.5/1.75=3.14, floor3.")
        ],
        [L0417PackingModelLogic] =
        [
            new("Under one fixed orientation, per-axis full-box counts can be multiplied to get a rectangular grid capacity. 1=yes,0=no.",1,"Product of three integer packing counts."),
            new("Jewelry boxes may have different packing capacities when rotated. 1=yes,0=no.",1,"Different edge alignment."),
            new("A capacity of144 per carton can ship270 items using just1 carton. 1=yes,0=no.",0,"Need2."),
            new("Two cartons each144 capacity can hold288 jewelry boxes. 1=yes,0=no.",1,"2×144=288."),
            new("At the historical source classroom prices, two medium2 cartons cost26.90 dollars. 1=yes,0=no.",1,"2×13.45=26.90."),
            new("Two medium2 cartons at historical prices are cheaper than two large cartons at18.75 each. 1=yes,0=no.",1,"26.90<37.50."),
            new("A chosen fixed orientation costing26.90 dollars proves global optimality among all mixed packing schemes and live USPS rates. 1=yes,0=no.",0,"This is a bounded model, not complete optimization or current pricing."),
            new("Small box packing counts3×3×2 imply18 jewelry boxes per carton. 1=yes,0=no.",1,"Product18."),
            new("Two medium1 cartons each132 would hold270 jewelry boxes. 1=yes,0=no.",0,"264<270; need3."),
            new("One medium2 capacity144 and one medium1 capacity132 total276, enough for270. 1=yes,0=no.",1,"276≥270."),
            new("Packaging material thickness, damaged capacity and real postal restrictions may change a physical shipping decision. 1=yes,0=no.",1,"Assumptions matter."),
            new("The source's historical flat-rate prices should be treated as verified current USPS prices in2026. 1=yes,0=no.",0,"Historical classroom dataset, not live prices."),
            new("A student-created orientation drawing needs human assessment even when a per-axis multiplication is correct. 1=yes,0=no.",1,"Actual packing feasibility must be reviewed."),
            new("For270 boxes with carton capacity144, the count is rounded UP to2 cartons. 1=yes,0=no.",1,"Ceiling, not rounding to nearest."),
            new("The cheapest plan among only four uniform-orientation illustrated methods may not be the cheapest unconstrained plan. 1=yes,0=no.",1,"Mixed orientations and cartons may change optimum.")
        ],
        [L0501DecimalArithmetic] =
        [
            new("Mai has1450 cents, spends435 cents at snack bar and525 at arcade. Amount LEFT in CENTS?",490,"1450-435-525=490 cents."),
            new("A large pizza costs750 cents. With4000 cents, maximum number of WHOLE pizzas?",5,"4000/750=5 and remainder250 cents."),
            new("Five pizzas at750cents each cost how many CENTS?",3750,"5×750=3750."),
            new("Adults tickets550cents, students425cents. Two adult tickets in CENTS?",1100,"2×550=1100."),
            new("Three student tickets425cents each cost in CENTS?",1275,"3×425=1275."),
            new("Two adults at550cents plus3 students at425cents cost in CENTS?",2375,"1100+1275=2375."),
            new("Family paid2500cents for2375cents tickets. Change in CENTS?",125,"2500-2375=125."),
            new("Chicken costs320cents/lb. Three pounds cost in CENTS?",960,"320×3=960."),
            new("Beef costs459cents/lb. Three pounds cost in CENTS?",1377,"459×3=1377."),
            new("Three pounds of beef versus chicken; difference in CENTS?",417,"1377-960=417."),
            new("A bottle holds1½L, glass holds1/5L. How many glasses in HALF-GLASS UNITS?",15,"3/2÷1/5=15/2 glasses."),
            new("A jug holds16L, bottle1½L. Number of bottles in THIRD-BOTTLE UNITS?",32,"16÷3/2=32/3 bottles."),
            new("Triangle base5½ units area19¼ units². Height in UNITS?",7,"2×19.25/5.5=7."),
            new("Five sixths divided by1/6 equals?",5,"5/6×6=5."),
            new("One and1/6 divided by1/12 equals?",14,"7/6×12=14."),
            new("10/6 divided by1/24 equals?",40,"10/6×24=40."),
            new("A chicken price320cents/lb. Price per pound in DOLLAR-CENTS?",320,"Money amount in cents is320."),
            new("Mai has1450cents and spends960cents total. Remainder CENTS?",490,"1450-960=490.")
        ],
        [L0501DecimalEstimation] =
        [
            new("Mai spent4.35+5.25=9.60 dollars. 1=yes,0=no.",1,"435+525=960cents."),
            new("Mai has4.90 dollars left from14.50 after spending4.35 and5.25. 1=yes,0=no.",1,"14.50-9.60=4.90."),
            new("40 dollars buys6 whole pizzas priced7.50 dollars each. 1=yes,0=no.",0,"6×7.50=45>40."),
            new("40 dollars buys5 pizzas at7.50 dollars each. 1=yes,0=no.",1,"5×7.50=37.50."),
            new("Two adult tickets and3 student tickets cost23.75 dollars. 1=yes,0=no.",1,"11+12.75=23.75."),
            new("When ticket cost23.75 is paid with25 dollars, change is1.25. 1=yes,0=no.",1,"25-23.75=1.25."),
            new("Three pounds of beef priced4.59/lb cost13.77 dollars. 1=yes,0=no.",1,"3×4.59=13.77."),
            new("Three pounds of chicken priced3.20/lb cost9.60 dollars. 1=yes,0=no.",1,"3×3.20=9.60."),
            new("Three pounds of beef cost4.17 dollars more than chicken at source prices. 1=yes,0=no.",1,"13.77-9.60=4.17."),
            new("An estimate can help reject an impossible total before exact decimal calculation. 1=yes,0=no.",1,"Benchmark estimate validates reasonableness."),
            new("Money units must be consistent when combining dollars and cents. 1=yes,0=no.",1,"Convert to same unit."),
            new("1½L bottle fills7½ one-fifth-liter glasses. 1=yes,0=no.",1,"1.5/.2=7.5."),
            new("Two triangles with same base5½ and perpendicular height7 have area19¼. 1=yes,0=no.",1,"5.5×7÷2=19.25."),
            new("Adding decimal prices with misaligned decimal places may produce a different numerical result. 1=yes,0=no.",1,"Place value matters.")
        ],
        [L0502PlaceValueArithmetic] =
        [
            new("0.137 +0.284 expressed in THOUSANDTHS?",421,"137+284=421 thousandths."),
            new("3.25 +1 expressed in HUNDREDTHS?",425,"325+100=425 hundredths."),
            new("0.5+1.15 expressed in HUNDREDTHS?",165,"50+115=165."),
            new("10.6+1.7 expressed in TENTHS?",123,"106+17=123 tenths."),
            new("123+0.2 expressed in TENTHS?",1232,"1230+2=1232."),
            new("0.216 expressed as number of THOUSANDTHS?",216,"2tenths+1hundredth+6thousandths=216."),
            new("0.304 expressed as number of THOUSANDTHS?",304,"3tenths+4thousandths=304."),
            new("In0.137 there are how many THOUSANDTHS altogether?",137,"0.137=137 thousandths."),
            new("In0.284 there are how many THOUSANDTHS altogether?",284,"0.284=284 thousandths."),
            new("0.137+0.284 includes how many HUNDREDTHS beyond regrouping? Enter whole hundredths in0.421.",42,"0.421=42hundredths plus1thousandth."),
            new("Andre completes135 facts in90sec. Facts per second in HALF-FACT units?",3,"135/90=1.5facts/s=3half-facts."),
            new("Noah completes75 facts in60sec. Facts per second in QUARTER-FACT units?",5,"75/60=1.25facts/s=5quarter-facts."),
            new("Andre1.5facts/s and Noah1.25facts/s. Enter1 Andre faster,2 Noah faster.",1,"1.5>1.25."),
            new("Andre and Noah rate gap in QUARTER-FACTS per second?",1,"1.5-1.25=.25."),
            new("The value0.137+0.284 expressed in TEN-THOUSANDTHS?",4210,"0.421=4210 ten-thousandths."),
            new("The sum0.5+1.15 expressed in THOUSANDTHS?",1650,"1.65=1650thousandths."),
            new("The difference1.15 minus0.5 in HUNDREDTHS?",65,"115-50=65."),
            new("If ten thousandths combine, how many HUNDREDTHS?",1,"10 thousandths=1 hundredth.")
        ],
        [L0502PlaceValueLogic] =
        [
            new("0.137+0.284=0.421. 1=yes,0=no.",1,"137+284=421thousandths."),
            new("0.137+0.284=0.311. 1=yes,0=no.",0,"Requires regrouping hundredths."),
            new("3.25+1=4.25. 1=yes,0=no.",1,"Align ones."),
            new("0.5+1.15=1.65. 1=yes,0=no.",1,"0.50+1.15."),
            new("10.6+1.7=12.3. 1=yes,0=no.",1,"106tenths+17tenths."),
            new("123+0.2=123.2. 1=yes,0=no.",1,"Place-value alignment."),
            new("Ten thousandths equal one hundredth. 1=yes,0=no.",1,"10×.001=.01."),
            new("Twelve hundredths can be regrouped as one tenth and2 hundredths. 1=yes,0=no.",1,"0.12=0.1+0.02."),
            new("One can add a ones-place digit directly to a tenths-place digit without converting units. 1=yes,0=no.",0,"Convert or align same units."),
            new("0.5 is numerically equal to0.50. 1=yes,0=no.",1,"Trailing zeros don't change value."),
            new("Andre135facts/90sec is faster than Noah75facts/60sec. 1=yes,0=no.",1,"1.5 vs1.25facts/s."),
            new("A physical base-ten block representation can show same decimal using different regroupings. 1=yes,0=no.",1,"E.g.10 thousandths=hundredth."),
            new("A decimal point should be aligned in vertical addition to match place value. 1=yes,0=no.",1,"Column unit alignment."),
            new("Teacher should verify blocks and visual digit placement separately from a numeric sum. 1=yes,0=no.",1,"Drawing rubric.")
        ],
        [L0503DecimalCalculations] =
        [
            new("1.13 minus0.46, answer in HUNDREDTHS?",67,"113-46=67 hundredths."),
            new("0.027+0.004 in THOUSANDTHS?",31,"27+4=31 thousandths."),
            new("0.203+0.01 in THOUSANDTHS?",213,"203+10=213."),
            new("1.2+0.145 in THOUSANDTHS?",1345,"1200+145=1345."),
            new("20-1.97 in HUNDREDTHS?",1803,"2000-197=1803."),
            new("0.3-0.006 in THOUSANDTHS?",294,"300-6=294."),
            new("14.6-1.4 in TENTHS?",132,"146-14=132."),
            new("38.60-6.75 in HUNDREDTHS?",3185,"3860-675=3185."),
            new("241.76-2.18 in HUNDREDTHS?",23958,"24176-218=23958."),
            new("Three pencils each30cents cost in CENTS?",90,"3×30=90."),
            new("A hat1450cents and2binders320cents each. Cost in CENTS?",2090,"1450+640=2090."),
            new("Three pencils30c each,hat1450c,2binders320c each. Total in CENTS?",2180,"90+1450+640=2180."),
            new("Pencils cost30c each; maximum whole pencils purchasable with500cents?",16,"500/30=16 remainder20."),
            new("Prism7½cm×12cm×15½cm. Volume in CUBIC CENTIMETERS?",1395,"7.5×12×15.5=1395."),
            new("Prism7½×12×15½cm packed with1/2cm cubes. Number of small cubes?",11160,"15×24×31=11160."),
            new("Car travels75miles in60minutes. Distance in18minutes in HALF-MILE units?",45,"75×18/60=22.5miles."),
            new("0.012-0.007 in THOUSANDTHS?",5,"12-7=5."),
            new("0.300-0.006 in THOUSANDTHS?",294,"300-6=294.")
        ],
        [L0503DecimalPrinciples] =
        [
            new("1.13-0.46=0.67. 1=yes,0=no.",1,"113-46=67 hundredths."),
            new("0.027+0.004=0.031. 1=yes,0=no.",1,"31 thousandths."),
            new("0.203+0.01=0.213. 1=yes,0=no.",1,"Align hundredths."),
            new("1.2+0.145=1.345. 1=yes,0=no.",1,"1.200+0.145."),
            new("20-1.97 cannot be calculated because20 has no decimal digits. 1=yes,0=no.",0,"20.00-1.97=18.03."),
            new("0.3-0.006=0.294. 1=yes,0=no.",1,"0.300-.006=.294."),
            new("0.3-0.006=0.024. 1=yes,0=no.",0,"Wrong place-value borrowing."),
            new("14.6-1.4=13.2. 1=yes,0=no.",1,"14.6-1.4=13.2."),
            new("38.60-6.75=31.85. 1=yes,0=no.",1,"Regroup hundredths/tenths."),
            new("241.76-2.18=239.58. 1=yes,0=no.",1,"Subtract using aligned decimal."),
            new("Three pencils+hat+two binders cost21.80 dollars and exceed20 dollars. 1=yes,0=no.",1,".90+14.50+6.40=21.80."),
            new("A 0.5cm cube volume is0.125cm³, so11160 of them pack1395cm³. 1=yes,0=no.",1,"11160×1/8=1395."),
            new("75miles in60minutes means22.5miles in18minutes at constant rate. 1=yes,0=no.",1,"75×18/60."),
            new("The same decimal number can be written with trailing zeros without changing its value. 1=yes,0=no.",1,"0.3=0.300.")
        ],
        [L0504PreciseArithmetic] =
        [
            new("7.2-3.67 expressed in HUNDREDTHS?",353,"7.20-3.67=3.53."),
            new("16-1.4 expressed in TENTHS?",146,"16.0-1.4=14.6."),
            new("1-0.1978 in TEN-THOUSANDTHS?",8022,"1.0000-.1978=.8022."),
            new("Chocolates label0.384lb, actual0.3798lb. Difference in TEN-THOUSANDTHS of pound?",42,"0.3840-.3798=.0042."),
            new("1.036 plus what decimal equals4? Unknown in THOUSANDTHS?",2964,"4-1.036=2.964."),
            new("0.738 plus what equals1? Unknown in THOUSANDTHS?",262,"1-.738=.262."),
            new("0.5137 plus what equals1? Unknown in TEN-THOUSANDTHS?",4863,"1-.5137=.4863."),
            new("Large cube10ft edge packs small cubes2½ft edge. Number along one edge?",4,"10/2.5=4."),
            new("Large cube10ft edge packs2½ft edge cubes. Number of crates?",64,"4³=64."),
            new("Chef bakes2 loaves for9 customers. Loaves for63 customers?",14,"63/9×2=14."),
            new("Chef bakes2 loaves for9 customers. Customers when making20 loaves?",90,"20/2×9=90."),
            new("Chef bakes2 loaves for9 customers. Fraction of loaf per customer in NINTHS?",2,"2/9 loaf per customer."),
            new("Six loaves at2 per9 customers feed how many customers?",27,"6/2×9=27."),
            new("0.3840-0.3798 equals how many MILLIONTHS of a pound?",4200,".0042=4200 millionths."),
            new("14.6-1.4 in HUNDREDTHS?",1320,"13.2=1320hundredths.")
        ],
        [L0504DecimalReasoning] =
        [
            new("7.2-3.67=3.53. 1=yes,0=no.",1,"7.20-3.67."),
            new("7.2-3.67=3.05. 1=yes,0=no.",0,"Misaligned place value."),
            new("16-1.4=14.6. 1=yes,0=no.",1,"16.0-1.4."),
            new("1-0.1978=0.8022. 1=yes,0=no.",1,"1.0000-.1978=.8022."),
            new("0.3840 is equal to0.384. 1=yes,0=no.",1,"Trailing zero."),
            new("0.3798lb chocolates are lighter than0.384lb label by0.0042lb. 1=yes,0=no.",1,"Subtract label minus actual."),
            new("0.3798lb are heavier than0.384lb. 1=yes,0=no.",0,"Actual less."),
            new("1.036+2.964=4. 1=yes,0=no.",1,"Regroup thousandths."),
            new("0.738+0.262=1. 1=yes,0=no.",1,"Sum1000thousandths."),
            new("0.5137+0.4863=1. 1=yes,0=no.",1,"Ten-thousandths complement."),
            new("2.5ft cubes along10ft edge fit4 each side. 1=yes,0=no.",1,"10/2.5=4."),
            new("Cube10ft edge packs64 cubes of2.5ft edge when aligned. 1=yes,0=no.",1,"4³=64."),
            new("63 customers at2 loaves per9 need14 loaves. 1=yes,0=no.",1,"63/9×2."),
            new("20 loaves at2 for9 customers correspond to90 customers. 1=yes,0=no.",1,"20/2×9."),
            new("In decimal subtraction, digits should be aligned by place value even when numbers have different displayed precision. 1=yes,0=no.",1,"Decimal points aligned.")
        ],
        [L0505DecimalProducts] =
        [
            new("122.1×1/100 in THOUSANDTHS?",1221,"122.1/100=1.221."),
            new("11.8×1/100 in TEN-THOUSANDTHS?",1180,"11.8/100=.118=1180 ten-thousandths."),
            new("1350.1×1/100 in THOUSANDTHS?",13501,"13.501."),
            new("1.704×1/100 in HUNDRED-THOUSANDTHS?",1704,"1.704/100=.01704, 1704 hundred-thousandths."),
            new("0.06×0.154 in HUNDRED-THOUSANDTHS?",924,".00924=924 hundred-thousandths."),
            new("0.01×0.02 in TEN-THOUSANDTHS?",2,".0002."),
            new("0.3×0.2 in HUNDREDTHS?",6,".06."),
            new("1.2×5 in TENTHS?",60,"6.0=60tenths."),
            new("0.9×1.1 in HUNDREDTHS?",99,".99."),
            new("1.5×2 in TENTHS?",30,"3.0."),
            new("0.0004×0.005 in MILLIONTHS?",2,".000002."),
            new("33.1+1.95 in HUNDREDTHS?",3505,"35.05."),
            new("1.075+27.105 in THOUSANDTHS?",28180,"28.180."),
            new("0.401+9.28 in THOUSANDTHS?",9681,"9.681."),
            new("13.2-1.78 in HUNDREDTHS?",1142,"11.42."),
            new("23.11-0.376 in THOUSANDTHS?",22734,"22.734."),
            new("0.9-0.245 in THOUSANDTHS?",655,"0.655."),
            new("An area diagram represents0.3×0.2 in HUNDREDTH-SQUARE-UNIT parts?",6,"6 of100 equal unit squares.")
        ],
        [L0505DecimalLogic] =
        [
            new("0.06×0.154=0.00924. 1=yes,0=no.",1,"6×154=924 scaled by1/100000."),
            new("0.06×0.154 can be written6×1/100×154×1/1000. 1=yes,0=no.",1,"Source option A."),
            new("0.06×0.154 can be written6×154×1/100000. 1=yes,0=no.",1,"Source option B."),
            new("0.06×0.154 can be written6×0.1×154×0.01. 1=yes,0=no.",0,"This equals.924, not.00924."),
            new("0.06×0.154 can be written6×154×0.00001. 1=yes,0=no.",1,"Source option D."),
            new("0.01×0.02=0.0002. 1=yes,0=no.",1,"1/100×2/100."),
            new("0.3×0.2=0.06. 1=yes,0=no.",1,"3/10×2/10."),
            new("0.9×1.1=0.99. 1=yes,0=no.",1,"9×11/100."),
            new("0.0004×0.005=0.000002. 1=yes,0=no.",1,"4×5×10^-7=2×10^-6."),
            new("Multiplying positive number by1/100 reduces its value to one hundredth. 1=yes,0=no.",1,"Shift place values by factor100."),
            new("Moving decimal points is a shortcut for scaling base-ten units, not a new arbitrary arithmetic rule. 1=yes,0=no.",1,"Place-value interpretation."),
            new("1.2×5=6. 1=yes,0=no.",1,"Six whole units."),
            new("A quadrilateral's drawn area must be graded using the actual polygon, not just a numeric result. 1=yes,0=no.",1,"Teacher geometry rubric."),
            new("The equivalent representations of decimal factors rely on associative and commutative multiplication. 1=yes,0=no.",1,"Regrouping factors.")
        ],
        [L0506Products] =
        [
            new("1.2×0.11 in THOUSANDTHS?",132,"0.132."),
            new("0.34×0.02 in TEN-THOUSANDTHS?",68,"0.0068."),
            new("120×0.002 in THOUSANDTHS?",240,"0.24."),
            new("0.3×0.5 in HUNDREDTHS?",15,"0.15."),
            new("Buffalo gasoline price per gallon in CENTS?",229,"Given $2.29."),
            new("Toronto gasoline91cents per liter,3.8 liters/gallon. Cost per gallon rounded to CENTS?",346,"91×3.8=345.8cents≈346."),
            new("Gas Toronto≈346cents/gal vs Buffalo229cents/gal. Difference in CENTS?",117,"346−229=117."),
            new("10.3+3.7 in TENTHS?",140,"14.0."),
            new("20.99−4.97 in HUNDREDTHS?",1602,"16.02."),
            new("15.99+23.51 in HUNDREDTHS?",3950,"39.50."),
            new("1.893−0.353 in THOUSANDTHS?",1540,"1.540."),
            new("49/50 divided by7/6, answer in TWENTY-FIFTHS?",21,"(49/50)×(6/7)=21/25."),
            new("1.05×2.8 in HUNDREDTHS?",294,"2.94."),
            new("0.0015×0.024 in MILLIONTHS?",36,"0.000036."),
            new("0.25×1.6 in TENTHS?",4,"0.4."),
            new("One gallon3.8L, at91cents/L. Price in TENTHS OF CENT?",3458,"91×3.8=345.8cents."),
            new("0.3×0.5 in THOUSANDTHS?",150,"0.150."),
            new("A rectangular area model of0.3by0.5 has how many HUNDREDTH-SQUARE-UNIT parts?",15,"3tenths×5tenths=15hundredths.")
        ],
        [L0506MethodsLogic] =
        [
            new("1.2×0.11=0.132. 1=yes,0=no.",1,"12×11=132 thousandths."),
            new("0.34×0.02=0.0068. 1=yes,0=no.",1,"34×2=68 ten-thousandths."),
            new("120×0.002=0.24. 1=yes,0=no.",1,"120×2 thousandths."),
            new("0.3×0.5=0.15. 1=yes,0=no.",1,"15 hundredths."),
            new("An area model for0.3×0.5 can have cells each0.01 square unit. 1=yes,0=no.",1,"3×5 of hundredth squares."),
            new("In Toronto,3.8 liters at0.91 dollars/L cost about3.46 dollars. 1=yes,0=no.",1,"3.458 rounds3.46."),
            new("Buffalo2.29dollars/gal is more expensive than Toronto3.46/gal under source conversion. 1=yes,0=no.",0,"Toronto costs more."),
            new("Difference between Toronto3.46 and Buffalo2.29 is1.17 dollars. 1=yes,0=no.",1,"3.46−2.29=1.17."),
            new("1.05×2.8=2.94 because105×28=2940 and product scale is thousandths. 1=yes,0=no.",1,"2940/1000=2.94."),
            new("0.0015×0.024=0.000036. 1=yes,0=no.",1,"15×24=360, scale1/10000000."),
            new("49/50÷7/6=21/25. 1=yes,0=no.",1,"49×6/(50×7)=21/25."),
            new("Products may be computed using fractions, powers of ten or partial area products. 1=yes,0=no.",1,"Equivalent strategies."),
            new("A numeric product alone guarantees a student's drawn area partitions are correct. 1=yes,0=no.",0,"Teacher visual review."),
            new("Multiplication by a positive number below1 produces a smaller positive product than the original factor. 1=yes,0=no.",1,"Scale factor below1.")
        ],
        [L0507ProductValues] =
        [
            new("3×0.6 in TENTHS?",18,"1.8."),
            new("0.4×2 in TENTHS?",8,"0.8."),
            new("0.4×0.6 in HUNDREDTHS?",24,"0.24."),
            new("3×2 in UNITS?",6,"6."),
            new("3.1×1.4 in HUNDREDTHS?",434,"4.34."),
            new("For3.1×1.4, area3×1 in HUNDREDTHS?",300,"3."),
            new("For3.1×1.4, area3×0.4 in HUNDREDTHS?",120,"1.2."),
            new("For3.1×1.4, area0.1×1 in HUNDREDTHS?",10,"0.1."),
            new("For3.1×1.4, area0.1×0.4 in HUNDREDTHS?",4,"0.04."),
            new("0.36×0.53 in TEN-THOUSANDTHS?",1908,".1908."),
            new("2.5×1.4 in TENTHS?",35,"3.5."),
            new("0.64×0.81 in TEN-THOUSANDTHS?",5184,"0.5184."),
            new("Diego buys12 muffins for420cents. Price per muffin in CENTS?",35,"420/12=35."),
            new("Diego buys12 muffins for420cents. Price for4 in CENTS?",140,"4×35=140."),
            new("Diego buys muffins35cents each. Maximum with300cents?",8,"300/35=8 whole,20c leftover."),
            new("0.4×0.6 plus0.4×2 in HUNDREDTHS?",104,".24+.8=1.04."),
            new("Rectangle18×23 whole units. Area?",414,"18×23=414."),
            new("An area3×0.6 plus0.4×2 plus0.4×0.6 plus3×2 in HUNDREDTHS?",884,"1.8+.8+.24+6=8.84.")
        ],
        [L0507PartialLogic] =
        [
            new("3.1×1.4=4.34. 1=yes,0=no.",1,"3×1+3×.4+.1×1+.1×.4=4.34."),
            new("0.36×0.53=0.1908. 1=yes,0=no.",1,"36×53/10000."),
            new("2.5×1.4=3.5. 1=yes,0=no.",1,"25×14/100."),
            new("0.64×0.81=0.5184. 1=yes,0=no.",1,"64×81/10000."),
            new("The area3×0.6 equals1.8. 1=yes,0=no.",1,"Source subrectangle."),
            new("The area0.4×0.6 equals0.24. 1=yes,0=no.",1,"Two decimal factors."),
            new("3.1×1.4 can be partitioned into four subrectangles using3+.1 and1+.4. 1=yes,0=no.",1,"Distributive law."),
            new("The sum of partial products equals the full rectangle area. 1=yes,0=no.",1,"No overlaps/gaps."),
            new("12 muffins for4.20 means four cost1.40. 1=yes,0=no.",1,"35c each."),
            new("A3-dollar budget buys9 whole35-cent muffins. 1=yes,0=no.",0,"9 cost315c; maximum8."),
            new("Adding digits from different decimal place columns without respecting units preserves all values. 1=yes,0=no.",0,"Regroup properly."),
            new("The method of whole-number partial products also extends to decimals after accounting for scale. 1=yes,0=no.",1,"Place-value decomposition."),
            new("A numeric correct answer alone validates a drawn source rectangle's placement labels. 1=yes,0=no.",0,"Requires visual checking."),
            new("An area model can represent products smaller than1 square unit. 1=yes,0=no.",1,"Fractional side lengths.")
        ],
        [L0508ProductArithmetic] =
        [
            new("0.54×3.8 in THOUSANDTHS?",2052,"2.052."),
            new("0.54×0.8 in THOUSANDTHS?",432,".432 source partial area."),
            new("0.54×3 in HUNDREDTHS?",162,"1.62."),
            new("0.03×0.65 in TEN-THOUSANDTHS?",195,".0195."),
            new("5.4×2.4 in HUNDREDTHS?",1296,"12.96."),
            new("1.67×3.5 in THOUSANDTHS?",5845,"5.845."),
            new("Blueberries0.6lb at398cents/lb. Cost in THOUSANDTHS OF DOLLAR?",2388,".6×3.98=2.388dollars."),
            new("Clementines1.8lb at249cents/lb. Cost in THOUSANDTHS OF DOLLAR?",4482,"1.8×2.49=4.482dollars."),
            new("Combined fruit cost2.388+4.482 in CENTS?",687,"6.870dollars=687cents."),
            new("7.4−0.0022 in TEN-THOUSANDTHS?",73978,"7.3978."),
            new("7.39−0.0012 in TEN-THOUSANDTHS?",73888,"7.3888."),
            new("Which difference is greater:7.4-.0022 or7.39-.0012? Enter1 first,2 second.",1,"7.3978>7.3888."),
            new("Andre plants3 saplings in30minutes. Minutes per sapling?",10,"30÷3=10."),
            new("Andre plants14 saplings at10minutes each. Total MINUTES?",140,"14×10=140."),
            new("0.54×3.8 represented in TEN-THOUSANDTHS?",20520,"2.052=20520 ten-thousandths."),
            new("Three times65 equals?",195,"3×65=195."),
            new("5.4×2.4 as partial 5×2+5×.4+.4×2+.4×.4, HUNDREDTHS?",1296,"10+2+.8+.16=12.96."),
            new("1.67×3.5 in TEN-THOUSANDTHS?",58450,"5.8450.")
        ],
        [L0508ProductLogic] =
        [
            new("0.54×3.8=2.052. 1=yes,0=no.",1,".54×(3+.8)=1.62+.432."),
            new("0.54×0.8=0.432. 1=yes,0=no.",1,"54×8 thousandths."),
            new("0.54×3=1.62. 1=yes,0=no.",1,"54hundredths×3."),
            new("0.03×0.65=0.0195. 1=yes,0=no.",1,"3×65/10000."),
            new("5.4×2.4=12.96. 1=yes,0=no.",1,"54×24/100."),
            new("1.67×3.5=5.845. 1=yes,0=no.",1,"167×35/1000."),
            new("Blueberries.6lb at3.98 and clementines1.8lb at2.49 have combined cost6.87 dollars rounded to cents. 1=yes,0=no.",1,"2.388+4.482=6.87."),
            new("7.4-.0022 is greater than7.39-.0012. 1=yes,0=no.",1,"7.3978>7.3888."),
            new("Andre at10min per sapling needs140min for14. 1=yes,0=no.",1,"14×10."),
            new("Decimal multiplication can be checked against an estimate of product magnitude. 1=yes,0=no.",1,"Reasonableness."),
            new("A decimal algorithm can be explained by using whole-number multiplication then a compensating power of ten. 1=yes,0=no.",1,"Place-value scaling."),
            new("A partial area diagram's total is the sum of non-overlapping subrectangle areas. 1=yes,0=no.",1,"Partition geometry."),
            new("Decimal-product answers should be rounded to cents only when the money context requires it. 1=yes,0=no.",1,"Intermediate precision matters."),
            new("The original pictured subrectangle mapping is automatically verified by a total numeric product. 1=yes,0=no.",0,"Image requires teacher review.")
        ],
        [L0509WholeDivision] =
        [
            new("2105÷5 equals?",421,"400+20+1=421."),
            new("657÷3 equals?",219,"200+10+9=219."),
            new("1150÷46 equals?",25,"46×25=1150."),
            new("534÷6 equals?",89,"6×(80+9)=534."),
            new("1032÷43 equals?",24,"43×24=1032."),
            new("In 2105÷5 partial quotients, subtract2000 and first partial quotient?",400,"5×400=2000."),
            new("After subtracting2000 from2105, remainder?",105,"2105-2000=105."),
            new("From105 divide by5 using partial quotient20, subtract how much?",100,"5×20=100."),
            new("After subtracting100 from105, remainder?",5,"105−100=5."),
            new("From657÷3, Andre uses partial quotient200. Subtraction amount?",600,"3×200=600."),
            new("For657÷3, Andre uses200,10,9. Their SUM?",219,"219."),
            new("For657÷3, Jada uses50,100,60,9. Sum?",219,"50+100+60+9=219."),
            new("534÷6 quotient partial80 and9. Subtraction for80?",480,"80×6=480."),
            new("534−480 remainder?",54,"534-480=54."),
            new("Rectangle3.25in×6.1in area in THOUSANDTHS of in²?",19825,"3.25×6.1=19.825."),
            new("Square side4.6in area in HUNDREDTHS of in²?",2116,"4.6²=21.16."),
            new("Parallelogram base5.875 height3.5in area in TEN-THOUSANDTHS?",205625,"5.875×3.5=20.5625."),
            new("Triangle base7.18 height5.4in area in THOUSANDTHS?",19386,"7.18×5.4/2=19.386."),
            new("Which polygon has largest area? Enter1 rectangle,2 square,3 parallelogram,4 triangle.",2,"Square21.16 > parallelogram20.5625."),
            new("Spider web4micrometers, fiber1/100000m=10 micrometers. Difference in MICROMETERS?",6,"10-4=6 micrometers."),
            new("Which is thicker:1 spider web4micrometers or2 fabric fiber10micrometers? Enter1 or2.",2,"Fiber10>4.")
        ],
        [L0509DivisionLogic] =
        [
            new("2105÷5=421. 1=yes,0=no.",1,"5×421=2105."),
            new("657÷3=219. 1=yes,0=no.",1,"3×219=657."),
            new("Andre and Jada may choose different partial quotients for657÷3 yet reach219. 1=yes,0=no.",1,"Same total quotient."),
            new("Andre's200+10+9 equals219. 1=yes,0=no.",1,"Sum partial quotients."),
            new("Jada's50+100+60+9 equals219. 1=yes,0=no.",1,"Valid alternative."),
            new("1150÷46=25. 1=yes,0=no.",1,"46×25=1150."),
            new("534÷6=89. 1=yes,0=no.",1,"6×89=534."),
            new("1032÷43=24. 1=yes,0=no.",1,"43×24=1032."),
            new("Partial quotients are pieces of the quotient whose divisor products are subtracted from dividend. 1=yes,0=no.",1,"Repeated subtract multiples."),
            new("One must subtract only multiples of the divisor to preserve the quotient decomposition. 1=yes,0=no.",1,"Place-value structure."),
            new("A correct final quotient automatically confirms every step of a student's partial-quotient notation is valid. 1=yes,0=no.",0,"Teacher checks reasoning."),
            new("Square side4.6 has area21.16sqin, largest among source polygons. 1=yes,0=no.",1,"Compare measured areas."),
            new("A shirt fiber10micrometers is thicker than4micrometer web. 1=yes,0=no.",1,"10>4."),
            new("One micrometer is one millionth of a meter. 1=yes,0=no.",1,"Metric definition.")
        ],
        [L0510LongDivisionNumbers] =
        [
            new("696 divided by12 equals?",58,"12×58=696."),
            new("For696÷12, first quotient5 represents which place? Enter1 hundreds,2 tens,3 ones,4 tenths.",2,"5tens means50."),
            new("917÷7 equals?",131,"7×131=917."),
            new("972÷9 equals?",108,"9×108=972."),
            new("180×9 equals?",1620,"Han's erroneous result doesn't equal972."),
            new("For917÷7,7×100 removed first from917 gives remainder?",217,"917-700=217."),
            new("For917÷7,7×30 removes how much?",210,"7×30=210."),
            new("For917÷7, after removing700 and210, remainder?",7,"917-700-210=7."),
            new("For972÷9, first quotient100 removes how much?",900,"9×100=900."),
            new("For972÷9, after removing900 remains?",72,"972−900=72."),
            new("For972÷9, next quotient8 removes how much?",72,"9×8=72."),
            new("One ounce yogurt contains1.2grams sugar,14.25oz contains grams in TENTHS?",171,"14.25×1.2=17.1g."),
            new("Coin2mass27.22g minus coin1mass16.718g in THOUSANDTHS OF GRAM?",10502,"27.220−16.718=10.502."),
            new("12 multiplied by50 equals?",600,"First tens quotient of696/12."),
            new("696−600 equals?",96,"Remainder after first step."),
            new("96 divided by12 equals?",8,"Second quotient digit8."),
            new("For917÷7, how many HUNDREDS are in first partial quotient?",1,"100 groups of7."),
            new("If quotient58 and divisor12, dividend?",696,"58×12=696.")
        ],
        [L0510LongDivisionLogic] =
        [
            new("696÷12=58. 1=yes,0=no.",1,"12×58=696."),
            new("The leading5 in quotient58 for696÷12 represents5 tens. 1=yes,0=no.",1,"50, not5 ones."),
            new("The leading5 in quotient58 for696÷12 represents5 hundreds. 1=yes,0=no.",0,"Not enough hundreds."),
            new("917÷7=131. 1=yes,0=no.",1,"7×131=917."),
            new("In917÷7, subtracting the7 written under initial9 refers to7 hundreds in the place-value algorithm. 1=yes,0=no.",1,"Place-value interpretation."),
            new("Han's result180 for972÷9 is correct. 1=yes,0=no.",0,"180×9=1620."),
            new("972÷9=108. 1=yes,0=no.",1,"9×108=972."),
            new("The0 in quotient108 represents the fact that there are no tens of groups in the middle step. 1=yes,0=no.",1,"Place holder matters."),
            new("14.25 ounces at1.2grams per ounce yields17.1grams sugar. 1=yes,0=no.",1,"Multiply14.25×1.2."),
            new("27.22grams minus16.718grams is10.502grams. 1=yes,0=no.",1,"Decimal place alignment."),
            new("A long-division digit's place value matters for each subtraction. 1=yes,0=no.",1,"Compressed notation."),
            new("Long division and partial quotients can yield the same quotient with different recording formats. 1=yes,0=no.",1,"Equivalent algorithms."),
            new("Checking a division by multiplication can reveal an impossible quotient. 1=yes,0=no.",1,"Divisor×quotient should equal dividend for zero remainder."),
            new("A correct total quotient alone automatically confirms the source diagram and student regrouping work. 1=yes,0=no.",0,"Teacher checks steps.")
        ],
        [L0511DecimalQuotients] =
        [
            new("Three quarters as a decimal in HUNDREDTHS?",75,"3÷4=0.75."),
            new("Three fiftieths as a decimal in HUNDREDTHS?",6,"3÷50=0.06."),
            new("Seven twenty-fifths as a decimal in HUNDREDTHS?",28,"7÷25=0.28."),
            new("Mai walked1/8 of30-mile trail. Distance in EIGHTH-MILE UNITS?",30,"30÷8=3.75miles=30 eighths."),
            new("Mai walked1/8 of30-mile trail. Distance in HUNDREDTHS of mile?",375,"3.75miles."),
            new("99 divided by12, quotient in HUNDREDTHS?",825,"99/12=8.25."),
            new("216 divided by5, quotient in TENTHS?",432,"216/5=43.2."),
            new("1988 divided by8, quotient in HUNDREDTHS?",24850,"1988/8=248.5."),
            new("Nine twenty-fifths as decimal in HUNDREDTHS?",36,"9/25=.36."),
            new("Eighteen fiftieths as decimal in HUNDREDTHS?",36,"18/50=.36."),
            new("Thirty-six hundredths as decimal in HUNDREDTHS?",36,"36/100=.36."),
            new("Five minus which decimal equals4.329? Answer in THOUSANDTHS?",671,"5-4.329=.671."),
            new("One minus which decimal equals0.015? Answer in THOUSANDTHS?",985,"1-.015=.985."),
            new("One minus which decimal equals0.863? Answer in THOUSANDTHS?",137,"1-.863=.137."),
            new("1.24×0.15, answer in TEN-THOUSANDTHS?",1860,"1.24×0.15=.186."),
            new("124×15 equals?",1860,"Exact source fact."),
            new("7/25 as hundredths count?",28,"Multiply numerator and denominator by4."),
            new("1/8 of80 miles gives how many MILES?",10,"80÷8=10.")
        ],
        [L0511DivisionReasoning] =
        [
            new("3/4=0.75. 1=yes,0=no.",1,"75/100."),
            new("3/50=0.6. 1=yes,0=no.",0,"Correct.06."),
            new("7/25=0.28. 1=yes,0=no.",1,"28/100."),
            new("99÷12=8.25. 1=yes,0=no.",1,"12×8.25=99."),
            new("216÷5=43.2. 1=yes,0=no.",1,"43.2×5=216."),
            new("1988÷8=248.5. 1=yes,0=no.",1,"8×248.5=1988."),
            new("9/25=18/50=36/100. 1=yes,0=no.",1,"Equivalent fractions."),
            new("18/50=0.36. 1=yes,0=no.",1,"36/100."),
            new("A decimal point may be appended with zero digits when extending a whole number dividend to find decimal quotient. 1=yes,0=no.",1,"Equivalent numerical value."),
            new("A terminating decimal quotient can be checked by multiplying it by divisor. 1=yes,0=no.",1,"Inverse relationship."),
            new("5-0.671=4.329. 1=yes,0=no.",1,"Subtraction."),
            new("1-.985=.015. 1=yes,0=no.",1,"Subtraction."),
            new("1-.137=.863. 1=yes,0=no.",1,"Subtraction."),
            new("1.24×.15=.186. 1=yes,0=no.",1,"Use place-value scaling from124×15."),
            new("A base-ten diagram can justify a quotient even when long division supplies the decimal digits. 1=yes,0=no.",1,"Equivalent representations."),
            new("Every quotient of two whole numbers has a terminating decimal. 1=yes,0=no.",0,"Some repeat, e.g.1/3.")
        ],
        [L0512DecimalDivision] =
        [
            new("4.5÷0.09 equals?",50,"Scale both×100:450÷9=50."),
            new("45÷0.9 equals?",50,"Scale both×10."),
            new("450÷9 equals?",50,"450/9=50."),
            new("4500÷90 equals?",50,"4500/90=50."),
            new("7.89÷2, result in THOUSANDTHS?",3945,"3.945×1000."),
            new("39.54÷3, result in HUNDREDTHS?",1318,"13.18×100."),
            new("0.176÷5, result in TEN-THOUSANDTHS?",352,"0.0352×10000."),
            new("Four students share17.52 dollars profit. Cents per person?",438,"1752÷4=438 cents."),
            new("11 inches in centimeters, using2.54cm/in, answer in HUNDREDTHS cm?",2794,"11×2.54=27.94cm."),
            new("8.5 inches in centimeters, using2.54cm/in, answer in THOUSANDTHS cm?",21590,"8.5×2.54=21.59cm or21590thousandths."),
            new("US letter paper11×8.5 inches. Area in TENTHS of square inch?",935,"11×8.5=93.5sqin."),
            new("European A4 paper21.0×29.7cm. Area in TENTHS of square centimeter?",6237,"21×29.7=623.7."),
            new("US letter27.94cm×21.59cm. Area in TEN-THOUSANDTHS sqcm?",6032246,"27.94×21.59=603.2246."),
            new("Compare A4 area623.7sqcm and US letter603.2246sqcm. Enter1 for A4 larger,2 for letter larger.",1,"623.7>603.2246."),
            new("Three equal groups divide9.6. Size of each group in TENTHS?",32,"9.6÷3=3.2."),
            new("One and a half divided by5. Answer in TENTHS?",3,"1.5÷5=0.3."),
            new("Eight and four tenths divided by4, result in TENTHS?",21,"8.4÷4=2.1."),
            new("Four point five divided by0.09 is same as450 divided by9. Quotient?",50,"Scale100.")
        ],
        [L0512DecimalScaleLogic] =
        [
            new("4.5÷0.09=45÷0.9=450÷9=50. 1=yes,0=no.",1,"Scale both amounts."),
            new("Multiplying both nonzero divisor and dividend by same positive factor preserves quotient. 1=yes,0=no.",1,"Ratio invariant."),
            new("Multiplying dividend alone by10 preserves quotient. 1=yes,0=no.",0,"Changes result tenfold."),
            new("7.89÷2=3.945. 1=yes,0=no.",1,"Long division."),
            new("39.54÷3=13.18. 1=yes,0=no.",1,"13.18×3=39.54."),
            new("0.176÷5=0.0352. 1=yes,0=no.",1,"0.0352×5=0.176."),
            new("17.52 dollars evenly split among4 gives4.38 dollars each. 1=yes,0=no.",1,"1752cents/4."),
            new("US letter dimensions converted by2.54cm/in are27.94cm and21.59cm. 1=yes,0=no.",1,"Multiply each inch length."),
            new("A4 area21×29.7cm exceeds US letter area27.94×21.59cm. 1=yes,0=no.",1,"623.7>603.2246."),
            new("A base-ten diagram can represent decimal quantities split into equal groups. 1=yes,0=no.",1,"Fractional place values."),
            new("4.5÷0.09=0.5. 1=yes,0=no.",0,"Quotient50."),
            new("Equal split of dollars can be checked by converting the result to cents before division. 1=yes,0=no.",1,"Avoids decimal point confusion."),
            new("When scaling division, zero cannot be used as a multiplier for both numbers, because divisor would be0. 1=yes,0=no.",1,"Undefined."),
            new("Paper area is found by multiplying both side lengths in same unit. 1=yes,0=no.",1,"Square units.")
        ],
        [L0513DecimalQuotients] =
        [
            new("109.2÷6, quotient in TENTHS?",182,"109.2/6=18.2."),
            new("31.59÷13, quotient in HUNDREDTHS?",243,"31.59/13=2.43."),
            new("61.12÷3.2, quotient in TENTHS?",191,"61.12/3.2=19.1."),
            new("611.2÷32, quotient in TENTHS?",191,"Equivalent scaling."),
            new("6112÷320, quotient in TENTHS?",191,"Equivalent scaling."),
            new("Bag weighs5.1kilograms and each penny2.5grams. Approximate count of pennies?",2040,"5100g/2.5g=2040."),
            new("2.5-1.6, answer in TENTHS?",9,"0.9."),
            new("0.72-0.4, answer in HUNDREDTHS?",32,"0.32."),
            new("11.3-1.75, answer in HUNDREDTHS?",955,"9.55."),
            new("73-1.3, answer in TENTHS?",717,"71.7."),
            new("Plant B6⅔in, Plant C4 4/15in. Plant C as fraction of B in TWENTY-FIFTHS?",16,"(64/15)/(20/3)=16/25."),
            new("Plant B6⅔in, C4 4/15in. Difference in FIFTHS of inch?",12,"20/3-64/15=12/5."),
            new("460 students walk.20% public transit count?",92,"460×.20=92."),
            new("460 students walk.5% bike count?",23,"460×.05=23."),
            new("460 students walk.110% school bus count?",506,"460×1.1=506."),
            new("2.43×13 in HUNDREDTHS?",3159,"31.59."),
            new("A 5.1kg bag is how many GRAMS?",5100,"1kg=1000grams."),
            new("1092÷60, result in TENTHS?",182,"18.2.")
        ],
        [L0513DecimalLogic] =
        [
            new("109.2÷6=1092÷60. 1=yes,0=no.",1,"Multiply both by10."),
            new("109.2÷6=18.2. 1=yes,0=no.",1,"Check18.2×6."),
            new("31.59÷13=2.43. 1=yes,0=no.",1,"13×2.43=31.59."),
            new("In 31.59÷13, the subtraction52 from55 in long division represents TENTHS. 1=yes,0=no.",1,"55 tenths step."),
            new("In31.59÷13, the subtraction39 from39 in long division represents HUNDREDTHS. 1=yes,0=no.",1,"39 hundredths step."),
            new("61.12÷3.2=611.2÷32. 1=yes,0=no.",1,"Scale×10."),
            new("61.12÷3.2=19.1. 1=yes,0=no.",1,"3.2×19.1=61.12."),
            new("Bag5.1kg containing2.5gram pennies has about2040 pennies neglecting bag weight. 1=yes,0=no.",1,"5100/2.5."),
            new("0.72-0.4=0.32. 1=yes,0=no.",1,"Subtract with place value."),
            new("11.3-1.75=9.55. 1=yes,0=no.",1,"Difference."),
            new("Plant C4 4/15 is16/25 as tall as plant B6⅔. 1=yes,0=no.",1,"Fraction ratio."),
            new("School bus110% of460 walkers equals506. 1=yes,0=no.",1,"Scale factor1.10."),
            new("Scaling only divisor changes quotient. 1=yes,0=no.",1,"Denominator factor."),
            new("The original penny count assumes penny-only mass, ignoring bag mass. 1=yes,0=no.",1,"Model assumption.")
        ]
    };

    public static bool Supports(string? family) =>
        family is L01Collections or L01Shade or L02Diagrams or L02Cards or L03RecipeBatches or L03EqualTaste
        or L04ColorAmounts or L04EqualShade ||
        (family is not null && AdditionalLessonFamilies.ContainsKey(family));

    public static Problem Build(string family, Random random)
    {
        if (!Supports(family)) throw new ArgumentException("Unknown G6 U02 ratio family",nameof(family));
        ArgumentNullException.ThrowIfNull(random);
        var variant = random.Next(AdditionalLessonFamilies.TryGetValue(family,out var extras)
            ? extras.Length : family switch
        {
            L01Collections => Pairs.Length,
            L01Shade => Shades.Length,
            L02Diagrams => Diagrams.Length,
            L02Cards => Cards.Length,
            L03RecipeBatches => RecipeBatches.Length,
            L03EqualTaste => EqualTaste.Length,
            L04ColorAmounts => ColorAmounts.Length,
            L04EqualShade => EqualShades.Length,
            _ => throw new ArgumentOutOfRangeException(nameof(family))
        });
        if (AdditionalLessonFamilies.TryGetValue(family,out var authored))
        {
            var item=authored[variant];
            return new(family,item.Prompt,item.Explanation+
                " Teacher checks source diagrams, ratio reasoning and physical groupings.",
                AssessmentItemType.Numeric,new Dictionary<string,int>{{"variant",variant}});
        }
        if (family is L04ColorAmounts or L04EqualShade)
        {
            var r=family==L04ColorAmounts?ColorAmounts[variant]:EqualShades[variant];
            return new(family,r.Prompt,
                r.Explanation+" Teacher checks actual mixture observations, same-shade model, correct units and labeled discrete diagrams.",
                AssessmentItemType.Numeric,new Dictionary<string,int>{{"variant",variant}});
        }
        if (family is L03RecipeBatches or L03EqualTaste)
        {
            return new(family,row.Prompt,
                row.Explanation+" Teacher checks the student's physical recipe grouping, visual ratios, ingredient units and explanation separately.",
                AssessmentItemType.Numeric,new Dictionary<string,int>{{"variant",variant}});
        }
        if (family == L02Diagrams)
        {
            var (first,a,unitA,second,b,unitB)=Diagrams[variant];
            return new(family,
                $"A labeled two-row diagram has {a} marks for {first} ({unitA}) and {b} marks for {second} ({unitB}). What is the ratio of the FIRST row to the SECOND? Enter the depicted A:B counts.",
                $"The labeled diagram represents {a} {unitA} of {first} for {b} {unitB} of {second}; order is {a}:{b}. Units must remain attached to their labels. Teacher checks physical cube arrangement and student sketches.",
                AssessmentItemType.ShortAnswer,new Dictionary<string,int>{{"variant",variant}});
        }
        if (family == L02Cards)
        {
            var (a,na,ua,b,nb,ub,c,nc,uc)=Cards[variant];
            return new(family,
                $"A three-row recipe diagram names {na} {ua} {a}, {nb} {ub} {b} and {nc} {uc} {c}. In THAT named order, what is the three-part ratio? Enter A:B:C.",
                $"The three separate labeled rows give {na}:{nb}:{nc}; the full names/units are {a} ({ua}), {b} ({ub}), {c} ({uc}). Teacher assesses actual diagram, card matching and partner critique.",
                AssessmentItemType.ShortAnswer,new Dictionary<string,int>{{"variant",variant}});
        }
        if (family == L01Collections)
        {
            var (first,a,second,b) = Pairs[variant];
            return new(family,
                $"A labeled collection has {a} {first} and {b} {second}. Give the ratio of {first} to {second}, in that order, using A:B notation.",
                $"The order is {first} first ({a}) and {second} second ({b}), so {a}:{b}. Equivalent positive ratios are accepted. Student-created category choices and diagrams require teacher review.",
                AssessmentItemType.ShortAnswer,new Dictionary<string,int>{{"variant",variant}});
        }

        var (total,x,y,firstColor) = Shades[variant];
        var first = total * x / (x+y);
        var second = total * y / (x+y);
        return new(family,
            $"A shape is made of {total} equal square units. Color A to color B is {x}:{y}. How many units should be color {(firstColor?"A":"B")}?",
            $"The ratio has {x+y} equal parts; each part is {total/(x+y)} square units. Color A takes {first} and B takes {second}. Teacher reviews actual coloring, equal units, and the student's other differently shaped diagram.",
            AssessmentItemType.Numeric,new Dictionary<string,int>{{"variant",variant}});
    }

    private static bool Valid(string? family,IReadOnlyDictionary<string,int>? p) =>
        Supports(family) && p is {Count:1} && p.TryGetValue("variant",out var v) &&
        v >= 0 && v < (AdditionalLessonFamilies.TryGetValue(family!,out var additional)
            ? additional.Length : family switch
        {
            L01Collections => Pairs.Length,
            L01Shade => Shades.Length,
            L02Diagrams => Diagrams.Length,
            L02Cards => Cards.Length,
            L03RecipeBatches => RecipeBatches.Length,
            L03EqualTaste => EqualTaste.Length,
            L04ColorAmounts => ColorAmounts.Length,
            L04EqualShade => EqualShades.Length,
            _ => 0
        });

    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if (!Valid(family,p)) throw new ArgumentException("Invalid ratio-family variant.");
        var v=p["variant"];
        if (AdditionalLessonFamilies.TryGetValue(family,out var authored))
            return authored[v].Answer.ToString(CultureInfo.InvariantCulture);
        if (family is L04ColorAmounts or L04EqualShade)
        {
            var r=family==L04ColorAmounts?ColorAmounts[v]:EqualShades[v];
            return r.Answer.ToString(CultureInfo.InvariantCulture);
        }
        if (family is L03RecipeBatches or L03EqualTaste)
        {
            var row=family==L03RecipeBatches?RecipeBatches[v]:EqualTaste[v];
            return row.Answer.ToString(CultureInfo.InvariantCulture);
        }
        if (family==L02Diagrams)
        {
            var d=Diagrams[v];
            return $"{d.A}:{d.B}";
        }
        if (family==L02Cards)
        {
            var c=Cards[v];
            return $"{c.CountA}:{c.CountB}:{c.CountC}";
        }
        if (family==L01Collections)
        {
            var pair=Pairs[v];
            return $"{pair.A}:{pair.B}";
        }

        var s=Shades[v];
        return (s.Total*(s.FirstColor?s.First:s.Second)/(s.First+s.Second))
            .ToString(CultureInfo.InvariantCulture);
    }

    public static bool Verify(string family,IReadOnlyDictionary<string,int>? p,string? answer)
    {
        if (!Valid(family,p) || string.IsNullOrWhiteSpace(answer)) return false;
        var expected=Solve(family,p!);
        if (AdditionalLessonFamilies.ContainsKey(family))
            return int.TryParse(answer,NumberStyles.None,CultureInfo.InvariantCulture,out var bankNumber)
                && bankNumber.ToString(CultureInfo.InvariantCulture)==expected;
        if (family is L04ColorAmounts or L04EqualShade)
            return int.TryParse(answer,NumberStyles.None,CultureInfo.InvariantCulture,out var colorNumber)
                && colorNumber.ToString(CultureInfo.InvariantCulture)==expected;
        if (family is L03RecipeBatches or L03EqualTaste)
            return int.TryParse(answer,NumberStyles.None,CultureInfo.InvariantCulture,out var n)
                && n.ToString(CultureInfo.InvariantCulture)==expected;
        if (family==L01Shade)
            return int.TryParse(answer,NumberStyles.None,CultureInfo.InvariantCulture,out var value)
                && value.ToString(CultureInfo.InvariantCulture)==expected;

        if (family is L02Diagrams or L02Cards)
            return string.Equals(answer.Trim().Replace(" ","",StringComparison.Ordinal),
                expected,StringComparison.Ordinal);
        var pieces=answer.Split(':');
        if (pieces.Length!=2 ||
            !int.TryParse(pieces[0].Trim(),NumberStyles.None,CultureInfo.InvariantCulture,out var a) ||
            !int.TryParse(pieces[1].Trim(),NumberStyles.None,CultureInfo.InvariantCulture,out var b) ||
            a<=0 || b<=0 || a>100000 || b>100000) return false;
        var pair=Pairs[p!["variant"]];
        return (long)a*pair.B==(long)b*pair.A;
    }
}

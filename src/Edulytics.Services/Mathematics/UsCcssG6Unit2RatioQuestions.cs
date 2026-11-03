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

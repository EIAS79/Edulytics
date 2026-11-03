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

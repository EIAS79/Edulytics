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

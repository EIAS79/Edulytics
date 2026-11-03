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

    public static bool Supports(string? family) =>
        family is L01Collections or L01Shade or L02Diagrams or L02Cards;

    public static Problem Build(string family, Random random)
    {
        if (!Supports(family)) throw new ArgumentException("Unknown G6 U02 ratio family",nameof(family));
        ArgumentNullException.ThrowIfNull(random);
        var variant = random.Next(family switch
        {
            L01Collections => Pairs.Length,
            L01Shade => Shades.Length,
            L02Diagrams => Diagrams.Length,
            L02Cards => Cards.Length,
            _ => throw new ArgumentOutOfRangeException(nameof(family))
        });
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
        v >= 0 && v < (family switch
        {
            L01Collections => Pairs.Length,
            L01Shade => Shades.Length,
            L02Diagrams => Diagrams.Length,
            L02Cards => Cards.Length,
            _ => 0
        });

    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if (!Valid(family,p)) throw new ArgumentException("Invalid ratio-family variant.");
        var v=p["variant"];
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

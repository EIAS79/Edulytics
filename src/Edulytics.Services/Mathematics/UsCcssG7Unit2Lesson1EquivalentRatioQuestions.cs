using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// OUR/IM Grade7 Unit2 Lesson1: exact unit-rate/double-line/geometry checks
/// only. Taste, drawn crescent geometry, contextual labels and explanations
/// are open responses requiring a teacher to review actual learner work.
/// </summary>
internal static class UsCcssG7Unit2Lesson1EquivalentRatioQuestions
{
    public const string Family="usccss.g7.u02.l01.ratio_mixtures_crescent";
    public const string Table="usccss.g7.u02.l02.proportional_tables";
    internal sealed record Problem(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? family)=>family is Family or Table;
    public static Problem Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported Grade7 U02 family.");
        if(family==Table) return BuildTable(random);
        var p=new Dictionary<string,int>(StringComparer.Ordinal)
        {["kind"]=random.Next(0,5),["feature"]=random.Next(0,4)};
        int kind=p["kind"],feature=p["feature"];
        string question=kind switch
        {
            0 => "A first-party DOUBLE NUMBER LINE has aligned top ticks 0,2,4,6,8,10 "+
                 "and bottom ticks 0,1,2,3,4,5. The top tick is "+
                 new[]{"2","4","6","8"}[feature]+
                 ". What number is aligned on the bottom line? "+
                 "Supply a context and units to a teacher.",
            1 => new[]{
                "Which original recipe has MOST powder per cup? Recipe 1: 1 cup water and 1.5 tsp powder. Recipe 2: 2 cups water and 0.5 tsp powder. Recipe 3: 1 cup water and 0.25 tsp powder. Enter its index 1,2,3.",
                "Do the ratios of Recipe 2 (2 cups water,0.5 tsp powder) and Recipe 3 (1 cup water,0.25 tsp powder) match? Enter 1 yes or 0 no.",
                "Do the ratios of Recipe 1 (1 cup water,1.5 tsp powder) and Recipe 3 (1 cup water,0.25 tsp powder) match? Enter 1 yes or 0 no.",
                "Recipe 2 has 2 cups of water and half a teaspoon of powder. How many teaspoons of powder PER ONE cup of water? Enter an exact rational."
            }[feature],
            2 => new[]{
                "Three original oval shapes have corresponding width/height lengths A=(5,4), B=(10,8), C=(10,6). Which is NOT a uniform scaled copy of A, B=2 or C=3?",
                "Original oval A has width5 height4; B has width10 height8. Does B preserve ONE linear scale on both dimensions? Enter 1 yes, 0 no.",
                "Original oval A has width5 height4; C has width10 height6. Does C preserve ONE linear scale on both dimensions? Enter 1 yes, 0 no.",
                "Trail mixes compare 3 cups peanuts:2 cups raisins to 4.5 cups peanuts:3 cups raisins. Are the two ratios equivalent? Enter 1 yes, 0 no."
            }[feature],
            3 => new[]{
                "First-party crescent B has width 1.5 times and height 1.5 times those of A. Is B a UNIFORM copy? Enter 1 yes, 0 no.",
                "First-party crescent C has width 1.5 times and height 1.0 times those of A. Is C a UNIFORM copy? Enter 1 yes, 0 no.",
                "A crescent drawing's width increases from W to 1.5W. What is the width scale factor? Enter an exact rational.",
                "A crescent C retains original A's height H while its width changes. What is the HEIGHT factor A-to-C? Enter a number."
            }[feature],
            _ => new[]{
                "Salt/sugar mixture A uses 2 cups water,4 tsp salt,0.25 cup sugar; C uses 1 cup water,2 tsp salt,0.125 cup sugar. Are BOTH component ratios equivalent? 1 yes, 0 no.",
                "Mixture B uses 1.5 cups water,3 tsp salt,0.2 cup sugar. Is its SUGAR-per-water ratio the same as A's 0.25 cup sugar in 2 cups water? 1 yes, 0 no.",
                "Mixture A has 4 tsp salt in 2 cups water. What is its salt per water cup?",
                "Mixture B has 0.2 cup sugar in 1.5 cups water. Give its exact sugar-to-water rate as a fraction."
            }[feature]
        };
        return new(Family,question+" Give the exact numeric answer only. "+
            "Drawings, invented double-line labels, physical taste testing "+
            "and explanation are teacher-reviewed.",
            "Compare the same ingredient per unit water, and examine BOTH "+
            "corresponding dimensions of a scaled figure. Different-sized "+
            "mixtures can have equivalent ingredient ratios. A numerical "+
            "response does not certify original source drawings or discussions.",
            AssessmentItemType.ShortAnswer,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid source ratio parameters.");
        var(n,d)=family==Table?ExpectedTable(p):Expected(p);
        long g=Gcd(n,d);n/=g;d/=g;
        return d==1?n.ToString(CultureInfo.InvariantCulture):
            n.ToString(CultureInfo.InvariantCulture)+"/"+
            d.ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Valid(family,p)||string.IsNullOrWhiteSpace(answer)||answer.Length>32)
            return false;
        var v=answer.Trim().Split('/');
        decimal supplied;
        if(v.Length==2)
        {
            if(!decimal.TryParse(v[0],NumberStyles.None,CultureInfo.InvariantCulture,out var n)||
               !decimal.TryParse(v[1],NumberStyles.None,CultureInfo.InvariantCulture,out var d)||
               n<0||n>1000000||d<=0||d>1000000)
                return false;
            supplied=n/d;
        }
        else if(v.Length==1)
        {
            if(!decimal.TryParse(v[0],NumberStyles.AllowDecimalPoint,
                  CultureInfo.InvariantCulture,out supplied)||supplied<0||supplied>1000000)
                return false;
        }
        else return false;
        var(n0,d0)=family==Table?ExpectedTable(p):Expected(p);
        return supplied*d0==n0;
    }

    private static Problem BuildTable(Random random)
    {
        var p=new Dictionary<string,int>(StringComparer.Ordinal)
        {["kind"]=random.Next(0,8),["feature"]=random.Next(0,4)};
        int kind=p["kind"],f=p["feature"];
        string prompt=kind switch
        {
            0 => "A notice-and-wonder TABLE reports cases and rolls "+
                 "(1,12),(2,36),(5,60),(10,120). "+
                 new[]{
                    "Which CASE COUNT has an inconsistent rolls/case ratio?",
                    "If each case contains 12 rolls, how many rolls should 2 cases contain?",
                    "What is the constant rolls PER CASE in the other three consistent rows?",
                    "Is the entire table as written proportional? Enter 1 yes, 0 no."
                 }[f]+" Give an exact response; explain the anomaly to a teacher.",
            1 => "Rice for a crowd: 2 cups of rice serve 6 people. "+
                 new[]{
                    "How many people can 10 cups of dry rice serve?",
                    "How many cups of dry rice serve 45 people?",
                    "How many people are served per ONE cup of rice?",
                    "How many people can 3 cups of rice serve?"
                 }[f],
            2 => "A spring-roll recipe has 6 rolls serving 3 people. "+
                 new[]{
                    "How many people do 30 rolls serve?",
                    "How many people do 40 rolls serve?",
                    "How many spring rolls serve 28 people?",
                    "How many PEOPLE per ONE spring roll is the constant?"
                 }[f],
            3 => "Bread dough uses 8 tablespoons of honey for 10 cups of flour. "+
                 new[]{
                    "How many cups of flour correspond to 20 tablespoons honey?",
                    "How many cups of flour correspond to 13 tablespoons honey?",
                    "How many tablespoons of honey correspond to 20 cups flour?",
                    "What is the constant CUPS FLOUR per tablespoon of honey?"
                 }[f],
            4 => "Four quarters have the same value as 10 dimes. "+
                 new[]{
                    "How many dimes have the value of 6 quarters?",
                    "How many dimes have the value of 14 quarters?",
                    "What number of dimes corresponds to 1 quarter?",
                    "What constant of proportionality converts quarters to dimes?"
                 }[f],
            5 => "Chocolate milk has 2 cups milk for 3 tablespoons chocolate syrup. "+
                 new[]{
                    "How many syrup tablespoons for 2 cups milk?",
                    "How many syrup tablespoons for 8 cups milk?",
                    "How many syrup tablespoons for 10 cups milk?",
                    "What is the constant syrup TABLESPOONS per 1 cup milk?"
                 }[f],
            6 => "Paint color combines 3 cups red for every 7 cups white. "+
                 new[]{
                    "What is the RED cups per ONE WHITE cup constant?",
                    "How many cups red for 7 cups white?",
                    "How many cups red for 14 cups white?",
                    "How many cups red for 21 cups white?"
                 }[f],
            _ => "Compare ratio cards A=4:7, B=8:15, C=16:28, D=2:3, E=20:35. "+
                 new[]{
                    "How many of A,B,C,D,E are equivalent to A?",
                    "Are A and C equivalent? Enter 1 yes, 0 no.",
                    "Are A and B equivalent? Enter 1 yes, 0 no.",
                    "Are A and E equivalent? Enter 1 yes, 0 no."
                 }[f]
        };
        return new(Table,prompt+" Respond with an exact number or rational fraction.",
            "In a proportional table every nonzero row has the same "+
            "constant second/first quantity. The anomalous towel row "+
            "breaks proportionality and must not be silently treated as "+
            "24 instead of 36. Row-to-row scaling differs from the fixed "+
            "column constant. Drawing, discussion and units need teacher review.",
            AssessmentItemType.ShortAnswer,p);
    }
    private static(long,long) ExpectedTable(IReadOnlyDictionary<string,int> p)
    {
        int f=p["feature"];
        return p["kind"] switch
        {
            0 => f switch {0=>(2,1),1=>(24,1),2=>(12,1),_=>(0,1)},
            1 => f switch {0=>(30,1),1=>(15,1),2=>(3,1),_=>(9,1)},
            2 => f switch {0=>(15,1),1=>(20,1),2=>(56,1),_=>(1,2)},
            3 => f switch {0=>(25,1),1=>(65,4),2=>(16,1),_=>(5,4)},
            4 => f switch {0=>(15,1),1=>(35,1),2=>(5,2),_=>(5,2)},
            5 => f switch {0=>(3,1),1=>(12,1),2=>(15,1),_=>(3,2)},
            6 => f switch {0=>(3,7),1=>(3,1),2=>(6,1),_=>(9,1)},
            _ => f switch {0=>(3,1),1=>(1,1),2=>(0,1),_=>(1,1)}
        };
    }

    private static(long,long) Expected(IReadOnlyDictionary<string,int> p)
    {
        int k=p["kind"],f=p["feature"];
        return k switch
        {
            0 => (f+1,1),
            1 => f switch {0=>(1,1),1=>(1,1),2=>(0,1),_=>(1,4)},
            2 => f switch {0=>(3,1),1=>(1,1),2=>(0,1),_=>(1,1)},
            3 => f switch {0=>(1,1),1=>(0,1),2=>(3,2),_=>(1,1)},
            _ => f switch {0=>(1,1),1=>(0,1),2=>(2,1),_=>(2,15)}
        };
    }
    private static long Gcd(long a,long b)
    {while(b!=0){long r=a%b;a=b;b=r;}return a;}
    private static bool Valid(string? family,IReadOnlyDictionary<string,int>? p)=>
        Supports(family)&&p is not null&&p.Count==2 &&
        p.TryGetValue("kind",out int k)&&k>=0&&k<=(family==Table?7:4) &&
        p.TryGetValue("feature",out int f)&&f is >=0 and <=3;
}

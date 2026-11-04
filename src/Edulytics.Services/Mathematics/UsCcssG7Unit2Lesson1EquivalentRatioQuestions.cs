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
    internal sealed record Problem(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? family)=>family==Family;
    public static Problem Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported Grade7 U02 L01 family.");
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
        var(n,d)=Expected(p);long g=Gcd(n,d);n/=g;d/=g;
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
        var(n0,d0)=Expected(p);
        return supplied*d0==n0;
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
        p.TryGetValue("kind",out int k)&&k is >=0 and <=4 &&
        p.TryGetValue("feature",out int f)&&f is >=0 and <=3;
}

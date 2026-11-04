using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;

/// <summary>
/// Grade 7 U02 graph reasoning. L10 checks line-through-origin structure,
/// exact slope and domain. L12 extends the same engine with comparisons of
/// proportional lines on common axes and the delayed-start boundary.
/// Physical graph construction and written explanations remain teacher reviewed.
/// </summary>
internal static class UsCcssG7Unit2Lesson10GraphProportionsQuestions
{
    public const string Family="usccss.g7.u02.l10.graphs_origin_slope";
    public const string CompareFamily="usccss.g7.u02.l12.compare_same_axes_rates";

    internal sealed record Problem(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);

    public static bool Supports(string? f)=>f==Family||f==CompareFamily;

    public static Problem Build(string f,Random r)
    {
        if(!Supports(f))throw new InvalidOperationException("Unsupported Grade 7 graph family.");
        var p=new Dictionary<string,int>(StringComparer.Ordinal)
        {["kind"]=r.Next(0,5),["feature"]=r.Next(0,4)};
        return f==CompareFamily ? BuildComparison(f,p) : BuildOriginSlope(f,p);
    }

    private static Problem BuildOriginSlope(string f,IReadOnlyDictionary<string,int> p)
    {
        int k=p["kind"],i=p["feature"];
        string q=k switch
        {
            0 => "Inspect the complete graph defined by "+
                new[]{"y=8x","y=10-2x","y=x^2","y=4x+3"}[i]+
                ". Is it a straight line through the origin with a fixed y/x? "+
                "Enter 1 for YES proportional or 0 for NO.",
            1 => "A shop sells shirts for $8 each with no fixed fee. "+
                "Find the total price in dollars of "+
                new[]{"1","2","3","6"}[i]+" shirts.",
            2 => "A straight-line model through the origin contains the point "+
                new[]{"(2,6)","(4,12)","(5,7.5)","(3,0.75)"}[i]+
                ". Find the exact y/x slope k of y=kx.",
            3 => "A proposed graphical relationship is "+
                new[]{"a straight line through (0,0) and (3,6)",
                     "a straight line through (0,2) and (3,6)",
                     "a curved graph y=x^2 beginning at (0,0)",
                     "a line with positive intercept 3 and positive slope 4"}[i]+
                ". Enter 1 if the entire specified graph is proportional, otherwise 0.",
            _ => "Blueberries cost $6 per pound, without a fixed fee. "+
                "What is the price in dollars for "+
                new[]{"0.5","1.25","2.75","4.5"}[i]+
                " pounds? Fractional weights are physically meaningful."
        };
        return new(f,q+" Submit an exact numeric response. "+
            "Coordinate plotting, axes, units and sorting explanations require teacher review.",
            "All points of an exact proportional function lie on a line "+
            "through the origin; its slope equals y/x for nonzero x. "+
            "A line with nonzero intercept or a curved graph fails.",
            AssessmentItemType.ShortAnswer,p);
    }

    private static Problem BuildComparison(string f,IReadOnlyDictionary<string,int> p)
    {
        int k=p["kind"],i=p["feature"];
        string q=k switch
        {
            0 => "Two proportional lines share the same axes and scales. Their constants are "+
                new[]{"A=5, B=3","A=4/3, B=3/2","A=7/4, B=5/4","A=2.5, B=2.25"}[i]+
                ". Enter the larger constant of proportionality.",
            1 => "Two proportional lines start at the origin and share the same axes. At the same x-value, "+
                new[]{"x=2: A has y=6 and B has y=4",
                     "x=4: A has y=5 and B has y=7",
                     "x=3: A has y=9 and B has y=6",
                     "x=5: A has y=10 and B has y=12.5"}[i]+
                ". Enter 1 if line A is steeper or 2 if line B is steeper.",
            2 => "Two constant-speed distance-time lines share the same axes. The data are "+
                new[]{"A: 50 m in 40 s; B: 50 m in 30 s",
                     "A: 24 m in 12 s; B: 30 m in 10 s",
                     "A: 18 m in 9 s; B: 14 m in 7 s",
                     "A: 7.5 m in 3 s; B: 9 m in 4.5 s"}[i]+
                ". Enter the greater speed in meters per second.",
            3 => "Two positive proportional lines use identical unlabeled axis scales. "+
                new[]{"At the same positive x, line A is above line B.",
                     "At the same positive x, line B is above line A.",
                     "For one common horizontal run, A rises farther than B.",
                     "For one common horizontal run, B rises farther than A."}[i]+
                " Enter 1 if A has the larger constant or 2 if B has the larger constant.",
            _ => "Time is measured from one shared clock. Decide whether the described distance-time graph "+
                new[]{"runs from (0,0) through (40,50)",
                     "starts later and runs from (10,0) to (40,50)",
                     "runs from (0,0) through (20,50)",
                     "runs from (0,5) through (20,50)"}[i]+
                ". Enter 1 if distance is proportional to the shared elapsed time, otherwise 0."
        };
        return new(f,q+" Submit an exact numeric response. "+
            "The comparison assumes identical axes and units. Drawn lines and contextual explanations require teacher review.",
            "On identical axes, a larger positive constant gives a steeper proportional line. "+
            "A delayed start or nonzero intercept is not proportional to the shared elapsed time.",
            AssessmentItemType.ShortAnswer,p);
    }

    public static string Solve(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(f,p))throw new InvalidOperationException("Invalid G7 graph task.");
        var(n,d)=Expected(f,p);var g=Gcd(n,d);n/=g;d/=g;
        return d==1?n.ToString(CultureInfo.InvariantCulture):
            n.ToString(CultureInfo.InvariantCulture)+"/"+d.ToString(CultureInfo.InvariantCulture);
    }

    public static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Valid(f,p)||string.IsNullOrWhiteSpace(answer)||answer.Length>32)
            return false;
        var a=answer.Trim().Split('/');
        decimal val;
        if(a.Length==2)
        {
            if(!decimal.TryParse(a[0],NumberStyles.None,CultureInfo.InvariantCulture,out var n)||
               !decimal.TryParse(a[1],NumberStyles.None,CultureInfo.InvariantCulture,out var d)||
               n<0||n>1000000||d<=0||d>1000000)return false;
            val=n/d;
        }
        else if(a.Length==1)
        {
            if(!decimal.TryParse(a[0],NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture,out val)||val<0||val>1000000)return false;
        }
        else return false;
        var(en,ed)=Expected(f,p);return val*ed==en;
    }

    private static(long,long) Expected(string f,IReadOnlyDictionary<string,int> p)
    {
        if(f==CompareFamily)return CompareExpected(p);
        int k=p["kind"],i=p["feature"];
        return k switch
        {
            0 => (i==0?1:0,1),
            1 => (new long[]{8,16,24,48}[i],1),
            2 => i switch
            {
                0=>(3,1),1=>(3,1),2=>(3,2),_=>(1,4)
            },
            3 => (i==0?1:0,1),
            _ => i switch{0=>(3,1),1=>(15,2),2=>(33,2),_=>(27,1)}
        };
    }

    private static(long,long) CompareExpected(IReadOnlyDictionary<string,int> p)
    {
        int k=p["kind"],i=p["feature"];
        return k switch
        {
            0 => i switch{0=>(5,1),1=>(3,2),2=>(7,4),_=>(5,2)},
            1 => (new long[]{1,2,1,2}[i],1),
            2 => i switch{0=>(5,3),1=>(3,1),2=>(2,1),_=>(5,2)},
            3 => (new long[]{1,2,1,2}[i],1),
            _ => (new long[]{1,0,1,0}[i],1)
        };
    }

    private static long Gcd(long a,long b)
    {while(b!=0){long t=a%b;a=b;b=t;}return a;}

    private static bool Valid(string? f,IReadOnlyDictionary<string,int>? p)=>
        Supports(f)&&p is not null&&p.Count==2&&
        p.TryGetValue("kind",out int k)&&k is >=0 and <=4&&
        p.TryGetValue("feature",out int i)&&i is >=0 and <=3;
}

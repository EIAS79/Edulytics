using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;

/// <summary>
/// Grade7 U02 L10. Line through origin versus straight offset or curved
/// graph, exact slope as y/x, and discrete vs continuous x domain.
/// Shared lesson-scoped exact mathematics engine; student plot construction
/// and graph-card sorting require teacher inspection.
/// </summary>
internal static class UsCcssG7Unit2Lesson10GraphProportionsQuestions
{
    public const string Family="usccss.g7.u02.l10.graphs_origin_slope";
    internal sealed record Problem(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? f)=>f==Family;
    public static Problem Build(string f,Random r)
    {
        if(!Supports(f))throw new InvalidOperationException("Unsupported L10 family.");
        var p=new Dictionary<string,int>(StringComparer.Ordinal)
        {["kind"]=r.Next(0,5),["feature"]=r.Next(0,4)};
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
        return new(Family,q+" Submit an exact numeric response. "+
            "Coordinate plotting, axes, units and sorting explanations require teacher review.",
            "All points of an exact proportional function lie on a line "+
            "through the origin; its slope equals y/x for nonzero x. "+
            "A line with nonzero intercept or a curved graph fails.",
            AssessmentItemType.ShortAnswer,p);
    }
    public static string Solve(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(f,p))throw new InvalidOperationException("Invalid G7 graph task.");
        var(n,d)=Expected(p);var g=Gcd(n,d);n/=g;d/=g;
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
        var(en,ed)=Expected(p);return val*ed==en;
    }
    private static(long,long) Expected(IReadOnlyDictionary<string,int> p)
    {
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
    private static long Gcd(long a,long b)
    {while(b!=0){long t=a%b;a=b;b=t;}return a;}
    private static bool Valid(string? f,IReadOnlyDictionary<string,int>? p)=>
        Supports(f)&&p is not null&&p.Count==2&&
        p.TryGetValue("kind",out int k)&&k is >=0 and <=4&&
        p.TryGetValue("feature",out int i)&&i is >=0 and <=3;
}

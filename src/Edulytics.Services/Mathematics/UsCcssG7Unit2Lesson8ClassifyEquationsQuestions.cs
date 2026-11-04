using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;

/// <summary>
/// Grade7 U02 L08: distinguish y=kx from affine and nonlinear rules.
/// Exact table evaluations and classification only; physical cube models,
/// explanations and predictions require teacher review.
/// </summary>
internal static class UsCcssG7Unit2Lesson8ClassifyEquationsQuestions
{
    public const string Family="usccss.g7.u02.l08.classify_equations";
    internal sealed record Problem(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? f)=>f==Family;
    public static Problem Build(string family,Random rng)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported L08 family");
        var p=new Dictionary<string,int>(StringComparer.Ordinal)
        {["mode"]=rng.Next(0,8),["feature"]=rng.Next(0,4),["variant"]=rng.Next(0,2)};
        int mode=p["mode"],feature=p["feature"],variant=p["variant"];
        string q;
        if(mode==0)
        {
            string[] formulas=["y=x+4","y=x/4","y=4x","y=4^x",
                               "y=4/x","y=x^4","y=3.2x+5","y=(5/2)x"];
            q="For all relevant positive x values, does "+formulas[feature+4*variant]+
              " describe a proportional y=kx relationship? Enter 1 YES or 0 NO.";
        }
        else if(mode==1)
        {
            q="Convert temperature with Fahrenheit F=(9/5)C+32. "+
              "Find F for C="+new[]{"20","4","175","0"}[feature]+
              ". Explain why the +32 term prevents direct proportionality.";
        }
        else if(mode==2)
        {
            q="Convert inches n into centimeters c=2.54n. "+
              "How many centimeters correspond to "+
              new[]{"10","8","3.5","1"}[feature]+
              " inches? Explain why this equation IS proportional.";
        }
        else if(mode is 3 or 4 or 5)
        {
            string[] lengths=["3","5","9.5","2"];
            string descriptor=mode switch
            {3=>"sum of the lengths of all 12 edges",4=>"total surface area",_=>"volume"};
            q="A cube has side length "+lengths[feature]+
              " units. Calculate its "+descriptor+
              " using "+(mode==3?"E=12s":mode==4?"A=6s^2":"V=s^3")+
              ". Show units and explain whether the full side-length relationship is proportional.";
        }
        else if(mode==6)
        {
            string[] rules=["cube's total edge length E=12s",
                "cube's surface area A=6s^2","cube's volume V=s^3",
                "rectangular prism volume V=50L with fixed width 10 and height 5"];
            q="Is the relationship between the varying side parameter "+
              "and "+rules[feature]+" proportional for all positive inputs? "+
              "Enter 1 YES or 0 NO.";
        }
        else
        {
            string[] contexts=["remaining rope length L=120-x after cutting x inches",
                "price with 8% sales tax t=1.08p",
                "m marbles split between four sisters x=m/4",
                "volume of prism with square side s and height12: V=12s^2"];
            q="Does "+contexts[feature]+" define a proportional relationship? "+
              "Enter 1 YES or 0 NO.";
        }
        return new(Family,q+" Answer exactly. Teacher review required for tables and reasoning.",
            "A proportional law has zero intercept and constant output/input for "+
            "all allowed nonzero inputs. Temperatures have an additive offset; "+
            "surface area varies quadratically and cube volume cubically.",
            AssessmentItemType.ShortAnswer,p);
    }
    public static string Solve(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(f,p))throw new InvalidOperationException("Invalid L08 parameters");
        var(n,d)=Expected(p);long g=Gcd(n,d);n/=g;d/=g;
        return d==1?n.ToString(CultureInfo.InvariantCulture):
            n.ToString(CultureInfo.InvariantCulture)+"/"+d.ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Valid(f,p)||string.IsNullOrWhiteSpace(answer)||answer.Length>32)return false;
        string[] a=answer.Trim().Split('/');
        decimal value;
        if(a.Length==2)
        {
            if(!decimal.TryParse(a[0],NumberStyles.None,
                 CultureInfo.InvariantCulture,out decimal n)||
               !decimal.TryParse(a[1],NumberStyles.None,
                 CultureInfo.InvariantCulture,out decimal d)||
               n<0||n>10000000||d<=0||d>10000000)return false;
            value=n/d;
        }
        else if(a.Length==1)
        {
            if(!decimal.TryParse(a[0],NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture,out value)||
                value<0||value>10000000)return false;
        }
        else return false;
        var(numer,denom)=Expected(p);return value*denom==numer;
    }
    private static(long,long) Expected(IReadOnlyDictionary<string,int> p)
    {
        int mode=p["mode"],f=p["feature"],v=p["variant"];
        if(mode==0)return (new long[]{0,1,1,0,0,0,0,1}[f+4*v],1);
        if(mode==1)return f switch{0=>(68,1),1=>(196,5),
                                   2=>(347,1),_=>(32,1)};
        if(mode==2)return f switch{0=>(127,5),1=>(508,25),
                                   2=>(889,100),_=>(127,50)};
        if(mode==3)return f switch{0=>(36,1),1=>(60,1),
                                   2=>(114,1),_=>(24,1)};
        if(mode==4)return f switch{0=>(54,1),1=>(150,1),
                                   2=>(1083,2),_=>(24,1)};
        if(mode==5)return f switch{0=>(27,1),1=>(125,1),
                                   2=>(6859,8),_=>(8,1)};
        if(mode==6)return (f is 0 or 3?1:0,1);
        return (f is 1 or 2?1:0,1);
    }
    private static long Gcd(long a,long b)
    {while(b!=0){long r=a%b;a=b;b=r;}return a;}
    private static bool Valid(string? f,IReadOnlyDictionary<string,int>? p)=>
        Supports(f)&&p is not null&&p.Count==3&&
        p.TryGetValue("mode",out int k)&&k is >=0 and <=7&&
        p.TryGetValue("feature",out int i)&&i is >=0 and <=3&&
        p.TryGetValue("variant",out int j)&&j is 0 or 1;
}

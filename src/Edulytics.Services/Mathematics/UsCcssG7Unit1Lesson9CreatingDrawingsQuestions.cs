using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Grade 7 U01:L09 first-party exact scale plans: original measured lengths,
/// drawing lengths and differently-sized representations of the same object.
/// The learner's actual floor-plan construction is still teacher-reviewed.
/// </summary>
internal static class UsCcssG7Unit1Lesson9CreatingDrawingsQuestions
{
    public const string Family="usccss.g7.u01.l09.create_drawing_scale_lengths";
    internal sealed record Problem(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);

    public static bool Supports(string? family) =>
        string.Equals(family,Family,StringComparison.Ordinal);

    public static Problem Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported G7 L09 family.");
        var p=new Dictionary<string,int>(StringComparer.Ordinal)
        {
            ["kind"]=random.Next(0,4),
            ["feature"]=random.Next(0,4),
            ["style"]=random.Next(0,2)
        };
        int k=p["kind"],f=p["feature"],s=p["style"];
        string prompt=k switch
        {
            0 => "A first-party bedroom floor plan represents 4 metres " +
                "by 16 centimetres at the initial scale. At the " +
                (s==0 ? "same scale (4 cm on drawing per real metre)"
                      : "reduced scale (2 cm on drawing per real metre)") +
                ", how many centimetres long is " +
                new[]{"a 4-metre wall","a 2.5-metre wall","a 2.75-metre wall",
                      "a 3.75-metre wall"}[f] + "?",
            1 => "An original outline of a region has " +
                new[]{"width 270 miles","height 350 miles",
                      "notch width 110 miles","notch height 70 miles"}[f] +
                ". Create a scale drawing at " + (s==0?"1 cm to 50 miles":"1 cm to 75 miles")+
                ". How many centimetres long is that feature?",
            2 => "A drawing of a book has a width of " + (s==0?"1.5":"3")+
                " inches, while its real width is 9 inches. " +
                (f switch
                {
                    0 => "A corresponding diagram height is " +
                        (s==0?"3":"6") + " inches. What is the REAL book height in inches?",
                    1 => "How many REAL inches correspond to 1 drawing inch?",
                    2 => "How many drawing inches represent the REAL width of 9 inches?",
                    _ => "How many drawing inches represent a REAL height of 18 inches?"
                }),
            _ => "A 6-foot-wide flag with three stripes (top 2 feet tall; " +
                 "middle 1 foot; bottom 1 foot) is to be drawn at " +
                 (s==0?"1 cm to 2 feet":"2 cm to 1 foot")+". How many drawing " +
                 "centimetres represent "+new[]{"its 6-foot width","the 2-foot top stripe",
                 "the 1-foot middle stripe","the 1-foot bottom stripe"}[f]+"?"
        };
        return new(Family,prompt+" Enter the exact value in the requested units; " +
            "a reduced equivalent fraction is allowed.",
            "A scale is a ratio between DRAWING length and ACTUAL length in " +
            "different stated units; use one ratio for every corresponding " +
            "feature. The same real object is drawn smaller at a scale that " +
            "represents more actual length per cm. The produced diagram, " +
            "placement of parts, annotations and peer critique remain teacher-reviewed.",
            AssessmentItemType.ShortAnswer,p);
    }

    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid G7 L09 parameters.");
        var (n,d)=Expected(p);
        return Fraction(n,d);
    }

    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Valid(family,p)||string.IsNullOrWhiteSpace(answer)||answer.Length>32)
            return false;
        if(!TryFraction(answer.Trim(),out long n,out long d))
            return false;
        var (wantN,wantD)=Expected(p);
        return n*wantD==wantN*d;
    }

    private static (long N,long D) Expected(IReadOnlyDictionary<string,int> p)
    {
        int kind=p["kind"],feature=p["feature"],style=p["style"];
        if(kind==0)
        {
            long[] quarterMetres=[16,10,11,15];
            return (quarterMetres[feature]*(style==0?4:2),4);
        }
        if(kind==1)
        {
            long[] miles=[270,350,110,70];
            return (miles[feature],style==0?50:75);
        }
        if(kind==2)
        {
            int inchesPerDrawInch=style==0?6:3;
            return feature switch
            {
                0 => (18,1),
                1 => (inchesPerDrawInch,1),
                2 => (9,inchesPerDrawInch),
                _ => (18,inchesPerDrawInch)
            };
        }
        long[] feet=[6,2,1,1];
        return (feet[feature]*(style==0?1:2),style==0?2:1);
    }

    private static bool Valid(string? family,IReadOnlyDictionary<string,int>? p) =>
        Supports(family)&&p is not null&&p.Count==3 &&
        p.TryGetValue("kind",out int kind)&&kind is >=0 and <=3 &&
        p.TryGetValue("feature",out int feature)&&feature is >=0 and <=3 &&
        p.TryGetValue("style",out int style)&&style is 0 or 1;

    private static string Fraction(long n,long d)
    {
        long g=Gcd(n,d);n/=g;d/=g;
        return d==1?n.ToString(CultureInfo.InvariantCulture):
            n.ToString(CultureInfo.InvariantCulture)+"/"+
            d.ToString(CultureInfo.InvariantCulture);
    }
    private static long Gcd(long a,long b)
    {
        while(b!=0){long t=a%b;a=b;b=t;}
        return a;
    }

    private static bool TryFraction(string value,out long n,out long d)
    {
        n=0;d=1;
        var split=value.Split('/');
        if(split.Length==2)
            return long.TryParse(split[0],NumberStyles.None,
                       CultureInfo.InvariantCulture,out n) &&
                   long.TryParse(split[1],NumberStyles.None,
                       CultureInfo.InvariantCulture,out d) &&
                   n>=0 && n<=1000000 && d>0 && d<=1000000;
        if(split.Length!=1 || !decimal.TryParse(value,NumberStyles.AllowDecimalPoint,
              CultureInfo.InvariantCulture,out decimal x) || x<0m || x>1000000m)
            return false;
        for(int i=0;i<=6;i++)
        {
            long denom=(long)Math.Pow(10,i);
            decimal scaled=x*denom;
            if(scaled==decimal.Truncate(scaled))
            {
                n=(long)scaled;d=denom;return true;
            }
        }
        return false;
    }
}

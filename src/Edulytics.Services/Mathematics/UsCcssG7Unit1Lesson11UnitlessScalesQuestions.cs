using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// G7 U01:L11 explicitly unitless scales: both lengths MUST first share the
/// same physical unit. First-party drawings avoid publisher spacecraft assets.
/// Drawing/measurement estimates are teacher-reviewed, never auto-certified.
/// </summary>
internal static class UsCcssG7Unit1Lesson11UnitlessScalesQuestions
{
    public const string Family="usccss.g7.u01.l11.unitless_scale_and_conversions";
    internal sealed record Problem(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);

    public static bool Supports(string? family) =>
        string.Equals(family,Family,StringComparison.Ordinal);

    public static Problem Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported G7 L11 family.");
        var p=new Dictionary<string,int>(StringComparer.Ordinal)
        {
            ["kind"]=random.Next(0,5),
            ["feature"]=random.Next(0,4)
        };
        string text=p["kind"] switch
        {
            0 => "An original scale map has scale 1 to 100 with NO units; " +
                 "each drawing unit and its actual 100 units use the SAME " +
                 "physical unit. " +
                 new[]{
                    "12 cm on the drawing represents how many ACTUAL cm?",
                    "7 cm on the drawing represents how many ACTUAL cm?",
                    "1 cm drawn represents how many ACTUAL cm?",
                    "1,000 actual centimetres should take how many DRAWING cm?"
                 }[p["feature"]],
            1 => "A first-party launch-module drawing has SAME-UNIT " +
                 "scale 1 to 50. " +
                 new[]{
                    "A leg is measured as 7 cm on the drawing. What is its actual length in cm?",
                    "A model height is 14 cm on the drawing. What is its actual height in cm?",
                    "A real figure is 71 inches tall. How many inches tall on this drawing?",
                    "The drawn leg is 7 cm. What is its ACTUAL length in metres?"
                 }[p["feature"]],
            2 => "A 120-foot by 75-foot parking lot is drawn at 1 inch to " +
                 "15 feet, which is also 1 to 180 with both units IN INCHES. " +
                 new[]{
                    "What is the dimensionless denominator of 1 inch to 15 feet?",
                    "How many drawing INCHES represent the 120-foot length at 1in:20ft?",
                    "How many drawing INCHES represent the 75-foot width at 1in:20ft?",
                    "How many drawing INCHES represent the width at the original 1in:15ft?"
                 }[p["feature"]],
            3 => "Use unitless scales and composition, with accurate physical " +
                 "unit conversions. " +
                 new[]{
                    "A model airplane is 8 in long at 1:72. How many real FEET long is it?",
                    "An 8-inch model at 1:72 is how many real INCHES long?",
                    "A scaled copy has length factor 5. What fraction of the copy's AREA is original?",
                    "A scale factor of 3 followed by a factor of 2 gives what overall factor?"
                 }[p["feature"]],
            _ => "Is " +
                 new[]{"1 to 12","1/12 to 1","100 to 0.12","9 to 108"}[p["feature"]] +
                 " equivalent to the scale 1 inch of drawing to 1 real foot? " +
                 "Express both sides in the same unit first. Enter 1 for YES, 0 for NO."
        };
        return new(Family,text+
            " Enter an exact integer or simplified fraction in the specified unit.",
            "A unitless scale uses one common unit for both lengths. " +
            "1 inch to 15 feet equals 1 inch to 180 inches = 1:180; " +
            "actual measurement is 180 times drawn, not 15 times drawn inches. " +
            "Independent scale drawings, ruler estimates and peer justification " +
            "remain teacher-reviewed.",
            AssessmentItemType.ShortAnswer,p);
    }

    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid L11 parameters");
        var (n,d)=Expected(p);
        return Format(n,d);
    }

    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Valid(family,p)||string.IsNullOrWhiteSpace(answer)||answer.Length>32)
            return false;
        if(!TryRational(answer.Trim(),out long n,out long d))return false;
        var (en,ed)=Expected(p);return n*ed==en*d;
    }

    private static (long,long) Expected(IReadOnlyDictionary<string,int> p)
    {
        int f=p["feature"];
        return p["kind"] switch
        {
            0 => f switch{0=>(1200,1),1=>(700,1),2=>(100,1),_=>(10,1)},
            1 => f switch{0=>(350,1),1=>(700,1),2=>(71,50),_=>(7,2)},
            2 => f switch{0=>(180,1),1=>(6,1),2=>(15,4),_=>(5,1)},
            3 => f switch{0=>(48,1),1=>(576,1),2=>(1,25),_=>(6,1)},
            _ => f==2 ? (0,1):(1,1)
        };
    }

    private static bool Valid(string? family,IReadOnlyDictionary<string,int>? p) =>
        Supports(family)&&p is not null&&p.Count==2 &&
        p.TryGetValue("kind",out int kind)&&kind is >=0 and <=4 &&
        p.TryGetValue("feature",out int f)&&f is >=0 and <=3;

    private static string Format(long n,long d)
    {
        long g=Gcd(n,d);n/=g;d/=g;
        return d==1?n.ToString(CultureInfo.InvariantCulture):
            n.ToString(CultureInfo.InvariantCulture)+"/"+
            d.ToString(CultureInfo.InvariantCulture);
    }
    private static long Gcd(long n,long d)
    {
        while(d!=0){long k=n%d;n=d;d=k;}return n;
    }
    private static bool TryRational(string input,out long n,out long d)
    {
        n=0;d=1;
        var parts=input.Split('/');
        if(parts.Length==2)
            return long.TryParse(parts[0],NumberStyles.None,
                     CultureInfo.InvariantCulture,out n) &&
                   long.TryParse(parts[1],NumberStyles.None,
                     CultureInfo.InvariantCulture,out d) &&
                   n>=0 && n<=1000000 && d>0 && d<=1000000;
        if(parts.Length!=1||!decimal.TryParse(input,NumberStyles.AllowDecimalPoint,
              CultureInfo.InvariantCulture,out decimal x)||x<0||x>1000000)return false;
        for(int i=0;i<=6;i++)
        {
            long den=(long)Math.Pow(10,i);
            decimal result=x*den;
            if(result==decimal.Truncate(result))
            {
                n=(long)result;d=den;return true;
            }
        }
        return false;
    }
}

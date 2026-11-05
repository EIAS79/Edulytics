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
    // Grade 7 L12 extends this exact unitless source engine to equivalent
    // customary/metric ratios, the 1:2000 flag table and real drawing areas.
    public const string EquivalentArea="usccss.g7.u01.l12.equivalent_scales_area";
    internal sealed record Problem(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);

    public static bool Supports(string? family) =>
        family is Family or EquivalentArea;

    public static Problem Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported G7 source family.");
        if(family==EquivalentArea) return BuildEquivalentArea(random);
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
        var (n,d)=family==EquivalentArea?ExpectedEquivalentArea(p):Expected(p);
        return Format(n,d);
    }

    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Valid(family,p)||string.IsNullOrWhiteSpace(answer)||answer.Length>32)
            return false;
        if(!TryRational(answer.Trim(),out long n,out long d))return false;
        var (en,ed)=family==EquivalentArea?ExpectedEquivalentArea(p):Expected(p);
        return n*ed==en*d;
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

    private static Problem BuildEquivalentArea(Random random)
    {
        var p=new Dictionary<string,int>(StringComparer.Ordinal)
        {
            ["kind"]=random.Next(0,5),
            ["feature"]=random.Next(0,4)
        };
        int k=p["kind"],v=p["feature"];
        string text=k switch
        {
            0 => new[]{
                "How many CENTIMETERS are in one mile? Use 5280 feet/mile, 12 inches/foot, 2.54 cm/inch.",
                "How many INCHES are in one mile? Use 5280 feet/mile and 12 inches/foot.",
                "How many CENTIMETERS are in one kilometer? Use metric units.",
                "How many FEET are in one mile?"
            }[v],
            1 => new[]{
                "Are 1 drawing inch to 2.5 actual feet and 5 drawing m to 150 actual m equivalent?",
                "Are 1 drawing inch to 1000 actual inches and 1 drawn cm to 10 actual m equivalent?",
                "Are 3 drawn cm to 4 actual km and 0.75 drawn cm to 1 actual km equivalent?",
                "Are 1 drawn cm to 12 actual km and 3 drawn cm to 4 actual km equivalent?"
            }[v]+" Enter 1 for YES, 0 for NO after converting units.",
            2 => "An original flag-like rectangular drawing has REAL length 396 m, "+
                "REAL central detail height 99 m, and FLAG DRAWING height 13.2 cm "+
                "at SAME-UNIT scale 1:2000. "+
                new[]{
                    "What is the DRAWN flag length in centimeters?",
                    "What is the REAL flag height in meters?",
                    "What is the DRAWN detail height in centimeters?",
                    "What is the ratio of REAL to DRAWN area in MATCHING square units?"
                }[v],
            3 => "A first-party pool plan has a 3-cm square pool side "+
                "representing a REAL 15-meter side. A second pool covers "+
                "3.2 drawing square centimeters; a larger pool covers "+
                "8 by 4 drawing centimeters. "+
                new[]{
                    "What is the equivalent unitless 1:n scale denominator?",
                    "What is the 3.2-square-cm pool's REAL area in square meters?",
                    "What is the 8-by-4-cm pool's REAL area in square meters?",
                    "How many real square meters does 1 drawn square centimeter represent?"
                }[v],
            _ => "Use the same rectangular flag-like 396-m by 264-m REAL "+
                "model with FLAG DRAWING height 13.2 cm at scale 1:2000. "+
                new[]{
                    "What is the REAL flag area in square meters?",
                    "What is the DRAWN flag area in square centimeters?",
                    "What is the exact REAL/DRAWN area ratio after matching square units?",
                    "If a real pool side 15m draws 3cm, how many real METERS does 1 drawn centimeter represent?"
                }[v]
        };
        return new(EquivalentArea,text+" Enter an exact number or rational fraction.",
            "Convert original measures to matching length units before "+
            "equating scales. For area apply the SQUARE of the scale "+
            "denominator. The original source table identifies 13.2 cm "+
            "as the flag DRAWING height, not the smaller detail height. "+
            "Freehand diagrams and explanations require teacher review.",
            AssessmentItemType.ShortAnswer,p);
    }

    private static (long,long) ExpectedEquivalentArea(IReadOnlyDictionary<string,int> p)
    {
        int k=p["kind"],v=p["feature"];
        return k switch
        {
            0 => v switch {0=>(1609344,10),1=>(63360,1),2=>(100000,1),_=>(5280,1)},
            1 => v==3?(0,1):(1,1),
            2 => v switch {0=>(198,10),1=>(264,1),2=>(495,100),_=>(4000000,1)},
            3 => v switch {0=>(500,1),1=>(80,1),2=>(800,1),_=>(25,1)},
            _ => v switch {0=>(104544,1),1=>(26136,100),
                            2=>(4000000,1),_=>(5,1)}
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
                   n>=0 && n<=1000000000 && d>0 && d<=1000000000;
        if(parts.Length!=1||!decimal.TryParse(input,NumberStyles.AllowDecimalPoint,
              CultureInfo.InvariantCulture,out decimal x)||x<0||x>1000000000)return false;
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

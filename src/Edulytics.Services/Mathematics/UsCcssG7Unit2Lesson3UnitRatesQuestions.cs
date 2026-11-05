using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

// Grade7 U02 L03. Exact linear conversions, reciprocal-unit rate and
// speed; independent sketching and explanatory arguments teacher-assessed.
internal static class UsCcssG7Unit2Lesson3UnitRatesQuestions
{
    public const string Family="usccss.g7.u02.l03.unit_rates_reciprocal";
    internal sealed record Problem(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? family)=>family==Family;
    public static Problem Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported G7 L03");
        var p=new Dictionary<string,int>(StringComparer.Ordinal)
        {["kind"]=random.Next(0,8),["feature"]=random.Next(0,4)};
        int k=p["kind"],f=p["feature"];
        string question=k switch
        {
            0 => "A proportional conversion of centimeters INTO millimeters. "+
                 "How many MILLIMETERS is "+
                 new[]{"9","12.5","50","88.49"}[f]+" centimeters?",
            1 => "A proportional conversion of millimeters INTO centimeters. "+
                 "How many CENTIMETERS is "+
                 new[]{"70","245","4","699.1"}[f]+" millimeters?",
            2 => new[]{
                "What is the constant MILLIMETERS per CENTIMETER?",
                "What is the inverse constant CENTIMETERS per MILLIMETER?",
                "How many square MILLIMETERS are in 1 square centimeter?",
                "How many square CENTIMETERS are in 1 square millimeter?"
            }[f]+" Give an exact number or fraction with correct units in your explanation.",
            3 => new[]{
                "An airplane flew 550 miles in one hour. What was its speed in miles per HOUR?",
                "At constant 550 miles/hour, how many miles in 1 hour 42 minutes?",
                "At constant 550 miles/hour, how many MINUTES to fly 330 miles?",
                "An airplane flies at 550 miles/hour. What is its speed in MILES per MINUTE?"
            }[f],
            4 => "A runner travels at 6 miles per hour. "+
                 new[]{
                    "How many miles in 1 hour?",
                    "How many miles in half an hour?",
                    "How many miles in 1 and one-third hours?",
                    "How many HOURS does a 9-mile run require?"
                 }[f],
            5 => new[]{
                "One kilometer is how many meters?",
                "250 meters is how many kilometers?",
                "12 kilometers is how many meters?",
                "0.3 kilometers is how many meters?"
            }[f],
            6 => new[]{
                "What is the exact constant INCHES per one FOOT?",
                "What is the exact inverse constant FEET per one INCH?",
                "How many inches are 4.5 feet?",
                "How many feet are 30 inches?"
            }[f],
            _ => new[]{
                "A rectangular desert's REAL area is 25000 square miles, and its PLAN area is 10 square inches. How many actual square MILES are represented by one drawing square INCH?",
                "A desert's REAL area is 25000 square miles, and drawing area is 10 square inches. What is the linear scale in actual MILES per drawing INCH?",
                "Is 3 drawing centimeters to 15 actual kilometers equivalent to 1 drawing centimeter to 5 actual kilometers? Enter 1 yes, 0 no.",
                "Three original ovals compare outer width, inner width and height: L=(5,3,4), M=(10,4,8), N=(15,9,12). Which is not a uniform scaled copy of L, M=2 or N=3?"
            }[f]
        };
        return new(Family,question+" Enter the exact numeric response. "+
            "Identify numerator, denominator and interpretation of the unit rate "+
            "to your teacher.",
            "A constant of proportionality always has a DIRECTION and units. "+
            "For cm to mm multiply by 10; for mm to cm multiply by 1/10. "+
            "Speed is distance per time; pace is time per distance. "+
            "For square area, square the conversion factor. "+
            "Original diagrams, route explanations and context labels require teacher review.",
            AssessmentItemType.ShortAnswer,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid source unit rate parameters");
        var(n,d)=Expected(p);long g=Gcd(n,d);n/=g;d/=g;
        return d==1?n.ToString(CultureInfo.InvariantCulture):
            n.ToString(CultureInfo.InvariantCulture)+"/"+
            d.ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if (!Valid(family, p))
            return false;
        var (n, d) = Expected(p);
        return G7ExactRationalAnswer.Matches(answer, n, d, 1000000);
    }

    private static(long,long) Expected(IReadOnlyDictionary<string,int> p)
    {
        int f=p["feature"];
        return p["kind"] switch
        {
            0 => f switch {0=>(90,1),1=>(125,1),2=>(500,1),_=>(8849,10)},
            1 => f switch {0=>(7,1),1=>(49,2),2=>(2,5),_=>(6991,100)},
            2 => f switch {0=>(10,1),1=>(1,10),2=>(100,1),_=>(1,100)},
            3 => f switch {0=>(550,1),1=>(935,1),2=>(36,1),_=>(55,6)},
            4 => f switch {0=>(6,1),1=>(3,1),2=>(8,1),_=>(3,2)},
            5 => f switch {0=>(1000,1),1=>(1,4),2=>(12000,1),_=>(300,1)},
            6 => f switch {0=>(12,1),1=>(1,12),2=>(54,1),_=>(5,2)},
            _ => f switch {0=>(2500,1),1=>(50,1),2=>(1,1),_=>(2,1)}
        };
    }
    private static long Gcd(long a,long b)
    {while(b!=0){long r=a%b;a=b;b=r;}return a;}
    private static bool Valid(string? family,IReadOnlyDictionary<string,int>? p)=>
        Supports(family)&&p is not null&&p.Count==2 &&
        p.TryGetValue("kind",out int k)&&k is >=0 and <=7 &&
        p.TryGetValue("feature",out int f)&&f is >=0 and <=3;
}

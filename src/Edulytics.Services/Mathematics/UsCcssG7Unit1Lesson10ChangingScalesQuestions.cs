using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// G7 U01:L10: changing the drawing scale changes drawing lengths linearly
/// and its area quadratically, while the actual object's area is invariant.
/// Includes a precision-choice source mode for appropriately scaled rulers.
/// All measured diagrams and justifications remain teacher-reviewed.
/// </summary>
internal static class UsCcssG7Unit1Lesson10ChangingScalesQuestions
{
    public const string Family="usccss.g7.u01.l10.change_scale_length_and_area";
    internal sealed record Problem(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    private static readonly int[] TriangularPlotScales=[5,15,50,200];
    public static bool Supports(string? family) =>
        string.Equals(family,Family,StringComparison.Ordinal);

    public static Problem Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported L10 question family.");
        var p=new Dictionary<string,int>(StringComparer.Ordinal)
        {
            ["kind"]=random.Next(0,5),
            ["feature"]=random.Next(0,4),
            ["style"]=random.Next(0,4)
        };
        string question=p["kind"] switch
        {
            0 => "A rectangular first-party park has a drawing 10 cm by 4 cm " +
                 "at 1 cm to 90 metres. The SAME REAL park is redrawn at " +
                 "1 cm to 30 metres. " +
                 (p["feature"] switch
                 {
                    0 => "What is the NEW drawing width in cm?",
                    1 => "What is the NEW drawing height in cm?",
                    2 => "By what factor did the DRAWING area increase?",
                    _ => "How many square metres of REAL area does 1 new square cm represent?"
                 }),
            1 => "A triangular plot has real base 120 m and height 90 m. " +
                 "A drawing uses 1 cm to "+TriangularPlotScales[p["style"]]+" m. " +
                 (p["feature"] switch
                 {
                    0 => "What is the base in drawing centimetres?",
                    1 => "What is the perpendicular height in drawing centimetres?",
                    2 => "What is the drawn triangular area in square cm?",
                    _ => "How many real square metres does 1 drawing square cm represent?"
                 }),
            2 => "Two drawings show the SAME actual playground, one at " +
                 "1 cm to 30 m and the other at 1 cm to 20 m. " +
                 (p["feature"] switch
                 {
                    0 => "By what factor do lengths IN THE NEW DRAWING increase?",
                    1 => "By what factor does the NEW DRAWING area increase?",
                    2 => "At the first scale, what real area (square metres) is 1 drawn square cm?",
                    _ => "At the second scale, what real area (square metres) is 1 drawn square cm?"
                 }),
            3 => "A restaurant's floor plan uses 1 drawing inch for 12 real " +
                 "feet, with drawing area 60 square inches. " +
                 (p["feature"] switch
                 {
                    0 => "What is the restaurant's ACTUAL area in square feet?",
                    1 => "What REAL area (square feet) is represented by 1 drawing square inch?",
                    2 => "Someone claims actual area is 720 square feet: " +
                         "enter 1 if true, 0 if false.",
                    _ => "What false numeric area would result from mistakenly " +
                         "multiplying 60 by the LINEAR factor 12?"
                 }),
            _ => "A ruler is marked only to a tenth of a centimetre or " +
                 "a quarter of an inch. Is reporting " +
                 new[]{"9 and 1/4 inches","9 and 5/64 inches",
                       "23.5 centimetres","23.47659 centimetres"}[p["feature"]] +
                 " plausible at that measurement precision? " +
                 "Enter 1 for defensible, 0 for unjustified false precision."
        };
        return new(Family,question+
            " Enter an exact integer or fraction in the specified units. " +
            "Measure and justify any original construction with your teacher.",
            "If actual metres per drawing cm changes from a to b, " +
            "new drawing lengths multiply by a/b and areas by (a/b)^2. " +
            "Real area represented by 1 cm² is (metres per centimetre)^2. " +
            "Drawing ruler precision cannot exceed its useful markings.",
            AssessmentItemType.ShortAnswer,p);
    }

    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid L10 parameters.");
        var (n,d)=Expected(p);
        return Format(n,d);
    }

    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Valid(family,p)||string.IsNullOrWhiteSpace(answer)||answer.Length>32)
            return false;
        if(!TryRational(answer.Trim(),out long n,out long d))return false;
        var (an,ad)=Expected(p);
        return n*ad==an*d;
    }

    private static (long N,long D) Expected(IReadOnlyDictionary<string,int> p)
    {
        int kind=p["kind"],f=p["feature"];
        if(kind==0)return f switch
        {
            0 => (30,1), 1 => (12,1), 2 => (9,1), _ => (900,1)
        };
        if(kind==1)
        {
            long s=TriangularPlotScales[p["style"]];
            return f switch
            {
                0 => (120,s), 1 => (90,s),
                2 => (5400,s*s), _ => (s*s,1)
            };
        }
        if(kind==2)return f switch
        {
            0 => (3,2), 1 => (9,4), 2 => (900,1), _ => (400,1)
        };
        if(kind==3)return f switch
        {
            0 => (8640,1), 1 => (144,1), 2 => (0,1), _ => (720,1)
        };
        return f is 0 or 2 ? (1,1):(0,1);
    }

    private static bool Valid(string? family,IReadOnlyDictionary<string,int>? p) =>
        Supports(family)&&p is not null&&p.Count==3 &&
        p.TryGetValue("kind",out int k)&&k is >=0 and <=4 &&
        p.TryGetValue("feature",out int f)&&f is >=0 and <=3 &&
        p.TryGetValue("style",out int s)&&s is >=0 and <=3;

    private static string Format(long n,long d)
    {
        long g=Gcd(n,d);n/=g;d/=g;
        return d==1?n.ToString(CultureInfo.InvariantCulture):
            n.ToString(CultureInfo.InvariantCulture)+"/"+
            d.ToString(CultureInfo.InvariantCulture);
    }
    private static long Gcd(long a,long b)
    {
        while(b!=0){long tmp=a%b;a=b;b=tmp;}
        return a;
    }
    private static bool TryRational(string input,out long n,out long d)
    {
        n=0;d=1;
        string[] parts=input.Split('/');
        if(parts.Length==2)
            return long.TryParse(parts[0],NumberStyles.None,
                        CultureInfo.InvariantCulture,out n)&&
                   long.TryParse(parts[1],NumberStyles.None,
                        CultureInfo.InvariantCulture,out d)&&
                   n>=0 && n<=1000000 && d>0 && d<=1000000;
        if(parts.Length!=1||!decimal.TryParse(input,NumberStyles.AllowDecimalPoint,
             CultureInfo.InvariantCulture,out decimal x)||x<0m||x>1000000m)return false;
        for(int exp=0;exp<=6;exp++)
        {
            long den=(long)Math.Pow(10,exp);
            decimal candidate=x*den;
            if(candidate==decimal.Truncate(candidate))
            {
                n=(long)candidate;d=den;return true;
            }
        }
        return false;
    }
}

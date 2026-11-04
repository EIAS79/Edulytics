using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Exact source-scoped Grade 7 U01 L06 area scaling, including rational factors.
/// Reuses existing exact question dispatch; physical pattern blocks, drawings
/// and justifications remain teacher-led, never counted as auto-grading.
/// </summary>
internal static class UsCcssG7Unit1Lesson6AreaScalingQuestions
{
    public const string Family = "usccss.g7.u01.l06.scaled_area_square_factor";
    internal sealed record Problem(string Family, string Prompt, string Solution,
        AssessmentItemType ItemType, IReadOnlyDictionary<string,int> Parameters);

    // Factor: 1, 2, 3, 4, 1/2, 1/3, 3/5.
    private static readonly (long N,long D)[] Factors =
    [
        (1,1), (2,1), (3,1), (4,1), (1,2), (1,3), (3,5)
    ];

    public static bool Supports(string? family) =>
        string.Equals(family, Family, StringComparison.Ordinal);

    public static Problem Build(string family, Random random)
    {
        if (!Supports(family))
            throw new InvalidOperationException("Unsupported Grade 7 L06 family.");
        var p = new Dictionary<string,int>(StringComparer.Ordinal)
        {
            ["kind"] = random.Next(0, 3),
            ["units"] = random.Next(1, 5),
            ["factor"] = random.Next(0, Factors.Length),
            ["asked"] = random.Next(0, 2)
        };
        var (n,d) = Factors[p["factor"]];
        long original = OriginalArea(p);
        string shape = p["kind"] switch
        {
            0 => "a first-party figure made from " + p["units"] +
                 " original-size pattern blocks (each block is 1 square unit)",
            1 => "a triangle of area " + original + " square units",
            _ => "a parallelogram of area " + original + " square units"
        };
        string prompt = "An original shape is " + shape + ". " +
            "Draw a uniformly scaled copy at length scale factor k=" +
            Fraction(n,d) + ". " +
            (p["asked"] == 0
                ? "What is the area of the copy in square units?"
                : "By what AREA factor (not length factor) has the original area changed?") +
            " Enter an exact whole number or reduced fraction such as 9/25. " +
            "Scaling lengths by k scales the area by k squared.";
        return new(Family,prompt,
            "Multiply each corresponding length by k, so base and height " +
            "are each scaled by k and area by k squared; n original unit " +
            "blocks require n*k*k blocks worth of area. Assembling puzzle " +
            "tiles and justifying a scaled drawing require teacher review.",
            AssessmentItemType.ShortAnswer,p);
    }

    public static string Solve(string family, IReadOnlyDictionary<string,int> p)
    {
        if (!Valid(family,p))
            throw new InvalidOperationException("Invalid G7 L06 parameter set.");
        var (n,d) = Factors[p["factor"]];
        long source = p["asked"] == 0 ? OriginalArea(p) : 1;
        return Fraction(source*n*n,d*d);
    }

    public static bool Verify(string family, IReadOnlyDictionary<string,int> p,string? answer)
    {
        if (!Valid(family,p) || string.IsNullOrWhiteSpace(answer))
            return false;
        string trimmed = answer.Trim();
        if (trimmed.Length>32 || !TryRational(trimmed,out long an,out long ad))
            return false;
        var (n,d) = Factors[p["factor"]];
        long source = p["asked"] == 0 ? OriginalArea(p) : 1;
        return an*d*d == ad*source*n*n;
    }

    private static long OriginalArea(IReadOnlyDictionary<string,int> p) =>
        p["kind"] switch
        {
            0 => p["units"],     // one original block = one square unit
            1 => 6,             // triangle 4 x 3 / 2
            _ => 8              // parallelogram 4 x 2
        };

    private static bool Valid(string? family,IReadOnlyDictionary<string,int>? p) =>
        Supports(family) && p is not null && p.Count==4 &&
        p.TryGetValue("kind",out int kind) && kind is >=0 and <=2 &&
        p.TryGetValue("units",out int units) && units is >=1 and <=4 &&
        p.TryGetValue("factor",out int factor) && factor is >=0 and <=6 &&
        p.TryGetValue("asked",out int asked) && asked is 0 or 1;

    private static string Fraction(long n,long d)
    {
        long g=Gcd(n,d);n/=g;d/=g;
        return d==1 ? n.ToString(CultureInfo.InvariantCulture) :
            n.ToString(CultureInfo.InvariantCulture)+"/"+
            d.ToString(CultureInfo.InvariantCulture);
    }

    private static long Gcd(long a,long b)
    {
        while(b!=0) {long x=a%b;a=b;b=x;}
        return a;
    }

    private static bool TryRational(string answer,out long n,out long d)
    {
        n=0;d=1;
        string[] values=answer.Split('/');
        if (values.Length==2)
            return long.TryParse(values[0],NumberStyles.None,
                    CultureInfo.InvariantCulture,out n) &&
                long.TryParse(values[1],NumberStyles.None,
                    CultureInfo.InvariantCulture,out d) &&
                n>=0 && n<=1000000 && d>0 && d<=1000000;
        if (values.Length!=1 || !decimal.TryParse(answer,NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture,out decimal value) ||
                value<0m || value>1000000m)
            return false;
        for(int digits=0;digits<=6;digits++)
        {
            long multiplier=(long)Math.Pow(10,digits);
            decimal scaled=value*multiplier;
            if (scaled==decimal.Truncate(scaled))
            {
                n=(long)scaled;d=multiplier;
                return true;
            }
        }
        return false;
    }
}

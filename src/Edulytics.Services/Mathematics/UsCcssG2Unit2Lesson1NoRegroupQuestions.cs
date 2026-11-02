using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>Independent numeric variants faithful to IM2021 G2 U02 L01:
/// ten-and-ones arithmetic within 100 without composing/breaking a ten.</summary>
internal static class UsCcssG2Unit2Lesson1NoRegroupQuestions
{
    public const string SnackTotal="usccss.g2.u02.l01.movie_snacks_combine";
    public const string SnackGap="usccss.g2.u02.l01.movie_snacks_difference";
    public const string TrainsTotal="usccss.g2.u02.l01.cube_trains_total";
    public const string TrainsGap="usccss.g2.u02.l01.cube_trains_difference";
    public sealed record Problem(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? family)=>family is SnackTotal or SnackGap or TrainsTotal or TrainsGap;

    public static Problem Build(string family,Random r)
    {
        if(!Supports(family))throw new InvalidOperationException("Not Grade2 U02 L01 exact family");
        var p=new Dictionary<string,int>(StringComparer.Ordinal);
        string prompt,solution;
        if(family is SnackTotal or SnackGap)
        {
            var popOnes=r.Next(0,6);
            var pretzelOnes=r.Next(0,10-popOnes);
            var nachosOnes=r.Next(popOnes,10);
            var nachos=30+nachosOnes;
            var pretzels=10+pretzelOnes;
            var popcorn=20+popOnes;
            p["nachos"]=nachos;p["pretzels"]=pretzels;p["popcorn"]=popcorn;
            prompt=$"FIRST-PARTY fictional club snack bar graph (scale one, zero baseline): "+
                $"Nachos {nachos}, Pretzels {pretzels}, Popcorn {popcorn}. ";
            if(family==SnackTotal)
            {
                prompt+="How many chose popcorn OR pretzels altogether?";
                solution=$"Add tens and ones without exchange: {popcorn}+{pretzels}={popcorn+pretzels}.";
            }
            else
            {
                prompt+="How many MORE chose nachos than popcorn?";
                solution=$"Subtract tens and ones without borrowing: {nachos}-{popcorn}={nachos-popcorn}.";
            }
        }
        else
        {
            var smallerOnes=r.Next(0,5);
            var largerOnes=r.Next(smallerOnes,10-smallerOnes);
            var larger=40+largerOnes;
            var smaller=20+smallerOnes;
            p["larger"]=larger;p["smaller"]=smaller;
            prompt=$"FIRST-PARTY imaginary connecting-cube trains: "+
                $"Partner A has {larger} cubes (tens and singles); partner B has {smaller}. ";
            if(family==TrainsTotal)
            {
                prompt+="How many cubes altogether? Do not compose a new ten.";
                solution=$"Combine tens with tens and ones with ones: {larger}+{smaller}={larger+smaller}.";
            }
            else
            {
                prompt+="How many MORE cubes does A have? Show the missing gap.";
                solution=$"Subtract tens and ones: {larger}-{smaller}={larger-smaller}; "+
                    $"{smaller}+{larger-smaller}={larger}.";
            }
        }
        return new Problem(family,prompt,solution,AssessmentItemType.Numeric,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid no-regroup parameters");
        var value=family switch{
            SnackTotal=>p["popcorn"]+p["pretzels"],
            SnackGap=>p["nachos"]-p["popcorn"],
            TrainsTotal=>p["larger"]+p["smaller"],
            TrainsGap=>p["larger"]-p["smaller"],
            _=>throw new InvalidOperationException("Unsupported no-regroup operation")
        };
        return value.ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string answer)=>
        Valid(family,p)&&string.Equals(answer,Solve(family,p),StringComparison.Ordinal);
    private static bool Valid(string? family,IReadOnlyDictionary<string,int> p)
    {
        if(!Supports(family))return false;
        if(family is SnackTotal or SnackGap)
        {
            if(p.Count!=3||!p.TryGetValue("nachos",out var n)||
               !p.TryGetValue("pretzels",out var t)||!p.TryGetValue("popcorn",out var c)||
               n is <30 or >39||t is <10 or >19||c is <20 or >29)return false;
            return c%10+t%10<10 && n%10>=c%10 && n>c && c+t<=100;
        }
        if(p.Count!=2||!p.TryGetValue("larger",out var big)||
           !p.TryGetValue("smaller",out var small)||
           big is <40 or >49||small is <20 or >29)return false;
        return big%10+small%10<10 && big%10>=small%10 &&
            big>small && big+small<=100;
    }
}

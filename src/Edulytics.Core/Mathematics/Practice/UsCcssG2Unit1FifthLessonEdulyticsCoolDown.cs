using System.Globalization;
using System.Text.RegularExpressions;

namespace Edulytics.Core.Mathematics.Practice;

/// <summary>
/// Original Edulytics Grade 2 U01 L05 exit ticket:
/// create two two-digit numbers from FOUR of seven physical cards and get as
/// close as possible to 50. Separate duplicate printed digits from card
/// positions and support equally optimal expressions. No publisher graphics.
/// </summary>
public static class UsCcssG2Unit1FifthLessonEdulyticsCoolDown
{
    public const string LessonCode="PED:US-CCSS-MATH:G2:U01:L05";
    public const string Version="edulytics-g2-u01-l05-two-digit-cards-cooldown-v1";
    public static readonly IReadOnlyList<int> SevenCards=[3,8,4,6,6,2,1];
    private static readonly Regex TwoAddends=new(
        @"^\s*([1-9][0-9])\s*\+\s*([1-9][0-9])\s*$",
        RegexOptions.CultureInvariant|RegexOptions.Compiled);

    public sealed record Review(bool MachineScorable,bool Correct,
        bool TeacherReviewNeeded,string Feedback);

    public static Review CheckFourCardTwoDigitExpression(string? raw)
    {
        var m=TwoAddends.Match(raw??"");
        if(!m.Success)return new(true,false,true,
            "Write two genuine two-digit numbers added with a + sign.");
        var a=int.Parse(m.Groups[1].Value,CultureInfo.InvariantCulture);
        var b=int.Parse(m.Groups[2].Value,CultureInfo.InvariantCulture);
        var chosen=new[]{a/10,a%10,b/10,b%10};
        var available=SevenCards.GroupBy(x=>x)
            .ToDictionary(g=>g.Key,g=>g.Count());
        if(chosen.GroupBy(x=>x).Any(g=>!available.TryGetValue(g.Key,out var count)
            || g.Count()>count))
            return new(true,false,true,"Choose four different physical card positions from the seven shown.");

        var c=SevenCards.ToArray();
        var best=int.MaxValue;
        for(var i=0;i<7;i++)
        for(var j=0;j<7;j++)
        for(var k=0;k<7;k++)
        for(var l=0;l<7;l++)
        {
            if(new HashSet<int>{i,j,k,l}.Count!=4 ||
                c[i]==0 || c[k]==0)continue;
            var value=10*c[i]+c[j]+10*c[k]+c[l];
            best=Math.Min(best,Math.Abs(50-value));
        }
        var distance=Math.Abs(50-(a+b));
        return new(true,distance==best,true,distance==best
            ?"Your two two-digit numbers are as close as possible to 50. Explain which four cards you used."
            :$"Your sum is {a+b}, which is {distance} away from 50. Look for a closer legal arrangement.");
    }

    public static Review CheckMentalTen(string? raw)
        => CheckInteger(raw,44,"38 + 6 = 44: make a new ten from 8 ones and 2 ones.");

    public static Review CheckRegrouping(string? raw)
        => CheckInteger(raw,42,"27 + 15 = 42: seven ones and five ones make one ten and two ones.");

    public static Review CheckFiftySum(string? raw)
        => CheckInteger(raw,50,"39 + 11 = 50: thirty-nine plus eleven makes five tens.");

    private static Review CheckInteger(string? raw,int expected,string correct)
    {
        if(!int.TryParse(raw,NumberStyles.None,CultureInfo.InvariantCulture,out var n))
            return new(true,false,false,"Enter one non-negative whole number.");
        return new(true,n==expected,false,n==expected
            ?correct:"Check the tens and ones and compose a ten when useful.");
    }

    public static readonly IReadOnlyList<string> TeacherRubric=
    [
        "Student builds exactly two two-digit numbers from four different physical cards out of seven; two separately drawn cards with the same printed digit are valid.",
        "Student compares distance to target 50 and explains why the chosen arrangement is as close as possible, accepting equally optimal answers.",
        "Student accurately represents sums up to 50 using tens, ones and regrouping, and explains a method using pictures, numbers, words or cubes.",
        "Teacher observes partner card selection, comparisons, explanations and Mathematical Community norms; a numeric score does not certify these tasks."
    ];
}

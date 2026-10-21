using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Independently authored two-digit mental addition for original U01 L06
/// number talk. It DOES NOT grade digit-card use across a Number Puzzles board,
/// physical center choice, partner speech or Math Community chart.
/// </summary>
internal static class UsCcssG2Unit1Lesson6NumberTalkQuestions
{
    public const string Family = "usccss.g2.u01.l06.number_talk_two_two_digit_within50";
    public sealed record Problem(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? family)=>family==Family;

    public static Problem Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Not original G2 U01 L06 family");
        var first=random.Next(10,36);
        var second=random.Next(10,51-first);
        var p=new Dictionary<string,int>(StringComparer.Ordinal)
        {
            ["first"]=first,["second"]=second
        };
        return new Problem(family,
            $"Number Talk: mentally find {first} + {second}, two two-digit numbers. " +
            "Explain tens and ones, then enter the sum (50 or less).",
            "Separate tens and ones, and exchange ten ones for a ten when needed. " +
            "The arithmetic score cannot evaluate the spoken mental strategy.",
            AssessmentItemType.Numeric,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Tampered L06 number talk");
        return (p["first"]+p["second"]).ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string answer)=>
        Valid(family,p) && string.Equals(answer,Solve(family,p),StringComparison.Ordinal);
    private static bool Valid(string? family,IReadOnlyDictionary<string,int> p) =>
        Supports(family) && p.Count==2 &&
        p.TryGetValue("first",out var first) &&
        p.TryGetValue("second",out var second) &&
        first is >=10 and <=35 && second>=10 && first+second<=50;
}

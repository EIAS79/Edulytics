using System.Globalization;
using System.Text.RegularExpressions;

namespace Edulytics.Core.Mathematics.Practice;

/// <summary>First-party arithmetic checks for original IM2021 G2 U02 L02;
/// students' actual tool use, drawings and MLR8 partner dialog remain teacher-scored.</summary>
public static class UsCcssG2Unit2SecondLessonUnknownAddends
{
    public const string LessonCode="PED:US-CCSS-MATH:G2:U02:L02";
    public const string Version="edulytics-original-g2-u02-l02-unknown-addends-v1";
    public static readonly IReadOnlyList<string> TeacherRubric=[
        "Observe oral choral count back by 10 from 97 to 7, with explanation of the fixed ones digit",
        "Observe learners meaningfully compare real connecting cubes and base-ten blocks, including fixed rods",
        "Check original student representations of 41+?=84 with either counting on or total-minus-known strategy",
        "Observe real pairs presenting Han's and Mai's two distinct models for 17+?=48",
        "Check each partner independently solves 21+?=96 and explains a valid strategy without regrouping",
        "Check student explanation of inverse equations 67−55=12 and55+12=67",
        "Do not treat correct typed numerical responses as proof of actual blocks or peer discussion"
    ];
    public sealed record Review(bool Correct,bool TeacherReviewNeeded,string Feedback);
    private static bool N(string? s,int min,int max,out int n)=>
        int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out n)&&n>=min&&n<=max;
    private static string Eq(string? s)=>Regex.Replace(s??"",@"\s+","").Replace('−','-');
    public static Review CheckWarmup(string? missing1,string? missing2,string? missing3)
    {
        var ok=N(missing1,0,100,out var a)&&a==77&&
            N(missing2,0,100,out var b)&&b==37&&
            N(missing3,0,100,out var c)&&c==7;
        return new(ok,true,ok
            ?"97,87,77,67,57,47,37,27,17,7 counts by tens; teacher checks your oral pattern explanation."
            :"Subtract exactly 10 each step from 97; every number should end in 7.");
    }
    public static Review CheckSourceUnknowns(string? first,string? second,string? third,string? inverse)
    {
        var ok=N(first,0,100,out var a)&&a==43&&
            N(second,0,100,out var b)&&b==31&&
            N(third,0,100,out var c)&&c==75&&
            N(inverse,0,100,out var d)&&d==12;
        return new(ok,true,ok
            ?"Original unknown addends:41+43=84;17+31=48;21+75=96;55+12=67."
            :"Find missing part by total−known: 84−41,48−17,96−21,67−55.");
    }
    public static Review CheckTwoMethods(string? addOn,string? takeAway)
    {
        var add=new[]{"17+31=48","31+17=48"}.Contains(Eq(addOn));
        var subtract=new[]{"48-17=31"}.Contains(Eq(takeAway));
        return new(add&&subtract,true,add&&subtract
            ?"Adding on and taking away agree on31. Teacher separately checks Han/Mai drawings and authentic tool comparisons."
            :"Show both equivalent equations:17+31=48 and48−17=31.");
    }
    public static Review CheckIndependentCoolDown(string? first,string? second,string? third)
    {
        var ok=N(first,0,100,out var a)&&a==44&&
            N(second,0,100,out var b)&&b==23&&
            N(third,0,100,out var c)&&c==22;
        return new(ok,false,ok
            ?"Original Edulytics exit correct:34+44=78;62+23=85;76−54=22."
            :"Check 78−34,85−62 and76−54. Do not use regrouping for these.");
    }
}

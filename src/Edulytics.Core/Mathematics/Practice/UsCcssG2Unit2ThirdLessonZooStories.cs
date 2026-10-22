using System.Globalization;
using System.Text.RegularExpressions;

namespace Edulytics.Core.Mathematics.Practice;

/// <summary>Only machine-checkable maths of source IM2021 G2 U02 L03;
/// original story explanations, tools and MLR6 Three Reads teacher-assessed.</summary>
public static class UsCcssG2Unit2ThirdLessonZooStories
{
    public const string LessonCode="PED:US-CCSS-MATH:G2:U02:L03";
    public const string Version="edulytics-g2-u02-l03-four-zoo-stories-v1";
    public static readonly IReadOnlyList<string> TeacherRubric=[
        "Observe warm-up visual explanation of tens, ones and grouping 10 ones into one ten",
        "Observe the original MLR6 Three Reads of the bus story; separate context, quantities and question",
        "Check student-created diagram reflects the START unknown and both 34 boarding and 55 final",
        "Evaluate independent representation of monkey house result-unknown, bears/lions compare, and birds part-unknown",
        "Check no ten was borrowed or carried in each of four original numerical strategies",
        "Observe authentic partner comparison of base-ten blocks/connecting cubes and student-written equations",
        "Correct digital numbers cannot certify oral reading, actual blocks, real drawing or official grade"
    ];
    public sealed record Review(bool Correct,bool TeacherReviewNeeded,string Feedback);
    private static bool N(string? s,int lo,int hi,out int value)=>
        int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out value)&&value>=lo&&value<=hi;
    private static string Eq(string? value)=>Regex.Replace(value??"",@"\s+","").Replace('−','-');
    public static Review CheckBus(string? first,string? equation)
    {
        var ok=N(first,0,100,out var count)&&count==21 &&
            new[]{"21+34=55","34+21=55","55-34=21"}.Contains(Eq(equation));
        return new(ok,true,ok?"The original bus START was21:21+34=55. Teacher checks MLR6 and original representation."
            :"The bus ended with55 after34 MORE boarded. Starting number55−34=21.");
    }
    public static Review CheckOtherStories(string? monkey,string? compare,string? birds)
    {
        var ok=N(monkey,0,100,out var m)&&m==42 &&
            N(compare,0,100,out var c)&&c==31 &&
            N(birds,0,100,out var b)&&b==54;
        return new(ok,true,ok
            ?"Source Activity2 exact outcomes:65−23=42,58−27=31,88−34=54. Teacher evaluates the story diagrams."
            :"Monkey65−23=42; bears58−lions27=31 MORE; all birds88−inside34=54 outdoors.");
    }
    public static Review CheckCoolDown(string? takeFrom,string? compare,string? missingPart)
    {
        var ok=N(takeFrom,0,100,out var a)&&a==42 &&
            N(compare,0,100,out var b)&&b==42 &&
            N(missingPart,0,100,out var c)&&c==43;
        return new(ok,false,ok
            ?"Independent Edulytics maths correct:74−32=42,67−25=42,86−43=43."
            :"Recheck 74−32,67−25 and86−43 with tens and ones. All avoid regrouping.");
    }
}

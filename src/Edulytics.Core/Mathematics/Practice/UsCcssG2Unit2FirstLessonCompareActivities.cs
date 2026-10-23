using System.Globalization;
using System.Text.RegularExpressions;

namespace Edulytics.Core.Mathematics.Practice;

/// <summary>2021 source mathematical data and original Edulytics no-regroup
/// checks. Real cube-train building and oral reasoning remain teacher-assessed.</summary>
public static class UsCcssG2Unit2FirstLessonCompareActivities
{
    public const string LessonCode="PED:US-CCSS-MATH:G2:U02:L01";
    public const string Version="edulytics-g2-u02-l01-source-exact-no-regroup-v1";
    public static readonly IReadOnlyList<string> TeacherRubric=[
        "Student explains a mathematically defensible Which One Doesn't Belong classification of base-ten/cube/tape/graph representations",
        "Observe an original Movie Snacks solution drawing or numerical strategy, including exactly the two source questions",
        "Observe partner A build 46 physical cubes and partner B build 22 in complete ten-towers and singles",
        "Compare multiple correct methods for total and difference, not only a final numerical answer",
        "Check student's original labeled cube drawings, open explanation and a genuine MLR7 gallery discussion",
        "Digital integers and equations never prove classroom construction, partner participation or a teacher-awarded grade"
    ];
    public sealed record Review(bool Correct,bool TeacherReviewNeeded,string Feedback);
    private static bool N(string? s,int min,int max,out int value)=>
        int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out value)
        && value>=min&&value<=max;
    private static string Canon(string? s)=>Regex.Replace(s??"",@"\s+","").Replace('−','-');
    public static Review CheckMovieSnacks(string? combined,string? difference,string? addEq,string? subtractEq)
    {
        var correct=N(combined,0,100,out var sum)&&sum==37 &&
            N(difference,0,100,out var gap)&&gap==11 &&
            new[]{"21+16=37","16+21=37"}.Contains(Canon(addEq)) &&
            new[]{"32-21=11","21+11=32","11+21=32"}.Contains(Canon(subtractEq));
        return new(correct,true,correct
            ?"Original graph has nachos32, pretzels16, popcorn21. Combined 21+16=37; difference32−21=11. Teacher assesses drawings and strategy."
            :"Use the exact bar labels: popcorn21 plus pretzels16 is37, and nachos32 minus popcorn21 is11.");
    }
    public static Review CheckBuildTrains(string? total,string? difference,string? inverseEquation)
    {
        var correct=N(total,0,100,out var sum)&&sum==68 &&
            N(difference,0,100,out var gap)&&gap==24 &&
            new[]{"22+24=46","24+22=46"}.Contains(Canon(inverseEquation));
        return new(correct,true,correct
            ?"Original partner trains46 and22: total68, gap24, checked by22+24=46. Physical cube building and peer work remain teacher-scored."
            :"Build 46 and22. Total46+22=68; gap46−22=24; inverse 22+24=46.");
    }
    public static Review CheckIndependentCoolDown(string? add,string? subtract,string? compare)
    {
        var correct=N(add,0,100,out var a)&&a==57 &&
            N(subtract,0,100,out var s)&&s==43 &&
            N(compare,0,100,out var d)&&d==21;
        return new(correct,false,correct
            ?"New Edulytics problems correct:34+23=57;74−31=43;43−22=21. Each requires NO tens exchange."
            :"Recheck independent 34+23,74−31 and the gap between43 and22. None requires regrouping.");
    }
}

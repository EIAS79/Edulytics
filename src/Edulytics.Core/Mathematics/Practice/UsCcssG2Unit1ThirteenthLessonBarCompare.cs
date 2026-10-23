using System.Globalization;
using System.Text.RegularExpressions;

namespace Edulytics.Core.Mathematics.Practice;

/// <summary>
/// First-party 2021-aligned G2 U01 L13 bar graph compare tasks and
/// independent Edulytics exit ticket. Original numerical source goals
/// remain 17 vs 8 pets and 20/14/8/6 dogs; figures redrawn by Edulytics.
/// Learner-written two strategies and comparison language require teacher.
/// </summary>
public static class UsCcssG2Unit1ThirteenthLessonBarCompare
{
    public const string LessonCode="PED:US-CCSS-MATH:G2:U01:L13";
    public const string Version="edulytics-g2-u01-l13-bar-compare-first-party-v1";
    public static readonly IReadOnlyDictionary<string,int> SourcePets=
        new Dictionary<string,int>(StringComparer.Ordinal) {
            ["Cats"]=17,["Rabbits"]=8
        };
    public static readonly IReadOnlyDictionary<string,int> SourceDogs=
        new Dictionary<string,int>(StringComparer.Ordinal) {
            ["Poodles"]=6,["Bulldogs"]=20,["Pugs"]=8,["Huskies"]=14
        };
    public static readonly IReadOnlyDictionary<string,int> IndependentCoolDown=
        new Dictionary<string,int>(StringComparer.Ordinal) {
            ["Climbing"]=13,["Sliding"]=7,["Cycling"]=9
        };

    public sealed record Review(bool MachineScorable,bool Correct,
        bool TeacherReviewNeeded,string Message);

    private static readonly Regex Addition=new(
        @"^\s*([0-9]{1,2})\s*\+\s*([0-9]{1,2})\s*=\s*([0-9]{1,2})\s*$",
        RegexOptions.CultureInvariant | RegexOptions.Compiled);
    private static readonly Regex Subtraction=new(
        @"^\s*([0-9]{1,2})\s*[-−]\s*([0-9]{1,2})\s*=\s*([0-9]{1,2})\s*$",
        RegexOptions.CultureInvariant | RegexOptions.Compiled);

    public static Review CheckTrueFalseWarmUp(
        string? first,string? second,string? third)
    {
        var correct=first?.Trim()=="1"&&second?.Trim()=="1"&&third?.Trim()=="0";
        return new(true,correct,true,correct
            ?"9+4=9+1+3 and 9+4=10+3 are true; 9+5=10+6 is false."
            :"Mark true with 1 and false with 0, explaining the make-ten structure.");
    }

    public static Review CheckSourcePetsDifference(
        string? gap,string? countOn,string? countBack)
    {
        var good=gap?.Trim()=="9" &&
            ContainsExactDifference(countOn,8,9,17,true) &&
            ContainsExactDifference(countBack,17,8,9,false);
        return new(true,good,true,good
            ?"17 cats and 8 rabbits differ by 9. Count on: 8+9=17; count back: 17−8=9."
            :"Count forward from the short bar (8) or backward from the tall bar (17), then write BOTH related equations.");
    }

    public static Review CheckDogComparison(
        string? more,string? fewer,string? addition,string? subtraction)
    {
        if(!SourceDogs.TryGetValue(more??"",out var a) ||
           !SourceDogs.TryGetValue(fewer??"",out var b) ||
           a<=b)
            return new(true,false,true,
                "Choose a dog category with MORE observations, then a different one with FEWER.");
        var gap=a-b;
        var correct=ContainsExactDifference(addition,b,gap,a,true) &&
            ContainsExactDifference(subtraction,a,b,gap,false);
        return new(true,correct,true,correct
            ?$"There are {gap} more {more} than {fewer}. The addition and subtraction equations agree."
            :"The larger bar equals smaller bar PLUS difference. Subtract the smaller bar from the larger one to find the SAME difference.");
    }

    public static Review CheckEdulyticsCoolDown(
        string? difference,string? addition,string? subtraction)
    {
        var correct=difference?.Trim()=="6" &&
            ContainsExactDifference(addition,7,6,13,true) &&
            ContainsExactDifference(subtraction,13,7,6,false);
        return new(true,correct,false,correct
            ?"13 climbing and 7 sliding make a difference of 6: 7+6=13, 13−7=6."
            :"Read the independently created climbing/sliding bars and show an addition and subtraction equation for their difference.");
    }

    private static bool ContainsExactDifference(string? input,
        int first,int second,int result,bool add)
    {
        var match=(add?Addition:Subtraction).Match(input??"");
        if(!match.Success)return false;
        var actual=Enumerable.Range(1,3).Select(i=>int.Parse(
            match.Groups[i].Value,CultureInfo.InvariantCulture)).ToArray();
        return actual[0]==first&&actual[1]==second&&actual[2]==result;
    }

    public static readonly IReadOnlyList<string> TeacherRubric=
    [
        "Learner identifies which bar is longer and writes truthful MORE and FEWER statements including the named categories and unknown difference.",
        "Learner explains TWO source-appropriate methods: counting ON from the shorter bar to the longer bar and counting BACK from the longer bar.",
        "Learner connects both actual quantities and the difference to related equations such as 8+9=17 and 17−8=9, not a total-of-categories sum.",
        "Learner writes and checks original statements for at least two different dog category pairs, with evidence on the redrawn unit-one horizontal graph.",
        "Teacher observes partner explanation, visual counting marks and why each numeral represents a category count or a difference; numerical input alone does not certify that work."
    ];
}

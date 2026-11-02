using System.Globalization;
using System.Text.RegularExpressions;

namespace Edulytics.Core.Mathematics.Practice;

/// <summary>
/// Grade2 U01 L14 first-party tape diagram activities derived from public
/// compare math facts. The dashed missing section is the DIFFERENCE.
/// Real physical cutting, alignment, gluing and student statements need
/// separate teacher review; publisher assets/Cool-down not reproduced.
/// </summary>
public static class UsCcssG2Unit1FourteenthLessonTapeDiagrams
{
    public const string LessonCode="PED:US-CCSS-MATH:G2:U01:L14";
    public const string Version="edulytics-original-g2-u01-l14-tape-diagrams-v1";

    public static readonly IReadOnlyDictionary<string,int> JuiceVotes =
        new Dictionary<string,int>(StringComparer.Ordinal){
            ["Lemonade"]=12,["Fruit punch"]=6,["Apple"]=15,["Grape"]=8};
    public static readonly IReadOnlyDictionary<string,int> PartyVotes =
        new Dictionary<string,int>(StringComparer.Ordinal){
            ["Pizza"]=19,["Burgers"]=8,["Hot dogs"]=14};
    public static readonly IReadOnlyDictionary<string,int> NewLibraryVotes =
        new Dictionary<string,int>(StringComparer.Ordinal){
            ["Adventure"]=16,["Fantasy"]=9};

    private static readonly Regex Add=new(
        @"^\s*([0-9]{1,2})\s*\+\s*([0-9]{1,2})\s*=\s*([0-9]{1,2})\s*$",
        RegexOptions.Compiled|RegexOptions.CultureInvariant);
    private static readonly Regex Subtract=new(
        @"^\s*([0-9]{1,2})\s*[-−]\s*([0-9]{1,2})\s*=\s*([0-9]{1,2})\s*$",
        RegexOptions.Compiled|RegexOptions.CultureInvariant);

    public sealed record Review(bool MachineScorable,bool Correct,
        bool TeacherReviewNeeded,string Reason);

    public static Review VerifyTapes(
        IReadOnlyDictionary<string,int> source,
        string? larger,string? smaller,string? missing,
        string? addition,string? subtraction)
    {
        if(!source.TryGetValue(larger??"",out var big)||
            !source.TryGetValue(smaller??"",out var small)||
            big<=small)
            return new(true,false,true,
                "Choose two DIFFERENT named categories and put the taller bar on top.");
        var gap=big-small;
        var correct=missing?.Trim()==gap.ToString(CultureInfo.InvariantCulture) &&
            ValidEquation(Add,addition,small,gap,big) &&
            ValidEquation(Subtract,subtraction,big,small,gap);
        return new(true,correct,true,correct
            ?$"{larger}={big} is the longer strip; {smaller}={small} plus the outlined missing {gap} fills it. The related equations agree."
            :"The dashed region is larger minus smaller, NOT the whole. Use smaller+gap=larger AND larger−smaller=gap.");
    }

    private static bool ValidEquation(Regex pattern,string? input,int a,int b,int c)
    {
        var m=pattern.Match(input??"");
        return m.Success &&
            int.Parse(m.Groups[1].Value,CultureInfo.InvariantCulture)==a &&
            int.Parse(m.Groups[2].Value,CultureInfo.InvariantCulture)==b &&
            int.Parse(m.Groups[3].Value,CultureInfo.InvariantCulture)==c;
    }

    public static Review CheckIndependentCoolDown(
        string? gap,string? addition,string? subtraction)
    {
        var result=VerifyTapes(NewLibraryVotes,"Adventure","Fantasy",gap,addition,subtraction);
        return result with {TeacherReviewNeeded=false};
    }

    public static readonly IReadOnlyList<string> TeacherRubric=
    [
        "Student begins with a bar graph or real set of bars and makes a new original tape diagram with TWO aligned labeled rectangles representing the source quantities.",
        "Student places the larger strip against the smaller and draws a dashed or open segment equal to the unknown DIFFERENCE, not an unrelated third category.",
        "Student writes TWO truthful more/fewer statements for hot dogs 14 versus burgers 8 and explains the related equations 8+6=14 and 14−8=6.",
        "Student completes at least two source pairs: pizza19 against burgers8 (gap11), pizza19 against hot dogs14 (gap5); explains where numbers and '?' appear on the tapes.",
        "Teacher checks actual construction or cutting, correct labels, mathematical story, partner discussion, and explanation; exact arithmetic alone cannot certify those tasks."
    ];
}

using System.Globalization;
using System.Text.RegularExpressions;

namespace Edulytics.Core.Mathematics.Practice;

/// <summary>
/// Independently-authored Edulytics Cool-down for Grade 2 U01 L02:
/// ten cubes, hidden part, related addition and subtraction, source-goal
/// letter/tower matching and teacher-observed strategy. Original publisher
/// educator-only assessment is not copied or purported to be the same text.
/// </summary>
public static class UsCcssG2Unit1SecondLessonEdulyticsCoolDown
{
    public const string LessonCode="PED:US-CCSS-MATH:G2:U01:L02";
    public const string Version="edulytics-original-g2-u01-l02-cooldown-v1";
    public const int EstimatedMinutes=5;

    public sealed record Review(bool MachineScorable,bool Correct,bool TeacherReviewNeeded,
        string Message);

    public static Review VerifyHiddenTen(string? raw)
    {
        if(!int.TryParse(raw,NumberStyles.None,CultureInfo.InvariantCulture,out var n))
            return new(true,false,false,"Enter the number of hidden cubes.");
        return new(true,n==7,false,n==7
            ?"Three visible cubes and seven hidden cubes make ten."
            :"Ten minus three equals the number hidden.");
    }

    public static Review VerifySubtractionBlank(string? raw)
    {
        if(!int.TryParse(raw,NumberStyles.None,CultureInfo.InvariantCulture,out var n))
            return new(true,false,false,"Enter the missing number.");
        return new(true,n==6,false,n==6
            ?"Ten take away six leaves four."
            :"Find the amount removed when four cubes remain from ten.");
    }

    // The precise two-part relationship is checked, not merely any expressions
    // that happen to equal 10 and 6. Equivalent order 4+6 is accepted.
    private static readonly Regex AdditionPattern = new(
        @"^\s*(?:6\s*\+\s*4|4\s*\+\s*6)\s*$",
        RegexOptions.CultureInvariant|RegexOptions.Compiled);
    private static readonly Regex SubtractionPattern = new(
        @"^\s*10\s*[-−]\s*4\s*$",
        RegexOptions.CultureInvariant|RegexOptions.Compiled);

    public static Review VerifyRelatedEquations(string? addition,string? subtraction)
    {
        var valid=AdditionPattern.IsMatch(addition??"") &&
            SubtractionPattern.IsMatch(subtraction??"");
        return new(true,valid,true,valid
            ?"These equations describe six and four as two parts of ten. Teacher may review the cube representation."
            :"Use the same six-blue, four-yellow tower in both related equations.");
    }

    /// <summary>
    /// First-party cube towers (blue/yellow counts), no publisher artwork.
    /// A=5+5, B=4+6, C=3+7, D=2+8, E=1+9.
    /// </summary>
    public static readonly IReadOnlyList<(string Label,int Blue,int Yellow)> Towers =
    [
        ("A",5,5),("B",4,6),("C",3,7),("D",2,8),("E",1,9)
    ];

    public static Review VerifyTowerForTenMinusSeven(string? letter)
    {
        var normalized=letter?.Trim().ToUpperInvariant()??"";
        if(normalized.Length!=1 || "ABCDE".IndexOf(normalized,StringComparison.Ordinal)<0)
            return new(true,false,false,"Choose A, B, C, D, or E.");
        return new(true,normalized=="C",false,normalized=="C"
            ?"Tower C has three blue and seven yellow: 10 − 7 = 3."
            :"Find the tower showing seven yellow cubes and three blue cubes.");
    }

    public static readonly IReadOnlyList<string> TeacherRubric =
    [
        "Learner names the whole ten and the visible/hidden parts in their diagram or real cubes.",
        "Learner relates a missing addend equation to at least one corresponding subtraction fact.",
        "Learner explains why the matching two-color tower represents the recorded equation.",
        "Teacher records partner strategy and Math Community discussion without converting it to a numerical-only grade."
    ];
}

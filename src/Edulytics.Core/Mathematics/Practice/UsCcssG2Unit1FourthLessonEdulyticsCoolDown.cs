using System.Globalization;
using System.Text.RegularExpressions;

namespace Edulytics.Core.Mathematics.Practice;

/// <summary>
/// Independent Edulytics Grade 2 U01 L04 Cool-down / How Close task.
/// Five distinct digit cards, choose exactly three to approach target 20,
/// plus strategies for add/subtract within 20. No publisher-restricted
/// student response or Cool-down is duplicated.
/// </summary>
public static class UsCcssG2Unit1FourthLessonEdulyticsCoolDown
{
    public const string LessonCode="PED:US-CCSS-MATH:G2:U01:L04";
    public const string Version="edulytics-g2-u01-l04-how-close-cooldown-v1";
    public static readonly IReadOnlyList<int> DigitCards=[5,8,2,4,6];
    private static readonly Regex TripleExpression=new(
        @"^\s*([0-9])\s*\+\s*([0-9])\s*\+\s*([0-9])\s*$",
        RegexOptions.CultureInvariant|RegexOptions.Compiled);
    public sealed record Review(bool MachineScorable,bool Correct,bool TeacherReviewNeeded,
        string Reason);

    public static Review VerifyOriginalThreeCardEquation(string? answer)
    {
        var m=TripleExpression.Match(answer??"");
        if(!m.Success)
            return new(true,false,true,"Write exactly three one-digit cards with two plus signs.");

        var chosen=Enumerable.Range(1,3).Select(i=>int.Parse(
            m.Groups[i].Value,CultureInfo.InvariantCulture)).ToArray();
        if(chosen.Distinct().Count()!=3 || chosen.Any(x=>!DigitCards.Contains(x)))
            return new(true,false,true,"Choose three different physical cards from the five shown.");

        var possible=DigitCards.SelectMany((x,i)=>DigitCards
            .SelectMany((y,j)=>DigitCards
                .Where((z,k)=>i<j && j<k)
                .Select(z=>Math.Abs(20-x-y-z)))).ToArray();
        // Above only includes triples where i<j<k, despite flattening
        // through explicit pair and third-card positions.
        var best=possible.Min();
        var gap=Math.Abs(20-chosen.Sum());
        return new(true,gap==best,true,
            gap==best?"Three chosen cards reach the closest possible sum to 20. Explain why your choices work."
                :$"Your three-card sum has a distance of {gap} from 20; look for a closer legal sum.");
    }

    public static Review VerifySubtractSeventeenNine(string? answer)
        => VerifyNumber(answer,8,"Seventeen minus nine equals eight; one strategy reaches ten first.");

    public static Review VerifySevenPlusSix(string? answer)
        => VerifyNumber(answer,13,"Seven and six make thirteen; decompose six to make ten.");

    private static Review VerifyNumber(string? raw,int expected,string success)
    {
        if(!int.TryParse(raw,NumberStyles.None,CultureInfo.InvariantCulture,out var n))
            return new(true,false,false,"Enter a non-negative whole number.");
        return new(true,n==expected,false,n==expected
            ?success:"Check your equation using counting, decomposing or making ten.");
    }

    public static readonly IReadOnlyList<string> TeacherRubric=
    [
        "Student uses exactly 3 of 5 digit cards (10 cards removed), records a correct mathematical equation and justifies proximity to twenty.",
        "Student can show a make-ten or another valid method to find sums and differences within twenty; all valid strategies are acceptable.",
        "Student-created drawings/numbers/words represent the arithmetic faithfully, even when the written explanation varies.",
        "A teacher observes physical partner turns, comparisons between choices, math language and Math Community norms. Correct auto-score alone does not certify them."
    ];
}

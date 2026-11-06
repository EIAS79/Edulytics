using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// IM 2021 G5 U03:L05. Exact numeric subskill from the source A image
/// (2/5 x 1/3) and Han's explicit 6/5 x 1/3 = 6/15.
/// Original source images, estimation and student reasoning teacher-reviewed.
/// Tests authored but NOT_RUN. Uses existing exact question dispatcher.
/// </summary>
public static class UsCcssG5U03L05ShadedAreaQuestions
{
    public const string Family = "usccss.g5.u03.l05.shaded_unit_rectangle_product";
    public sealed record Problem(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    private static readonly (int Numerator,int Denominator,int OtherDenominator)[] Examples =
        [(2,5,3),(6,5,3),(1,5,3),(4,5,3)];
    public static bool Supports(string? family) => family == Family;
    private static bool Valid(string? family,IReadOnlyDictionary<string,int>? p,
        out (int Numerator,int Denominator,int OtherDenominator) e)
    {
        e=default;
        if(!Supports(family)||p is null||p.Count!=2||
           !p.TryGetValue("case",out int c)||c is <0 or >=4||
           !p.TryGetValue("slot",out int s)||s is <0 or >2) return false;
        e=Examples[c];return true;
    }
    public static Problem Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Wrong G5 L05 family.");
        ArgumentNullException.ThrowIfNull(random);
        IReadOnlyDictionary<string,int> p=new Dictionary<string,int>(StringComparer.Ordinal)
        {["case"]=random.Next(Examples.Length),["slot"]=random.Next(3)};
        if(!Valid(family,p,out var e))throw new InvalidOperationException("Bad shaded model.");
        int n=e.Numerator,den=e.Denominator*e.OtherDenominator;
        string correct=$"{n}/{den}";
        string[] opts=p["slot"] switch
        {
            0=>[correct,$"{n}/{e.Denominator}",$"{n}/{e.OtherDenominator}"],
            1=>[$"{n}/{e.Denominator}",correct,$"{n}/{e.OtherDenominator}"],
            _=>[$"{n}/{e.Denominator}",$"{n}/{e.OtherDenominator}",correct]
        };
        string question=p["case"]==1
           ? "Han's source shaded region is represented by (6/5) x (1/3). Six 1/15 unit-square cells are shaded across more than one fifth-width unit. "
           : p["case"]==3
           ? "Edulytiks-original supplementary practice using the same original-whole partition model. " +
             $"One unit square has {e.Denominator} equal columns and {e.OtherDenominator} equal rows. The shaded amount is ({n}/{e.Denominator}) x (1/{e.OtherDenominator}). "
           : $"The ORIGINAL whole unit square is divided into {e.Denominator} equal columns and {e.OtherDenominator} equal rows. The shaded amount is ({n}/{e.Denominator}) x (1/{e.OtherDenominator}). ";
        string prompt=question+$"What is the area in square units? Select 1: {opts[0]}; 2: {opts[1]}; 3: {opts[2]}. " +
            "Publisher diagram matching, estimates and freehand explanations are TEACHER_REVIEW_REQUIRED.";
        string solution=$"Each original unit square is partitioned into {den} equal-area pieces. " +
            $"The product is {correct} square unit (equivalent reduced fractions accepted in human explanation). " +
            "Do not divide by thirty simply because multiple unit squares are visible.";
        return new Problem(family,prompt,solution,AssessmentItemType.Numeric,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p,out _))throw new InvalidOperationException("Malformed G5 L05 parameters.");
        return (p["slot"]+1).ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int>? p,string? answer) =>
        Valid(family,p,out _) &&
        string.Equals(answer,(p!["slot"]+1).ToString(CultureInfo.InvariantCulture),StringComparison.Ordinal);
}

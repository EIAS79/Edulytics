using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// IM 2021 G5 U03:L06 exact sources 2/6*4/5, 3/6*4/5,
/// and 6*4/(5*4). Teacher reviews source A-D images and
/// student explanations. AUTHORING ONLY, tests AUTHORED_NOT_RUN.
/// </summary>
public static class UsCcssG5U03L06NonunitAreaQuestions
{
    public const string Family="usccss.g5.u03.l06.nonunit_fraction_area_source_products";
    public sealed record Problem(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    private static readonly (int A,int B,int C,int D)[] Examples=
        [(2,6,4,5),(3,6,4,5),(6,5,4,4),(4,6,4,5)];
    public static bool Supports(string? family) => family==Family;
    private static bool Valid(string? family,IReadOnlyDictionary<string,int>? p,
        out (int A,int B,int C,int D) e)
    {
        e=default;
        if(!Supports(family)||p is null||p.Count!=2||
           !p.TryGetValue("case",out int c)||c is <0 or >=4||
           !p.TryGetValue("slot",out int slot)||slot is <0 or >2) return false;
        e=Examples[c];return true;
    }
    public static Problem Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Not Grade 5 U03:L06 family.");
        ArgumentNullException.ThrowIfNull(random);
        IReadOnlyDictionary<string,int> p=new Dictionary<string,int>(StringComparer.Ordinal)
        {["case"]=random.Next(Examples.Length),["slot"]=random.Next(3)};
        if(!Valid(family,p,out var e))throw new InvalidOperationException("Invalid source bounds.");
        string correct=$"{e.A*e.C}/{e.B*e.D}";
        string wrongA=$"{e.A+e.C}/{e.B+e.D}";
        string wrongB=$"{e.A*e.C}/{e.B+e.D}";
        string[] opts=p["slot"] switch
        {
            0=>[correct,wrongA,wrongB],
            1=>[wrongA,correct,wrongB],
            _=>[wrongA,wrongB,correct]
        };
        string question=p["case"]==2
          ? "The last original source table asks how (6 x 4)/(5 x 4) represents a shaded area, even though more than one unit can be visible. "
          : p["case"]==0
          ? "The source Activity 1 area corresponds to 2/6 x 4/5 = 2 x 4 x (1/6 x 1/5). "
          : p["case"]==3
          ? "Edulytiks-original supplementary variation: 4/6 x 4/5 of the original unit square. "
          : "The preparation model uses 3/6 x 4/5 of a unit square. ";
        string prompt=question+ $"The ORIGINAL one-square-unit area is the reference. Select 1: {opts[0]}; 2: {opts[1]}; 3: {opts[2]}. " +
            "Source diagrams, the WODB reasoning and learner calculations remain TEACHER_REVIEW_REQUIRED.";
        string solution=$"{e.A}/{e.B} x {e.C}/{e.D} = ({e.A} x {e.C})/({e.B} x {e.D}) = {correct} square unit. " +
            "The numerator counts shaded equal cells and the denominator counts cells in one original unit square.";
        return new Problem(family,prompt,solution,AssessmentItemType.Numeric,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p,out _))throw new InvalidOperationException("Invalid source item.");
        return (p["slot"]+1).ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int>? p,string? answer) =>
        Valid(family,p,out _) &&
        string.Equals(answer,(p!["slot"]+1).ToString(CultureInfo.InvariantCulture),
            StringComparison.Ordinal);
}

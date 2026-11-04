using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Source-exact Grade 5 U03:L08 flag tasks: measured lengths in inches and
/// areas in square inches are not interchangeable. National-flag images and
/// qualitative question-writing remain TEACHER_REVIEW_REQUIRED.
/// Existing exact Practice engine reused; fixture AUTHORED_NOT_RUN.
/// </summary>
public static class UsCcssG5U03L08FlagMeasureQuestions
{
    public const string Family="usccss.g5.u03.l08.original_flags_fraction_measure";
    public sealed record Problem(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    private static readonly (int A,int B,int C,int D,string Context,string Unit)[] Cases=
    [
        (1,3,3,5,"Warm-up: one third of three fifths","fraction"),
        (2,3,3,5,"Warm-up: two thirds of three fifths","fraction"),
        (5,3,3,5,"Warm-up: five thirds of three fifths","fraction"),
        (2,3,13,5,"Warm-up: two thirds of thirteen fifths","fraction"),
        (5,1,15,2,"Thailand replica: full 5-inch width by 7 1/2-inch length","square inches"),
        (5,6,15,2,"Thailand: one red stripe, width 5/6 in by length 7 1/2 in","square inches"),
        (10,6,15,2,"Thailand: blue stripe, width 10/6 in by length 7 1/2 in","square inches"),
        (1,2,7,2,"Colombia: yellow stripe width half of 3 1/2 inches","inches"),
        (1,4,7,2,"Colombia: blue/red stripe width quarter of 3 1/2 inches","inches"),
        (7,4,21,4,"Colombia: yellow stripe area using width 7/4 and length 21/4","square inches")
    ];
    public static bool Supports(string? family)=>family==Family;
    private static bool Valid(string? family,IReadOnlyDictionary<string,int>? p,
        out (int A,int B,int C,int D,string Context,string Unit) c)
    {
        c=default;
        if(!Supports(family)||p is null||p.Count!=2||
           !p.TryGetValue("case",out int index)||index is <0 or >9||
           !p.TryGetValue("slot",out int slot)||slot is <0 or >2)return false;
        c=Cases[index];return true;
    }
    private static string Fraction(int a,int b)
    {
        int x=Math.Abs(a),y=Math.Abs(b);
        while(y!=0){int t=x%y;x=y;y=t;}
        return $"{a/x}/{b/x}";
    }
    public static Problem Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Wrong G5 L08.");
        ArgumentNullException.ThrowIfNull(random);
        IReadOnlyDictionary<string,int> p=new Dictionary<string,int>(StringComparer.Ordinal)
        {["case"]=random.Next(10),["slot"]=random.Next(3)};
        if(!Valid(family,p,out var c))throw new InvalidOperationException("Wrong flag sample.");
        string correct=Fraction(c.A*c.C,c.B*c.D);
        string wrongA=Fraction(c.A,c.B),wrongB=Fraction(c.C,c.D);
        string[] opts=p["slot"] switch
        {
            0=>[correct,wrongA,wrongB],
            1=>[wrongA,correct,wrongB],
            _=>[wrongA,wrongB,correct]
        };
        string prompt=$"{c.Context}. In the source scenario, multiply ({c.A}/{c.B}) x ({c.C}/{c.D}). " +
            $"Which result has the correct {c.Unit} unit? Select 1: {opts[0]}; 2: {opts[1]}; 3: {opts[2]}. " +
            "Teacher separately checks the source flag drawing, units and question interpretation: TEACHER_REVIEW_REQUIRED.";
        string solution=$"The exact product is ({c.A} x {c.C})/({c.B} x {c.D}) = {correct} {c.Unit}. " +
            "Length in inches is not area in square inches; context and source image must be interpreted.";
        return new Problem(family,prompt,solution,AssessmentItemType.Numeric,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p,out _))throw new InvalidOperationException("Malformed original flag parameters.");
        return (p["slot"]+1).ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int>? p,string? answer)=>
        Valid(family,p,out _) &&
        string.Equals(answer,(p!["slot"]+1).ToString(CultureInfo.InvariantCulture),
            StringComparison.Ordinal);
}

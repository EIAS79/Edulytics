using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>Source-aligned G5 U03:L07 products, missing factors and first-party
/// numeric choices. Original source drawings/equivalence arguments require
/// TEACHER_REVIEW_REQUIRED. AUTHORED_NOT_RUN, existing exact engine reused.
/// </summary>
public static class UsCcssG5U03L07FractionEquationQuestions
{
    public const string ProductFamily="usccss.g5.u03.l07.source_fraction_products";
    public const string MissingFamily="usccss.g5.u03.l07.missing_fraction_factors";
    public sealed record Problem(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    private static readonly (int A,int B,int C,int D)[] Products=
        [(2,5,3,4),(3,7,4,5),(9,11,5,8),(3,8,2,5),(3,4,9,5),(10,5,6,5)];
    private static readonly (string Left,string Answer,string WrongA,string WrongB)[] Missing=
        [("8/9 x ? = 56/36","7/4","7/9","8/4"),("5 x ? = 15/8","3/8","5/8","15/5")];
    public static bool Supports(string? family) => family is ProductFamily or MissingFamily;
    private static bool Valid(string? family,IReadOnlyDictionary<string,int>? p,out int c)
    {
        c=0;
        if(!Supports(family)||p is null||p.Count!=2||
           !p.TryGetValue("case",out c)||c<0||
           c>=(family==ProductFamily?Products.Length:Missing.Length)||
           !p.TryGetValue("slot",out int slot)||slot is <0 or >2)return false;
        return true;
    }
    public static Problem Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Wrong G5 L07 family.");
        ArgumentNullException.ThrowIfNull(random);
        IReadOnlyDictionary<string,int> p=new Dictionary<string,int>(StringComparer.Ordinal)
        {["case"]=random.Next(family==ProductFamily?Products.Length:Missing.Length),
         ["slot"]=random.Next(3)};
        if(!Valid(family,p,out int i))throw new InvalidOperationException("Bad source case.");
        string correct,wrongA,wrongB,prompt,solution;
        if(family==ProductFamily)
        {
            var ex=Products[i];
            correct=$"{ex.A*ex.C}/{ex.B*ex.D}";
            wrongA=$"{ex.A+ex.C}/{ex.B+ex.D}";
            wrongB=$"{ex.A*ex.C}/{ex.B+ex.D}";
            prompt=$"IM 2021 Grade 5 source fraction product: ({ex.A}/{ex.B}) x ({ex.C}/{ex.D}). " +
                "What fraction of the ORIGINAL whole is the product? ";
            solution=$"Multiply numerator counts {ex.A} x {ex.C} and denominator partitions {ex.B} x {ex.D}; area = {correct}.";
        }
        else
        {
            var ex=Missing[i];
            correct=ex.Answer;wrongA=ex.WrongA;wrongB=ex.WrongB;
            prompt=$"Original source missing-factor equation: {ex.Left}. Choose the correct factor. ";
            solution=$"The exact source equation is satisfied by {correct}, not by adding or swapping numerator and denominator.";
        }
        string[] options=p["slot"] switch
        {
            0=>[correct,wrongA,wrongB],
            1=>[wrongA,correct,wrongB],
            _=>[wrongA,wrongB,correct]
        };
        prompt+=$"Select 1: {options[0]}; 2: {options[1]}; 3: {options[2]}. " +
            "Original image comparisons and student strategy justifications are TEACHER_REVIEW_REQUIRED.";
        return new Problem(family,prompt,solution,AssessmentItemType.Numeric,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p,out _))throw new InvalidOperationException("Malformed G5 L07 parameters.");
        return (p["slot"]+1).ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int>? p,string? answer) =>
        Valid(family,p,out _) &&
        string.Equals(answer,(p!["slot"]+1).ToString(CultureInfo.InvariantCulture),
            StringComparison.Ordinal);
}

using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Source-skill-support ONLY for optional 2021 G5 U03:L09. Original learner
/// selected flag dimensions, symbolism, drawings and estimates have no fixed
/// automatic answer; always TEACHER_REVIEW_REQUIRED. Existing exact engine reuse.
/// </summary>
public static class UsCcssG5U03L09FlagDesignSupportQuestions
{
    public const string Family="usccss.g5.u03.l09.first_party_flag_band_fabric_area_support";
    public sealed record Problem(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    private static readonly (int A,int B,int C,int D,string Label)[] Samples=
        [(8,1,5,4,"whole first-party banner: 8 x 5 1/2 inches"),
         (8,1,5,4,"first-party top band: 8 x 1 1/4 inches"),
         (8,1,2,1,"first-party center band: 8 x 2 inches"),
         (8,1,9,4,"first-party bottom band: 8 x 2 1/4 inches")];
    public static bool Supports(string? family)=>family==Family;
    private static bool Valid(string? family,IReadOnlyDictionary<string,int>? p,out int c)
    {
        c=0;
        if(!Supports(family)||p is null||p.Count!=2||
           !p.TryGetValue("case",out c)||c is <0 or >3||
           !p.TryGetValue("slot",out int s)||s is <0 or >2)return false;
        return true;
    }
    public static Problem Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Wrong G5 flag design support family.");
        ArgumentNullException.ThrowIfNull(random);
        IReadOnlyDictionary<string,int> p=new Dictionary<string,int>(StringComparer.Ordinal)
        {["case"]=random.Next(4),["slot"]=random.Next(3)};
        if(!Valid(family,p,out int c))throw new InvalidOperationException("Bad flag-support case.");
        var e=Samples[c];
        int n=c==0?44:c==1?10:c==2?16:18;
        string correct=n.ToString(CultureInfo.InvariantCulture);
        string wrongA=(n+8).ToString(CultureInfo.InvariantCulture);
        string wrongB=(n-4).ToString(CultureInfo.InvariantCulture);
        string[] opts=p["slot"] switch
        {
            0=>[correct,wrongA,wrongB],
            1=>[wrongA,correct,wrongB],
            _=>[wrongA,wrongB,correct]
        };
        string prompt=$"OPTIONAL FIRST-PARTY SUPPORT MODEL, NOT THE PUPIL'S SOURCE FLAG DESIGN: {e.Label}. " +
            $"Find the rectangular fabric area in square inches. Select 1: {opts[0]}; 2: {opts[1]}; 3: {opts[2]}. " +
            "Student-drawn flags, symbols, band boundaries, fabric estimates and peer design discussion are TEACHER_REVIEW_REQUIRED.";
        string explanation=$"The optional model's correct area is {correct} square inches. " +
            "This fixed model does not score any student's original 2021 flag-design project.";
        return new Problem(family,prompt,explanation,AssessmentItemType.Numeric,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p,out _))throw new InvalidOperationException("Invalid optional flag model.");
        return (p["slot"]+1).ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int>? p,string? answer)=>
        Valid(family,p,out _) &&
        string.Equals(answer,(p!["slot"]+1).ToString(CultureInfo.InvariantCulture),
            StringComparison.Ordinal);
}

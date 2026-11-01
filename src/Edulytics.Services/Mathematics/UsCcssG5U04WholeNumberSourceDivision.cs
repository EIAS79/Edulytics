using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Source-bounded Grade5 Unit4 exact WHOLE NUMBER DIVISION selected cases,
/// delegated to existing ExactSkillContractQuestionEngine. This is a lesson
/// adapter, not an additional engine. Original diagrams and numerical reasoning
/// require TEACHER_REVIEW_REQUIRED; authored fixtures NOT_RUN.
/// </summary>
public static class UsCcssG5U04WholeNumberSourceDivision
{
    public const string L10Dance="usccss.g5.u04.l10.original_folk_dance_equal_groups";
    public sealed record Problem(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    private sealed record SourceCase(string Family,string Context,string Expression,
        int Result,string Units);
    private static readonly SourceCase[] Cases =
    [
        new(L10Dance,"Original 2021 world-record Peru folk dance: 4704 people in groups of eight",
            "4704 ÷ 8",588,"groups of eight dancers"),
        new(L10Dance,"Original 2021 source same 4704 people in groups of four",
            "4704 ÷ 4",1176,"groups of four dancers"),
        new(L10Dance,"Original 2021 source same 4704 people in groups of two",
            "4704 ÷ 2",2352,"groups of two dancers"),
        // END_G5_U04_DIVISION_SOURCE_CASES
    ];
    public static bool Supports(string? family)=>
        !string.IsNullOrWhiteSpace(family) &&
        Cases.Any(c=>string.Equals(c.Family,family,StringComparison.Ordinal));
    private static bool Valid(string? family,IReadOnlyDictionary<string,int>? p,
        out SourceCase? item)
    {
        item=null;
        if(!Supports(family)||p is null||p.Count!=2||
           !p.TryGetValue("case",out int index)||index<0||
           !p.TryGetValue("slot",out int slot)||slot is <0 or >2)return false;
        var options=Cases.Where(c=>c.Family==family).ToArray();
        if(index>=options.Length)return false;
        item=options[index];return true;
    }
    public static Problem Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported source whole number division.");
        ArgumentNullException.ThrowIfNull(random);
        var selected=Cases.Where(c=>c.Family==family).ToArray();
        int idx=random.Next(selected.Length);
        IReadOnlyDictionary<string,int> p=new Dictionary<string,int>(StringComparer.Ordinal)
            {["case"]=idx,["slot"]=random.Next(3)};
        if(!Valid(family,p,out var c)||c is null)throw new InvalidOperationException("Malformed selected division case.");
        int correct=c.Result;
        int wrongA=correct+Math.Max(1,correct/4);
        int wrongB=Math.Max(1,correct-Math.Max(1,correct/4));
        int[] answers=p["slot"] switch
        {
            0=>[correct,wrongA,wrongB],
            1=>[wrongA,correct,wrongB],
            _=>[wrongA,wrongB,correct]
        };
        string prompt=$"{c.Context}. Find the source quotient {c.Expression}. "+
            $"Select 1: {answers[0]}; 2: {answers[1]}; 3: {answers[2]}. "+
            "Pupil's original grouping diagram, interpretation and partner explanations are TEACHER_REVIEW_REQUIRED.";
        string solution=$"{c.Expression}={correct} {c.Units}. "+
            "Use multiplication to check the source's number of groups and preserve the original 4704-person total.";
        return new Problem(family,prompt,solution,AssessmentItemType.Numeric,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p,out _))throw new InvalidOperationException("Malformed source division parameters.");
        return (p["slot"]+1).ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int>? p,string? answer)=>
        Valid(family,p,out _) &&
        string.Equals(answer,(p!["slot"]+1).ToString(CultureInfo.InvariantCulture),StringComparison.Ordinal);
}

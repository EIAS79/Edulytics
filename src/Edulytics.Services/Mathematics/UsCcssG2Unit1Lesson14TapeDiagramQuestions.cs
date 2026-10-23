using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Independently generated comparisons of labelled rectangular strips,
/// with a dashed extra segment. All quantities limited to 20; no publisher art.
/// Learner drawing, placement, labels and actual partner work are separate.
/// </summary>
internal static class UsCcssG2Unit1Lesson14TapeDiagramQuestions
{
    public const string Gap="usccss.g2.u01.l14.tape_unknown_gap";
    public const string Add="usccss.g2.u01.l14.tape_addition_missing_part";
    public const string Sub="usccss.g2.u01.l14.tape_subtraction_missing_part";
    public sealed record Problem(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? family)=>family is Gap or Add or Sub;

    public static Problem Build(string family,Random rng)
    {
        if(!Supports(family))throw new InvalidOperationException("Not a G2 tape family");
        var small=rng.Next(1,20);
        var big=rng.Next(small+1,21);
        var p=new Dictionary<string,int>(StringComparer.Ordinal) {
            ["small"]=small,["big"]=big
        };
        var beginning=$"Edulytics first-party tape diagram (NOT a publisher image): " +
            $"Top strip called Puzzle Games is length {big}; " +
            $"bottom strip called Board Games is length {small} plus a DASHED question-mark section, " +
            "both strips begin together and finish at the same aligned right edge. ";
        var prompt=family switch{
            Gap=>beginning+"How long is the outlined MISSING section? This is the DIFFERENCE, not the total.",
            Add=>beginning+$"Complete the related addition: {small} + ? = {big}. What does ? represent?",
            Sub=>beginning+$"Complete the related subtraction: {big} − {small} = ?. What does ? represent?",
            _=>throw new InvalidOperationException("Invalid tape question")
        };
        var diff=big-small;
        return new Problem(family,prompt,$"Difference {diff}; {small}+{diff}={big} and {big}−{small}={diff}. A teacher separately checks the original tape drawing.",
            AssessmentItemType.Numeric,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid G2 L14 tape parameters");
        return (p["big"]-p["small"]).ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string answer)=>
        Valid(family,p) && string.Equals(answer,Solve(family,p),StringComparison.Ordinal);
    private static bool Valid(string? family,IReadOnlyDictionary<string,int> p)=>
        Supports(family) && p.Count==2 &&
        p.TryGetValue("small",out var small) && small is >=1 and <=19 &&
        p.TryGetValue("big",out var big) && big is >=2 and <=20 && big>small;
}

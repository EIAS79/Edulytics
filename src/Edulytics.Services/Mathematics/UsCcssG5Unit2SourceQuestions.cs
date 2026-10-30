using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;

/// <summary>
/// Reuses the exact generation/independent verification engine for Grade 5
/// source-targeted MULTIPLE-CHOICE numeric subskills only. A correct answer does
/// not grade source-required drawings, physical builds or open explanations.
/// One lesson bank is added only after the previous lesson closes.
/// </summary>
internal static class UsCcssG5Unit2SourceQuestions
{
    private sealed record SourceCase(string Prompt,string Correct,string WrongA,string WrongB);
    private static readonly IReadOnlyDictionary<string,SourceCase[]> Banks=
        new Dictionary<string,SourceCase[]>(StringComparer.Ordinal)
        {
            ["usccss.g5.u02.l01.source_fair_share_choice"]=[
                new("Two sandwiches shared fairly by five people. Choose the amount EACH person receives; also draw two equal wholes separated into five fair shares.","2/5 sandwich","5/2 sandwiches","1/5 sandwich"),
                new("Five equal sandwiches shared by two people. What share does EACH person receive? Explain why the share is more than one whole.","2 1/2 sandwiches","2/5 sandwich","1/2 sandwich"),
                new("Three sandwiches shared by two people. Choose the share received by one person; show the same total number of whole sandwiches.","1 1/2 sandwiches","2/3 sandwich","1/3 sandwich"),
                new("Two sandwiches shared by three people. Choose the share each receives.","2/3 sandwich","3/2 sandwiches","1/3 sandwich"),
                new("Three sandwiches shared by five people. Choose the share each receives.","3/5 sandwich","5/3 sandwiches","1/5 sandwich"),
                new("Five sandwiches shared by three people. Choose the amount per person and describe equal portions.","1 2/3 sandwiches","3/5 sandwich","2/3 sandwich")
            ],
                        ["usccss.g5.u02.l02.unit_share_equivalence"]=[
                new("One sandwich shared equally among 2 people. How much for each person?","1/2 sandwich","2 sandwiches","1/3 sandwich"),
                new("One sandwich shared equally among 3 people. How much for each person?","1/3 sandwich","3 sandwiches","2/3 sandwich"),
                new("One sandwich shared equally among 4 people. How much for each person?","1/4 sandwich","4 sandwiches","1/2 sandwich"),
                new("One sandwich shared equally among 5 people. How much for each person?","1/5 sandwich","5 sandwiches","1/4 sandwich"),
                new("Which equality models exactly one sandwich shared by three people?","1÷3=1/3","3÷1=1/3","1÷3=3"),
                new("Which claim about equal shares of one sandwich is correct?","Each share becomes smaller as more people share one whole","The share becomes bigger as more people share","All equal divisions always give a whole sandwich")
            ],
            // NEXT_LESSON_BANK
        };
    public sealed record Problem(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? family)=>family is not null&&Banks.ContainsKey(family);
    public static Problem Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported G5 U02 exact source family");
        ArgumentNullException.ThrowIfNull(random);
        var cases=Banks[family];
        var index=random.Next(cases.Length);
        var slot=random.Next(3);
        var c=cases[index];
        var opts=slot switch{
            0=>new[]{c.Correct,c.WrongA,c.WrongB},
            1=>new[]{c.WrongA,c.Correct,c.WrongB},
            _=>new[]{c.WrongA,c.WrongB,c.Correct}
        };
        var prompt=c.Prompt+
            $" Choose 1: {opts[0]}; 2: {opts[1]}; 3: {opts[2]}. "+
            "Justify with a diagram or explanation for teacher review. "+
            "The numeric selection alone does not complete the open task.";
        return new Problem(family,prompt,"The equal-sharing or product model must match the same whole. "+
            "Teacher separately reviews diagrams, source activities and explanations.",
            AssessmentItemType.Numeric,
            new Dictionary<string,int>(StringComparer.Ordinal) {["case"]=index,["slot"]=slot});
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid G5 U02 source item");
        return (p["slot"]+1).ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)=>
        Valid(family,p)&&string.Equals(answer,Solve(family,p),StringComparison.Ordinal);
    private static bool Valid(string? family,IReadOnlyDictionary<string,int>? p)=>
        Supports(family)&&p is not null&&p.Count==2&&
        p.TryGetValue("case",out var index)&&index>=0&&index<Banks[family!].Length&&
        p.TryGetValue("slot",out var slot)&&slot is >=0 and <=2;
}

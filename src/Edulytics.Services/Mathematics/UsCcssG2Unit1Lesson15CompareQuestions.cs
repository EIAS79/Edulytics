using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Grade 2 L15 original compare stories as missing larger/smaller/difference
/// constrained to 100. Some cards use whole tens for place-value strategy;
/// all original student-authored tape diagrams remain teacher reviewed.
/// </summary>
internal static class UsCcssG2Unit1Lesson15CompareQuestions
{
    public const string Smaller="usccss.g2.u01.l15.compare_unknown_smaller";
    public const string Larger="usccss.g2.u01.l15.compare_unknown_larger";
    public const string Difference="usccss.g2.u01.l15.compare_unknown_difference";
    public sealed record Problem(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? family)=>family is Smaller or Larger or Difference;

    public static Problem Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported G2 L15 compare position");
        var small=random.Next(10,71);
        var tens=random.Next(1,4)*10;
        var diff=Math.Min(tens,100-small);
        var big=small+diff;
        if(big==small)throw new InvalidOperationException("Missing compare gap");
        var p=new Dictionary<string,int>(StringComparer.Ordinal){
            ["small"]=small,["diff"]=diff,["big"]=big
        };
        var prompt=family switch
        {
            Smaller=>$"Original Edulytics tape story: Diego has {big} shells; Lin has {diff} FEWER than Diego. The larger Diego strip is {big}, dashed gap {diff}, smaller Lin strip ?. How many shells has Lin?",
            Larger=>$"Original Edulytics tape story: Tala has {small} sea shells; Rami has {diff} MORE than Tala. The smaller Tala strip is {small}, dashed gap {diff}, larger Rami strip ?. How many has Rami?",
            Difference=>$"Original Edulytics tape story: Nora has {big} shells and Omar {small}. Both quantities known; dashed tape part ? is their difference. How many MORE does Nora have?",
            _=>throw new InvalidOperationException("Unsupported comparison position")
        };
        return new Problem(family,prompt,
            $"The aligned tapes satisfy smaller {small} + difference {diff} = larger {big}, and larger {big} − difference {diff} = smaller {small}. Identify the unknown position FIRST.",
            AssessmentItemType.Numeric,p);
    }

    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid Grade2 L15 source diagram parameters");
        var result=family switch{
            Smaller=>p["small"],
            Larger=>p["big"],
            Difference=>p["diff"],
            _=>throw new InvalidOperationException("Invalid family")
        };
        return result.ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string answer)=>
        Valid(family,p) && string.Equals(answer,Solve(family,p),StringComparison.Ordinal);
    private static bool Valid(string? family,IReadOnlyDictionary<string,int> p)=>
        Supports(family) && p.Count==3 &&
        p.TryGetValue("small",out var small) && small is >=1 and <=99 &&
        p.TryGetValue("diff",out var difference) && difference is >=1 and <=99 &&
        p.TryGetValue("big",out var big) && big is >=2 and <=100 &&
        small+difference==big && big>small;
}

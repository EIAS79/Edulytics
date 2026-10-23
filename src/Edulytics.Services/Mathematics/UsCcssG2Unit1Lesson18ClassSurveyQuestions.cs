using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>Independent Edulytics fictional graph values; authentic student
/// survey authorship and physical classroom participation are teacher-only.</summary>
internal static class UsCcssG2Unit1Lesson18ClassSurveyQuestions
{
    public const string Read="usccss.g2.u01.l18.read_category_count";
    public const string Combine="usccss.g2.u01.l18.combine_two_categories";
    public const string Compare="usccss.g2.u01.l18.compare_two_categories";
    public const string Tape="usccss.g2.u01.l18.tape_difference_from_graph";
    public sealed record Problem(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? family)=>family is Read or Combine or Compare or Tape;
    private static readonly string[] Labels=["Stories","Comics","Poetry","Nonfiction"];
    public static Problem Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Not an original G2 U01 L18 family");
        var n=random.Next(2)==0?3:4;
        var a=random.Next(3,11);var b=random.Next(2,10);
        var c=random.Next(2,10);var d=n==4?random.Next(1,10):0;
        var first=random.Next(n);var second=(first+1+random.Next(n-1))%n;
        var target=random.Next(n);
        var p=new Dictionary<string,int>(StringComparer.Ordinal){
            ["categoryCount"]=n,["a"]=a,["b"]=b,["c"]=c,["d"]=d,
            ["firstIndex"]=first,["secondIndex"]=second,["targetIndex"]=target
        };
        var v=new[]{a,b,c,d};
        var description=string.Join(", ",Enumerable.Range(0,n)
            .Select(i=>$"{Labels[i]} {v[i]}"));
        var prompt=$"Independent Edulytics FICTIONAL survey. " +
            $"A properly labeled pictograph (one circle per response) or numbered bar graph has {description}. ";
        var firstValue=v[first];var secondValue=v[second];
        prompt+=family switch{
            Read=>$"How many fictional responses chose {Labels[target]}?",
            Combine=>$"How many altogether chose {Labels[first]} and {Labels[second]}?",
            Compare=>$"What is the DIFFERENCE between {Labels[first]} and {Labels[second]}?",
            Tape=>$"A two-bar tape compares {Labels[first]} with {Labels[second]}. What is the missing extra strip between them?",
            _=>throw new InvalidOperationException("Unsupported survey family")
        };
        var solution=family switch{
            Read=>$"Read the single category {Labels[target]}: {v[target]} responses.",
            Combine=>$"{firstValue} + {secondValue} = {firstValue+secondValue}.",
            _=>$"{Math.Max(firstValue,secondValue)} − {Math.Min(firstValue,secondValue)} = {Math.Abs(firstValue-secondValue)}."
        };
        return new Problem(family,prompt,solution,AssessmentItemType.Numeric,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Bad survey graph parameters");
        var v=new[]{p["a"],p["b"],p["c"],p["d"]};
        var a=v[p["firstIndex"]];var b=v[p["secondIndex"]];
        var answer=family switch{
            Read=>v[p["targetIndex"]],Combine=>a+b,
            Compare or Tape=>Math.Abs(a-b),
            _=>throw new InvalidOperationException("Unsupported survey math")
        };
        return answer.ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string answer)=>
        Valid(family,p)&&string.Equals(answer,Solve(family,p),StringComparison.Ordinal);
    private static bool Valid(string? family,IReadOnlyDictionary<string,int> p)
    {
        if(!Supports(family)||p.Count!=8||
           !p.TryGetValue("categoryCount",out var n)||n is not (3 or 4)||
           !p.TryGetValue("a",out var a)||a is <0 or >12||
           !p.TryGetValue("b",out var b)||b is <0 or >12||
           !p.TryGetValue("c",out var c)||c is <0 or >12||
           !p.TryGetValue("d",out var d)||d is <0 or >12||
           (n==3&&d!=0)||
           a+b+c+d is <1 or >50||
           !p.TryGetValue("firstIndex",out var first)||first<0||first>=n||
           !p.TryGetValue("secondIndex",out var second)||second<0||second>=n||first==second||
           !p.TryGetValue("targetIndex",out var target)||target<0||target>=n)
            return false;
        return true;
    }
}

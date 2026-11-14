using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
/// <summary>Exact one-step UAE Arabic Grade 6 equations; teacher assesses open explanations.</summary>
internal static class UaeG6U7L01EquationPracticeQuestions
{
    internal const string Add="uae.g6.u07.l01.add",Subtract="uae.g6.u07.l01.subtract",
        OtherMinus="uae.g6.u07.l01.result_minus_unknown",Multiply="uae.g6.u07.l01.multiply",
        Divide="uae.g6.u07.l01.divide",Candidate="uae.g6.u07.l01.test_candidate";
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f)=>f is Add or Subtract or OtherMinus or Multiply or Divide or Candidate;
    private static string Norm(string? s)=>new string((s??"").Where(ch=>!char.IsWhiteSpace(ch))
      .Select(ch=>ch switch{
       >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
       >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
       '−' or '–'=>'-','٫'=>'.',_=>ch}).ToArray());
    private static bool Expected(string f,IReadOnlyDictionary<string,int>? p,out string expected)
    {
        expected="";
        if(!Supports(f)||p is null||!p.TryGetValue("x",out int x)||
           !p.TryGetValue("a",out int a)||x is <1 or >40||a is <1 or >20)return false;
        if(f==Candidate)
        {
            if(p.Count!=3||!p.TryGetValue("option",out int option)||
               option is <0 or >41)return false;
            expected=option==x?"نعم":"لا";return true;
        }
        if(p.Count!=2)return false;
        expected=x.ToString(CultureInfo.InvariantCulture);return true;
    }
    internal static Item Build(string f,Random r)
    {
        if(!Supports(f))throw new InvalidOperationException("Unsupported equation family");
        ArgumentNullException.ThrowIfNull(r);
        int x=r.Next(1,41),a=r.Next(1,21);
        Dictionary<string,int> p=new(){{"x",x},{"a",a}};
        string prompt;
        if(f==Candidate)
        {
            int option=r.Next(2)==0?x:x+1;p["option"]=option;
            prompt=$"هل س={option} حل للمعادلة س+{a}={x+a}؟ أجب بنعم أو لا.";
        }
        else prompt=f switch
        {
            Add=>$"أوجد س في المعادلة س+{a}={x+a}؛ تحقق بالتعويض.",
            Subtract=>$"أوجد س في المعادلة س−{a}={x-a}.",
            OtherMinus=>$"أوجد س في المعادلة {x+a}−س={a}.",
            Multiply=>$"أوجد س في المعادلة {a}س={a*x}.",
            _=>$"أوجد س في المعادلة س÷{a}={x}. لا تقسم على صفر."
        };
        return new Item(f,prompt,"أحدد العملية العكسية وأتحقق من تساوي طرفي المعادلة بالتعويض.",
            AssessmentItemType.ShortAnswer,p);
    }
    internal static string Solve(string f,IReadOnlyDictionary<string,int> p)=>
        Expected(f,p,out string answer)?answer:throw new InvalidOperationException("Invalid equation parameters");
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? response)=>
        !string.IsNullOrWhiteSpace(response)&&response.Length<=65&&
        Expected(f,p,out string expected)&&Norm(response)==expected;
}

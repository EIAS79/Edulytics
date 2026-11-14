using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
/// <summary>Exact bounded division-equation writing for UAE Arabic G6 U07L09.
 /// Freehand tape models and explanations are teacher reviewed.</summary>
internal static class UaeG6U7L09DivisionLabQuestions
{
    internal const string Write="uae.g6.u07.l09.write_quotient_equation";
    internal const string Inverse="uae.g6.u07.l09.inverse_multiplication";
    internal sealed record Item(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f)=>f is Write or Inverse;
    private static string Normalize(string? s)=>new string((s??"").Where(c=>!char.IsWhiteSpace(c))
      .Select(c=>c switch{
        >= '٠' and <= '٩'=>(char)('0'+c-'٠'),
        >= '۰' and <= '۹'=>(char)('0'+c-'۰'),
        '÷'=>'/','×' or '·'=>'*',_=>c}).ToArray());
    private static bool TryExpected(string family,IReadOnlyDictionary<string,int>? p,out string result)
    {
        result="";
        if(!Supports(family)||p is null||p.Count!=2||
           !p.TryGetValue("groups",out int groups)||
           !p.TryGetValue("share",out int share)||groups is <2 or >12||
           share is <1 or >100)return false;
        result=family==Write?$"س/{groups}={share}":$"س={groups*share}";
        return true;
    }
    internal static Item Build(string f,Random r)
    {
        if(!Supports(f))throw new InvalidOperationException("Unsupported division model");
        ArgumentNullException.ThrowIfNull(r);
        int n=r.Next(2,13),share=r.Next(1,101);
        Dictionary<string,int> p=new(){{"groups",n},{"share",share}};
        string q=f==Write?
            $"قسم {n} أصدقاء فاتورة مجهولة س بالتساوي، فدفع كل منهم {share} درهمًا. اكتب معادلة القسمة باستخدام س.":
            $"حل س÷{n}={share} بكتابة جملة ضرب عكسية توضح قيمة س. اكتبها س=عدد فقط.";
        return new Item(f,q,"أقسم الإجمالي على عدد الأفراد، ثم أضرب الحصة في عددهم لإعادة بناء الكل.",
            AssessmentItemType.ShortAnswer,p);
    }
    internal static string Solve(string f,IReadOnlyDictionary<string,int> p)=>
       TryExpected(f,p,out var expected)?expected:throw new InvalidOperationException("Invalid division parameters");
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? response)
    {
        if(string.IsNullOrWhiteSpace(response)||response.Length>90||
           !TryExpected(f,p,out string expected))return false;
        string norm=Normalize(response),wanted=Normalize(expected);
        if(norm==wanted)return true;
        int n=p["groups"],share=p["share"];
        if(f==Write)return norm==$"{share}=س/{n}"||norm==$"س÷{n}={share}";
        return norm==$"س={share}*{n}"||norm==$"س={n}*{share}";
    }
}

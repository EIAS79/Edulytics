using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
/// <summary>Source-scoped exact algebra tiles and numeric area distribution.
/// Retains the existing G4/G3 distributive numeric practice families.</summary>
internal static class UaeG6U6L09DistributionLabQuestions
{
    internal const string Expand="uae.g6.u06.l09.expand_variable";
    internal const string Evaluate="uae.g6.u06.l09.evaluate_distributive";
    internal const string Tiles="uae.g6.u06.l09.count_variable_tiles";
    internal sealed record Item(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f)=>f is Expand or Evaluate or Tiles;
    private static string Normalized(string? s)=>new string((s??"").Where(ch=>!char.IsWhiteSpace(ch))
        .Select(ch=>ch switch{
            >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
            >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
            '×' or '·'=>'*','−' or '–'=>'-',_=>ch}).ToArray());
    private static bool TryExpected(string f,IReadOnlyDictionary<string,int>? p,out string result)
    {
        result="";
        if(!Supports(f)||p is null)return false;
        if(f==Evaluate)
        {
            if(p.Count!=3||!p.TryGetValue("a",out int a)||!p.TryGetValue("b",out int b)||
              !p.TryGetValue("c",out int c)||a is <2 or >12||b is <1 or >30||
              c is <1 or >30)return false;
            result=(a*(b+c)).ToString(CultureInfo.InvariantCulture);
            return true;
        }
        if(p.Count!=2||!p.TryGetValue("a",out int multiplier)||
            !p.TryGetValue("b",out int added)||multiplier is <2 or >12||
            added is <1 or >30)return false;
        result=f==Expand?$"{multiplier}س+{multiplier*added}":
            multiplier.ToString(CultureInfo.InvariantCulture);
        return true;
    }
    internal static Item Build(string f,Random r)
    {
        if(!Supports(f))throw new InvalidOperationException("Unsupported distribution family");
        ArgumentNullException.ThrowIfNull(r);
        Dictionary<string,int> p;
        string question;
        if(f==Evaluate)
        {
            int a=r.Next(2,13),b=r.Next(1,31),c=r.Next(1,31);
            p=new(){{"a",a},{"b",b},{"c",c}};
            question=$"استخدم نموذج المساحة لإيجاد قيمة {a}({b}+{c})؛ وزع العامل على الحدين.";
        }
        else
        {
            int a=r.Next(2,13),b=r.Next(1,31);
            p=new(){{"a",a},{"b",b}};
            question=f==Expand?
              $"وسّع التعبير {a}(س+{b}) إلى مجموع حدين دون أقواس.":
              $"هناك {a} مجموعات من (س+{b}) في قطع جبرية. كم قطعة س في المجموع؟ اكتب العدد فقط.";
        }
        return new Item(f,question,
            "وزع عامل الضرب على كل جزء داخل القوس أو عدّ قطع المتغير والثوابت في كل مجموعة.",
            AssessmentItemType.ShortAnswer,p);
    }
    internal static string Solve(string f,IReadOnlyDictionary<string,int> p)=>
        TryExpected(f,p,out string expected)?expected:
        throw new InvalidOperationException("Invalid distribution parameters");
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? response)
    {
        if(string.IsNullOrWhiteSpace(response)||response.Length>90||
            !TryExpected(f,p,out string expected))return false;
        string answer=Normalized(response),wanted=Normalized(expected);
        if(answer==wanted)return true;
        if(f==Expand)
        {
            int a=p["a"],b=p["b"];
            return answer==$"{a}*س+{a*b}";
        }
        return int.TryParse(answer,NumberStyles.AllowLeadingSign,
            CultureInfo.InvariantCulture,out int actual)&&
            int.TryParse(expected,NumberStyles.AllowLeadingSign,
            CultureInfo.InvariantCulture,out int target)&&actual==target;
    }
}

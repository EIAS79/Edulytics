using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
/// <summary>
/// Bounded classification of commutativity, associativity, identity, and
/// counterexamples; existing G4 kernels handle numerical property subskills.
/// Free explanation and diagrams remain teacher-reviewed.
/// </summary>
internal static class UaeG6U6L08PropertiesQuestions
{
    internal const string Classify="uae.g6.u06.l08.classify_property";
    internal const string Rewrite="uae.g6.u06.l08.rewrite_associative";
    internal const string Area="uae.g6.u06.l08.triangle_area_reordered";
    internal sealed record Item(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? family)=>family is Classify or Rewrite or Area;
    private static string Normalize(string? s)
    {
        if(string.IsNullOrWhiteSpace(s))return "";
        return new string(s.Where(ch=>!char.IsWhiteSpace(ch))
          .Select(ch=>ch switch{
             >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
             >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
             '−' or '–'=>'-','٫'=>'.',_=>ch}).ToArray());
    }
    private static bool TryExpected(string family,IReadOnlyDictionary<string,int>? p,out string result)
    {
        result="";
        if(!Supports(family)||p is null)return false;
        if(family==Classify)
        {
            if(p.Count!=4||!p.TryGetValue("mode",out int mode)||
               !p.TryGetValue("a",out int a)||!p.TryGetValue("b",out int b)||
               !p.TryGetValue("c",out int c)||mode is <0 or >7||
               a is <2 or >30||b is <2 or >30||c is <2 or >30||a==b)return false;
            result=mode switch{0 or 5=>"التبديل",1 or 6=>"التجميع",
                2 or 7=>"المحايد",_=>"غير متكافئين"};
            return true;
        }
        if(family==Rewrite)
        {
            if(p.Count!=2||!p.TryGetValue("a",out int a)||
                !p.TryGetValue("b",out int b)||a is <1 or >20||b is <1 or >20)return false;
            result=$"س+{a}+{b}";return true;
        }
        if(p.Count!=2||!p.TryGetValue("b",out int basis)||
           !p.TryGetValue("h",out int height)||
           basis is <2 or >30||height is <2 or >30)return false;
        result=(basis*height/2m).ToString("0.#",CultureInfo.InvariantCulture);
        return true;
    }
    internal static Item Build(string family,Random r)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported algebra property");
        ArgumentNullException.ThrowIfNull(r);
        Dictionary<string,int> p;
        string prompt;
        if(family==Classify)
        {
            int mode=r.Next(8),a=r.Next(2,31),b=r.Next(2,31),c=r.Next(2,31);
            if(a==b)b=b==30?29:b+1;
            p=new(){{"mode",mode},{"a",a},{"b",b},{"c",c}};
            string left=mode switch{
               0=>$"{a}+{b} = {b}+{a}",
               1=>$"({a}+{b})+{c} = {a}+({b}+{c})",
               2=>$"{a}+0 = {a}",
               3=>$"{a}−{b} = {b}−{a}",
               4=>$"{a}÷{b} = {b}÷{a}",
               5=>$"{a}×{b} = {b}×{a}",
               6=>$"({a}×{b})×{c} = {a}×({b}×{c})",
               _=>$"{a}×1 = {a}"
            };
            prompt=$"صنف العلاقة {left}: اكتب التبديل أو التجميع أو المحايد أو غير متكافئين.";
        }
        else if(family==Rewrite)
        {
            int a=r.Next(1,21),b=r.Next(1,21);
            p=new(){{"a",a},{"b",b}};
            prompt=$"اكتب التعبير (س+{a})+{b} دون أقواس بنفس ترتيب الحدود مستعملًا خاصية التجميع.";
        }
        else
        {
            int b=r.Next(2,31),h=r.Next(2,31);
            p=new(){{"b",b},{"h",h}};
            prompt=$"أوجد مساحة مثلث قاعدته {b} متر وارتفاعه {h} متر باستخدام (١/٢)×القاعدة×الارتفاع، ويمكن تبديل ترتيب عوامل الضرب.";
        }
        return new Item(family,prompt,"تميّز بين تغيير ترتيب الحدود وتغيير التجميع، ولا تنقل خصائص الجمع إلى الطرح أو القسمة.",
            AssessmentItemType.ShortAnswer,p);
    }
    internal static string Solve(string family,IReadOnlyDictionary<string,int> p)=>
        TryExpected(family,p,out var answer)?answer:
            throw new InvalidOperationException("Invalid property parameters");
    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? response)
    {
        if(string.IsNullOrWhiteSpace(response)||response.Length>100||
            !TryExpected(family,p,out string expected))return false;
        string answer=Normalize(response),wanted=Normalize(expected);
        if(answer==wanted)return true;
        if(family==Rewrite&&p.TryGetValue("a",out int a)&&p.TryGetValue("b",out int b))
            return answer==$"س+{a+b}";
        if(family==Area&&decimal.TryParse(answer,NumberStyles.Number,
            CultureInfo.InvariantCulture,out decimal numeric)&&
            decimal.TryParse(expected,NumberStyles.Number,
            CultureInfo.InvariantCulture,out decimal target))return numeric==target;
        return false;
    }
}

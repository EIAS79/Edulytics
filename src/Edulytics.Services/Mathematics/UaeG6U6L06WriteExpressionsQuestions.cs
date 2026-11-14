using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;

/// <summary>
/// Bounded Arabic expression-writing questions for Grade 6 U06L06.
/// Preserves brackets for per-ticket charges; no general algebra equivalence claim.
/// </summary>
internal static class UaeG6U6L06WriteExpressionsQuestions
{
    internal const string MultipleMinus="uae.g6.u06.l06.multiple_minus";
    internal const string MultiplePlus="uae.g6.u06.l06.multiple_plus";
    internal const string FixedMinus="uae.g6.u06.l06.fixed_minus_variable";
    internal const string GroupPrice="uae.g6.u06.l06.grouped_ticket_cost";
    internal const string Inches="uae.g6.u06.l06.inches_to_centimeters";
    internal const string Markup="uae.g6.u06.l06.percentage_markup";
    internal sealed record Item(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f)=>f is MultipleMinus or MultiplePlus
        or FixedMinus or GroupPrice or Inches or Markup;
    private static string Norm(string? s) => new string((s??"").Where(ch=>!char.IsWhiteSpace(ch))
        .Select(ch=>ch switch{
            >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
            >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
            '−' or '–'=>'-','×' or '·'=>'*','٫'=>'.',_=>ch}).ToArray());
    private static bool TryExpected(string family,IReadOnlyDictionary<string,int>? p,out string expected)
    {
        expected="";
        if(!Supports(family)||p is null)return false;
        if(family==Inches)
        {
            if(p.Count!=0)return false;
            expected="2.54س";return true;
        }
        if(family==Markup)
        {
            if(p.Count!=1||!p.TryGetValue("pct",out int pct)||
                pct is not (10 or 15 or 20 or 25))return false;
            expected=$"{(1+pct/100m).ToString("0.##",CultureInfo.InvariantCulture)}س";
            return true;
        }
        if(family==FixedMinus)
        {
            if(p.Count!=1||!p.TryGetValue("fixed",out int fix)||
                fix is <1 or >100)return false;
            expected=$"{fix}-س";return true;
        }
        if(family==GroupPrice)
        {
            if(p.Count!=2||!p.TryGetValue("q",out int q)||
                !p.TryGetValue("fee",out int fee)||q is <2 or >6||
                fee is <1 or >30)return false;
            expected=$"{q}*(س+{fee})";return true;
        }
        if(p.Count!=2||!p.TryGetValue("a",out int a)||
           !p.TryGetValue("b",out int b)||a is <2 or >8||b is <1 or >30)return false;
        expected=family==MultipleMinus?$"{a}س-{b}":$"{a}س+{b}";
        return true;
    }
    internal static Item Build(string family,Random r)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported writing family");
        ArgumentNullException.ThrowIfNull(r);
        Dictionary<string,int> p;
        string question;
        if(family==Inches)
        {
            p=new();
            question="س عدد البوصات. إذا كانت البوصة تساوي ٢٫٥٤ سنتيمتر، اكتب تعبير عدد السنتيمترات.";
        }
        else if(family==Markup)
        {
            int[] rates=[10,15,20,25];
            int rate=rates[r.Next(rates.Length)];p=new(){{"pct",rate}};
            question=$"السعر س؛ اكتب تعبير السعر بعد زيادة بنسبة {rate}%.";
        }
        else if(family==FixedMinus)
        {
            int fixedAmount=r.Next(1,101);p=new(){{"fixed",fixedAmount}};
            question=$"اكتب تعبير المبلغ المتبقي عندما تطرح س من {fixedAmount}.";
        }
        else if(family==GroupPrice)
        {
            int q=r.Next(2,7),fee=r.Next(1,31);
            p=new(){{"q",q},{"fee",fee}};
            question=$"س ثمن تذكرة الواحدة، ورسوم ثابتة {fee} لكل تذكرة. اكتب تكلفة {q} تذاكر (مع الأقواس اللازمة).";
        }
        else
        {
            int a=r.Next(2,9),b=r.Next(1,31);
            p=new(){{"a",a},{"b",b}};
            question=family==MultipleMinus?
                $"اكتب تعبير «أقل بـ{b} من {a} أضعاف س».":
                $"اكتب تعبير «أكبر بـ{b} من {a} أضعاف س».";
        }
        return new Item(family,question,
            "حدد المتغير والمضاعف أولًا ثم الثابت، ولا تحذف الأقواس التي تغير المعنى.",
            AssessmentItemType.ShortAnswer,p);
    }
    internal static string Solve(string family,IReadOnlyDictionary<string,int> p)=>
        TryExpected(family,p,out string answer)?answer:
            throw new InvalidOperationException("Invalid expression parameters");
    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(string.IsNullOrWhiteSpace(answer)||answer.Length>80||
           !TryExpected(family,p,out string expected))return false;
        string actual=Norm(answer),target=Norm(expected);
        if(actual==target)return true;
        if(family==GroupPrice)
        {
            int q=p["q"],fee=p["fee"];
            return actual==$"{q}(س+{fee})";
        }
        if(family is MultiplePlus or MultipleMinus)
        {
            int a=p["a"],b=p["b"];
            return actual==$"{a}*س{(family==MultiplePlus?"+":"-")}{b}";
        }
        if(family==Inches)return actual=="2.54*س";
        if(family==Markup&&p.TryGetValue("pct",out int pct))
            return actual==$"س+{(pct/100m).ToString("0.##",CultureInfo.InvariantCulture)}س";
        return false;
    }
}

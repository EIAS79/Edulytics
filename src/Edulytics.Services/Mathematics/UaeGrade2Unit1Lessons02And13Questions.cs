using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

// First-party UAE G2 Unit 1 extensions to the existing exact Mathematics engine.
// Prepared in A-B-C; runtime and CI verification is deferred to Stage D.
internal static class UaeGrade2Unit1Lessons02And13Questions
{
    public const string CountOn = "uae.g2.u01.l02.count_on_number_line";
    public const string TwoStep = "uae.g2.u01.l13.two_step_story";
    public sealed record Item(string Family, string Prompt, string Solution,
        AssessmentItemType ItemType, IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? family) => family is CountOn or TwoStep;
    public static Item Build(string family, Random r)
    {
        if (!Supports(family)) throw new InvalidOperationException("Unknown UAE G2 U1 family");
        if (family == CountOn)
        {
            var a = r.Next(3,13);
            var b = r.Next(1,Math.Min(10,21-a));
            var p = new Dictionary<string,int>(StringComparer.Ordinal) {
                ["a"]=a, ["b"]=b, ["result"]=a+b };
            return new Item(family,
                $"ابدأ عند {a} على خط الأعداد 0–20، واقفز {b} قفزات إلى اليمين. أين تصل؟ {a}+{b}=؟",
                "لا نعد موضع البداية أول قفزة، بل نبدأ بالعلامة التالية ثم نتوقف بعد عدد القفزات.",
                AssessmentItemType.Numeric,p);
        }
        var start = r.Next(5,10);
        var add = r.Next(2,9);
        var remove = r.Next(1,start+1);
        var mode = r.Next(0,2);
        var p2 = new Dictionary<string,int>(StringComparer.Ordinal){
            ["start"]=start, ["add"]=add, ["remove"]=remove,
            ["mode"]=mode, ["result"]=start+add-remove };
        var prompt = mode == 0
            ? $"كان لدي {start} بطاقات، وجدت {add} ثم أعطيت {remove}. كم بقي؟ احسب مرحلتين."
            : $"في الحديقة {start} ورود، قطفنا {remove} ثم أضفنا {add} وردة. كم أصبح العدد؟ اكتب جملتين.";
        return new Item(family,prompt,
            "رتّب الحدث الأول والثاني، احسب النتيجة المؤقتة، ثم طبّق العملية الثانية عليها.",
            AssessmentItemType.Numeric,p2);
    }
    private static bool Valid(string family,IReadOnlyDictionary<string,int> p)
    {
        if (family == CountOn)
            return p.Count == 3 && p.TryGetValue("a",out var a) && a is >=3 and <=12 &&
                p.TryGetValue("b",out var b) && b is >=1 and <=9 && a+b<=20 &&
                p.TryGetValue("result",out var res) && res==a+b;
        return family == TwoStep && p.Count == 5 &&
            p.TryGetValue("start",out var start) && start is >=5 and <=9 &&
            p.TryGetValue("add",out var add) && add is >=2 and <=8 &&
            p.TryGetValue("remove",out var remove) && remove>=1 && remove<=start &&
            p.TryGetValue("mode",out var mode) && mode is 0 or 1 &&
            p.TryGetValue("result",out var result) && result == start + add - remove;
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid UAE G2 source contract");
        return p["result"].ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string answer)
    {
        if(!Valid(family,p)||string.IsNullOrWhiteSpace(answer))return false;
        var digits=new string(answer.Trim().Select(c=>
            c is >= '٠' and <= '٩' ? (char)('0'+c-'٠') :
            c is >= '۰' and <= '۹' ? (char)('0'+c-'۰') : c).ToArray());
        return int.TryParse(digits,NumberStyles.Integer,CultureInfo.InvariantCulture,out var x)
            && x==p["result"];
    }
}

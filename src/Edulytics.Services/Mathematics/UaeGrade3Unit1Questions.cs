using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;

// Grade 3 Unit 1: new lesson families plugged into the EXISTING exact engine.
// Class is first-party; no publisher wording or images. Stage D tests NOT_RUN.
internal static class UaeGrade3Unit1Questions
{
    internal const string DigitValue = "uae.g3.u01.l01.digit_value";
    internal const string Compose = "uae.g3.u01.l01.compose_four_digits";
    internal const string Expanded = "uae.g3.u01.l01.expanded_to_standard";
    internal sealed record Item(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? family) =>
        family is DigitValue or Compose or Expanded;

    public static Item Build(string family, Random rng)
    {
        if (!Supports(family)) throw new InvalidOperationException("Unknown UAE G3 U01 family");
        int number = rng.Next(1000, 10000);
        int pos = family == DigitValue ? rng.Next(0, 4) : -1;
        int[] digits = [number/1000, number/100%10, number/10%10, number%10];
        int result = pos >= 0 ? digits[pos] * (pos switch {0 => 1000,1 => 100,2 => 10,_ => 1}) : number;
        string prompt = family switch
        {
            DigitValue => $"في العدد {number}، ما قيمة الرقم في منزلة {new[]{"الآلاف","المئات","العشرات","الآحاد"}[pos]}؟",
            Compose => $"كوّن العدد: {digits[0]} آلاف، {digits[1]} مئات، {digits[2]} عشرات، {digits[3]} آحاد. اكتب الصيغة القياسية.",
            _ => $"اكتب الصيغة القياسية: {digits[0]*1000} + {digits[1]*100} + {digits[2]*10} + {digits[3]}."
        };
        var p = new Dictionary<string,int> { ["number"]=number,["pos"]=pos,["result"]=result };
        return new Item(family,prompt,
            "اكتب منزلة كل رقم، واضربه في قيمة المنزلة، ثم اجمع مع المحافظة على الأصفار.",
            AssessmentItemType.Numeric,p);
    }
    private static bool Valid(string family,IReadOnlyDictionary<string,int> p,out int result)
    {
        result=0;
        if (!Supports(family) || p.Count!=3 ||
            !p.TryGetValue("number",out int n) || n is <1000 or >9999 ||
            !p.TryGetValue("pos",out int pos) ||
            !p.TryGetValue("result",out int supplied)) return false;
        if (family==DigitValue && pos is <0 or >3) return false;
        if (family!=DigitValue && pos!=-1) return false;
        int[] digits=[n/1000,n/100%10,n/10%10,n%10];
        result=family!=DigitValue ? n : digits[pos] * (pos switch {0=>1000,1=>100,2=>10,_=>1});
        return result==supplied;
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if (!Valid(family,p,out int v)) throw new InvalidOperationException("Invalid G3 place-value parameters");
        return v.ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string answer)
    {
        if (!Valid(family,p,out int v) || string.IsNullOrWhiteSpace(answer)) return false;
        string digits=new(answer.Trim().Select(c =>
            c is >= '٠' and <= '٩' ? (char)('0'+c-'٠') :
            c is >= '۰' and <= '۹' ? (char)('0'+c-'۰') : c).ToArray());
        return int.TryParse(digits,NumberStyles.Integer,CultureInfo.InvariantCulture,out int entered) && entered==v;
    }
}

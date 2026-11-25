using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Grade 10 General U02L04 scientific notation. Bounded decimal-tenths
/// arithmetic with exact exponent normalization; no floating-point rounding.
/// Written in Stage C. Tests and production activation are Stage D only.
/// </summary>
public sealed partial class ExactSkillContractQuestionEngine
{
    private static ExactProblem BuildG10ScientificExponent(
        string family,Random random,ExactSkillQuestionDifficulty difficulty)
    {
        int raw=random.Next(1,1000);
        int a=random.Next(12,99),b=random.Next(12,99);
        int e1=random.Next(-6,7),e2=random.Next(-6,7);
        string number=(raw/10m).ToString("0.0",CultureInfo.InvariantCulture);
        string f1=(a/10m).ToString("0.0",CultureInfo.InvariantCulture);
        string f2=(b/10m).ToString("0.0",CultureInfo.InvariantCulture);
        string question=family switch
        {
            UaeG10ScientificExponentResponse.Normalize =>
                $"اكتب ({number} × 10^{e1}) بالترميز العلمي المعياري حيث 1≤المعامل<10. ما أس العشرة في الصورة النهائية؟ اكتب الأس الصحيح فقط.",
            UaeG10ScientificExponentResponse.Product =>
                $"اضرب ({f1} × 10^{e1}) في ({f2} × 10^{e2})، ثم طبع المعامل إلى المجال [1,10). ما الأس النهائي للعشرة؟ اكتب العدد الصحيح فقط.",
            _=>$"اقسم ({f1} × 10^{e1}) على ({f2} × 10^{e2})، ثم طبّع المعامل إلى المجال [1,10). ما أس العشرة في الصورة النهائية؟ اكتب العدد الصحيح فقط."
        };
        return Problem(family,question,
            "عند التحويل حرك الفاصلة العشرية حتى يصبح قدر المعامل بين 1 و10، واضبط أس 10 بالعكس. عند الضرب تجمع الأسس ويُطبَّع المعامل، وعند القسمة تطرح الأسس وتُطبَّع النسبة. راجع الوحدات في المسائل السياقية.",
            AssessmentItemType.Numeric,("raw",raw),("a",a),("b",b),("e1",e1),("e2",e2));
    }
}

internal static class UaeG10ScientificExponentResponse
{
    internal const string Normalize="uae.g10.general.u02.l04.scientific_normalize_exponent";
    internal const string Product="uae.g10.general.u02.l04.scientific_product_exponent";
    internal const string Quotient="uae.g10.general.u02.l04.scientific_quotient_exponent";

    internal static bool Supports(string? family)=>family is Normalize or Product or Quotient;
    private static bool Valid(IReadOnlyDictionary<string,int> p)
    {
        string[] keys=["raw","a","b","e1","e2"];
        return p.Count==keys.Length&&keys.All(p.ContainsKey)&&
            p["raw"] is >=1 and <=999&&p["a"] is >=12 and <=98&&
            p["b"] is >=12 and <=98&&p["e1"] is >=-6 and <=6&&p["e2"] is >=-6 and <=6;
    }
    private static int Exponent(string family,IReadOnlyDictionary<string,int> p)
    {
        if(family==Normalize)
            return p["e1"]+(p["raw"]<10?-1:p["raw"]>=100?1:0);
        if(family==Product)
            return p["e1"]+p["e2"]+(p["a"]*p["b"]>=1000?1:0);
        return p["e1"]-p["e2"]-(p["a"]<p["b"]?1:0);
    }
    internal static string Format(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Supports(family)||!Valid(p))
            throw new InvalidOperationException("Invalid Grade10 scientific notation parameters.");
        return Exponent(family,p).ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Supports(family)||!Valid(p)||string.IsNullOrWhiteSpace(answer)||answer.Length>32)return false;
        string normalized=string.Concat(answer.Trim().Select(c=>c switch
        {
            >= '٠' and <= '٩'=>(char)('0'+c-'٠'),
            >= '۰' and <= '۹'=>(char)('0'+c-'۰'),
            '−' or '–'=>'-',_=>c
        }));
        return int.TryParse(normalized,NumberStyles.AllowLeadingSign,CultureInfo.InvariantCulture,out int n)
               && n==Exponent(family,p);
    }
}

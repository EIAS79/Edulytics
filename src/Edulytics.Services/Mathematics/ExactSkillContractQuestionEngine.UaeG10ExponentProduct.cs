using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Owner-approved UAE Grade10 General U02L01, original Arabic 2025–2026
/// Volume1 PDF 98–103 / printed 85–90.
/// Product of same-base powers, power of a power, power of a product.
/// Source-accurate bounded integer probes. Authored in A–B–C;
/// Stage D must execute the tests before practice activation.
/// </summary>
public sealed partial class ExactSkillContractQuestionEngine
{
    private static ExactProblem BuildG10ExponentProduct(
        string family, Random random, ExactSkillQuestionDifficulty difficulty)
    {
        int bound=difficulty==ExactSkillQuestionDifficulty.Challenge?7:5;
        int baseNumber=random.Next(2,10);
        int a=random.Next(1,bound+1);
        int b=random.Next(1,bound+1);
        int coefficient=random.Next(1,7)*(random.Next(2)==0?1:-1);
        int power=random.Next(2,4);
        string prompt=family switch
        {
            UaeG10ExponentProductResponse.SameBase =>
                $"بسّط {baseNumber}^{a} × {baseNumber}^{b} على صورة قوة للأساس نفسه. ما الأس النهائي؟ اكتب عددًا صحيحًا فقط.",
            UaeG10ExponentProductResponse.PowerOfPower =>
                $"حوّل ({baseNumber}^{a})^{b} إلى قوة واحدة ذات الأساس نفسه. ما الأس النهائي؟ اكتب العدد فقط.",
            _=>$"إذا كان الحد الأحادي ({coefficient}س^{a}ص^{b}) مرفوعًا للأس {power}، فما المعامل العددي بعد التبسيط؟ لا تخلط بين ضرب المعامل ورفع كل عامل للقوة."
        };
        return Problem(family,prompt,
            "عند ضرب قوى الأساس نفسه تجمع الأسس؛ عند رفع قوة لقوة تضرب الأسس؛ وعند رفع حاصل ضرب لقوة ترفع كل عامل على حدة.",
            AssessmentItemType.Numeric,
            ("base",baseNumber),("a",a),("b",b),("coefficient",coefficient),("power",power));
    }
}

internal static class UaeG10ExponentProductResponse
{
    internal const string SameBase="uae.g10.general.u02.l01.same_base_exponent";
    internal const string PowerOfPower="uae.g10.general.u02.l01.power_of_power_exponent";
    internal const string ProductCoefficient="uae.g10.general.u02.l01.power_of_product_coefficient";
    internal static bool Supports(string? family)=>
        family is SameBase or PowerOfPower or ProductCoefficient;

    private static bool Valid(IReadOnlyDictionary<string,int> p)
    {
        string[] keys=["base","a","b","coefficient","power"];
        return p.Count==keys.Length&&keys.All(p.ContainsKey)&&
               p["base"] is >=2 and <=9 &&
               p["a"] is >=1 and <=7 &&
               p["b"] is >=1 and <=7 &&
               p["coefficient"] is >=-6 and <=6 and not 0 &&
               p["power"] is >=2 and <=3;
    }

    private static int Expected(string family,IReadOnlyDictionary<string,int> p)=>
        family switch
        {
            SameBase => p["a"]+p["b"],
            PowerOfPower => p["a"]*p["b"],
            ProductCoefficient => (int)Math.Pow(p["coefficient"],p["power"]),
            _=>throw new InvalidOperationException("Unsupported U02L01 source family.")
        };

    internal static string Format(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Supports(family)||!Valid(p))
            throw new InvalidOperationException("Invalid U02L01 power parameters.");
        return Expected(family,p).ToString(CultureInfo.InvariantCulture);
    }

    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Supports(family)||!Valid(p)||string.IsNullOrWhiteSpace(answer))return false;
        var normalized=string.Concat(answer.Trim().Select(c=>c switch
        {
            >= '٠' and <= '٩'=>(char)('0'+c-'٠'),
            >= '۰' and <= '۹'=>(char)('0'+c-'۰'),
            '−' or '–'=>'-',
            _=>c
        }));
        return int.TryParse(normalized,NumberStyles.AllowLeadingSign,CultureInfo.InvariantCulture,
                   out var actual) && actual==Expected(family,p);
    }
}

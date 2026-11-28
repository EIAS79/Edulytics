using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;

/// <summary>
/// UAE G10 canonical U04L03: independent exact numeric components of complex
/// arithmetic. Source includes square roots negative, i powers, conjugate,
/// addition, multiplication, division, complex plane. Bounded numeric subskills
/// only; graph drawing and rational complex division evaluated by a teacher.
/// Tests written in Stage C, NOT RUN or production enabled.
/// </summary>
public sealed partial class ExactSkillContractQuestionEngine
{
    private static ExactProblem BuildG10ComplexNumber(
        string family,Random random,ExactSkillQuestionDifficulty difficulty)
    {
        int lim=difficulty==ExactSkillQuestionDifficulty.Challenge?7:5;
        int a=random.Next(-lim,lim+1),b=random.Next(-lim,lim+1);
        int c=random.Next(-lim,lim+1),d=random.Next(-lim,lim+1);
        int n=random.Next(0,29);
        string z1=$"({a}+({b})ت)",z2=$"({c}+({d})ت)";
        string prompt=family switch
        {
            UaeG10ComplexResponse.SumReal=>
                $"اجمع العددين المركبين {z1} و{z2}. ما الجزء الحقيقي للناتج؟ اكتب عددًا صحيحًا.",
            UaeG10ComplexResponse.SumImag=>
                $"اجمع العددين المركبين {z1} و{z2}. ما معامل ت (الجزء التخيلي العددي) في الناتج؟ اكتب عددًا صحيحًا.",
            UaeG10ComplexResponse.ProductReal=>
                $"اضرب العددين المركبين {z1}×{z2}. ما الجزء الحقيقي للجداء بعد تبسيط ت²=−١؟",
            UaeG10ComplexResponse.ProductImag=>
                $"اضرب العددين المركبين {z1}×{z2}. ما معامل ت في الجداء بعد تجميع الحدود؟",
            UaeG10ComplexResponse.PowerReal=>
                $"عند تبسيط ت^{n} باستخدام ت²=−١ ودورية قوى ت الأربع، ما الجزء الحقيقي العددي؟ اكتب −١ أو ٠ أو ١.",
            _=>$"للعدد المركب {z1}، ما مربع قيمته المطلقة |ز|²؟ استخدم مجموع مربعي جزأيه واكتب عددًا صحيحًا."
        };
        return Problem(family,prompt,
            "إذا ز₁=أ+ب ت وز₂=ج+د ت، فإن الجمع (أ+ج)+(ب+د)ت، والجداء (أج−بد)+(أد+بج)ت، وت²=−١، وت⁴=١، ومربع القيمة المطلقة أ²+ب².",
            AssessmentItemType.Numeric,
            ("a",a),("b",b),("c",c),("d",d),("n",n));
    }
}

internal static class UaeG10ComplexResponse
{
    internal const string SumReal="uae.g10.general.u04.l03.sum_real";
    internal const string SumImag="uae.g10.general.u04.l03.sum_imag";
    internal const string ProductReal="uae.g10.general.u04.l03.product_real";
    internal const string ProductImag="uae.g10.general.u04.l03.product_imag";
    internal const string PowerReal="uae.g10.general.u04.l03.i_power_real";
    internal const string ModulusSquared="uae.g10.general.u04.l03.modulus_squared";
    internal static bool Supports(string? f)=>f is SumReal or SumImag or ProductReal or ProductImag or PowerReal or ModulusSquared;

    private static bool TryExpected(string family,IReadOnlyDictionary<string,int> p,out int answer)
    {
        answer=0;var keys=new[]{"a","b","c","d","n"};
        if(!Supports(family)||p.Count!=5||keys.Any(k=>!p.ContainsKey(k))||
           keys.Take(4).Any(k=>p[k] is < -7 or > 7)||
           p["n"] is <0 or >28)return false;
        int a=p["a"],b=p["b"],c=p["c"],d=p["d"],n=p["n"];
        answer=family switch
        {
            SumReal=>a+c,
            SumImag=>b+d,
            ProductReal=>a*c-b*d,
            ProductImag=>a*d+b*c,
            PowerReal=>(n%4) switch{0=>1,2=>-1,_=>0},
            _=>a*a+b*b
        };
        return true;
    }

    internal static string Format(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!TryExpected(family,p,out int x))throw new InvalidOperationException("Invalid G10 complex parameters.");
        return x.ToString(CultureInfo.InvariantCulture);
    }

    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? response)
    {
        if(!TryExpected(family,p,out int correct)||string.IsNullOrWhiteSpace(response))return false;
        var text=string.Concat(response.Trim().Select(ch=>ch switch
        {
            >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
            >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
            '−' or '–'=>'-',_=>ch
        }));
        return int.TryParse(text,NumberStyles.AllowLeadingSign,CultureInfo.InvariantCulture,out var submitted)&&submitted==correct;
    }
}

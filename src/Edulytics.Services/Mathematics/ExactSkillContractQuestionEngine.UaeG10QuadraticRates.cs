using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Canonical Grade10 U04L08: Applied quadratic rate-of-change lab, source
/// book printed p214 / PDF p31. One unit of start2 or step2 is 1/2 unit.
/// Even quadratic coefficient yields exact integral average slope on half-steps.
/// Stage C authored; CI and student validation exclusively in Stage D.
/// </summary>
public sealed partial class ExactSkillContractQuestionEngine
{
    private static ExactProblem BuildG10QuadraticRates(
        string family,Random random,ExactSkillQuestionDifficulty difficulty)
    {
        int a;
        do a=2*random.Next(-8,9); while(a==0);
        int b=random.Next(-20,21);
        int c=random.Next(0,101);
        int start2=random.Next(-8,9),step2=random.Next(1,5);
        if(random.Next(0,6)==0)
        {
            a=-16;b=144;c=0;
            start2=random.Next(0,17);step2=1;
        }
        double u=start2/2.0,v=(start2+step2)/2.0;
        string start=u.ToString("0.0",CultureInfo.InvariantCulture);
        string end=v.ToString("0.0",CultureInfo.InvariantCulture);
        string formula=$"د(ز)={a}ز²+({b})ز+({c})";
        string prompt=family==UaeG10QuadraticRatesResponse.Rate
            ? $"للدالة {formula} احسب معدل التغير المتوسط على الفترة من ز={start} إلى ز={end}. احسب (د(النهاية)−د(البداية))/(النهاية−البداية) ثم اكتب العدد الصحيح."
            : $"للدالة {formula} على الفترة [{start}، {end}]، هل معدل التغير المتوسط موجب؟ اكتب ١ إذا كان موجبًا و٠ إذا كان صفرًا أو سالبًا.";
        return Problem(family,prompt,
            "احسب قيمتي الدالة عند طرفي الفترة، ثم اقسم فرق المخرجات على فرق المدخلات. إذا كان المعامل أ سالبًا فالميل يتناقص مع الزمن، لكن قد يكون موجبًا قبل رأس القطع المكافئ.",
            AssessmentItemType.Numeric,("a",a),("b",b),("c",c),("start2",start2),("step2",step2));
    }
}
internal static class UaeG10QuadraticRatesResponse
{
    internal const string Rate="uae.g10.general.u04.l08.average_rate";
    internal const string Positive="uae.g10.general.u04.l08.rate_is_positive";
    internal static bool Supports(string? f)=>f is Rate or Positive;

    private static bool Expected(string family,IReadOnlyDictionary<string,int> p,out int answer)
    {
        answer=0;
        if(!Supports(family)||p.Count!=5||
            !p.ContainsKey("a")||!p.ContainsKey("b")||!p.ContainsKey("c")||
            !p.ContainsKey("start2")||!p.ContainsKey("step2"))return false;
        int a=p["a"],b=p["b"],c=p["c"],start2=p["start2"],step2=p["step2"];
        if(a==0||a is < -16 or > 16||a%2!=0||
           b is < -150 or > 150||c is < 0 or > 100||
           start2 is < -8 or > 16||step2 is < 1 or > 4)return false;
        // u=start2/2, v=(start2+step2)/2.
        // [a(v^2-u^2)+b(v-u)]/(v-u) = a(u+v)+b.
        int rate=(a/2)*(2*start2+step2)+b;
        answer=family==Rate?rate:(rate>0?1:0);
        return true;
    }

    internal static string Format(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Expected(family,p,out int value))
            throw new InvalidOperationException("Invalid G10 quadratic rates parameters.");
        return value.ToString(CultureInfo.InvariantCulture);
    }

    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? response)
    {
        if(!Expected(family,p,out int expected)||string.IsNullOrWhiteSpace(response))return false;
        string raw=string.Concat(response.Trim().Select(ch=>ch switch
        {
            >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
            >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
            '−' or '–'=>'-',_=>ch
        }));
        return int.TryParse(raw,NumberStyles.AllowLeadingSign,
            CultureInfo.InvariantCulture,out int parsed)&&parsed==expected;
    }
}

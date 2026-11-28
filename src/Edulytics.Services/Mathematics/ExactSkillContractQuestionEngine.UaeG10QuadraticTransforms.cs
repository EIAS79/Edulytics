using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Grade 10 canonical U04L07: source-bounded vertex-form transformations
/// y=a(x-h)^2+k. Shared exact engine; Stage D tests are written, not run.
/// </summary>
public sealed partial class ExactSkillContractQuestionEngine
{
    private static ExactProblem BuildG10QuadraticTransforms(
        string family,Random random,ExactSkillQuestionDifficulty difficulty)
    {
        int limit=difficulty==ExactSkillQuestionDifficulty.Challenge?4:3;
        int a;
        do a=random.Next(-limit,limit+1); while(a==0);
        int h=random.Next(-4,5),k=random.Next(-7,8),probe=h+random.Next(-2,3);
        string polynomial=$"د(س)={a}(س−({h}))²+({k})";
        string prompt=family switch
        {
            UaeG10QuadraticTransformsResponse.VertexX=>
                $"يُعطى القطع المكافئ بالصورة الرأسية {polynomial}. ما الإحداثي السيني للرأس بعد الإزاحة الأفقية؟ اكتب العدد.",
            UaeG10QuadraticTransformsResponse.VertexY=>
                $"يُعطى القطع المكافئ بالصورة الرأسية {polynomial}. ما إحداثي الرأس الصادي بعد الإزاحة الرأسية؟ اكتب العدد.",
            UaeG10QuadraticTransformsResponse.Opening=>
                $"للمنحنى {polynomial} هل تنفتح أذرعه لأعلى؟ اكتب ١ إذا كان لأعلى و٠ إذا كان لأسفل.",
            _=>
                $"للمنحنى {polynomial} احسب د({probe}) لتحديد نقطة تساعد على رسم المنحنى والتحقق من التناظر حول س={h}."
        };
        return Problem(family,prompt,
            "من الصورة الرأسية أ(س−هـ)²+ك: الرأس (هـ،ك)، والمحور س=هـ، وإشارة أ تحدد جهة الفتحة. عند التعويض في الدالة يُربّع الفرق س−هـ أولاً.",
            AssessmentItemType.Numeric,("a",a),("h",h),("k",k),("probe",probe));
    }
}

internal static class UaeG10QuadraticTransformsResponse
{
    internal const string VertexX="uae.g10.general.u04.l07.vertex_h";
    internal const string VertexY="uae.g10.general.u04.l07.vertex_k";
    internal const string Opening="uae.g10.general.u04.l07.opens_up";
    internal const string Probe="uae.g10.general.u04.l07.value_at_x";

    internal static bool Supports(string? family) =>
        family is VertexX or VertexY or Opening or Probe;

    private static bool Expected(string family,IReadOnlyDictionary<string,int> p,out int value)
    {
        value=0;
        if(!Supports(family)||p.Count!=4||
           !p.ContainsKey("a")||!p.ContainsKey("h")||
           !p.ContainsKey("k")||!p.ContainsKey("probe"))return false;
        int a=p["a"],h=p["h"],k=p["k"],probe=p["probe"];
        if(a is 0 or < -4 or > 4 ||h is < -4 or > 4 ||
           k is < -7 or > 7 ||probe < h-2||probe > h+2)return false;
        value=family switch
        {
            VertexX=>h,
            VertexY=>k,
            Opening=>a>0?1:0,
            _=>a*(probe-h)*(probe-h)+k
        };
        return true;
    }

    internal static string Format(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Expected(family,p,out int value))
            throw new InvalidOperationException("Invalid G10 vertex-transform parameters.");
        return value.ToString(CultureInfo.InvariantCulture);
    }

    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Expected(family,p,out int expected)||string.IsNullOrWhiteSpace(answer))return false;
        string raw=string.Concat(answer.Trim().Select(ch=>ch switch
        {
            >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
            >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
            '−' or '–'=>'-',_=>ch
        }));
        return int.TryParse(raw,NumberStyles.AllowLeadingSign,CultureInfo.InvariantCulture,out int result)
            &&result==expected;
    }
}

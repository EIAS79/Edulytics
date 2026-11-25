using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// UAE G10 U04L01: exact integer-coordinate parabola graph features.
/// Authored in source-based Stage C; test execution and production route belong to Stage D.
/// </summary>
public sealed partial class ExactSkillContractQuestionEngine
{
    private static ExactProblem BuildG10QuadraticGraph(
        string family,Random random,ExactSkillQuestionDifficulty difficulty)
    {
        int maxA=difficulty==ExactSkillQuestionDifficulty.Challenge?4:3;
        int a;do a=random.Next(-maxA,maxA+1);while(a==0);
        int h=random.Next(-4,5),k=random.Next(-8,9);
        int b=-2*a*h,c=a*h*h+k;
        int probe=h+random.Next(-3,4);
        string polynomial=$"د(س)={a}س²+({b})س+({c})";
        string prompt=family switch
        {
            UaeG10QuadraticGraphResponse.VertexX =>
                $"للدالة التربيعية {polynomial} ما الإحداثي السيني لرأس القطع المكافئ؟ اكتب عددًا صحيحًا.",
            UaeG10QuadraticGraphResponse.VertexY =>
                $"للدالة التربيعية {polynomial} ما الإحداثي الصادي لرأس القطع المكافئ، أي القيمة العظمى أو الصغرى؟ اكتب عددًا صحيحًا.",
            UaeG10QuadraticGraphResponse.Upward =>
                $"للدالة {polynomial} هل يفتح القطع المكافئ لأعلى؟ اكتب ١ إذا كان لأعلى و٠ إذا كان لأسفل.",
            _=>$"للدالة {polynomial} ما قيمة د({probe})؟ اكتب قيمة النقطة الصادية في جدول التمثيل البياني."
        };
        return Problem(family,prompt,
            "محور التناظر س=−ب/(٢أ)، والرأس (هـ،ك)؛ الدالة أ(س−هـ)²+ك. إذا أ>٠ فتحة لأعلى وقيمة صغرى، وإذا أ<٠ فتحة لأسفل وقيمة عظمى. حقق بالنقاط المتناظرة.",
            AssessmentItemType.Numeric,("a",a),("b",b),("c",c),("probe",probe));
    }
}
internal static class UaeG10QuadraticGraphResponse
{
    internal const string VertexX="uae.g10.general.u04.l01.vertex_x";
    internal const string VertexY="uae.g10.general.u04.l01.vertex_y";
    internal const string Upward="uae.g10.general.u04.l01.opens_up";
    internal const string Probe="uae.g10.general.u04.l01.value_at_x";
    internal static bool Supports(string? f)=>f is VertexX or VertexY or Upward or Probe;
    private static bool Expected(string family,IReadOnlyDictionary<string,int> p,out int expected)
    {
        expected=0;
        if(!Supports(family)||p.Count!=4||
            !p.ContainsKey("a")||!p.ContainsKey("b")||
            !p.ContainsKey("c")||!p.ContainsKey("probe"))return false;
        int a=p["a"],b=p["b"],c=p["c"],x=p["probe"];
        if(a is 0 or < -4 or > 4||x is < -7 or > 7||
           b is < -32 or > 32||c is < -72 or > 72||b%(2*a)!=0)return false;
        int h=-b/(2*a),k=c-a*h*h;
        if(h is < -4 or > 4||k is < -8 or > 8)return false;
        expected=family switch
        {
            VertexX=>h,
            VertexY=>k,
            Upward=>a>0?1:0,
            _=>a*x*x+b*x+c
        };
        return true;
    }
    internal static string Format(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Expected(family,p,out int x))throw new InvalidOperationException("Invalid G10 quadratic parameters.");
        return x.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Expected(family,p,out int expected)||string.IsNullOrWhiteSpace(answer))return false;
        var raw=string.Concat(answer.Trim().Select(ch=>ch switch
        {
            >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
            >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
            '−' or '–'=>'-',_=>ch
        }));
        return int.TryParse(raw,NumberStyles.AllowLeadingSign,
            CultureInfo.InvariantCulture,out int actual)&&actual==expected;
    }
}

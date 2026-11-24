using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Grade 10 approved General U01L12, original Advanced Arabic source 3-4,
/// PDF 221–230: bounded graph-analysis assessment. Original sampled
/// quadratic and cubic graphs are interpreted from mathematically accurate
/// tables. Drawing/approximate extrema/regression lab are teacher-scored.
/// Source-specific authoring during Stage C; tests NOT RUN until Stage D.
/// </summary>
public sealed partial class ExactSkillContractQuestionEngine
{
    private static ExactProblem BuildG10GraphAnalysis(
        string family,Random random,ExactSkillQuestionDifficulty difficulty)
    {
        int limit=difficulty==ExactSkillQuestionDifficulty.Challenge?4:3;
        if(family==UaeG10GraphAnalysisResponse.Intercepts)
        {
            int leading;
            do leading=random.Next(-3,4);while(leading==0);
            int[] roots=Enumerable.Range(-3,7)
                .OrderBy(_=>random.Next()).Take(3).OrderBy(x=>x).ToArray();
            string table=string.Join(" ؛ ",Enumerable.Range(-3,7).Select(x=>
            {
                int y=leading*(x-roots[0])*(x-roots[1])*(x-roots[2]);
                return $"({x}،{y})";
            }));
            string prompt=$"يوضح الجدول نقاطًا من منحنى دالة تكعيبية كثيرة الحدود: {table}. " +
                          "حدد قيم س عند تقاطع منحنى الدالة مع محور س. " +
                          "اكتب الأصفار الثلاثة بترتيب تصاعدي مفصولة بفاصلة.";
            return Problem(family,prompt,
                "تكون النقطة على محور السينات إذا كان الإحداثي الصادي صفرًا. استخرج جميع النقاط ذات القيمة 0 ثم اكتب قيم س تصاعديًا.",
                AssessmentItemType.ShortAnswer,
                ("a",leading),("h",0),("k",0),("r1",roots[0]),
                ("r2",roots[1]),("r3",roots[2]),("mode",2));
        }
        int a;
        do a=random.Next(-limit,limit+1);while(a==0);
        int h=random.Next(-3,4),k=random.Next(-4,5);
        string points=string.Join(" ؛ ",Enumerable.Range(-2,5).Select(offset=>
        {
            int x=h+offset,y=a*offset*offset+k;
            return $"({x}،{y})";
        }));
        string question=family==UaeG10GraphAnalysisResponse.Vertex
            ? "استنتج إحداثيي نقطة التحول (س،ص) من الجدول. اكتب العددين مفصولين بفاصلة."
            : "صنّف نقطة التحول من الجدول: هل تمثل قيمة عظمى نسبية أم صغرى نسبية؟ اكتب عظمى أو صغرى فقط.";
        string prompt=$"النقاط التالية مأخوذة من منحنى دالة تربيعية: {points}. {question}";
        return Problem(family,prompt,
            "تتبّع قيم د(س) في الجدول. عند نقطة التحول تتبدل من التزايد إلى التناقص أو العكس؛ رأس هذا القطع المكافئ هو (h,k).",
            AssessmentItemType.ShortAnswer,
            ("a",a),("h",h),("k",k),("r1",0),
            ("r2",0),("r3",0),("mode",1));
    }
}

internal static class UaeG10GraphAnalysisResponse
{
    internal const string Vertex="uae.g10.general.u01.l12.graph_vertex_pair";
    internal const string Kind="uae.g10.general.u01.l12.graph_extremum_kind";
    internal const string Intercepts="uae.g10.general.u01.l12.graph_x_intercepts";
    internal static bool Supports(string? family)=>
        family is Vertex or Kind or Intercepts;

    private static bool Valid(string family,IReadOnlyDictionary<string,int> p)
    {
        string[] required=["a","h","k","r1","r2","r3","mode"];
        if(!Supports(family)||p.Count!=required.Length||required.Any(k=>!p.ContainsKey(k)))
            return false;
        int a=p["a"],h=p["h"],k=p["k"];
        if(a==0||a is <-4 or >4||h is <-3 or >3||k is <-4 or >4)return false;
        if(family==Intercepts)
            return p["mode"]==2&&Math.Abs(a)<=3&&h==0&&k==0&&
                p["r1"] is >=-3 and <=3&&p["r2"] is >=-3 and <=3&&
                p["r3"] is >=-3 and <=3&&
                p["r1"]<p["r2"]&&p["r2"]<p["r3"];
        return p["mode"]==1&&p["r1"]==0&&p["r2"]==0&&p["r3"]==0;
    }

    private static string Normalize(string input)
    {
        return string.Concat(input.Trim().Select(c=>c switch
        {
            >= '٠' and <= '٩'=>(char)('0'+c-'٠'),
            >= '۰' and <= '۹'=>(char)('0'+c-'۰'),
            '−' or '–'=>'-',
            '،' or ';'=>',',
            _=>c
        }));
    }

    private static int[] ExactZeros(IReadOnlyDictionary<string,int> p)
    {
        int[] result=Enumerable.Range(-3,7).Where(x=>
            p["a"]*(x-p["r1"])*(x-p["r2"])*(x-p["r3"])==0).ToArray();
        if(result.Length!=3)
            throw new InvalidOperationException("Invalid cubic graph root count.");
        return result;
    }

    internal static string Format(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid graph-analysis parameters.");
        return family switch
        {
            Vertex=>$"{p["h"]},{p["k"]}",
            Kind=>p["a"]>0?"صغرى":"عظمى",
            _=>string.Join(",",ExactZeros(p))
        };
    }

    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? response)
    {
        if(!Valid(family,p)||string.IsNullOrWhiteSpace(response))return false;
        string text=Normalize(response);
        if(family==Kind)
        {
            string compact=string.Concat(text.Where(c=>!char.IsWhiteSpace(c)));
            return compact==(p["a"]>0?"صغرى":"عظمى");
        }
        text=text.Trim();
        if(text.StartsWith('(')&&text.EndsWith(')'))text=text[1..^1];
        var parts=text.Split(',',StringSplitOptions.None);
        int count=family==Vertex?2:3;
        if(parts.Length!=count)return false;
        int[] parsed=new int[count];
        for(int i=0;i<count;i++)
            if(!int.TryParse(parts[i].Trim(),NumberStyles.AllowLeadingSign,
                CultureInfo.InvariantCulture,out parsed[i]))return false;
        if(family==Vertex)
            return parsed[0]==p["h"]&&parsed[1]==p["k"];
        return parsed.SequenceEqual(ExactZeros(p));
    }
}

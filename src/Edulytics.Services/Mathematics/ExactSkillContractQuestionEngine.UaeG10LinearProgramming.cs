using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// UAE G10 U03L03 — exact bounded objective-function optimisation on a triangular
/// feasible region. Full hand-plotted regions, nontriangular systems and contexts
/// remain teacher reviewed; Stage D tests and production routing NOT RUN.
/// </summary>
public sealed partial class ExactSkillContractQuestionEngine
{
    private static ExactProblem BuildG10LinearProgramming(
        string family,Random random,ExactSkillQuestionDifficulty difficulty)
    {
        int bound=difficulty==ExactSkillQuestionDifficulty.Challenge?6:4;
        int a=random.Next(1,6),b=random.Next(1,6),k=random.Next(1,5);
        int p=0,q=0,mode=random.Next(0,2);
        int xVertex=b*k,yVertex=a*k;
        for(int attempt=0;attempt<128;attempt++)
        {
            p=random.Next(-bound,bound+1);
            q=random.Next(-bound,bound+1);
            if(p==0&&q==0)continue;
            int vx=p*xVertex,vy=q*yVertex;
            int opt=mode==0?Math.Max(0,Math.Max(vx,vy)):Math.Min(0,Math.Min(vx,vy));
            int winners=(opt==0?1:0)+(opt==vx?1:0)+(opt==vy?1:0);
            if(family==UaeG10LinearProgrammingResponse.ValueFamily||winners==1)break;
            if(attempt==127)throw new InvalidOperationException("Could not build unique optimum.");
        }
        int limit=a*b*k;
        string direction=mode==0?"العظمى":"الصغرى";
        string constraint=$"س≥٠، ص≥٠، ({a})س+({b})ص≤{limit}";
        string objective=$"ز=({p})س+({q})ص";
        string prompt=family==UaeG10LinearProgrammingResponse.ValueFamily
            ? $"في منطقة الحل الممكنة المحددة بالقيود {constraint}، أوجد القيمة {direction} للدالة الهدف {objective}. ارسم منطقة القيود أولًا وحدد رؤوسها، ثم اكتب القيمة النهائية عددًا صحيحًا."
            : $"في منطقة الحل الممكنة المحددة بالقيود {constraint}، تبلغ الدالة الهدف {objective} قيمة {direction} عند رأس واحد فقط. اكتب رمز الرأس: 0 عند الأصل (٠،٠)، 1 عند (#{xVertex}،٠)، 2 عند (٠،#{yVertex}).";
        prompt=prompt.Replace("#","");
        return Problem(family,prompt,
            "ارسم القيود واستخرج رؤوس المنطقة المثلثية: (٠،٠)، ("+xVertex+"،٠)، (٠،"+yVertex+"). قيّم الدالة الهدف عند كل رأس ثم اختر أكبر أو أصغر قيمة حسب المطلوب. إذا تساوت القيمة عند عدة رؤوس فإن الحل الأمثل قد يمتد على ضلع، ويُقيَّم ذلك في مسائل الرسم المطولة.",
            AssessmentItemType.Numeric,("a",a),("b",b),("k",k),("p",p),("q",q),("mode",mode));
    }
}

internal static class UaeG10LinearProgrammingResponse
{
    internal const string ValueFamily="uae.g10.general.u03.l03.optimal_objective_value";
    internal const string VertexFamily="uae.g10.general.u03.l03.unique_optimal_vertex";
    internal static bool Supports(string? family)=>family is ValueFamily or VertexFamily;

    private static bool Valid(IReadOnlyDictionary<string,int> v)
    {
        string[] keys=["a","b","k","p","q","mode"];
        return v.Count==6&&keys.All(v.ContainsKey)&&
            v["a"] is >=1 and <=5 && v["b"] is >=1 and <=5 &&
            v["k"] is >=1 and <=4 && v["p"] is >=-6 and <=6 &&
            v["q"] is >=-6 and <=6 && v["mode"] is 0 or 1 &&
            (v["p"]!=0||v["q"]!=0);
    }

    private static bool TryExpected(string family,IReadOnlyDictionary<string,int> v,out int expected)
    {
        expected=0;
        if(!Supports(family)||!Valid(v))return false;
        int[] values=[0,v["p"]*v["b"]*v["k"],v["q"]*v["a"]*v["k"]];
        int optimal=v["mode"]==0?values.Max():values.Min();
        if(family==ValueFamily){expected=optimal;return true;}
        int winners=values.Count(x=>x==optimal);
        if(winners!=1)return false;
        expected=Array.IndexOf(values,optimal);
        return true;
    }
    internal static string Format(string family,IReadOnlyDictionary<string,int> v)
    {
        if(!TryExpected(family,v,out var expected))
            throw new InvalidOperationException("Invalid linear programming parameters.");
        return expected.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string family,IReadOnlyDictionary<string,int> v,string? answer)
    {
        if(!TryExpected(family,v,out var expected)||
            string.IsNullOrWhiteSpace(answer)||answer.Length>20)return false;
        string normalized=string.Concat(answer.Trim().Select(c=>c switch
        {
            >= '٠' and <= '٩'=>(char)('0'+c-'٠'),
            >= '۰' and <= '۹'=>(char)('0'+c-'۰'),
            '−' or '–'=>'-',_=>c
        }));
        return int.TryParse(normalized,NumberStyles.AllowLeadingSign,
            CultureInfo.InvariantCulture,out int submitted)&&submitted==expected;
    }
}

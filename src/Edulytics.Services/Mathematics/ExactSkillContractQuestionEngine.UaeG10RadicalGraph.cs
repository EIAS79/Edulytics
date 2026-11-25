using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Grade10 General U02L07: table values and domain boundary of transformed
/// real square-root graphs y=a√(x-h)+k; exact point arithmetic, no graph
/// screenshot or pixel-location auto grading. Source Advanced4-3 and Applied6-1.
/// </summary>
public sealed partial class ExactSkillContractQuestionEngine
{
    private static ExactProblem BuildG10RadicalGraph(
        string family,Random random,ExactSkillQuestionDifficulty difficulty)
    {
        int a=random.Next(1,4)*(random.Next(0,2)==0?-1:1);
        int h=random.Next(-4,5),k=random.Next(-4,5),n=random.Next(0,6);
        string rule=$"ص={a}√(س−({h}))+({k})";
        string prompt=family==UaeG10RadicalGraphResponse.Value
            ? $"لدالة الجذر التربيعي {rule}، إذا كانت س={h+n*n} فما قيمة ص الصحيحة عند هذه النقطة؟ تحقق من شرط المجال وأجب بعدد صحيح."
            : $"ما أصغر قيمة حقيقية مسموحة للمتغير س (بداية المجال) للدالة {rule}؟ اكتب العدد الصحيح دون تكرار قاعدة الجذر.";
        return Problem(family,prompt,
            "ابدأ من س−h≥0، وبالتعويض في قيم تجعل س−h مربعًا كاملًا ينتج جذر صحيح؛ مجال الدالة س≥h، نقطة البداية (h,k)، واتجاهها يعكس إشارة معامل الجذر.",
            AssessmentItemType.Numeric,("a",a),("h",h),("k",k),("n",n));
    }
}
internal static class UaeG10RadicalGraphResponse
{
    internal const string Value="uae.g10.general.u02.l07.radical_graph_y_value";
    internal const string DomainStart="uae.g10.general.u02.l07.radical_graph_domain_start";
    internal static bool Supports(string? family)=>family is Value or DomainStart;
    private static bool Valid(IReadOnlyDictionary<string,int> p)
    {
        string[] keys=["a","h","k","n"];
        return p.Count==keys.Length&&keys.All(p.ContainsKey)&&
            p["a"] is >=-3 and <=3 and not 0&&p["h"] is >=-4 and <=4&&
            p["k"] is >=-4 and <=4&&p["n"] is >=0 and <=5;
    }
    private static int Expected(string family,IReadOnlyDictionary<string,int> p)=>
        family==DomainStart?p["h"]:p["a"]*p["n"]+p["k"];
    internal static string Format(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Supports(family)||!Valid(p))throw new InvalidOperationException("Invalid Grade10 radical graph parameters.");
        return Expected(family,p).ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Supports(family)||!Valid(p)||string.IsNullOrWhiteSpace(answer)||answer.Length>32)return false;
        var input=string.Concat(answer.Trim().Select(c=>c switch
        {
            >= '٠' and <= '٩'=>(char)('0'+c-'٠'),
            >= '۰' and <= '۹'=>(char)('0'+c-'۰'),
            '−' or '–'=>'-',_=>c
        }));
        return int.TryParse(input,NumberStyles.AllowLeadingSign,CultureInfo.InvariantCulture,out int actual)&&
               actual==Expected(family,p);
    }
}

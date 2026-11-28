using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Grade 10 General U04L04: exact, bounded completing-the-square tasks.
/// The integer-root subfamily genuinely requires shifting to a squared
/// binomial; non-integer/complex solutions and graphical proofs are teacher
/// assessed. Stage C authored; no Stage D tests or production activation.
/// </summary>
public sealed partial class ExactSkillContractQuestionEngine
{
    private static ExactProblem BuildG10CompletingSquare(
        string family,Random random,ExactSkillQuestionDifficulty difficulty)
    {
        int lim=difficulty==ExactSkillQuestionDifficulty.Challenge?7:5;
        int p;
        do p=random.Next(-lim,lim+1); while(p==0);
        int a=family==UaeG10CompletingSquareResponse.TermFamily?1:random.Next(1,5);
        int r=family==UaeG10CompletingSquareResponse.TermFamily?0:random.Next(1,7);
        string prompt;
        if(family==UaeG10CompletingSquareResponse.TermFamily)
        {
            prompt=$"أكمل المربع: ما الحد الثابت ج الذي نضيفه إلى س²+({2*p})س حتى تكون س²+({2*p})س+ج مربعًا كاملًا؟ اكتب العدد ج فقط.";
        }
        else
        {
            int b=2*a*p,c=a*(p*p-r*r);
            prompt=$"حل المعادلة {a}س²+({b})س+({c})=٠ بإكمال المربع. اقسم أولًا على {a} ثم أضف مربع نصف معامل س، واكتب الجذرين الصحيحين مفصولين بفاصلة دون كتابة س=.";
        }
        return Problem(family,prompt,
            "أكمل المربع بإضافة مربع نصف المعامل الخطي بعد قسمة المعادلة على أ. من أ(س+ر)²=أك² يكون س=−ر±ك؛ استخدم كلا الجذرين.",
            family==UaeG10CompletingSquareResponse.TermFamily
                ?AssessmentItemType.Numeric:AssessmentItemType.ShortAnswer,
            ("a",a),("p",p),("r",r));
    }
}

internal static class UaeG10CompletingSquareResponse
{
    internal const string TermFamily="uae.g10.general.u04.l04.square_constant";
    internal const string RootsFamily="uae.g10.general.u04.l04.completed_square_roots";
    internal static bool Supports(string? f)=>f is TermFamily or RootsFamily;

    private static bool Valid(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Supports(family)||p.Count!=3||
           !p.ContainsKey("a")||!p.ContainsKey("p")||!p.ContainsKey("r"))return false;
        int a=p["a"],b=p["p"],r=p["r"];
        if(a is <1 or >4||b is < -7 or >7 or 0)return false;
        return family==TermFamily ? (a==1&&r==0) : (r is >=1 and <=6);
    }

    private static string Normalize(string s)=>string.Concat(s.Trim().Select(c=>c switch
    {
        >= '٠' and <= '٩'=>(char)('0'+c-'٠'),
        >= '۰' and <= '۹'=>(char)('0'+c-'۰'),
        '−' or '–'=>'-',
        '،'=>',',
        _=>c
    }));

    internal static string Format(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid G10 completing-square parameters.");
        int x=p["p"],r=p["r"];
        if(family==TermFamily)return (x*x).ToString(CultureInfo.InvariantCulture);
        return string.Join(",",new[]{-x-r,-x+r}.OrderBy(z=>z).Select(z=>z.ToString(CultureInfo.InvariantCulture)));
    }

    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? response)
    {
        if(!Valid(family,p)||string.IsNullOrWhiteSpace(response)||response.Length>80)return false;
        string s=Normalize(response);
        if(family==TermFamily)
        {
            return int.TryParse(s,NumberStyles.AllowLeadingSign,
                CultureInfo.InvariantCulture,out var got) && got==p["p"]*p["p"];
        }
        var parts=s.Split(',',StringSplitOptions.None);
        if(parts.Length!=2)return false;
        if(!int.TryParse(parts[0].Trim(),NumberStyles.AllowLeadingSign,CultureInfo.InvariantCulture,out int one)||
           !int.TryParse(parts[1].Trim(),NumberStyles.AllowLeadingSign,CultureInfo.InvariantCulture,out int two))return false;
        int left=-p["p"]-p["r"],right=-p["p"]+p["r"];
        return (one==left&&two==right)||(one==right&&two==left);
    }
}

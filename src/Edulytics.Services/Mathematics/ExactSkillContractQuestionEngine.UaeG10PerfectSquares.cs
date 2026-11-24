using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// UAE G10 General U01L09: inverse perfect-square trinomial recognition,
/// full square factorization and exact shifted-square roots.
/// Stage C only; irrational square roots and contexts require teacher review.
/// </summary>
public sealed partial class ExactSkillContractQuestionEngine
{
    private static ExactProblem BuildG10PerfectSquares(
        string family,Random random,ExactSkillQuestionDifficulty difficulty)
    {
        if(family==UaeG10PerfectSquaresResponse.ShiftRootsFamily)
        {
            int h=random.Next(-6,7),r=random.Next(1,9);
            return Problem(family,
                $"حل المعادلة (س−({h}))²={r*r} بخاصية الجذر التربيعي؛ اكتب الجذرين مفصولين بفاصلة.",
                "خذ الجذر التربيعي الموجب والسالب للطرفين ثم أضف الإزاحة؛ لا تُسقط الجذر الثاني.",
                AssessmentItemType.ShortAnswer,("h",h),("r",r));
        }

        int bound=difficulty==ExactSkillQuestionDifficulty.Challenge?6:4;
        int a=random.Next(1,bound+1),b=random.Next(1,8);
        int sign=random.Next(0,2)==0?-1:1;
        int delta=family==UaeG10PerfectSquaresResponse.RecognitionFamily?random.Next(0,2):0;
        int middle=sign*2*a*b+delta;
        string expression=$"{a*a}س²"+(middle>=0?"+":"−")+
            Math.Abs(middle).ToString(CultureInfo.InvariantCulture)+"س+"+
            (b*b).ToString(CultureInfo.InvariantCulture);
        return Problem(family,
            family==UaeG10PerfectSquaresResponse.RecognitionFamily
            ?$"هل ثلاثية الحدود {expression} مربع كامل لثنائية حد؟ اكتب «نعم» أو «لا»؛ تحقق من الحد الأوسط."
            :$"حلّل ثلاثية الحدود المربعة الكاملة {expression} إلى مربع ثنائية حد، واكتب الناتج في الصورة (أ س±ب)².",
            "تحقق أن أول وحدين طرفيين مربعان كاملان، وأن الوسط يساوي ±ضعف حاصل ضرب جذريهما، ثم استعمل (أ±ب)².",
            AssessmentItemType.ShortAnswer,("a",a),("b",b),("sign",sign),("delta",delta));
    }
}

internal static class UaeG10PerfectSquaresResponse
{
    internal const string FactorFamily="uae.g10.general.u01.l09.full_perfect_square_factor";
    internal const string RecognitionFamily="uae.g10.general.u01.l09.classify_perfect_square";
    internal const string ShiftRootsFamily="uae.g10.general.u01.l09.shifted_square_two_roots";
    internal static bool Supports(string? f)=>f is FactorFamily or RecognitionFamily or ShiftRootsFamily;

    private static bool Valid(IReadOnlyDictionary<string,int> p,string f)
    {
        if(f==ShiftRootsFamily)return p.Count==2&&p.ContainsKey("h")&&p.ContainsKey("r")&&
            p["h"] is >=-6 and <=6&&p["r"] is >=1 and <=8;
        return p.Count==4&&new[]{"a","b","sign","delta"}.All(p.ContainsKey)&&
            p["a"] is >=1 and <=6&&p["b"] is >=1 and <=7&&
            p["sign"] is -1 or 1&&
            p["delta"] is >=0 and <=1&&
            (f!=FactorFamily||p["delta"]==0);
    }

    private static string Factor(int a,int b,int sign)=>
        "("+(a==1?"س":a.ToString(CultureInfo.InvariantCulture)+"س")+
        (sign>0?"+":"−")+b.ToString(CultureInfo.InvariantCulture)+")²";

    internal static string Format(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Supports(family)||!Valid(p,family))throw new InvalidOperationException("Invalid G10 perfect-square question parameters.");
        if(family==RecognitionFamily)return p["delta"]==0?"نعم":"لا";
        if(family==FactorFamily)return Factor(p["a"],p["b"],p["sign"]);
        return (p["h"]-p["r"]).ToString(CultureInfo.InvariantCulture)+","+
            (p["h"]+p["r"]).ToString(CultureInfo.InvariantCulture);
    }

    private static string? Normalize(string? raw)
    {
        if(string.IsNullOrWhiteSpace(raw)||raw.Length>128)return null;
        var s=new StringBuilder();
        foreach(char c0 in raw.Trim())
        {
            char c=c0 switch {
                >= '٠' and <= '٩'=>(char)('0'+c0-'٠'),
                >= '۰' and <= '۹'=>(char)('0'+c0-'۰'),
                '−' or '–'=>'-','س' or 'X'=>'x','²'=>'^','،'=>',',_=>c0
            };
            if(char.IsWhiteSpace(c))continue;
            if(c is not ('+' or '-' or '(' or ')' or 'x' or '^' or ',' or 'ن' or 'ع' or 'م' or 'ل' or 'ا')&&
                (c<'0'||c>'9'))return null;
            s.Append(c);
            if(c0=='²')s.Append('2');
        }
        return s.ToString();
    }

    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Supports(family)||!Valid(p,family))return false;
        string? text=Normalize(answer);
        if(text is null)return false;
        if(family==RecognitionFamily)return text==(p["delta"]==0?"نعم":"لا");
        if(family==ShiftRootsFamily)
        {
            var parts=text.Split(',',StringSplitOptions.None);
            if(parts.Length!=2||
               !int.TryParse(parts[0],NumberStyles.AllowLeadingSign,CultureInfo.InvariantCulture,out int n1)||
               !int.TryParse(parts[1],NumberStyles.AllowLeadingSign,CultureInfo.InvariantCulture,out int n2))return false;
            int a=p["h"]-p["r"],b=p["h"]+p["r"];
            return (n1==a&&n2==b)||(n1==b&&n2==a);
        }
        var m=Regex.Match(text,@"^\((?<lead>[1-9][0-9]*)?x(?<sign>[+-])(?<constant>[1-9][0-9]*)\)\^2$",
            RegexOptions.CultureInvariant);
        if(!m.Success)return false;
        int lead=1;
        if(m.Groups["lead"].Success&&!int.TryParse(m.Groups["lead"].Value,
             NumberStyles.None,CultureInfo.InvariantCulture,out lead))return false;
        if(!int.TryParse(m.Groups["constant"].Value,NumberStyles.None,
             CultureInfo.InvariantCulture,out int b))return false;
        int sgn=m.Groups["sign"].Value=="+"?1:-1;
        return lead==p["a"]&&b==p["b"]&&sgn==p["sign"];
    }
}

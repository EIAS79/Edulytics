using System.Globalization;
using System.Text;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// UAE Grade10 approved General U01L15, Advanced source Arabic
/// 2025–2026 Unit3 3-7 (PDF249–257). Fundamental theorem of algebra,
/// Descartes sign-variation upper bounds, and complex conjugate zeros.
/// Source-specific stage C authoring; runtime test/activation deferred to D.
/// </summary>
public sealed partial class ExactSkillContractQuestionEngine
{
    private static ExactProblem BuildG10RootsZeros(
        string family,Random random,ExactSkillQuestionDifficulty difficulty)
    {
        if(family==UaeG10RootsZerosResponse.Descartes)
        {
            int max=difficulty==ExactSkillQuestionDifficulty.Challenge?5:3;
            int degree=random.Next(2,6);
            var c=new int[6];
            for(int i=0;i<degree;i++)c[i]=random.Next(-max,max+1);
            do c[degree]=random.Next(-max,max+1);while(c[degree]==0);
            string expression=UaeG10PolynomialFunctionResponse.FormatPolynomial(c);
            return Problem(family,
                $"لدالة كثيرة الحدود د(س)={expression}. حسب قاعدة ديكارت للإشارات، ما أكبر عدد ممكن من الأصفار الحقيقية الموجبة وما أكبر عدد ممكن من الأصفار الحقيقية السالبة؟ اكتب عددين (موجب،سالب) مفصولين بفاصلة. هذه حدود عليا وليست عدد الأصفار الفعلي.",
                "احسب تغير الإشارات بين المعاملات غير الصفرية لد(س) بعد ترتيبها تنازليًا، ثم في د(−س). يمكن أن يقل عدد الأصفار الحقيقية عن الحد الأعلى بعدد زوجي.",
                AssessmentItemType.ShortAnswer,
                ("a0",c[0]),("a1",c[1]),("a2",c[2]),
                ("a3",c[3]),("a4",c[4]),("a5",c[5]),("degree",degree));
        }
        if(family==UaeG10RootsZerosResponse.Conjugate)
        {
            int real=random.Next(-4,5);
            int imaginary=random.Next(1,5)*(random.Next(0,2)==0?1:-1);
            string root=UaeG10RootsZerosResponse.FormatComplex(real,imaginary);
            return Problem(family,
                $"لِكثيرة حدود ذات معاملات حقيقية جذر مركب هو {root}. وفق نظرية المترافقين المركبين، اكتب الجذر المركب المرافق باستخدام i ورقمين صحيحين مثل 2-3i.",
                "جذور المعادلات ذات المعاملات الحقيقية غير الحقيقية تأتي في أزواج مرافقات: أ+بi ↔ أ−بi.",
                AssessmentItemType.ShortAnswer,("real",real),("imag",imaginary));
        }
        int m=random.Next(1,5),n=random.Next(1,5);
        if(m+n>6)n=6-m;
        int r=random.Next(-4,5),s;
        do s=random.Next(-4,5);while(s==r);
        string a=r<0?$"(س+{-r})":$"(س−{r})";
        string b=s<0?$"(س+{-s})":$"(س−{s})";
        return Problem(family,
            $"كثيرة الحدود د(س)={a}^{m}×{b}^{n}. بحسب النظرية الأساسية للجبر، كم جذرًا لها في مجموعة الأعداد المركبة إذا حُسب تكرار كل جذر؟ اكتب عددًا صحيحًا فقط.",
            "عدد الجذور المركبة مع التكرار يساوي درجة كثيرة الحدود. درجة حاصل ضرب العاملين الخطيين = م+ن، حتى إذا لم تظهر إلا قيمتان مختلفتان للجذور.",
            AssessmentItemType.Numeric,("m",m),("n",n),("r",r),("s",s));
    }
}

internal static class UaeG10RootsZerosResponse
{
    internal const string Descartes="uae.g10.general.u01.l15.descartes_variations";
    internal const string Conjugate="uae.g10.general.u01.l15.complex_conjugate_zero";
    internal const string Multiplicity="uae.g10.general.u01.l15.total_roots_with_multiplicity";
    internal static bool Supports(string? family)=>
        family is Descartes or Conjugate or Multiplicity;

    internal static string FormatComplex(int real,int imag)=>
        $"{real}{(imag<0?"-":"+")}{Math.Abs(imag)}i";

    private static bool Valid(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Supports(family))return false;
        if(family==Conjugate)
            return p.Count==2&&p.ContainsKey("real")&&p.ContainsKey("imag")&&
                p["real"] is >=-4 and <=4 &&
                p["imag"] is >=-4 and <=4 and not 0;
        if(family==Multiplicity)
            return p.Count==4&&new[]{"m","n","r","s"}.All(p.ContainsKey)&&
                p["m"] is >=1 and <=4 && p["n"] is >=1 and <=4 &&
                p["m"]+p["n"] is >=2 and <=6 &&
                p["r"] is >=-4 and <=4&&p["s"] is >=-4 and <=4&&p["r"]!=p["s"];
        string[] keys=["a0","a1","a2","a3","a4","a5","degree"];
        if(p.Count!=7||keys.Any(k=>!p.ContainsKey(k))||p["degree"] is <2 or >5)
            return false;
        for(int i=0;i<=5;i++)
        {
            int c=p["a"+i.ToString(CultureInfo.InvariantCulture)];
            if(c is <-5 or >5 ||(i>p["degree"]&&c!=0))return false;
        }
        return p["a"+p["degree"].ToString(CultureInfo.InvariantCulture)]!=0;
    }

    private static int Changes(IReadOnlyDictionary<string,int> p,bool negative)
    {
        int degree=p["degree"],count=0,prev=0;
        for(int d=degree;d>=0;d--)
        {
            int c=p["a"+d.ToString(CultureInfo.InvariantCulture)];
            if(negative&&d%2!=0)c=-c;
            if(c==0)continue;
            int sign=Math.Sign(c);
            if(prev!=0&&sign!=prev)count++;
            prev=sign;
        }
        return count;
    }

    internal static string Format(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))
            throw new InvalidOperationException("Invalid source Grade10 roots/zeros family parameters.");
        return family switch
        {
            Descartes=>$"{Changes(p,false)},{Changes(p,true)}",
            Conjugate=>FormatComplex(p["real"],-p["imag"]),
            _=>(p["m"]+p["n"]).ToString(CultureInfo.InvariantCulture)
        };
    }

    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Valid(family,p)||string.IsNullOrWhiteSpace(answer))return false;
        var b=new StringBuilder();
        foreach(char c in answer.Trim())
            b.Append(c switch
            {
                >= '٠' and <= '٩'=>(char)('0'+c-'٠'),
                >= '۰' and <= '۹'=>(char)('0'+c-'۰'),
                '−' or '–'=>'-',
                '،' or ';'=>',',
                _=>c
            });
        string normalized=string.Concat(b.ToString().Where(c=>!char.IsWhiteSpace(c)));
        if(family==Conjugate)
            return normalized==FormatComplex(p["real"],-p["imag"]);
        if(family==Multiplicity)
            return int.TryParse(normalized,NumberStyles.None,
                CultureInfo.InvariantCulture,out int n)&&n==p["m"]+p["n"];
        var parts=normalized.Split(',',StringSplitOptions.None);
        return parts.Length==2&&
            int.TryParse(parts[0],NumberStyles.None,CultureInfo.InvariantCulture,out int pos)&&
            int.TryParse(parts[1],NumberStyles.None,CultureInfo.InvariantCulture,out int neg)&&
            pos==Changes(p,false)&&neg==Changes(p,true);
    }
}

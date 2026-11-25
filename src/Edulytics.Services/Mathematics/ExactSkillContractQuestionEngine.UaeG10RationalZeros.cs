using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// UAE Grade10 unified U01L16: rational zero theorem, based on Advanced
/// 2025–2026 Arabic Unit3 Lesson3-8, PDF258–263.
/// Exact integer arithmetic only; no CAS or fabricated polynomial roots.
/// Written during A–B–C, runtime proof reserved for Stage D.
/// </summary>
public sealed partial class ExactSkillContractQuestionEngine
{
    private static ExactProblem BuildG10RationalZeros(
        string family,Random random,ExactSkillQuestionDifficulty difficulty)
    {
        int range=difficulty==ExactSkillQuestionDifficulty.Challenge?5:3;
        int a,b,c,p,q;
        do a=random.Next(-range,range+1);while(a==0);
        b=random.Next(-range,range+1);
        do c=random.Next(-range,range+1);while(c==0);
        do p=random.Next(-range,range+1);while(p==0);
        q=random.Next(1,4);
        // Construct (q*x-p)(a*x²+b*x+c); thus p/q is a guaranteed
        // zero in one branch. Other candidates are selected independently.
        int a3=q*a,a2=q*b-p*a,a1=q*c-p*b,a0=-p*c;
        var parameters=new Dictionary<string,int>(StringComparer.Ordinal)
        {
            ["a3"]=a3,["a2"]=a2,["a1"]=a1,["a0"]=a0,
            ["p"]=p,["q"]=q
        };
        string polynomial=UaeG10RationalZerosResponse.FormatPolynomial(parameters);
        if(family==UaeG10RationalZerosResponse.CandidateCount)
            return Problem(family,
                $"للدالة كثيرة الحدود د(س)={polynomial} ذات معاملات صحيحة، كم عدد الأعداد النسبية المختلفة المحتملة حسب نظرية الصفر النسبي بعد اختزال الكسور وإدراج الإشارتين؟ لا تقل إن جميعها أصفار فعلية. اكتب عددًا صحيحًا.",
                "لأي صفر نسبي مختزل ر/م يكون ر عاملًا للحد الثابت وم عاملًا للمعامل الرئيس. اختزل التكرارات واحسب القيم الموجبة والسالبة.",
                AssessmentItemType.Numeric,("a3",a3),("a2",a2),("a1",a1),("a0",a0),("p",p),("q",q));
        if(random.Next(0,2)==1)
        {
            int other;
            do other=random.Next(-range*2,range*2+1);while(other==0||other==p);
            p=other;
            q=random.Next(1,5);
        }
        parameters["p"]=p;parameters["q"]=q;
        string candidate=q==1?p.ToString(CultureInfo.InvariantCulture):
            $"{p}/{q}";
        string request=family==UaeG10RationalZerosResponse.CandidateCheck
            ? $"للدالة د(س)={polynomial}، هل العدد النسبي {candidate} من الأصفار المحتملة وفق عوامل الحد الثابت والمعامل الرئيس فقط؟ الإجابة «نعم» أو «لا». الاحتمال لا يعني أنه صفر فعلي."
            : $"للدالة د(س)={polynomial}، هل العدد {candidate} صفر فعلي للدالة؟ اختبر بالتعويض أو القسمة التركيبية قبل الإجابة «نعم» أو «لا».";
        return Problem(family,request,
            "اختزل الكسر أولًا؛ للأصفار المحتملة تحقق من عوامل الثابت والرئيس. للصفر الفعلي احسب حاصل تعويض البسط/المقام في كثيرة الحدود بالضرب في المقام المرفوع للقوة الثالثة.",
            AssessmentItemType.ShortAnswer,("a3",a3),("a2",a2),("a1",a1),("a0",a0),("p",p),("q",q));
    }
}

internal static class UaeG10RationalZerosResponse
{
    internal const string CandidateCount="uae.g10.general.u01.l16.rational_candidate_count";
    internal const string CandidateCheck="uae.g10.general.u01.l16.rational_candidate_membership";
    internal const string ActualZero="uae.g10.general.u01.l16.rational_actual_zero";
    internal static bool Supports(string? family)=>
        family is CandidateCount or CandidateCheck or ActualZero;

    internal static string FormatPolynomial(IReadOnlyDictionary<string,int> p)=>
        $"{p["a3"]}س³+({p["a2"]})س²+({p["a1"]})س+({p["a0"]})";

    private static int Gcd(int a,int b)
    {
        a=Math.Abs(a);b=Math.Abs(b);
        while(b!=0){int t=a%b;a=b;b=t;}
        return a;
    }

    private static bool Valid(IReadOnlyDictionary<string,int> p)=>
        p.Count==6&&new[]{"a3","a2","a1","a0","p","q"}.All(p.ContainsKey)&&
        p["a3"] is >=-30 and <=30 and not 0&&
        p["a2"] is >=-40 and <=40&&p["a1"] is >=-40 and <=40&&
        p["a0"] is >=-30 and <=30 and not 0&&
        p["p"] is >=-10 and <=10 and not 0&&p["q"] is >=1 and <=5;

    private static HashSet<(int,int)> Possible(IReadOnlyDictionary<string,int> p)
    {
        var result=new HashSet<(int,int)>();
        int constant=Math.Abs(p["a0"]),leading=Math.Abs(p["a3"]);
        for(int numerator=1;numerator<=constant;numerator++)
        {
            if(constant%numerator!=0)continue;
            for(int denominator=1;denominator<=leading;denominator++)
            {
                if(leading%denominator!=0)continue;
                int divisor=Gcd(numerator,denominator);
                int n=numerator/divisor,d=denominator/divisor;
                result.Add((n,d));result.Add((-n,d));
            }
        }
        return result;
    }

    private static bool Candidate(IReadOnlyDictionary<string,int> p)
    {
        int g=Gcd(p["p"],p["q"]);
        return Possible(p).Contains((p["p"]/g,p["q"]/g));
    }

    private static bool IsZero(IReadOnlyDictionary<string,int> p)
    {
        long n=p["p"],d=p["q"];
        long value=p["a3"]*n*n*n+p["a2"]*n*n*d+
                   p["a1"]*n*d*d+p["a0"]*d*d*d;
        return value==0;
    }

    internal static string Format(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Supports(family)||!Valid(p))
            throw new InvalidOperationException("Invalid source-bounded rational zero question.");
        if(family==CandidateCount)
            return Possible(p).Count.ToString(CultureInfo.InvariantCulture);
        return (family==CandidateCheck?Candidate(p):IsZero(p))?"نعم":"لا";
    }

    private static string Normalize(string? value)
    {
        if(value is null)return string.Empty;
        var b=new System.Text.StringBuilder();
        foreach(char c in value.Trim())
            b.Append(c switch
            {
                >= '٠' and <= '٩'=>(char)('0'+c-'٠'),
                >= '۰' and <= '۹'=>(char)('0'+c-'۰'),
                _=>c
            });
        return b.ToString();
    }

    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Supports(family)||!Valid(p)||string.IsNullOrWhiteSpace(answer))return false;
        string input=Normalize(answer);
        if(family==CandidateCount)
            return int.TryParse(input,NumberStyles.None,CultureInfo.InvariantCulture,out int count)&&
                   count==Possible(p).Count;
        // Arabic, English and numeral glyphs do not alter the underlying oracle.
        return input==Format(family,p);
    }
}

using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Canonical UAE G10 General U01L10 from Advanced 2025 student pp204–212.
/// Polynomial long/synthetic division with a monic linear divisor x−r.
/// Source teacher scope also includes monomial division, arbitrary
/// leading divisors, hand-calculator work and real-world modeling.
/// Stage C authored only, production disabled and tests NOT RUN.
/// </summary>
public sealed partial class ExactSkillContractQuestionEngine
{
    private static ExactProblem BuildG10PolynomialDivision(
        string family, Random random, ExactSkillQuestionDifficulty difficulty)
    {
        int limit=difficulty==ExactSkillQuestionDifficulty.Challenge?6:4;
        int degree=random.Next(1,3);
        int q2=degree==2?NonZeroG10PolynomialDivision(random,limit):0;
        int q1=degree==1?NonZeroG10PolynomialDivision(random,limit):random.Next(-limit,limit+1);
        int q0=random.Next(-limit,limit+1),r=random.Next(-4,5);
        int remainder=random.Next(-6,7);
        var p=new Dictionary<string,int>(StringComparer.Ordinal)
        {
            ["degree"]=degree,["q2"]=q2,["q1"]=q1,["q0"]=q0,
            ["r"]=r,["remainder"]=remainder
        };
        string dividend=UaeG10PolynomialDivisionResponse.Dividend(p);
        string divisor=r>=0?$"(س−{r})":$"(س+{-r})";
        string prompt=family==UaeG10PolynomialDivisionResponse.QuotientFamily
            ?$"أوجد خارج قسمة كثيرة الحدود {dividend} على {divisor} بالقسمة المطولة أو التركيبية. اكتب خارج القسمة بالصورة القياسية."
            :$"عند قسمة كثيرة الحدود {dividend} على {divisor} بالقسمة التركيبية، ما **الباقي**؟ اكتب عددًا صحيحًا فقط.";
        return Problem(family,prompt,
            "اكتب الحد المفقود بمعامل صفري، ثم استخدم القسمة المطولة/التركيبية. تحقق من D(x)=(x−r)Q(x)+R ومن أن الباقي يساوي D(r).",
            family==UaeG10PolynomialDivisionResponse.QuotientFamily?
                AssessmentItemType.ShortAnswer:AssessmentItemType.Numeric,
            ("degree",degree),("q2",q2),("q1",q1),("q0",q0),("r",r),("remainder",remainder));
    }

    private static int NonZeroG10PolynomialDivision(Random random,int limit)
    {
        int n;
        do n=random.Next(-limit,limit+1);while(n==0);
        return n;
    }
}

internal static class UaeG10PolynomialDivisionResponse
{
    internal const string QuotientFamily="uae.g10.general.u01.l10.full_linear_divisor_quotient";
    internal const string RemainderFamily="uae.g10.general.u01.l10.linear_divisor_remainder";
    internal static bool Supports(string? family)=>family is QuotientFamily or RemainderFamily;

    private static bool Valid(IReadOnlyDictionary<string,int> p)
    {
        string[] names=["degree","q2","q1","q0","r","remainder"];
        return p.Count==names.Length && names.All(p.ContainsKey) &&
            p["degree"] is 1 or 2 &&
            p["q2"] is >=-6 and <=6 &&
            p["q1"] is >=-6 and <=6 &&
            p["q0"] is >=-6 and <=6 &&
            p["r"] is >=-4 and <=4 &&
            p["remainder"] is >=-6 and <=6 &&
            (p["degree"]==1 ? p["q2"]==0&&p["q1"]!=0 : p["q2"]!=0);
    }

    private static int[] QuotientCoefficients(IReadOnlyDictionary<string,int> p)=>
        [p["q0"],p["q1"],p["q2"],0,0];

    private static int[] DividendCoefficients(IReadOnlyDictionary<string,int> p)
    {
        int q0=p["q0"],q1=p["q1"],q2=p["q2"],r=p["r"];
        // (x−r)(q2*x²+q1*x+q0)+remainder
        return [p["remainder"]-r*q0,q0-r*q1,q1-r*q2,q2,0];
    }

    internal static string Dividend(IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(p))throw new InvalidOperationException("Invalid G10 polynomial division parameters.");
        return UaeG10MonomialPolynomialResponse.Format(DividendCoefficients(p));
    }

    internal static string Format(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Supports(family)||!Valid(p))throw new InvalidOperationException("Invalid G10 polynomial division family/parameters.");
        return family==QuotientFamily
            ? UaeG10MonomialPolynomialResponse.Format(QuotientCoefficients(p))
            : p["remainder"].ToString(CultureInfo.InvariantCulture);
    }

    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Supports(family)||!Valid(p)||string.IsNullOrWhiteSpace(answer))return false;
        if(family==QuotientFamily)
            return UaeG10MonomialPolynomialResponse.TryParse(answer,out var coeffs)&&
                coeffs.SequenceEqual(QuotientCoefficients(p));
        string normalized=string.Concat(answer.Trim().Select(ch=>ch switch
        {
            >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
            >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
            '−' or '–'=>'-',
            _=>ch
        }));
        return int.TryParse(normalized,NumberStyles.AllowLeadingSign,
            CultureInfo.InvariantCulture,out int submitted)&&
            submitted==p["remainder"];
    }
}

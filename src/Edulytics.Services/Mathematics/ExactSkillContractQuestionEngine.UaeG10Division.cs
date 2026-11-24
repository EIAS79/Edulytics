using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Owner approved unified Grade10 General U01L10 polynomial division.
/// Limited integer-exact monomial division and monic linear synthetic/long division.
/// Source Advanced 2025–26 Volume1 PDF pages 204–212. Stage C only.
/// </summary>
public sealed partial class ExactSkillContractQuestionEngine
{
    private static ExactProblem BuildG10Division(
        string family,Random random,ExactSkillQuestionDifficulty difficulty)
    {
        int bnd=difficulty==ExactSkillQuestionDifficulty.Challenge?6:4;
        if(family==UaeG10DivisionResponse.MonomialFamily)
        {
            int m=random.Next(1,bnd+1)*(random.Next(0,2)==0?-1:1);
            int k=random.Next(0,3);
            int q0=random.Next(-bnd,bnd+1),q1=random.Next(-bnd,bnd+1);
            int q2=random.Next(1,bnd+1)*(random.Next(0,2)==0?-1:1);
            var dividend=new int[5];
            dividend[k]=m*q0;dividend[k+1]=m*q1;dividend[k+2]=m*q2;
            string divisor=UaeG10MonomialPolynomialResponse.Format(
                Enumerable.Range(0,5).Select(d=>d==k?m:0).ToArray());
            string prompt=$"اقسم كثيرة الحدود ({UaeG10MonomialPolynomialResponse.Format(dividend)}) " +
                $"على الحد الأحادي ({divisor}). اكتب خارج القسمة المبسط بالصورة القياسية.";
            return Problem(family,prompt,
                "اقسم كل حد على الحد الأحادي، ثم اطرح أسس المتغير المتشابه واكتب الناتج المبسط. تحقق بأن حاصل ضرب القاسم وخارج القسمة يساوي المقسوم.",
                AssessmentItemType.ShortAnswer,
                ("m",m),("k",k),("q0",q0),("q1",q1),("q2",q2));
        }

        int r;
        do r=random.Next(-bnd,bnd+1);while(r==0);
        int c2=random.Next(1,bnd+1)*(random.Next(0,2)==0?-1:1);
        int c1=random.Next(-bnd,bnd+1),c0=random.Next(-bnd,bnd+1);
        int rem=random.Next(-bnd,bnd+1);
        int[] numerator=[rem-r*c0,c0-r*c1,c1-r*c2,c2,0];
        string divisorText=r<0?$"(س+{-r})":$"(س−{r})";
        string question=$"اقسم كثيرة الحدود ({UaeG10MonomialPolynomialResponse.Format(numerator)}) " +
            $"على {divisorText}.";
        bool remainderOnly=family==UaeG10DivisionResponse.RemainderFamily;
        question+=remainderOnly
            ?" ما الباقي الصحيح؟ اكتب عددًا فقط."
            :" اكتب خارج القسمة والباقي بالصيغة Q=<كثيرة الحدود>;R=<عدد>.";
        return Problem(family,question,
            "استخدم القسمة المطولة أو التركيبية على س−ر؛ اكتب الحدود الناقصة بمعامل صفر. تحقق من المقسوم=(القاسم×خارج القسمة)+الباقي وأن درجة الباقي أقل من درجة القاسم.",
            remainderOnly?AssessmentItemType.Numeric:AssessmentItemType.ShortAnswer,
            ("r",r),("q0",c0),("q1",c1),("q2",c2),("rem",rem));
    }
}

internal static class UaeG10DivisionResponse
{
    internal const string MonomialFamily="uae.g10.general.u01.l10.monomial_division_expression";
    internal const string QuotientRemainderFamily="uae.g10.general.u01.l10.linear_quotient_remainder";
    internal const string RemainderFamily="uae.g10.general.u01.l10.linear_remainder_integer";
    internal static bool Supports(string? s)=>
        s is MonomialFamily or QuotientRemainderFamily or RemainderFamily;

    private static bool ValidMonomial(IReadOnlyDictionary<string,int> p)
    {
        string[] keys=["m","k","q0","q1","q2"];
        return p.Count==5&&keys.All(p.ContainsKey)&&
            p["m"] is >=-6 and <=6 and not 0&&
            p["k"] is >=0 and <=2&&
            p["q0"] is >=-6 and <=6&&p["q1"] is >=-6 and <=6&&
            p["q2"] is >=-6 and <=6 and not 0;
    }

    private static bool ValidLinear(IReadOnlyDictionary<string,int> p)
    {
        string[] keys=["r","q0","q1","q2","rem"];
        return p.Count==5&&keys.All(p.ContainsKey)&&
            p["r"] is >=-6 and <=6 and not 0&&
            p["q0"] is >=-6 and <=6&&p["q1"] is >=-6 and <=6&&
            p["q2"] is >=-6 and <=6 and not 0&&p["rem"] is >=-6 and <=6;
    }

    private static int[] Quotient(IReadOnlyDictionary<string,int> p)=>
        [p["q0"],p["q1"],p["q2"],0,0];

    internal static string Format(string family,IReadOnlyDictionary<string,int> p)
    {
        if(family==MonomialFamily&&ValidMonomial(p))
            return UaeG10MonomialPolynomialResponse.Format(Quotient(p));
        if(family==RemainderFamily&&ValidLinear(p))
            return p["rem"].ToString(CultureInfo.InvariantCulture);
        if(family==QuotientRemainderFamily&&ValidLinear(p))
            return "Q="+UaeG10MonomialPolynomialResponse.Format(Quotient(p))+
                ";R="+p["rem"].ToString(CultureInfo.InvariantCulture);
        throw new InvalidOperationException("Invalid Grade10 Division family/parameters.");
    }

    private static string NormalizeInteger(string s)=>
        string.Concat(s.Select(c=>c switch
        {
            >= '٠' and <= '٩'=>(char)('0'+c-'٠'),
            >= '۰' and <= '۹'=>(char)('0'+c-'۰'),
            '−' or '–'=>'-',
            _=>c
        }));

    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Supports(family)||string.IsNullOrWhiteSpace(answer))return false;
        if(family==MonomialFamily)
            return ValidMonomial(p)&&
                UaeG10MonomialPolynomialResponse.TryParse(answer,out var parsedMono)&&
                parsedMono.SequenceEqual(Quotient(p));
        if(!ValidLinear(p))return false;
        if(family==RemainderFamily)
            return int.TryParse(NormalizeInteger(answer.Trim()),NumberStyles.AllowLeadingSign,
                CultureInfo.InvariantCulture,out var r)&&r==p["rem"];
        // A compound structured input, exact quotient and exact integer remainder;
        // both must independently match (no accepting numeric-only remainders).
        string compact=string.Concat(answer.Where(c=>!char.IsWhiteSpace(c)));
        int delimiter=compact.IndexOf(';');
        if(delimiter<3||delimiter!=compact.LastIndexOf(';'))return false;
        string left=compact[..delimiter],right=compact[(delimiter+1)..];
        if(!left.StartsWith("Q=",StringComparison.OrdinalIgnoreCase)||
           !right.StartsWith("R=",StringComparison.OrdinalIgnoreCase))return false;
        return UaeG10MonomialPolynomialResponse.TryParse(left[2..],out var q)&&
               q.SequenceEqual(Quotient(p))&&
               int.TryParse(NormalizeInteger(right[2..]),NumberStyles.AllowLeadingSign,
                   CultureInfo.InvariantCulture,out int remainder)&&remainder==p["rem"];
    }
}

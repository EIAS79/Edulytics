using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// First-party bounded G10 General U01L06: monic-trinomial integer factorization,
/// distinct roots, and prime-over-integers decision. Uses the existing exact engine;
/// manual area tiles, geometric feasibility, graphs and proofs remain teacher-reviewed.
/// Tests authored in Stage C are NOT RUN; productionRouting=false.
/// </summary>
public sealed partial class ExactSkillContractQuestionEngine
{
    private static ExactProblem BuildG10MonicQuadratic(
        string family,Random random,ExactSkillQuestionDifficulty difficulty)
    {
        if(family==UaeG10MonicQuadraticResponse.PrimeFamily)
        {
            int b=random.Next(-9,10),c;
            do c=random.Next(-20,21);while(c==0);
            string poly=UaeG10MonicQuadraticResponse.Polynomial(b,c);
            return Problem(family,
                $"هل يمكن تحليل ثلاثية الحدود {poly} إلى عاملين خطيين بمعاملات صحيحة؟ اكتب «نعم» أو «لا» بعد فحص العددين اللذين مجموعهما معامل س وحاصل ضربهما الحد الثابت.",
                "إذا أمكن إيجاد عددين صحيحين مجموعهما ب وحاصل ضربهما ج، فإن ثلاثية الحدود قابلة للتحليل الصحيح. وإلا فهي أولية بالنسبة للأعداد الصحيحة.",
                AssessmentItemType.ShortAnswer,("b",b),("c",c));
        }
        int r1,r2;
        do r1=random.Next(-6,7);while(r1==0);
        do r2=random.Next(-6,7);while(r2==0||
            (family==UaeG10MonicQuadraticResponse.RootsFamily&&r2==r1));
        int bCoeff=-(r1+r2),cCoeff=r1*r2;
        string expr=UaeG10MonicQuadraticResponse.Polynomial(bCoeff,cCoeff);
        string prompt=family==UaeG10MonicQuadraticResponse.FactorFamily
            ?$"حلّل ثلاثية الحدود {expr} إلى عاملين خطيين بمعاملات صحيحة. اكتب الصيغة (س±عدد)(س±عدد)؛ يمكن تبديل ترتيب العاملين، واستعمل مربع ثنائية عند تكرارهما."
            :$"حل المعادلة {expr}=٠ بالتحليل إلى العوامل وخاصية الضرب الصفري، واكتب الجذرين المختلفين مفصولين بفاصلة.";
        return Problem(family,prompt,
            "ابحث عن عددين حاصل ضربهما ج ومجموعهما ب، اكتب العاملين (س+م)(س+ن)، وتحقق بالتوزيع. للمعادلة حلّ س+م=٠ أو س+ن=٠.",
            AssessmentItemType.ShortAnswer,("r1",r1),("r2",r2));
    }
}

internal static class UaeG10MonicQuadraticResponse
{
    internal const string FactorFamily="uae.g10.general.u01.l06.monic_full_factor";
    internal const string RootsFamily="uae.g10.general.u01.l06.monic_integer_roots";
    internal const string PrimeFamily="uae.g10.general.u01.l06.monic_prime_over_integers";
    internal static bool Supports(string? family)=>family is FactorFamily or RootsFamily or PrimeFamily;

    internal static string Polynomial(int b,int c)
    {
        var middle=b==0?"":(b<0?"−":"+")+(Math.Abs(b)==1?"":Math.Abs(b).ToString(CultureInfo.InvariantCulture))+"س";
        var end=c<0?"−"+Math.Abs(c).ToString(CultureInfo.InvariantCulture):
            "+"+c.ToString(CultureInfo.InvariantCulture);
        return "س²"+middle+end;
    }
    private static bool ValidRoots(IReadOnlyDictionary<string,int> p,string family)=>
        p.Count==2&&p.TryGetValue("r1",out int r1)&&p.TryGetValue("r2",out int r2)&&
        r1 is >= -6 and <=6 and not 0&&r2 is >=-6 and <=6 and not 0&&
        (family!=RootsFamily||r1!=r2);
    private static bool ValidPrime(IReadOnlyDictionary<string,int> p)=>
        p.Count==2&&p.TryGetValue("b",out int b)&&p.TryGetValue("c",out int c)&&
        b is >=-9 and <=9&&c is >=-20 and <=20 and not 0;
    private static bool IntegerFactorable(int b,int c)
    {
        // For integer-coefficient monic quadratics, a factorization into
        // integer monic linear terms exists iff there are integer roots.
        for(int root=-20;root<=20;root++)
        {
            if(root==0||c%root!=0)continue;
            int mate=c/root;
            if(root+mate==-b)return true;
        }
        return false;
    }
    private static string SignedBinomial(int root)=>
        "(س"+(root<0?"+"+Math.Abs(root):"−"+root)+")";

    internal static string Format(string family,IReadOnlyDictionary<string,int> p)
    {
        if(family==PrimeFamily)
        {
            if(!ValidPrime(p))throw new InvalidOperationException("Bad prime parameters.");
            return IntegerFactorable(p["b"],p["c"])?"نعم":"لا";
        }
        if(!ValidRoots(p,family))throw new InvalidOperationException("Bad roots parameters.");
        int a=p["r1"],b=p["r2"];
        if(family==RootsFamily)
            return string.Join(",",new[]{a,b}.OrderBy(x=>x)
                .Select(x=>x.ToString(CultureInfo.InvariantCulture)));
        if(a==b)return SignedBinomial(a)+"²";
        return SignedBinomial(a)+SignedBinomial(b);
    }
    private static string? Normalize(string? raw)
    {
        if(string.IsNullOrWhiteSpace(raw)||raw.Length>128)return null;
        var sb=new StringBuilder();
        foreach(char input in raw.Trim())
        {
            char c=input switch
            {
                >= '٠' and <= '٩'=>(char)('0'+input-'٠'),
                >= '۰' and <= '۹'=>(char)('0'+input-'۰'),
                '−' or '–'=>'-','س' or 'X'=>'x','،'=>',',_=>input
            };
            if(char.IsWhiteSpace(c))continue;
            if(c=='²'){sb.Append("^2");continue;}
            if(c is not ('+' or '-' or 'x' or '^' or '(' or ')' or ',')&&
                (c<'0'||c>'9')&&c is not ('ن' or 'ع' or 'م' or 'ل' or 'ا'))return null;
            sb.Append(c);
        }
        return sb.ToString();
    }
    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Supports(family))return false;
        var input=Normalize(answer);if(input is null)return false;
        if(family==PrimeFamily)
            return ValidPrime(p)&&input==Format(family,p);
        if(!ValidRoots(p,family))return false;
        int r1=p["r1"],r2=p["r2"];
        if(family==RootsFamily)
        {
            var parts=input.Split(',',StringSplitOptions.None);
            if(parts.Length!=2||
                !int.TryParse(parts[0],NumberStyles.AllowLeadingSign,CultureInfo.InvariantCulture,out int a)||
                !int.TryParse(parts[1],NumberStyles.AllowLeadingSign,CultureInfo.InvariantCulture,out int b))return false;
            return (a==r1&&b==r2)||(a==r2&&b==r1);
        }
        // Both factor orderings or a squared binomial for repeated roots are
        // accepted. Verify by independently reconstructing b and c.
        const string term=@"\(x(?<sgn>[+-])(?<mag>[1-9][0-9]*)\)";
        string twoTerms="^"+term+term+"$";
        string squared="^"+term+@"\^2$";
        var match=Regex.Match(input,twoTerms,RegexOptions.CultureInvariant);
        int m,n;
        if(match.Success)
        {
            var sign=match.Groups["sgn"].Captures;
            var mag=match.Groups["mag"].Captures;
            if(sign.Count!=2||mag.Count!=2||
               !int.TryParse(mag[0].Value,NumberStyles.None,CultureInfo.InvariantCulture,out int v1)||
               !int.TryParse(mag[1].Value,NumberStyles.None,CultureInfo.InvariantCulture,out int v2))return false;
            m=sign[0].Value=="-"?-v1:v1;n=sign[1].Value=="-"?-v2:v2;
        }
        else
        {
            match=Regex.Match(input,squared,RegexOptions.CultureInvariant);
            if(!match.Success||r1!=r2)return false;
            if(!int.TryParse(match.Groups["mag"].Value,NumberStyles.None,CultureInfo.InvariantCulture,out int v))return false;
            m=n=match.Groups["sgn"].Value=="-"?-v:v;
        }
        return m+n==-(r1+r2)&&m*n==r1*r2;
    }
}

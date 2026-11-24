using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// UAE G10 General U01L07: a*x²+b*x+c, a>1, exact integer factor pairs,
/// rational roots and integer-factorability. First-party Stage C authored.
/// Not an unrestricted CAS. All actual Practice is fail-closed pending Stage D.
/// </summary>
public sealed partial class ExactSkillContractQuestionEngine
{
    private static ExactProblem BuildG10NonmonicQuadratic(
        string family,Random random,ExactSkillQuestionDifficulty difficulty)
    {
        if(family==UaeG10NonmonicQuadraticResponse.PrimeFamily)
        {
            int a,b,c;
            if(random.Next(0,2)==0)
            {
                int u,w,v,z;
                do
                {
                    u=random.Next(1,5);w=random.Next(1,5);
                    v=random.Next(-5,6);z=random.Next(-5,6);
                    a=u*w;b=u*z+w*v;c=v*z;
                }
                while(a<=1||a>8||b==0||Math.Abs(b)>14||c==0||Math.Abs(c)>20);
            }
            else
            {
                a=random.Next(2,9);
                do b=random.Next(-14,15);while(b==0);
                do c=random.Next(-20,21);while(c==0);
            }
            return Problem(family,
                $"هل يمكن تحليل ثلاثية الحدود {UaeG10NonmonicQuadraticResponse.Polynomial(a,b,c)} إلى حاصل ضرب ثنائيتين بمعاملات صحيحة؟ اكتب «نعم» أو «لا»، وحدد سبب اختيارك.",
                "ابحث عن تحليل صحيح باستعمال أ×ج وفصل الحد الأوسط، أو اختبر الأزواج الصحيحة لمعاملَي س والحدين الثابتين. قد يكون وجود جذور حقيقية غير صحيحة لا يعني تحليلًا صحيحًا.",
                AssessmentItemType.ShortAnswer,("a",a),("b",b),("c",c));
        }

        int u,w,v,z,aCoeff,bCoeff,cCoeff;
        do
        {
            u=random.Next(1,5);w=random.Next(1,5);
            v=random.Next(-6,7);z=random.Next(-6,7);
            aCoeff=u*w;bCoeff=u*z+w*v;cCoeff=v*z;
        }while(aCoeff<=1||v==0||z==0||bCoeff==0||
              (family==UaeG10NonmonicQuadraticResponse.RootsFamily&&u*z==w*v));

        string expression=UaeG10NonmonicQuadraticResponse.Polynomial(aCoeff,bCoeff,cCoeff);
        bool roots=family==UaeG10NonmonicQuadraticResponse.RootsFamily;
        return Problem(family,
            roots
            ?$"حل المعادلة {expression}=٠ بالتحليل إلى العوامل، واكتب الجذرين مفصولين بفاصلة؛ استخدم الكسر المبسط أ/ب عندما يلزم."
            :$"حلّل ثلاثية الحدود {expression} إلى حاصل ضرب ثنائيتين بمعاملات صحيحة، باستعمال أ×ج وتقسيم الحد الأوسط ثم التحليل بالتجميع. اكتب العاملين بين قوسين.",
            "اختر عددين حاصل ضربهما أ×ج ومجموعهما ب، فصل الحد الأوسط ثم اجمع الحدود في مجموعتين. تحقق بضرب العاملين، وللحلول استخدم خاصية الضرب الصفري.",
            AssessmentItemType.ShortAnswer,("u",u),("w",w),("v",v),("z",z));
    }
}

internal static class UaeG10NonmonicQuadraticResponse
{
    internal const string FactorFamily="uae.g10.general.u01.l07.nonmonic_full_factor";
    internal const string RootsFamily="uae.g10.general.u01.l07.nonmonic_rational_roots";
    internal const string PrimeFamily="uae.g10.general.u01.l07.nonmonic_prime_over_integers";
    internal static bool Supports(string? family)=>family is FactorFamily or RootsFamily or PrimeFamily;

    internal static string Polynomial(int a,int b,int c)
    {
        string leading=a.ToString(CultureInfo.InvariantCulture)+"س²";
        string middle=b<0?"−":"+";middle+=(Math.Abs(b)==1?"":Math.Abs(b).ToString(CultureInfo.InvariantCulture))+"س";
        return leading+middle+(c<0?"−":"+")+Math.Abs(c).ToString(CultureInfo.InvariantCulture);
    }

    private static bool ValidFactor(IReadOnlyDictionary<string,int> p,string family)
    {
        if(p.Count!=4||!new[]{"u","w","v","z"}.All(p.ContainsKey))return false;
        int u=p["u"],w=p["w"],v=p["v"],z=p["z"];
        return u is >=1 and <=4&&w is >=1 and <=4&&u*w>1&&
            v is >=-6 and <=6 and not 0&&z is >=-6 and <=6 and not 0&&
            u*z+w*v!=0&&(family!=RootsFamily||u*z!=w*v);
    }

    private static bool ValidPrime(IReadOnlyDictionary<string,int> p)=>
        p.Count==3&&new[]{"a","b","c"}.All(p.ContainsKey)&&
        p["a"] is >=2 and <=8&&p["b"] is >=-14 and <=14 and not 0&&
        p["c"] is >=-20 and <=20 and not 0;

    private static bool FactorableOverIntegers(int a,int b,int c)
    {
        // Nonmonic integer binomial factorization:
        // (u*x+v)*(w*x+z): u*w=a, v*z=c, u*z+w*v=b.
        for(int u=1;u<=a;u++)
        {
            if(a%u!=0)continue;
            int w=a/u;
            for(int v=-Math.Abs(c);v<=Math.Abs(c);v++)
            {
                if(v==0||c%v!=0)continue;
                int z=c/v;
                if(u*z+w*v==b)return true;
            }
        }
        return false;
    }

    private static int Gcd(int a,int b)
    {
        a=Math.Abs(a);b=Math.Abs(b);
        while(b!=0){int t=a%b;a=b;b=t;}
        return a;
    }
    private static string Fraction(int numerator,int denominator)
    {
        if(denominator<=0)throw new InvalidOperationException("Bad denominator.");
        int g=Gcd(numerator,denominator),n=numerator/g,d=denominator/g;
        return d==1?n.ToString(CultureInfo.InvariantCulture):
            n.ToString(CultureInfo.InvariantCulture)+"/"+d.ToString(CultureInfo.InvariantCulture);
    }

    private static string Term(int lead,int constant)
    {
        string head=lead==1?"س":lead.ToString(CultureInfo.InvariantCulture)+"س";
        return "("+head+(constant<0?"−":"+")+
            Math.Abs(constant).ToString(CultureInfo.InvariantCulture)+")";
    }

    internal static string Format(string family,IReadOnlyDictionary<string,int> p)
    {
        if(family==PrimeFamily)
        {
            if(!ValidPrime(p))throw new InvalidOperationException("Invalid prime parameters.");
            return FactorableOverIntegers(p["a"],p["b"],p["c"])?"نعم":"لا";
        }
        if(!ValidFactor(p,family))throw new InvalidOperationException("Invalid nonmonic factor parameters.");
        if(family==FactorFamily)return Term(p["u"],p["v"])+Term(p["w"],p["z"]);
        if(family==RootsFamily)
        {
            var r1=Fraction(-p["v"],p["u"]),r2=Fraction(-p["z"],p["w"]);
            // Lexical order is display-only; grading accepts either order.
            return string.Join(",",new[]{r1,r2}.OrderBy(x=>x,StringComparer.Ordinal));
        }
        throw new InvalidOperationException("Unsupported nonmonic family.");
    }

    private static string? Normalize(string? answer)
    {
        if(string.IsNullOrWhiteSpace(answer)||answer.Length>128)return null;
        var builder=new StringBuilder();
        foreach(char input in answer.Trim())
        {
            char c=input switch
            {
                >= '٠' and <= '٩'=>(char)('0'+input-'٠'),
                >= '۰' and <= '۹'=>(char)('0'+input-'۰'),
                '−' or '–'=>'-','س' or 'X'=>'x','،'=>',',_=>input
            };
            if(char.IsWhiteSpace(c))continue;
            if(c is not ('+' or '-' or '(' or ')' or 'x' or ',' or '/')&&
                (c<'0'||c>'9')&&c is not ('ن' or 'ع' or 'م' or 'ل' or 'ا'))return null;
            builder.Append(c);
        }
        return builder.ToString();
    }

    private static bool ParseFraction(string text,out int num,out int den)
    {
        num=0;den=1;
        string[] parts=text.Split('/',StringSplitOptions.None);
        if(parts.Length is <1 or >2)return false;
        if(!int.TryParse(parts[0],NumberStyles.AllowLeadingSign,CultureInfo.InvariantCulture,out num))return false;
        if(parts.Length==2&&!int.TryParse(parts[1],NumberStyles.None,CultureInfo.InvariantCulture,out den))return false;
        return den is >=1 and <=16&&num is >=-100 and <=100;
    }

    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Supports(family))return false;
        string? input=Normalize(answer);if(input is null)return false;
        if(family==PrimeFamily)return ValidPrime(p)&&input==Format(family,p);
        if(!ValidFactor(p,family))return false;
        int u=p["u"],w=p["w"],v=p["v"],z=p["z"];
        if(family==RootsFamily)
        {
            var parts=input.Split(',',StringSplitOptions.None);
            if(parts.Length!=2||
               !ParseFraction(parts[0],out int n1,out int d1)||
               !ParseFraction(parts[1],out int n2,out int d2))return false;
            bool first=n1*u==-v*d1&&n2*w==-z*d2;
            bool second=n1*w==-z*d1&&n2*u==-v*d2;
            return first||second;
        }

        // Bounded positive-leading integer factors. Reconstruct the coefficients,
        // never trust a generated correct-answer string as the math oracle.
        const string term1=@"\((?<lead1>[1-9][0-9]*)?x(?<sign1>[+-])(?<constant1>[1-9][0-9]*)\)";
        const string term2=@"\((?<lead2>[1-9][0-9]*)?x(?<sign2>[+-])(?<constant2>[1-9][0-9]*)\)";
        var match=Regex.Match(input,"^"+term1+term2+"$",RegexOptions.CultureInvariant);
        if(!match.Success)return false;
        int firstLead=1,secondLead=1,firstConst,secondConst;
        if(match.Groups["lead1"].Success &&
            !int.TryParse(match.Groups["lead1"].Value,NumberStyles.None,
                CultureInfo.InvariantCulture,out firstLead))return false;
        if(match.Groups["lead2"].Success &&
            !int.TryParse(match.Groups["lead2"].Value,NumberStyles.None,
                CultureInfo.InvariantCulture,out secondLead))return false;
        if(!int.TryParse(match.Groups["constant1"].Value,NumberStyles.None,
                CultureInfo.InvariantCulture,out firstConst)||
           !int.TryParse(match.Groups["constant2"].Value,NumberStyles.None,
                CultureInfo.InvariantCulture,out secondConst))return false;
        if(match.Groups["sign1"].Value=="-")firstConst=-firstConst;
        if(match.Groups["sign2"].Value=="-")secondConst=-secondConst;
        int[] factors=[firstLead,secondLead],constantsSigned=[firstConst,secondConst];
        int a=u*w,b=u*z+w*v,c=v*z;
        return factors[0]*factors[1]==a &&
            factors[0]*constantsSigned[1]+factors[1]*constantsSigned[0]==b &&
            constantsSigned[0]*constantsSigned[1]==c;
    }
}

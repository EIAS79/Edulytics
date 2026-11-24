using System.Globalization;
using System.Text;
namespace Edulytics.Services.Mathematics;

/// <summary>
/// Grade 10 General U01L02: bounded first-party monomial times polynomial
/// exact questions. This is NOT polynomial times polynomial and is not a CAS.
/// Authored in Stage C; execution and registry activation belong to Stage D.
/// </summary>
public sealed partial class ExactSkillContractQuestionEngine
{
    private static ExactProblem BuildG10MonomialPolynomial(
        string family, Random random, ExactSkillQuestionDifficulty difficulty)
    {
        int limit=difficulty switch
        {
            ExactSkillQuestionDifficulty.Challenge => 7,
            ExactSkillQuestionDifficulty.Stretch => 6,
            _=>4
        };
        int m;
        do m=random.Next(-limit,limit+1); while(m==0);
        int k=random.Next(0,3);
        int a0,a1,a2;
        do
        {
            a0=random.Next(-limit,limit+1);
            a1=random.Next(-limit,limit+1);
            a2=random.Next(-limit,limit+1);
        }while(a0==0&&a1==0&&a2==0);
        int degree=random.Next(0,5);
        bool whole=family==UaeG10MonomialPolynomialResponse.WholeFamily;
        string unit=k switch {0=>" ",1=>"س",_=>"س²"};
        string prompt=whole
            ? $"اضرب الحد الأحادي ({m}{unit}) في كثيرة الحدود ({a2}س²+({a1})س+({a0})). وزّع الحد الأحادي على كل حد ثم اكتب الناتج بالصورة القياسية المبسطة."
            : $"اضرب الحد الأحادي ({m}{unit}) في ({a2}س²+({a1})س+({a0})). ما معامل س^{degree} في الناتج؟ الدرجة صفر تعني الحد الثابت. اكتب العدد فقط.";
        return Problem(family,prompt,
            "وزّع معامل الحد الأحادي على معاملات جميع الحدود، واجمع أسس س عند ضرب القوى المتفقة الأساس. تأكد من إشارات الحدود ومواضع الدرجات الصفرية.",
            whole ? AssessmentItemType.ShortAnswer:AssessmentItemType.Numeric,
            ("m",m),("k",k),("a0",a0),("a1",a1),("a2",a2),("degree",degree));
    }
}

internal static class UaeG10MonomialPolynomialResponse
{
    internal const string WholeFamily="uae.g10.general.u01.l02.full_monomial_polynomial_product";
    internal const string CoefficientFamily="uae.g10.general.u01.l02.monomial_polynomial_coefficient";
    internal static bool Supports(string? family) =>
        family is WholeFamily or CoefficientFamily;

    private static bool Valid(IReadOnlyDictionary<string,int> p)
    {
        string[] keys=["m","k","a0","a1","a2","degree"];
        return p.Count==6&&keys.All(p.ContainsKey)&&
            p["m"] is >= -7 and <= 7 and not 0&&
            p["k"] is >=0 and <=2&&
            p["degree"] is >=0 and <=4&&
            p["a0"] is >=-7 and <=7&&p["a1"] is >=-7 and <=7&&
            p["a2"] is >=-7 and <=7&&
            (p["a0"]!=0||p["a1"]!=0||p["a2"]!=0);
    }

    private static int[] Expected(IReadOnlyDictionary<string,int> p)
    {
        var result=new int[5];
        for(int d=0;d<=2;d++)
            result[d+p["k"]]=p["m"]*p["a"+d.ToString(CultureInfo.InvariantCulture)];
        return result;
    }

    internal static string Format(IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(p))throw new InvalidOperationException("Invalid G10 monomial parameters.");
        return Format(Expected(p));
    }

    internal static string NumericCoefficient(IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(p))throw new InvalidOperationException("Invalid G10 monomial parameters.");
        return Expected(p)[p["degree"]].ToString(CultureInfo.InvariantCulture);
    }

    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Supports(family)||!Valid(p)||string.IsNullOrWhiteSpace(answer))return false;
        if(family==CoefficientFamily)
        {
            string digits=NormalizeDigits(answer.Trim());
            return int.TryParse(digits,NumberStyles.AllowLeadingSign,
                CultureInfo.InvariantCulture,out var actual) &&
                actual==Expected(p)[p["degree"]];
        }
        return TryParse(answer,out var parsed)&&parsed.SequenceEqual(Expected(p));
    }

    private static string Format(IReadOnlyList<int> values)
    {
        var b=new StringBuilder();
        for(int d=4;d>=0;d--)
        {
            int c=values[d];if(c==0)continue;
            b.Append(b.Length>0?(c<0?"−":"+"):(c<0?"−":""));
            if(d==0||Math.Abs(c)!=1)b.Append(Math.Abs(c).ToString(CultureInfo.InvariantCulture));
            if(d>0)b.Append("س");
            if(d==2)b.Append("²");
            else if(d==3)b.Append("³");
            else if(d==4)b.Append("⁴");
        }
        return b.Length==0?"0":b.ToString();
    }

    private static string NormalizeDigits(string input)
    {
        var b=new StringBuilder(input.Length);
        foreach(char c in input)
            b.Append(c switch
            {
                >= '٠' and <= '٩'=>(char)('0'+c-'٠'),
                >= '۰' and <= '۹'=>(char)('0'+c-'۰'),
                '−' or '–'=>'-',
                _=>c
            });
        return b.ToString();
    }

    private static bool TryParse(string? answer,out int[] result)
    {
        result=new int[5];
        if(string.IsNullOrWhiteSpace(answer)||answer.Length>128)return false;
        var b=new StringBuilder();
        foreach(char c in NormalizeDigits(answer.Trim()))
        {
            if(char.IsWhiteSpace(c))continue;
            switch(c)
            {
                case 'س':case 'X':b.Append('x');break;
                case '²':b.Append("^2");break;
                case '³':b.Append("^3");break;
                case '⁴':b.Append("^4");break;
                case '+' :case '-' :case '^':case 'x':b.Append(c);break;
                default:
                    if(c>='0'&&c<='9')b.Append(c);
                    else return false;
                    break;
            }
        }
        string expr=b.ToString();
        if(expr.Length==0)return false;
        if(expr[0] is not ('+' or '-'))expr="+"+expr;
        int pos=0,count=0,previousDegree=5;
        while(pos<expr.Length)
        {
            char sign=expr[pos];if(sign is not ('+' or '-'))return false;
            int start=++pos;
            while(pos<expr.Length&&expr[pos] is not ('+' or '-'))pos++;
            if(start==pos||++count>5)return false;
            string term=expr[start..pos];
            int loc=term.IndexOf('x'),degree=0,coefficient;
            if(loc<0)
            {
                if(!int.TryParse(term,NumberStyles.None,
                    CultureInfo.InvariantCulture,out coefficient))return false;
            }
            else
            {
                string suffix=term[(loc+1)..];
                if(suffix.Length==0||suffix=="^1")degree=1;
                else if(suffix is "^2" or "^3" or "^4")degree=suffix[1]-'0';
                else return false;
                string prefix=term[..loc];
                if(prefix.Length==0)coefficient=1;
                else if(!int.TryParse(prefix,NumberStyles.None,
                    CultureInfo.InvariantCulture,out coefficient))return false;
            }
            if(coefficient>1000||degree>=previousDegree||coefficient==0&&(count!=1||pos!=expr.Length||degree!=0))
                return false;
            result[degree]=sign=='-'?-coefficient:coefficient;
            previousDegree=degree;
        }
        return count>0;
    }
}

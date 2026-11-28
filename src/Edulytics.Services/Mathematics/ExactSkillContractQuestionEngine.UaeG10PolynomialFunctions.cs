using System.Globalization;
using System.Text;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// UAE G10 approved General U01L11, original Advanced 2025–2026
/// Unit3 Lesson3-3 PDF213–220. Stage C bounded polynomial
/// degree, integer evaluation and parity/leading-sign end behavior.
/// Tests are WRITTEN_NOT_RUN until Stage D.
/// </summary>
public sealed partial class ExactSkillContractQuestionEngine
{
    private static ExactProblem BuildG10PolynomialFunction(
        string family,Random random,ExactSkillQuestionDifficulty difficulty)
    {
        int limit=difficulty==ExactSkillQuestionDifficulty.Challenge?5:3;
        int degree=random.Next(1,6);
        int[] coeffs=new int[6];
        for(int d=0;d<degree;d++)coeffs[d]=random.Next(-limit,limit+1);
        do coeffs[degree]=random.Next(-limit,limit+1);while(coeffs[degree]==0);
        int x=random.Next(-3,4);
        string f=UaeG10PolynomialFunctionResponse.FormatPolynomial(coeffs);
        string prompt=family switch
        {
            UaeG10PolynomialFunctionResponse.EvaluateFamily=>
                $"لتكن الدالة كثيرة الحدود د(س)={f}. احسب قيمة د({x}) عددًا صحيحًا، مع مراعاة إشارات القوى.",
            UaeG10PolynomialFunctionResponse.DegreeFamily=>
                $"لتكن الدالة كثيرة الحدود د(س)={f}. ما درجة الدالة؟ اكتب العدد الصحيح فقط.",
            _=>$"لتكن الدالة كثيرة الحدود د(س)={f}. حدد السلوك الطرفي عندما س→−∞ ثم س→+∞ بالترتيب: اكتب كلمتين من (أعلى،أسفل) مفصولتين بفاصلة."
        };
        return Problem(family,prompt,
            "رتب حدود كثيرة الحدود، حدد أكبر أس لمعامل غير صفري. في التعويض اجمع قيم الحدود بدقة. في السلوك الطرفي يعتمد الاتجاه فقط على زوجية الدرجة وإشارة المعامل الرئيس.",
            family==UaeG10PolynomialFunctionResponse.EndFamily?
                AssessmentItemType.ShortAnswer:AssessmentItemType.Numeric,
            ("a0",coeffs[0]),("a1",coeffs[1]),("a2",coeffs[2]),
            ("a3",coeffs[3]),("a4",coeffs[4]),("a5",coeffs[5]),
            ("degree",degree),("x",x));
    }
}

internal static class UaeG10PolynomialFunctionResponse
{
    internal const string EvaluateFamily="uae.g10.general.u01.l11.polynomial_evaluate_integer";
    internal const string DegreeFamily="uae.g10.general.u01.l11.polynomial_degree_integer";
    internal const string EndFamily="uae.g10.general.u01.l11.polynomial_end_behavior";
    internal static bool Supports(string? family)=>
        family is EvaluateFamily or DegreeFamily or EndFamily;

    private static bool Valid(IReadOnlyDictionary<string,int> p)
    {
        string[] keys=["a0","a1","a2","a3","a4","a5","degree","x"];
        if(p.Count!=keys.Length||keys.Any(k=>!p.ContainsKey(k))||
           p["degree"] is <1 or >5||p["x"] is <-3 or >3)return false;
        for(int d=0;d<6;d++)
        {
            int a=p["a"+d.ToString(CultureInfo.InvariantCulture)];
            if(a is <-5 or >5) return false;
            if(d>p["degree"]&&a!=0)return false;
        }
        return p["a"+p["degree"].ToString(CultureInfo.InvariantCulture)]!=0;
    }

    internal static string FormatPolynomial(IReadOnlyList<int> coefficients)
    {
        if(coefficients.Count!=6)throw new InvalidOperationException("Expected 6 polynomial coefficients.");
        var b=new StringBuilder();
        for(int d=5;d>=0;d--)
        {
            int c=coefficients[d];if(c==0)continue;
            b.Append(b.Length>0?(c<0?"−":"+"):(c<0?"−":""));
            if(d==0||Math.Abs(c)!=1)b.Append(Math.Abs(c).ToString(CultureInfo.InvariantCulture));
            if(d>0)b.Append("س");
            b.Append(d switch {2=>"²",3=>"³",4=>"⁴",5=>"⁵",_=>""});
        }
        return b.Length==0?"0":b.ToString();
    }

    private static int Eval(IReadOnlyDictionary<string,int> p)
    {
        int sum=0;
        for(int d=5;d>=0;d--)
            sum=checked(sum*p["x"]+p["a"+d.ToString(CultureInfo.InvariantCulture)]);
        return sum;
    }

    private static string End(IReadOnlyDictionary<string,int> p)
    {
        bool positive=p["a"+p["degree"].ToString(CultureInfo.InvariantCulture)]>0;
        bool even=p["degree"]%2==0;
        return even?(positive?"أعلى,أعلى":"أسفل,أسفل"):
            (positive?"أسفل,أعلى":"أعلى,أسفل");
    }

    internal static string Format(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Supports(family)||!Valid(p))throw new InvalidOperationException("Invalid G10 polynomial function parameters.");
        return family switch
        {
            EvaluateFamily=>Eval(p).ToString(CultureInfo.InvariantCulture),
            DegreeFamily=>p["degree"].ToString(CultureInfo.InvariantCulture),
            _=>End(p)
        };
    }

    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Supports(family)||!Valid(p)||string.IsNullOrWhiteSpace(answer))return false;
        string normalized=string.Concat(answer.Trim().Select(c=>c switch
        {
            >= '٠' and <= '٩'=>(char)('0'+c-'٠'),
            >= '۰' and <= '۹'=>(char)('0'+c-'۰'),
            '−' or '–'=>'-',
            '،'=>',',
            _=>c
        }));
        if(family==EndFamily)
        {
            string compact=string.Concat(normalized.Where(c=>!char.IsWhiteSpace(c)));
            return compact==End(p);
        }
        return int.TryParse(normalized,NumberStyles.AllowLeadingSign,
            CultureInfo.InvariantCulture,out int x)&&
            x==(family==DegreeFamily?p["degree"]:Eval(p));
    }
}

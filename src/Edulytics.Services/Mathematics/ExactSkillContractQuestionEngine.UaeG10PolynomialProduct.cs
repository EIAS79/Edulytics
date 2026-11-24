using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// UAE Grade10 General U01L03: bounded full product of two non-monomial polynomials.
/// A mathematical convolution oracle is independent of the generated answer string.
/// Stage C authored; production routing and runtime tests remain in Stage D.
/// </summary>
public sealed partial class ExactSkillContractQuestionEngine
{
    private static ExactProblem BuildG10PolynomialProduct(
        string family,Random random,ExactSkillQuestionDifficulty difficulty)
    {
        int bnd=difficulty switch
        {
            ExactSkillQuestionDifficulty.Challenge=>7,
            ExactSkillQuestionDifficulty.Stretch=>6,
            _=>4
        };
        int[] a=new int[3],b=new int[3];
        do
        {
            for(int i=0;i<3;i++)
            {
                a[i]=random.Next(-bnd,bnd+1);
                b[i]=random.Next(-bnd,bnd+1);
            }
        }while(a.Count(x=>x!=0)<2||b.Count(x=>x!=0)<2);
        int degree=random.Next(0,5);
        bool whole=family==UaeG10PolynomialProductResponse.WholeFamily;
        string prompt=whole
            ? $"اضرب كثيرتي الحدود ({a[2]}س²+({a[1]})س+({a[0]})) × ({b[2]}س²+({b[1]})س+({b[0]})). وزّع جميع الحدود ثم اجمع الحدود المتشابهة واكتب الناتج بالصورة القياسية."
            : $"اضرب كثيرتي الحدود ({a[2]}س²+({a[1]})س+({a[0]})) × ({b[2]}س²+({b[1]})س+({b[0]})). ما معامل س^{degree} في حاصل الضرب؟ الدرجة صفر تعني الثابت. اكتب عددًا صحيحًا فقط.";
        return Problem(family,prompt,
            "اضرب كل حد من كثيرة الحدود الأولى في كل حد من الثانية، واجمع أسس المتغير المتشابه في الضرب، ثم اجمع معاملات الحدود ذات الدرجات المتساوية.",
            whole?AssessmentItemType.ShortAnswer:AssessmentItemType.Numeric,
            ("a2",a[2]),("a1",a[1]),("a0",a[0]),
            ("b2",b[2]),("b1",b[1]),("b0",b[0]),("degree",degree));
    }
}

internal static class UaeG10PolynomialProductResponse
{
    internal const string WholeFamily="uae.g10.general.u01.l03.full_polynomial_product";
    internal const string CoefficientFamily="uae.g10.general.u01.l03.product_coefficient";
    internal static bool Supports(string? family) => family is WholeFamily or CoefficientFamily;

    private static bool Valid(IReadOnlyDictionary<string,int> p)
    {
        string[] keys=["a2","a1","a0","b2","b1","b0","degree"];
        return p.Count==keys.Length&&keys.All(p.ContainsKey)&&
            keys.Take(6).All(x=>p[x] is >=-7 and <=7)&&
            p["degree"] is >=0 and <=4 &&
            Enumerable.Range(0,3).Count(i=>p["a"+i]!=0)>=2 &&
            Enumerable.Range(0,3).Count(i=>p["b"+i]!=0)>=2;
    }

    private static int[] Expected(IReadOnlyDictionary<string,int> p)
    {
        var c=new int[5];
        for(int a=0;a<=2;a++)
        for(int b=0;b<=2;b++)
            c[a+b]+=p["a"+a.ToString(CultureInfo.InvariantCulture)]*
                    p["b"+b.ToString(CultureInfo.InvariantCulture)];
        return c;
    }

    internal static string Format(IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(p))throw new InvalidOperationException("Invalid G10 polynomial product parameters.");
        return UaeG10MonomialPolynomialResponse.Format(Expected(p));
    }

    internal static string NumericCoefficient(IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(p))throw new InvalidOperationException("Invalid G10 polynomial product parameters.");
        return Expected(p)[p["degree"]].ToString(CultureInfo.InvariantCulture);
    }

    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Supports(family)||!Valid(p)||string.IsNullOrWhiteSpace(answer))return false;
        if(family==CoefficientFamily)
        {
            var raw=string.Concat(answer.Trim().Select(c=>c switch
            {
                >= '٠' and <= '٩'=>(char)('0'+c-'٠'),
                >= '۰' and <= '۹'=>(char)('0'+c-'۰'),
                '−' or '–'=>'-',
                _=>c
            }));
            return int.TryParse(raw,NumberStyles.AllowLeadingSign,
                CultureInfo.InvariantCulture,out int actual)&&actual==Expected(p)[p["degree"]];
        }
        return UaeG10MonomialPolynomialResponse.TryParse(answer,out var result)&&
            result.SequenceEqual(Expected(p));
    }
}

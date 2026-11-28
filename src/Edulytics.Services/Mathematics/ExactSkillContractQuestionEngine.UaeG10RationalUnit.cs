using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

// Original Arabic bounded digital subskills, G10 U06 L03-L07.
// Source pages 28-78 in the 2018-2019 Advanced volume 3; scoped coverage,
// not exhaustive source or production certification.
public sealed partial class ExactSkillContractQuestionEngine
{
    private static ExactProblem BuildG10RationalUnit(string family, Random random, ExactSkillQuestionDifficulty difficulty)
    {
        int upper=difficulty==ExactSkillQuestionDifficulty.Challenge?13:10;
        int a=random.Next(2,upper), b=random.Next(2,upper), c=random.Next(2,upper);
        int d=random.Next(2,upper);
        while(d==b)d=random.Next(2,upper);
        int e=random.Next(1,upper), f=random.Next(2,upper);
        string prompt=family switch
        {
            UaeG10RationalUnitResponse.ProductConstant=>$"إذا كان x ≠ {b}، بسّط ({a}/(x−{b})) × ((x−{b})/{c}). اكتب الكسر الناتج.",
            UaeG10RationalUnitResponse.DivisionConstant=>$"إذا كان x ≠ {b}، بسّط ({a}/(x−{b})) ÷ ({c}/(x−{b})). اكتب الكسر الناتج.",
            UaeG10RationalUnitResponse.Excluded=>$"حدد قيمة x الممنوعة من المجال الأصلي للتعبير ((x−{b})(x+{c}))/(x−{b})، حتى بعد الاختصار. اكتب عددًا فقط.",
            UaeG10RationalUnitResponse.ComplexFraction=>$"بسّط الكسر المركب ({a}/{b}) ÷ ({c}/{d}) إلى كسر مكافئ.",
            UaeG10RationalUnitResponse.SumNumerator=>$"عند جمع {a}/x + {b}/x، حيث x ≠ 0، ما بسط الناتج عند كتابته بمقام x؟",
            UaeG10RationalUnitResponse.DifferenceNumerator=>$"عند طرح {a}/x − {b}/x، حيث x ≠ 0، ما بسط الناتج عند كتابته بمقام x؟",
            UaeG10RationalUnitResponse.UnlikeCoefficient=>$"اجمع {a}/(x−{b}) + {c}/(x−{d}) بالمقام المشترك (x−{b})(x−{d}). ما معامل x في البسط بعد التوزيع؟",
            UaeG10RationalUnitResponse.UnlikeConstant=>$"اجمع {a}/(x−{b}) + {c}/(x−{d}) بالمقام المشترك (x−{b})(x−{d}). ما الحد الثابت في البسط بعد التوزيع؟",
            UaeG10RationalUnitResponse.ComplexSum=>$"بسّط الكسر المركب ({a}/{b}+{c}/{d}) ÷ ({e}/{f}). اكتب عددًا أو كسرًا.",
            UaeG10RationalUnitResponse.Vertical=>$"للدالة y = {a}/(x−{b}) + {c}، اكتب قيمة x لخط التقارب الرأسي.",
            UaeG10RationalUnitResponse.Horizontal=>$"للدالة y = {a}/(x−{b}) + {c}، اكتب قيمة y لخط التقارب الأفقي.",
            UaeG10RationalUnitResponse.GraphValue=>$"للدالة y = {a}/(x−{b}) + {c}، أوجد قيمة y عندما x = {b+e}. اكتب عددًا أو كسرًا.",
            UaeG10RationalUnitResponse.HoleX=>$"للدالة y = ((x−{b})(x+{c}))/(x−{b})، ما الإحداثي x للثقب القابل للإزالة؟",
            UaeG10RationalUnitResponse.HoleY=>$"للدالة y = ((x−{b})(x+{c}))/(x−{b})، ما الإحداثي y للثقب القابل للإزالة؟",
            UaeG10RationalUnitResponse.Oblique=>$"للدالة y = {a}x + {c} + {e}/(x−{b})، ما المقطع الصادي لخط التقارب المائل؟",
            UaeG10RationalUnitResponse.Direct=>$"يتغير y طرديًا مع x. عندما x = {b} تكون y = {a*b}. أوجد y عندما x = {d}.",
            UaeG10RationalUnitResponse.Inverse=>$"يتغير y عكسيًا مع x. عندما x = {b} تكون y = {a}. أوجد y عندما x = {d}. اكتب عددًا أو كسرًا.",
            UaeG10RationalUnitResponse.Joint=>$"يتغير y طرديًا مع حاصل ضرب x وz، وثابت التغير {a}. احسب y عند x = {b} وz = {c}.",
            UaeG10RationalUnitResponse.Combined=>$"يتغير y طرديًا مع x وعكسيًا مع z، وثابت التغير {a}. احسب y عند x = {b} وz = {c}.",
            UaeG10RationalUnitResponse.Equation=>$"حل المعادلة {a}/(x−{b}) = {c}/{d} مع مراعاة x ≠ {b}. اكتب قيمة x كعدد أو كسر.",
            UaeG10RationalUnitResponse.Extraneous=>$"كم حلًا حقيقيًا للمعادلة ((x−{b})(x+{c}))/(x−{b}) = {b+c} بعد استبعاد القيمة الممنوعة؟ اكتب عدد الحلول.",
            UaeG10RationalUnitResponse.Inequality=>$"هل تحقق x = {b+e} المتباينة {a}/(x−{b}) > {c}/{d}؟ اكتب 1 لنعم و0 للا.",
            UaeG10RationalUnitResponse.WorkRate=>$"تنجز آلة عملًا في {b} ساعات وأخرى العمل نفسه في {d} ساعات. إذا عملتا معًا بمعدل ثابت، فكم ساعة يحتاج العمل؟ اكتب عددًا أو كسرًا.",
            _=>throw new InvalidOperationException("Unsupported rational unit family.")
        };
        return Problem(family,prompt,"حدد القيم الممنوعة قبل إجراء العمليات؛ استعمل المقام المشترك أو المقلوب عند الحاجة، وتحقق من الحل في التعبير الأصلي. في التغير عيّن العلاقة والثابت أولًا.",
            UaeG10RationalUnitResponse.IsFraction(family)?AssessmentItemType.ShortAnswer:AssessmentItemType.Numeric,
            ("a",a),("b",b),("c",c),("d",d),("e",e),("f",f));
    }
}

internal static class UaeG10RationalUnitResponse
{
    internal const string ProductConstant="uae.g10.general.u06.l03.product_cancel_constant";
    internal const string DivisionConstant="uae.g10.general.u06.l03.division_cancel_constant";
    internal const string Excluded="uae.g10.general.u06.l03.cancellation_original_exclusion";
    internal const string ComplexFraction="uae.g10.general.u06.l03.complex_fraction_quotient";
    internal const string SumNumerator="uae.g10.general.u06.l04.like_denominator_sum_numerator";
    internal const string DifferenceNumerator="uae.g10.general.u06.l04.like_denominator_difference_numerator";
    internal const string UnlikeCoefficient="uae.g10.general.u06.l04.unlike_denominator_linear_coefficient";
    internal const string UnlikeConstant="uae.g10.general.u06.l04.unlike_denominator_constant";
    internal const string ComplexSum="uae.g10.general.u06.l04.complex_fraction_sum";
    internal const string Vertical="uae.g10.general.u06.l05.reciprocal_vertical_asymptote";
    internal const string Horizontal="uae.g10.general.u06.l05.reciprocal_horizontal_asymptote";
    internal const string GraphValue="uae.g10.general.u06.l05.reciprocal_graph_value";
    internal const string HoleX="uae.g10.general.u06.l05.removable_hole_x";
    internal const string HoleY="uae.g10.general.u06.l05.removable_hole_y";
    internal const string Oblique="uae.g10.general.u06.l05.oblique_asymptote_intercept";
    internal const string Direct="uae.g10.general.u06.l06.direct_variation_value";
    internal const string Inverse="uae.g10.general.u06.l06.inverse_variation_value";
    internal const string Joint="uae.g10.general.u06.l06.joint_variation_value";
    internal const string Combined="uae.g10.general.u06.l06.combined_variation_value";
    internal const string Equation="uae.g10.general.u06.l07.rational_equation_exact_root";
    internal const string Extraneous="uae.g10.general.u06.l07.rational_equation_extraneous_count";
    internal const string Inequality="uae.g10.general.u06.l07.rational_inequality_membership";
    internal const string WorkRate="uae.g10.general.u06.l07.rational_combined_work_time";
    internal static bool Supports(string? family)=>family is ProductConstant or DivisionConstant or Excluded or ComplexFraction or SumNumerator or DifferenceNumerator or UnlikeCoefficient or UnlikeConstant or ComplexSum or Vertical or Horizontal or GraphValue or HoleX or HoleY or Oblique or Direct or Inverse or Joint or Combined or Equation or Extraneous or Inequality or WorkRate;
    internal static bool IsFraction(string family)=>family is ProductConstant or DivisionConstant or ComplexFraction or ComplexSum or GraphValue or Inverse or Combined or Equation or WorkRate;
    private static bool Valid(string family,IReadOnlyDictionary<string,int> p)=>
        Supports(family) && p.Count==6 && new[]{"a","b","c","d","e","f"}.All(p.ContainsKey) &&
        p["a"] is >=2 and <=12 && p["b"] is >=2 and <=12 && p["c"] is >=2 and <=12 &&
        p["d"] is >=2 and <=12 && p["d"]!=p["b"] && p["e"] is >=1 and <=12 && p["f"] is >=2 and <=12;

    private static long Gcd(long a,long b)
    {
        a=Math.Abs(a); b=Math.Abs(b);
        while(b!=0){long r=a%b;a=b;b=r;}
        return a;
    }

    internal static string Format(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid bounded rational unit parameters.");
        long a=p["a"],b=p["b"],c=p["c"],d=p["d"],e=p["e"],f=p["f"];
        (long numerator,long denominator)=family switch
        {
            ProductConstant=>(a,c),
            DivisionConstant=>(a,c),
            Excluded=>(b,1),
            ComplexFraction=>(a*d,b*c),
            SumNumerator=>(a+b,1),
            DifferenceNumerator=>(a-b,1),
            UnlikeCoefficient=>(a+c,1),
            UnlikeConstant=>(-a*d-c*b,1),
            ComplexSum=>((a*d+c*b)*f,b*d*e),
            Vertical=>(b,1),
            Horizontal=>(c,1),
            GraphValue=>(a+c*e,e),
            HoleX=>(b,1),
            HoleY=>(b+c,1),
            Oblique=>(c,1),
            Direct=>(a*d,1),
            Inverse=>(a*b,d),
            Joint=>(a*b*c,1),
            Combined=>(a*b,c),
            Equation=>(b*c+a*d,c),
            Extraneous=>(0,1),
            Inequality=>(a*d>c*e?1:0,1),
            WorkRate=>(b*d,b+d),
            _=>throw new InvalidOperationException()
        };
        long g=Gcd(numerator,denominator); numerator/=g; denominator/=g;
        return denominator==1?numerator.ToString(CultureInfo.InvariantCulture):
            numerator.ToString(CultureInfo.InvariantCulture)+"/"+denominator.ToString(CultureInfo.InvariantCulture);
    }

    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Valid(family,p)||string.IsNullOrWhiteSpace(answer)||answer.Length>48)return false;
        string raw=string.Concat(answer.Trim().Select(ch=>ch switch
        {
            >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
            >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
            '−' or '–'=>'-', '⁄' or '∕'=>'/', _=>ch
        }));
        var parts=raw.Split('/');
        if(parts.Length is <1 or >2 || (!IsFraction(family)&&parts.Length!=1))return false;
        if(!long.TryParse(parts[0],NumberStyles.AllowLeadingSign,CultureInfo.InvariantCulture,out long n))return false;
        long q=1;
        if(parts.Length==2&&!long.TryParse(parts[1],NumberStyles.AllowLeadingSign,CultureInfo.InvariantCulture,out q))return false;
        if(n is < -1000000 or >1000000 || q is <=0 or >1000000)return false;
        long a=p["a"],b=p["b"],c=p["c"],d=p["d"],e=p["e"],f=p["f"];
        // Cross-multiplied identities and substitution instead of comparing
        // text with the generator's reduced answer.
        return family switch
        {
            ProductConstant=>n*c==q*a,
            DivisionConstant=>n*c==q*a,
            Excluded=>n==q*b,
            ComplexFraction=>n*b*c==q*a*d,
            SumNumerator=>n==q*a+q*b,
            DifferenceNumerator=>n+q*b==q*a,
            UnlikeCoefficient=>n==q*a+q*c,
            UnlikeConstant=>n+q*a*d+q*c*b==0,
            ComplexSum=>n*b*d*e==q*f*a*d+q*f*c*b,
            Vertical=>n==q*b,
            Horizontal=>n==q*c,
            GraphValue=>(n-q*c)*e==q*a,
            HoleX=>n==q*b,
            HoleY=>n-q*c==q*b,
            Oblique=>n==q*c,
            Direct=>n*b==q*a*b*d,
            Inverse=>n*d==q*a*b,
            Joint=>n==q*a*b*c,
            Combined=>n*c==q*a*b,
            Equation=>(n-q*b)!=0 && (n-q*b)*c==q*a*d,
            Extraneous=>n==0,
            Inequality=>n==q*(a*d>c*e?1:0),
            WorkRate=>n*(b+d)==q*b*d,
            _=>false
        };
    }
}

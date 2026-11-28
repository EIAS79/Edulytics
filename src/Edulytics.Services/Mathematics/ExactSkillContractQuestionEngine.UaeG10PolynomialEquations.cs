using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Grade10 General approved U01L13; original Advanced Arabic 3-5 PDF233–242.
/// Factoring/zero-product real integer roots of cubic and biquadratic quartic
/// equations. Approximate higher-degree noninteger roots and calculator
/// procedures stay teacher reviewed. Stage C authored; tests NOT RUN.
/// </summary>
public sealed partial class ExactSkillContractQuestionEngine
{
    private static ExactProblem BuildG10PolynomialEquation(
        string family,Random random,ExactSkillQuestionDifficulty difficulty)
    {
        int a;
        do a=random.Next(-3,4);while(a==0);
        if(family==UaeG10PolynomialEquationResponse.CubicFamily)
        {
            var roots=Enumerable.Range(-4,9).OrderBy(_=>random.Next())
                .Take(3).OrderBy(x=>x).ToArray();
            int[] c=UaeG10PolynomialEquationResponse.CubicCoefficients(
                a,roots[0],roots[1],roots[2]);
            string expression=UaeG10PolynomialFunctionResponse.FormatPolynomial(c);
            return Problem(family,
                $"حل المعادلة كثيرة الحدود {expression}=٠ باستخدام التحليل إلى عوامل وخاصية الضرب الصفري. اكتب الجذور الحقيقية الثلاثة مرتبة من الأصغر إلى الأكبر، مفصولة بفواصل.",
                "أعد كتابة كثيرة الحدود كحاصل ضرب ثلاثة عوامل خطية، وساوِ كل عامل بالصفر، ثم تحقق بالتعويض في المعادلة الأصلية.",
                AssessmentItemType.ShortAnswer,
                ("a",a),("r1",roots[0]),("r2",roots[1]),
                ("r3",roots[2]),("u",0),("v",0),("mode",1));
        }
        int u=random.Next(1,5),v;
        do v=random.Next(1,5);while(v==u);
        if(u>v)(u,v)=(v,u);
        int[] b=UaeG10PolynomialEquationResponse.BiquadraticCoefficients(a,u,v);
        string poly=UaeG10PolynomialFunctionResponse.FormatPolynomial(b);
        return Problem(family,
            $"حل المعادلة {poly}=٠ بالتعويض ص=س² ثم التحليل إلى عوامل. اكتب جميع الجذور الحقيقية الأربعة تصاعديًا مع الفواصل.",
            "عوّض ص=س² فتصبح المعادلة تربيعية في ص. حلّل ثم استخرج كلا الجذرين الموجب والسالب لكل قيمة موجبة لس².",
            AssessmentItemType.ShortAnswer,
            ("a",a),("r1",0),("r2",0),("r3",0),
            ("u",u),("v",v),("mode",2));
    }
}

internal static class UaeG10PolynomialEquationResponse
{
    internal const string CubicFamily="uae.g10.general.u01.l13.cubic_integer_roots";
    internal const string BiquadraticFamily="uae.g10.general.u01.l13.biquadratic_integer_roots";
    internal static bool Supports(string? family)=>
        family is CubicFamily or BiquadraticFamily;

    internal static int[] CubicCoefficients(int a,int r1,int r2,int r3)=>
        [-(a*r1*r2*r3),a*(r1*r2+r1*r3+r2*r3),-a*(r1+r2+r3),a,0,0];

    internal static int[] BiquadraticCoefficients(int a,int u,int v)=>
        [a*u*u*v*v,0,-a*(u*u+v*v),0,a,0];

    private static bool Valid(string family,IReadOnlyDictionary<string,int> p)
    {
        string[] keys=["a","r1","r2","r3","u","v","mode"];
        if(!Supports(family)||p.Count!=7||keys.Any(k=>!p.ContainsKey(k))||
            p["a"] is 0 or <-3 or >3)return false;
        if(family==CubicFamily)
            return p["mode"]==1&&p["u"]==0&&p["v"]==0&&
                p["r1"] is >=-4 and <=4&&p["r2"] is >=-4 and <=4&&
                p["r3"] is >=-4 and <=4&&
                p["r1"]<p["r2"]&&p["r2"]<p["r3"];
        return p["mode"]==2&&p["r1"]==0&&p["r2"]==0&&p["r3"]==0&&
            p["u"] is >=1 and <=4&&p["v"] is >=1 and <=4&&p["u"]<p["v"];
    }

    private static int[] ExactRoots(string family,IReadOnlyDictionary<string,int> p)
    {
        var c=family==CubicFamily
            ?CubicCoefficients(p["a"],p["r1"],p["r2"],p["r3"])
            :BiquadraticCoefficients(p["a"],p["u"],p["v"]);
        int[] roots=Enumerable.Range(-4,9).Where(x=>
        {
            int value=0;
            for(int degree=5;degree>=0;degree--)
                value=checked(value*x+c[degree]);
            return value==0;
        }).ToArray();
        int expectedCount=family==CubicFamily?3:4;
        if(roots.Length!=expectedCount)
            throw new InvalidOperationException("Invalid polynomial equation parameter root count.");
        return roots;
    }

    internal static string Format(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid polynomial equation parameters.");
        return string.Join(",",ExactRoots(family,p));
    }

    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? response)
    {
        if(!Valid(family,p)||string.IsNullOrWhiteSpace(response))return false;
        var normal=string.Concat(response.Trim().Select(ch=>ch switch
        {
            >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
            >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
            '−' or '–'=>'-',
            '،' or ';'=>',',
            _=>ch
        }));
        if(normal.StartsWith('(')&&normal.EndsWith(')'))
            normal=normal[1..^1];
        var split=normal.Split(',',StringSplitOptions.None);
        int count=family==CubicFamily?3:4;
        if(split.Length!=count)return false;
        var values=new int[count];
        for(int i=0;i<count;i++)
        {
            if(!int.TryParse(split[i].Trim(),NumberStyles.AllowLeadingSign,
                CultureInfo.InvariantCulture,out values[i]))return false;
            if(i>0&&values[i]<=values[i-1])return false;
        }
        return values.SequenceEqual(ExactRoots(family,p));
    }
}

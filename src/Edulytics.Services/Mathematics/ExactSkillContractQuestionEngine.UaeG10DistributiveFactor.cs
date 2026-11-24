using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// UAE Grade 10 General U01L05: inverse distributive property and the
/// zero-product rule. Bounded first-party family in existing exact engine.
/// Grouping diagrams and written reasoning remain teacher-reviewed.
/// Stage C tests are authored, NOT RUN; production routing stays false.
/// </summary>
public sealed partial class ExactSkillContractQuestionEngine
{
    private static ExactProblem BuildG10DistributiveFactor(
        string family,Random random,ExactSkillQuestionDifficulty difficulty)
    {
        int g=random.Next(2,8),a=random.Next(1,6);
        int b;
        do b=random.Next(-5,6);
        while(b==0||UaeG10DistributiveFactorResponse.Gcd(a,Math.Abs(b))!=1);
        int k=random.Next(0,3);
        if(family==UaeG10DistributiveFactorResponse.CommonFamily)
        {
            int high=g*a,low=g*b;
            string head=UaeG10DistributiveFactorResponse.Term(high,k+1);
            string tail=UaeG10DistributiveFactorResponse.Term(Math.Abs(low),k);
            string expression=head+(low<0?"−":"+")+tail;
            return Problem(family,
                $"حلّل {expression} بإخراج العامل المشترك الأكبر، واكتب الناتج على الصورة القياسية: عامل مشترك × (ثنائية حدود يبدأ حد س فيها أولاً).",
                "ابحث عن القاسم المشترك الأكبر للمعاملات، وأقل أس للمتغير س؛ أخرجه عاملاً خارج القوس، وتحقق بالتوزيع.",
                AssessmentItemType.ShortAnswer,("g",g),("a",a),("b",b),("k",k));
        }

        int r;do r=random.Next(-5,6);while(r==0);
        int constant=-a*r;
        string inside=$"{UaeG10DistributiveFactorResponse.Term(a,1)}"+
            (constant<0?"−":"+")+Math.Abs(constant).ToString(CultureInfo.InvariantCulture);
        return Problem(family,
            $"حل المعادلة {g}س({inside})=٠ بخاصية الضرب الصفري. اكتب الجذرين الصحيحين مفصولين بفاصلة، بما في ذلك الجذر صفر.",
            "إذا كان حاصل ضرب عاملين يساوي صفرًا، فأحدهما يساوي صفرًا. حلّ س=٠ أو الحد الموجود بين القوسين يساوي صفرًا، ثم تحقق.",
            AssessmentItemType.ShortAnswer,("g",g),("a",a),("r",r));
    }
}

internal static class UaeG10DistributiveFactorResponse
{
    internal const string CommonFamily="uae.g10.general.u01.l05.gcf_complete_factor";
    internal const string RootsFamily="uae.g10.general.u01.l05.zero_product_roots";
    internal static bool Supports(string? family)=>family is CommonFamily or RootsFamily;
    internal static int Gcd(int a,int b)
    {
        while(b!=0){int t=a%b;a=b;b=t;}
        return a;
    }
    internal static string Term(int coef,int degree)
    {
        string c=(degree>0&&coef==1)?"":coef.ToString(CultureInfo.InvariantCulture);
        return degree switch
        {
            0=>c,1=>c+"س",2=>c+"س²",3=>c+"س³",
            _=>throw new ArgumentOutOfRangeException(nameof(degree))
        };
    }
    private static bool ValidCommon(IReadOnlyDictionary<string,int> p)=>
        p.Count==4&&new[]{"g","a","b","k"}.All(p.ContainsKey)&&
        p["g"] is >=2 and <=7&&p["a"] is >=1 and <=5&&
        p["b"] is >= -5 and <=5 and not 0&&p["k"] is >=0 and <=2&&
        Gcd(p["a"],Math.Abs(p["b"]))==1;
    private static bool ValidRoots(IReadOnlyDictionary<string,int> p)=>
        p.Count==3&&new[]{"g","a","r"}.All(p.ContainsKey)&&
        p["g"] is >=2 and <=7&&p["a"] is >=1 and <=5&&
        p["r"] is >= -5 and <=5 and not 0;
    internal static string Format(string family,IReadOnlyDictionary<string,int> p)
    {
        if(family==CommonFamily)
        {
            if(!ValidCommon(p))throw new InvalidOperationException("Invalid GCF parameters.");
            int g=p["g"],a=p["a"],b=p["b"],k=p["k"];
            return Term(g,k)+"("+Term(a,1)+(b<0?"−":"+")+
                Math.Abs(b).ToString(CultureInfo.InvariantCulture)+")";
        }
        if(family==RootsFamily)
        {
            if(!ValidRoots(p))throw new InvalidOperationException("Invalid zero-product parameters.");
            int r=p["r"];
            return r<0?$"{r.ToString(CultureInfo.InvariantCulture)},0":
                $"0,{r.ToString(CultureInfo.InvariantCulture)}";
        }
        throw new InvalidOperationException("Unsupported Grade10 factor family.");
    }
    private static string? Normalize(string? raw)
    {
        if(string.IsNullOrWhiteSpace(raw)||raw.Length>128)return null;
        var b=new StringBuilder();
        foreach(char input in raw.Trim())
        {
            char c=input switch
            {
                >= '٠' and <= '٩'=>(char)('0'+input-'٠'),
                >= '۰' and <= '۹'=>(char)('0'+input-'۰'),
                '−' or '–'=>'-',
                'س' or 'X'=>'x',
                '،'=>',',
                _=>input
            };
            if(char.IsWhiteSpace(c))continue;
            if(c=='²'){b.Append("^2");continue;}
            if(c is not ('+' or '-' or 'x' or '^' or '(' or ')' or ',')&&
                (c<'0'||c>'9'))return null;
            b.Append(c);
        }
        return b.ToString();
    }
    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Supports(family))return false;
        var norm=Normalize(answer);if(norm is null)return false;
        if(family==RootsFamily)
        {
            if(!ValidRoots(p))return false;
            string[] parts=norm.Split(',',StringSplitOptions.None);
            if(parts.Length!=2||
                !int.TryParse(parts[0],NumberStyles.AllowLeadingSign,CultureInfo.InvariantCulture,out int x)||
                !int.TryParse(parts[1],NumberStyles.AllowLeadingSign,CultureInfo.InvariantCulture,out int y))
                return false;
            return x!=y&&((x==0&&y==p["r"])||(y==0&&x==p["r"]));
        }
        if(!ValidCommon(p))return false;
        // Canonical positive GCF in front, followed by the primitive binomial
        // with the x term first. Not an unrestricted algebraic equivalence CAS.
        var match=Regex.Match(norm,
            @"^(?<g>[1-9][0-9]*)(?<factor>x(?:\^(?<k>2))?)?\((?<a>[1-9][0-9]*)?x(?<sign>[+-])(?<b>[1-9][0-9]*)\)$",
            RegexOptions.CultureInvariant);
        if(!match.Success)return false;
        int factorPower=match.Groups["factor"].Success
            ?(match.Groups["k"].Success?2:1):0;
        if(!int.TryParse(match.Groups["g"].Value,NumberStyles.None,
                CultureInfo.InvariantCulture,out int g)||
           !int.TryParse(match.Groups["b"].Value,NumberStyles.None,
                CultureInfo.InvariantCulture,out int magnitude))return false;
        int a=1;
        if(match.Groups["a"].Success&&
           !int.TryParse(match.Groups["a"].Value,NumberStyles.None,
                CultureInfo.InvariantCulture,out a))return false;
        int b=match.Groups["sign"].Value=="-"?-magnitude:magnitude;
        return factorPower==p["k"]&&g==p["g"]&&a==p["a"]&&b==p["b"];
    }
}

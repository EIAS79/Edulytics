using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Grade10 General U01L08 source-bounded factorization of a²x²−b² and its
/// exact rational roots. Authored at Stage C, NOT RUN or active in production.
/// Higher powers, GCF-first and geometric drawings remain teacher-scored.
/// </summary>
public sealed partial class ExactSkillContractQuestionEngine
{
    private static ExactProblem BuildG10DifferenceSquares(
        string family, Random random, ExactSkillQuestionDifficulty difficulty)
    {
        int bound=difficulty==ExactSkillQuestionDifficulty.Challenge?8:6;
        int a,b;
        do {a=random.Next(1,bound+1);b=random.Next(1,10);}
        while(UaeG10DifferenceSquaresResponse.Gcd(a,b)!=1);
        string expression=$"{a*a}س²−{b*b}";
        string prompt=family==UaeG10DifferenceSquaresResponse.FactorFamily
            ?$"حلّل {expression} إلى حاصل ضرب ثنائيتين باستعمال الفرق بين مربعين. اكتب العاملين كاملين بين قوسين."
            :$"حل المعادلة {expression}=٠ بالتحليل إلى الفرق بين مربعين. اكتب الجذرين مفصولين بفاصلة، واستخدم كسرًا مبسطًا عند الحاجة.";
        return Problem(family,prompt,
            "حدد المربعين (أ س)² وب² ثم استخدم (أ س−ب)(أ س+ب)، وبعدها استخدم خاصية ناتج الضرب الصفري إذا كانت معادلة.",
            AssessmentItemType.ShortAnswer,("a",a),("b",b));
    }
}

internal static class UaeG10DifferenceSquaresResponse
{
    internal const string FactorFamily="uae.g10.general.u01.l08.full_difference_squares_factor";
    internal const string RootsFamily="uae.g10.general.u01.l08.difference_squares_rational_roots";
    internal static bool Supports(string? family)=>family is FactorFamily or RootsFamily;
    internal static int Gcd(int a,int b)
    {
        a=Math.Abs(a);b=Math.Abs(b);
        while(b!=0){int v=a%b;a=b;b=v;}
        return a;
    }

    private static bool Valid(IReadOnlyDictionary<string,int> p)=>
        p.Count==2&&p.ContainsKey("a")&&p.ContainsKey("b")&&
        p["a"] is >=1 and <=8&&p["b"] is >=1 and <=9&&
        Gcd(p["a"],p["b"])==1;

    private static string Term(int a,int b,bool positive)=>
        "("+(a==1?"س":a.ToString(CultureInfo.InvariantCulture)+"س")+
        (positive?"+":"−")+b.ToString(CultureInfo.InvariantCulture)+")";
    private static string Frac(int n,int d)
    {
        int g=Gcd(n,d);n/=g;d/=g;
        return d==1?n.ToString(CultureInfo.InvariantCulture):
            n.ToString(CultureInfo.InvariantCulture)+"/"+d.ToString(CultureInfo.InvariantCulture);
    }
    internal static string Format(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Supports(family)||!Valid(p))throw new InvalidOperationException("Bad G10 difference-squares parameters.");
        int a=p["a"],b=p["b"];
        return family==FactorFamily?Term(a,b,false)+Term(a,b,true):
            Frac(-b,a)+","+Frac(b,a);
    }
    private static string? Normalize(string? answer)
    {
        if(string.IsNullOrWhiteSpace(answer)||answer.Length>128)return null;
        var text=new StringBuilder();
        foreach(char raw in answer.Trim())
        {
            char c=raw switch {
                >= '٠' and <= '٩'=>(char)('0'+raw-'٠'),
                >= '۰' and <= '۹'=>(char)('0'+raw-'۰'),
                '−' or '–'=>'-', 'س' or 'X'=>'x','،'=>',',_=>raw
            };
            if(char.IsWhiteSpace(c))continue;
            if(c is not ('+' or '-' or '(' or ')' or 'x' or ',' or '/') &&
                (c<'0'||c>'9'))return null;
            text.Append(c);
        }
        return text.ToString();
    }
    private static bool TryFraction(string s,out int n,out int d)
    {
        n=0;d=1;
        string[] p=s.Split('/',StringSplitOptions.None);
        if(p.Length is <1 or >2||!int.TryParse(p[0],NumberStyles.AllowLeadingSign,
           CultureInfo.InvariantCulture,out n))return false;
        if(p.Length==2&&!int.TryParse(p[1],NumberStyles.None,
           CultureInfo.InvariantCulture,out d))return false;
        return n is >=-200 and <=200&&d is >=1 and <=64;
    }
    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Supports(family)||!Valid(p))return false;
        string? text=Normalize(answer);
        if(text is null)return false;
        int a=p["a"],b=p["b"];
        if(family==RootsFamily)
        {
            var roots=text.Split(',',StringSplitOptions.None);
            if(roots.Length!=2||!TryFraction(roots[0],out int n1,out int d1)||
               !TryFraction(roots[1],out int n2,out int d2))return false;
            return (n1*a==-b*d1&&n2*a==b*d2)||
                   (n1*a==b*d1&&n2*a==-b*d2);
        }
        var matches=Regex.Matches(text,
            @"\((?<lead>[1-9][0-9]*)?x(?<sign>[+-])(?<constant>[1-9][0-9]*)\)",
            RegexOptions.CultureInvariant);
        if(matches.Count!=2||matches[0].Index!=0||
           matches[1].Index!=matches[0].Length||
           matches[1].Index+matches[1].Length!=text.Length)return false;
        int[] lead=new int[2],constants=new int[2];
        for(int i=0;i<2;i++)
        {
            Match m=matches[i];
            int coefficient=1;
            if(m.Groups["lead"].Success&&!int.TryParse(m.Groups["lead"].Value,
                NumberStyles.None,CultureInfo.InvariantCulture,out coefficient))return false;
            if(!int.TryParse(m.Groups["constant"].Value,NumberStyles.None,
                CultureInfo.InvariantCulture,out int v))return false;
            if(coefficient is <1 or >64||v is <1 or >64)return false;
            lead[i]=coefficient;
            constants[i]=m.Groups["sign"].Value=="-"?-v:v;
        }
        return lead[0]*lead[1]==a*a&&
            lead[0]*constants[1]+lead[1]*constants[0]==0&&
            constants[0]*constants[1]==-b*b;
    }
}

using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// UAE Grade10 canonical38 U04L05 quadratic formula/discriminant. Bounded
/// numerical source mechanics. Radical and complex exact roots and open
/// graphical methods are teacher assessed until separate Stage D verification.
/// </summary>
public sealed partial class ExactSkillContractQuestionEngine
{
    private static ExactProblem BuildG10QuadraticFormula(
        string family,Random random,ExactSkillQuestionDifficulty difficulty)
    {
        int lim=difficulty==ExactSkillQuestionDifficulty.Challenge?9:6;
        int a,b,c;
        do a=random.Next(-4,5);while(a==0);
        if(family==UaeG10QuadraticFormulaResponse.RootsFamily)
        {
            int first=random.Next(-6,7),second;
            do second=random.Next(-6,7);while(second==first);
            b=-a*(first+second);
            c=a*first*second;
        }
        else
        {
            b=random.Next(-lim,lim+1);
            c=random.Next(-lim,lim+1);
        }
        string prompt=family switch
        {
            UaeG10QuadraticFormulaResponse.DiscriminantFamily=>
                $"للمعادلة {a}س²+({b})س+({c})=٠ حيث أ={a} وب={b} وج={c}، احسب المميز Δ=ب²−٤أج واكتب قيمة صحيحة فقط.",
            UaeG10QuadraticFormulaResponse.RealCountFamily=>
                $"للمعادلة {a}س²+({b})س+({c})=٠، كم عدد الجذور الحقيقية المختلفة بناءً على Δ=ب²−٤أج؟ أجب بالعدد ٠ أو ١ أو ٢.",
            _=>$"استخدم القانون العام س=(−ب±√Δ)/(٢أ) لحل {a}س²+({b})س+({c})=٠. اكتب الجذرين الصحيحين مفصولين بفاصلة، دون س=؛ ترتيب الجذرين غير مهم."
        };
        return Problem(family,prompt,
            "حدد أ وب وج مع إشاراتها، احسب Δ=ب²−٤أج؛ إذا Δ≥٠ فاستخدم القانون العام مع ± ومقام ٢أ. إذا كان سالبًا فلا يوجد جذر حقيقي.",
            family==UaeG10QuadraticFormulaResponse.RootsFamily
                ?AssessmentItemType.ShortAnswer:AssessmentItemType.Numeric,
            ("a",a),("b",b),("c",c));
    }
}

internal static class UaeG10QuadraticFormulaResponse
{
    internal const string DiscriminantFamily="uae.g10.general.u04.l05.discriminant";
    internal const string RealCountFamily="uae.g10.general.u04.l05.real_root_count";
    internal const string RootsFamily="uae.g10.general.u04.l05.integer_roots_formula";
    internal static bool Supports(string? f)=>f is DiscriminantFamily or RealCountFamily or RootsFamily;

    private static bool TryExpected(string family,IReadOnlyDictionary<string,int> p,out int delta,out int[] roots)
    {
        delta=0;roots=[];
        if(!Supports(family)||p.Count!=3||!p.ContainsKey("a")||!p.ContainsKey("b")||!p.ContainsKey("c"))
            return false;
        int a=p["a"],b=p["b"],c=p["c"];
        if(a is <-4 or >4 or 0||b is <-48 or >48||c is <-144 or >144)return false;
        delta=b*b-4*a*c;
        if(family!=RootsFamily)return true;
        if(delta<=0)return false;
        int sqrt=(int)Math.Sqrt(delta);
        if(sqrt*sqrt!=delta)return false;
        int den=2*a,n1=-b-sqrt,n2=-b+sqrt;
        if(n1%den!=0||n2%den!=0)return false;
        roots=[n1/den,n2/den];
        return roots[0]!=roots[1]&&roots.All(r=>r is >=-6 and <=6);
    }

    internal static string Format(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!TryExpected(family,p,out int delta,out int[] roots))
            throw new InvalidOperationException("Invalid Grade10 quadratic-formula parameters.");
        if(family==DiscriminantFamily)return delta.ToString(CultureInfo.InvariantCulture);
        if(family==RealCountFamily)return (delta>0?2:delta==0?1:0).ToString(CultureInfo.InvariantCulture);
        return string.Join(",",roots.OrderBy(x=>x).Select(x=>x.ToString(CultureInfo.InvariantCulture)));
    }

    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? response)
    {
        if(!TryExpected(family,p,out int delta,out int[] roots)||string.IsNullOrWhiteSpace(response)||response.Length>100)return false;
        string s=string.Concat(response.Trim().Select(ch=>ch switch
        {
            >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
            >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
            '−' or '–'=>'-',
            '،'=>',',
            _=>ch
        }));
        if(family!=RootsFamily)
        {
            int expected=family==DiscriminantFamily?delta:(delta>0?2:delta==0?1:0);
            return int.TryParse(s,NumberStyles.AllowLeadingSign,CultureInfo.InvariantCulture,out int got)&&got==expected;
        }
        var parts=s.Split(',',StringSplitOptions.None);
        if(parts.Length!=2||
           !int.TryParse(parts[0].Trim(),NumberStyles.AllowLeadingSign,CultureInfo.InvariantCulture,out int one)||
           !int.TryParse(parts[1].Trim(),NumberStyles.AllowLeadingSign,CultureInfo.InvariantCulture,out int two))return false;
        return (one==roots[0]&&two==roots[1])||(one==roots[1]&&two==roots[0]);
    }
}

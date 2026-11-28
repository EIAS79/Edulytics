using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;

/// <summary>Canonical G10 U04L02. Graph intersections of quadratic with y=0.
/// Exact bounded integer roots or zero/one/two count. Noninteger graphical
/// approximations and calculator usage remain teacher-reviewed. Stage D not run.
/// </summary>
public sealed partial class ExactSkillContractQuestionEngine
{
    private static ExactProblem BuildG10QuadraticGraphRoots(
        string family,Random random,ExactSkillQuestionDifficulty difficulty)
    {
        int a=0;
        while(a==0)a=random.Next(-4,5);
        int mode=family==UaeG10QuadraticGraphRootsResponse.CountFamily
            ?random.Next(0,3):random.Next(0,2);
        int r1=random.Next(-5,6),r2=random.Next(-5,6),h=random.Next(-4,5);
        int k=random.Next(1,7);
        if(mode==1)r2=r1;
        if(mode==0)
        {
            while(r2==r1)r2=random.Next(-5,6);
        }
        int b,c;
        if(mode==2) {b=-2*a*h;c=a*(h*h+k);}
        else {b=-a*(r1+r2);c=a*r1*r2;}
        string polynomial=$"{a}س²+({b})س+({c})";
        string prompt=family switch
        {
            UaeG10QuadraticGraphRootsResponse.CountFamily=>
               $"عند رسم ص={polynomial}، كم نقطة تقاطع حقيقية مختلفة مع محور السينات؟ اكتب ٠ أو ١ أو ٢. المماس للمحور عند الرأس يُعد نقطة واحدة.",
            UaeG10QuadraticGraphRootsResponse.LeftFamily=>
               $"عند رسم ص={polynomial}، ما الإحداثي السيني لأقصى نقطة تقاطع مع محور السينات إلى اليسار؟ اكتب قيمة س عددًا صحيحًا، وإن كان تماس واحد فاكتب قيمة التماس.",
            _=>$"عند رسم ص={polynomial}، ما الإحداثي السيني لنقطة التقاطع الحقيقية الواقعة أقصى اليمين على محور السينات؟ اكتب القيمة الصحيحة، وإن كان تماس واحد فاكتبها."
        };
        return Problem(family,prompt,
            "التقاطعات السينية هي الأصفار الحقيقية للدالة حيث ص=٠. يحدد المميز Δ=ب²−٤أج عدد التقاطعات: موجب اثنان، صفر واحد، سالب صفر. تحقق من أن الدالة عند الجذر تساوي الصفر.",
            AssessmentItemType.Numeric,
            ("a",a),("b",b),("c",c),("mode",mode),
            ("r1",r1),("r2",r2),("h",h),("k",k));
    }
}

internal static class UaeG10QuadraticGraphRootsResponse
{
    internal const string CountFamily="uae.g10.general.u04.l02.real_root_count";
    internal const string LeftFamily="uae.g10.general.u04.l02.left_intercept";
    internal const string RightFamily="uae.g10.general.u04.l02.right_intercept";
    internal static bool Supports(string? family)=>family is CountFamily or LeftFamily or RightFamily;

    private static bool Expected(string family,IReadOnlyDictionary<string,int> p,out int result)
    {
        result=0;
        string[] keys=["a","b","c","mode","r1","r2","h","k"];
        if(!Supports(family)||p.Count!=keys.Length||keys.Any(key=>!p.ContainsKey(key)))
            return false;
        int a=p["a"],b=p["b"],c=p["c"],mode=p["mode"];
        int r1=p["r1"],r2=p["r2"],h=p["h"],k=p["k"];
        if(a is 0 or < -4 or >4||r1 is < -5 or > 5||r2 is < -5 or >5||
           h is < -4 or >4||k is < 1 or >6||mode is <0 or >2)return false;
        if(mode==0&&(r1==r2||b!=-a*(r1+r2)||c!=a*r1*r2))return false;
        if(mode==1&&(r1!=r2||b!=-2*a*r1||c!=a*r1*r1))return false;
        if(mode==2&&(family!=CountFamily||b!=-2*a*h||c!=a*(h*h+k)))return false;
        if(mode!=2)
        {
            if(b*b-4*a*c<0)return false;
            result=family switch
            {
                CountFamily=>r1==r2?1:2,
                LeftFamily=>Math.Min(r1,r2),
                _=>Math.Max(r1,r2)
            };
            return true;
        }
        if(b*b-4*a*c>=0)return false;
        result=0;return true;
    }

    internal static string Format(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Expected(family,p,out int x))throw new InvalidOperationException("Invalid source graph roots parameters.");
        return x.ToString(CultureInfo.InvariantCulture);
    }

    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Expected(family,p,out var x)||string.IsNullOrWhiteSpace(answer))return false;
        var digits=string.Concat(answer.Trim().Select(ch=>ch switch
        {
            >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
            >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
            '−' or '–'=>'-',_=>ch
        }));
        return int.TryParse(digits,NumberStyles.AllowLeadingSign,CultureInfo.InvariantCulture,out int val)&&val==x;
    }
}

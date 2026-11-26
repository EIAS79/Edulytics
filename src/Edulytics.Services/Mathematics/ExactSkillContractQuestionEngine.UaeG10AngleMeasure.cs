using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;

public sealed partial class ExactSkillContractQuestionEngine
{
    private static ExactProblem BuildG10AngleMeasure(string family,Random random,ExactSkillQuestionDifficulty difficulty)
    {
        int baseAngle=15*random.Next(0,24),k=random.Next(-4,5),r=random.Next(2,31),m=random.Next(1,13),b=random.Next(2,21),turns=random.Next(1,11),mode=random.Next(0,3);
        if(k==0)k=1;
        int theta=baseAngle+360*k, angle=mode==0?30:mode==1?90:150;
        string name=family.Substring(UaeG10AngleMeasureResponse.Prefix.Length);
        string prompt=name switch
        {
            "principal_degree"=>$"أوجد قياس الزاوية المشتركة في ضلع الانتهاء مع {theta}° في الفترة [0°,360°).",
            "coterminal_degree"=>$"انطلاقًا من زاوية قياسها {baseAngle}°، أضف {k} دورة كاملة موجهة (الإشارة تحدد الاتجاه). ما قياس الزاوية الناتجة بالدرجات؟",
            "degree_to_pi_coefficient"=>$"حوّل {theta}° إلى راديان. اكتب معامل π فقط على صورة كسر أو عدد صحيح.",
            "pi_coefficient_to_degree"=>$"حوّل ({theta/15}/12)π راديان إلى درجات.",
            "principal_radian_pi_coefficient"=>$"الزاوية ({theta/15}/12)π راديان. أوجد الزاوية المشتركة في [0,2π)، واكتب معامل π فقط.",
            "arc_pi_coefficient"=>$"دائرة نصف قطرها {r} سم وقوس قياس زاويته {15*m}°. اكتب معامل π في طول القوس.",
            "arc_from_radians"=>$"نصف قطر دائرة {r} سم وزاوية قوس {m} راديان. أوجد طول مسار القوس بالسنتيمتر (مع احتساب الدورات الكاملة إن وجدت).",
            "radians_from_arc"=>$"مسار دائري طوله {m*b} سم ونصف قطره {r} سم. أوجد مقدار زاوية الدوران بالراديان على صورة كسر أو عدد صحيح.",
            "radius_from_arc"=>$"مسار دائري طوله {r*m} سم وزاوية دورانه {m} راديان. أوجد نصف قطره بالسنتيمتر.",
            "parallelogram_area_exact"=>$"متوازي أضلاع ضلعاه المتجاوران {2*r} و{b} سم والزاوية المحصورة {angle}°. أوجد مساحته بالسنتيمتر المربع.",
            _=>$"عجلة نصف قطرها {r} سم تدور {turns}/2 دورة دون انزلاق. اكتب معامل π في المسافة المقطوعة بالسنتيمتر."
        };
        bool rational=name.Contains("coefficient")||name=="radians_from_arc";
        return Problem(family,prompt,"الدورة 360°=2π، والتحويل بقانون 180°=π؛ طول القوس rθ للراديان؛ مساحة متوازي الأضلاع ab sin θ.",rational?AssessmentItemType.ShortAnswer:AssessmentItemType.Numeric,("baseAngle",baseAngle),("k",k),("r",r),("m",m),("b",b),("turns",turns),("mode",mode));
    }
}

internal static class UaeG10TrigScalar
{
    internal static string Rational(long n,long d)
    {
        if(d<=0)throw new InvalidOperationException("Invalid denominator");
        long a=Math.Abs(n),b=d;while(b!=0)(a,b)=(b,a%b);n/=a;d/=a;
        return d==1?n.ToString(CultureInfo.InvariantCulture):$"{n}/{d}";
    }
    internal static string Normalize(string raw)=>string.Concat(raw.Trim().Where(c=>!char.IsWhiteSpace(c)).Select(c=>c switch{>= '٠' and <= '٩'=>(char)('0'+c-'٠'),>= '۰' and <= '۹'=>(char)('0'+c-'۰'),'٫'=>'.','⁄'=>'/','−'=>'-',_=>c}));
    internal static bool Matches(long n,long d,string? raw)
    {
        if(string.IsNullOrWhiteSpace(raw)||raw.Length>40||d<=0)return false;
        string s=Normalize(raw);var pair=s.Split('/');
        if(pair.Length==2)return long.TryParse(pair[0],NumberStyles.AllowLeadingSign,CultureInfo.InvariantCulture,out long a)&&long.TryParse(pair[1],NumberStyles.None,CultureInfo.InvariantCulture,out long b)&&a>=-1000000&&a<=1000000&&b>0&&b<=1000000&&a*d==b*n;
        return pair.Length==1&&decimal.TryParse(s,NumberStyles.AllowLeadingSign|NumberStyles.AllowDecimalPoint,CultureInfo.InvariantCulture,out decimal q)&&q>=-1000000&&q<=1000000&&q*d==n;
    }
    internal static string Rounded(double x)=>Math.Round(x,2,MidpointRounding.AwayFromZero).ToString("0.00",CultureInfo.InvariantCulture);
    internal static bool MatchesRounded(double value,string? raw)
    {
        if(string.IsNullOrWhiteSpace(raw)||raw.Length>40||!double.IsFinite(value))return false;
        if(!decimal.TryParse(Normalize(raw),NumberStyles.AllowLeadingSign|NumberStyles.AllowDecimalPoint,CultureInfo.InvariantCulture,out decimal q)||q < -1000000||q > 1000000||q*100!=decimal.Truncate(q*100))return false;
        return q==(decimal)Math.Round(value,2,MidpointRounding.AwayFromZero);
    }
}
internal static class UaeG10AngleMeasureResponse
{
    internal const string Prefix="uae.g10.general.u14.l02.";
    private static readonly HashSet<string> Names=new(StringComparer.Ordinal){"principal_degree","coterminal_degree","degree_to_pi_coefficient","pi_coefficient_to_degree","principal_radian_pi_coefficient","arc_pi_coefficient","arc_from_radians","radians_from_arc","radius_from_arc","parallelogram_area_exact","wheel_arc_pi_coefficient"};
    internal static bool Supports(string? f)=>f!=null&&f.StartsWith(Prefix,StringComparison.Ordinal)&&Names.Contains(f.Substring(Prefix.Length));
    private static bool Valid(string f,IReadOnlyDictionary<string,int> p)=>Supports(f)&&p.Count==7&&new[]{"baseAngle","k","r","m","b","turns","mode"}.All(p.ContainsKey)&&p["baseAngle"] is >=0 and <=345&&p["baseAngle"]%15==0&&p["k"] is >=-4 and <=4&&p["k"]!=0&&p["r"] is >=2 and <=30&&p["m"] is >=1 and <=12&&p["b"] is >=2 and <=20&&p["turns"] is >=1 and <=10&&p["mode"] is >=0 and <=2;
    private static (long N,long D) Answer(string f,IReadOnlyDictionary<string,int> p)
    {
        long a=p["baseAngle"],k=p["k"],r=p["r"],m=p["m"],b=p["b"],t=p["turns"],theta=a+360*k;
        return f.Substring(Prefix.Length) switch{"principal_degree"=>(a,1),"coterminal_degree"=>(theta,1),"degree_to_pi_coefficient"=>(theta,180),"pi_coefficient_to_degree"=>(theta,1),"principal_radian_pi_coefficient"=>(a,180),"arc_pi_coefficient"=>(r*m,12),"arc_from_radians"=>(r*m,1),"radians_from_arc"=>(m*b,r),"radius_from_arc"=>(r,1),"parallelogram_area_exact"=>(p["mode"]==1?2*r*b:r*b,1),"wheel_arc_pi_coefficient"=>(r*t,1),_=>throw new InvalidOperationException()};
    }
    internal static string Format(string f,IReadOnlyDictionary<string,int> p)
    {if(!Valid(f,p))throw new InvalidOperationException("Invalid G10 angle measure parameters");var(n,d)=Answer(f,p);return UaeG10TrigScalar.Rational(n,d);}
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? raw)
    {
        if(!Valid(f,p))return false;
        long theta=p["baseAngle"]+360L*p["k"],principal=((theta%360)+360)%360,r=p["r"],m=p["m"],b=p["b"];
        var(n,d)=f.Substring(Prefix.Length) switch{"principal_degree"=>(principal,1L),"coterminal_degree"=>(theta,1L),"degree_to_pi_coefficient"=>(theta/15,12L),"pi_coefficient_to_degree"=>(theta/15*180,12L),"principal_radian_pi_coefficient"=>(principal/15,12L),"arc_pi_coefficient"=>(r*15*m,180L),"arc_from_radians"=>(r*m,1L),"radians_from_arc"=>(m*b,r),"radius_from_arc"=>(r*m,m),"parallelogram_area_exact"=>(2*r*b*(p["mode"]==1?2:1),2L),"wheel_arc_pi_coefficient"=>(2*r*p["turns"],2L),_=>(0L,0L)};
        return UaeG10TrigScalar.Matches(n,d,raw);
    }
}

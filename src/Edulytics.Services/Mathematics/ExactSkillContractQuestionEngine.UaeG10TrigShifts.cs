using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
public sealed partial class ExactSkillContractQuestionEngine
{
    private static ExactProblem BuildG10TrigShifts(string family, Random random, ExactSkillQuestionDifficulty difficulty)
    {
        int a=random.Next(1,10)*(random.Next(2)==0?-1:1), b=random.Next(1,9), c=15*random.Next(-8,9), k=random.Next(-9,10), j=random.Next(0,5), fn=random.Next(0,3), sign=random.Next(0,2);
        string name=family.Substring(UaeG10TrigShiftsResponse.Prefix.Length);
        if(name is "amplitude" or "range_extreme" or "quarter_cycle_value" or "midline_from_extrema" or "amplitude_from_extrema") fn%=2;
        string[] labels={"sin","cos","tan"};
        string expr=$"y={a}{labels[fn]}({b}x{(c>=0?"+":"")}{c}°){(k>=0?"+":"")}{k}";
        int t=fn==2?180:360;
        string prompt=name switch {
            "phase_shift_degrees"=>$"أوجد قيمة الإزاحة الأفقية h بالدرجات للدالة {expr} بصيغة b(x−h). الموجب يمينًا.",
            "vertical_shift"=>$"أوجد الإزاحة الرأسية الموقعة للدالة {expr}.",
            "amplitude"=>$"أوجد سعة الدالة {expr}.",
            "period_degrees"=>$"أوجد الدور بالدرجات للدالة {expr}.",
            "range_extreme"=>$"أوجد القيمة {(sign==0?"الصغرى":"العظمى")} للدالة {expr}.",
            "quarter_cycle_value"=>$"أوجد قيمة {expr} عند x=({90*j-c}/{b})°.",
            "midline_from_extrema"=>$"لدالة جيبية القيمة العظمى {k+Math.Abs(a)} والصغرى {k-Math.Abs(a)}. أوجد الإحداثي y لمحور المنتصف.",
            "amplitude_from_extrema"=>$"لدالة جيبية القيمة العظمى {k+Math.Abs(a)} والصغرى {k-Math.Abs(a)}. أوجد السعة.",
            _=>$"دور {labels[fn]}(bx) هو ({t}/{b})° و b موجب. أوجد b."
        };
        bool rational=name is "phase_shift_degrees" or "period_degrees" or "coefficient_from_period";
        return Problem(family,prompt,"حلل الزاوية الداخلية وحدد نوع الدالة قبل حساب الدور أو السعة.",rational?AssessmentItemType.ShortAnswer:AssessmentItemType.Numeric,("a",a),("b",b),("c",c),("k",k),("j",j),("fn",fn),("sign",sign));
    }
}
internal static class UaeG10TrigShiftsResponse
{
    internal const string Prefix="uae.g10.general.u14.l08.";
    private static readonly HashSet<string> Names=new(StringComparer.Ordinal){"phase_shift_degrees","vertical_shift","amplitude","period_degrees","range_extreme","quarter_cycle_value","midline_from_extrema","amplitude_from_extrema","coefficient_from_period"};
    internal static bool Supports(string? f)=>f!=null&&f.StartsWith(Prefix,StringComparison.Ordinal)&&Names.Contains(f.Substring(Prefix.Length));
    private static bool Valid(string f,IReadOnlyDictionary<string,int> p){
        if(!Supports(f)||p.Count!=7||!p.TryGetValue("a",out int a)||!p.TryGetValue("b",out int b)||!p.TryGetValue("c",out int c)||!p.TryGetValue("k",out int k)||!p.TryGetValue("j",out int j)||!p.TryGetValue("fn",out int fn)||!p.TryGetValue("sign",out int sign)||a==0||a< -9||a>9||b<1||b>8||c< -120||c>120||c%15!=0||k< -9||k>9||j<0||j>4||fn<0||fn>2||sign<0||sign>1)return false;
        string n=f.Substring(Prefix.Length);
        return (n is "amplitude" or "range_extreme" or "quarter_cycle_value" or "midline_from_extrema" or "amplitude_from_extrema")?fn<2:true;
    }
    private static (long N,long D) Answer(string f,IReadOnlyDictionary<string,int> p){
        int a=p["a"],b=p["b"],c=p["c"],k=p["k"],j=p["j"],fn=p["fn"],sign=p["sign"];
        return f.Substring(Prefix.Length) switch {
            "phase_shift_degrees"=>(-c,b),
            "vertical_shift" or "midline_from_extrema"=>(k,1),
            "amplitude" or "amplitude_from_extrema"=>(Math.Abs(a),1),
            "period_degrees"=>(fn==2?180:360,b),
            "quarter_cycle_value"=>(k+a*(fn==0?new[]{0,1,0,-1,0}[j]:new[]{1,0,-1,0,1}[j]),1),
            "range_extreme"=>(k+(sign==0?-Math.Abs(a):Math.Abs(a)),1),
            _=>(b,1)
        };
    }
    internal static string Format(string f,IReadOnlyDictionary<string,int> p){if(!Valid(f,p))throw new InvalidOperationException("Invalid shift parameters");var(n,d)=Answer(f,p);return UaeG10TrigScalar.Rational(n,d);}
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? raw){
        if(!Valid(f,p))return false;
        int a=p["a"],b=p["b"],c=p["c"],k=p["k"],j=p["j"],fn=p["fn"],sign=p["sign"],m=k-Math.Abs(a),M=k+Math.Abs(a),t=fn==2?180:360;
        double x=(90.0*j-c)/b, z=(b*x+c)*Math.PI/180;
        var(n,d)=f.Substring(Prefix.Length) switch {
            "phase_shift_degrees"=>((long)-c,(long)b),
            "vertical_shift" or "midline_from_extrema"=>((long)M+m,2L),
            "amplitude" or "amplitude_from_extrema"=>((long)M-m,2L),
            "period_degrees"=>((long)t,(long)b),
            "quarter_cycle_value"=>((long)Math.Round(k+a*(fn==0?Math.Sin(z):Math.Cos(z))),1L),
            "range_extreme"=>((long)(sign==0?m:M),1L),
            _=>((long)t*b,(long)t)
        };
        return UaeG10TrigScalar.Matches(n,d,raw);
    }
}

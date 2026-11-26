using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
public sealed partial class ExactSkillContractQuestionEngine
{
    private static ExactProblem BuildG10Periodic(string family, Random random, ExactSkillQuestionDifficulty difficulty)
    {
        int triple=random.Next(0,4),q=random.Next(1,5),mode=random.Next(0,2),T=random.Next(2,13),start=random.Next(0,12),k=random.Next(1,9),value=random.Next(-15,16),axis=random.Next(0,4),special=random.Next(0,4),turn=random.Next(-4,5);
        var(x,y)=UaeG10GeneralAnglesResponse.Point(triple,q);
        int r=new[]{5,13,17,25}[triple],angle=(mode==0?new[]{30,150,210,330}[special]:new[]{60,120,240,300}[special])+360*turn;
        string fn=mode==0?"sin":"cos",name=family.Substring(UaeG10PeriodicResponse.Prefix.Length);
        string prompt=name switch
        {
            "unit_circle_coordinate"=>$"النقطة ({x}/{r},{y}/{r}) على دائرة الوحدة. أوجد {fn} θ على صورة كسر أو عدد صحيح.",
            "period_from_peaks"=>$"منحنى دوري له قمة واحدة في كل دورة. قمتان متتاليتان عند t={start} وt={start+T}. أوجد الدور الأساسي.",
            "periodic_value"=>$"دالة دورية دورها {T}، وf({start})={value}. أوجد f({start+k*T}).",
            "cycle_count"=>$"نمط يتكرر كل {T} ثانية. كم دورة كاملة في {T*k} ثانية؟",
            "axis_trig_value"=>$"أوجد {fn} ({90*axis+360*turn}°).",
            "special_sin_cos_rational"=>$"أوجد {fn} ({angle}°) بالضبط على صورة كسر أو عدد صحيح.",
            _=>$"ما معامل π في الدور الأساسي للدالة {new[]{"sin","cos","tan"}[special%3]} عندما يقاس المدخل بالراديان؟"
        };
        bool rational=name=="unit_circle_coordinate"||name=="special_sin_cos_rational";
        return Problem(family,prompt,"حدد الإحداثيات أو اختزل المدخل بالدورة؛ الدور مسافة أفقية.",rational?AssessmentItemType.ShortAnswer:AssessmentItemType.Numeric,("triple",triple),("q",q),("mode",mode),("T",T),("start",start),("k",k),("value",value),("axis",axis),("special",special),("turn",turn));
    }
}
internal static class UaeG10PeriodicResponse
{
    internal const string Prefix="uae.g10.general.u14.l06.";
    private static readonly HashSet<string> Names=new(StringComparer.Ordinal){"unit_circle_coordinate","period_from_peaks","periodic_value","cycle_count","axis_trig_value","special_sin_cos_rational","period_pi_coefficient"};
    internal static bool Supports(string? f)=>f!=null&&f.StartsWith(Prefix,StringComparison.Ordinal)&&Names.Contains(f.Substring(Prefix.Length));
    private static bool Valid(string f,IReadOnlyDictionary<string,int> p)=>Supports(f)&&p.Count==10&&new[]{"triple","q","mode","T","start","k","value","axis","special","turn"}.All(p.ContainsKey)&&p["triple"] is >=0 and <=3&&p["q"] is >=1 and <=4&&p["mode"] is >=0 and <=1&&p["T"] is >=2 and <=12&&p["start"] is >=0 and <=11&&p["k"] is >=1 and <=8&&p["value"] is >=-15 and <=15&&p["axis"] is >=0 and <=3&&p["special"] is >=0 and <=3&&p["turn"] is >=-4 and <=4;
    private static (long N,long D) Answer(string f,IReadOnlyDictionary<string,int> p)
    {
        int mode=p["mode"],s=p["special"],axis=p["axis"];var(x,y)=UaeG10GeneralAnglesResponse.Point(p["triple"],p["q"]);int r=(int)Math.Sqrt(x*x+y*y);
        return f.Substring(Prefix.Length) switch
        {
            "unit_circle_coordinate"=>(mode==0?y:x,r),
            "period_from_peaks"=>(p["T"],1),
            "periodic_value"=>(p["value"],1),
            "cycle_count"=>(p["k"],1),
            "axis_trig_value"=>(mode==0?new[]{0,1,0,-1}[axis]:new[]{1,0,-1,0}[axis],1),
            "special_sin_cos_rational"=>((mode==0?s<2:s==0||s==3)?1:-1,2),
            _=>(s%3==2?1:2,1)
        };
    }
    internal static string Format(string f,IReadOnlyDictionary<string,int> p)
    {if(!Valid(f,p))throw new InvalidOperationException("Invalid periodic parameters");var(n,d)=Answer(f,p);return UaeG10TrigScalar.Rational(n,d);}
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? raw)
    {
        if(!Valid(f,p))return false;
        int mode=p["mode"],s=p["special"],axis=p["axis"],T=p["T"],start=p["start"],k=p["k"],turn=p["turn"];
        var(x,y)=UaeG10GeneralAnglesResponse.Point(p["triple"],p["q"]);int r=new[]{5,13,17,25}[p["triple"]];
        int angle=(mode==0?new[]{30,150,210,330}[s]:new[]{60,120,240,300}[s])+360*turn;
        int av=axis*90+360*turn;
        double v=mode==0?Math.Sin(angle*Math.PI/180):Math.Cos(angle*Math.PI/180),w=mode==0?Math.Sin(av*Math.PI/180):Math.Cos(av*Math.PI/180);
        var(n,d)=f.Substring(Prefix.Length) switch
        {
            "unit_circle_coordinate"=>((long)(mode==0?y:x),(long)r),
            "period_from_peaks"=>((long)(start+T-start),1L),
            "periodic_value"=>((long)p["value"],1L),
            "cycle_count"=>((long)(T*k/T),1L),
            "axis_trig_value"=>((long)Math.Round(w),1L),
            "special_sin_cos_rational"=>((long)Math.Round(2*v),2L),
            _=>((long)(s%3==2?180:360)/180,1L)
        };
        return UaeG10TrigScalar.Matches(n,d,raw);
    }
}

using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
public sealed partial class ExactSkillContractQuestionEngine
{
    private static ExactProblem BuildG10TrigGraphs(string family, Random random, ExactSkillQuestionDifficulty difficulty)
    {
        int a=random.Next(1,10)*(random.Next(2)==0?-1:1), b=random.Next(1,9), k=random.Next(0,5), fn=random.Next(0,6), sign=random.Next(0,2);
        string name=family.Substring(UaeG10TrigGraphsResponse.Prefix.Length);
        if(name is "amplitude" or "quarter_cycle_value" or "range_extreme" or "frequency_hz") fn%=2;
        if(name=="first_positive_asymptote_degree") fn=2+fn%4;
        if(name=="reciprocal_range_boundary") fn=4+fn%2;
        string[] labels={"sin","cos","tan","cot","sec","csc"};
        string expr=$"y={a}{labels[fn]}({b}x)";
        string prompt=name switch {
            "amplitude"=>$"أوجد سعة {expr}.",
            "period_pi_coefficient"=>$"أوجد معامل π في الدور الأساسي للدالة {expr} (x بالراديان).",
            "quarter_cycle_value"=>$"أوجد قيمة {expr} عند x={k}π/(2×{b}).",
            "frequency_hz"=>$"إذا كان y={a}sin(2π×{b}t) والزمن بالثواني، فما التردد بالهرتز؟",
            "range_extreme"=>$"أوجد القيمة {(sign==0?"الصغرى":"العظمى")} للدالة {expr}.",
            "first_positive_asymptote_degree"=>$"ما قياس x بالدرجات عند أول خط تقارب رأسي موجب للدالة {expr}؟",
            _=>$"للدالة {expr} يكون المدى y≤−m أو y≥m. أوجد m."
        };
        bool rational=name is "period_pi_coefficient" or "first_positive_asymptote_degree";
        return Problem(family,prompt,"حدد نوع الدالة ثم استعمل خصائص الدور والمدى والمجال.",rational?AssessmentItemType.ShortAnswer:AssessmentItemType.Numeric,("a",a),("b",b),("k",k),("fn",fn),("sign",sign));
    }
}
internal static class UaeG10TrigGraphsResponse
{
    internal const string Prefix="uae.g10.general.u14.l07.";
    private static readonly HashSet<string> Names=new(StringComparer.Ordinal){"amplitude","period_pi_coefficient","quarter_cycle_value","frequency_hz","range_extreme","first_positive_asymptote_degree","reciprocal_range_boundary"};
    internal static bool Supports(string? f)=>f!=null&&f.StartsWith(Prefix,StringComparison.Ordinal)&&Names.Contains(f.Substring(Prefix.Length));
    private static bool Valid(string f,IReadOnlyDictionary<string,int> p){
        if(!Supports(f)||p.Count!=5||!p.TryGetValue("a",out int a)||!p.TryGetValue("b",out int b)||!p.TryGetValue("k",out int k)||!p.TryGetValue("fn",out int fn)||!p.TryGetValue("sign",out int sign)||a==0||a < -9||a>9||b<1||b>8||k<0||k>4||fn<0||fn>5||sign<0||sign>1)return false;
        string n=f.Substring(Prefix.Length);
        return n switch {"amplitude" or "quarter_cycle_value" or "range_extreme" or "frequency_hz"=>fn<2,"first_positive_asymptote_degree"=>fn>=2,"reciprocal_range_boundary"=>fn>=4,_=>true};
    }
    private static (long N,long D) Answer(string f,IReadOnlyDictionary<string,int> p){
        int a=p["a"],b=p["b"],k=p["k"],fn=p["fn"],sign=p["sign"];
        return f.Substring(Prefix.Length) switch {
            "amplitude" or "reciprocal_range_boundary"=>(Math.Abs(a),1),
            "period_pi_coefficient"=>(fn is 2 or 3?1:2,b),
            "quarter_cycle_value"=>(a*(fn==0?new[]{0,1,0,-1,0}[k]:new[]{1,0,-1,0,1}[k]),1),
            "frequency_hz"=>(b,1),
            "range_extreme"=>(sign==0?-Math.Abs(a):Math.Abs(a),1),
            _=>(fn is 2 or 4?90:180,b)
        };
    }
    internal static string Format(string f,IReadOnlyDictionary<string,int> p){if(!Valid(f,p))throw new InvalidOperationException("Invalid graph parameters");var(n,d)=Answer(f,p);return UaeG10TrigScalar.Rational(n,d);}
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? raw){
        if(!Valid(f,p))return false;
        int a=p["a"],b=p["b"],k=p["k"],fn=p["fn"],sign=p["sign"];
        var(n,d)=f.Substring(Prefix.Length) switch {
            "amplitude" or "reciprocal_range_boundary"=>((long)Math.Sqrt(a*a),1L),
            "period_pi_coefficient"=>((long)(fn==2||fn==3?180:360),180L*b),
            "quarter_cycle_value"=>((long)Math.Round(a*(fn==0?Math.Sin(k*Math.PI/2):Math.Cos(k*Math.PI/2))),1L),
            "frequency_hz"=>((long)b*360,360L),
            "range_extreme"=>((long)Math.Max(a,-a)*(2*sign-1),1L),
            _=>((long)(fn==2||fn==4?90:180), (long)b)
        };
        return UaeG10TrigScalar.Matches(n,d,raw);
    }
}

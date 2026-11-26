using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
public sealed partial class ExactSkillContractQuestionEngine
{
    private static ExactProblem BuildG10InverseTrig(string family, Random random, ExactSkillQuestionDifficulty difficulty)
    {
        int fn=random.Next(0,3),j=random.Next(0,5),angle=30*random.Next(-12,13),num=random.Next(-15,16),den=10,t=random.Next(0,4),sign=random.Next(0,2);
        string name=family.Substring(UaeG10InverseTrigResponse.Prefix.Length);
        string[] labels={"arcsin","arccos","arctan"},ordinary={"sin","cos","tan"};
        if(name=="inverse_of_trig_principal_degrees")fn%=2;
        if(name=="rounded_angle_degrees"&&fn<2)num=Math.Clamp(num,-10,10);
        if(name=="right_triangle_angle_degrees"){num=random.Next(1,10);fn%=2;}
        var(x,y,h)=UaeG10InverseTrigResponse.Triple(t);
        string v=fn==2?new[]{"−√3","−1","0","1","√3"}[j]:new[]{"−1","−1/2","0","1/2","1"}[j];
        string prompt=name switch {
            "principal_angle_degrees"=>$"أوجد القيمة الرئيسية بالدرجات لـ {labels[fn]}({v}).",
            "principal_pi_coefficient"=>$"أوجد معامل π في القيمة الرئيسية بالراديان لـ {labels[fn]}({v}).",
            "inverse_defined"=>$"هل {labels[fn]}({num}/10) معرفة على الأعداد الحقيقية؟ اكتب 1 لنعم و0 للا.",
            "inverse_of_trig_principal_degrees"=>$"أوجد القيمة بالدرجات لـ {labels[fn]}({ordinary[fn]}({angle}°)).",
            "composition_rational"=>$"أوجد cos(arcsin({(sign==0?-x:x)}/{h})) في صورة كسر أو عدد صحيح.",
            "rounded_angle_degrees"=>$"أوجد {labels[fn]}({num}/10) بالدرجات لأقرب جزء من مئة.",
            "right_triangle_angle_degrees"=>fn==0?$"مثلث قائم فيه المقابل {num} والوتر 10؛ أوجد الزاوية الحادة بالدرجات لأقرب جزء من مئة.":$"مثلث قائم فيه المجاور {num} والوتر 10؛ أوجد الزاوية الحادة بالدرجات لأقرب جزء من مئة.",
            _=>$"كم حلًا للمعادلة {(fn==0?"csc":fn==1?"sec":"cot")} θ={(sign==0?-2:2)} في [0°,360°]؟"
        };
        bool rational=name is "principal_pi_coefficient" or "composition_rational";
        return Problem(family,prompt,"تحقق من المجال والزاوية الرئيسية، واضبط وحدة القياس قبل الحساب.",rational?AssessmentItemType.ShortAnswer:AssessmentItemType.Numeric,("fn",fn),("j",j),("angle",angle),("num",num),("den",den),("t",t),("sign",sign));
    }
}
internal static class UaeG10InverseTrigResponse
{
    internal const string Prefix="uae.g10.general.u14.l09.";
    private static readonly HashSet<string> Names=new(StringComparer.Ordinal){"principal_angle_degrees","principal_pi_coefficient","inverse_defined","inverse_of_trig_principal_degrees","composition_rational","rounded_angle_degrees","right_triangle_angle_degrees","reciprocal_equation_solution_count"};
    internal static bool Supports(string? f)=>f!=null&&f.StartsWith(Prefix,StringComparison.Ordinal)&&Names.Contains(f.Substring(Prefix.Length));
    internal static (int X,int Y,int H) Triple(int t)=>t switch{0=>(3,4,5),1=>(5,12,13),2=>(8,15,17),_=>(7,24,25)};
    private static bool Valid(string f,IReadOnlyDictionary<string,int> p){
        if(!Supports(f)||p.Count!=7||!p.TryGetValue("fn",out int fn)||!p.TryGetValue("j",out int j)||!p.TryGetValue("angle",out int angle)||!p.TryGetValue("num",out int num)||!p.TryGetValue("den",out int den)||!p.TryGetValue("t",out int t)||!p.TryGetValue("sign",out int sign)||fn<0||fn>2||j<0||j>4||angle< -360||angle>360||angle%30!=0||num< -15||num>15||den!=10||t<0||t>3||sign<0||sign>1)return false;
        string n=f.Substring(Prefix.Length);
        if(n=="inverse_of_trig_principal_degrees")return fn<2;
        if(n=="rounded_angle_degrees")return fn==2||Math.Abs(num)<=10;
        if(n=="right_triangle_angle_degrees")return fn<2&&num>0&&num<10;
        return true;
    }
    private static double Inverse(int fn,double v)=>fn==0?Math.Asin(v):fn==1?Math.Acos(v):Math.Atan(v);
    private static int Principal(int fn,int j)=>fn==0?new[]{-90,-30,0,30,90}[j]:fn==1?new[]{180,120,90,60,0}[j]:new[]{-60,-45,0,45,60}[j];
    private static bool Rounded(string f)=>f==Prefix+"rounded_angle_degrees"||f==Prefix+"right_triangle_angle_degrees";
    private static (long N,long D) Answer(string f,IReadOnlyDictionary<string,int> p){
        int fn=p["fn"],j=p["j"],num=p["num"],angle=p["angle"];var(x,y,h)=Triple(p["t"]);
        return f.Substring(Prefix.Length) switch {
            "principal_angle_degrees"=>(Principal(fn,j),1),
            "principal_pi_coefficient"=>(Principal(fn,j),180),
            "inverse_defined"=>(fn==2||Math.Abs(num)<=10?1:0,1),
            "inverse_of_trig_principal_degrees"=>((long)Math.Round(Inverse(fn,fn==0?Math.Clamp(Math.Sin(angle*Math.PI/180),-1,1):Math.Clamp(Math.Cos(angle*Math.PI/180),-1,1))*180/Math.PI),1),
            "composition_rational"=>(y,h),
            _=>(2,1)
        };
    }
    internal static string Format(string f,IReadOnlyDictionary<string,int> p){
        if(!Valid(f,p))throw new InvalidOperationException("Invalid inverse trigonometry parameters");
        if(Rounded(f))return UaeG10TrigScalar.Rounded(Inverse(p["fn"],p["num"]/10.0)*180/Math.PI);
        var(n,d)=Answer(f,p);return UaeG10TrigScalar.Rational(n,d);
    }
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? raw){
        if(!Valid(f,p))return false;
        int fn=p["fn"],j=p["j"],num=p["num"],angle=p["angle"];var(x,y,h)=Triple(p["t"]);
        if(Rounded(f))return UaeG10TrigScalar.MatchesRounded(Inverse(fn,num/10.0)*(180/Math.PI),raw);
        double value=fn==2?new[]{-Math.Sqrt(3),-1,0,1,Math.Sqrt(3)}[j]:new[]{-1,-0.5,0,0.5,1}[j];
        int reduced=((angle%360)+360)%360;
        long folded=fn==0?(reduced<=90?reduced:reduced<=270?180-reduced:reduced-360):(reduced<=180?reduced:360-reduced);
        long deg=(long)Math.Round(Inverse(fn,value)*180/Math.PI);
        var(n,d)=f.Substring(Prefix.Length) switch {
            "principal_angle_degrees"=>(deg,1L),
            "principal_pi_coefficient"=>(deg,180L),
            "inverse_defined"=>((long)(fn==2||num>=-10&&num<=10?1:0),1L),
            "inverse_of_trig_principal_degrees"=>(folded,1L),
            "composition_rational"=>((long)Math.Round(Math.Sqrt(h*h-x*x)),(long)h),
            _=>(2L,1L)
        };
        return UaeG10TrigScalar.Matches(n,d,raw);
    }
}

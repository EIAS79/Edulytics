using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
public sealed partial class ExactSkillContractQuestionEngine {
    private static ExactProblem BuildG10TrigAngleSum(string family,Random random,ExactSkillQuestionDifficulty difficulty){
        int kind=random.Next(0,6),shift=random.Next(0,3),a=random.Next(0,3),b=random.Next(0,3),sign=random.Next(0,2);
        string prompt;
        bool choice=family==UaeG10TrigAngleSumResponse.Choice;
        if(choice){
            var (q,options)=UaeG10TrigAngleSumResponse.Item(kind);
            prompt=$"اختر رقم القيمة الدقيقة لـ {q}: 1) {options[shift]}؛ 2) {options[(shift+1)%3]}؛ 3) {options[(shift+2)%3]}.";
        }else{
            var(x,y,h)=UaeG10TrigAngleSumResponse.Triple(a);var(u,v,w)=UaeG10TrigAngleSumResponse.Triple(b);
            string fn=family.EndsWith("sine_sum_difference")&&!family.EndsWith("cosine_sum_difference")?"sin":family.EndsWith("cosine_sum_difference")?"cos":"tan";
            prompt=$"زاويتان حادتان A و B؛ sin A={x}/{h}، cos A={y}/{h}، sin B={u}/{w}، cos B={v}/{w}. أوجد {fn}(A{(sign==0?"+":"−")}B) في صورة كسر أو عدد صحيح.";
        }
        return Problem(family,prompt,"استخدم قانون مجموع الزاويتين أو الفرق، ثم بسّط الناتج بدقة.",choice?AssessmentItemType.Numeric:AssessmentItemType.ShortAnswer,("kind",kind),("shift",shift),("a",a),("b",b),("sign",sign));
    }
}
internal static class UaeG10TrigAngleSumResponse {
    internal const string Choice="uae.g10.general.u14.l12.special_angle_exact_choice",Sin="uae.g10.general.u14.l12.sine_sum_difference",Cos="uae.g10.general.u14.l12.cosine_sum_difference",Tan="uae.g10.general.u14.l12.tangent_sum_difference";
    internal static bool Supports(string? f)=>f is Choice or Sin or Cos or Tan;
    internal static (long,long,long) Triple(int i)=>i switch{0=>(3,4,5),1=>(5,12,13),_=>(8,15,17)};
    private static bool Valid(string f,IReadOnlyDictionary<string,int> p)=>Supports(f)&&p.Count==5&&p.TryGetValue("kind",out int k)&&k>=0&&k<6&&p.TryGetValue("shift",out int h)&&h>=0&&h<3&&p.TryGetValue("a",out int a)&&a>=0&&a<3&&p.TryGetValue("b",out int b)&&b>=0&&b<3&&p.TryGetValue("sign",out int s)&&s>=0&&s<2;
    internal static (string,string[]) Item(int k)=>k switch {
        0=>("sin 105°",new[]{"(√6+√2)/4","(√6−√2)/4","−(√6+√2)/4"}),
        1=>("cos 165°",new[]{"−(√6+√2)/4","(√6+√2)/4","−(√6−√2)/4"}),
        2=>("tan 15°",new[]{"2−√3","2+√3","√3−2"}),
        3=>("tan 75°",new[]{"2+√3","2−√3","−2−√3"}),
        4=>("sin (−15°)",new[]{"(√2−√6)/4","(√6−√2)/4","(√6+√2)/4"}),
        _=>("cos (7π/12)",new[]{"(√2−√6)/4","(√6−√2)/4","−(√6+√2)/4"})
    };
    private static (long,long) Answer(string f,IReadOnlyDictionary<string,int> p){
        if(f==Choice)return(1+(3-p["shift"])%3,1);
        var(x,y,h)=Triple(p["a"]);var(u,v,w)=Triple(p["b"]);int s=p["sign"]==0?1:-1;long n=x*v+s*y*u,d=y*v-s*x*u;
        return f==Sin?(n,h*w):f==Cos?(d,h*w):(n,d);
    }
    internal static string Format(string f,IReadOnlyDictionary<string,int> p){if(!Valid(f,p))throw new InvalidOperationException("Invalid angle-sum parameters");var(n,d)=Answer(f,p);return UaeG10TrigScalar.Rational(n,d);}
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? raw){
        if(!Valid(f,p))return false;
        if(f==Choice){
            int k=p["kind"];double q=Math.PI/180,hi=(Math.Sqrt(6)+Math.Sqrt(2))/4,lo=(Math.Sqrt(6)-Math.Sqrt(2))/4,t=Math.Sqrt(3);
            (double actual,double[] options)=k switch{
                0=>(Math.Sin(105*q),new[]{hi,lo,-hi}),
                1=>(Math.Cos(165*q),new[]{-hi,hi,-lo}),
                2=>(Math.Tan(15*q),new[]{2-t,2+t,t-2}),
                3=>(Math.Tan(75*q),new[]{2+t,2-t,-2-t}),
                4=>(Math.Sin(-15*q),new[]{-lo,lo,hi}),
                _=>(Math.Cos(7*Math.PI/12),new[]{-lo,lo,-hi})
            };
            var matches=Enumerable.Range(0,3).Where(i=>Math.Abs(actual-options[i])<1e-10).ToArray();
            return matches.Length==1&&UaeG10TrigScalar.Matches(1+(matches[0]+3-p["shift"])%3,1,raw);
        }
        var(x,y,h)=Triple(p["a"]);var(u,v,w)=Triple(p["b"]);double angle=Math.Atan2(x,y)+(p["sign"]==0?1:-1)*Math.Atan2(u,v);
        long d=f==Tan?(long)Math.Round(Math.Cos(angle)*h*w):h*w;
        long n=(long)Math.Round((f==Cos?Math.Cos(angle):Math.Sin(angle))*h*w);
        return d!=0&&UaeG10TrigScalar.Matches(n,d,raw);
    }
}

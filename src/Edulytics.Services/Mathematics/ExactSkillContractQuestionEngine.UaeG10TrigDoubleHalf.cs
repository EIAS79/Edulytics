using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
public sealed partial class ExactSkillContractQuestionEngine {
    private static ExactProblem BuildG10TrigDoubleHalf(string family,Random random,ExactSkillQuestionDifficulty difficulty){
        int kind=random.Next(0,6),shift=random.Next(0,3),t=random.Next(0,3),q=random.Next(0,4),fn=random.Next(0,2);
        var(x,y,h)=UaeG10TrigAngleSumResponse.Triple(t);
        long sx=q<2?x:-x,cy=q==0||q==3?y:-y;
        string angle=$"sin θ={sx}/{h}، cos θ={cy}/{h}، و0°<θ<360°";
        string prompt=family switch{
            UaeG10TrigDoubleHalfResponse.Sin=>$"إذا {angle}، أوجد sin(2θ) في صورة كسر أو عدد صحيح.",
            UaeG10TrigDoubleHalfResponse.Cos=>$"إذا {angle}، أوجد cos(2θ) في صورة كسر أو عدد صحيح.",
            UaeG10TrigDoubleHalfResponse.Tan=>$"إذا {angle}، أوجد tan(2θ) في صورة كسر أو عدد صحيح.",
            UaeG10TrigDoubleHalfResponse.HalfSq=>$"إذا {angle}، أوجد {(fn==0?"sin²":"cos²")}(θ/2) في صورة كسر.",
            UaeG10TrigDoubleHalfResponse.HalfTan=>$"إذا {angle}، أوجد tan(θ/2) في صورة كسر أو عدد صحيح.",
            _=>ChoicePrompt(kind,shift)
        };
        return Problem(family,prompt,"طبق قانون الضعف أو النصف، وتحقق من إشارة الربع والمقام.",family==UaeG10TrigDoubleHalfResponse.Choice?AssessmentItemType.Numeric:AssessmentItemType.ShortAnswer,("kind",kind),("shift",shift),("t",t),("q",q),("fn",fn));
    }
    private static string ChoicePrompt(int kind,int shift){
        var(q,options)=UaeG10TrigDoubleHalfResponse.Item(kind);
        return $"اختر رقم القيمة الدقيقة لـ {q}: 1) {options[shift]}؛ 2) {options[(shift+1)%3]}؛ 3) {options[(shift+2)%3]}.";
    }
}
internal static class UaeG10TrigDoubleHalfResponse {
    internal const string Sin="uae.g10.general.u14.l13.double_sine",Cos="uae.g10.general.u14.l13.double_cosine",Tan="uae.g10.general.u14.l13.double_tangent",HalfSq="uae.g10.general.u14.l13.half_squared",HalfTan="uae.g10.general.u14.l13.half_tangent",Choice="uae.g10.general.u14.l13.half_exact_choice";
    internal static bool Supports(string? f)=>f is Sin or Cos or Tan or HalfSq or HalfTan or Choice;
    private static bool Valid(string f,IReadOnlyDictionary<string,int> p)=>Supports(f)&&p.Count==5&&p.TryGetValue("kind",out int k)&&k>=0&&k<6&&p.TryGetValue("shift",out int h)&&h>=0&&h<3&&p.TryGetValue("t",out int t)&&t>=0&&t<3&&p.TryGetValue("q",out int q)&&q>=0&&q<4&&p.TryGetValue("fn",out int n)&&n>=0&&n<2;
    internal static (string,string[]) Item(int k)=>k switch {
        0=>("sin 22.5°",new[]{"√(2−√2)/2","√(2+√2)/2","−√(2−√2)/2"}),
        1=>("cos 22.5°",new[]{"√(2+√2)/2","√(2−√2)/2","−√(2+√2)/2"}),
        2=>("cos 135°",new[]{"−√2/2","√2/2","−1"}),
        3=>("tan (π/8)",new[]{"√2−1","√2+1","1−√2"}),
        4=>("sin 112.5°",new[]{"√(2+√2)/2","−√(2+√2)/2","√(2−√2)/2"}),
        _=>("cos 112.5°",new[]{"−√(2−√2)/2","√(2−√2)/2","−√(2+√2)/2"})
    };
    private static (long,long) Answer(string f,IReadOnlyDictionary<string,int> p){
        if(f==Choice)return(1+(3-p["shift"])%3,1);
        var(x,y,h)=UaeG10TrigAngleSumResponse.Triple(p["t"]);int q=p["q"];long s=q<2?x:-x,c=q==0||q==3?y:-y;
        return f switch{Sin=>(2*s*c,h*h),Cos=>(c*c-s*s,h*h),Tan=>(2*s*c,c*c-s*s),HalfSq=>(h+(p["fn"]==0?-c:c),2*h),_=>(s,h+c)};
    }
    internal static string Format(string f,IReadOnlyDictionary<string,int> p){if(!Valid(f,p))throw new InvalidOperationException("Invalid double/half parameters");var(n,d)=Answer(f,p);return UaeG10TrigScalar.Rational(n,d);}
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? raw){
        if(!Valid(f,p))return false;
        if(f==Choice){
            double a=Math.Sqrt(2),lo=Math.Sqrt(2-a)/2,hi=Math.Sqrt(2+a)/2,rad=Math.PI/180;
            (double actual,double[] opts)=p["kind"] switch{0=>(Math.Sin(22.5*rad),new[]{lo,hi,-lo}),1=>(Math.Cos(22.5*rad),new[]{hi,lo,-hi}),2=>(Math.Cos(135*rad),new[]{-a/2,a/2,-1.0}),3=>(Math.Tan(Math.PI/8),new[]{a-1,a+1,1-a}),4=>(Math.Sin(112.5*rad),new[]{hi,-hi,lo}),_=>(Math.Cos(112.5*rad),new[]{-lo,lo,-hi})};
            var matches=Enumerable.Range(0,3).Where(i=>Math.Abs(actual-opts[i])<1e-10).ToArray();
            return matches.Length==1&&UaeG10TrigScalar.Matches(1+(matches[0]+3-p["shift"])%3,1,raw);
        }
        var(x,y,h)=UaeG10TrigAngleSumResponse.Triple(p["t"]);int q=p["q"];
        double theta=Math.Atan2(q<2?x:-x,q==0||q==3?y:-y);if(theta<0)theta+=2*Math.PI;
        long d=f==Tan?(long)Math.Round(Math.Cos(2*theta)*h*h):f==HalfSq?2*h:f==HalfTan?(long)Math.Round(h*(1+Math.Cos(theta))):h*h;
        double v=f==Sin?Math.Sin(2*theta):f==Cos?Math.Cos(2*theta):f==Tan?Math.Tan(2*theta):f==HalfSq?Math.Pow(p["fn"]==0?Math.Sin(theta/2):Math.Cos(theta/2),2):Math.Tan(theta/2);
        return d!=0&&UaeG10TrigScalar.Matches((long)Math.Round(v*d),d,raw);
    }
}

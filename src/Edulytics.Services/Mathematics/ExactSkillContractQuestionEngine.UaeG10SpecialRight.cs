using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
public sealed partial class ExactSkillContractQuestionEngine {
    private static ExactProblem BuildG10SpecialRight(string family,Random random,ExactSkillQuestionDifficulty difficulty){
        int a=random.Next(2,21),kind=random.Next(0,4),from=random.Next(0,3),to=(from+random.Next(1,3))%3,shift=random.Next(0,3);
        string[] labels=kind%2==0?new[]{"الساق الأولى","الساق الثانية","الوتر"}:new[]{"الضلع المقابل لـ30°","الضلع المقابل لـ60°","الوتر"};
        string prompt=family switch{
            UaeG10SpecialRightResponse.Length=>$"مثلث {(kind%2==0?"45°–45°–90°":"30°–60°–90°")}: طول {labels[from]} يساوي {a}. أوجد مربع طول {labels[to]} في صورة كسر أو عدد صحيح.",
            UaeG10SpecialRightResponse.Choice=>ChoicePrompt(a,kind,shift),
            UaeG10SpecialRightResponse.Equilateral=>$"مثلث متساوي الأضلاع طول ضلعه {a}. أوجد مربع ارتفاعه في صورة كسر أو عدد صحيح.",
            _=>$"مربع طول ضلعه {a}. أوجد مربع قطره."
        };
        bool rational=family==UaeG10SpecialRightResponse.Length||family==UaeG10SpecialRightResponse.Equilateral;
        return Problem(family,prompt,"حدد الضلع المقابل للزاوية واستعمل النسب الدقيقة أو فيثاغورس.",rational?AssessmentItemType.ShortAnswer:AssessmentItemType.Numeric,("a",a),("kind",kind),("from",from),("to",to),("shift",shift));
    }
    private static string ChoicePrompt(int a,int kind,int shift){
        var(q,options)=UaeG10SpecialRightResponse.Item(a,kind);
        return $"{q} اختر رقم القيمة الدقيقة: 1) {options[shift]}؛ 2) {options[(shift+1)%3]}؛ 3) {options[(shift+2)%3]}.";
    }
}
internal static class UaeG10SpecialRightResponse {
    internal const string Length="uae.g10.general.u14.l18.special_length_squared",Choice="uae.g10.general.u14.l18.special_exact_choice",Equilateral="uae.g10.general.u14.l18.equilateral_altitude_squared",Square="uae.g10.general.u14.l18.square_diagonal_squared";
    internal static bool Supports(string? f)=>f is Length or Choice or Equilateral or Square;
    private static bool Valid(string f,IReadOnlyDictionary<string,int> p)=>Supports(f)&&p.Count==5&&p.TryGetValue("a",out int a)&&a>=2&&a<=20&&p.TryGetValue("kind",out int k)&&k>=0&&k<4&&p.TryGetValue("from",out int x)&&x>=0&&x<3&&p.TryGetValue("to",out int y)&&y>=0&&y<3&&x!=y&&p.TryGetValue("shift",out int s)&&s>=0&&s<3;
    internal static (string,string[]) Item(int a,int k)=>k switch{
        0=>($"مثلث 45°–45°–90° ساقه {a}؛ أوجد الوتر.",new[]{$"{a}√2",$"{a}√3",$"{2*a}"}),
        1=>($"مثلث 30°–60°–90° ضلعه القصير {a}؛ أوجد الطويل.",new[]{$"{a}√3",$"{a}√2",$"{2*a}"}),
        2=>($"مثلث 45°–45°–90° وتره {a}√2؛ أوجد الساق.",new[]{$"{a}",$"{a}√2",$"{2*a}"}),
        _=>($"مثلث 30°–60°–90° ضلعه الطويل {a}√3؛ أوجد الوتر.",new[]{$"{2*a}",$"{a}√3",$"{a}"})
    };
    internal static string Format(string f,IReadOnlyDictionary<string,int> p){
        if(!Valid(f,p))throw new InvalidOperationException("Invalid special triangle parameters");
        long a=p["a"];int[] ratios=p["kind"]%2==0?new[]{1,1,2}:new[]{1,3,4};
        (long n,long d)=f switch{Length=>(a*a*ratios[p["to"]],ratios[p["from"]]),Choice=>(1+(3-p["shift"])%3,1),Equilateral=>(3*a*a,4),_=>(2*a*a,1)};
        return UaeG10TrigScalar.Rational(n,d);
    }
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? raw){
        if(!Valid(f,p))return false;
        long a=p["a"];int k=p["kind"];
        if(f==Choice){
            double x=Math.Sqrt(2),y=Math.Sqrt(3);
            (double actual,double[] opts)=k switch{0=>(a/Math.Sin(Math.PI/4),new[]{a*x,a*y,2.0*a}),1=>(a*Math.Tan(Math.PI/3),new[]{a*y,a*x,2.0*a}),2=>(a*x*Math.Sin(Math.PI/4),new[]{(double)a,a*x,2.0*a}),_=>(a*y/Math.Sin(Math.PI/3),new[]{2.0*a,a*y,(double)a})};
            var matches=Enumerable.Range(0,3).Where(i=>Math.Abs(actual-opts[i])<1e-8).ToArray();
            return matches.Length==1&&UaeG10TrigScalar.Matches(1+(matches[0]+3-p["shift"])%3,1,raw);
        }
        if(f==Square)return UaeG10TrigScalar.Matches((long)Math.Round(Math.Pow(a/Math.Cos(Math.PI/4),2)),1,raw);
        if(f==Equilateral)return UaeG10TrigScalar.Matches((long)Math.Round(4*Math.Pow(a*Math.Sin(Math.PI/3),2)),4,raw);
        double[] ratios=k%2==0?new[]{Math.Sin(Math.PI/4),Math.Cos(Math.PI/4),1.0}:new[]{Math.Sin(Math.PI/6),Math.Sin(Math.PI/3),1.0};
        long d=k%2==0?(p["from"]==2?2:1):p["from"]==0?1:p["from"]==1?3:4;
        long n=(long)Math.Round(d*Math.Pow(a*ratios[p["to"]]/ratios[p["from"]],2));
        return UaeG10TrigScalar.Matches(n,d,raw);
    }
}

using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
public sealed partial class ExactSkillContractQuestionEngine {
 private static ExactProblem BuildG10GeometricExponential(string family,Random random,ExactSkillQuestionDifficulty difficulty){
 int a=random.Next(2,11),r=random.Next(2,5),n=random.Next(2,7),j=random.Next(0,2),t=random.Next(0,4),delta=random.Next(1,4);
 int q=j==0?r:-r;
 long pow=UaeG10GeometricExponentialResponse.Pow(r,n-1),value=a*UaeG10GeometricExponentialResponse.Pow(r,t);
 string prompt=family switch{
 UaeG10GeometricExponentialResponse.Identify=>$"هل المتتالية {a}، {a*r}، {a*r*r+j} هندسية؟ اكتب 1 لنعم و0 للا.",
 UaeG10GeometricExponentialResponse.Ratio=>$"متتالية هندسية غير صفرية أول حدين فيها {a} و{a*q}. أوجد النسبة المشتركة.",
 UaeG10GeometricExponentialResponse.Signed=>$"متتالية هندسية a1={a}، r={q}. أوجد الحد رقم {n}.",
 UaeG10GeometricExponentialResponse.Decay=>$"متتالية هندسية a1={a}، r=1/{r}. أوجد الحد رقم {n} ككسر أو عدد صحيح.",
 UaeG10GeometricExponentialResponse.Initial=>$"متتالية هندسية r={r} وحدها رقم {n} يساوي {a*pow}. أوجد a1.",
 UaeG10GeometricExponentialResponse.Rate=>$"إذا f(t)={a}×{r}^t، أوجد متوسط معدل التغير من {t} إلى {t+delta} ككسر أو عدد صحيح.",
 _=>$"عدد ابتدائي {a} يتضاعف بمعامل {r} في كل فترة. كم العدد بعد {t} فترات؟"};
 bool rational=family is UaeG10GeometricExponentialResponse.Decay or UaeG10GeometricExponentialResponse.Rate or UaeG10GeometricExponentialResponse.Ratio;
 return Problem(family,prompt,"حدد الحد الأول والنسبة وفهرس البداية، أو فرق المخرجات على فرق الزمن.",rational?AssessmentItemType.ShortAnswer:AssessmentItemType.Numeric,("a",a),("r",r),("n",n),("j",j),("t",t),("delta",delta));}
}
internal static class UaeG10GeometricExponentialResponse {
 internal const string Identify="uae.g10.general.u17.l05.identify_geometric",Ratio="uae.g10.general.u17.l05.common_ratio",Signed="uae.g10.general.u17.l05.nth_term_signed",Decay="uae.g10.general.u17.l05.nth_term_decay",Initial="uae.g10.general.u17.l05.initial_term",Rate="uae.g10.general.u17.l05.average_rate",Growth="uae.g10.general.u17.l05.growth_model";
 internal static bool Supports(string? f)=>f is Identify or Ratio or Signed or Decay or Initial or Rate or Growth;
 internal static long Pow(int r,int n){long p=1;for(int i=0;i<n;i++)p*=r;return p;}
 private static bool Range(IReadOnlyDictionary<string,int> p,string k,int lo,int hi)=>p.TryGetValue(k,out int n)&&n>=lo&&n<=hi;
 private static bool Valid(string f,IReadOnlyDictionary<string,int> p)=>Supports(f)&&p.Count==6&&Range(p,"a",2,10)&&Range(p,"r",2,4)&&Range(p,"n",2,6)&&Range(p,"j",0,1)&&Range(p,"t",0,3)&&Range(p,"delta",1,3);
 internal static string Format(string f,IReadOnlyDictionary<string,int> p){if(!Valid(f,p))throw new InvalidOperationException("Invalid geometric exponential parameters");int a=p["a"],r=p["r"],n=p["n"],j=p["j"],t=p["t"],d=p["delta"],q=j==0?r:-r;var(x,y)=f switch{Identify=>(1L-j,1L),Ratio=>((long)q,1L),Signed=>(a*Pow(q,n-1),1L),Decay=>((long)a,Pow(r,n-1)),Initial=>((long)a,1L),Rate=>(a*(Pow(r,t+d)-Pow(r,t)),(long)d),_=>(a*Pow(r,t),1L)};return UaeG10TrigScalar.Rational(x,y);}
 internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? raw){
 if(!Valid(f,p))return false;
 int a=p["a"],r=p["r"],n=p["n"],j=p["j"],t=p["t"],d=p["delta"],q=j==0?r:-r;long z=a,den=1,start=a,end=a;
 for(int i=1;i<n;i++){z*=q;den*=r;}for(int i=0;i<t+d;i++){if(i<t)start*=r;end*=r;}
 var(x,y)=f switch{Identify=>((long)(a*(a*r*r+j)==(a*r)*(a*r)?1:0),1L),Ratio=>((long)(a*q), (long)a),Signed=>(z,1L),Decay=>((long)a,den),Initial=>((long)Math.Round(a*Math.Pow(r,n-1)/den),1L),Rate=>(end-start,(long)d),_=>(start,1L)};
 return UaeG10TrigScalar.Matches(x,y,raw);}
}

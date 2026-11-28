using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
public sealed partial class ExactSkillContractQuestionEngine {
 private static ExactProblem BuildG10SolidSimilarity(string family,Random random,ExactSkillQuestionDifficulty difficulty){
 int a=random.Next(2,10),b=random.Next(2,10),c=random.Next(2,10),u=random.Next(1,6),v=random.Next(1,6),n=random.Next(1,21),j=random.Next(0,2);
 string prompt=family switch{
 UaeG10SolidSimilarityResponse.Classify=>$"صندوقان أبعادهما {a*u}،{b*u}،{c*u} سم و{a*v}،{b*v}،{c*v+j} سم، بحسب التناظر المعطى. هل يتشابهان؟ اكتب 1 لنعم و0 للا.",
 UaeG10SolidSimilarityResponse.Scale=>$"مجسمان متشابهان؛ طول متناظر في الأول {a*u} سم وفي الثاني {a*v} سم. أوجد معامل الثاني للأول ككسر أو عدد صحيح.",
 UaeG10SolidSimilarityResponse.AreaRatio=>$"مجسمان متشابهان معامل طول الثاني للأول {v}/{u}. أوجد نسبة مساحة سطح الثاني للأول ككسر أو عدد صحيح.",
 UaeG10SolidSimilarityResponse.VolumeRatio=>$"مجسمان متشابهان معامل طول الثاني للأول {v}/{u}. أوجد نسبة حجم الثاني للأول ككسر أو عدد صحيح.",
 UaeG10SolidSimilarityResponse.Area=>$"مساحة سطح مجسم {n*u*u} سم² ومعامل طول مجسم مشابه له {v}/{u}. أوجد مساحة سطح الثاني بالسم².",
 UaeG10SolidSimilarityResponse.Volume=>$"حجم مجسم {n*u*u*u} سم³ ومعامل طول مجسم مشابه له {v}/{u}. أوجد حجم الثاني بالسم³.",
 UaeG10SolidSimilarityResponse.FromVolume=>$"مجسمان متشابهان حجماهما {n*u*u*u} و{n*v*v*v} سم³. أوجد معامل طول الثاني للأول ككسر أو عدد صحيح.",
 _=>$"مجسمان متشابهان مساحتا سطحيهما {n*u*u} و{n*v*v} سم². أوجد معامل طول الثاني للأول ككسر أو عدد صحيح."};
 return Problem(family,prompt,"حدد اتجاه النسبة؛ المساحة تتغير بالمربع والحجم بالمكعب.",family==UaeG10SolidSimilarityResponse.Classify?AssessmentItemType.Numeric:AssessmentItemType.ShortAnswer,("a",a),("b",b),("c",c),("u",u),("v",v),("n",n),("j",j));}
}
internal static class UaeG10SolidSimilarityResponse {
 internal const string Classify="uae.g10.general.u15.l08.box_similarity",Scale="uae.g10.general.u15.l08.scale_ratio",AreaRatio="uae.g10.general.u15.l08.surface_area_ratio",VolumeRatio="uae.g10.general.u15.l08.volume_ratio",Area="uae.g10.general.u15.l08.surface_area_scaled",Volume="uae.g10.general.u15.l08.volume_scaled",FromVolume="uae.g10.general.u15.l08.length_from_volume_ratio",FromArea="uae.g10.general.u15.l08.length_from_area_ratio";
 internal static bool Supports(string? f)=>f is Classify or Scale or AreaRatio or VolumeRatio or Area or Volume or FromVolume or FromArea;
 private static bool Range(IReadOnlyDictionary<string,int> p,string k,int lo,int hi)=>p.TryGetValue(k,out int n)&&n>=lo&&n<=hi;
 private static bool Valid(string f,IReadOnlyDictionary<string,int> p)=>Supports(f)&&p.Count==7&&Range(p,"a",2,9)&&Range(p,"b",2,9)&&Range(p,"c",2,9)&&Range(p,"u",1,5)&&Range(p,"v",1,5)&&Range(p,"n",1,20)&&Range(p,"j",0,1);
 internal static string Format(string f,IReadOnlyDictionary<string,int> p){if(!Valid(f,p))throw new InvalidOperationException("Invalid solid similarity parameters");long u=p["u"],v=p["v"],n=p["n"];var(x,y)=f switch{Classify=>(1L-p["j"],1L),AreaRatio=>(v*v,u*u),VolumeRatio=>(v*v*v,u*u*u),Area=>(n*v*v,1L),Volume=>(n*v*v*v,1L),_=>(v,u)};return UaeG10TrigScalar.Rational(x,y);}
 internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? raw){
 if(!Valid(f,p))return false;
 long a=p["a"],b=p["b"],c=p["c"],u=p["u"],v=p["v"],n=p["n"],j=p["j"];
 long s1=2*(a*u*b*u+b*u*c*u+c*u*a*u),s2=2*(a*v*b*v+b*v*c*v+c*v*a*v),v1=a*b*c*u*u*u,v2=a*b*c*v*v*v;
 var(x,y)=f switch{Classify=>(a*v*c*u==(c*v+j)*a*u?1L:0L,1L),AreaRatio=>(s2,s1),VolumeRatio=>(v2,v1),Area=>(n*u*u*s2/s1,1L),Volume=>(n*u*u*u*v2/v1,1L),FromVolume=>((long)Math.Round(Math.Cbrt(n*v*v*v)/(Math.Cbrt(n))), (long)Math.Round(Math.Cbrt(n*u*u*u)/Math.Cbrt(n))),FromArea=>((long)Math.Round(Math.Sqrt(n*v*v)/Math.Sqrt(n)),(long)Math.Round(Math.Sqrt(n*u*u)/Math.Sqrt(n))),_=>(a*v,a*u)};
 return UaeG10TrigScalar.Matches(x,y,raw);}
}

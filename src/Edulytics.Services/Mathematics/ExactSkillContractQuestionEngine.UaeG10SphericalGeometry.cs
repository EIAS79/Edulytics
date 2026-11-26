using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
public sealed partial class ExactSkillContractQuestionEngine {
 private static ExactProblem BuildG10SphericalGeometry(string family,Random random,ExactSkillQuestionDifficulty difficulty){
 int r=random.Next(2,31),d=random.Next(0,r),a=random.Next(1,181),kind=random.Next(0,2);
 string prompt=family switch{
 UaeG10SphericalGeometryResponse.Classify=>$"كرة نصف قطرها {r} سم، يبعد مستوى المقطع {d} سم عن المركز. هل دائرة المقطع دائرة كبرى؟ اكتب 1 لنعم و0 للا.",
 UaeG10SphericalGeometryResponse.Intersections=>$"كم نقطة تقاطع لدائرتين كبريين مختلفتين على كرة نصف قطرها {r} سم؟",
 UaeG10SphericalGeometryResponse.Unique=>$"على كرة نصف قطرها {r} سم، نقطتان مختلفتان {(kind==0?"غير متقابلتين قطبيًا":"متقابلتان قطبيًا")}. هل تمر بهما دائرة كبرى واحدة فقط؟ اكتب 1 لنعم و0 للا.",
 _=>$"كرة نصف قطرها {r} سم، زاوية مركزية للقوس الأقصر {a}°. أوجد معامل π في طول القوس بالسم في صورة كسر أو عدد صحيح."};
 return Problem(family,prompt,"حدد المقطع المركزي أو زاوية القوس ونصف قطر الكرة.",family==UaeG10SphericalGeometryResponse.Arc?AssessmentItemType.ShortAnswer:AssessmentItemType.Numeric,("r",r),("d",d),("a",a),("kind",kind));}
}
internal static class UaeG10SphericalGeometryResponse {
 internal const string Classify="uae.g10.general.u15.l07.great_circle_classification",Intersections="uae.g10.general.u15.l07.great_circle_intersections",Unique="uae.g10.general.u15.l07.great_circle_uniqueness",Arc="uae.g10.general.u15.l07.arc_length_pi_coefficient";
 internal static bool Supports(string? f)=>f is Classify or Intersections or Unique or Arc;
 private static bool Valid(string f,IReadOnlyDictionary<string,int> p)=>Supports(f)&&p.Count==4&&p.TryGetValue("r",out int r)&&r>=2&&r<=30&&p.TryGetValue("d",out int d)&&d>=0&&d<r&&p.TryGetValue("a",out int a)&&a>=1&&a<=180&&p.TryGetValue("kind",out int k)&&k>=0&&k<=1;
 internal static string Format(string f,IReadOnlyDictionary<string,int> p){if(!Valid(f,p))throw new InvalidOperationException("Invalid spherical geometry parameters");int n=f switch{Classify=>p["d"]==0?1:0,Intersections=>2,Unique=>1-p["kind"],_=>p["r"]*p["a"]};return UaeG10TrigScalar.Rational(n,f==Arc?180:1);}
 internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? raw){
 if(!Valid(f,p))return false;
 int r=p["r"],d=p["d"],a=p["a"],k=p["kind"];
 long n=f switch{Classify=>r*r-d*d==r*r?1:0,Intersections=>new[]{-r,r}.Distinct().Count(),Unique=>k==0?1:0,_=>2L*r*a};
 return UaeG10TrigScalar.Matches(n,f==Arc?360:1,raw);}
}

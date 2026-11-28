using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
public sealed partial class ExactSkillContractQuestionEngine {
    private static ExactProblem BuildG10SolidSurface(string family,Random random,ExactSkillQuestionDifficulty difficulty){
        int a=random.Next(2,13),b=random.Next(2,13),h=random.Next(2,13);
        string prompt=family switch{
        UaeG10SolidSurfaceResponse.PrismL=>$"منشور قائم قاعدته مستطيل {a}×{b} سم وارتفاعه {h} سم. أوجد المساحة الجانبية بالسم².",
        UaeG10SolidSurfaceResponse.PrismS=>$"منشور قائم قاعدته مستطيل {a}×{b} سم وارتفاعه {h} سم. أوجد مساحة السطح الكلية بالسم².",
        UaeG10SolidSurfaceResponse.CylL=>$"أسطوانة قائمة نصف قطرها {a} سم وارتفاعها {h} سم. أوجد معامل π في المساحة الجانبية بالسم².",
        UaeG10SolidSurfaceResponse.CylS=>$"أسطوانة قائمة نصف قطرها {a} سم وارتفاعها {h} سم. أوجد معامل π في مساحة السطح الكلية بالسم².",
        UaeG10SolidSurfaceResponse.CylH=>$"أسطوانة قائمة نصف قطرها {a} سم ومساحتها الجانبية {2*a*h}π سم². أوجد ارتفاعها بالسم.",
        _=>$"أسطوانة قائمة مفتوحة من الأعلى ولها قاع؛ نصف قطرها {a} سم وارتفاعها {h} سم. أوجد معامل π في مساحة السطح بالسم²."};
        return Problem(family,prompt,"حدد المساحة الجانبية وعدد القواعد المطلوبة، ثم عوض بالأبعاد.",AssessmentItemType.Numeric,("a",a),("b",b),("h",h));
    }
}
internal static class UaeG10SolidSurfaceResponse {
    internal const string PrismL="uae.g10.general.u15.l02.rectangular_prism_lateral",PrismS="uae.g10.general.u15.l02.rectangular_prism_total",CylL="uae.g10.general.u15.l02.cylinder_lateral_pi_coefficient",CylS="uae.g10.general.u15.l02.cylinder_total_pi_coefficient",CylH="uae.g10.general.u15.l02.cylinder_height_from_lateral",CylOpen="uae.g10.general.u15.l02.open_cylinder_surface_pi_coefficient";
    internal static bool Supports(string? f)=>f is PrismL or PrismS or CylL or CylS or CylH or CylOpen;
    private static bool Valid(string f,IReadOnlyDictionary<string,int> p)=>Supports(f)&&p.Count==3&&p.TryGetValue("a",out int a)&&a>=2&&a<=12&&p.TryGetValue("b",out int b)&&b>=2&&b<=12&&p.TryGetValue("h",out int h)&&h>=2&&h<=12;
    internal static string Format(string f,IReadOnlyDictionary<string,int> p){
        if(!Valid(f,p))throw new InvalidOperationException("Invalid solid surface parameters");
        int a=p["a"],b=p["b"],h=p["h"];
        int n=f switch{PrismL=>2*(a+b)*h,PrismS=>2*(a*b+a*h+b*h),CylL=>2*a*h,CylS=>2*a*(a+h),CylH=>h,_=>a*(a+2*h)};
        return UaeG10TrigScalar.Rational(n,1);
    }
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? raw){
        if(!Valid(f,p))return false;
        int a=p["a"],b=p["b"],h=p["h"];
        int strip=new[]{a,b,a,b}.Select(w=>w*h).Sum(),ring=(a+a)*h,disk=a*a;
        int n=f switch{PrismL=>strip,PrismS=>strip+2*a*b,CylL=>ring,CylS=>ring+disk+disk,CylH=>ring/(a+a),_=>ring+disk};
        return UaeG10TrigScalar.Matches(n,1,raw);
    }
}

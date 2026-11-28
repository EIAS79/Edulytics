using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
public sealed partial class ExactSkillContractQuestionEngine {
    private static ExactProblem BuildG10PrismVolume(string family,Random random,ExactSkillQuestionDifficulty difficulty){
        int a=random.Next(2,13),b=random.Next(2,13),h=random.Next(2,13);
        string prompt=family switch{
        UaeG10PrismVolumeResponse.Tri=>$"منشور قائم قاعدته مثلث قائم ساقاه {2*a} و{b} سم، وارتفاع المنشور {h} سم. أوجد الحجم بالسم³.",
        UaeG10PrismVolumeResponse.Oblique=>$"منشور مائل مساحة قاعدته {a*b} سم²، ارتفاعه العمودي {h} سم، وحرفه الجانبي أطول. أوجد الحجم بالسم³.",
        UaeG10PrismVolumeResponse.Cyl=>$"أسطوانة دائرية قطرها {2*a} سم وارتفاعها العمودي {h} سم. أوجد معامل π في حجمها بالسم³.",
        UaeG10PrismVolumeResponse.Height=>$"أسطوانة حجمها {a*a*h}π سم³ ونصف قطرها {a} سم. أوجد ارتفاعها بالسم.",
        UaeG10PrismVolumeResponse.Radius=>$"أسطوانة حجمها {a*a*h}π سم³ وارتفاعها {h} سم. أوجد نصف قطرها بالسم.",
        UaeG10PrismVolumeResponse.Equal=>$"منشور قائم مساحة قاعدته {2*a*b} سم² وارتفاعه {h} سم. منشور آخر قاعدته {a*b} سم² وله الحجم نفسه؛ أوجد ارتفاعه بالسم.",
        _=>$"صندوق أبعاده {2*a} و{2*b} و{2*h} سم، أزيل منه جزء داخلي مستطيل أبعاده {a} و{b} و{h} سم. أوجد الحجم المتبقي بالسم³."};
        return Problem(family,prompt,"حدد مساحة القاعدة والارتفاع العمودي أو اطرح الحجم المزال.",AssessmentItemType.Numeric,("a",a),("b",b),("h",h));
    }
}
internal static class UaeG10PrismVolumeResponse {
    internal const string Tri="uae.g10.general.u15.l05.triangular_prism_volume",Oblique="uae.g10.general.u15.l05.oblique_prism_volume",Cyl="uae.g10.general.u15.l05.cylinder_volume_pi_coefficient",Height="uae.g10.general.u15.l05.cylinder_height",Radius="uae.g10.general.u15.l05.cylinder_radius",Equal="uae.g10.general.u15.l05.equal_volume_prism_height",Removed="uae.g10.general.u15.l05.removed_rectangular_volume";
    internal static bool Supports(string? f)=>f is Tri or Oblique or Cyl or Height or Radius or Equal or Removed;
    private static bool Valid(string f,IReadOnlyDictionary<string,int> p)=>Supports(f)&&p.Count==3&&p.TryGetValue("a",out int a)&&a>=2&&a<=12&&p.TryGetValue("b",out int b)&&b>=2&&b<=12&&p.TryGetValue("h",out int h)&&h>=2&&h<=12;
    internal static string Format(string f,IReadOnlyDictionary<string,int> p){
        if(!Valid(f,p))throw new InvalidOperationException("Invalid volume parameters");
        int a=p["a"],b=p["b"],h=p["h"],n=f switch{Tri or Oblique=>a*b*h,Cyl=>a*a*h,Height=>h,Radius=>a,Equal=>2*h,_=>7*a*b*h};
        return UaeG10TrigScalar.Rational(n,1);
    }
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? raw){
        if(!Valid(f,p))return false;
        int a=p["a"],b=p["b"],h=p["h"],n=f switch{Tri=>2*a*b*h/2,Oblique=>Enumerable.Repeat(a*b,h).Sum(),Cyl=>Enumerable.Repeat(a*a,h).Sum(),Height=>a*a*h/(a*a),Radius=>(int)Math.Sqrt(a*a*h/h),Equal=>(2*a*b*h)/(a*b),_=>(2*a)*(2*b)*(2*h)-a*b*h};
        return UaeG10TrigScalar.Matches(n,1,raw);
    }
}

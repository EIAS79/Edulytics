using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
public sealed partial class ExactSkillContractQuestionEngine {
    private static ExactProblem BuildG10PyramidVolume(string family,Random random,ExactSkillQuestionDifficulty difficulty){
        int a=random.Next(2,13),b=random.Next(2,13),h=random.Next(2,13),k=random.Next(1,9),j=random.Next(0,2);
        int r=(j==0?3:5)*k,v=(j==0?4:12)*k,l=(j==0?5:13)*k;
        string prompt=family switch{
        UaeG10PyramidVolumeResponse.Pyr=>$"هرم قاعدته مستطيل {a}×{b} سم وارتفاعه العمودي {h} سم. أوجد حجمه بالسم³ ككسر دقيق أو عدد صحيح.",
        UaeG10PyramidVolumeResponse.Cone=>$"مخروط دائري نصف قطره {a} سم وارتفاعه العمودي {h} سم. أوجد معامل π في حجمه بالسم³ ككسر دقيق أو عدد صحيح.",
        UaeG10PyramidVolumeResponse.ConeSlant=>$"مخروط قائم نصف قطره {r} سم وارتفاعه المائل {l} سم. أوجد معامل π في حجمه بالسم³ ككسر دقيق أو عدد صحيح.",
        UaeG10PyramidVolumeResponse.PyrSlant=>$"هرم مربع منتظم ضلع قاعدته {2*r} سم وارتفاع وجهه المائل {l} سم. أوجد حجمه بالسم³ ككسر دقيق أو عدد صحيح.",
        UaeG10PyramidVolumeResponse.Height=>$"مخروط حجمه ({a*a*h}/3)π سم³ ونصف قطره {a} سم. أوجد ارتفاعه العمودي بالسم.",
        UaeG10PyramidVolumeResponse.Radius=>$"مخروط حجمه ({a*a*h}/3)π سم³ وارتفاعه العمودي {h} سم. أوجد نصف قطره بالسم.",
        _=>$"أسطوانة نصف قطرها {a} وارتفاعها {b} سم تعلوها قمة مخروطية نصف قطرها {a} وارتفاعها العمودي {h} سم. أوجد معامل π في الحجم الكلي بالسم³ ككسر دقيق أو عدد صحيح."};
        return Problem(family,prompt,"حدد الارتفاع العمودي ومساحة القاعدة وعامل الثلث.",family is UaeG10PyramidVolumeResponse.Height or UaeG10PyramidVolumeResponse.Radius?AssessmentItemType.Numeric:AssessmentItemType.ShortAnswer,("a",a),("b",b),("h",h),("k",k),("j",j));
    }
}
internal static class UaeG10PyramidVolumeResponse {
    internal const string Pyr="uae.g10.general.u15.l06.rectangular_pyramid_volume",Cone="uae.g10.general.u15.l06.cone_volume_pi_coefficient",ConeSlant="uae.g10.general.u15.l06.cone_volume_from_slant_pi_coefficient",PyrSlant="uae.g10.general.u15.l06.square_pyramid_volume_from_slant",Height="uae.g10.general.u15.l06.cone_height",Radius="uae.g10.general.u15.l06.cone_radius",Composite="uae.g10.general.u15.l06.composite_cylinder_cone_pi_coefficient";
    internal static bool Supports(string? f)=>f is Pyr or Cone or ConeSlant or PyrSlant or Height or Radius or Composite;
    private static bool Valid(string f,IReadOnlyDictionary<string,int> p)=>Supports(f)&&p.Count==5&&p.TryGetValue("a",out int a)&&a>=2&&a<=12&&p.TryGetValue("b",out int b)&&b>=2&&b<=12&&p.TryGetValue("h",out int h)&&h>=2&&h<=12&&p.TryGetValue("k",out int k)&&k>=1&&k<=8&&p.TryGetValue("j",out int j)&&j>=0&&j<=1;
    internal static string Format(string f,IReadOnlyDictionary<string,int> p){
        if(!Valid(f,p))throw new InvalidOperationException("Invalid pyramid volume parameters");
        int a=p["a"],b=p["b"],h=p["h"],k=p["k"],j=p["j"],r=(j==0?3:5)*k,v=(j==0?4:12)*k;
        int n=f switch{Pyr=>a*b*h,Cone=>a*a*h,ConeSlant=>r*r*v,PyrSlant=>4*r*r*v,Height=>h,Radius=>a,_=>a*a*(3*b+h)};
        return UaeG10TrigScalar.Rational(n,f is Height or Radius?1:3);
    }
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? raw){
        if(!Valid(f,p))return false;
        int a=p["a"],b=p["b"],h=p["h"],k=p["k"],j=p["j"],r=(j==0?3:5)*k,l=(j==0?5:13)*k,v=(int)Math.Sqrt(l*l-r*r),baseArea=a*a,threeV=baseArea*h;
        int n=f switch{Pyr=>Enumerable.Repeat(a*b,h).Sum(),Cone=>threeV,ConeSlant=>r*r*v,PyrSlant=>(r+r)*(r+r)*v,Height=>threeV/baseArea,Radius=>(int)Math.Sqrt(threeV/h),_=>3*baseArea*b+threeV};
        return UaeG10TrigScalar.Matches(n,f is Height or Radius?1:3,raw);
    }
}

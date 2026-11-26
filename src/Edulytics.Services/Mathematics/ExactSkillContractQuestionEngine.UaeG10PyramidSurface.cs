using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
public sealed partial class ExactSkillContractQuestionEngine {
    private static ExactProblem BuildG10PyramidSurface(string family,Random random,ExactSkillQuestionDifficulty difficulty){
        int k=random.Next(1,9),j=random.Next(0,2);
        int a=(j==0?3:5)*k,h=(j==0?4:12)*k,l=(j==0?5:13)*k;
        string prompt=family switch{
        UaeG10PyramidSurfaceResponse.PyrL=>$"هرم مربع منتظم طول ضلع قاعدته {2*a} سم وارتفاع وجهه المائل {l} سم. أوجد المساحة الجانبية بالسم².",
        UaeG10PyramidSurfaceResponse.PyrS=>$"هرم مربع منتظم طول ضلع قاعدته {2*a} سم وارتفاعه العمودي {h} سم. أوجد مساحة السطح الكلية بالسم².",
        UaeG10PyramidSurfaceResponse.PyrSlant=>$"هرم مربع منتظم طول ضلع قاعدته {2*a} سم وارتفاعه العمودي {h} سم. أوجد ارتفاع الوجه المائل بالسم.",
        UaeG10PyramidSurfaceResponse.ConeL=>$"مخروط دائري قائم نصف قطره {a} سم وارتفاعه المائل {l} سم. أوجد معامل π في المساحة الجانبية بالسم².",
        UaeG10PyramidSurfaceResponse.ConeS=>$"مخروط دائري قائم نصف قطره {a} سم وارتفاعه العمودي {h} سم. أوجد معامل π في مساحة السطح الكلية بالسم².",
        _=>$"مخروط قائم نصف قطر قاعدته {k} سم وارتفاعه المائل {3*k} سم. أوجد زاوية قطاع شبكته بالدرجات."};
        return Problem(family,prompt,"حدد الارتفاع المائل ومساحة القاعدة، أو استخدم نسبة طول القوس.",AssessmentItemType.Numeric,("k",k),("j",j));
    }
}
internal static class UaeG10PyramidSurfaceResponse {
    internal const string PyrL="uae.g10.general.u15.l03.square_pyramid_lateral",PyrS="uae.g10.general.u15.l03.square_pyramid_total",PyrSlant="uae.g10.general.u15.l03.square_pyramid_slant",ConeL="uae.g10.general.u15.l03.cone_lateral_pi_coefficient",ConeS="uae.g10.general.u15.l03.cone_total_pi_coefficient",Sector="uae.g10.general.u15.l03.cone_sector_angle";
    internal static bool Supports(string? f)=>f is PyrL or PyrS or PyrSlant or ConeL or ConeS or Sector;
    private static bool Valid(string f,IReadOnlyDictionary<string,int> p)=>Supports(f)&&p.Count==2&&p.TryGetValue("k",out int k)&&k>=1&&k<=8&&p.TryGetValue("j",out int j)&&j>=0&&j<=1;
    internal static string Format(string f,IReadOnlyDictionary<string,int> p){
        if(!Valid(f,p))throw new InvalidOperationException("Invalid pyramid surface parameters");
        int k=p["k"],j=p["j"],a=(j==0?3:5)*k,l=(j==0?5:13)*k;
        int n=f switch{PyrL=>4*a*l,PyrS=>4*a*(a+l),PyrSlant=>l,ConeL=>a*l,ConeS=>a*(a+l),_=>120};
        return UaeG10TrigScalar.Rational(n,1);
    }
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? raw){
        if(!Valid(f,p))return false;
        int k=p["k"],j=p["j"],a=(j==0?3:5)*k,h=(j==0?4:12)*k,l=(int)Math.Sqrt(a*a+h*h),s=2*a,faces=4*(s*l/2);
        int n=f switch{PyrL=>faces,PyrS=>faces+s*s,PyrSlant=>l,ConeL=>a*l,ConeS=>a*l+a*a,_=>360*k/(3*k)};
        return UaeG10TrigScalar.Matches(n,1,raw);
    }
}

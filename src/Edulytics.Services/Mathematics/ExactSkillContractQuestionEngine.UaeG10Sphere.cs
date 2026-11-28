using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
public sealed partial class ExactSkillContractQuestionEngine {
    private static ExactProblem BuildG10Sphere(string family,Random random,ExactSkillQuestionDifficulty difficulty){
        int r=random.Next(2,13);
        string prompt=family switch{
        UaeG10SphereResponse.Surface=>$"كرة نصف قطرها {r} سم. أوجد معامل π في مساحة سطحها بالسم².",
        UaeG10SphereResponse.Volume=>$"كرة نصف قطرها {r} سم. أوجد معامل π في حجمها بالسم³؛ اكتب كسرًا دقيقًا أو عددًا صحيحًا.",
        UaeG10SphereResponse.HemiS=>$"نصف كرة مغلق بقرص مستوٍ نصف قطره {r} سم. أوجد معامل π في المساحة الكلية بالسم².",
        UaeG10SphereResponse.HemiV=>$"نصف كرة نصف قطره {r} سم. أوجد معامل π في حجمه بالسم³؛ اكتب كسرًا دقيقًا أو عددًا صحيحًا.",
        UaeG10SphereResponse.RadiusS=>$"كرة مساحة سطحها {4*r*r}π سم². أوجد نصف قطرها بالسم.",
        UaeG10SphereResponse.RadiusV=>$"كرة حجمها ({4*r*r*r}/3)π سم³. أوجد نصف قطرها بالسم.",
        _=>$"محيط دائرة عظمى لكرة {2*r}π سم. أوجد نصف قطر الكرة بالسم."};
        return Problem(family,prompt,"حدد نصف القطر ونوع السطح أو الحجم، واعزل المجهول إن لزم.",family is UaeG10SphereResponse.Volume or UaeG10SphereResponse.HemiV?AssessmentItemType.ShortAnswer:AssessmentItemType.Numeric,("r",r));
    }
}
internal static class UaeG10SphereResponse {
    internal const string Surface="uae.g10.general.u15.l04.surface_pi_coefficient",Volume="uae.g10.general.u15.l04.volume_pi_coefficient",HemiS="uae.g10.general.u15.l04.hemisphere_total_surface_pi_coefficient",HemiV="uae.g10.general.u15.l04.hemisphere_volume_pi_coefficient",RadiusS="uae.g10.general.u15.l04.radius_from_surface",RadiusV="uae.g10.general.u15.l04.radius_from_volume",RadiusC="uae.g10.general.u15.l04.radius_from_great_circle";
    internal static bool Supports(string? f)=>f is Surface or Volume or HemiS or HemiV or RadiusS or RadiusV or RadiusC;
    private static bool Valid(string f,IReadOnlyDictionary<string,int> p)=>Supports(f)&&p.Count==1&&p.TryGetValue("r",out int r)&&r>=2&&r<=12;
    internal static string Format(string f,IReadOnlyDictionary<string,int> p){
        if(!Valid(f,p))throw new InvalidOperationException("Invalid sphere parameters");
        int r=p["r"],n=f switch{Surface=>4*r*r,Volume=>4*r*r*r,HemiS=>3*r*r,HemiV=>2*r*r*r,_=>r};
        return UaeG10TrigScalar.Rational(n,f is Volume or HemiV?3:1);
    }
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? raw){
        if(!Valid(f,p))return false;
        int r=p["r"],disk=r*r,area=disk+disk+disk+disk,vol=area*r;
        int n=f switch{Surface=>area,Volume=>vol,HemiS=>area/2+disk,HemiV=>vol/2,RadiusS=>(int)Math.Sqrt(area/4),RadiusV=>(int)Math.Round(Math.Cbrt(vol/4)),_ => (r+r)/2};
        return UaeG10TrigScalar.Matches(n,f is Volume or HemiV?3:1,raw);
    }
}

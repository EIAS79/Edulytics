using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
public sealed partial class ExactSkillContractQuestionEngine {
    private static ExactProblem BuildG10Elevation(string family,Random random,ExactSkillQuestionDifficulty difficulty){
        int a=random.Next(5,41),b=random.Next(5,31),k=random.Next(0,6),t=random.Next(0,4);
        int alpha=20+5*k,beta=50+5*t,eye=12+t*2;
        string prompt=family switch{
            UaeG10ElevationResponse.Eye=>$"راصد يبعد أفقيًا {a} مترًا عن برج؛ ارتفاع عينه {eye}/10 متر وزاوية ارتفاع القمة {alpha}°. أوجد ارتفاع البرج بالمتر لأقرب جزء من مئة.",
            UaeG10ElevationResponse.Two=>$"نقطتا رصد في الجهة نفسها وعلى مستوى أفقي واحد تبعدان {b} مترًا؛ زاوية ارتفاع الهدف من البعيدة {alpha}° ومن القريبة {beta}°. أوجد ارتفاع الهدف فوق مستوى العين بالمتر لأقرب جزء من مئة.",
            UaeG10ElevationResponse.Sight=>$"فرق الارتفاع {a} متر وزاوية الارتفاع {alpha}°. أوجد طول خط النظر بالمتر لأقرب جزء من مئة.",
            _=>$"فرق الارتفاع {a} متر والمسافة الأفقية {b} متر. أوجد زاوية الارتفاع بالدرجات لأقرب جزء من مئة."
        };
        return Problem(family,prompt,"حدد الأفقي والفرق الرأسي وخط النظر، ثم استخدم النسبة المناسبة.",AssessmentItemType.Numeric,("a",a),("b",b),("k",k),("t",t));
    }
}
internal static class UaeG10ElevationResponse {
    internal const string Eye="uae.g10.general.u14.l17.height_with_eye_rounded",Two="uae.g10.general.u14.l17.two_station_height_rounded",Sight="uae.g10.general.u14.l17.line_of_sight_rounded",Angle="uae.g10.general.u14.l17.angle_from_rise_run_rounded";
    internal static bool Supports(string? f)=>f is Eye or Two or Sight or Angle;
    private static bool Valid(string f,IReadOnlyDictionary<string,int> p)=>Supports(f)&&p.Count==4&&p.TryGetValue("a",out int a)&&a>=5&&a<=40&&p.TryGetValue("b",out int b)&&b>=5&&b<=30&&p.TryGetValue("k",out int k)&&k>=0&&k<6&&p.TryGetValue("t",out int t)&&t>=0&&t<4;
    internal static string Format(string f,IReadOnlyDictionary<string,int> p){
        if(!Valid(f,p))throw new InvalidOperationException("Invalid elevation parameters");
        double a=p["a"],b=p["b"],alpha=(20+5*p["k"])*Math.PI/180,beta=(50+5*p["t"])*Math.PI/180,eye=(12+2*p["t"])/10.0;
        double x=Math.Tan(alpha),y=Math.Tan(beta);
        return UaeG10TrigScalar.Rounded(f==Eye?eye+a*x:f==Two?b*x*y/(y-x):f==Sight?a/Math.Sin(alpha):Math.Atan(a/b)*180/Math.PI);
    }
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? raw){
        if(!Valid(f,p))return false;
        double a=p["a"],b=p["b"],alpha=(20+5*p["k"])*Math.PI/180,beta=(50+5*p["t"])*Math.PI/180,eye=(12+2*p["t"])/10.0;
        double v=f==Eye?eye+a*Math.Sin(alpha)/Math.Cos(alpha):f==Two?b*Math.Sin(alpha)*Math.Sin(beta)/Math.Sin(beta-alpha):f==Sight?Math.Sqrt(a*a+Math.Pow(a*Math.Cos(alpha)/Math.Sin(alpha),2)):Math.Acos(b/Math.Sqrt(a*a+b*b))*180/Math.PI;
        return UaeG10TrigScalar.MatchesRounded(v,raw);
    }
}

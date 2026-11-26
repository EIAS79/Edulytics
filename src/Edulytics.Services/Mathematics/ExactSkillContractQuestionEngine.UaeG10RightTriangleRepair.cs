using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
public sealed partial class ExactSkillContractQuestionEngine {
    private static ExactProblem BuildG10RightTriangleRepair(string family,Random random,ExactSkillQuestionDifficulty difficulty){
        int a=random.Next(2,13),b=random.Next(2,13),k=random.Next(0,6),t=random.Next(0,4);
        var(x,y,h)=UaeG10RightTriangleRepairResponse.Triple(t);
        var(A,B,C)=UaeG10RightTriangleRepairResponse.Sides(a,k,t);
        string prompt=family switch{
            UaeG10RightTriangleRepairResponse.Mean=>$"أوجد الوسط الهندسي للعددين {a} و {b} لأقرب جزء من مئة.",
            UaeG10RightTriangleRepairResponse.Altitude=>$"ارتفاع من الرأس القائم يقسم الوتر إلى {a} و {b}. أوجد مربع طول الارتفاع.",
            UaeG10RightTriangleRepairResponse.Leg=>$"وتر مثلث قائم مقسوم إلى {a} و {b}. أوجد مربع طول الساق التي إسقاطها {a}.",
            UaeG10RightTriangleRepairResponse.Projection=>$"مثلث قائم وتره {h*a} وطول ساقه {x*a}. أوجد طول إسقاط هذه الساق على الوتر في صورة كسر أو عدد صحيح.",
            UaeG10RightTriangleRepairResponse.Classify=>$"صنّف الأطوال {A}، {B}، {C}: اكتب 0 إذا لا تشكل مثلثًا، 1 حاد، 2 قائم، 3 منفرج.",
            _=>$"أوجد مربع المسافة بين P(−{a},{t}) و Q({b},−{k})."
        };
        return Problem(family,prompt,"تحقق من الأضلاع والزاوية القائمة، ثم استعمل العلاقة المناسبة.",family==UaeG10RightTriangleRepairResponse.Projection?AssessmentItemType.ShortAnswer:AssessmentItemType.Numeric,("a",a),("b",b),("k",k),("t",t));
    }
}
internal static class UaeG10RightTriangleRepairResponse {
    internal const string Mean="uae.g10.general.u14.l15.mean_rounded",Altitude="uae.g10.general.u14.l15.altitude_squared",Leg="uae.g10.general.u14.l15.leg_squared",Projection="uae.g10.general.u14.l15.projection_from_leg",Classify="uae.g10.general.u14.l16.triangle_classification",Distance="uae.g10.general.u14.l16.coordinate_distance_squared";
    internal static bool Supports(string? f)=>f is Mean or Altitude or Leg or Projection or Classify or Distance;
    internal static (long,long,long) Triple(int t)=>t switch{0=>(3,4,5),1=>(5,12,13),2=>(8,15,17),_=>(7,24,25)};
    internal static (long,long,long) Sides(int a,int k,int t){var(x,y,h)=Triple(t);return (a*x,a*y,a*(k%4==0?x+y:k%4==1?h-1:k%4==2?h:h+1));}
    private static bool Valid(string f,IReadOnlyDictionary<string,int> p)=>Supports(f)&&p.Count==4&&p.TryGetValue("a",out int a)&&a>=2&&a<=12&&p.TryGetValue("b",out int b)&&b>=2&&b<=12&&p.TryGetValue("k",out int k)&&k>=0&&k<6&&p.TryGetValue("t",out int t)&&t>=0&&t<4;
    internal static string Format(string f,IReadOnlyDictionary<string,int> p){
        if(!Valid(f,p))throw new InvalidOperationException("Invalid right triangle parameters");
        long a=p["a"],b=p["b"],k=p["k"],t=p["t"];
        if(f==Mean)return UaeG10TrigScalar.Rounded(Math.Sqrt(a*b));
        var(x,y,h)=Triple((int)t);
        (long n,long d)=f switch{Altitude=>(a*b,1L),Leg=>(a*(a+b),1L),Projection=>(a*x*x,h),Classify=>(k%4,1L),_=>((a+b)*(a+b)+(t+k)*(t+k),1L)};
        return UaeG10TrigScalar.Rational(n,d);
    }
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? raw){
        if(!Valid(f,p))return false;
        long a=p["a"],b=p["b"],k=p["k"],t=p["t"];
        if(f==Mean)return UaeG10TrigScalar.MatchesRounded(Math.Sqrt(a)*Math.Sqrt(b),raw);
        var(x,y,h)=Triple((int)t);
        if(f==Classify){
            var(A,B,C)=Sides((int)a,(int)k,(int)t);var sides=new[]{A,B,C}.OrderBy(v=>v).ToArray();
            if(sides[0]+sides[1]<=sides[2])return UaeG10TrigScalar.Matches(0,1,raw);
            long delta=sides[0]*sides[0]+sides[1]*sides[1]-sides[2]*sides[2];
            return UaeG10TrigScalar.Matches(delta>0?1:delta==0?2:3,1,raw);
        }
        // Coordinates A=(0,0), B=(a+b,0), C=(a,sqrt(ab)) independently establish the altitude and leg lengths.
        double height=Math.Sqrt(a*b);
        (long n,long d)=f switch{Altitude=>((long)Math.Round(height*height),1L),Leg=>((long)Math.Round(Math.Pow(a,2)+height*height),1L),Projection=>(a*(h*h-y*y),h),_=>((long)Math.Round(Math.Pow(b-(-a),2)+Math.Pow(-k-t,2)),1L)};
        return UaeG10TrigScalar.Matches(n,d,raw);
    }
}

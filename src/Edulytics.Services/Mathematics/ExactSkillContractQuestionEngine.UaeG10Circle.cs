using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

// Source-specific families extend the existing ExactSkillContractQuestionEngine.
public sealed partial class ExactSkillContractQuestionEngine
{
    private static ExactProblem BuildG10Circle(string family,Random random,ExactSkillQuestionDifficulty difficulty)
    {
        if(UaeG10CircleResponse.IsArc(family))
        {
            int a1=random.Next(10,171),a2=random.Next(10,171),r=random.Next(2,31),angle=random.Next(10,351),percent=5*random.Next(1,20);
            string prompt=family switch
            {
                UaeG10CircleResponse.Central=>$"ثلاث زوايا مركزية غير متداخلة تكمل دورة. قياس اثنتين {a1}° و{a2}°. أوجد الثالثة بالدرجات.",
                UaeG10CircleResponse.Major=>$"قوس صغير قياسه {a1}°. أوجد قياس القوس الكبير المتمم للدورة بالدرجات.",
                UaeG10CircleResponse.Pie=>$"قطاع يمثل {percent}٪ من بيانات مخطط دائري. أوجد قياس زاويته المركزية بالدرجات.",
                UaeG10CircleResponse.ArcSum=>$"قوسان متجاوران غير متداخلين قياساهما {a1}° و{a2}°. أوجد قياس القوس المكون منهما.",
                UaeG10CircleResponse.ArcCoeff=>$"دائرة نصف قطرها {r} سم، وقوس قياسه {angle}°. اكتب معامل π في طول القوس (عددًا أو كسرًا).",
                UaeG10CircleResponse.ArcApprox=>$"دائرة نصف قطرها {r} سم، وقوس قياسه {angle}°. أوجد طول القوس بالسنتيمتر مقربًا لأقرب جزء من مائة.",
                _=>$"قوس طوله {percent}/100 من محيط الدائرة. أوجد قياس القوس بالدرجات."
            };
            return Problem(family,prompt,"مجموع دورة كاملة 360°؛ طول القوس = (قياسه/360)×2π×نصف القطر، والقياس الزاوي يختلف عن الطول.",family is UaeG10CircleResponse.ArcCoeff or UaeG10CircleResponse.ArcApprox?AssessmentItemType.ShortAnswer:AssessmentItemType.Numeric,("a1",a1),("a2",a2),("r",r),("angle",angle),("percent",percent));
        }
        int a=random.Next(2,31),b=random.Next(2,31),distance=random.Next(0,61),k=random.Next(1,11);
        if(family==UaeG10CircleResponse.Intersection&&distance==0&&a==b)distance=1;
        if(family==UaeG10CircleResponse.Overlap)distance=random.Next(Math.Abs(a-b)+1,a+b);
        string prompt=family switch
        {
            UaeG10CircleResponse.Diameter=>$"نصف قطر دائرة {a} سم. أوجد قطرها بالسنتيمتر.",
            UaeG10CircleResponse.Radius=>$"قطر دائرة {a} سم. أوجد نصف قطرها، واكتب عددًا صحيحًا أو كسرًا.",
            UaeG10CircleResponse.Circumference=>$"نصف قطر دائرة {a} سم. إذا كان محيطها م×π سم، فأوجد المعامل م.",
            UaeG10CircleResponse.Approx=>$"قطر دائرة {a} سم. أوجد محيطها بالسنتيمتر باستخدام π، مقربًا إلى أقرب جزء من مائة.",
            UaeG10CircleResponse.Overlap=>$"دائرتان نصفا قطريهما {a} و{b} سم والمسافة بين مركزيهما {distance} سم. أوجد طول الجزء المشترك من خط المركزين بالسنتيمتر.",
            UaeG10CircleResponse.Intersection=>$"دائرتان مختلفتان نصفا قطريهما {a} و{b} سم والمسافة بين مركزيهما {distance} سم. كم نقطة تقاطع مشتركة على المحيطين؟",
            UaeG10CircleResponse.Rectangle=>$"مستطيل ضلعاه {3*k} و{4*k} سم، رؤوسه الأربعة على دائرة. إذا كان محيط الدائرة م×π سم، فأوجد م.",
            _=>$"محيط دائرة {a}π سم. أوجد نصف قطرها، واكتب عددًا صحيحًا أو كسرًا."
        };
        return Problem(family,prompt,"حدد نصف القطر والقطر. المحيط π×القطر. قارن مسافة المركزين بمجموع نصفي القطرين والفرق بينهما؛ قطر الدائرة المحيطة بمستطيل يساوي قطر المستطيل.",family is UaeG10CircleResponse.Radius or UaeG10CircleResponse.Inverse or UaeG10CircleResponse.Approx?AssessmentItemType.ShortAnswer:AssessmentItemType.Numeric,("a",a),("b",b),("distance",distance),("k",k));
    }
}
internal static class UaeG10CircleResponse
{
    internal const string Diameter="uae.g10.general.u13.l01.diameter_from_radius";
    internal const string Radius="uae.g10.general.u13.l01.radius_from_diameter";
    internal const string Circumference="uae.g10.general.u13.l01.circumference_pi_coefficient";
    internal const string Approx="uae.g10.general.u13.l01.circumference_rounded";
    internal const string Overlap="uae.g10.general.u13.l01.center_line_overlap";
    internal const string Intersection="uae.g10.general.u13.l01.intersection_count";
    internal const string Rectangle="uae.g10.general.u13.l01.circumscribed_rectangle_pi_coefficient";
    internal const string Inverse="uae.g10.general.u13.l01.radius_from_circumference_pi";
    internal const string Central="uae.g10.general.u13.l02.central_remaining";
    internal const string Major="uae.g10.general.u13.l02.major_arc";
    internal const string Pie="uae.g10.general.u13.l02.pie_angle";
    internal const string ArcSum="uae.g10.general.u13.l02.adjacent_arc_sum";
    internal const string ArcCoeff="uae.g10.general.u13.l02.arc_length_pi_coefficient";
    internal const string ArcApprox="uae.g10.general.u13.l02.arc_length_rounded";
    internal const string ArcInverse="uae.g10.general.u13.l02.angle_from_arc_fraction";
    internal static bool IsArc(string? f)=>f is Central or Major or Pie or ArcSum or ArcCoeff or ArcApprox or ArcInverse;
    internal static bool Supports(string? f)=>IsArc(f)||f is Diameter or Radius or Circumference or Approx or Overlap or Intersection or Rectangle or Inverse;
    private static bool Valid(string f,IReadOnlyDictionary<string,int> p)=>IsArc(f)?
        p.Count==5&&new[]{"a1","a2","r","angle","percent"}.All(p.ContainsKey)&&p["a1"] is >=10 and <=170&&p["a2"] is >=10 and <=170&&p["r"] is >=2 and <=30&&p["angle"] is >=10 and <=350&&p["percent"] is >=5 and <=95&&p["percent"]%5==0:
        Supports(f)&&p.Count==4&&new[]{"a","b","distance","k"}.All(p.ContainsKey)&&p["a"] is >=2 and <=30&&p["b"] is >=2 and <=30&&p["distance"] is >=0 and <=60&&p["k"] is >=1 and <=10&&
        (f!=Intersection||p["distance"]!=0||p["a"]!=p["b"])&&(f!=Overlap||p["distance"]>Math.Abs(p["a"]-p["b"])&&p["distance"]<p["a"]+p["b"]);
    private static int Count(int a,int b,int d)=>d>a+b||d<Math.Abs(a-b)?0:d==a+b||d==Math.Abs(a-b)?1:2;
    private static (long N,long D) Answer(string f,IReadOnlyDictionary<string,int> p)
    {
        if(IsArc(f))
        {
            int a1=p["a1"],a2=p["a2"],r=p["r"],angle=p["angle"],percent=p["percent"];
            return f switch{Central=>(360-a1-a2,1),Major=>(360-a1,1),Pie or ArcInverse=>(percent*18/5,1),ArcSum=>(a1+a2,1),ArcCoeff=>(r*angle,180),ArcApprox=>((long)Math.Round(r*angle*Math.PI/180*100,MidpointRounding.AwayFromZero),100),_=>throw new InvalidOperationException()};
        }
        int a=p["a"],b=p["b"],d=p["distance"],k=p["k"];
        return f switch {Diameter=>(2*a,1),Radius or Inverse=>(a,2),Circumference=>(2*a,1),Approx=>((long)Math.Round(a*Math.PI*100,MidpointRounding.AwayFromZero),100),Overlap=>(a+b-d,1),Intersection=>(Count(a,b,d),1),Rectangle=>(5*k,1),_=>throw new InvalidOperationException()};
    }
    internal static string Format(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(f,p))throw new InvalidOperationException("Invalid G10 circle parameters");
        var(n,d)=Answer(f,p);
        if(f is Approx or ArcApprox)return (n/100m).ToString("0.00",CultureInfo.InvariantCulture);
        if(f==ArcCoeff){long a=n,b=d;while(b!=0)(a,b)=(b,a%b);n/=a;d/=a;}
        return n%d==0?(n/d).ToString(CultureInfo.InvariantCulture):n.ToString(CultureInfo.InvariantCulture)+"/"+d.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? raw)
    {
        if(!Valid(f,p)||string.IsNullOrWhiteSpace(raw)||raw.Length>40)return false;
        string s=string.Concat(raw.Trim().Where(c=>!char.IsWhiteSpace(c)).Select(c=>c switch{>= '٠' and <= '٩'=>(char)('0'+c-'٠'),>= '۰' and <= '۹'=>(char)('0'+c-'۰'),'٫'=>'.','⁄'=>'/',_=>c}));
        string[] pair=s.Split('/');
        long n,d=1;
        if(pair.Length==2){if(!long.TryParse(pair[0],NumberStyles.None,CultureInfo.InvariantCulture,out n)||!long.TryParse(pair[1],NumberStyles.None,CultureInfo.InvariantCulture,out d)||n>1000000||d<1||d>1000000)return false;}
        else if(pair.Length==1&&decimal.TryParse(s,NumberStyles.AllowDecimalPoint,CultureInfo.InvariantCulture,out decimal value)&&value>=0&&value<=1000000)
        {decimal scaled=value;while(scaled!=decimal.Truncate(scaled)&&d<10000){scaled*=10;d*=10;}if(scaled!=decimal.Truncate(scaled))return false;n=(long)scaled;}
        else return false;
        if(IsArc(f))
        {
            long a1=p["a1"],a2=p["a2"],r=p["r"],angle=p["angle"],percent=p["percent"];
            if(f==Central)return n+d*(a1+a2)==360*d;
            if(f==Major)return n+a1*d==360*d;
            if(f is Pie or ArcInverse)return 100*n==360*percent*d;
            if(f==ArcSum)return n==(a1+a2)*d;
            if(f==ArcCoeff)return 180*n==r*angle*d;
            double actual=r*angle*Math.PI/180;
            return f==ArcApprox&&n*100%d==0&&(double)n/d-0.005<=actual&&actual<(double)n/d+0.005;
        }
        long a=p["a"],b=p["b"],distance=p["distance"],k=p["k"];
        if(f is Radius or Inverse)return 2*n==a*d;
        if(f is Diameter or Circumference)return n==2*a*d;
        if(f==Approx)return n*100%d==0&&((double)n/d-0.005)<=a*Math.PI&&a*Math.PI<((double)n/d+0.005);
        if(f==Overlap)return n+d*distance==d*(a+b);
        if(f==Intersection)return n==Count((int)a,(int)b,(int)distance)*d;
        return f==Rectangle&&n==5*k*d&& (5*k)*(5*k)==(3*k)*(3*k)+(4*k)*(4*k);
    }
}

using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

// Source-specific families extend the existing ExactSkillContractQuestionEngine.
public sealed partial class ExactSkillContractQuestionEngine
{
    private static ExactProblem BuildG10Circle(string family,Random random,ExactSkillQuestionDifficulty difficulty)
    {
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
    internal static bool Supports(string? f)=>f is Diameter or Radius or Circumference or Approx or Overlap or Intersection or Rectangle or Inverse;
    private static bool Valid(string f,IReadOnlyDictionary<string,int> p)=>Supports(f)&&p.Count==4&&new[]{"a","b","distance","k"}.All(p.ContainsKey)&&p["a"] is >=2 and <=30&&p["b"] is >=2 and <=30&&p["distance"] is >=0 and <=60&&p["k"] is >=1 and <=10&&
        (f!=Intersection||p["distance"]!=0||p["a"]!=p["b"])&&(f!=Overlap||p["distance"]>Math.Abs(p["a"]-p["b"])&&p["distance"]<p["a"]+p["b"]);
    private static int Count(int a,int b,int d)=>d>a+b||d<Math.Abs(a-b)?0:d==a+b||d==Math.Abs(a-b)?1:2;
    private static (long N,long D) Answer(string f,IReadOnlyDictionary<string,int> p)
    {
        int a=p["a"],b=p["b"],d=p["distance"],k=p["k"];
        return f switch {Diameter=>(2*a,1),Radius or Inverse=>(a,2),Circumference=>(2*a,1),Approx=>((long)Math.Round(a*Math.PI*100,MidpointRounding.AwayFromZero),100),Overlap=>(a+b-d,1),Intersection=>(Count(a,b,d),1),Rectangle=>(5*k,1),_=>throw new InvalidOperationException()};
    }
    internal static string Format(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(f,p))throw new InvalidOperationException("Invalid G10 circle parameters");
        var(n,d)=Answer(f,p);
        if(f==Approx)return (n/100m).ToString("0.00",CultureInfo.InvariantCulture);
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
        long a=p["a"],b=p["b"],distance=p["distance"],k=p["k"];
        if(f is Radius or Inverse)return 2*n==a*d;
        if(f is Diameter or Circumference)return n==2*a*d;
        if(f==Approx)return n*100%d==0&&((double)n/d-0.005)<=a*Math.PI&&a*Math.PI<((double)n/d+0.005);
        if(f==Overlap)return n+d*distance==d*(a+b);
        if(f==Intersection)return n==Count((int)a,(int)b,(int)distance)*d;
        return f==Rectangle&&n==5*k*d&& (5*k)*(5*k)==(3*k)*(3*k)+(4*k)*(4*k);
    }
}

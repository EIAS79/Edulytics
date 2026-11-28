using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

public sealed partial class ExactSkillContractQuestionEngine
{
    private static ExactProblem BuildG10Trig(string family, Random random, ExactSkillQuestionDifficulty difficulty)
    {
        int triple=random.Next(0,4),m=random.Next(1,13),ratio=random.Next(0,6),angle=5*random.Next(4,15),distance=random.Next(10,91),special=random.Next(0,3);
        var (a,b,c)=UaeG10TrigResponse.Triple(triple,m);
        string name=family.Substring(UaeG10TrigResponse.Prefix.Length);
        string prompt=name switch
        {
            "ratio_six_exact"=>$"مثلث قائم عند الزاوية θ مقابله {a} ومجاوره {b} ووتره {c}. أوجد {new[]{"جا","جتا","ظا","قتا","قا","ظتا"}[ratio]} θ على صورة كسر أو عدد صحيح.",
            "side_opposite_exact"=>$"مثلث قائم وتره {c} سم، وجا θ={a/m}/{c/m}. أوجد المقابل.",
            "side_adjacent_exact"=>$"مثلث قائم وتره {c} سم، وجتا θ={b/m}/{c/m}. أوجد المجاور.",
            "side_hypotenuse_exact"=>$"مثلث قائم مقابله {a} سم وجا θ={a/m}/{c/m}. أوجد الوتر.",
            "angle_inverse_sine_rounded"=>$"مقابل زاوية حادة θ يساوي {a} سم ووتر المثلث القائم {c} سم. أوجد θ بالدرجات لأقرب جزء من مائة.",
            "angle_inverse_cosine_rounded"=>$"مجاور زاوية حادة θ يساوي {b} سم ووتر المثلث القائم {c} سم. أوجد θ بالدرجات لأقرب جزء من مائة.",
            "angle_inverse_tangent_rounded"=>$"مقابل زاوية حادة θ يساوي {a} سم ومجاورها {b} سم. أوجد θ بالدرجات لأقرب جزء من مائة.",
            "special_angle_exact"=>special==0?$"وتر مثلث قائم {2*m} سم وزاوية حادة 30°. أوجد مقابل الزاوية.":special==1?$"وتر مثلث قائم {2*m} سم وزاوية حادة 60°. أوجد مجاور الزاوية.":$"مثلث قائم متساوي الساقين طول ساقه {m} سم. أوجد الساق الأخرى.",
            "elevation_height_rounded"=>$"المسافة الأفقية من نقطة رصد أرضية إلى قاعدة برج {distance} مترًا وزاوية ارتفاع قمته {angle}°. أوجد الارتفاع بالمتر لأقرب جزء من مائة.",
            "depression_distance_rounded"=>$"من منصة ارتفاعها {distance} مترًا فوق أرض أفقية، زاوية انخفاض نقطة على الأرض {angle}°. أوجد المسافة الأفقية بالمتر لأقرب جزء من مائة.",
            _=>throw new InvalidOperationException()
        };
        return Problem(family,prompt,"حدد المقابل والمجاور والوتر، واختر النسبة أو معكوسها بالدرجات. لا تقرب قبل النتيجة النهائية.",name.EndsWith("_rounded")||name=="ratio_six_exact"?AssessmentItemType.ShortAnswer:AssessmentItemType.Numeric,("triple",triple),("m",m),("ratio",ratio),("angle",angle),("distance",distance),("special",special));
    }
}

internal static class UaeG10TrigResponse
{
    internal const string Prefix="uae.g10.general.u14.l01.";
    private static readonly HashSet<string> Names=new(StringComparer.Ordinal){"ratio_six_exact","side_opposite_exact","side_adjacent_exact","side_hypotenuse_exact","angle_inverse_sine_rounded","angle_inverse_cosine_rounded","angle_inverse_tangent_rounded","special_angle_exact","elevation_height_rounded","depression_distance_rounded"};
    internal static bool Supports(string? f)=>f!=null&&f.StartsWith(Prefix,StringComparison.Ordinal)&&Names.Contains(f.Substring(Prefix.Length));
    internal static (int A,int B,int C) Triple(int t,int m)
    {
        var (a,b,c)=t switch{0=>(3,4,5),1=>(5,12,13),2=>(8,15,17),_=>(7,24,25)};
        return (a*m,b*m,c*m);
    }
    private static bool Valid(string f,IReadOnlyDictionary<string,int> p)=>Supports(f)&&p.Count==6&&new[]{"triple","m","ratio","angle","distance","special"}.All(p.ContainsKey)&&p["triple"] is >=0 and <=3&&p["m"] is >=1 and <=12&&p["ratio"] is >=0 and <=5&&p["angle"] is >=20 and <=70&&p["angle"]%5==0&&p["distance"] is >=10 and <=90&&p["special"] is >=0 and <=2;
    private static (int N,int D) Ratio(int a,int b,int c,int r)=>r switch{0=>(a,c),1=>(b,c),2=>(a,b),3=>(c,a),4=>(c,b),_=>(b,a)};
    private static double Degrees(double radians)=>radians*180/Math.PI;
    private static double Radians(int degrees)=>degrees*Math.PI/180;
    internal static string Format(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(f,p))throw new InvalidOperationException("Invalid right triangle parameters");
        var (a,b,c)=Triple(p["triple"],p["m"]);
        string name=f.Substring(Prefix.Length);
        if(name=="ratio_six_exact")
        {
            var (n,d)=Ratio(a,b,c,p["ratio"]);int x=n,y=d;while(y!=0)(x,y)=(y,x%y);n/=x;d/=x;
            return d==1?n.ToString(CultureInfo.InvariantCulture):$"{n}/{d}";
        }
        double value=name switch{"side_opposite_exact"=>a,"side_adjacent_exact"=>b,"side_hypotenuse_exact"=>c,"special_angle_exact"=>p["m"],"angle_inverse_sine_rounded"=>Degrees(Math.Asin((double)a/c)),"angle_inverse_cosine_rounded"=>Degrees(Math.Acos((double)b/c)),"angle_inverse_tangent_rounded"=>Degrees(Math.Atan((double)a/b)),"elevation_height_rounded"=>p["distance"]*Math.Tan(Radians(p["angle"])),"depression_distance_rounded"=>p["distance"]/Math.Tan(Radians(p["angle"])),_=>throw new InvalidOperationException()};
        return name.EndsWith("_rounded")?Math.Round(value,2,MidpointRounding.AwayFromZero).ToString("0.00",CultureInfo.InvariantCulture):value.ToString("0",CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? raw)
    {
        if(!Valid(f,p)||string.IsNullOrWhiteSpace(raw)||raw.Length>40)return false;
        string s=string.Concat(raw.Trim().Where(c=>!char.IsWhiteSpace(c)).Select(c=>c switch{>= '٠' and <= '٩'=>(char)('0'+c-'٠'),>= '۰' and <= '۹'=>(char)('0'+c-'۰'),'٫'=>'.','⁄'=>'/',_=>c}));
        string name=f.Substring(Prefix.Length);var(a,b,c)=Triple(p["triple"],p["m"]);
        if(name=="ratio_six_exact")
        {
            var(n,d)=Ratio(a,b,c,p["ratio"]);var pair=s.Split('/');
            if(pair.Length==2)return long.TryParse(pair[0],NumberStyles.None,CultureInfo.InvariantCulture,out long n1)&&long.TryParse(pair[1],NumberStyles.None,CultureInfo.InvariantCulture,out long d1)&&n1>=0&&n1<=1000000&&d1>0&&d1<=1000000&&n1*d==d1*n;
            return pair.Length==1&&decimal.TryParse(s,NumberStyles.AllowDecimalPoint,CultureInfo.InvariantCulture,out decimal v)&&v>=0&&v<=1000000&&v*d==n;
        }
        if(!decimal.TryParse(s,NumberStyles.AllowDecimalPoint,CultureInfo.InvariantCulture,out decimal submitted)||submitted<0||submitted>1000000)return false;
        if(name=="side_opposite_exact")return submitted*c==a*c;
        if(name=="side_adjacent_exact")return submitted*c==b*c;
        if(name=="side_hypotenuse_exact")return submitted*a==a*c;
        if(name=="special_angle_exact")return submitted==p["m"];
        if(submitted*100!=decimal.Truncate(submitted*100))return false;
        double q=(double)submitted,lo=q-0.005,hi=q+0.005;
        if(name.StartsWith("angle_inverse_"))
        {
            if(q<=0||q>=90)return false;
            double low=RadiansFromDouble(Math.Max(0,lo)),high=RadiansFromDouble(Math.Min(90,hi));
            return name=="angle_inverse_sine_rounded"?c*Math.Sin(low)<=a&&a<c*Math.Sin(high):name=="angle_inverse_cosine_rounded"?c*Math.Cos(high)<b&&b<=c*Math.Cos(low):b*Math.Tan(low)<=a&&a<b*Math.Tan(high);
        }
        double tangent=Math.Tan(Radians(p["angle"]));int dist=p["distance"];
        return name=="elevation_height_rounded"?lo<=dist*tangent&&dist*tangent<hi:name=="depression_distance_rounded"&&lo*tangent<=dist&&dist<hi*tangent;
    }
    private static double RadiansFromDouble(double degrees)=>degrees*Math.PI/180;
}

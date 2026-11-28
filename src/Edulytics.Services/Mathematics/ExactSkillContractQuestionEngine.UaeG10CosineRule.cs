using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
public sealed partial class ExactSkillContractQuestionEngine
{
    private static ExactProblem BuildG10CosineRule(string family, Random random, ExactSkillQuestionDifficulty difficulty)
    {
        int a=random.Next(2,31),b=random.Next(2,31),c=random.Next(Math.Max(a,b),a+b),angle=5*random.Next(4,33),mode=random.Next(0,4);
        string name=family.Substring(UaeG10CosineRuleResponse.Prefix.Length);
        if(name=="sas_square_exact")angle=new[]{60,90,120}[random.Next(3)];
        string prompt=name switch
        {
            "sas_side_rounded"=>$"في مثلث ABC، a={a} وb={b} وC={angle}°. أوجد c لأقرب جزء من مئة.",
            "sas_square_exact"=>$"في مثلث ABC، a={a} وb={b} وC={angle}°. أوجد مربع طول c.",
            "sss_cosine_exact"=>$"في مثلث ABC، a={a} وb={b} وc={c}. أوجد cos C في صورة كسر أو عدد صحيح.",
            "sss_angle_rounded"=>$"في مثلث ABC، a={a} وb={b} وc={c}. أوجد C بالدرجات لأقرب جزء من مئة.",
            "triangle_type"=>$"أطوال مثلث {a} و{b} و{c}. صنفه حسب الزوايا: اكتب 1 للحاد، 2 للقائم، 3 للمنفرج.",
            "choose_law"=>$"معطيات حل مثلث: {new[]{"زاويتان وضلع","ضلعان وزاوية غير محصورة","ضلعان وزاوية محصورة","الأضلاع الثلاثة"}[mode]}. أي قانون تبدأ به؟ اكتب 1 للجيب و2 لجيب التمام.",
            _=>$"مساران مستقيمان من نقطة مشتركة طولهما {a} و{b} مترًا والزاوية المحصورة {angle}°. أوجد المسافة بين نهايتيهما لأقرب جزء من مئة متر."
        };
        return Problem(family,prompt,"حدد الزاوية المقابلة والضلعين المحيطين بها. تحقق من متباينة المثلث.",name=="sss_cosine_exact"?AssessmentItemType.ShortAnswer:AssessmentItemType.Numeric,("a",a),("b",b),("c",c),("angle",angle),("mode",mode));
    }
}
internal static class UaeG10CosineRuleResponse
{
    internal const string Prefix="uae.g10.general.u14.l05.";
    private static readonly HashSet<string> Names=new(StringComparer.Ordinal){"sas_side_rounded","sas_square_exact","sss_cosine_exact","sss_angle_rounded","triangle_type","choose_law","indirect_distance_rounded"};
    internal static bool Supports(string? f)=>f!=null&&f.StartsWith(Prefix,StringComparison.Ordinal)&&Names.Contains(f.Substring(Prefix.Length));
    private static bool Valid(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!Supports(f)||p.Count!=5||!new[]{"a","b","c","angle","mode"}.All(p.ContainsKey))return false;
        int a=p["a"],b=p["b"],c=p["c"],v=p["angle"];
        return a is >=2 and <=30&&b is >=2 and <=30&&c>=Math.Max(a,b)&&c<a+b&&v is >=20 and <=160&&v%5==0&&p["mode"] is >=0 and <=3&&(f!=Prefix+"sas_square_exact"||(v is 60 or 90 or 120));
    }
    private static double Value(string f,IReadOnlyDictionary<string,int> p)
    {
        int a=p["a"],b=p["b"],c=p["c"],v=p["angle"];
        return f.Substring(Prefix.Length) switch
        {
            "sas_square_exact"=>a*a+b*b+(v==60?-a*b:v==90?0:a*b),
            "sss_angle_rounded"=>Math.Acos((a*a+b*b-c*c)/(2d*a*b))*180/Math.PI,
            "triangle_type"=>c*c<a*a+b*b?1:c*c==a*a+b*b?2:3,
            "choose_law"=>p["mode"]<2?1:2,
            _=>Math.Sqrt(a*a+b*b-2*a*b*Math.Cos(v*Math.PI/180))
        };
    }
    internal static string Format(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(f,p))throw new InvalidOperationException("Invalid cosine rule parameters");
        long a=p["a"],b=p["b"],c=p["c"];
        if(f==Prefix+"sss_cosine_exact")return UaeG10TrigScalar.Rational(a*a+b*b-c*c,2*a*b);
        return f.EndsWith("_rounded",StringComparison.Ordinal)?UaeG10TrigScalar.Rounded(Value(f,p)):UaeG10TrigScalar.Rational((long)Value(f,p),1);
    }
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? raw)
    {
        if(!Valid(f,p))return false;
        long a=p["a"],b=p["b"],c=p["c"],v=p["angle"],q=a*a+b*b-c*c;
        string name=f.Substring(Prefix.Length);
        if(name=="sss_cosine_exact")return UaeG10TrigScalar.Matches(q,2*a*b,raw);
        if(name=="triangle_type")return UaeG10TrigScalar.Matches(q>0?1:q==0?2:3,1,raw);
        if(name=="choose_law")return UaeG10TrigScalar.Matches(new[]{1,1,2,2}[p["mode"]],1,raw);
        if(name=="sas_square_exact")return UaeG10TrigScalar.Matches((a-b)*(a-b)+(v==60?1:v==90?2:3)*a*b,1,raw);
        if(name=="sss_angle_rounded")
        {
            double x=(double)q/(2*a*b);
            return UaeG10TrigScalar.MatchesRounded(Math.Atan2(Math.Sqrt(Math.Max(0,1-x*x)),x)*180/Math.PI,raw);
        }
        double s=Math.Sin(v*Math.PI/360);
        return UaeG10TrigScalar.MatchesRounded(Math.Sqrt((a-b)*(a-b)+4*a*b*s*s),raw);
    }
}

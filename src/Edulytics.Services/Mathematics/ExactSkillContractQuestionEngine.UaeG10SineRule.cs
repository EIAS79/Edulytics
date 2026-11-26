using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
public sealed partial class ExactSkillContractQuestionEngine
{
    private static ExactProblem BuildG10SineRule(string family, Random random, ExactSkillQuestionDifficulty difficulty)
    {
        int a=random.Next(2,31),b=2*random.Next(2,16),A=5*random.Next(4,17),B=5*random.Next(4,17),n=random.Next(3,13),angle=5*random.Next(4,33);
        string name=family.Substring(UaeG10SineRuleResponse.Prefix.Length);
        if(name=="ssa_triangle_count") A=new[]{30,90,120}[random.Next(3)];
        if(name=="ssa_acute_solution_angle_rounded"){A=30;a=b+random.Next(1,11);}
        string prompt=name switch
        {
            "sas_area_rounded"=>$"في مثلث، الضلعان a={a} وb={b} والزاوية المحصورة C={angle}°. أوجد المساحة لأقرب جزء من مئة.",
            "aas_side_rounded"=>$"في مثلث ABC، A={A}° وB={B}° وa={a}. أوجد طول b لأقرب جزء من مئة.",
            "third_angle"=>$"زاويتا مثلث قياسهما {A}° و{B}°. أوجد قياس الزاوية الثالثة بالدرجات.",
            "ssa_triangle_count"=>$"في مثلث ABC، A={A}° وa={a} وb={b}. كم مثلثًا ممكنًا؟ اكتب 0 أو 1 أو 2.",
            "ssa_acute_solution_angle_rounded"=>$"في مثلث ABC، A=30° وa={a} وb={b}. أوجد قياس B بالدرجات لأقرب جزء من مئة.",
            "regular_polygon_area_rounded"=>$"مضلع منتظم له {n} أضلاع ونصف قطر دائرته المحيطة {a}. أوجد المساحة لأقرب جزء من مئة.",
            _=>$"محطتان A وB تفصل بينهما {a} مترًا. إذا كانت زاويتا المثلث عند A وB تساويان {A}° و{B}°، فأوجد المسافة AC بالمتر لأقرب جزء من مئة."
        };
        return Problem(family,prompt,"حدد الزاوية المقابلة للضلع؛ في SSA افحص عدد الحلول قبل الحساب.",AssessmentItemType.Numeric,("a",a),("b",b),("A",A),("B",B),("n",n),("angle",angle));
    }
}
internal static class UaeG10SineRuleResponse
{
    internal const string Prefix="uae.g10.general.u14.l04.";
    private static readonly HashSet<string> Names=new(StringComparer.Ordinal){"sas_area_rounded","aas_side_rounded","third_angle","ssa_triangle_count","ssa_acute_solution_angle_rounded","regular_polygon_area_rounded","survey_distance_rounded"};
    internal static bool Supports(string? f)=>f!=null&&f.StartsWith(Prefix,StringComparison.Ordinal)&&Names.Contains(f.Substring(Prefix.Length));
    private static bool Valid(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!Supports(f)||p.Count!=6||!new[]{"a","b","A","B","n","angle"}.All(p.ContainsKey))return false;
        int a=p["a"],b=p["b"],A=p["A"],B=p["B"],n=p["n"],angle=p["angle"];
        if(a<2||a>40||b<4||b>30||b%2!=0||B<20||B>80||B%5!=0||n<3||n>12||angle<20||angle>160||angle%5!=0)return false;
        string name=f.Substring(Prefix.Length);
        if(name=="ssa_triangle_count")return a<=30&&(A is 30 or 90 or 120);
        if(name=="ssa_acute_solution_angle_rounded")return A==30&&a>b&&a<=b+10;
        return a<=30&&A>=20&&A<=80&&A%5==0;
    }
    private static double Sin(double d)=>Math.Sin(d*Math.PI/180);
    private static int Count(int A,int a,int b)=>A==30?(2*a<b?0:2*a==b||a>=b?1:2):a>b?1:0;
    private static double Value(string f,IReadOnlyDictionary<string,int> p)
    {
        int a=p["a"],b=p["b"],A=p["A"],B=p["B"],n=p["n"],angle=p["angle"];
        return f.Substring(Prefix.Length) switch
        {
            "sas_area_rounded"=>a*b*Sin(angle)/2,
            "aas_side_rounded"=>a*Sin(B)/Sin(A),
            "third_angle"=>180-A-B,
            "ssa_triangle_count"=>Count(A,a,b),
            "ssa_acute_solution_angle_rounded"=>Math.Asin(b*Sin(A)/a)*180/Math.PI,
            "regular_polygon_area_rounded"=>n*a*a*Sin(360d/n)/2,
            _=>a*Sin(B)/Sin(180-A-B)
        };
    }
    internal static string Format(string f,IReadOnlyDictionary<string,int> p)
    {if(!Valid(f,p))throw new InvalidOperationException("Invalid sine rule parameters");return f.EndsWith("_rounded",StringComparison.Ordinal)?UaeG10TrigScalar.Rounded(Value(f,p)):UaeG10TrigScalar.Rational((long)Value(f,p),1);}
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? raw)
    {
        if(!Valid(f,p))return false;
        double a=p["a"],b=p["b"],A=p["A"],B=p["B"],n=p["n"],angle=p["angle"],rad=Math.PI/180;
        string name=f.Substring(Prefix.Length);
        if(name=="third_angle")return UaeG10TrigScalar.Matches((long)(180-A-B),1,raw);
        if(name=="ssa_triangle_count")
        {
            double ratio=b*Math.Sin(A*rad)/a;
            int count=0;
            if(ratio<=1+1e-12)
            {
                double x=Math.Abs(ratio-1)<=1e-12?90:Math.Asin(ratio)/rad;
                if(A+x<180-1e-9)count++;
                if(Math.Abs(180-2*x)>1e-9&&A+180-x<180-1e-9)count++;
            }
            return UaeG10TrigScalar.Matches(count,1,raw);
        }
        double expected=name switch
        {
            "sas_area_rounded"=>a*(b*Math.Sin(angle*rad))/2,
            "aas_side_rounded"=>Math.Sin(B*rad)*(a/Math.Sin(A*rad)),
            "ssa_acute_solution_angle_rounded"=>Math.Asin(b/(2*a))/rad,
            "regular_polygon_area_rounded"=>n*(2*a*Math.Sin(Math.PI/n))*(a*Math.Cos(Math.PI/n))/2,
            _=>Math.Sin(B*rad)*(a/Math.Sin((A+B)*rad))
        };
        return UaeG10TrigScalar.MatchesRounded(expected,raw);
    }
}

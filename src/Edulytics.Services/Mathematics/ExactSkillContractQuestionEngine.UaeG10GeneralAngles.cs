using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
public sealed partial class ExactSkillContractQuestionEngine
{
    private static ExactProblem BuildG10GeneralAngles(string family, Random random, ExactSkillQuestionDifficulty difficulty)
    {
        int triple=random.Next(0,4), quadrant=random.Next(1,5), ratio=random.Next(0,6), axis=random.Next(0,4);
        int reference=15*random.Next(1,6), turns=random.Next(-2,3);
        int theta=(quadrant switch {1=>reference,2=>180-reference,3=>180+reference,_=>360-reference})+360*turns;
        var (x,y)=UaeG10GeneralAnglesResponse.Point(triple,quadrant);
        string name=family.Substring(UaeG10GeneralAnglesResponse.Prefix.Length);
        if(name=="axis_value") ratio=random.Next(0,2);
        string fn=new[]{"sin","cos","tan","csc","sec","cot"}[ratio];
        string prompt=name switch
        {
            "coordinate_ratio"=>$"النقطة ({x},{y}) تقع على ضلع انتهاء θ. أوجد {fn} θ على صورة كسر أو عدد صحيح.",
            "reference_degree"=>$"أوجد قياس الزاوية المرجعية للزاوية {theta}° بالدرجات.",
            "reference_radian_pi_coefficient"=>$"أوجد الزاوية المرجعية للزاوية ({theta/15}/12)π راديان. اكتب معامل π فقط.",
            "quadrant_from_angle"=>$"حدد رقم الربع (1–4) لزاوية {theta}° في الوضع القياسي.",
            "quadrant_from_signs"=>$"إذا كان sin θ {(y>0?">":"<")} 0 وcos θ {(x>0?">":"<")} 0، فاكتب رقم الربع (1–4).",
            "axis_defined"=>$"هل {fn} {90*axis}° معرفة؟ اكتب 1 إذا كانت معرفة و0 إذا كانت غير معرفة.",
            _=>$"أوجد {fn} {90*axis}°."
        };
        bool rational=name=="coordinate_ratio"||name=="reference_radian_pi_coefficient";
        return Problem(family,prompt,"حدد الإحداثيات والإشارة والمقام؛ للمرجع رد الزاوية إلى دورة واحدة.",rational?AssessmentItemType.ShortAnswer:AssessmentItemType.Numeric,("triple",triple),("quadrant",quadrant),("ratio",ratio),("axis",axis),("reference",reference),("turns",turns));
    }
}
internal static class UaeG10GeneralAnglesResponse
{
    internal const string Prefix="uae.g10.general.u14.l03.";
    private static readonly HashSet<string> Names=new(StringComparer.Ordinal){"coordinate_ratio","reference_degree","reference_radian_pi_coefficient","quadrant_from_angle","quadrant_from_signs","axis_defined","axis_value"};
    internal static bool Supports(string? f)=>f!=null&&f.StartsWith(Prefix,StringComparison.Ordinal)&&Names.Contains(f.Substring(Prefix.Length));
    internal static (int X,int Y) Point(int t,int q)
    {
        var (x,y)=t switch {0=>(3,4),1=>(5,12),2=>(8,15),_=>(7,24)};
        return (q is 2 or 3?-x:x,q is 3 or 4?-y:y);
    }
    private static bool Valid(string f,IReadOnlyDictionary<string,int> p)=>Supports(f)&&p.Count==6&&new[]{"triple","quadrant","ratio","axis","reference","turns"}.All(p.ContainsKey)&&p["triple"] is >=0 and <=3&&p["quadrant"] is >=1 and <=4&&p["ratio"] is >=0 and <=5&&p["axis"] is >=0 and <=3&&p["reference"] is >=15 and <=75&&p["reference"]%15==0&&p["turns"] is >=-2 and <=2&&(f!=Prefix+"axis_value"||p["ratio"]<=1);
    private static (long N,long D) Normalize(long n,long d)=>d<0?(-n,-d):(n,d);
    private static (long N,long D) Answer(string f,IReadOnlyDictionary<string,int> p)
    {
        var (x,y)=Point(p["triple"],p["quadrant"]);int r=(int)Math.Sqrt(x*x+y*y),ratio=p["ratio"],axis=p["axis"];
        int ax=new[]{1,0,-1,0}[axis],ay=new[]{0,1,0,-1}[axis];
        return f.Substring(Prefix.Length) switch
        {
            "coordinate_ratio"=>ratio switch {0=>((long)y,(long)r),1=>((long)x,(long)r),2=>Normalize(y,x),3=>Normalize(r,y),4=>Normalize(r,x),_=>Normalize(x,y)},
            "reference_degree"=>(p["reference"],1),
            "reference_radian_pi_coefficient"=>(p["reference"],180),
            "quadrant_from_angle" or "quadrant_from_signs"=>(p["quadrant"],1),
            "axis_defined"=>((ratio<=1||(ratio is 2 or 4?ax!=0:ay!=0))?1:0,1),
            _=>(ratio==0?ay:ax,1)
        };
    }
    internal static string Format(string f,IReadOnlyDictionary<string,int> p)
    {if(!Valid(f,p))throw new InvalidOperationException("Invalid general angle parameters");var(n,d)=Answer(f,p);return UaeG10TrigScalar.Rational(n,d);}
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? raw)
    {
        if(!Valid(f,p))return false;
        int q=p["quadrant"],v=p["reference"],ratio=p["ratio"],axis=p["axis"];
        var(x,y)=Point(p["triple"],q);int r=new[]{5,13,17,25}[p["triple"]];
        int theta=(q switch{1=>v,2=>180-v,3=>180+v,_=>360-v})+360*p["turns"],t=((theta%360)+360)%360,alpha=Math.Min(t%180,180-t%180);
        var(n,d)=f.Substring(Prefix.Length) switch
        {
            "coordinate_ratio"=>Normalize(new long[]{y,x,y,r,r,x}[ratio],new long[]{r,r,x,y,x,y}[ratio]),
            "reference_degree"=>((long)alpha,1L),
            "reference_radian_pi_coefficient"=>((long)alpha,180L),
            "quadrant_from_angle"=>((long)(t/90+1),1L),
            "quadrant_from_signs"=>((long)(x>0?(y>0?1:4):(y>0?2:3)),1L),
            "axis_defined"=>((long)((ratio<2||(ratio is 2 or 4?axis%2==0:axis%2==1))?1:0),1L),
            _=>((long)(ratio==0?new[]{0,1,0,-1}[axis]:new[]{1,0,-1,0}[axis]),1L)
        };
        return UaeG10TrigScalar.Matches(n,d,raw);
    }
}

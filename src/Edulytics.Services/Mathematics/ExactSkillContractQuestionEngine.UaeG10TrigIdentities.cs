using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
public sealed partial class ExactSkillContractQuestionEngine
{
    private static ExactProblem BuildG10TrigIdentities(string family, Random random, ExactSkillQuestionDifficulty difficulty)
    {
        int t=random.Next(0,4),q=random.Next(1,5),fn=random.Next(0,6),n=random.Next(1,31),angle=random.Next(1,90);
        var(x,y,h)=UaeG10InverseTrigResponse.Triple(t);
        int sx=q<=2?1:-1,sy=q==1||q==4?1:-1;
        string[] labels={"sin","cos","tan","cot","sec","csc"},co={"cos","sin","cot","tan","csc","sec"};
        string name=family.Substring(UaeG10TrigIdentitiesResponse.Prefix.Length);
        string signedTan=$"{sx*sy*x}/{y}",signedCot=$"{sx*sy*y}/{x}";
        string prompt=name switch {
            "cos_from_sin_quadrant"=>$"إذا sin θ={sx*x}/{h} والزاوية في الربع {q} فأوجد cos θ في صورة كسر أو عدد صحيح.",
            "sin_from_cos_quadrant"=>$"إذا cos θ={sy*y}/{h} والزاوية في الربع {q} فأوجد sin θ في صورة كسر أو عدد صحيح.",
            "sec_from_tan_quadrant"=>$"إذا tan θ={signedTan} والزاوية في الربع {q} فأوجد sec θ في صورة كسر أو عدد صحيح.",
            "csc_from_cot_quadrant"=>$"إذا cot θ={signedCot} والزاوية في الربع {q} فأوجد csc θ في صورة كسر أو عدد صحيح.",
            "quotient_from_pair"=>$"إذا sin θ={sx*x}/{h} وcos θ={sy*y}/{h} فأوجد tan θ في صورة كسر أو عدد صحيح.",
            "identity_numeric_constant"=>$"بسّط {(fn%3==0?$"{n}(sin²θ+cos²θ)":fn%3==1?$"{n}(sec²θ−tan²θ)":$"{n}(csc²θ−cot²θ)")} حيث التعبير معرّف.",
            "cofunction_angle_degrees"=>$"أوجد θ بالدرجات في (0°,90°) بحيث {labels[fn]}({angle}°)={co[fn]}(θ).",
            _=>$"أوجد العدد m∈{{−1,1}} الذي يجعل {labels[fn]}(−θ)=m {labels[fn]}(θ) متطابقة على المجال المشترك."
        };
        bool rational=name is "cos_from_sin_quadrant" or "sin_from_cos_quadrant" or "sec_from_tan_quadrant" or "csc_from_cot_quadrant" or "quotient_from_pair";
        return Problem(family,prompt,"حدد المتطابقة المناسبة وإشارات الربع، واحفظ شروط المجال.",rational?AssessmentItemType.ShortAnswer:AssessmentItemType.Numeric,("t",t),("q",q),("fn",fn),("n",n),("angle",angle));
    }
}
internal static class UaeG10TrigIdentitiesResponse
{
    internal const string Prefix="uae.g10.general.u14.l10.";
    private static readonly HashSet<string> Names=new(StringComparer.Ordinal){"cos_from_sin_quadrant","sin_from_cos_quadrant","sec_from_tan_quadrant","csc_from_cot_quadrant","quotient_from_pair","identity_numeric_constant","cofunction_angle_degrees","odd_even_multiplier"};
    internal static bool Supports(string? f)=>f!=null&&f.StartsWith(Prefix,StringComparison.Ordinal)&&Names.Contains(f.Substring(Prefix.Length));
    private static bool Valid(string f,IReadOnlyDictionary<string,int> p)=>Supports(f)&&p.Count==5&&p.TryGetValue("t",out int t)&&t>=0&&t<=3&&p.TryGetValue("q",out int q)&&q>=1&&q<=4&&p.TryGetValue("fn",out int fn)&&fn>=0&&fn<=5&&p.TryGetValue("n",out int n)&&n>=1&&n<=30&&p.TryGetValue("angle",out int angle)&&angle>=1&&angle<=89;
    private static (long N,long D) Answer(string f,IReadOnlyDictionary<string,int> p){
        var(x,y,h)=UaeG10InverseTrigResponse.Triple(p["t"]);int q=p["q"],fn=p["fn"],sx=q<=2?1:-1,sy=q==1||q==4?1:-1;
        return f.Substring(Prefix.Length) switch {
            "cos_from_sin_quadrant"=>(sy*y,h),
            "sin_from_cos_quadrant"=>(sx*x,h),
            "sec_from_tan_quadrant"=>(sy*h,y),
            "csc_from_cot_quadrant"=>(sx*h,x),
            "quotient_from_pair"=>(sx*sy*x,y),
            "identity_numeric_constant"=>(p["n"],1),
            "cofunction_angle_degrees"=>(90-p["angle"],1),
            _=>(fn==1||fn==4?1:-1,1)
        };
    }
    internal static string Format(string f,IReadOnlyDictionary<string,int> p){if(!Valid(f,p))throw new InvalidOperationException("Invalid identity parameters");var(n,d)=Answer(f,p);return UaeG10TrigScalar.Rational(n,d);}
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? raw){
        if(!Valid(f,p))return false;
        var(x,y,h)=UaeG10InverseTrigResponse.Triple(p["t"]);int q=p["q"],fn=p["fn"];long sx=new[]{1,1,-1,-1}[q-1],sy=new[]{1,-1,-1,1}[q-1];
        long x2=(long)Math.Round(Math.Sqrt(h*h-y*y)),y2=(long)Math.Round(Math.Sqrt(h*h-x*x));
        var(n,d)=f.Substring(Prefix.Length) switch {
            "cos_from_sin_quadrant"=>(sy*y2,(long)h),
            "sin_from_cos_quadrant"=>(sx*x2,(long)h),
            "sec_from_tan_quadrant"=>(sy*(long)Math.Round(Math.Sqrt(x*x+y*y)),(long)y),
            "csc_from_cot_quadrant"=>(sx*(long)Math.Round(Math.Sqrt(x*x+y*y)),(long)x),
            "quotient_from_pair"=>(sx*sy*x2,y2),
            "identity_numeric_constant"=>((long)p["n"],1L),
            "cofunction_angle_degrees"=>(90L-p["angle"],1L),
            _=>((long)new[]{-1,1,-1,-1,1,-1}[fn],1L)
        };
        return UaeG10TrigScalar.Matches(n,d,raw);
    }
}

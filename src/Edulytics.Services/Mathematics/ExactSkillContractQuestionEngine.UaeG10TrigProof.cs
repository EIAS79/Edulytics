using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
public sealed partial class ExactSkillContractQuestionEngine
{
    private static ExactProblem BuildG10TrigProof(string family, Random random, ExactSkillQuestionDifficulty difficulty)
    {
        int kind=random.Next(0,6), shift=random.Next(0,3), scale=random.Next(1,21);
        string prompt;
        if(family==UaeG10TrigProofResponse.Choice){
            var (expression, options)=UaeG10TrigProofResponse.Item(kind);
            prompt=$"اختر رقم التعبير المكافئ لـ {expression} على المجال المشترك: 1) {options[(0+shift)%3]}؛ 2) {options[(1+shift)%3]}؛ 3) {options[(2+shift)%3]}.";
        }else{
            prompt=kind%3 switch {
                0=>$"أوجد العدد k بحيث {scale}(sin θ+cos θ)²−{scale}=k sin θ cos θ متطابقة.",
                1=>$"أوجد العدد k بحيث {scale}(1−cos²θ)=k sin²θ متطابقة.",
                _=>$"أوجد العدد k بحيث {scale}(sec²θ−tan²θ)=k حيث cos θ≠0."
            };
        }
        return Problem(family,prompt,"حدد المجال، ثم استخدم متطابقة أساسية أو وسع المربع. البرهان المفتوح يتطلب تبرير الخطوات.",AssessmentItemType.Numeric,("kind",kind),("shift",shift),("scale",scale));
    }
}
internal static class UaeG10TrigProofResponse
{
    internal const string Choice="uae.g10.general.u14.l11.equivalent_expression_choice", Coefficient="uae.g10.general.u14.l11.proof_coefficient";
    internal static bool Supports(string? family)=>family is Choice or Coefficient;
    private static bool Valid(string f,IReadOnlyDictionary<string,int> p)=>Supports(f)&&p.Count==3&&p.TryGetValue("kind",out int k)&&k>=0&&k<6&&p.TryGetValue("shift",out int h)&&h>=0&&h<3&&p.TryGetValue("scale",out int s)&&s>=1&&s<=20;
    internal static (string,string[]) Item(int k)=>k switch {
        0=>("(1−cos²θ)/sin θ, sin θ≠0",new[]{"sin θ","cos θ","1"}),
        1=>("(1−sin²θ)/cos θ, cos θ≠0",new[]{"cos θ","sin θ","1"}),
        2=>("1+tan²θ, cos θ≠0",new[]{"sec²θ","tan²θ","1"}),
        3=>("(sec θ−tan θ)(sec θ+tan θ), cos θ≠0",new[]{"1","sec²θ","tan²θ"}),
        4=>("(1−cos θ)(1+cos θ)",new[]{"sin²θ","cos²θ","1"}),
        _=>("(sin θ+cos θ)²−1",new[]{"2 sin θ cos θ","sin²θ","1"})
    };
    internal static string Format(string f,IReadOnlyDictionary<string,int> p){
        if(!Valid(f,p))throw new InvalidOperationException("Invalid proof-step parameters");
        int n=f==Choice?1+(3-p["shift"])%3:(p["kind"]%3==0?2:1)*p["scale"];
        return UaeG10TrigScalar.Rational(n,1);
    }
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? raw){
        if(!Valid(f,p))return false;
        int k=p["kind"],shift=p["shift"],scale=p["scale"];
        if(f==Coefficient){
            // Evaluate at a nonzero exact Pythagorean ratio, independently from the coefficient formatter.
            const double s=3.0/5,c=4.0/5;
            double n=k%3 switch {0=>(scale*Math.Pow(s+c,2)-scale)/(s*c),1=>scale*(1-c*c)/(s*s),_=>scale*(1/(c*c)-s*s/(c*c))};
            return UaeG10TrigScalar.Matches((long)Math.Round(n),1,raw);
        }
        // Each authored choice identity is symbolic; independent evaluation distinguishes the three bounded options.
        var candidates=new bool[]{true,true,true};
        foreach(var (s,c) in new[]{(3.0/5,4.0/5),(5.0/13,12.0/13),(-4.0/5,3.0/5)}){
            double t=s/c,sec=1/c;
            (double lhs,double[] rhs)=k switch {
                0=>((1-c*c)/s,new[]{s,c,1.0}),
                1=>((1-s*s)/c,new[]{c,s,1.0}),
                2=>(1+t*t,new[]{sec*sec,t*t,1.0}),
                3=>((sec-t)*(sec+t),new[]{1.0,sec*sec,t*t}),
                4=>((1-c)*(1+c),new[]{s*s,c*c,1.0}),
                _=>((s+c)*(s+c)-1,new[]{2*s*c,s*s,1.0})
            };
            for(int i=0;i<3;i++) candidates[i]&=Math.Abs(lhs-rhs[i])<1e-10;
        }
        if(candidates.Count(x=>x)!=1)return false;
        int index=Array.IndexOf(candidates,true);
        return UaeG10TrigScalar.Matches(1+(index+3-shift)%3,1,raw);
    }
}

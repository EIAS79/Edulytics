using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
public sealed partial class ExactSkillContractQuestionEngine {
    private static ExactProblem BuildG10TrigEquations(string family,Random random,ExactSkillQuestionDifficulty difficulty){
        int kind=random.Next(0,4),fn=random.Next(0,kind==0?3:2),j=random.Next(0,5),rhs=random.Next(-4,5),rank=0;
        var roots=UaeG10TrigEquationsResponse.Roots(kind,fn,j);
        rank=random.Next(0,roots.Length);
        string label=new[]{"sin","cos","tan"}[fn],expr=UaeG10TrigEquationsResponse.Expression(kind,fn,j);
        string prompt=family switch{
            UaeG10TrigEquationsResponse.Count=>$"كم حلًا للمعادلة {expr} في [0°,360°)؟",
            UaeG10TrigEquationsResponse.Rank=>$"حل {expr} في [0°,360°)، ثم اكتب قياس الحل رقم {rank+1} بالدرجات بعد ترتيب الحلول تصاعديًا.",
            UaeG10TrigEquationsResponse.Exists=>$"هل للمعادلة {(fn==2?"tan":label)} θ={rhs}/2 حل حقيقي؟ اكتب 1 لنعم و0 للا.",
            _=>$"أوجد الدور الأساسي للدالة y={label} θ بالدرجات."
        };
        return Problem(family,prompt,"حدد المجال والفترة، واستعمل جميع الفروع أو حل العوامل ثم تحقق.",AssessmentItemType.Numeric,("kind",kind),("fn",fn),("j",j),("rank",rank),("rhs",rhs));
    }
}
internal static class UaeG10TrigEquationsResponse {
    internal const string Count="uae.g10.general.u14.l14.bounded_root_count",Rank="uae.g10.general.u14.l14.bounded_root_by_rank",Exists="uae.g10.general.u14.l14.real_solution_exists",Period="uae.g10.general.u14.l14.function_period_degrees";
    internal static bool Supports(string? f)=>f is Count or Rank or Exists or Period;
    private static bool Valid(string f,IReadOnlyDictionary<string,int> p)=>Supports(f)&&p.Count==5&&p.TryGetValue("kind",out int k)&&k>=0&&k<4&&p.TryGetValue("fn",out int fn)&&fn>=0&&fn<(k==0?3:2)&&p.TryGetValue("j",out int j)&&j>=0&&j<5&&p.TryGetValue("rank",out int r)&&r>=0&&r<Roots(k,fn,j).Length&&p.TryGetValue("rhs",out int rhs)&&rhs>=-4&&rhs<=4;
    internal static string Expression(int k,int fn,int j){
        string f=new[]{"sin","cos","tan"}[fn],v=fn==2?new[]{"−1","0","1"}[j%3]:new[]{"−1","−1/2","0","1/2","1"}[j];
        return k switch{0=>$"{f} θ={v}",1=>$"{f}(2θ)={v}",2=>$"{f} θ({f} θ−({v}))=0",_=>$"{f}²θ={(j%2==0?"1/4":"1")}"};
    }
    private static int[] BaseRoots(int fn,int j)=>fn switch{
        0=>j switch{0=>new[]{270},1=>new[]{210,330},2=>new[]{0,180},3=>new[]{30,150},_=>new[]{90}},
        1=>j switch{0=>new[]{180},1=>new[]{120,240},2=>new[]{90,270},3=>new[]{60,300},_=>new[]{0}},
        _=>(j%3) switch{0=>new[]{135,315},1=>new[]{0,180},_=>new[]{45,225}}
    };
    internal static int[] Roots(int k,int fn,int j){
        var b=BaseRoots(fn,j);
        return (k switch{0=>b,1=>b.SelectMany(x=>new[]{x/2,(x+360)/2}).ToArray(),2=>b.Concat(BaseRoots(fn,2)).ToArray(),_=>BaseRoots(fn,j%2==0?1:0).Concat(BaseRoots(fn,j%2==0?3:4)).ToArray()}).Distinct().OrderBy(x=>x).ToArray();
    }
    internal static string Format(string f,IReadOnlyDictionary<string,int> p){
        if(!Valid(f,p))throw new InvalidOperationException("Invalid equation parameters");
        var roots=Roots(p["kind"],p["fn"],p["j"]);
        int n=f==Count?roots.Length:f==Rank?roots[p["rank"]]:f==Exists?(p["fn"]==2||Math.Abs(p["rhs"])<=2?1:0):p["fn"]==2?180:360;
        return UaeG10TrigScalar.Rational(n,1);
    }
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? raw){
        if(!Valid(f,p))return false;
        int k=p["kind"],fn=p["fn"],j=p["j"];
        if(f==Exists){double rhs=p["rhs"]/2.0;return UaeG10TrigScalar.Matches(fn==2||rhs>=-1&&rhs<=1?1:0,1,raw);}
        if(f==Period)return UaeG10TrigScalar.Matches(fn==2?(long)Math.Round(Math.PI*180/Math.PI):(long)Math.Round(2*Math.PI*180/Math.PI),1,raw);
        var roots=new List<int>();double target=fn==2?j%3-1:(j-2)/2.0;
        for(int x=0;x<360;x++){
            double theta=x*Math.PI/180*(k==1?2:1);
            if(fn==2&&Math.Abs(Math.Cos(theta))<1e-10)continue;
            double v=fn==0?Math.Sin(theta):fn==1?Math.Cos(theta):Math.Tan(theta);
            double res=k==2?v*(v-target):k==3?v*v-(j%2==0?0.25:1):v-target;
            if(Math.Abs(res)<1e-10)roots.Add(x);
        }
        if(p["rank"]>=roots.Count)return false;
        return UaeG10TrigScalar.Matches(f==Count?roots.Count:roots[p["rank"]],1,raw);
    }
}

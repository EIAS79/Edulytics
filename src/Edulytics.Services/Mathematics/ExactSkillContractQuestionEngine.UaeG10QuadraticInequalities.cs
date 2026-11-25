using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>Owner-source Grade 10 U04L06. Bounded interval/sign questions
/// only; full interval notation/graphs are teacher-scored. Stage D NOT RUN.</summary>
public sealed partial class ExactSkillContractQuestionEngine
{
    private static ExactProblem BuildG10QuadraticInequality(
        string family,Random random,ExactSkillQuestionDifficulty difficulty)
    {
        int lim=difficulty==ExactSkillQuestionDifficulty.Challenge?8:6;
        int r1=random.Next(-lim,lim+1),r2;
        do r2=random.Next(-lim,lim+1);while(r2==r1);
        if(r1>r2)(r1,r2)=(r2,r1);
        int a;
        do a=random.Next(-4,5);while(a==0);
        int q=random.Next(0,4),x=random.Next(-9,10);
        string sign=q switch{0=>">",1=>"≥",2=>"<",_=>"≤"};
        string inequality=$"{a}(س−({r1}))(س−({r2})){sign}٠";
        string prompt=family switch
        {
            UaeG10QuadraticInequalityResponse.ProbeFamily=>
                $"في المتباينة {inequality}، هل س={x} في مجموعة الحل؟ أجب ١ للنعم أو ٠ للا.",
            UaeG10QuadraticInequalityResponse.MiddleFamily=>
                $"في المتباينة {inequality}، هل كل عدد في الفاصل المفتوح ({r1}،{r2}) يحققها؟ أجب ١ للنعم أو ٠ للا.",
            _=>$"في المتباينة {inequality}، هل تدخل نقطتا الجذر {r1} و{r2} ضمن الحل؟ اكتب ١ للنعم و٠ للا."
        };
        return Problem(family,prompt,
            "افحص إشارة المعامل والفواصل بين الجذرين وخارجهما؛ الحدود تدخل فقط عند ≥ أو ≤.",
            AssessmentItemType.Numeric,("a",a),("r1",r1),("r2",r2),("q",q),("x",x));
    }
}
internal static class UaeG10QuadraticInequalityResponse
{
    internal const string ProbeFamily="uae.g10.general.u04.l06.probe_truth";
    internal const string MiddleFamily="uae.g10.general.u04.l06.between_roots_truth";
    internal const string BoundaryFamily="uae.g10.general.u04.l06.roots_included";
    internal static bool Supports(string? f)=>f is ProbeFamily or MiddleFamily or BoundaryFamily;
    private static bool TryExpected(string family,IReadOnlyDictionary<string,int> p,out int expected)
    {
        expected=0;
        string[] keys=["a","r1","r2","q","x"];
        if(!Supports(family)||p.Count!=keys.Length||keys.Any(k=>!p.ContainsKey(k))||
           p["a"] is <-4 or >4 or 0||p["r1"] is <-8 or >8||
           p["r2"] is <-8 or >8||p["r1"]>=p["r2"]||
           p["q"] is <0 or >3||p["x"] is <-9 or >9)return false;
        int q=p["q"];
        if(family==BoundaryFamily){expected=(q==1||q==3)?1:0;return true;}
        long value=family==MiddleFamily
            ? -Math.Sign(p["a"])
            : (long)p["a"]*(p["x"]-p["r1"])*(p["x"]-p["r2"]);
        expected=q switch{0=>value>0?1:0,1=>value>=0?1:0,2=>value<0?1:0,_=>value<=0?1:0};
        return true;
    }
    internal static string Format(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!TryExpected(family,p,out int n))throw new InvalidOperationException("Invalid G10 inequality parameters");
        return n.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? response)
    {
        if(!TryExpected(family,p,out int correct)||string.IsNullOrWhiteSpace(response)||response.Length>20)return false;
        string raw=string.Concat(response.Trim().Select(ch=>ch switch
        {
            >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
            >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
            '−' or '–'=>'-',_=>ch
        }));
        return int.TryParse(raw,NumberStyles.AllowLeadingSign,CultureInfo.InvariantCulture,out int actual)&&actual==correct;
    }
}

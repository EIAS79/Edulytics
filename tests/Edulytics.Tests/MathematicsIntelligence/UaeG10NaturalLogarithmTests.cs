using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using System.Globalization;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG10NaturalLogarithmTests
{
    const string Code="PED:UAE-MOE-MATH:L10:GENERAL:UNIFIED:U05:L07:C048";
    [InlineData("uae.g10.general.u05.l07.convert_equivalent",LessonPracticeInteractionKind.StructuredEntry)]
    [InlineData("uae.g10.general.u05.l07.inverse_identity",LessonPracticeInteractionKind.IntegerKeypad)]
    [InlineData("uae.g10.general.u05.l07.ln_value",LessonPracticeInteractionKind.NumericEntry)]
    [InlineData("uae.g10.general.u05.l07.ln_condense_ratio",LessonPracticeInteractionKind.FractionEntry)]
    [InlineData("uae.g10.general.u05.l07.ln_expand_vector",LessonPracticeInteractionKind.StructuredEntry)]
    [InlineData("uae.g10.general.u05.l07.natural_exponential_equation",LessonPracticeInteractionKind.NumericEntry)]
    [InlineData("uae.g10.general.u05.l07.positive_rhs_possible",LessonPracticeInteractionKind.IntegerKeypad)]
    [InlineData("uae.g10.general.u05.l07.natural_log_equation",LessonPracticeInteractionKind.NumericEntry)]
    [InlineData("uae.g10.general.u05.l07.natural_exponential_interval",LessonPracticeInteractionKind.StructuredEntry)]
    [InlineData("uae.g10.general.u05.l07.natural_log_interval",LessonPracticeInteractionKind.StructuredEntry)]
    [InlineData("uae.g10.general.u05.l07.even_power_interval",LessonPracticeInteractionKind.StructuredEntry)]
    [InlineData("uae.g10.general.u05.l07.reciprocal_ln_interval",LessonPracticeInteractionKind.StructuredEntry)]
    [InlineData("uae.g10.general.u05.l07.continuous_balance",LessonPracticeInteractionKind.NumericEntry)]
    [InlineData("uae.g10.general.u05.l07.continuous_principal",LessonPracticeInteractionKind.NumericEntry)]
    [InlineData("uae.g10.general.u05.l07.continuous_time",LessonPracticeInteractionKind.NumericEntry)]
    [InlineData("uae.g10.general.u05.l07.continuous_rate",LessonPracticeInteractionKind.NumericEntry)]
    [InlineData("uae.g10.general.u05.l07.continuous_depreciation",LessonPracticeInteractionKind.NumericEntry)]
    [InlineData("uae.g10.general.u05.l07.continuous_half_time",LessonPracticeInteractionKind.NumericEntry)]
    [InlineData("uae.g10.general.u05.l07.continuous_threshold",LessonPracticeInteractionKind.NumericEntry)]
    [InlineData("uae.g10.general.u05.l07.cooling_constant",LessonPracticeInteractionKind.NumericEntry)]
    [InlineData("uae.g10.general.u05.l07.cooling_temperature",LessonPracticeInteractionKind.NumericEntry)]
    [InlineData("uae.g10.general.u05.l07.cooling_time",LessonPracticeInteractionKind.NumericEntry)]
    [InlineData("uae.g10.general.u05.l07.exponential_substitution",LessonPracticeInteractionKind.NumericEntry)]
    [InlineData("uae.g10.general.u05.l06.exponential_equation",LessonPracticeInteractionKind.NumericEntry)]
    [Theory]
    public void FamilyDispatchRendererAndBoundaryOracle(string family,LessonPracticeInteractionKind kind)
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var interaction));Assert.Equal(kind,interaction!.InteractionKind);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(int seed in new[]{48,20261126,20261127,723,876})
        foreach(var task in engine.Generate(Code,Code,[family],ExactSkillQuestionDifficulty.Standard,4,seed,[]))
        {
            var a=task.Parameters;Assert.True(ExactSkillContractQuestionEngine.Verify(family,a,task.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,a,"wrong"));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,a,""));
            string arabic=string.Concat(task.CorrectAnswer.Select(ch=>ch is >= '0' and <= '9'?(char)('٠'+ch-'0'):ch=='-'?'−':ch==','?'،':ch=='.'?'٫':ch));
            Assert.True(ExactSkillContractQuestionEngine.Verify(family,a,arabic));
            var invalid=a.ToDictionary(x=>x.Key,x=>x.Value,StringComparer.Ordinal);invalid["b"]=1;
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,invalid,task.CorrectAnswer));
            if(family.EndsWith(".exponential_equation")&&family.Contains(".l06."))
            {
                double basis=a["dir"]==0?a["b"]:1.0/a["b"];
                Assert.InRange(Math.Abs(Number(task.CorrectAnswer)-(Math.Log(a["v"])/Math.Log(basis)-a["n"])/a["m"]),0,.0000500001);continue;
            }
            double b=a["b"],m=a["m"],n=a["n"],h=a["h"],p=a["p"],v=a["v"],r=a["r"]/100.0,t=a["t"],z=a["z"],k=-Math.Log(z/5)/t;
            string suffix=family.Split('.').Last();
            if(suffix=="convert_equivalent")
            {Assert.Equal(a["dir"]==0?$"ln({v})={m}x":$"e^({m})=x",task.CorrectAnswer);continue;}
            if(suffix=="ln_expand_vector"){Assert.Equal($"(1,{Math.Abs(m)},{-z})",task.CorrectAnswer);continue;}
            if(suffix=="ln_condense_ratio"){var x=task.CorrectAnswer.Split('/');Assert.Equal(Math.Pow(b,Math.Abs(m))*a["c"]/v,Number(x[0])/(x.Length==2?Number(x[1]):1),10);continue;}
            if(suffix.EndsWith("_interval"))
            {
                // Check membership at interior points rather than merely the answer string.
                var raw=task.CorrectAnswer;var parts=raw.Split('∪');
                bool Member(double x)=>parts.Any(s=>{var q=s[1..^1].Split(',');double lo=q[0]=="-∞"?double.NegativeInfinity:Number(q[0]),hi=q[1]=="∞"?double.PositiveInfinity:Number(q[1]);return (x>lo||x==lo&&s[0]=='[')&&(x<hi||x==hi&&s[^1]==']');});
                bool Rel(double a0,double b0)=>a["op"] switch{0=>a0<b0,1=>a0<=b0,2=>a0>b0,_=>a0>=b0};
                foreach(double x in new[]{-101.123,-10.123,-3.123,-.123,.123,1.123,3.123,5.123,10.123,101.123})
                {
                    bool ok=suffix switch{"natural_exponential_interval"=>Rel(b*Math.Exp(m*x)+n,b*v+n),"natural_log_interval"=>x>-h&&Rel(m*Math.Log(b*(x+h)),n),"even_power_interval"=>x!=0&&Rel(2*z*Math.Log(Math.Abs(x)),v),"reciprocal_ln_interval"=>x>h&&x!=h+1&&-b/Math.Log(x-h)<=a["c"],_=>throw new InvalidOperationException()};
                    Assert.Equal(ok,Member(x));
                }
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,a,"(-∞,∞)"));continue;
            }
            double expected=suffix switch{"inverse_identity"=>a["dir"]==0?m:v,"ln_value"=>Math.Log(v/b),"natural_exponential_equation"=>Math.Log(v)/m,"positive_rhs_possible"=>a["dir"]==0?1:0,"natural_log_equation"=>Math.Exp(n/m)/b-h,"continuous_balance"=>p*Math.Exp(r*t),"continuous_principal"=>p*v*Math.Exp(-r*t),"continuous_time"=>Math.Log(v)/r,"continuous_rate"=>100*Math.Log(v)/t,"continuous_depreciation"=>5*p*Math.Exp(-r*z/2),"continuous_half_time"=>Math.Log(2)/r,"continuous_threshold"=>Math.Log(5)/r,"cooling_constant"=>k,"cooling_temperature"=>20+10*b*Math.Pow(z/5,(2*t+z)/t),"cooling_time"=>Math.Log(v+2)/k,"exponential_substitution"=>Math.Log(v)/Math.Log(b),_=>throw new InvalidOperationException()};
            int d=suffix=="cooling_constant"?6:suffix is "continuous_balance" or "continuous_principal" or "continuous_depreciation" or "cooling_temperature"?2:4;
            Assert.InRange(Math.Abs(expected-Number(task.CorrectAnswer)),0,.5/Math.Pow(10,d)+1e-10);
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,a,(Number(task.CorrectAnswer)+1).ToString("F"+d,CultureInfo.InvariantCulture)));
        }
    }
    static double Number(string x)=>double.Parse(x,CultureInfo.InvariantCulture);
}

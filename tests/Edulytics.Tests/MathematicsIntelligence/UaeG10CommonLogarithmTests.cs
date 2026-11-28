using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using System.Globalization;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG10CommonLogarithmTests
{
    const string Code="PED:UAE-MOE-MATH:L10:GENERAL:UNIFIED:U05:L06:C047";
    [InlineData("uae.g10.general.u05.l06.common_log_rounded",LessonPracticeInteractionKind.NumericEntry)]
    [InlineData("uae.g10.general.u05.l06.change_base_rounded",LessonPracticeInteractionKind.NumericEntry)]
    [InlineData("uae.g10.general.u05.l06.exponential_equation",LessonPracticeInteractionKind.NumericEntry)]
    [InlineData("uae.g10.general.u05.l06.exponential_two_bases",LessonPracticeInteractionKind.NumericEntry)]
    [InlineData("uae.g10.general.u05.l06.exponential_inequality",LessonPracticeInteractionKind.StructuredEntry)]
    [InlineData("uae.g10.general.u05.l06.exponential_quadratic_roots",LessonPracticeInteractionKind.StructuredEntry)]
    [InlineData("uae.g10.general.u05.l06.sound_intensity_micro",LessonPracticeInteractionKind.NumericEntry)]
    [InlineData("uae.g10.general.u05.l06.sound_reduction_percent",LessonPracticeInteractionKind.NumericEntry)]
    [InlineData("uae.g10.general.u05.l06.energy_scaled",LessonPracticeInteractionKind.NumericEntry)]
    [InlineData("uae.g10.general.u05.l06.log_model_population",LessonPracticeInteractionKind.NumericEntry)]
    [InlineData("uae.g10.general.u05.l06.log_model_time",LessonPracticeInteractionKind.NumericEntry)]
    [InlineData("uae.g10.general.u05.l06.observed_growth_rate",LessonPracticeInteractionKind.NumericEntry)]
    [InlineData("uae.g10.general.u05.l06.observed_growth_time",LessonPracticeInteractionKind.NumericEntry)]
    [InlineData("uae.g10.general.u05.l06.music_cents",LessonPracticeInteractionKind.NumericEntry)]
    [InlineData("uae.g10.general.u05.l06.music_frequency",LessonPracticeInteractionKind.NumericEntry)]
    [InlineData("uae.g10.general.u05.l06.ph_from_concentration",LessonPracticeInteractionKind.NumericEntry)]
    [InlineData("uae.g10.general.u05.l06.mass_ppm",LessonPracticeInteractionKind.NumericEntry)]
    [InlineData("uae.g10.general.u05.l06.lab_intersection",LessonPracticeInteractionKind.NumericEntry)]
    [InlineData("uae.g10.general.u05.l06.lab_interval",LessonPracticeInteractionKind.StructuredEntry)]
    [Theory]
    public void FamilyDispatchRendererAndBoundaryOracle(string family,LessonPracticeInteractionKind kind)
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var interaction));Assert.Equal(kind,interaction!.InteractionKind);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(int seed in new[]{47,20261126,20261127,723,876})
        foreach(var task in engine.Generate(Code,Code,[family],ExactSkillQuestionDifficulty.Standard,4,seed,[]))
        {
            var a=task.Parameters;Assert.True(ExactSkillContractQuestionEngine.Verify(family,a,task.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,a,"wrong"));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,a,""));
            string arabic=string.Concat(task.CorrectAnswer.Select(ch=>ch is >= '0' and <= '9'?(char)('٠'+ch-'0'):ch=='-'?'−':ch==','?'،':ch=='.'?'٫':ch));
            Assert.True(ExactSkillContractQuestionEngine.Verify(family,a,arabic));
            var invalid=a.ToDictionary(x=>x.Key,x=>x.Value,StringComparer.Ordinal);invalid["b"]=1;
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,invalid,task.CorrectAnswer));
            double b=a["dir"]==0?a["b"]:1.0/a["b"],c=a["c"],m=a["m"],p=a["p"],n=a["n"],q=a["q"],v=a["v"],h=a["h"],t=a["t"];
            double ratio=(100*a["b"]+10*v)/(100*a["b"]);
            string suffix=family.Split('.').Last();
            if(suffix=="exponential_quadratic_roots")
            {
                double x=Math.Sqrt(Math.Log(v)/Math.Log(a["b"]));Assert.Equal(-x,Number(task.CorrectAnswer.Trim('(',')').Split(',')[0]),4);Assert.Equal(x,Number(task.CorrectAnswer.Trim('(',')').Split(',')[1]),4);
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,a,Number(task.CorrectAnswer.Trim('(',')').Split(',')[1]).ToString(CultureInfo.InvariantCulture)));continue;
            }
            if(suffix=="exponential_inequality")
            {
                double slope=m*Math.Log(b)-(a["k"]==0?0:p*Math.Log(c)),x=((a["k"]==0?Math.Log(v):q*Math.Log(c))-n*Math.Log(b))/slope;
                var raw=task.CorrectAnswer;var parts=raw[1..^1].Split(',');int op=slope<0?a["op"]^2:a["op"];
                Assert.Equal(x,Number(parts[op<2?1:0]),4);Assert.Equal(op<2?"-∞":"∞",parts[op<2?0:1]);
                Assert.Equal(op%2==1?(op<2?']':'['):(op<2?')':'('),op<2?raw[^1]:raw[0]);continue;
            }
            if(suffix=="lab_interval"){Assert.Equal(a["b"]<a["c"]?$"({-h},{1-h})":$"({1-h},∞)",task.CorrectAnswer);continue;}
            double expected=suffix switch{
                "common_log_rounded"=>Math.Log(v/Math.Pow(10,a["k"]))/Math.Log(10),
                "change_base_rounded"=>Math.Log(v/Math.Pow(10,a["k"]))/Math.Log(b),
                "exponential_equation"=>(Math.Log(v)/Math.Log(b)-n)/m,
                "exponential_two_bases"=>(q*Math.Log(c)-n*Math.Log(b))/(m*Math.Log(b)-p*Math.Log(c)),
                "sound_intensity_micro"=>Math.Exp(((50+h)/10-6)*Math.Log(10)),
                "sound_reduction_percent"=>100*(1-Math.Exp(-h/10*Math.Log(10))),
                "energy_scaled"=>Math.Exp((11.8+1.5*(3+h/10)-16)*Math.Log(10)),
                "log_model_population"=>10*a["b"]*Math.Log(v+2)/Math.Log(c),"log_model_time"=>Math.Exp(h/2*Math.Log(c))-2,
                "observed_growth_rate"=>100*(Math.Exp(Math.Log(ratio)/t)-1),"observed_growth_time"=>t*Math.Log(2)/Math.Log(ratio),
                "music_cents"=>1200*Math.Log((100*a["b"]+v)/(100*a["b"]))/Math.Log(2),
                "music_frequency"=>100*a["b"]*Math.Exp((a["dir"]==0?1:-1)*h*25/1200*Math.Log(2)),
                "ph_from_concentration"=>-Math.Log(v*Math.Pow(10,-h-3))/Math.Log(10),"mass_ppm"=>v/(100*t),"lab_intersection"=>1-h,
                _=>throw new InvalidOperationException()};
            Assert.InRange(Math.Abs(expected-Number(task.CorrectAnswer)),0,.0000500001);
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,a,(Number(task.CorrectAnswer)+.001).ToString("0.0000",CultureInfo.InvariantCulture)));
        }
    }
    static double Number(string x)=>double.Parse(x,CultureInfo.InvariantCulture);
}

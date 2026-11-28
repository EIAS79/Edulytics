using System.Globalization;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UaeG10ExponentialLogisticModelsTests
{
    private const string Code="PED:UAE-MOE-MATH:L10:GENERAL:UNIFIED:U05:L08:C049";
    private const string Prefix="uae.g10.general.u05.l08.";
    [Theory]
    [InlineData("half_life_constant",LessonPracticeInteractionKind.NumericEntry)]
    [InlineData("half_life_amount",LessonPracticeInteractionKind.NumericEntry)]
    [InlineData("decay_age",LessonPracticeInteractionKind.NumericEntry)]
    [InlineData("half_life_from_loss",LessonPracticeInteractionKind.NumericEntry)]
    [InlineData("continuous_growth_rate",LessonPracticeInteractionKind.NumericEntry)]
    [InlineData("continuous_growth_forecast",LessonPracticeInteractionKind.NumericEntry)]
    [InlineData("continuous_growth_target",LessonPracticeInteractionKind.NumericEntry)]
    [InlineData("continuous_crossing",LessonPracticeInteractionKind.NumericEntry)]
    [InlineData("logistic_initial",LessonPracticeInteractionKind.NumericEntry)]
    [InlineData("logistic_limit",LessonPracticeInteractionKind.IntegerKeypad)]
    [InlineData("logistic_value",LessonPracticeInteractionKind.NumericEntry)]
    [InlineData("logistic_time",LessonPracticeInteractionKind.NumericEntry)]
    [InlineData("logistic_rate",LessonPracticeInteractionKind.NumericEntry)]
    [InlineData("logistic_reachable",LessonPracticeInteractionKind.IntegerKeypad)]
    [InlineData("exponential_logistic_relation",LessonPracticeInteractionKind.IntegerKeypad)]
    [InlineData("spreadsheet_balance",LessonPracticeInteractionKind.NumericEntry)]
    [InlineData("spreadsheet_interest",LessonPracticeInteractionKind.NumericEntry)]
    [InlineData("exponential_logistic_crossing",LessonPracticeInteractionKind.NumericEntry)]
    public void FamilyDispatchRendererAndIndependentModel(string suffix, LessonPracticeInteractionKind kind)
    {
        string family=Prefix+suffix;
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var interaction));
        Assert.Equal(kind,interaction!.InteractionKind);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(int seed in new[]{49,20261126,20261127,723})
        {
            var tasks=engine.Generate(Code,Code,[family],ExactSkillQuestionDifficulty.Standard,4,seed,[]);
            Assert.Equal(4,tasks.Count);
            foreach(var task in tasks)
            {
                var x=task.Parameters;
                double a=x["a"],b=x["b"],c=x["c"],h=x["h"],t=x["t"],v=x["v"],r=x["r"]/100.0,q=x["q"]/100.0,n=x["n"],p=x["p"],ratio=(100*a+10*b)/(100*a);
                int digits=suffix=="half_life_constant"?8:suffix is "continuous_growth_rate" or "logistic_rate"?6:suffix.StartsWith("spreadsheet_")?2:suffix is "logistic_limit" or "logistic_reachable" or "exponential_logistic_relation"?0:4;
                double actual=double.Parse(task.CorrectAnswer,CultureInfo.InvariantCulture);
                if(suffix=="exponential_logistic_crossing")
                {
                    Assert.True(actual>=0);
                    double d=.00005;
                    double F(double z)=>c*Math.Exp(r*z)/(2*(a+1))-c/(1+a*Math.Exp(-q*z));
                    Assert.True(F(actual-d)<=0&&F(actual+d)>=0);
                }
                else
                {
                    double expected=suffix switch{
                        "half_life_constant"=>Math.Log(2)/h,
                        "half_life_amount"=>p*Math.Pow(2,-100*t/h),
                        "decay_age"=>h*Math.Log(10/v,2),
                        "half_life_from_loss"=>100*t/Math.Log(10/v,2),
                        "continuous_growth_rate"=>Math.Log(ratio)/t,
                        "continuous_growth_forecast"=>100*a*Math.Pow(ratio,(2*t+n)/t),
                        "continuous_growth_target"=>t*Math.Log(2*ratio)/Math.Log(ratio),
                        "continuous_crossing"=>Math.Log(ratio)/(r-q),
                        "logistic_initial"=>c/(1+a),"logistic_limit"=>c,
                        "logistic_value"=>c/(1+a*Math.Exp(-r*t)),
                        "logistic_time"=>Math.Log(a*v)/r,"logistic_rate"=>Math.Log(a*v)/t,
                        "logistic_reachable"=>x["mode"]>=2?1:0,"exponential_logistic_relation"=>1,
                        "spreadsheet_balance"=>p*Math.Pow(1+x["rate"]/10000.0,n),
                        "spreadsheet_interest"=>p*Math.Pow(1+x["rate"]/10000.0,n-1)*(x["rate"]/10000.0),
                        _=>throw new InvalidOperationException()};
                    Assert.InRange(Math.Abs(actual-expected),0,.50001*Math.Pow(10,-digits));
                }
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,x,task.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,x,(actual+1).ToString(CultureInfo.InvariantCulture)));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,x,""));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,x,"wrong"));
                string arabic=string.Concat(task.CorrectAnswer.Select(ch=>ch is >= '0' and <= '9'?(char)('٠'+ch-'0'):ch=='.'?'٫':ch));
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,x,arabic));
                var invalid=x.ToDictionary(x=>x.Key,x=>x.Value,StringComparer.Ordinal);invalid["h"]=0;
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,invalid,task.CorrectAnswer));
            }
        }
    }
    [Fact]
    public void CapacityAndSpreadsheetGoldenCases()
    {
        var p=new Dictionary<string,int>{{"a",5},{"b",5},{"c",2400},{"h",800},{"t",8},{"v",3},{"r",8},{"q",3},{"n",30},{"rate",800},{"p",20000},{"mode",1}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(Prefix+"logistic_limit",p,"2400"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Prefix+"logistic_initial",p,"400"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Prefix+"logistic_reachable",p,"0"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Prefix+"logistic_reachable",p,"1"));
        p["mode"]=3;Assert.True(ExactSkillContractQuestionEngine.Verify(Prefix+"logistic_reachable",p,"1"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Prefix+"exponential_logistic_crossing",p,"12.4128"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Prefix+"exponential_logistic_crossing",p,"12.4129"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Prefix+"spreadsheet_balance",p,"201253.14"));
    }
}

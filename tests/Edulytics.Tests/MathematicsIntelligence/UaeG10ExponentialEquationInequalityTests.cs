using System.Globalization;
using Edulytics.Core.Enums;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Source-specific exact engine extension; Stage D tests authored, NOT RUN.</summary>
public sealed class UaeG10ExponentialEquationInequalityTests
{
    private const string Code="PED:UAE-MOE-MATH:L10:GENERAL:UNIFIED:U05:L02:C043";
    private const string Prefix="uae.g10.general.u05.l02.";
    [Theory]
    [InlineData("equation_rational",LessonPracticeInteractionKind.FractionEntry)]
    [InlineData("inequality_rational_boundary",LessonPracticeInteractionKind.StructuredEntry)]
    [InlineData("model_factor_rounded",LessonPracticeInteractionKind.NumericEntry)]
    [InlineData("model_forecast_rounded",LessonPracticeInteractionKind.IntegerKeypad)]
    [InlineData("compound_balance",LessonPracticeInteractionKind.NumericEntry)]
    [InlineData("equivalent_period_factor",LessonPracticeInteractionKind.FractionEntry)]
    [InlineData("cooling_value",LessonPracticeInteractionKind.IntegerKeypad)]
    [InlineData("cooling_threshold",LessonPracticeInteractionKind.IntegerKeypad)]
    [InlineData("power_law_prediction",LessonPracticeInteractionKind.IntegerKeypad)]
    [InlineData("like_power_sum",LessonPracticeInteractionKind.IntegerKeypad)]
    [InlineData("model_parameters",LessonPracticeInteractionKind.StructuredEntry)]
    [InlineData("power_law_coefficient",LessonPracticeInteractionKind.IntegerKeypad)]
    public void FamilyDispatchRendererAndIndependentAnswers(string suffix,LessonPracticeInteractionKind kind)
    {
        string family=Prefix+suffix;
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var interaction));
        Assert.Equal(kind,interaction!.InteractionKind);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(int seed in new[]{20261125,20261126,20261127})
        {
            var tasks=engine.Generate(Code,Code,[family],ExactSkillQuestionDifficulty.Standard,4,seed,[]);
            Assert.Equal(4,tasks.Count);
            foreach(var task in tasks)
            {
                var p=task.Parameters;
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,task.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"wrong"));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,""));
                var arabic=string.Concat(task.CorrectAnswer.Select(ch=>ch is >= '0' and <= '9'?(char)('٠'+ch-'0'):ch=='x'?'س':ch));
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,arabic));
                if(suffix=="equation_rational"||suffix=="inequality_rational_boundary")
                {
                    double boundary=(double)(p["rp"]*p["s"]-p["lp"]*p["n"])/(p["lp"]*p["m"]-p["rp"]*p["r"]);
                    string raw=task.CorrectAnswer;
                    if(suffix=="inequality_rational_boundary")
                    {
                        Assert.Equal('x',raw[0]);
                        int relation=raw[1] switch{'>'=>0,'≥'=>1,'<'=>2,'≤'=>3,_=>-1};
                        int sign=p["lp"]*p["m"]-p["rp"]*p["r"];
                        Assert.Equal(sign>0?p["op"]:p["op"]^2,relation);
                        string wrong="x"+(raw[1] is '>' or '≥'?"<":">")+raw[2..];
                        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,wrong));
                        raw=raw[2..];
                    }
                    Assert.Equal(boundary,Number(raw),10);
                    var invalid=p.ToDictionary(x=>x.Key,x=>x.Value,StringComparer.Ordinal);
                    invalid["base"]=1;Assert.False(ExactSkillContractQuestionEngine.Verify(family,invalid,task.CorrectAnswer));
                }
                else if(suffix=="model_parameters")
                {
                    string[] pair=task.CorrectAnswer.Trim('(',')').Split(',');
                    Assert.Equal(2,pair.Length);
                    Assert.Equal((double)p["start"],Number(pair[0]));
                    Assert.Equal(Math.Round(Math.Pow((double)p["end"]/p["start"],1d/p["observed"]),3,MidpointRounding.AwayFromZero),Number(pair[1]),7);
                    Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,$"({p["start"]+1},{pair[1]})"));
                }
                else
                {
                    double expected=suffix switch
                    {
                        "model_factor_rounded"=>Math.Round(Math.Pow((double)p["end"]/p["start"],1d/p["observed"]),3,MidpointRounding.AwayFromZero),
                        "model_forecast_rounded"=>Math.Round(p["start"]*Math.Pow((double)p["end"]/p["start"],(double)p["target"]/p["observed"]),MidpointRounding.AwayFromZero),
                        "compound_balance"=>Math.Round(p["initial"]*Math.Pow(1+p["rate"]/10000d/p["frequency"],p["frequency"]*p["years"]),2,MidpointRounding.AwayFromZero),
                        "equivalent_period_factor"=>1+p["percent"]/100d,
                        "cooling_value"=>p["ambient"]+p["delta"]*Math.Pow(0.5,p["step"]),
                        "cooling_threshold"=>p["step"]*p["period"],
                        "power_law_prediction"=>p["coeff"]*Math.Pow(p["b"],p["num"]),
                        "power_law_coefficient"=>p["coeff"],
                        "like_power_sum"=>Math.Log(Math.Pow(p["base"],p["exponent"])*Math.Pow(p["base"],p["multiplicity"]),p["base"]),
                        _=>throw new InvalidOperationException()
                    };
                    Assert.Equal(expected,Number(task.CorrectAnswer),7);
                }
            }
        }
    }
    private static double Number(string s)
    {
        string[] parts=s.Split('/');
        return parts.Length==2?double.Parse(parts[0],CultureInfo.InvariantCulture)/double.Parse(parts[1],CultureInfo.InvariantCulture):double.Parse(s,CultureInfo.InvariantCulture);
    }

    [Fact]
    public void ExplicitReciprocalAndStrictnessBoundary()
    {
        var p=new Dictionary<string,int>{{"base",2},{"lp",-1},{"rp",1},{"m",1},{"r",0},{"n",1},{"s",-4},{"op",0}};
        string f=Prefix+"inequality_rational_boundary";
        Assert.True(ExactSkillContractQuestionEngine.Verify(f,p,"س<٣"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(f,p,"x<6/2"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(f,p,"x<=3"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(f,p,"x>3"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(f,p,"x<4"));
    }
    [Fact]
    public void ModelPairAndNonUnitRationalBase()
    {
        var model=new Dictionary<string,int>{{"start",400},{"end",900},{"observed",2},{"target",3}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(Prefix+"model_parameters",model,"(٤٠٠،١٫٥٠٠)"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Prefix+"model_parameters",model,"(400,1.499)"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Prefix+"model_parameters",model,"(401,1.5)"));
        var rational=new Dictionary<string,int>{{"base",3},{"baseDen",2},{"lp",-1},{"rp",1},{"m",1},{"r",0},{"n",1},{"s",-4},{"op",0}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(Prefix+"inequality_rational_boundary",rational,"x<3"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Prefix+"inequality_rational_boundary",rational,"x>3"));
        rational["baseDen"]=3;
        Assert.False(ExactSkillContractQuestionEngine.Verify(Prefix+"inequality_rational_boundary",rational,"x<3"));
    }
    [Fact]
    public void RoundingAndPeriodBoundaries()
    {
        var tiny=new Dictionary<string,int>{{"start",1000},{"end",50},{"observed",2},{"target",6}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(Prefix+"model_forecast_rounded",tiny,"٠"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Prefix+"model_forecast_rounded",tiny,"1"));
        var compound=new Dictionary<string,int>{{"initial",1000},{"rate",800},{"frequency",4},{"years",1}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(Prefix+"compound_balance",compound,"١٠٨٢٫٤٣"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Prefix+"compound_balance",compound,"1082.44"));
        var period=new Dictionary<string,int>{{"percent",2},{"periods",12}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(Prefix+"equivalent_period_factor",period,"51/50"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Prefix+"equivalent_period_factor",period,"1.03"));
    }
}

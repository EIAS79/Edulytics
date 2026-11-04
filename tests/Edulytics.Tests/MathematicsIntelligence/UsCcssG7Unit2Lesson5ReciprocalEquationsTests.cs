using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>G7 U02 L05 source-scoped fixtures. NOT_RUN during A-B-C authoring.</summary>
public sealed class UsCcssG7Unit2Lesson5ReciprocalEquationsTests
{
    private const string L="PED:US-CCSS-MATH:G7:U02:L05",F="usccss.g7.u02.l05.reciprocal_proportional_equations";
    private static Dictionary<string,int> P(int mode,int feature,int variant=0)=>new()
    {
        ["mode"]=mode,["feature"]=feature,["variant"]=variant
    };
    [Fact]
    public void SelectedPracticeReusesPriorEquationFamily()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(L,out var c));
        Assert.NotNull(c);
        Assert.Equal("READY_VERIFIED",c.Readiness);
        Assert.Contains(F,c.AllowedQuestionFamilies);
        Assert.Contains("usccss.g7.u02.l04.proportional_equations",c.AllowedQuestionFamilies);
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(F));
        Assert.True(LessonPracticeContractRegistry.TryResolve(
            "PED:US-CCSS-MATH:G7:U02:L04",out _));
    }
    [Theory]
    [InlineData(0,0,0,"100")]
    [InlineData(0,0,1,"1/100")]
    [InlineData(0,1,0,"8/5")]
    [InlineData(0,1,1,"5/8")]
    [InlineData(0,2,0,"3/5")]
    [InlineData(0,2,1,"5/3")]
    [InlineData(1,0,0,"94")]
    [InlineData(1,1,1,"391/500")]
    [InlineData(1,2,1,"1239/1000")]
    [InlineData(2,2,0,"8")]
    [InlineData(2,2,1,"25/8")]
    [InlineData(3,3,0,"5")]
    [InlineData(3,3,1,"45")]
    [InlineData(4,2,0,"18")]
    [InlineData(4,2,1,"50")]
    [InlineData(5,3,0,"300")]
    [InlineData(5,3,1,"50")]
    [InlineData(6,1,0,"7/2")]
    [InlineData(6,1,1,"4000")]
    public void ReciprocalAndContextAnswersExact(int mode,int feature,int variant,string expected)
    {
        var p=P(mode,feature,variant);
        Assert.True(ExactSkillContractQuestionEngine.Verify(F,p,expected));
        Assert.False(ExactSkillContractQuestionEngine.Verify(F,p,"999"));
    }
    [Fact]
    public void InvalidParametersAndUnapprovedFamilyFailClosed()
    {
        Assert.False(ExactSkillContractQuestionEngine.Verify(F,P(7,0),"1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(F,P(0,4),"1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(F,P(0,0,2),"100"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(F,P(0,1,1),"0.62"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(F,P(0,0,1),"1/0"));
        var extra=P(0,0);extra["extra"]=1;
        Assert.False(ExactSkillContractQuestionEngine.Verify(F,extra,"100"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            "usccss.g7.u02.l06.foreign",P(0,0),"100"));
    }
    [Fact]
    public void QuestionsAreInExistingExactGeneratorAndVerifier()
    {
        var e=new ExactSkillContractQuestionEngine();
        var items=e.Generate("g7-u02-l05-abc",L,[F],
            ExactSkillQuestionDifficulty.Standard,4,7205,Array.Empty<string>());
        Assert.Equal(4,items.Count);
        foreach(var x in items)
        {
            Assert.Equal(F,x.Family);
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                F,x.Parameters,x.CorrectAnswer));
        }
    }
}

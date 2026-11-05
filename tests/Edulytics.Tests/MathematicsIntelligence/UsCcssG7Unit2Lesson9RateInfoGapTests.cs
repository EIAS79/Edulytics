using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>G7 U02:L09 A-B-C source-scoped acceptance, NOT_RUN.</summary>
public sealed class UsCcssG7Unit2Lesson9RateInfoGapTests
{
    const string L="PED:US-CCSS-MATH:G7:U02:L09",F="usccss.g7.u02.l09.missing_info_rate_comparison";
    static Dictionary<string,int> P(int m,int f,int v=0)=>new()
    {["mode"]=m,["feature"]=f,["variant"]=v};
    [Fact]
    public void LessonSelectedAndPreviousFamiliesPreserved()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(L,out var c));
        Assert.NotNull(c);
        Assert.Equal("READY_VERIFIED",c.Readiness);
        Assert.Contains(F,c.AllowedQuestionFamilies);
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(F));
        Assert.True(LessonPracticeContractRegistry.TryResolve(
            "PED:US-CCSS-MATH:G7:U02:L08",out _));
    }
    [Theory]
    [InlineData(0,0,0,"64")]
    [InlineData(0,3,0,"40")]
    [InlineData(1,1,0,"6")]
    [InlineData(2,0,0,"50000")]
    [InlineData(2,1,0,"53000")]
    [InlineData(2,2,0,"39720")]
    [InlineData(2,3,0,"40000")]
    [InlineData(3,0,0,"5000/21")]
    [InlineData(3,1,0,"265")]
    [InlineData(3,2,0,"331")]
    [InlineData(3,3,0,"800/3")]
    [InlineData(4,0,0,"2")]
    [InlineData(4,0,1,"4")]
    [InlineData(4,2,1,"1")]
    [InlineData(5,0,0,"1")]
    [InlineData(5,1,0,"0")]
    [InlineData(6,3,0,"50")]
    public void SourceComparisonsHaveExactAnswers(int m,int f,int v,string answer)
    {
        Assert.True(ExactSkillContractQuestionEngine.Verify(F,P(m,f,v),answer));
        Assert.False(ExactSkillContractQuestionEngine.Verify(F,P(m,f,v),"999"));
    }
    [Fact]
    public void OutOfScopeDataAndApproximationFailClosed()
    {
        Assert.False(ExactSkillContractQuestionEngine.Verify(F,P(7,0),"1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(F,P(0,4),"1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(F,P(0,0,2),"64"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(F,P(3,0),"238.095"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(F,P(3,0),"1/0"));
        var bad=P(0,0);bad["foreign"]=1;
        Assert.False(ExactSkillContractQuestionEngine.Verify(F,bad,"64"));
    }
    [Fact]
    public void SharedExactGeneratorSelected()
    {
        var e=new ExactSkillContractQuestionEngine();
        var qs=e.Generate("g7-u02-l09-abc",L,[F],
            ExactSkillQuestionDifficulty.Standard,4,7209,Array.Empty<string>());
        Assert.Equal(4,qs.Count);
        foreach(var q in qs)
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                q.Family,q.Parameters,q.CorrectAnswer));
    }
}

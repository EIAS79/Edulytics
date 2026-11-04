using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
// A-C authoring fixtures, Stage D tests NOT_RUN.
public sealed class UsCcssG7Unit2Lesson1ExactRatioTests
{
    const string Family="usccss.g7.u02.l01.ratio_mixtures_crescent";
    const string Lesson="PED:US-CCSS-MATH:G7:U02:L01";
    static Dictionary<string,int> P(int kind,int feature)=>new()
    {["kind"]=kind,["feature"]=feature};
    [Fact]
    public void LessonSelectsExactRatiosWithoutTouchingGrade7Unit1()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson,out var contract));
        Assert.NotNull(contract);
        Assert.Equal("READY_VERIFIED",contract.Readiness);
        Assert.Equal(new[]{Family},contract.AllowedQuestionFamilies);
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(Family));
        Assert.True(LessonPracticeContractRegistry.TryResolve(
            "PED:US-CCSS-MATH:G7:U01:L13",out _));
    }
    [Theory]
    [InlineData(0,0,"1")]
    [InlineData(0,3,"4")]
    [InlineData(1,0,"1")]
    [InlineData(1,1,"1")]
    [InlineData(1,2,"0")]
    [InlineData(1,3,"1/4")]
    [InlineData(2,0,"3")]
    [InlineData(2,1,"1")]
    [InlineData(2,2,"0")]
    [InlineData(2,3,"1")]
    [InlineData(3,0,"1")]
    [InlineData(3,1,"0")]
    [InlineData(3,2,"3/2")]
    [InlineData(3,3,"1")]
    [InlineData(4,0,"1")]
    [InlineData(4,1,"0")]
    [InlineData(4,2,"2")]
    [InlineData(4,3,"2/15")]
    public void IndependentSourceRatiosAndCorrectNegativeExamples(int k,int f,string a)
    {
        Assert.True(ExactSkillContractQuestionEngine.Verify(Family,P(k,f),a));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(k,f),"999"));
    }
    [Fact]
    public void NonEquivalentAndMalformedInputsFailClosed()
    {
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(1,3),"1/2"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(2,2),"1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(5,0),"1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(0,4),"1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(4,3),"2/0"));
        var tooMany=P(1,3);tooMany["foreign"]=1;
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,tooMany,"1/4"));
    }
    [Fact]
    public void ExactGenerationAndVerifierReturnConsistentResults()
    {
        var e=new ExactSkillContractQuestionEngine();
        var items=e.Generate("g7-u02-l01",Lesson,new[]{Family},
            ExactSkillQuestionDifficulty.Standard,4,7201,Array.Empty<string>());
        Assert.Equal(4,items.Count);
        foreach(var q in items)
        {
            Assert.Equal(Family,q.Family);
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                Family,q.Parameters,q.CorrectAnswer));
        }
    }
}

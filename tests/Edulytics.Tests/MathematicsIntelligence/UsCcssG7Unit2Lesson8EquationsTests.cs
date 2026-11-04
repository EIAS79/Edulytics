using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>G7 U02 L08 authoring fixtures; intentionally NOT_RUN in A-C.</summary>
public sealed class UsCcssG7Unit2Lesson8EquationsTests
{
    const string L="PED:US-CCSS-MATH:G7:U02:L08",F="usccss.g7.u02.l08.classify_equations";
    static Dictionary<string,int> P(int mode,int feature,int variant=0)=>new()
    {["mode"]=mode,["feature"]=feature,["variant"]=variant};
    [Fact]
    public void ExactLessonContractAndPriorFamiliesSelected()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(L,out var c));
        Assert.NotNull(c);
        Assert.Equal("READY_VERIFIED",c.Readiness);
        Assert.Contains(F,c.AllowedQuestionFamilies);
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(F));
        Assert.True(LessonPracticeContractRegistry.TryResolve(
            "PED:US-CCSS-MATH:G7:U02:L07",out _));
    }
    [Theory]
    [InlineData(0,0,0,"0")]
    [InlineData(0,1,0,"1")]
    [InlineData(0,2,0,"1")]
    [InlineData(0,3,0,"0")]
    [InlineData(0,0,1,"0")]
    [InlineData(0,3,1,"1")]
    [InlineData(1,0,0,"68")]
    [InlineData(1,1,0,"196/5")]
    [InlineData(1,2,0,"347")]
    [InlineData(1,3,0,"32")]
    [InlineData(2,2,0,"889/100")]
    [InlineData(3,2,0,"114")]
    [InlineData(4,2,0,"1083/2")]
    [InlineData(5,2,0,"6859/8")]
    [InlineData(6,0,0,"1")]
    [InlineData(6,2,0,"0")]
    [InlineData(6,3,0,"1")]
    [InlineData(7,0,0,"0")]
    [InlineData(7,1,0,"1")]
    [InlineData(7,3,0,"0")]
    public void EquationTypeAndOutputAreExact(
        int m,int f,int v,string expected)
    {
        Assert.True(ExactSkillContractQuestionEngine.Verify(F,P(m,f,v),expected));
        Assert.False(ExactSkillContractQuestionEngine.Verify(F,P(m,f,v),"999"));
    }
    [Fact]
    public void RejectMalformedAndApproximateAnswers()
    {
        Assert.False(ExactSkillContractQuestionEngine.Verify(F,P(8,0),"1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(F,P(0,4),"1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(F,P(0,0,2),"0"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(F,P(5,2),"857.38"));
        var bad=P(6,0);bad["foreign"]=1;
        Assert.False(ExactSkillContractQuestionEngine.Verify(F,bad,"1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            "usccss.g7.u02.l09.foreign",P(0,0),"0"));
    }
    [Fact]
    public void SharedGeneratorVerifiesEachQuestion()
    {
        var engine=new ExactSkillContractQuestionEngine();
        var items=engine.Generate("g7-u02-l08-authoring",L,[F],
            ExactSkillQuestionDifficulty.Standard,4,7208,Array.Empty<string>());
        Assert.Equal(4,items.Count);
        foreach(var q in items)Assert.True(ExactSkillContractQuestionEngine.Verify(
            q.Family,q.Parameters,q.CorrectAnswer));
    }
}

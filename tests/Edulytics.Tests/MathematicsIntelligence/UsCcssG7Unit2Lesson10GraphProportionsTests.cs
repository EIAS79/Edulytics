using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>G7 U02 L10 source authoring fixture; NOT_RUN during A-C.</summary>
public sealed class UsCcssG7Unit2Lesson10GraphProportionsTests
{
    const string L="PED:US-CCSS-MATH:G7:U02:L10",F="usccss.g7.u02.l10.graphs_origin_slope";
    static Dictionary<string,int> P(int mode,int feature)=>new()
    {["kind"]=mode,["feature"]=feature};
    [Fact]
    public void ContractIsScopedAndRetainsEarlierLessons()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(L,out var c));
        Assert.NotNull(c);
        Assert.Contains(F,c.AllowedQuestionFamilies);
        Assert.Equal("READY_VERIFIED",c.Readiness);
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(F));
        Assert.True(LessonPracticeContractRegistry.TryResolve(
            "PED:US-CCSS-MATH:G7:U02:L09",out _));
    }
    [Theory]
    [InlineData(0,0,"1")]
    [InlineData(0,1,"0")]
    [InlineData(0,2,"0")]
    [InlineData(0,3,"0")]
    [InlineData(1,0,"8")]
    [InlineData(1,3,"48")]
    [InlineData(2,0,"3")]
    [InlineData(2,2,"3/2")]
    [InlineData(2,3,"1/4")]
    [InlineData(3,0,"1")]
    [InlineData(3,1,"0")]
    [InlineData(3,2,"0")]
    [InlineData(4,0,"3")]
    [InlineData(4,1,"15/2")]
    [InlineData(4,2,"33/2")]
    [InlineData(4,3,"27")]
    public void GraphSlopeAndOriginExactWitnesses(int mode,int feature,string correct)
    {
        Assert.True(ExactSkillContractQuestionEngine.Verify(F,P(mode,feature),correct));
        Assert.False(ExactSkillContractQuestionEngine.Verify(F,P(mode,feature),"999"));
    }
    [Fact]
    public void InvalidAndUnapprovedParametersFailClosed()
    {
        Assert.False(ExactSkillContractQuestionEngine.Verify(F,P(5,0),"1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(F,P(1,4),"1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(F,P(2,2),"1.49"));
        var extra=P(2,2);extra["other"]=2;
        Assert.False(ExactSkillContractQuestionEngine.Verify(F,extra,"3/2"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            "usccss.g7.u02.l11.foreign",P(0,0),"1"));
    }
    [Fact]
    public void SharedGeneratorReverifiesExactAnswers()
    {
        var engine=new ExactSkillContractQuestionEngine();
        var qs=engine.Generate("g7-u02-l10-authoring",L,[F],
            ExactSkillQuestionDifficulty.Standard,4,7210,Array.Empty<string>());
        Assert.Equal(4,qs.Count);
        foreach(var q in qs)
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                q.Family,q.Parameters,q.CorrectAnswer));
    }
}

using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>G7 U02 L06 source-scoped authoring fixtures; NOT_RUN during A-C.</summary>
public sealed class UsCcssG7Unit2Lesson6EquationProblemsTests
{
    const string Lesson="PED:US-CCSS-MATH:G7:U02:L06", Family="usccss.g7.u02.l06.concert_recycling_equations";
    static Dictionary<string,int> P(int mode,int feature)=>new()
    {["mode"]=mode,["feature"]=feature};
    [Fact]
    public void ExactG7LessonScopedRouteDoesNotRegressPriorFamilies()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson,out var c));
        Assert.NotNull(c);
        Assert.Equal("READY_VERIFIED",c.Readiness);
        Assert.Contains(Family,c.AllowedQuestionFamilies);
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(Family));
        Assert.True(LessonPracticeContractRegistry.TryResolve(
            "PED:US-CCSS-MATH:G7:U02:L05",out _));
    }
    [Theory]
    [InlineData(0,0,"311/5")]
    [InlineData(1,0,"435400")]
    [InlineData(1,1,"622000")]
    [InlineData(2,0,"6500")]
    [InlineData(2,1,"25000000/311")]
    [InlineData(2,2,"5000")]
    [InlineData(3,0,"10")]
    [InlineData(3,1,"150")]
    [InlineData(3,2,"200")]
    [InlineData(4,2,"21/10")]
    [InlineData(5,1,"21/10")]
    [InlineData(6,0,"195/2")]
    [InlineData(6,1,"2/5")]
    [InlineData(7,0,"867")]
    [InlineData(7,1,"3")]
    public void ExactTicketAndRecyclingRates(int mode,int feature,string correct)
    {
        Assert.True(ExactSkillContractQuestionEngine.Verify(Family,P(mode,feature),correct));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(mode,feature),"999"));
    }
    [Fact]
    public void AnswerBoundaryAndScopeAreStrict()
    {
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(8,0),"1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(0,4),"1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(0,0),"62.21"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(2,1),"80385.85"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(2,1),"1/0"));
        var foreign=P(0,0);foreign["other"]=1;
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,foreign,"311/5"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            "usccss.g7.u02.l07.unapproved",P(0,0),"311/5"));
    }
    [Fact]
    public void SelectedGeneratorCreatesVerifiableExactItems()
    {
        var engine=new ExactSkillContractQuestionEngine();
        var items=engine.Generate("g7-u02-l06-authoring",Lesson,[Family],
            ExactSkillQuestionDifficulty.Standard,4,7206,Array.Empty<string>());
        Assert.Equal(4,items.Count);
        foreach(var q in items)
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                q.Family,q.Parameters,q.CorrectAnswer));
    }
}

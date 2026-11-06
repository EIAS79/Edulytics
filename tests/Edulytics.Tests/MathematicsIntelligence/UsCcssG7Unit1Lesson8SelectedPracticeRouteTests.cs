using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>G7 L08 source A-C numeric authoring, not executed until D.</summary>
public sealed class UsCcssG7Unit1Lesson8SelectedPracticeRouteTests
{
    private const string Lesson="PED:US-CCSS-MATH:G7:U01:L08";
    private const string Family="usccss.g7.u01.l08.map_route_rate_time";
    private static Dictionary<string,int> P(int s,int t,int mode) =>
        new() {["scale"]=s,["minutes"]=t,["mode"]=mode};

    [Fact]
    public void OptionalLessonHasSelectedRateFamily()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson,out var c));
        Assert.NotNull(c);
        Assert.Equal(new[]{Family},c.AllowedQuestionFamilies);
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(Family));
    }

    [Theory]
    [InlineData(0,0,0,"63.75")]  // road 8.5mi in 8min
    [InlineData(0,0,1,"48.75")]  // helicopter direct 6.5mi in 8min
    [InlineData(0,0,2,"0")]      // driver exceeded 55mph
    [InlineData(0,1,2,"1")]      // road 8.5mi in 10min = 51mph
    [InlineData(0,3,0,"34")]    // 8.5mi in 15min
    [InlineData(1,0,0,"127.5")]
    [InlineData(2,1,1,"156")]    // 26mi in 10min
    [InlineData(0,0,3,"200")]    // 12.5 bars *4mi /15mph
    public void RouteDistanceAndTimeUseCorrectUnitsAndRouteType(
        int scale,int minuteCode,int mode,string expected)
    {
        Assert.True(ExactSkillContractQuestionEngine.Verify(
            Family,P(scale,minuteCode,mode),expected));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            Family,P(scale,minuteCode,mode),"999"));
    }

    [Fact]
    public void EquivalentRepresentationAndBoundsFailClosed()
    {
        Assert.True(ExactSkillContractQuestionEngine.Verify(Family,P(0,0,0),"255/4"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(0,0,0),"48.75"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(0,0,1),"63.75"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(0,0,2),"2"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(3,0,0),"63.75"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(0,4,0),"63.75"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(0,0,4),"200"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(0,0,3),"200/0"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            "usccss.g7.u01.l09.unapproved",P(0,0,0),"63.75"));
    }

    [Fact]
    public void ExactGeneratorReconstructsAndChecksAllItemsWithoutAnswerLeak()
    {
        var engine=new ExactSkillContractQuestionEngine();
        var qs=engine.Generate("g7-u01-l08",Lesson,[Family],
            ExactSkillQuestionDifficulty.Standard,3,708,Array.Empty<string>());
        Assert.Equal(3,qs.Count);
        foreach(var q in qs)
        {
            Assert.Equal(Family,q.Family);
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                Family,q.Parameters,q.CorrectAnswer));
            Assert.DoesNotContain("CorrectAnswer",q.Prompt);
        }
    }
}

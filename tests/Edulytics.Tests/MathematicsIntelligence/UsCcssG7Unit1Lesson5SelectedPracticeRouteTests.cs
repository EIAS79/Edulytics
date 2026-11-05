using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>G7 U01:L05 source-scoped tests; AUTHORING_NOT_RUN until Stage D.</summary>
public sealed class UsCcssG7Unit1Lesson5SelectedPracticeRouteTests
{
    private const string Lesson="PED:US-CCSS-MATH:G7:U01:L05";
    private const string Family="usccss.g7.u01.l05.factor_size_and_reciprocal";
    private static Dictionary<string,int> P(int n,int mode) =>
        new() { ["factor"]=n, ["mode"]=mode };

    [Fact]
    public void LessonSelectsExactFamilyAndReusesEarlierRatioFamily()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson,out var c));
        Assert.NotNull(c);
        Assert.Equal(new[] { Family,"usccss.g7.u01.l02.scale_factor" },
            c.AllowedQuestionFamilies);
        Assert.All(c.AllowedQuestionFamilies,
            x=>Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(x)));
    }

    [Theory]
    [InlineData(0,"1")]
    [InlineData(1,"1")]
    [InlineData(2,"2")]
    [InlineData(3,"3")]
    [InlineData(4,"3")]
    [InlineData(5,"3")]
    [InlineData(6,"1")]
    public void AllSizeClassesHaveCorrectDirection(int idx,string expected)
    {
        Assert.True(ExactSkillContractQuestionEngine.Verify(Family,P(idx,0),expected));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(idx,0),
            expected=="3" ? "1" : "3"));
    }

    [Theory]
    [InlineData(0,"5")]
    [InlineData(1,"4")]
    [InlineData(2,"3")]
    [InlineData(3,"2")]
    [InlineData(4,"1")]
    [InlineData(5,"7")]
    [InlineData(6,"6")]
    public void ReciprocalIndicesReverseScaling(int index,string answer)
    {
        Assert.True(ExactSkillContractQuestionEngine.Verify(Family,P(index,1),answer));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            Family,P(index,1),answer+"x"));
    }

    [Fact]
    public void MalformedOrOutOfRangeFailsClosed()
    {
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(-1,0),"1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(7,0),"3"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(0,2),"5"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(0,0)," 1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            "usccss.g7.u01.l06.not_selected",P(0,0),"1"));
    }

    [Fact]
    public void SelectedFamilyGeneratesVerifiableWithoutLeakingExpectedResult()
    {
        var engine=new ExactSkillContractQuestionEngine();
        var items=engine.Generate("g7-u01-l05",Lesson,[Family],
            ExactSkillQuestionDifficulty.Standard,4,705,Array.Empty<string>());
        Assert.Equal(4,items.Count);
        foreach(var item in items)
        {
            Assert.Equal(Family,item.Family);
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                Family,item.Parameters,item.CorrectAnswer));
            Assert.DoesNotContain("CorrectAnswer",item.Prompt);
        }
    }
}

using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>G7 U01:L10 exact source mathematical oracles AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG7Unit1Lesson10SelectedPracticeRouteTests
{
    private const string Lesson="PED:US-CCSS-MATH:G7:U01:L10";
    private const string Family="usccss.g7.u01.l10.change_scale_length_and_area";
    private static Dictionary<string,int> P(int k,int f,int s=0) =>
        new() {["kind"]=k,["feature"]=f,["style"]=s};

    [Fact]
    public void LessonSelectsChangingScaleAndReusesSourceSquaredArea()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson,out var c));
        Assert.NotNull(c);
        Assert.Equal(new[]{Family,"usccss.g7.u01.l06.scaled_area_square_factor"},
            c.AllowedQuestionFamilies);
        Assert.All(c.AllowedQuestionFamilies,
            f=>Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f)));
    }

    [Theory]
    [InlineData(0,0,0,"30")]  // park new width
    [InlineData(0,1,0,"12")]  // park new height
    [InlineData(0,2,0,"9")]   // area new/old factor
    [InlineData(0,3,0,"900")] // new actual m² per cm²
    [InlineData(1,0,0,"24")]  // triangle120m at5m/cm
    [InlineData(1,1,1,"6")]   // triangle90m at15m/cm
    [InlineData(1,2,0,"216")] // triangle area5400/25=216
    [InlineData(1,3,1,"225")] // actual area per drawn cm²
    [InlineData(1,2,2,"54/25")] // 5400/2500 =2.16
    [InlineData(2,0,0,"3/2")]
    [InlineData(2,1,0,"9/4")]
    [InlineData(3,0,0,"8640")]// 60×12²
    [InlineData(3,1,0,"144")]
    [InlineData(3,2,0,"0")]   // false 720 area assertion
    [InlineData(4,0,0,"1")]
    [InlineData(4,1,0,"0")]
    [InlineData(4,2,0,"1")]
    [InlineData(4,3,0,"0")]
    public void ActualAndDrawingAreaNeverConfused(
        int kind,int feature,int style,string expected)
    {
        Assert.True(ExactSkillContractQuestionEngine.Verify(
            Family,P(kind,feature,style),expected));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            Family,P(kind,feature,style),"1000001"));
    }

    [Fact]
    public void EquivalentFractionsAndBadRatios()
    {
        Assert.True(ExactSkillContractQuestionEngine.Verify(Family,P(2,1),"2.25"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Family,P(2,1),"18/8"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(3,0),"720"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(0,0),"9"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(1,0,4),"24"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(5,0),"30"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(0,4),"30"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(0,0),"1/0"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            "usccss.g7.u01.l11.foreign",P(0,0),"30"));
    }

    [Fact]
    public void ExactSelectedFamilyReturnsVerifiedAnswers()
    {
        var engine=new ExactSkillContractQuestionEngine();
        var questions=engine.Generate("g7-u01-l10",Lesson,[Family],
            ExactSkillQuestionDifficulty.Standard,3,710,Array.Empty<string>());
        Assert.Equal(3,questions.Count);
        foreach(var q in questions)
        {
            Assert.Equal(Family,q.Family);
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                Family,q.Parameters,q.CorrectAnswer));
            Assert.DoesNotContain("CorrectAnswer",q.Prompt);
        }
    }
}

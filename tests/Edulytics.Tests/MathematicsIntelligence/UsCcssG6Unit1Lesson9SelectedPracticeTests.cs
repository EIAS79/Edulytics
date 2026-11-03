using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Edulytics.Services.Practice;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Original OUR/IM G6 U01:L09 source authoring fixtures.
/// AUTHORED_NOT_RUN in Stage A–B–C. Visual constructions require teacher review.
/// </summary>
public sealed class UsCcssG6Unit1Lesson9SelectedPracticeTests
{
    private const string Lesson="PED:US-CCSS-MATH:G6:U01:L09";
    private const string Area=UsCcssG6Unit1Lesson8TriangleAreaQuestions.L09Area;
    private const string Heights=UsCcssG6Unit1Lesson8TriangleAreaQuestions.L09Height;

    [Fact]
    public void ReusesExistingTriangleEngine_OnlyOriginalL09Families()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson,out var c));
        Assert.NotNull(c);
        Assert.Equal("READY_VERIFIED",c.Readiness);
        Assert.Equal(new[]{Area,Heights},c.AllowedQuestionFamilies);
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(Area));
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(Heights));
        Assert.DoesNotContain("v360",c.SourceType,StringComparison.OrdinalIgnoreCase);
        Assert.False(c.AllowedQuestionFamilies.Contains(
            UsCcssG6Unit1Lesson8TriangleAreaQuestions.TriangleArea));
    }

    [Theory]
    [InlineData(0,"30")]
    [InlineData(1,"12")]
    [InlineData(2,"12")]
    [InlineData(3,"16")]
    [InlineData(4,"12")]
    [InlineData(5,"15")]
    [InlineData(6,"8")]
    [InlineData(7,"10.5")]
    [InlineData(8,"14")]
    [InlineData(9,"15")]
    [InlineData(10,"18")]
    [InlineData(11,"13.5")]
    [InlineData(12,"82")]
    [InlineData(13,"4.5")]
    public void OriginalSourceTriangleAreas_ExactVariantsRejectWrongAnswers(
        int variant,string answer)
    {
        var p=new Dictionary<string,int>{{"variant",variant}};
        Assert.Equal(answer,UsCcssG6Unit1Lesson8TriangleAreaQuestions.Solve(Area,p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Area,p,answer));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Area,p,"-3"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Area,p,"1/0"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Area,p," 12 "));
    }

    [Theory]
    [InlineData(0,"1")]
    [InlineData(1,"0")]
    [InlineData(2,"0")]
    [InlineData(3,"0")]
    [InlineData(4,"1")]
    [InlineData(5,"0")]
    [InlineData(6,"0")]
    public void OriginalNinePointOne_OnlyStatementsOneAndFiveTrue(
        int variant,string expected)
    {
        var p=new Dictionary<string,int>{{"variant",variant}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(Heights,p,expected));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            Heights,p,expected=="1"?"0":"1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Heights,p,"1.0"));
    }

    [Fact]
    public void FractionsBoundaryAndForgery_FailClosed()
    {
        Assert.True(ExactSkillContractQuestionEngine.Verify(Area,
            new Dictionary<string,int>{{"variant",7}},"21/2"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Area,
            new Dictionary<string,int>{{"variant",13}},"9/2"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Area,
            new Dictionary<string,int>{{"variant",14}},"15"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Heights,
            new Dictionary<string,int>{{"variant",7}},"1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Area,
            new Dictionary<string,int>{{"variant",2},{"forged",1}},"12"));
    }

    [Fact]
    public void SelectedGenerationProducesIndependentVerifiableNumericalItems()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson,out var c));
        Assert.NotNull(c);
        var qs=new ExactSkillContractQuestionEngine().Generate(
            "g6-u01-l09-original-im",Lesson,c.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,6,20261103,Array.Empty<string>());
        Assert.Equal(6,qs.Count);
        Assert.All(qs,q=>{
            Assert.Contains(q.Family,c.AllowedQuestionFamilies);
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                q.Family,q.Parameters,q.CorrectAnswer));
            Assert.Contains("Teacher reviews actual cuts",q.Solution);
        });
        var bad=LessonPracticeAlignmentValidator.ValidatePersisted(
            c.ToLegacyStage18Contract(),
            "usccss.g6.u01.l08.triangle_half_area",
            new Dictionary<string,int>{{"variant",0}},"12");
        Assert.False(bad.IsAligned);
        Assert.Equal("QuestionFamilyNotAllowed",bad.ReasonCode);
    }
}

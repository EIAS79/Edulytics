using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// A/B/C authored Stage C contract fixtures; deliberately NOT_RUN until Stage D.
/// Teacher-reviewed constructions and explanations are not auto-scored.
/// </summary>
public sealed class UsCcssG6Unit1Lesson1SelectedPracticeRouteTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U01:L01";
    private const string Area = "usccss.g6.u01.l01.triangular_unit_tile_area";
    private const string Coverage = "usccss.g6.u01.l01.structured_tiling_coverage";

    [Fact]
    public void SourceLesson_SelectsExactlyBothNumericMechanics()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson, out var contract));
        Assert.NotNull(contract);
        Assert.Equal("geometry.area_tiling", contract.SkillId);
        Assert.Equal("READY_VERIFIED", contract.Readiness);
        Assert.Equal(new[] { Area, Coverage }, contract.AllowedQuestionFamilies);
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(Area));
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(Coverage));
    }
    [Theory]
    [InlineData(8, 0, "72")]
    [InlineData(8, 1, "64")]
    [InlineData(8, 2, "56")]
    [InlineData(1, 0, "9")]
    [InlineData(4, 1, "32")]
    [InlineData(6, 2, "42")]
    public void ExactSourceUnitArea_ValidPositiveCases(int hexagons,int color,string answer)
    {
        var p = new Dictionary<string,int>{{"hexagons",hexagons},{"color",color}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(Area,p,answer));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Area,p,"1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Area,p,answer+"0"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Area,p,answer+" triangular units"));
    }

    [Theory]
    [InlineData(0, "1")]
    [InlineData(1, "1")]
    [InlineData(2, "0")]
    [InlineData(3, "0")]
    public void ExactTilingCoverage_RejectsOverlapsAndGaps(int pattern,string answer)
    {
        var p = new Dictionary<string,int>{{"pattern",pattern}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(Coverage,p,answer));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            Coverage,p,answer=="1"?"0":"1"));
    }
    [Fact]
    public void MalformedAndOutOfRangeParameters_FailClosed()
    {
        Assert.False(ExactSkillContractQuestionEngine.Verify(Area,
            new Dictionary<string,int>{{"hexagons",0},{"color",0}},"0"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Area,
            new Dictionary<string,int>{{"hexagons",9},{"color",0}},"81"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Area,
            new Dictionary<string,int>{{"hexagons",8},{"color",-1}},"72"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Area,
            new Dictionary<string,int>{{"hexagons",8},{"color",0},{"unsafe",1}},"72"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Coverage,
            new Dictionary<string,int>{{"pattern",4}},"1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Coverage,
            new Dictionary<string,int>(), "1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Area,
            new Dictionary<string,int>{{"hexagons",8},{"color",0}}," 72 "));
    }

    [Fact]
    public void DeterministicSourceFamilies_GenerateAndSelfVerifyAuthoringFixture()
    {
        var engine = new ExactSkillContractQuestionEngine();
        var questions = engine.Generate(
            "g6-u01-l01-candidate", Lesson,
            new[] { Area, Coverage }, ExactSkillQuestionDifficulty.Standard,
            4, 260, Array.Empty<string>());
        Assert.Equal(4,questions.Count);
        foreach (var question in questions)
        {
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                question.Family,question.Parameters,question.CorrectAnswer));
            Assert.Contains(question.Family,new[]{Area,Coverage});
        }
    }
}

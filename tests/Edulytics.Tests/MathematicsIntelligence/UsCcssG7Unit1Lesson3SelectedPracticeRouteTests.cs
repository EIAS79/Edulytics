using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>G7 U01:L03 independent scoped fixtures. AUTHORING ONLY, NOT_RUN.</summary>
public sealed class UsCcssG7Unit1Lesson3SelectedPracticeRouteTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G7:U01:L03";
    private const string Polygon = "usccss.g7.u01.l03.scaled_l_polygon";
    private const string Additive = "usccss.g7.u01.l03.multiplicative_vs_additive";
    private const string Reused = "usccss.g7.u01.l02.scale_factor";

    [Fact]
    public void LessonSelectsTwoNewFamiliesAndTheExistingRatioFamily()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson, out var c));
        Assert.NotNull(c);
        Assert.Equal(new[] { Polygon, Additive, Reused }, c.AllowedQuestionFamilies);
        Assert.All(c.AllowedQuestionFamilies,
            f => Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f)));
    }

    private static Dictionary<string, int> Shape(int factor, int transform) =>
        new()
        {
            ["factor"] = factor, ["transform"] = transform,
            ["dx"] = 1, ["dy"] = -1
        };
    private static Dictionary<string, int> Numbers(int factor, int method, int n = 5,
        int step = 2) =>
        new()
        {
            ["first"] = n, ["step"] = step,
            ["factor"] = factor, ["method"] = method
        };

    [Theory]
    [InlineData(0, 0, true)] // half-scale copy
    [InlineData(1, 0, true)] // triple-scale copy
    [InlineData(0, 1, true)] // rotated half-scale
    [InlineData(1, 1, true)] // rotated triple-scale
    [InlineData(0, 2, false)] // nonuniform stretch
    [InlineData(1, 3, false)] // shear changes angles
    [InlineData(1, 4, false)] // doubled instead of requested scale
    public void SourceLPolygonUniformCheck_OnlyAcceptsExactFactor(
        int factor, int transform, bool valid)
    {
        var p = Shape(factor, transform);
        Assert.True(ExactSkillContractQuestionEngine.Verify(
            Polygon, p, valid ? "1" : "0"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            Polygon, p, valid ? "0" : "1"));
    }

    [Theory]
    [InlineData(0, 0, "1")]
    [InlineData(1, 0, "1")]
    [InlineData(0, 1, "0")]
    [InlineData(1, 1, "0")]
    public void MultiplicationNotUniformAddition_IsRequired(
        int factor, int method, string correct)
    {
        var p = Numbers(factor, method);
        Assert.True(ExactSkillContractQuestionEngine.Verify(Additive, p, correct));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Additive, p,
            correct == "1" ? "0" : "1"));
    }

    [Fact]
    public void WrongForeignAndMalformedAnswersFailClosed()
    {
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            Polygon, Shape(2, 0), "1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            Polygon, Shape(0, 6), "1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            Additive, Numbers(1, 0, 1), "1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            Additive, Numbers(1, 0, 5, 0), "1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            Additive, Numbers(1, 0), "yes"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            "usccss.g7.u01.l04.not_authorized", Shape(1, 0), "1"));
    }

    [Fact]
    public void ExactEngineGeneratesSourceSelectedQuestions()
    {
        var engine = new ExactSkillContractQuestionEngine();
        foreach (var family in new[] { Polygon, Additive })
        {
            var items = engine.Generate("g7-u01-l03", Lesson, [family],
                ExactSkillQuestionDifficulty.Standard, 3, 703,
                Array.Empty<string>());
            Assert.Equal(3, items.Count);
            foreach (var item in items)
            {
                Assert.Equal(family, item.Family);
                Assert.True(ExactSkillContractQuestionEngine.Verify(
                    family, item.Parameters, item.CorrectAnswer));
                Assert.Contains(item.CorrectAnswer, new[] { "0", "1" });
                Assert.DoesNotContain("CorrectAnswer", item.Prompt);
            }
        }
    }
}

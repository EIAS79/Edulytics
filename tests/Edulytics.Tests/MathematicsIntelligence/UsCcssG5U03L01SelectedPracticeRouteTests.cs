using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Grade 5 U03:L01 A-B-C AUTHORING fixture, AUTHORED_NOT_RUN.
/// Selected digital multiple choice and exact original-whole identity only.
/// Teacher reviews learners' drawings, explanations and chosen physical whole.
/// </summary>
public sealed class UsCcssG5U03L01SelectedPracticeRouteTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G5:U03:L01";
    private const string Shade = "usccss.g5.u03.l01.fraction_of_half_original_whole";
    private const string Compare = "usccss.g5.u03.l01.compare_original_whole_grids";

    private static Dictionary<string, int> P(int n, int slot, int relation,
        int half = 0, int subcut = 0, int otherHalf = 1, int otherSubcut = 0) =>
        new(StringComparer.Ordinal)
        {
            ["parts"] = n, ["half"] = half, ["subcut"] = subcut,
            ["slot"] = slot, ["relation"] = relation,
            ["otherHalf"] = otherHalf, ["otherSubcut"] = otherSubcut
        };

    [Fact]
    public void SelectedLesson_UsesOnlyExactOriginalWholeFamilies()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson, out var contract));
        Assert.NotNull(contract);
        Assert.Equal("READY_VERIFIED", contract.Readiness);
        Assert.Equal("supporting.fractions.multiply_divide", contract.SkillId);
        Assert.Equal(new[] { Shade, Compare }, contract.AllowedQuestionFamilies);
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(Shade));
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(Compare));
    }

    [Theory]
    [InlineData(3, 0, "1")]
    [InlineData(3, 1, "2")]
    [InlineData(3, 2, "3")]
    [InlineData(4, 0, "1")]
    [InlineData(4, 2, "3")]
    public void OneOfThreeOrFourOfHalf_IsOneOfSixOrEightOfOriginal(
        int parts, int slot, string correct)
    {
        var p = P(parts, slot, 0);
        Assert.True(ExactSkillContractQuestionEngine.Verify(Shade, p, correct));
        foreach (var wrong in new[] { "0", "4", "1/3", "1/4", "1/2", "", " 1" })
            Assert.False(ExactSkillContractQuestionEngine.Verify(Shade, p, wrong));
    }

    [Theory]
    [InlineData(3, 0, "1")]
    [InlineData(4, 0, "1")]
    [InlineData(3, 1, "0")]
    [InlineData(4, 1, "0")]
    [InlineData(3, 2, "0")]
    [InlineData(4, 2, "0")]
    [InlineData(3, 3, "1")]
    [InlineData(4, 3, "1")]
    public void DifferentLayouts_CannotChangeOriginalWholeOrPartitionCount(
        int parts, int relation, string expected)
    {
        var p = P(parts, 0, relation, half: 1, subcut: 1, otherHalf: 0, otherSubcut: 1);
        Assert.True(ExactSkillContractQuestionEngine.Verify(Compare, p, expected));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            Compare, p, expected == "1" ? "0" : "1"));
    }

    [Fact]
    public void MalformedOutsideDomainAndUnregisteredCases_FailClosed()
    {
        foreach (var family in new[] { Shade, Compare })
        {
            Assert.False(ExactSkillContractQuestionEngine.Verify(family, P(2, 0, 0), "1"));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family, P(5, 0, 0), "1"));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family, P(3, 3, 0), "1"));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family, P(4, 0, -1), "1"));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family, P(4, 0, 4), "1"));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family, P(3, 0, 0, half: 2), "1"));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,
                new Dictionary<string, int>(), "1"));
            var extra = P(3, 0, 0);
            extra["unapproved"] = 42;
            Assert.False(ExactSkillContractQuestionEngine.Verify(family, extra, "1"));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family, P(3, 0, 0), "1 "));
        }
        Assert.False(ExactSkillContractQuestionEngine.SupportsFamily(
            "usccss.g5.u03.l02.unapproved"));
    }

    [Fact]
    public void GeneratedSelectedDigitalItems_AgreeWithIndependentVerifier()
    {
        var engine = new ExactSkillContractQuestionEngine();
        var generated = engine.Generate(
            "g5-u03-l01-accepted", Lesson, [Shade, Compare],
            ExactSkillQuestionDifficulty.Standard, 8, 50301, Array.Empty<string>());
        Assert.Equal(8, generated.Count);
        foreach (var item in generated)
        {
            Assert.Contains(item.Family, new[] { Shade, Compare });
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                item.Family, item.Parameters, item.CorrectAnswer));
            Assert.Equal(7, item.Parameters.Count);
            Assert.Contains("ORIGINAL WHOLE", item.Prompt, StringComparison.Ordinal);
        }
    }

    [Fact]
    public void PredecessorAndReleasedConsumers_RemainAvailable()
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(
            "usccss.g5.u02.l17.mosaic_rectangle_area_cost"));
        Assert.True(LessonPracticeContractRegistry.TryResolve(
            "PED:US-CCSS-MATH:G6:U01:L01", out _));
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(
            "usccss.g6.u01.l01.triangular_unit_tile_area"));
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(
            "usccss.g6.u01.l01.structured_tiling_coverage"));
        Assert.True(LessonPracticeContractRegistry.TryResolve(
            "PED:US-CCSS-MATH:G7:U01:L01", out _));
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(
            "usccss.g7.u01.l01.scaled_copy_correspondence"));
    }
}

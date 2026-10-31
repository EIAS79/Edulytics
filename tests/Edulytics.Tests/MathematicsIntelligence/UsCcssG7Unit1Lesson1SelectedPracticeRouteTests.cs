using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Source-scoped A-B-C acceptance fixtures. AUTHORED_NOT_RUN until Stage D.
/// Numeric recognition is not a persisted or automatically graded construction.
/// </summary>
public sealed class UsCcssG7Unit1Lesson1SelectedPracticeRouteTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G7:U01:L01";
    private const string Family = "usccss.g7.u01.l01.scaled_copy_correspondence";

    private static Dictionary<string, int> P(int shape, int factor, int transform,
        int dx = 1, int dy = -2) =>
        new()
        {
            ["shape"] = shape,
            ["factor"] = factor,
            ["transform"] = transform,
            ["dx"] = dx,
            ["dy"] = dy
        };

    [Fact]
    public void ExactLessonBindsToSourceFamily_WithoutRoutingOtherLessons()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson, out var contract));
        Assert.NotNull(contract);
        Assert.Equal("geometry.similarity.scale_factor", contract.SkillId);
        Assert.Equal("READY_VERIFIED", contract.Readiness);
        Assert.Equal(new[] { Family }, contract.AllowedQuestionFamilies);
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(Family));
        Assert.False(LessonPracticeContractRegistry.TryResolve(
            "PED:US-CCSS-MATH:G7:U01:L02", out _));
    }

    [Theory]
    [InlineData(0, 0, 0, true)]    // k=1/2, uniform dilation
    [InlineData(0, 1, 1, true)]    // k=1, rigid rotation + translation
    [InlineData(0, 2, 2, true)]    // k=2, 180 rotation + reversal
    [InlineData(1, 0, 7, true)]    // reflected reversed quadrilateral
    [InlineData(2, 2, 0, true)]    // irregular quadrilateral uniform k2
    [InlineData(0, 2, 3, false)]   // nonuniform horizontal stretch
    [InlineData(1, 1, 4, false)]   // shear changes angles
    [InlineData(2, 0, 5, false)]   // single vertex distortion
    [InlineData(1, 2, 6, false)]   // candidate factor is double stated k
    public void ExactGeometry_RecognizesUniformCopiesOnly(
        int shape, int factor, int transform, bool expected)
    {
        var p = P(shape, factor, transform);
        string correct = expected ? "1" : "0";
        string incorrect = expected ? "0" : "1";
        Assert.True(ExactSkillContractQuestionEngine.Verify(Family, p, correct));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family, p, incorrect));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family, p, "YES"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family, p, " " + correct));
    }

    [Fact]
    public void InvalidParametersAndForeignFamilies_FailClosed()
    {
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family, P(3, 0, 0), "1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family, P(0, 3, 0), "1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family, P(0, 0, 8), "1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family, P(0, 0, 0, 5), "1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,
            new Dictionary<string,int>(), "1"));
        var extra = P(0, 0, 0);
        extra["unapproved"] = 1;
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family, extra, "1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            "usccss.g7.u01.l02.unapproved", P(0, 0, 0), "1"));
    }

    [Fact]
    public void SelectedSourceFamily_GeneratesExactlyVerifiableQuestionCandidates()
    {
        var engine = new ExactSkillContractQuestionEngine();
        var generated = engine.Generate(
            "g7-u01-l01-candidate", Lesson, [Family],
            ExactSkillQuestionDifficulty.Standard, 5, 701,
            Array.Empty<string>());
        Assert.Equal(5, generated.Count);
        foreach (var item in generated)
        {
            Assert.Equal(Family, item.Family);
            Assert.Equal(5, item.Parameters.Count);
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                item.Family, item.Parameters, item.CorrectAnswer));
            Assert.Contains(item.CorrectAnswer, new[] { "0", "1" });
            Assert.Contains("Coordinates", item.Prompt);
        }
    }

    [Fact]
    public void Grade6PreviousSharedConsumer_RemainsRegistered()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(
            "PED:US-CCSS-MATH:G6:U01:L01", out var prior));
        Assert.NotNull(prior);
        Assert.Equal(2, prior.AllowedQuestionFamilies.Count);
        foreach (var family in prior.AllowedQuestionFamilies)
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
    }
}

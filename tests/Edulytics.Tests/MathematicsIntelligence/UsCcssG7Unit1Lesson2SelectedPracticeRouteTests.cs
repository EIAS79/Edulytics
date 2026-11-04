using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>G7 U01 L02 source-scoped authored tests; NOT_RUN in A-B-C.</summary>
public sealed class UsCcssG7Unit1Lesson2SelectedPracticeRouteTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G7:U01:L02";
    private const string Parts = "usccss.g7.u01.l02.corresponding_parts";
    private const string Scale = "usccss.g7.u01.l02.scale_factor";

    [Fact]
    public void LessonHasTwoExactSelectedFamilies_WithoutChangingL01()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson, out var c));
        Assert.NotNull(c);
        Assert.Equal("READY_VERIFIED", c.Readiness);
        Assert.Equal(new[] { Parts, Scale }, c.AllowedQuestionFamilies);
        Assert.All(c.AllowedQuestionFamilies,
            f => Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f)));
        Assert.True(LessonPracticeContractRegistry.TryResolve(
            "PED:US-CCSS-MATH:G7:U01:L01", out var old));
        Assert.Equal(new[] { "usccss.g7.u01.l01.scaled_copy_correspondence" },
            old!.AllowedQuestionFamilies);
    }

    private static Dictionary<string, int> PartsP(int shift, int part, int asked) =>
        new()
        {
            ["shift"] = shift, ["part"] = part, ["asked"] = asked,
            ["factor"] = 1, ["dx"] = 2, ["dy"] = -1
        };

    private static Dictionary<string, int> ScaleP(int factor, int reverse, int distorted) =>
        new()
        {
            ["factor"] = factor, ["reverse"] = reverse, ["distorted"] = distorted
        };

    [Fact]
    public void PointSegmentAngleCorrespondences_AreCyclicallyCorrect()
    {
        for (int shift = 0; shift < 3; shift++)
        for (int part = 0; part < 3; part++)
        for (int asked = 0; asked < 3; asked++)
        {
            var p = PartsP(shift, part, asked);
            var correct = ((asked + shift) % 3 + 1).ToString();
            Assert.True(ExactSkillContractQuestionEngine.Verify(Parts, p, correct));
            Assert.False(ExactSkillContractQuestionEngine.Verify(
                Parts, p, ((asked + shift + 1) % 3 + 1).ToString()));
            Assert.False(ExactSkillContractQuestionEngine.Verify(Parts, p, " " + correct));
        }
    }

    [Theory]
    [InlineData(0, 0, 0, "1")] // k=1/2
    [InlineData(1, 0, 0, "2")] // k=3/2
    [InlineData(2, 0, 0, "3")] // k=2
    [InlineData(0, 1, 0, "3")] // inverse of 1/2
    [InlineData(1, 1, 0, "5")] // inverse of 3/2
    [InlineData(2, 1, 0, "1")] // inverse of 2
    [InlineData(1, 0, 1, "4")] // mismatched third side
    [InlineData(2, 1, 1, "4")] // invalid inverse as well
    public void FractionalScaleAndInverseAndDistortionAreExact(
        int factor, int reverse, int distorted, string correct)
    {
        var p = ScaleP(factor, reverse, distorted);
        Assert.True(ExactSkillContractQuestionEngine.Verify(Scale, p, correct));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            Scale, p, correct == "4" ? "2" : "4"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Scale, p, correct + "x"));
    }

    [Fact]
    public void InvalidParametersAndForeignFamiliesFailClosed()
    {
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            Parts, PartsP(3, 0, 0), "1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            Parts, PartsP(0, 0, -1), "1"));
        var extra = PartsP(0, 0, 0);
        extra["unexpected"] = 1;
        Assert.False(ExactSkillContractQuestionEngine.Verify(Parts, extra, "1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            Scale, ScaleP(3, 0, 0), "1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            Scale, ScaleP(1, 0, 2), "2"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            "usccss.g7.u01.l03.unapproved", PartsP(0, 0, 0), "1"));
    }

    [Fact]
    public void ExactEngineBuildsAndVerifiesBothFamilies_WithoutLeakingTheAnswer()
    {
        var engine = new ExactSkillContractQuestionEngine();
        foreach (string family in new[] { Parts, Scale })
        {
            var items = engine.Generate(
                "g7-u01-l02-abc", Lesson, new[] { family },
                ExactSkillQuestionDifficulty.Standard, 4, 702,
                Array.Empty<string>());
            Assert.Equal(4, items.Count);
            foreach (var item in items)
            {
                Assert.Equal(family, item.Family);
                Assert.Equal(family == Parts ? 6 : 3, item.Parameters.Count);
                Assert.True(ExactSkillContractQuestionEngine.Verify(
                    family, item.Parameters, item.CorrectAnswer));
                Assert.Contains(item.CorrectAnswer, new[] { "1", "2", "3", "4", "5" });
                Assert.Contains("Choose", item.Prompt, StringComparison.OrdinalIgnoreCase);
            }
        }
    }
}

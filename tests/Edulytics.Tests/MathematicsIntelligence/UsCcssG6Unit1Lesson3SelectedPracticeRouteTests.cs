using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Edulytics.Services.Practice;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Grade 6 U01:L03 original IM/OUR 6-8 source: A/B/C fixtures authored for
/// Stage D; deliberately NOT_RUN here. Numeric tasks do not grade drawings.
/// </summary>
public sealed class UsCcssG6Unit1Lesson3SelectedPracticeRouteTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U01:L03";
    private const string Shaded = UsCcssG6Unit1Lesson3ShadedAreaQuestions.Shaded;
    private const string Conservation = UsCcssG6Unit1Lesson3ShadedAreaQuestions.Conservation;

    [Fact]
    public void SourceLesson_OnlySelectsItsTwoExactNumericFamilies()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson, out var contract));
        Assert.NotNull(contract);
        Assert.Equal("READY_VERIFIED", contract.Readiness);
        Assert.Equal("geometry.area_tiling", contract.SkillId);
        Assert.Equal(new[] { Shaded, Conservation }, contract.AllowedQuestionFamilies);
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(Shaded));
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(Conservation));
        Assert.DoesNotContain("usccss.g6.u01.l02.tangram_piece_area",
            contract.AllowedQuestionFamilies);
        Assert.DoesNotContain("v360", contract.SourceType, StringComparison.OrdinalIgnoreCase);
    }

    [Theory]
    [InlineData(0, "27")] // 6x6 square less a 3x3 hole
    [InlineData(1, "15")] // Two right triangles, 5x3/2 each
    [InlineData(2, "16")] // Four right triangles, 2x4/2 each
    [InlineData(3, "21")] // 5x5 outer square less 2x2
    [InlineData(4, "22")] // 5x2 + 4x2 + 2x2 source rectangles
    [InlineData(5, "3200")] // 80x40 original practice wall
    public void OriginalSourceExactShadedArea_IsVerified(int variant, string correct)
    {
        var p = new Dictionary<string, int> { ["variant"] = variant };
        Assert.Equal(correct, UsCcssG6Unit1Lesson3ShadedAreaQuestions.Solve(Shaded, p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Shaded, p, correct));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Shaded, p, "-1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Shaded, p, "1/0"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Shaded, p, " 27 "));
    }

    [Theory]
    [InlineData(0, "1")]
    [InlineData(1, "0")]
    [InlineData(2, "1")]
    [InlineData(3, "1")]
    [InlineData(4, "1")]
    [InlineData(5, "0")]
    public void OriginalSourceAreaConservation_RejectsWrongBinaryAnswer(
        int variant, string correct)
    {
        var p = new Dictionary<string, int> { ["variant"] = variant };
        Assert.True(ExactSkillContractQuestionEngine.Verify(Conservation, p, correct));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            Conservation, p, correct == "1" ? "0" : "1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Conservation, p, "1.0"));
    }

    [Fact]
    public void ExtraMissingOrOutOfDomainParameters_FailClosed()
    {
        Assert.False(ExactSkillContractQuestionEngine.Verify(Shaded,
            new Dictionary<string, int>(), "27"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Shaded,
            new Dictionary<string, int> { ["variant"] = -1 }, "27"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Shaded,
            new Dictionary<string, int> { ["variant"] = 6 }, "27"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Shaded,
            new Dictionary<string, int> { ["variant"] = 0, ["forged"] = 1 }, "27"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Conservation,
            new Dictionary<string, int> { ["variant"] = 6 }, "1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            "usccss.g6.u01.l01.structured_tiling_coverage",
            new Dictionary<string, int> { ["variant"] = 0 }, "1"));
    }

    [Fact]
    public void LessonSelectedGeneration_ProducesExactIndependentNumericalQuestions()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson, out var contract));
        Assert.NotNull(contract);
        var engine = new ExactSkillContractQuestionEngine();
        var questions = engine.Generate(
            "g6-u01-l03-original-im", Lesson, contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,
            4, 20261103, Array.Empty<string>());

        Assert.Equal(4, questions.Count);
        Assert.All(questions, q =>
        {
            Assert.Contains(q.Family, contract.AllowedQuestionFamilies);
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                q.Family, q.Parameters, q.CorrectAnswer));
            Assert.Contains("teacher checks the student's diagram", q.Solution);
        });
    }

    [Fact]
    public void PersistedLessonAlignment_RejectsCrossLessonFamily()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson, out var contract));
        Assert.NotNull(contract);
        var legacy = contract.ToLegacyStage18Contract();
        var invalid = LessonPracticeAlignmentValidator.ValidatePersisted(
            legacy, "usccss.g6.u01.l02.tangram_piece_area",
            new Dictionary<string, int> { ["variant"] = 2 }, "2");
        Assert.False(invalid.IsAligned);
        Assert.Equal("QuestionFamilyNotAllowed", invalid.ReasonCode);

        var valid = LessonPracticeAlignmentValidator.ValidatePersisted(
            legacy, Shaded, new Dictionary<string, int> { ["variant"] = 3 }, "21");
        Assert.True(valid.IsAligned);
    }
}

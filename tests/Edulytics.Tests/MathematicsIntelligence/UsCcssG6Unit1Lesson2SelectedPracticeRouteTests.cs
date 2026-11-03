using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Edulytics.Services.Practice;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Source-specific Grade 6 U01:L02 A/B/C authoring acceptance.
/// NOT RUN: runtime .NET tests and persisted learner scoring belong to Stage D.
/// Teacher judges physical/digital constructions, shape distinctness and proofs.
/// </summary>
public sealed class UsCcssG6Unit1Lesson2SelectedPracticeRouteTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U01:L02";
    private const string Piece = "usccss.g6.u01.l02.tangram_piece_area";
    private const string Equivalent = "usccss.g6.u01.l02.tangram_equal_area";

    [Fact]
    public void OriginalImGrade6_L02SelectsOnlySourceTangramNumericFamilies()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson, out var contract));
        Assert.NotNull(contract);
        Assert.Equal("geometry.area_tiling", contract.SkillId);
        Assert.Equal("READY_VERIFIED", contract.Readiness);
        Assert.Equal(new[] { Piece, Equivalent }, contract.AllowedQuestionFamilies);
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(Piece));
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(Equivalent));
        Assert.DoesNotContain("usccss.g6.u01.l01.triangular_unit_tile_area",
            contract.AllowedQuestionFamilies);
        Assert.DoesNotContain("v360", contract.SourceType, StringComparison.OrdinalIgnoreCase);
    }

    [Theory]
    [InlineData(0, "1/2")]
    [InlineData(1, "1")]
    [InlineData(2, "2")]
    [InlineData(3, "1")]
    [InlineData(4, "2")]
    [InlineData(5, "8")]
    [InlineData(6, "1.5")]
    [InlineData(7, "3/2")]
    public void SourceKitExactAnswers_AreIndependentlyVerified(int variant, string answer)
    {
        var parameters = new Dictionary<string, int> { ["variant"] = variant };
        Assert.True(ExactSkillContractQuestionEngine.Verify(Piece, parameters, answer));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Piece, parameters, "-2"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Piece, parameters, "1/0"));
    }

    [Theory]
    [InlineData(0, "1")]
    [InlineData(1, "1")]
    [InlineData(2, "0")]
    [InlineData(3, "0")]
    public void OnlyNumericEquivalenceIsAutoChecked_NotShapeAssembly(int variant, string answer)
    {
        var parameters = new Dictionary<string, int> { ["variant"] = variant };
        Assert.True(ExactSkillContractQuestionEngine.Verify(Equivalent, parameters, answer));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            Equivalent, parameters, answer == "1" ? "0" : "1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Equivalent, parameters, "1.0"));
    }

    [Fact]
    public void OutOfInventoryAndForgedParameters_FailClosed()
    {
        Assert.False(ExactSkillContractQuestionEngine.Verify(Piece,
            new Dictionary<string, int> { ["variant"] = 8 }, "8"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Equivalent,
            new Dictionary<string, int> { ["variant"] = 4 }, "1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Piece,
            new Dictionary<string, int> { ["variant"] = 6, ["forged"] = 1 }, "1.5"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Piece,
            new Dictionary<string, int>(), "0.5"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Piece,
            new Dictionary<string, int> { ["variant"] = 6 }, "2"));
    }

    [Fact]
    public void SelectedLessonGeneration_ProducesExactTeacherBoundedPractice()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson, out var contract));
        Assert.NotNull(contract);
        var engine = new ExactSkillContractQuestionEngine();
        var questions = engine.Generate(
            "g6-u01-l02-original-im-first-edition", Lesson,
            contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard,
            4, 20261103, Array.Empty<string>());

        Assert.Equal(4, questions.Count);
        Assert.All(questions, q =>
        {
            Assert.Contains(q.Family, contract.AllowedQuestionFamilies);
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                q.Family, q.Parameters, q.CorrectAnswer));
            Assert.Contains("teacher checks constructed figures", q.Solution);
        });
    }

    [Fact]
    public void SharedAlignmentGate_RejectsOtherLessonFamilies()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson, out var contract));
        Assert.NotNull(contract);
        var legacy = contract.ToLegacyStage18Contract();
        var result = LessonPracticeAlignmentValidator.ValidatePersisted(
            legacy, "usccss.g6.u01.l01.triangular_unit_tile_area",
            new Dictionary<string, int> { ["hexagons"] = 8, ["color"] = 0 }, "72");
        Assert.False(result.IsAligned);
        Assert.Equal("QuestionFamilyNotAllowed", result.ReasonCode);

        var matching = LessonPracticeAlignmentValidator.ValidatePersisted(
            legacy, Piece,
            new Dictionary<string, int> { ["variant"] = 7 }, "3/2");
        Assert.True(matching.IsAligned);
    }
}

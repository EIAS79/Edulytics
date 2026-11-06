using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Edulytics.Services.Practice;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Grade6 first-edition IM 6–8 U01:L04 A/B/C authored acceptance fixtures.
/// NOT_RUN until Stage D; cannot auto-score actual coordinate construction.
/// </summary>
public sealed class UsCcssG6Unit1Lesson4SelectedPracticeRouteTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G6:U01:L04";
    private const string Area = UsCcssG6Unit1Lesson4ParallelogramQuestions.ShadedArea;
    private const string TwoPairs = UsCcssG6Unit1Lesson4ParallelogramQuestions.TwoPairs;
    private const string Invariant = UsCcssG6Unit1Lesson4ParallelogramQuestions.AreaInvariant;

    [Fact]
    public void SourceLesson_SelectsExactlyThreeSourceMechanics()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson, out var contract));
        Assert.NotNull(contract);
        Assert.Equal("READY_VERIFIED", contract.Readiness);
        Assert.Equal("geometry.area_tiling", contract.SkillId);
        Assert.Equal(new[] { Area, TwoPairs, Invariant }, contract.AllowedQuestionFamilies);
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(Area));
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(TwoPairs));
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(Invariant));
        Assert.DoesNotContain("v360", contract.SourceType, StringComparison.OrdinalIgnoreCase);
    }

    [Theory]
    [InlineData(0, "12")] // Original 4x3 shaded
    [InlineData(1, "16")] // Height changes from 3 to 4
    [InlineData(2, "28")] // 7x4 parallelogram
    [InlineData(3, "24")] // Original 6x4 first-party Cool-down
    [InlineData(4, "24")] // Source 4.3 C 4x6 not the slanted 4.5
    [InlineData(5, "28")] // 8x4 enclosure less two 1x4 halves
    [InlineData(6, "12")] // 5x3 enclosure less two 1x3 halves
    [InlineData(7, "24")] // Q enclosure 11x4 less two 5x4 halves
    public void ExactArea_AcceptsSourceAndProvenFirstPartyVariants(int variant, string correct)
    {
        var parameters = new Dictionary<string, int> { ["variant"] = variant };
        Assert.Equal(correct, UsCcssG6Unit1Lesson4ParallelogramQuestions.Solve(Area, parameters));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Area, parameters, correct));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Area, parameters, "-1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Area, parameters, "0/0"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Area, parameters, " 12 "));
    }

    [Theory]
    [InlineData(0, "1")]
    [InlineData(1, "1")]
    [InlineData(2, "1")]
    [InlineData(3, "0")]
    [InlineData(4, "0")]
    [InlineData(5, "0")]
    public void OriginalAThroughF_ClassifiesBothOppositeSidePairs(int variant, string expected)
    {
        var p = new Dictionary<string, int> { ["variant"] = variant };
        Assert.True(ExactSkillContractQuestionEngine.Verify(TwoPairs, p, expected));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            TwoPairs, p, expected == "1" ? "0" : "1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(TwoPairs, p, "yes"));
    }

    [Theory]
    [InlineData(0, "1")]
    [InlineData(1, "1")]
    [InlineData(2, "0")]
    [InlineData(3, "1")]
    [InlineData(4, "0")]
    [InlineData(5, "1")]
    public void AreaConservation_DistinguishesPerpendicularFromSlantedSide(
        int variant, string expected)
    {
        var p = new Dictionary<string, int> { ["variant"] = variant };
        Assert.True(ExactSkillContractQuestionEngine.Verify(Invariant, p, expected));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            Invariant, p, expected == "1" ? "0" : "1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Invariant, p, "1.0"));
    }

    [Fact]
    public void ForgedOrOutOfInventoryParameters_FailClosed()
    {
        Assert.False(ExactSkillContractQuestionEngine.Verify(Area,
            new Dictionary<string, int>(), "12"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Area,
            new Dictionary<string, int> { ["variant"] = -1 }, "12"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Area,
            new Dictionary<string, int> { ["variant"] = 8 }, "12"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(TwoPairs,
            new Dictionary<string, int> { ["variant"] = 6 }, "1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Invariant,
            new Dictionary<string, int> { ["variant"] = 0, ["forged"] = 1 }, "1"));
    }

    [Fact]
    public void SourceSelectedPractice_GeneratesAndVerifiesNumericOnly()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson, out var contract));
        Assert.NotNull(contract);
        var questions = new ExactSkillContractQuestionEngine().Generate(
            "g6-u01-l04-original-im", Lesson, contract.AllowedQuestionFamilies,
            ExactSkillQuestionDifficulty.Standard, 6, 20261103, Array.Empty<string>());
        Assert.Equal(6, questions.Count);
        Assert.All(questions, q =>
        {
            Assert.Contains(q.Family, contract.AllowedQuestionFamilies);
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                q.Family, q.Parameters, q.CorrectAnswer));
            Assert.Contains("teacher separately checks all constructions", q.Solution);
        });
    }

    [Fact]
    public void IndependentLessonGate_RejectsPreviousLessonFamilies()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson, out var contract));
        Assert.NotNull(contract);
        var legacy = contract.ToLegacyStage18Contract();
        var rejected = LessonPracticeAlignmentValidator.ValidatePersisted(
            legacy, "usccss.g6.u01.l03.source_shaded_area",
            new Dictionary<string, int> { ["variant"] = 0 }, "27");
        Assert.False(rejected.IsAligned);
        Assert.Equal("QuestionFamilyNotAllowed", rejected.ReasonCode);
        var accepted = LessonPracticeAlignmentValidator.ValidatePersisted(
            legacy, Area, new Dictionary<string, int> { ["variant"] = 6 }, "12");
        Assert.True(accepted.IsAligned);
    }
}

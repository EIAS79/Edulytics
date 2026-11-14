using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

// Arabic G5 student book 02-1.pdf printed 443-448.
// Stage C tests WRITTEN_NOT_RUN; physical toothpick constructions remain teacher-reviewed.
public sealed class UaeGrade5Unit6Lesson07HandsOnPatternsTests
{
    private const string Lesson = "PED:UAE-MOE-MATH:L5:GENERAL:06:07";
    private const string Squares = "uae.g5.u06.l07.matchstick_adjacent_squares";
    private const string Diagonal = "uae.g5.u06.l07.matchstick_square_diagonal";
    private const string Triangles = "uae.g5.u06.l07.matchstick_shared_triangles";
    private const string Compare = "uae.g5.u06.l07.compare_three_six_growth";

    [Fact]
    public void ExactHandsOnSourceFamiliesRouteToExistingNumericPatternsEngine()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson, out var contract));
        Assert.NotNull(contract);
        Assert.Equal("number.numeric_sequences_identify_extend", contract!.SkillId);
        Assert.Equal(4, contract.AllowedQuestionFamilies.Count);
        Assert.Contains(Squares, contract.AllowedQuestionFamilies);
        Assert.Contains(Diagonal, contract.AllowedQuestionFamilies);
        Assert.Contains(Triangles, contract.AllowedQuestionFamilies);
        Assert.Contains(Compare, contract.AllowedQuestionFamilies);
        var engine = new ExactSkillContractQuestionEngine();
        foreach (var family in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family, out var ui));
            Assert.NotNull(ui);
            foreach (var q in engine.Generate(
                Lesson, family, [family],
                ExactSkillQuestionDifficulty.Standard, 2, 20261113, []))
                Assert.True(ExactSkillContractQuestionEngine.Verify(
                    family, q.Parameters, q.CorrectAnswer));
        }
    }

    [Fact]
    public void IndependentArabicPatternFixturesAndTamperChecks()
    {
        var p = new Dictionary<string,int> { ["index"] = 6 };
        Assert.True(ExactSkillContractQuestionEngine.Verify(Squares, p, "١٩"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Diagonal, p, "٢٤"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Triangles, p, "١٣"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Compare, p, "١٥"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Squares, p, "24"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Triangles, p, "١٩"));
        p["index"] = 10;
        Assert.False(ExactSkillContractQuestionEngine.Verify(Squares, p, "31"));
        p["index"] = 6;
        p["untrusted"] = 999;
        Assert.False(ExactSkillContractQuestionEngine.Verify(Diagonal, p, "24"));
    }

    [Fact]
    public void SourceHandsOnModelsCountSharedEdgesAndCompareRates()
    {
        const int index = 5;
        Assert.Equal(16, 4 + 3 * (index - 1));
        Assert.Equal(20, 4 * index);
        Assert.Equal(11, 3 + 2 * (index - 1));
        Assert.Equal(12, (4 + 6 * (index - 1)) - (4 + 3 * (index - 1)));
        Assert.NotEqual(4 * index, 4 + 3 * (index - 1));
    }
}

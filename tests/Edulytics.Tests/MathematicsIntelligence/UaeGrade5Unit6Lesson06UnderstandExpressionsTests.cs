using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

// Arabic Grade 5 student book 02-1.pdf, printed pages 437-440.
// Stage A-B-C exact family tests are WRITTEN_NOT_RUN, not Stage D proof.
public sealed class UaeGrade5Unit6Lesson06UnderstandExpressionsTests
{
    private const string Lesson = "PED:UAE-MOE-MATH:L5:GENERAL:06:06";
    private const string Add = "uae.g5.u06.l06.model_unknown_plus";
    private const string Subtract = "uae.g5.u06.l06.model_unknown_minus";
    private const string Groups = "uae.g5.u06.l06.model_repeated_group";

    [Fact]
    public void SourceSpecificModelFamiliesReplaceGenericEnglishAlgebraPractice()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson, out var contract));
        Assert.NotNull(contract);
        Assert.Equal("number.algebra.write_expressions", contract!.SkillId);
        Assert.Equal(3, contract.AllowedQuestionFamilies.Count);
        Assert.Contains(Add, contract.AllowedQuestionFamilies);
        Assert.Contains(Subtract, contract.AllowedQuestionFamilies);
        Assert.Contains(Groups, contract.AllowedQuestionFamilies);
        Assert.DoesNotContain("supporting.algebra.simplify", contract.AllowedQuestionFamilies);
        Assert.DoesNotContain("supporting.algebra.substitute", contract.AllowedQuestionFamilies);
        Assert.DoesNotContain("supporting.algebra.expressions.mixed", contract.AllowedQuestionFamilies);

        var engine = new ExactSkillContractQuestionEngine();
        foreach (var family in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family, out var ui));
            Assert.NotNull(ui);
            foreach (var q in engine.Generate(
                Lesson, family, [family],
                ExactSkillQuestionDifficulty.Standard, 2, 20261113, []))
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(
                    family, q.Parameters, q.CorrectAnswer));
            }
        }
    }

    [Fact]
    public void OriginalConcreteModelsAcceptCorrectArabicExpressions()
    {
        var add = new Dictionary<string, int> { ["n"] = 5, ["groups"] = 0 };
        Assert.True(ExactSkillContractQuestionEngine.Verify(Add, add, "x+5"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Add, add, "٥+س"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Add, add, "x−5"));

        Assert.True(ExactSkillContractQuestionEngine.Verify(Subtract, add, "س-٥"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Subtract, add, "5−x"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Subtract, add, "x+5"));

        var group = new Dictionary<string, int> { ["n"] = 2, ["groups"] = 3 };
        Assert.True(ExactSkillContractQuestionEngine.Verify(Groups, group, "3×(x+2)"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Groups, group, "٣(س+٢)"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Groups, group, "3x+6"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Groups, group, "3x+2"));
        group["groups"] = 0;
        Assert.False(ExactSkillContractQuestionEngine.Verify(Groups, group, "3x+6"));
    }

    [Fact]
    public void IndependentSourceScopedModelArithmeticFixtures()
    {
        const int unknown = 4;
        Assert.Equal(9, unknown + 5);
        const int initialBoxes = 11;
        Assert.Equal(8, initialBoxes - 3);
        Assert.NotEqual(8, 3 - initialBoxes);
        Assert.Equal(18, 3 * (unknown + 2));
        Assert.NotEqual(3 * (unknown + 2), 3 * unknown + 2);
        const int group = 6;
        Assert.Equal(15, group + group + 3);
        Assert.Equal(15, 2 * group + 3);
    }
}

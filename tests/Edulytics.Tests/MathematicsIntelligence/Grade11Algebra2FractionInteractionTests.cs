using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade 11 Stage D: exact fraction answer types must not crash the shared practice registry.</summary>
public sealed class Grade11Algebra2FractionInteractionTests
{
    private static readonly string[] FractionFamilies =
    [
        "usccss.g11.alg2.u01.l01.halving_fraction",
        "usccss.g11.alg2.u01.l02.paper_piece_area",
        "usccss.g11.alg2.u01.l02.rational_geometric_term",
        "usccss.g11.alg2.u01.l02.growth_factor"
    ];

    [Fact]
    public void Grade11FractionAnswersAreFractionInputsWithoutBreakingOtherGrades()
    {
        foreach (var family in FractionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family, out var interaction), family);
            Assert.NotNull(interaction);
            Assert.Equal(LessonPracticeInteractionKind.FractionEntry, interaction!.InteractionKind);
        }
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(
            "number.whole.add.direct", out var original));
        Assert.NotNull(original);
    }

    [Fact]
    public void Grade11FirstPartyFractionGeneratorRejectsMalformedAndWrongAnswers()
    {
        var engine = new ExactSkillContractQuestionEngine();
        foreach (var family in FractionFamilies)
        foreach (var seed in new [] { 20261106, 20261107 })
        {
            var questions = engine.Generate(
                "usccss-g11-alg2-d1", family, [family],
                ExactSkillQuestionDifficulty.Standard, 2, seed, []);
            Assert.Equal(2, questions.Count);
            foreach (var question in questions)
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(
                    family, question.Parameters, question.CorrectAnswer), family);
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family, question.Parameters, "invalid_fraction"));
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family, question.Parameters, "99999999"));
            }
        }
    }
}

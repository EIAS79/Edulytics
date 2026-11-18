using System.Globalization;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UaeGrade8U12L04SetPracticeRoutingTests
{
    private const string LessonCode =
        "PED:UAE-MOE-MATH:L8:GENERAL:12:04:USING-SETS-TO-SOLVE-PROBLEMS";
    private const string ExpectedFamily = "supporting.sets.union_count";

    [Fact]
    public void ProductionLessonRoutesToInclusionExclusionRatherThanFrequencyTableAddition()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(LessonCode, out var contract));
        Assert.NotNull(contract);
        Assert.Equal("READY_VERIFIED", contract!.Readiness);
        Assert.Equal("SET_WORD_PROBLEM_INCLUSION_EXCLUSION_VENN_COUNT", contract.Mechanic);
        Assert.Equal(ExpectedFamily, Assert.Single(contract.AllowedQuestionFamilies));
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(ExpectedFamily));

        var engine = new ExactSkillContractQuestionEngine();
        foreach (var seed in new[] { 31001, 31002, 31003 })
        {
            var questions = engine.Generate(
                "uae-g8-u12-l04-sets",
                LessonCode,
                contract.AllowedQuestionFamilies,
                ExactSkillQuestionDifficulty.Standard,
                5,
                seed,
                Array.Empty<string>());

            Assert.Equal(5, questions.Count);
            Assert.All(questions, question =>
            {
                Assert.Equal(ExpectedFamily, question.Family);
                Assert.Contains("A∩B", question.Prompt);
                Assert.DoesNotContain("frequency table", question.Prompt,
                    StringComparison.OrdinalIgnoreCase);

                var a = question.Parameters["a"];
                var b = question.Parameters["b"];
                var intersection = question.Parameters["intersection"];
                Assert.InRange(intersection, 1, Math.Min(a, b));
                var expected = (a + b - intersection)
                    .ToString(CultureInfo.InvariantCulture);
                Assert.Equal(expected, question.CorrectAnswer);

                Assert.True(ExactSkillContractQuestionEngine.Verify(
                    question.Family, question.Parameters, expected));
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    question.Family, question.Parameters,
                    (a + b).ToString(CultureInfo.InvariantCulture)));
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    question.Family, question.Parameters, "not-a-number"));
            });
        }
    }
}

using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssG2Unit3Lesson18YardstickQuestionsTests
{
    [Fact]
    public void FourLessonFamiliesResolveToNumericStudentInput()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            UsCcssG2Unit3EighteenthLessonYardstick.LessonCode, out var contract));
        Assert.NotNull(contract);
        Assert.Equal(4, contract!.AllowedQuestionFamilies.Count);
        foreach (var family in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family, out var interaction));
            Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad, interaction!.InteractionKind);
        }
    }

    [Fact]
    public void GeneratedYardstickMathPreservesCardEdgesAndRejectsCorruptParameters()
    {
        var generator = new ExactSkillContractQuestionEngine();
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            UsCcssG2Unit3EighteenthLessonYardstick.LessonCode, out var contract));
        foreach (var family in contract!.AllowedQuestionFamilies)
        foreach (var seed in new[] { 20261022, 20261023 })
        {
            var questions = generator.Generate("g2-u03-l18-source", family, [family],
                ExactSkillQuestionDifficulty.Standard, 4, seed, []);
            Assert.Equal(4, questions.Count);
            foreach (var question in questions)
            {
                var parameters = question.Parameters;
                Assert.Equal(3 * parameters["count3"] + 5 * parameters["count5"], parameters["marked"]);
                Assert.InRange(parameters["marked"], 8, 36);
                Assert.True(ExactSkillContractQuestionEngine.Verify(family, parameters, question.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family, parameters, "-1"));
                var invalid = parameters.ToDictionary(x => x.Key, x => x.Value, StringComparer.Ordinal);
                invalid["count3"] = 5;
                Assert.False(ExactSkillContractQuestionEngine.Verify(family, invalid, question.CorrectAnswer));
                if (family.EndsWith(".fictional_measurement_gap", StringComparison.Ordinal))
                    Assert.Contains("FICTIONAL", question.Prompt);
            }
        }
    }
}

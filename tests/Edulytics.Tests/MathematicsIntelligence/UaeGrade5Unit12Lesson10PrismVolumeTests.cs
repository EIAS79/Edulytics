using System.Globalization;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

// UAE Grade 5 U12 L10 prism volume pp.935–940, Arabic original source.
// Independent exact oracles and Arabic-digit acceptance.
public sealed class UaeGrade5Unit12Lesson10PrismVolumeTests
{
    [Theory]
    [InlineData("uae.g5.u12.l10.volume_lwh")]
    [InlineData("uae.g5.u12.l10.volume_base_area_height")]
    [InlineData("uae.g5.u12.l10.missing_prism_height")]
    [InlineData("uae.g5.u12.l10.compare_prism_volumes")]
    public void RectangularPrismVolumeRulesGiveExactAnswers(string family)
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(family, out var route));
        Assert.NotNull(route);
        var engine = new ExactSkillContractQuestionEngine();
        foreach (int seed in new[] { 101, 205, 309 })
        {
            var generated = engine.Generate("PED:UAE-MOE-MATH:L5:GENERAL:12:10",
                family, [family], ExactSkillQuestionDifficulty.Standard, 5, seed, []);
            Assert.NotEmpty(generated);
            foreach (var question in generated)
            {
                int a = question.Parameters["a"];
                int b = question.Parameters["b"];
                int c = question.Parameters["c"];
                Assert.True(a > 0 && b > 0 && c > 0);
                int oracle = family switch
                {
                    "uae.g5.u12.l10.missing_prism_height" => c,
                    "uae.g5.u12.l10.compare_prism_volumes" =>
                        a * (b + 1) * c == (a + 1) * b * c ? 0 :
                        a * (b + 1) * c > (a + 1) * b * c ? 1 : 2,
                    _ => a * b * c
                };
                Assert.Equal(oracle.ToString(CultureInfo.InvariantCulture), question.CorrectAnswer);
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,
                    question.Parameters, question.CorrectAnswer));
                string arabic = new(question.CorrectAnswer.Select(ch => (char)('٠' + ch - '0')).ToArray());
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,
                    question.Parameters, arabic));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,
                    question.Parameters, "wrong"));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,
                    question.Parameters, "999999"));
                var injected = question.Parameters.ToDictionary(kv => kv.Key, kv => kv.Value);
                injected.Add("answer", oracle);
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,
                    injected, question.CorrectAnswer));
            }
        }
    }
}

using System.Globalization;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

// UAE Grade5 U12 L07 original Arabic student book pp.915-920.
// Physical cut-and-fold network correctness remains teacher-scored.
public sealed class UaeGrade5Unit12Lesson07NetsTests
{
    [Theory]
    [InlineData("uae.g5.u12.l07.net_solid_classification")]
    [InlineData("uae.g5.u12.l07.net_total_faces")]
    [InlineData("uae.g5.u12.l07.net_congruent_matching_faces")]
    public void ThreeDimensionalNetDescriptionUsesCorrectFaceCounting(string family)
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(family, out var route));
        Assert.NotNull(route);

        var engine = new ExactSkillContractQuestionEngine();
        foreach (var seed in new[] { 123, 345, 567 })
        {
            var questions = engine.Generate("PED:UAE-MOE-MATH:L5:GENERAL:12:07",
                family, [family], ExactSkillQuestionDifficulty.Standard, 5, seed, []);
            Assert.NotEmpty(questions);
            foreach (var question in questions)
            {
                int kind = question.Parameters["a"];
                int baseWidth = question.Parameters["b"];
                int heightComponent = question.Parameters["c"];
                Assert.InRange(kind, 1, 2);
                Assert.InRange(baseWidth, 2, 8);
                Assert.InRange(heightComponent, 2, 6);

                int expected = family switch
                {
                    "uae.g5.u12.l07.net_solid_classification" => kind == 1 ? 0 : 1,
                    "uae.g5.u12.l07.net_total_faces" => 6,
                    _ => kind == 1 ? 6 : 2
                };
                Assert.Equal(expected.ToString(CultureInfo.InvariantCulture), question.CorrectAnswer);
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,
                    question.Parameters, question.CorrectAnswer));

                string arabicNumerals = new(question.CorrectAnswer.Select(ch =>
                    (char)('٠' + ch - '0')).ToArray());
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,
                    question.Parameters, arabicNumerals));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,
                    question.Parameters, "9999"));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,
                    question.Parameters, "not a number"));
                var forged = question.Parameters.ToDictionary(kv => kv.Key, kv => kv.Value);
                forged.Add("expected", expected);
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,
                    forged, question.CorrectAnswer));
            }
        }
    }
}

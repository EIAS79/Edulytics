using System.Globalization;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

// UAE Grade5 U12 L08, printed student Arabic pp.921–926.
// Classification uses the descriptions and exact 3D polyhedron elements.
public sealed class UaeGrade5Unit12Lesson08SolidElementsTests
{
    [Theory]
    [InlineData("uae.g5.u12.l08.solid_type")]
    [InlineData("uae.g5.u12.l08.solid_faces")]
    [InlineData("uae.g5.u12.l08.solid_edges")]
    [InlineData("uae.g5.u12.l08.solid_vertices")]
    public void CubesAndPrismsHaveCorrectFacesEdgesAndVertices(string family)
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(family, out var interaction));
        Assert.NotNull(interaction);

        var engine = new ExactSkillContractQuestionEngine();
        foreach (int seed in new[] { 107, 211, 307 })
        {
            var generated = engine.Generate("PED:UAE-MOE-MATH:L5:GENERAL:12:08",
                family, [family], ExactSkillQuestionDifficulty.Standard, 5, seed, []);
            Assert.NotEmpty(generated);
            foreach (var question in generated)
            {
                int type = question.Parameters["a"];
                Assert.InRange(type, 1, 3);
                int oracle = family switch
                {
                    "uae.g5.u12.l08.solid_type" => type - 1,
                    "uae.g5.u12.l08.solid_faces" => type == 3 ? 5 : 6,
                    "uae.g5.u12.l08.solid_edges" => type == 3 ? 9 : 12,
                    _ => type == 3 ? 6 : 8
                };
                Assert.Equal(oracle.ToString(CultureInfo.InvariantCulture), question.CorrectAnswer);
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,
                    question.Parameters, question.CorrectAnswer));
                string arabicDigits = new(question.CorrectAnswer.Select(ch => (char)('٠' + ch - '0')).ToArray());
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,
                    question.Parameters, arabicDigits));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,
                    question.Parameters, "not-a-number"));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,
                    question.Parameters, "99999"));
                var tampered = question.Parameters.ToDictionary(kv => kv.Key, kv => kv.Value);
                tampered.Add("expected", oracle);
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,
                    tampered, question.CorrectAnswer));
            }
        }
    }
}

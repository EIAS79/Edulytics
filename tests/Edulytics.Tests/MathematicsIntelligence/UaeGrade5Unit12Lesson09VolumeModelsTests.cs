using System.Globalization;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

// UAE Grade5 Arabic student U12 L09 pp.929–934.
// Real-world stacking and physical model quality are teacher-scored.
public sealed class UaeGrade5Unit12Lesson09VolumeModelsTests
{
    [Theory]
    [InlineData("uae.g5.u12.l09.model_cubes_per_layer")]
    [InlineData("uae.g5.u12.l09.model_total_unit_cubes")]
    [InlineData("uae.g5.u12.l09.model_volume_cm3")]
    [InlineData("uae.g5.u12.l09.model_missing_layers")]
    public void ExactUnitCubeLayerAndVolumeModelsAgree(string family)
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(family, out var route));
        Assert.NotNull(route);
        var engine = new ExactSkillContractQuestionEngine();
        foreach (int seed in new[] { 102, 203, 304 })
        {
            var questions = engine.Generate("PED:UAE-MOE-MATH:L5:GENERAL:12:09",
                family, [family], ExactSkillQuestionDifficulty.Standard, 5, seed, []);
            Assert.NotEmpty(questions);
            foreach (var question in questions)
            {
                int l = question.Parameters["a"];
                int w = question.Parameters["b"];
                int h = question.Parameters["c"];
                Assert.True(l > 0 && w > 0 && h > 0);
                int expected = family switch
                {
                    "uae.g5.u12.l09.model_cubes_per_layer" => l * w,
                    "uae.g5.u12.l09.model_missing_layers" => h,
                    _ => l * w * h
                };
                Assert.Equal(expected.ToString(CultureInfo.InvariantCulture), question.CorrectAnswer);
                Assert.True(ExactSkillContractQuestionEngine.Verify(family, question.Parameters,
                    question.CorrectAnswer));
                string arabicDigits = new(question.CorrectAnswer.Select(c => (char)('٠' + c - '0')).ToArray());
                Assert.True(ExactSkillContractQuestionEngine.Verify(family, question.Parameters,
                    arabicDigits));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family, question.Parameters,
                    "invalid"));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family, question.Parameters,
                    "999999"));
                var forged = question.Parameters.ToDictionary(kv => kv.Key, kv => kv.Value);
                forged.Add("expected", expected);
                Assert.False(ExactSkillContractQuestionEngine.Verify(family, forged,
                    question.CorrectAnswer));
            }
        }
    }
}

using System.Globalization;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

// Separate mathematics oracle for the seven geometry families that previously
// exhausted their unique-question pools. It does not use the family Solve method.
public sealed class UaeGrade3Unit13StageDIndependentMathTests
{
    [Theory]
    [InlineData("uae.g3.u13.l01.line_point_ray")]
    [InlineData("uae.g3.u13.l02.angle_types")]
    [InlineData("uae.g3.u13.l03.polygons")]
    [InlineData("uae.g3.u13.l05.quadrilaterals")]
    [InlineData("uae.g3.u13.l06.quadrilateral_attributes")]
    [InlineData("uae.g3.u13.l09.solid_figures")]
    [InlineData("uae.g3.u13.l10.more_solids")]
    public void FamilyProducesTwelveDistinctMathematicallyVerifiedItems(string family)
    {
        var engine = new ExactSkillContractQuestionEngine();
        var questions = engine.Generate(
            "uae-g3-stage-d-independent-math",
            family, [family],
            ExactSkillQuestionDifficulty.Standard,
            12, 20261110, []);

        Assert.Equal(12, questions.Count);
        foreach (var question in questions)
        {
            int a = question.Parameters["a"];
            int b = question.Parameters["b"];
            int mode = question.Parameters["mode"];
            Assert.InRange(b, 1, 6);
            var expected = family switch
            {
                "uae.g3.u13.l01.line_point_ray" =>
                    (a == 0 ? 2 : a == 1 ? 1 : 0) * b,
                "uae.g3.u13.l02.angle_types" => b,
                "uae.g3.u13.l03.polygons" => a * b,
                "uae.g3.u13.l05.quadrilaterals" =>
                    b * (mode == 0 ? 4 : a == 2 ? 0 : 4),
                "uae.g3.u13.l06.quadrilateral_attributes" =>
                    b * (mode == 0 ? 4 : 2),
                "uae.g3.u13.l09.solid_figures" =>
                    b * (a < 2 ? (mode == 0 ? 6 : 8)
                        : a == 2 ? (mode == 0 ? 2 : 0)
                        : a == 3 ? 1 : 0),
                "uae.g3.u13.l10.more_solids" =>
                    b * (a switch { 0 => 6, 1 => 6, 2 => 5,
                                     3 => 5, 4 => 12, _ => 8 }),
                _ => throw new InvalidOperationException("Unsupported family")
            };

            Assert.Equal(expected.ToString(CultureInfo.InvariantCulture),
                question.CorrectAnswer);
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                family, question.Parameters, question.CorrectAnswer));
            var wrong = expected == 0 ? "1" : "0";
            Assert.False(ExactSkillContractQuestionEngine.Verify(
                family, question.Parameters, wrong));

            // Reject tampering with the non-answer parameters too, not only
            // a modified 'result' field from older A-B-C tests.
            var invalid = question.Parameters.ToDictionary(x => x.Key, x => x.Value);
            invalid["b"] = 0;
            Assert.False(ExactSkillContractQuestionEngine.Verify(
                family, invalid, question.CorrectAnswer));

            var changedResult = question.Parameters.ToDictionary(x => x.Key, x => x.Value);
            changedResult["result"] = expected + 1;
            Assert.False(ExactSkillContractQuestionEngine.Verify(
                family, changedResult, question.CorrectAnswer));
        }
    }
}

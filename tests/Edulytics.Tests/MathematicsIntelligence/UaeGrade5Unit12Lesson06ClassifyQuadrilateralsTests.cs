using System.Globalization;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

// UAE Grade 5 U12 L06, student Arabic pp.909-914. Category hierarchy
// uses the textbook's exclusive definition of trapezoid (exactly one parallel pair).
public sealed class UaeGrade5Unit12Lesson06ClassifyQuadrilateralsTests
{
    [Theory]
    [InlineData("uae.g5.u12.l06.quadrilateral_most_specific")]
    [InlineData("uae.g5.u12.l06.quadrilateral_subclass")]
    public void QuadrilateralCategoriesRespectHierarchyAndIndependentOracle(string family)
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(family, out var route));
        Assert.NotNull(route);

        var engine = new ExactSkillContractQuestionEngine();
        foreach (int seed in new[] { 103, 209, 307 })
        {
            var questions = engine.Generate("PED:UAE-MOE-MATH:L5:GENERAL:12:06",
                family, [family], ExactSkillQuestionDifficulty.Standard, 5, seed, []);
            Assert.NotEmpty(questions);
            foreach (var q in questions)
            {
                int kind = q.Parameters["kind"];
                Assert.InRange(kind, 0, 7);
                int expected;
                if (family == "uae.g5.u12.l06.quadrilateral_most_specific")
                {
                    Assert.Single(q.Parameters);
                    expected = new[] { 4, 3, 1, 2, 0, 0, 5, 5 }[kind];
                }
                else
                {
                    Assert.Equal(2, q.Parameters.Count);
                    int target = q.Parameters["target"];
                    Assert.InRange(target, 0, 4);
                    expected = target switch
                    {
                        0 => kind < 4 ? 1 : 0,
                        1 => kind is 0 or 1 ? 1 : 0,
                        2 => kind is 0 or 3 ? 1 : 0,
                        3 => kind == 0 ? 1 : 0,
                        _ => kind is 4 or 5 ? 1 : 0
                    };
                }

                Assert.Equal(expected.ToString(CultureInfo.InvariantCulture), q.CorrectAnswer);
                Assert.True(ExactSkillContractQuestionEngine.Verify(family, q.Parameters, q.CorrectAnswer));
                string arabicNumerals = new(q.CorrectAnswer.Select(c => (char)('٠' + c - '0')).ToArray());
                Assert.True(ExactSkillContractQuestionEngine.Verify(family, q.Parameters, arabicNumerals));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family, q.Parameters, "999"));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family, q.Parameters, "invalid"));
                var forged = q.Parameters.ToDictionary(kv => kv.Key, kv => kv.Value);
                forged.Add("expected", expected);
                Assert.False(ExactSkillContractQuestionEngine.Verify(family, forged, q.CorrectAnswer));
            }
        }
    }
}

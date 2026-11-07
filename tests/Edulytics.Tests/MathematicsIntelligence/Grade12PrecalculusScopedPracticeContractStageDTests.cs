using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Six first-party precalculus lessons have a real, independently verifiable
/// numerical Practice subset. This does not approve graphs, source-book
/// equivalence, teacher grading or the complete lesson for release.
/// </summary>
public sealed class Grade12PrecalculusScopedPracticeContractStageDTests
{
    private const string Prefix =
        "PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:";

    public static IEnumerable<object[]> Cases()
    {
        yield return ["U01:L04", "usccss.hs.inverse_composition"];
        yield return ["U01:L05", "functions.absolute_value.translate_evaluate"];
        yield return ["U01:L06", "functions.absolute_value.translate_evaluate"];
        yield return ["U02:L01", "geometry.coordinate.gradient_between_points"];
        yield return ["U02:L02", "geometry.coordinate.y_intercept_from_rule"];
        yield return ["U02:L04", "statistics.linear_model.predict"];
    }

    [Theory]
    [MemberData(nameof(Cases))]
    public void StudentPracticeFamilyActuallyGeneratesAndVerifies(
        string suffix, string expectedFamily)
    {
        var code = Prefix + suffix;
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(code, out var contract),
            code);
        Assert.NotNull(contract);
        Assert.Equal("READY_VERIFIED", contract.Readiness);
        Assert.Contains(expectedFamily, contract.AllowedQuestionFamilies);

        var engine = new ExactSkillContractQuestionEngine();
        foreach (var difficulty in new[]
        {
            ExactSkillQuestionDifficulty.Standard,
            ExactSkillQuestionDifficulty.Stretch,
            ExactSkillQuestionDifficulty.Challenge
        })
        {
            var question = Assert.Single(engine.Generate(
                "g12-scoped-stage-d",
                code,
                [expectedFamily],
                difficulty,
                1,
                702501,
                []));
            Assert.Equal(expectedFamily, question.Family);
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                question.Family, question.Parameters, question.CorrectAnswer),
                $"Engine oracle rejected {code} / {difficulty}");
        }
    }
}

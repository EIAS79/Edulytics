using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Twenty-one first-party precalculus lessons have a real, independently verifiable
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
        yield return ["U03:L08", "usccss.hs.restricted_inverse"];
        yield return ["U04:L01", "functions.exponential.growth.evaluate"];
        yield return ["U04:L02", "functions.exponential.growth.evaluate"];
        yield return ["U04:L07", "functions.exponential.growth.evaluate"];
        yield return ["U09:L01", "algebra.linear.systems.two_by_two.integer_coefficients"];
        yield return ["U09:L06", "algebra.linear.systems.two_by_two.integer_coefficients"];
        yield return ["U09:L08", "algebra.linear.systems.two_by_two.integer_coefficients"];
        yield return ["U11:L07", "probability.theoretical.complement"];
        yield return ["U03:L03", "functions.quadratic.output_roots.solve"];
        yield return ["U03:L06", "functions.quadratic.output_roots.solve"];
        yield return ["U05:L04", "trigonometry.right_triangle.ratio_exact"];
        yield return ["U08:L08", "vectors.add.exact_rational"];
        yield return ["U11:L01", "sequences.core.arithmetic_nth_term"];
        yield return ["U11:L02", "sequences.core.arithmetic_nth_term"];
        yield return ["U11:L03", "sequences.core.geometric_nth_term"];
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

using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssG6FiniteGeometryCompositeSessionTests
{
    [Theory]
    [InlineData("usccss.g6.u01.l02.tangram_equal_area")]
    [InlineData("usccss.g6.u01.l02.tangram_piece_area")]
    [InlineData("usccss.g6.u01.l03.area_conservation")]
    [InlineData("usccss.g6.u01.l03.source_shaded_area")]
    [InlineData("usccss.g6.u01.l04.equal_area_rearrangement")]
    [InlineData("usccss.g6.u01.l04.parallelogram_exact_area")]
    [InlineData("usccss.g6.u01.l04.two_pairs_parallel")]
    [InlineData("usccss.g6.u01.l05.area_scaling")]
    [InlineData("usccss.g6.u01.l05.corresponding_height")]
    [InlineData("usccss.g6.u01.l05.small_triangle_fraction")]
    [InlineData("usccss.g6.u01.l06.composite_area")]
    [InlineData("usccss.g6.u01.l06.equal_area_reasoning")]
    [InlineData("usccss.g6.u01.l07.original_fraction_dimensions")]
    [InlineData("usccss.g6.u01.l07.parallelogram_diagonal_half")]
    [InlineData("usccss.g6.u01.l07.triangle_composition_logic")]
    [InlineData("usccss.g6.u01.l08.decomposition_invariance")]
    [InlineData("usccss.g6.u01.l08.original_decimal_practice")]
    [InlineData("usccss.g6.u01.l08.triangle_half_area")]
    [InlineData("usccss.g6.u01.l09.triangle_height_validity")]
    [InlineData("usccss.g6.u01.l10.height_construction_validity")]
    [InlineData("usccss.g6.u01.l12.surface_area_meaning")]
    [InlineData("usccss.g6.u01.l15.net_design_and_volume_reasoning")]
    [InlineData("usccss.g6.u01.l16.surface_volume_unit_logic")]
    public void FiniteSourceVariantsCreateTenUniqueVerifiableCompositeGeometryProblems(string family)
    {
        var questions = new ExactSkillContractQuestionEngine().Generate(
            "grade6-source-geometry-compound-regression", family, [family],
            ExactSkillQuestionDifficulty.Standard, 10, 20261104, []);

        Assert.Equal(10, questions.Count);
        Assert.Equal(10, questions.Select(q => q.ExposureFingerprint)
            .Distinct(StringComparer.Ordinal).Count());

        foreach (var question in questions)
        {
            Assert.Equal(family, question.Family);
            Assert.Contains("Situation A:", question.Prompt, StringComparison.Ordinal);
            Assert.Contains("Situation B:", question.Prompt, StringComparison.Ordinal);
            Assert.Equal(4, question.Parameters.Count);
            Assert.Equal(1, question.Parameters["g6Composite"]);
            Assert.InRange(question.Parameters["copies"], 2, 7);
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                family, question.Parameters, question.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(
                family, question.Parameters,
                question.CorrectAnswer == "0" ? "1" : "0"));
            Assert.False(ExactSkillContractQuestionEngine.Verify(
                family, question.Parameters, "1/0"));
            Assert.False(ExactSkillContractQuestionEngine.Verify(
                family, new Dictionary<string, int>
                {
                    ["g6Composite"] = 1,
                    ["a"] = question.Parameters["a"],
                    ["b"] = question.Parameters["b"],
                    ["copies"] = 2000000
                }, question.CorrectAnswer));
        }
    }
}

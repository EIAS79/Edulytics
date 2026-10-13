using System.Globalization;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssB01SourceAlignedPracticeCorrectionTests
{
    private static readonly IReadOnlyDictionary<string, string[]> Expected =
        new Dictionary<string, string[]>(StringComparer.Ordinal)
        {
            ["PED:US-CCSS-MATH:G1:U01:L12"] =
                ["supporting.statistics.category_total"],
            ["PED:US-CCSS-MATH:G2:U01:L11"] =
                ["supporting.statistics.category_total",
                 "supporting.statistics.compare_category_counts"],
            ["PED:US-CCSS-MATH:G2:U05:L01"] =
                ["supporting.number.place_value"],
            ["PED:US-CCSS-MATH:G2:U05:L05"] =
                ["supporting.number.place_value"],
            ["PED:US-CCSS-MATH:G1:U06:L07"] =
                ["supporting.measurement.unit_iteration"],
            ["PED:US-CCSS-MATH:G1:U07:L14"] =
                ["usccss.b01.clock.read_half_hour"],
            ["PED:US-CCSS-MATH:G3:U08:L10"] =
                ["supporting.number.multiply", "supporting.number.divide"],
            ["PED:US-CCSS-MATH:G3:U01:L15"] =
                ["number.whole.multiply.fact_recall.apply"],
            ["PED:US-CCSS-MATH:G3:U04:L07"] =
                ["supporting.number.multiply", "supporting.number.divide"],
            ["PED:US-CCSS-MATH:G4:U01:L04"] =
                ["number.whole.multiply.fact_recall.build",
                 "number.whole.multiply.fact_recall.apply"],
            ["PED:US-CCSS-MATH:G4:U01:L05"] =
                ["supporting.sequences.skip_count"]
        };

    [Fact]
    public void ElevenSourceMatchedLessonContractsDoNotGenerateLegacyWrongTopic()
    {
        foreach (var (code, families) in Expected)
        {
            Assert.True(LessonPracticeCapabilityResolver.TryResolve(code, out var contract));
            Assert.NotNull(contract);
            Assert.Equal(families, contract!.AllowedQuestionFamilies);
            Assert.StartsWith("IM2021_", contract.SourceType, StringComparison.Ordinal);
            Assert.DoesNotContain(contract.AllowedQuestionFamilies,
                f => f.StartsWith("ratio.unit_rate.", StringComparison.Ordinal) ||
                     f.StartsWith("statistics.center_spread.", StringComparison.Ordinal) ||
                     f.Contains("circle_area_pi", StringComparison.Ordinal));
        }
        // Preserve the sole historically certified lesson's special contract.
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G1:U01:L01", out var pilot));
        Assert.Contains("usccss.g1.u01.l01.count_collection_to20",
            pilot!.AllowedQuestionFamilies);
    }

    [Fact]
    public void CorrectedFamiliesGenerateAndIndependentlyCheckEveryDifficulty()
    {
        var engine = new ExactSkillContractQuestionEngine();
        var seed = 720000;
        var wrongNumericRejected = 0;
        var generated = 0;
        foreach (var (code, families) in Expected)
            foreach (var family in families)
                foreach (var difficulty in new[] {
                    ExactSkillQuestionDifficulty.Standard,
                    ExactSkillQuestionDifficulty.Stretch,
                    ExactSkillQuestionDifficulty.Challenge
                })
                {
                    var q = Assert.Single(engine.Generate(
                        "usccss-b01-source-aligned-families",
                        code, [family], difficulty, 1, seed++, []));
                    Assert.Equal(family,q.Family);
                    Assert.True(ExactSkillContractQuestionEngine.Verify(
                        family, q.Parameters, q.CorrectAnswer));
                    Assert.False(string.IsNullOrWhiteSpace(q.Prompt));
                    generated++;
                    if (int.TryParse(q.CorrectAnswer,NumberStyles.Integer,
                        CultureInfo.InvariantCulture,out var answer))
                    {
                        Assert.False(ExactSkillContractQuestionEngine.Verify(
                            family,q.Parameters,(answer+999).ToString(
                                CultureInfo.InvariantCulture)));
                        wrongNumericRejected++;
                    }
                }
        Assert.Equal(45,generated);
        Assert.True(wrongNumericRejected>0);
    }
}

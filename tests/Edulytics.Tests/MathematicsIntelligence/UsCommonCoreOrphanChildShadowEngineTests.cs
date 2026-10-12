using System.Text.Json;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Runtime smoke and negative-answer probing of pre-existing exact-family
/// mathematical kernels that MAY be relevant to fifteen unresolved official
/// child clauses. It grants no publisher source linkage, LessonPractice
/// authorization, independent mathematical signoff, or mastery credit.
/// </summary>
public sealed class UsCommonCoreOrphanChildShadowEngineTests
{
    private static readonly string[] CandidateFamilies =
    [
        "usccss.g3.rectangle_unit_tiles",
        "usccss.g5.unit_cube_count",
        "usccss.g6.observation_count",
        "usccss.hs.function_composition",
        "usccss.hs.inverse_composition",
        "usccss.hs.inverse_table",
        "usccss.hs.restricted_inverse",
        "usccss.hs.rational_hole_or_pole",
        "usccss.vector.add_x",
        "usccss.vector.resultant_angle",
        "usccss.vector.subtract_y",
        "usccss.vector.scalar_y",
        "usccss.vector.scalar_magnitude",
        "usccss.probability.expected_net_payoff",
        "usccss.probability.compare_expected_cost"
    ];

    [Fact]
    public void AllFifteenExistingCandidatesRemainNonProductionRegistryFamilies()
    {
        var assembly = typeof(LessonPracticeCapabilityResolver).Assembly;
        using var stream = assembly.GetManifestResourceStream(
            "Edulytics.Core.Mathematics.Generation.question-family-registry.v1.json");
        Assert.NotNull(stream);
        using var document = JsonDocument.Parse(stream!);
        var rows = document.RootElement.GetProperty("families").EnumerateArray()
            .ToArray();

        Assert.Equal(15, CandidateFamilies.Length);
        Assert.Equal(15, CandidateFamilies.Distinct(StringComparer.Ordinal).Count());

        foreach (var family in CandidateFamilies)
        {
            var entry = Assert.Single(rows, x =>
                x.GetProperty("id").GetString() == family);

            Assert.Equal("ShadowVerified",
                entry.GetProperty("status").GetString());
            Assert.False(entry.GetProperty("productionRouting").GetBoolean());
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));

            // lessonPracticeRouting on older family manifests can be true.
            // Neither that field nor SupportsFamily() confers actual
            // ReadyVerified LessonPractice capability for an orphan clause.
            Assert.True(entry.TryGetProperty("lessonPracticeRouting", out _));
        }
    }

    [Fact]
    public void FourHundredFiftyShadowEngineQuestionsRejectAlteredAnswers()
    {
        var engine = new ExactSkillContractQuestionEngine();
        var total = 0;
        foreach (var family in CandidateFamilies)
        {
            foreach (var difficulty in new[]
                     {
                         ExactSkillQuestionDifficulty.Standard,
                         ExactSkillQuestionDifficulty.Stretch,
                         ExactSkillQuestionDifficulty.Challenge
                     })
            {
                for (var seed = 1; seed <= 10; seed++)
                {
                    var item = Assert.Single(engine.Generate(
                        "us-ccss-orphan-shadow-engine-test",
                        "academic-orphan-source-hypothesis",
                        [family],
                        difficulty,
                        1,
                        530000 + total * 100 + seed,
                        []));
                    Assert.Equal(family, item.Family);
                    Assert.NotEmpty(item.Parameters);
                    Assert.False(string.IsNullOrWhiteSpace(item.CorrectAnswer));
                    Assert.True(ExactSkillContractQuestionEngine.Verify(
                        family, item.Parameters, item.CorrectAnswer));

                    var incorrect = item.CorrectAnswer == "0" ? "1" : "0";
                    Assert.False(ExactSkillContractQuestionEngine.Verify(
                        family, item.Parameters, incorrect));
                    total++;
                }
            }
        }
        Assert.Equal(450, total);
    }
}

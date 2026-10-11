using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Audits scoped Practice remediation without asserting full child-clause certification.</summary>
public sealed class UsCommonCoreRemainingClauseTargetedTests
{
    private static readonly string[] Families =
    [
        "usccss.g3.rectangle_unit_tiles",
        "usccss.g5.unit_cube_count",
        "usccss.g6.observation_count",
        "usccss.hs.function_sum",
        "usccss.hs.function_composition",
        "usccss.hs.inverse_composition",
        "usccss.hs.inverse_table",
        "usccss.hs.restricted_inverse",
        "usccss.hs.rational_vertical_asymptote",
        "usccss.hs.rational_x_intercept",
        "usccss.hs.rational_horizontal_asymptote"
    ];

    private static readonly Dictionary<string, string[]> PublisherLessons = new(StringComparer.Ordinal)
    {
        ["PED:US-CCSS-MATH:G3:U02:L08"] = ["usccss.g3.rectangle_unit_tiles"],
        ["PED:US-CCSS-MATH:G5:U01:L01"] = ["usccss.g5.unit_cube_count"],
        ["PED:US-CCSS-MATH:G6:U08:L17"] = ["usccss.g6.observation_count"],
        ["PED:US-CCSS-MATH:HS:TRAD:ALG2:IM-1E:U05:L10"] = ["usccss.hs.function_sum"],
        ["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U04:L15"] = ["usccss.hs.inverse_composition", "usccss.hs.inverse_table"],
        ["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U04:L16"] = ["usccss.hs.inverse_table"],
        ["PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U04:L17"] = ["usccss.hs.inverse_composition"],
        ["PED:US-CCSS-MATH:HS:TRAD:ALG2:IM-1E:U02:L17"] = ["usccss.hs.rational_vertical_asymptote", "usccss.hs.rational_x_intercept", "usccss.hs.rational_horizontal_asymptote"]
    };

    [Fact]
    public void ExactPublisherLessonsHaveGradeAppropriateFamilyRouting()
    {
        Assert.Equal(8, PublisherLessons.Count);
        foreach (var (lessonCode, families) in PublisherLessons)
        {
            Assert.True(LessonPracticeCapabilityResolver.TryResolve(lessonCode, out var contract),
                lessonCode);
            Assert.NotNull(contract);
            Assert.Equal(families, contract!.AllowedQuestionFamilies);
            Assert.DoesNotContain("supporting.logarithms.evaluate", contract.AllowedQuestionFamilies);
            Assert.DoesNotContain("supporting.algebra.quadratic_larger_root", contract.AllowedQuestionFamilies);
        }
    }

    [Fact]
    public void ElevenExactFamiliesProduceIndependentlyVerifiedAnswersForManySeeds()
    {
        var engine = new ExactSkillContractQuestionEngine();
        var total = 0;
        foreach (var family in Families)
        {
            var lesson = PublisherLessons
                .FirstOrDefault(x => x.Value.Contains(family, StringComparer.Ordinal)).Key
                ?? "PED:US-CCSS-MATH:HS:TRAD:ALG2:IM-1E:U05:L10";

            foreach (var difficulty in new[]
            {
                ExactSkillQuestionDifficulty.Standard,
                ExactSkillQuestionDifficulty.Stretch,
                ExactSkillQuestionDifficulty.Challenge
            })
            {
                for (var seed = 0; seed < 12; seed++)
                {
                    var question = Assert.Single(engine.Generate(
                        "us-ccss-remaining-clause-audit",
                        lesson,
                        [family],
                        difficulty,
                        1,
                        9000000 + total,
                        []));
                    Assert.Equal(family, question.Family);
                    Assert.True(ExactSkillContractQuestionEngine.Verify(
                        question.Family, question.Parameters, question.CorrectAnswer));
                    total++;
                }
            }
        }
        Assert.Equal(11 * 3 * 12, total);
    }
}
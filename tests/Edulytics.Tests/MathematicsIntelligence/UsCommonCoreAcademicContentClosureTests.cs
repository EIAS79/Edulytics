using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCommonCoreAcademicContentClosureTests
{
    private const string Vectors = "PED:US-CCSS-MATH:HS:FOURTH:VECTOR-MATRIX:U01:L01";
    private const string ExpectedValue = "PED:US-CCSS-MATH:HS:FOURTH:PROB-DECISION:U01:L05";

    private static readonly string[] VectorFamilies =
    [
        "usccss.vector.add_x",
        "usccss.vector.subtract_y",
        "usccss.vector.scalar_y",
        "usccss.vector.sum_magnitude",
        "usccss.vector.scalar_magnitude",
        "usccss.vector.resultant_angle"
    ];

    private static readonly string[] ExpectedValueFamilies =
    [
        "usccss.probability.expected_net_payoff",
        "usccss.probability.compare_expected_cost"
    ];

    [Theory]
    [InlineData(Vectors)]
    [InlineData(ExpectedValue)]
    public void AcademicRepairContentIsSpecificAndDigestMatchesUpdatedBody(string lessonCode)
    {
        var relativeFile = lessonCode == Vectors
            ? "us-ccss-math-hs-vector-matrix-phase29-v1.lesson-content-pack.json"
            : "us-ccss-math-hs-prob-decision-phase29-v1.lesson-content-pack.json";
        var assembly = typeof(Edulytics.Core.Curriculum.MathematicsCurriculumPackRegistry).Assembly;
        var resource = Assert.Single(assembly.GetManifestResourceNames(),
            x => x.EndsWith(relativeFile, StringComparison.OrdinalIgnoreCase));
        using var stream = assembly.GetManifestResourceStream(resource)!;
        using var doc = JsonDocument.Parse(stream);
        var lesson = Assert.Single(doc.RootElement.GetProperty("lessons").EnumerateArray(),
            x => x.GetProperty("lessonCode").GetString() == lessonCode);
        var translation = Assert.Single(lesson.GetProperty("translations").EnumerateArray(),
            x => x.GetProperty("cultureCode").GetString() == "en");
        var fields = new[] { "title", "explanation", "keyConceptsAndRules", "workedExamples",
            "stepByStepSolutions", "commonMistakes", "quickSummary" };
        var text = string.Join("\n", fields.Select(f =>
            f + ":" + translation.GetProperty(f).GetString()));
        var digest = Convert.ToHexString(SHA256.HashData(
            Encoding.UTF8.GetBytes(text))).ToLowerInvariant();
        Assert.Equal(digest, lesson.GetProperty("canonicalBodySha256").GetString());
        Assert.Matches("^[0-9a-f]{64}$", lesson.GetProperty("sourceSha256").GetString());
        Assert.DoesNotContain("Skip to main content", text, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("Reader Mode", text, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("Deki.Logic", text, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("Connecting the equations of parabolas", text,
            StringComparison.OrdinalIgnoreCase);
        Assert.True(translation.GetProperty("workedExamples").GetString()!.Length > 550);
        if (lessonCode == Vectors)
        {
            Assert.Contains("parallelogram", text, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("u−v", text, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("||kv||", text, StringComparison.Ordinal);
        }
        else
        {
            Assert.Contains("expected NET payoff", text, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("Plan B", text, StringComparison.Ordinal);
            Assert.Contains("negative expectation", text, StringComparison.OrdinalIgnoreCase);
        }
    }

    [Theory]
    [InlineData(Vectors, true)]
    [InlineData(ExpectedValue, false)]
    public void CorrectedLessonsRouteToExactRelevantFamilies(string lessonCode, bool isVector)
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(lessonCode, out var contract));
        Assert.NotNull(contract);
        Assert.Equal(isVector ? VectorFamilies : ExpectedValueFamilies,
            contract!.AllowedQuestionFamilies);
        Assert.DoesNotContain("supporting.probability_statistics.mixed",
            contract.AllowedQuestionFamilies);
    }

    [Fact]
    public void AcademicRepairQuestionFamiliesProduceCorrectVerifiedAnswersAcrossSeeds()
    {
        var engine = new ExactSkillContractQuestionEngine();
        var checkedCount = 0;
        foreach (var family in VectorFamilies.Concat(ExpectedValueFamilies))
        {
            var lesson = family.StartsWith("usccss.vector.", StringComparison.Ordinal)
                ? Vectors : ExpectedValue;
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
                        "us-ccss-content-closure",
                        lesson,
                        [family],
                        difficulty,
                        1,
                        checkedCount + 7800000,
                        []));
                    Assert.Equal(family, question.Family);
                    Assert.True(ExactSkillContractQuestionEngine.Verify(
                        question.Family, question.Parameters, question.CorrectAnswer));
                    checkedCount++;
                }
            }
        }
        Assert.Equal(8 * 3 * 12, checkedCount);
    }
}
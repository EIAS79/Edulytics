using System.Text.Json;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Edulytics.Services.Practice;
using Edulytics.Web.Presentation;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class PracticeV2NarrowFamilyRemediationTests
{
    private static readonly string[] RemediatedFamilies =
    [
        "supporting.statistics.collection_method",
        "supporting.circle.angle_semicircle",
        "supporting.geometry.locus_equidistant",
        "supporting.probability.independent_product",
        "supporting.probability.normal_symmetry"
    ];

    [Theory]
    [MemberData(nameof(RemediatedFamilyData))]
    public void RemediatedFamilySupportsCertifiedStandardAndProgressiveSessions(
        string family)
    {
        var contract = new Stage18PracticeSkillContract(
            "PRACTICE-V2-REMEDIATION:" + family,
            "practice.v2.remediation",
            "PRACTICE_V2_REMEDIATION",
            [family]);

        var engine = new Stage18SkillContractPracticeEngine();
        var validator = new PracticeSessionQualityValidator();

        var standard = engine.GenerateComposed(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            contract,
            StudentPrivatePracticeDifficulty.MyLevel,
            10,
            20260923,
            [],
            Guid.NewGuid());

        var standardQuality = validator.Validate(
            contract,
            standard,
            10);

        Assert.True(
            standardQuality.IsReady,
            $"{family} Standard failed: {standardQuality.StatusCode} " +
            string.Join(",", standardQuality.ReasonCodes));
        Assert.True(standard.Count >= 3);
        Assert.Equal(
            standard.Count,
            standard
                .Select(SemanticKey)
                .Distinct(StringComparer.Ordinal)
                .Count());

        var progressive = engine.GenerateProgressiveLesson(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            contract,
            20260924,
            [],
            Guid.NewGuid());

        var progressiveQuality = validator.Validate(
            contract,
            progressive,
            8);

        Assert.True(
            progressiveQuality.IsReady,
            $"{family} Progressive failed: {progressiveQuality.StatusCode} " +
            string.Join(",", progressiveQuality.ReasonCodes));
        Assert.Equal(8, progressive.Count);
        Assert.Equal(
            progressive.Count,
            progressive
                .Select(SemanticKey)
                .Distinct(StringComparer.Ordinal)
                .Count());

        Assert.All(
            standard.Concat(progressive),
            item =>
                Assert.True(
                    Stage18SkillContractPracticeEngine.VerifyPersistedItem(
                        contract,
                        item)));
    }

    [Fact]
    public void EnrichedFamiliesPreserveLegacyPersistedParameterVerification()
    {
        Assert.True(
            ExactSkillContractQuestionEngine.Verify(
                "supporting.probability.independent_product",
                new Dictionary<string, int> { ["variant"] = 431 },
                "1/4"));
        Assert.True(
            ExactSkillContractQuestionEngine.Verify(
                "supporting.probability.normal_symmetry",
                new Dictionary<string, int> { ["variant"] = 322 },
                "1/2"));
        Assert.True(
            ExactSkillContractQuestionEngine.Verify(
                "supporting.circle.angle_semicircle",
                new Dictionary<string, int> { ["variant"] = 777 },
                "90"));
        Assert.True(
            ExactSkillContractQuestionEngine.Verify(
                "supporting.geometry.locus_equidistant",
                new Dictionary<string, int> { ["variant"] = 51 },
                "perpendicular bisector"));
        Assert.True(
            ExactSkillContractQuestionEngine.Verify(
                "supporting.statistics.collection_method",
                new Dictionary<string, int>
                {
                    ["method"] = 1,
                    ["variant"] = 7
                },
                "sample"));
    }

    [Fact]
    public void LocusVisualShowsGivenGeometryWithoutDrawingOrNamingTheAnswerLocus()
    {
        var json = JsonSerializer.Serialize(new
        {
            parameters = new
            {
                mode = 1,
                start = -4,
                end = 8,
                midpoint = 2,
                variant = 5
            }
        });

        var svg = PracticeMathVisualRenderer.RenderSvg(
            "supporting.geometry.locus_equidistant",
            json);

        Assert.False(string.IsNullOrWhiteSpace(svg));
        Assert.Contains("A", svg!, StringComparison.Ordinal);
        Assert.Contains("B", svg!, StringComparison.Ordinal);
        Assert.Contains("(-4, 0)", svg!, StringComparison.Ordinal);
        Assert.Contains("(8, 0)", svg!, StringComparison.Ordinal);
        Assert.DoesNotContain(
            "perpendicular bisector",
            svg!,
            StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain(
            "x = 2",
            svg!,
            StringComparison.OrdinalIgnoreCase);
    }

    [Theory]
    [MemberData(nameof(RemediatedFamilyData))]
    public void EveryRemediatedFamilyExposesAllThreeCognitiveDifficultyLevels(
        string family)
    {
        var capabilities =
            PracticeQuestionFormCapabilityRegistry.Resolve(family);

        Assert.NotEmpty(capabilities);

        foreach (var difficulty in Enum.GetValues<PracticeCognitiveDifficulty>())
        {
            Assert.Contains(
                capabilities,
                capability =>
                    difficulty >= capability.MinimumDifficulty &&
                    difficulty <= capability.MaximumDifficulty);
        }
    }

    public static IEnumerable<object[]> RemediatedFamilyData() =>
        RemediatedFamilies.Select(family => new object[] { family });

    private static string SemanticKey(
        Edulytics.Core.Entities.AssessmentItem item)
    {
        using var document = JsonDocument.Parse(
            item.GenerationParametersJson!);
        var values = document.RootElement
            .GetProperty("parameters")
            .EnumerateObject()
            .ToDictionary(
                property => property.Name,
                property => property.Value.GetInt32(),
                StringComparer.Ordinal);

        return PracticeSemanticQuestionIdentityPolicy.Create(
            item.GenerationFamily!,
            values).Key;
    }
}

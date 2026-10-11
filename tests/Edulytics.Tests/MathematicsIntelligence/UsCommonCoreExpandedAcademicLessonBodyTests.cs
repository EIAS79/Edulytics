using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Pedagogical regression: six source-constrained learner-facing lessons have
/// coherent worked examples and cannot silently revert to scraped prompts.
/// This is not complete certification of the associated child standard clauses.
/// </summary>
public sealed class UsCommonCoreExpandedAcademicLessonBodyTests
{
    [Theory]
    [InlineData("PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U04:L15", "us-ccss-math-hs-alg1-phase29-v1.lesson-content-pack.json", "1,932", "currency")]
    [InlineData("PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U04:L16", "us-ccss-math-hs-alg1-phase29-v1.lesson-content-pack.json", "68°F", "Fahrenheit")]
    [InlineData("PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U04:L17", "us-ccss-math-hs-alg1-phase29-v1.lesson-content-pack.json", "w⁻¹", "water")]
    [InlineData("PED:US-CCSS-MATH:HS:TRAD:ALG2:IM-1E:U02:L17", "us-ccss-math-hs-alg2-phase29-v1.lesson-content-pack.json", "asymptote", "10-mile")]
    [InlineData("PED:US-CCSS-MATH:HS:TRAD:ALG2:IM-1E:U02:L18", "us-ccss-math-hs-alg2-phase29-v1.lesson-content-pack.json", "horizontal asymptote", "hole")]
    [InlineData("PED:US-CCSS-MATH:HS:TRAD:ALG2:IM-1E:U05:L10", "us-ccss-math-hs-alg2-phase29-v1.lesson-content-pack.json", "8.178", "book")]
    public void RepairedContentHasVerifiedDigestAndSpecificWorkedMathematics(
        string lessonCode, string packName, string expectedProof, string context)
    {
        var assembly = typeof(Edulytics.Core.Curriculum.MathematicsCurriculumPackRegistry).Assembly;
        var resource = Assert.Single(assembly.GetManifestResourceNames(),
            x => x.EndsWith(packName, StringComparison.OrdinalIgnoreCase));
        using var stream = assembly.GetManifestResourceStream(resource)!;
        using var document = JsonDocument.Parse(stream);
        var lesson = Assert.Single(document.RootElement.GetProperty("lessons").EnumerateArray(),
            x => x.GetProperty("lessonCode").GetString() == lessonCode);
        var body = Assert.Single(lesson.GetProperty("translations").EnumerateArray(),
            x => x.GetProperty("cultureCode").GetString() == "en");

        var fields = new[] { "title", "explanation", "keyConceptsAndRules", "workedExamples",
            "stepByStepSolutions", "commonMistakes", "quickSummary" };
        var canonical = string.Join("\n", fields.Select(k =>
            k + ":" + body.GetProperty(k).GetString()));
        var digest = Convert.ToHexString(SHA256.HashData(
            Encoding.UTF8.GetBytes(canonical))).ToLowerInvariant();

        Assert.Equal(lesson.GetProperty("canonicalBodySha256").GetString(), digest);
        Assert.Matches("^[0-9a-f]{64}$", lesson.GetProperty("sourceSha256").GetString());
        Assert.Contains(expectedProof, canonical, StringComparison.OrdinalIgnoreCase);
        Assert.Contains(context, canonical, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("Read the hour and minute hands", canonical, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("Step 1: Identify what the task asks you", canonical, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("You will writing Inverse", canonical, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("Approach: Represent the quantities", canonical, StringComparison.OrdinalIgnoreCase);
        Assert.True(body.GetProperty("workedExamples").GetString()!.Length > 400);
        Assert.True(body.GetProperty("stepByStepSolutions").GetString()!.Length > 350);
        Assert.True(body.GetProperty("commonMistakes").GetString()!.Length > 150);
    }

    [Fact]
    public void FunctionCompositionIsNotMisrepresentedAsFunctionArithmetic()
    {
        var assembly = typeof(Edulytics.Core.Curriculum.MathematicsCurriculumPackRegistry).Assembly;
        var name = Assert.Single(assembly.GetManifestResourceNames(),
            x => x.EndsWith("us-ccss-math-hs-alg2-phase29-v1.lesson-content-pack.json",
                StringComparison.OrdinalIgnoreCase));
        using var stream = assembly.GetManifestResourceStream(name)!;
        using var doc = JsonDocument.Parse(stream);
        var lesson = Assert.Single(doc.RootElement.GetProperty("lessons").EnumerateArray(),
            x => x.GetProperty("lessonCode").GetString() ==
                "PED:US-CCSS-MATH:HS:TRAD:ALG2:IM-1E:U05:L10");
        var t = Assert.Single(lesson.GetProperty("translations").EnumerateArray(),
            x => x.GetProperty("cultureCode").GetString() == "en");
        var rules = t.GetProperty("keyConceptsAndRules").GetString()!;
        Assert.Contains("f(g(x))", t.GetProperty("commonMistakes").GetString()!, StringComparison.Ordinal);
        Assert.Contains("different", t.GetProperty("quickSummary").GetString()!, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("g(x)≠0", rules, StringComparison.Ordinal);
    }
}
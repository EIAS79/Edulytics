using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Source-labelled Edulytics-authored extensions for HSF-BF.A.1.c and HSF-BF.B.4.d.
/// No claim is made of publisher-specific child-code attribution or graphical grading.
/// </summary>
public sealed class UsCommonCoreOriginalStandardsExtensionTests
{
    private const string InverseLesson =
        "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U04:L15";
    private const string CombinedLesson =
        "PED:US-CCSS-MATH:HS:TRAD:ALG2:IM-1E:U05:L10";

    [Theory]
    [InlineData(InverseLesson, "us-ccss-math-hs-alg1-phase29-v1.lesson-content-pack.json",
        "HSF-BF.B.4.d", "q(1)=q(5)", "x≥3", "q⁻¹(16)=3+4=7")]
    [InlineData(CombinedLesson, "us-ccss-math-hs-alg2-phase29-v1.lesson-content-pack.json",
        "HSF-BF.A.1.c", "f(g(x))=6x−7", "T(h(t))", "13.94°C")]
    public void OriginalExtensionIsExplicitlyLabelledAndHasVerifiedBodyDigest(
        string lessonCode, string packFile, string clause, string fact1,
        string fact2, string fact3)
    {
        var assembly = typeof(Edulytics.Core.Curriculum.MathematicsCurriculumPackRegistry)
            .Assembly;
        var resource = Assert.Single(assembly.GetManifestResourceNames(),
            x => x.EndsWith(packFile, StringComparison.OrdinalIgnoreCase));
        using var stream = assembly.GetManifestResourceStream(resource)!;
        using var document = JsonDocument.Parse(stream);
        var lesson = Assert.Single(
            document.RootElement.GetProperty("lessons").EnumerateArray(),
            x => x.GetProperty("lessonCode").GetString() == lessonCode);
        var english = Assert.Single(
            lesson.GetProperty("translations").EnumerateArray(),
            x => x.GetProperty("cultureCode").GetString() == "en");
        var keys = new[] { "title", "explanation", "keyConceptsAndRules", "workedExamples",
            "stepByStepSolutions", "commonMistakes", "quickSummary" };
        var original = string.Join("\n", keys.Select(x =>
            x + ":" + english.GetProperty(x).GetString()));
        var digest = Convert.ToHexString(
            SHA256.HashData(Encoding.UTF8.GetBytes(original))).ToLowerInvariant();
        Assert.Equal(digest, lesson.GetProperty("canonicalBodySha256").GetString());
        Assert.Matches("^[0-9a-f]{64}$", lesson.GetProperty("sourceSha256").GetString());
        Assert.Contains("Edulytics original standards extension", original);
        Assert.Contains("not attributed to the source publisher", original);
        Assert.Contains(clause, original);
        Assert.Contains(fact1, original);
        Assert.Contains(fact2, original);
        Assert.Contains(fact3, original);
    }

    [Fact]
    public void AuthoredExtensionsAreNowRoutedWithExistingCorePracticeFamilies()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(InverseLesson, out var inverse));
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(CombinedLesson, out var combined));
        Assert.Equal(
            new[] { "usccss.hs.inverse_composition", "usccss.hs.inverse_table",
                "usccss.hs.restricted_inverse" }, inverse!.AllowedQuestionFamilies);
        Assert.Equal(
            new[] { "usccss.hs.function_sum", "usccss.hs.function_composition" },
            combined!.AllowedQuestionFamilies);
    }

    [Fact]
    public void ReassignedExactFamiliesCalculateAndVerifyAcrossDifficulties()
    {
        var engine = new ExactSkillContractQuestionEngine();
        var total = 0;
        foreach (var (family, lesson) in new[]
        {
            ("usccss.hs.restricted_inverse", InverseLesson),
            ("usccss.hs.function_composition", CombinedLesson)
        })
        {
            foreach (var difficulty in new[]
            {
                ExactSkillQuestionDifficulty.Standard,
                ExactSkillQuestionDifficulty.Stretch,
                ExactSkillQuestionDifficulty.Challenge
            })
            {
                for (var seed = 0; seed < 12; seed++)
                {
                    var q = Assert.Single(engine.Generate(
                        "us-ccss-authored-extension", lesson, [family],
                        difficulty, 1, 9600000 + total, []));
                    Assert.Equal(family, q.Family);
                    Assert.True(ExactSkillContractQuestionEngine.Verify(
                        q.Family, q.Parameters, q.CorrectAnswer));
                    total++;
                }
            }
        }
        Assert.Equal(2 * 3 * 12, total);
    }

    [Fact]
    public void MathematicalExamplesPassIndependentChecks()
    {
        static int Q(int x) => (x - 3) * (x - 3);
        Assert.Equal(Q(5), Q(1));
        Assert.Equal(16, Q(7));
        Assert.Equal(7.0, 3.0 + Math.Sqrt(16.0));
        static int F(int x) => 2 * x + 1;
        static int G(int x) => 3 * x - 4;
        Assert.Equal(5, F(G(2)));
        Assert.Equal(11, G(F(2)));
        static double H(double minutes) => 1000 + 2 * minutes;
        static double T(double height) => 20 - 0.006 * height;
        Assert.Equal(13.94, Math.Round(T(H(5)), 2));
    }
}
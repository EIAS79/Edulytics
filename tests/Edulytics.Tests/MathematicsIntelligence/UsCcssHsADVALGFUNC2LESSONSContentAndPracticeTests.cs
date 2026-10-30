using System.Text.Json;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Course-scoped, fail-closed source/learner and independent mathematical oracles.
/// These are NOT authenticated runtime, scoring-persistence or production tests.
/// </summary>
public sealed class UsCcssHsADVALGFUNC2LESSONSContentAndPracticeTests
{
    private const string Prefix = "PED:US-CCSS-MATH:HS:FOURTH:ADV-ALG-FUNC:U01:";
    private const string ContentRelativePath =
        "src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-hs-adv-alg-func-phase29-v1.lesson-content-pack.json";
    private const string BlueprintRelativePath =
        "src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/us-ccss-math-hs-fourth-advanced-algebra-functions.lesson-blueprint.json";

    [Fact]
    public void ExactTwoLessonManifestAndLockedSourceEvidenceArePreserved()
    {
        using var blueprint = Read(BlueprintRelativePath);
        using var content = Read(ContentRelativePath);

        var b = blueprint.RootElement;
        var p = content.RootElement;
        Assert.Equal("US-CCSS-MATH:HS:FOURTH:ADV-ALG-FUNC", b.GetProperty("BlueprintCode").GetString());
        Assert.Equal("ADV-ALG-FUNC", b.GetProperty("CourseCode").GetString());
        Assert.Equal("Fourth Courses", b.GetProperty("Pathway").GetString());
        Assert.Equal("CCSSM-2010", b.GetProperty("VersionCode").GetString());
        Assert.Single(b.GetProperty("Units").EnumerateArray());
        Assert.Equal("us-ccss-hs-adv-alg-func-2lesson-teaching-v2",
            p.GetProperty("contentVersion").GetString());

        var expected = new[] { Prefix + "L01", Prefix + "L02" };
        var entries = p.GetProperty("lessons").EnumerateArray().ToArray();
        var blueprintLessons = b.GetProperty("Lessons").EnumerateArray().ToArray();
        Assert.Equal(expected, entries.Select(x => x.GetProperty("lessonCode").GetString()).ToArray());
        Assert.Equal(expected, blueprintLessons.Select(x => x.GetProperty("LessonCode").GetString()).ToArray());
        Assert.Equal(new[] { "CCSS:HSA-REI.C.8", "CCSS:HSA-REI.C.9" },
            entries[0].GetProperty("outcomeCodes").EnumerateArray().Select(x => x.GetString()).ToArray());
        Assert.Equal(new[] { "CCSS:HSF-BF.B.5" },
            entries[1].GetProperty("outcomeCodes").EnumerateArray().Select(x => x.GetString()).ToArray());

        Assert.Equal("87f0872674e13e3f53e4ccc81004cb150eb7a64539879efa9f04f8cd58349e55",
            entries[0].GetProperty("sourceSha256").GetString());
        Assert.Equal("590d11f4e182141dc1ba220a9824243fc7b60c18793ebefa5f628931b3a54db3",
            entries[1].GetProperty("sourceSha256").GetString());
        Assert.Equal("9c30b68f87f48b668c32875483260a44b6600ee69ca46b9ec5b8a0706ed6b6f7",
            entries[0].GetProperty("canonicalBodySha256").GetString());
        Assert.Equal("f26d0b43c2be38cbbc5851564a85ab28fc31c3dbb4c970ca829f03343c1f4ec6",
            entries[1].GetProperty("canonicalBodySha256").GetString());
        Assert.All(entries, x => Assert.Single(x.GetProperty("translations").EnumerateArray()));
    }

    [Fact]
    public void BothLessonsContainExplicitSourceGoalTeachingRepairs()
    {
        using var pack = Read(ContentRelativePath);
        var lessons = pack.RootElement.GetProperty("lessons").EnumerateArray().ToArray();
        var first = lessons[0].GetProperty("translations")[0];
        var second = lessons[1].GetProperty("translations")[0];
        Assert.Contains("det(A)=ad−bc", first.GetProperty("keyConceptsAndRules").GetString()!);
        Assert.Contains("A⁻¹b=[5−3,−5+6]", first.GetProperty("workedExamples").GetString()!);
        Assert.Contains("technology for 3×3", first.GetProperty("workedExamples").GetString()!);
        Assert.Contains("infinitely many solutions", first.GetProperty("workedExamples").GetString()!);
        Assert.Contains("row reduction", first.GetProperty("stepByStepSolutions").GetString()!);
        Assert.Contains("painting-supplies context", first.GetProperty("workedExamples").GetString()!);
        Assert.Contains("2p+r=30, p+4q=20, and 2q+r=10",
            first.GetProperty("workedExamples").GetString()!);
        Assert.Contains("2(12)+6=30, 12+4(2)=20, and 2(2)+6=10",
            first.GetProperty("workedExamples").GetString()!);
        Assert.Contains("negative paint price", first.GetProperty("workedExamples").GetString()!);
        Assert.Contains("zero coefficients", first.GetProperty("stepByStepSolutions").GetString()!);

        Assert.Contains("0<b<1, they reverse order: 0<x<y implies log_b x>log_b y",
            second.GetProperty("keyConceptsAndRules").GetString()!);
        Assert.Contains("log₄3=ln3/ln4", second.GetProperty("workedExamples").GetString()!);
        Assert.Contains("log₂(x−3)=1 requires x>3", second.GetProperty("workedExamples").GetString()!);
        Assert.Contains("log_b(x+y)", second.GetProperty("commonMistakes").GetString()!);
    }

    [Fact]
    public void MatrixInverseOracleIsExactAndChecksOriginalSystem()
    {
        var a = new int[,] { { 1, 1, 1 }, { 2, -1, 1 }, { 1, 2, -1 } };
        var numeratorOfInverse = new int[,] { { -1, 3, 2 }, { 3, -2, 1 }, { 5, -1, -3 } };
        var rhs = new[] { 6, 3, 2 };
        var solution = new[] { 1, 2, 3 };
        for (var row = 0; row < 3; row++)
        {
            for (var column = 0; column < 3; column++)
            {
                var product = 0;
                for (var k = 0; k < 3; k++)
                    product += a[row, k] * numeratorOfInverse[k, column];
                Assert.Equal(row == column ? 7 : 0, product);
            }

            var numeratorResult = 0;
            var originalResult = 0;
            for (var k = 0; k < 3; k++)
            {
                numeratorResult += numeratorOfInverse[row, k] * rhs[k];
                originalResult += a[row, k] * solution[k];
            }
            Assert.Equal(7 * solution[row], numeratorResult);
            Assert.Equal(rhs[row], originalResult);
        }

        var invertible2By2 = new int[,] { { 2, 1 }, { 1, 1 } };
        Assert.Equal(1, invertible2By2[0, 0] * invertible2By2[1, 1] -
                        invertible2By2[0, 1] * invertible2By2[1, 0]);
        Assert.Equal(0, 2 * 2 - 2 * 2); // singular dependent/inconsistent family
    }


    [Fact]
    public void AllSystemsGoPaintingReceiptsHaveVerifiedInverseAndContextualBoundaries()
    {
        // Source-backed order: [paint per gallon, brush, tape roll].
        // Each row is one weekly purchase; absent goods have zero coefficients.
        var a = new int[,] { { 2, 0, 1 }, { 1, 4, 0 }, { 0, 2, 1 } };
        var inverseNumerator = new int[,] { { 4, 2, -4 }, { -1, 2, 1 }, { 2, -4, 8 } };
        var originalTotals = new[] { 30, 20, 10 };
        var prices = new[] { 12, 2, 6 };

        // det(A)=10: the valid 3x3 inverse is adj(A)/10.
        var determinant = a[0, 0] * (a[1, 1] * a[2, 2] - a[1, 2] * a[2, 1])
            - a[0, 1] * (a[1, 0] * a[2, 2] - a[1, 2] * a[2, 0])
            + a[0, 2] * (a[1, 0] * a[2, 1] - a[1, 1] * a[2, 0]);
        Assert.Equal(10, determinant);

        for (var i = 0; i < 3; i++)
        {
            var reconstructedTotal = 0;
            var numeratorPrice = 0;
            for (var j = 0; j < 3; j++)
            {
                reconstructedTotal += a[i, j] * prices[j];
                numeratorPrice += inverseNumerator[i, j] * originalTotals[j];
                var identityProduct = 0;
                for (var k = 0; k < 3; k++)
                    identityProduct += a[i, k] * inverseNumerator[k, j];
                Assert.Equal(i == j ? determinant : 0, identityProduct);
            }
            Assert.Equal(originalTotals[i], reconstructedTotal);
            Assert.Equal(determinant * prices[i], numeratorPrice);
        }
        Assert.All(prices, price => Assert.True(price >= 0));

        // The source receipt's first row has no brushes; a wrong column cannot pass Ax=b.
        var incorrectlyModelledFirstTotal = 2 * prices[0] + prices[1];
        Assert.NotEqual(originalTotals[0], incorrectlyModelledFirstTotal);

        // Hypothetical boundary, NOT a claim about the original source:
        // raising only week-three total to $50 keeps A invertible but gives
        // negative paint: an impossible price interpretation, not an inconsistent system.
        var alteredTotals = new[] { 30, 20, 50 };
        var alteredPrices = new[] { -4, 6, 38 };
        for (var i = 0; i < 3; i++)
        {
            var reconstructed = 0;
            for (var j = 0; j < 3; j++)
                reconstructed += a[i, j] * alteredPrices[j];
            Assert.Equal(alteredTotals[i], reconstructed);
        }
        Assert.True(alteredPrices[0] < 0);
        Assert.NotEqual(0, determinant);
    }

    [Theory]
    [InlineData(2, 32, 5)]
    [InlineData(3, 1.0 / 9.0, -2)]
    [InlineData(0.5, 4, -2)]
    [InlineData(2, 0.125, -3)]
    public void LogarithmIndependentOracleRespectsInversePowers(double b, double x, double y)
    {
        Assert.True(b > 0 && b != 1 && x > 0);
        Assert.InRange(Math.Abs(Math.Log(x) / Math.Log(b) - y), 0, 1e-10);
        Assert.InRange(Math.Abs(Math.Pow(b, y) - x), 0, 1e-10);
    }

    [Fact]
    public void LogComparisonAndRealDomainOracleHandleBoundaryAndReversal()
    {
        Assert.True(Math.Log(3, 2) < Math.Log(5, 2));
        Assert.True(Math.Log(3, 0.5) > Math.Log(5, 0.5));
        Assert.True(Math.Log(3, 4) < Math.Log(3, 2));
        Assert.True(Math.Abs(Math.Log(6, 2) -
            (Math.Log(2, 2) + Math.Log(4, 2))) > 0.1);
        Assert.False(ValidRealLog(1, 2));
        Assert.False(ValidRealLog(0, 2));
        Assert.False(ValidRealLog(-2, 2));
        Assert.False(ValidRealLog(2, 0));
        Assert.False(ValidRealLog(2, -1));
        Assert.True(ValidRealLog(0.5, 4));
    }

    [Fact]
    public void MatrixStandardsMustNotSilentlyUseOneVariablePractice()
    {
        using var practice = Read(
            "src/Edulytics.Core/Mathematics/Curriculum/official-outcome-practice-map.v1.json");
        var entries = practice.RootElement.GetProperty("entries").EnumerateArray().ToArray();
        foreach (var code in new[] { "CCSS:HSA-REI.C.8", "CCSS:HSA-REI.C.9" })
        {
            var entry = Assert.Single(entries, row =>
                row.GetProperty("outcomeCode").GetString() == code);
            var target = entry.GetProperty("targetRuleId").GetString()!;
            // Expected to FAIL on the prior main mapping until the designated
            // shared integration owner fixes this cross-course registry.
            Assert.NotEqual("one-step-formula-equation", target);
            Assert.Contains("matrix", target, StringComparison.OrdinalIgnoreCase);
        }
        var logMap = Assert.Single(entries, row =>
            row.GetProperty("outcomeCode").GetString() == "CCSS:HSF-BF.B.5");
        Assert.Equal("exponential-log", logMap.GetProperty("targetRuleId").GetString());
    }

    private static bool ValidRealLog(double b, double x) =>
        double.IsFinite(b) && double.IsFinite(x) && b > 0 && b != 1 && x > 0;

    private static JsonDocument Read(string relativePath) =>
        JsonDocument.Parse(File.ReadAllText(Path.Combine(FindRoot(),
            relativePath.Replace('/', Path.DirectorySeparatorChar))));

    private static string FindRoot()
    {
        var cursor = new DirectoryInfo(AppContext.BaseDirectory);
        while (cursor is not null)
        {
            if (File.Exists(Path.Combine(cursor.FullName, "Edulytics.sln")))
                return cursor.FullName;
            cursor = cursor.Parent;
        }
        throw new DirectoryNotFoundException("Edulytics.sln root is missing.");
    }
}

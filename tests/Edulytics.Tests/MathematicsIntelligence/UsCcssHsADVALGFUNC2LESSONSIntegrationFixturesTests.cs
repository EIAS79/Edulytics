using System.Text.Json;
using Edulytics.Core.Mathematics.Practice;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Course-specific mathematical fixtures intended for the shared integration owner.
/// Static oracles and registry assertions are NOT substitutes for authenticated
/// student submissions, persisted grading, source signoff or owner UAT.
/// </summary>
public sealed class UsCcssHsADVALGFUNC2LESSONSIntegrationFixturesTests
{
    private const string MatrixLesson = "PED:US-CCSS-MATH:HS:FOURTH:ADV-ALG-FUNC:U01:L01";
    private const string LogLesson = "PED:US-CCSS-MATH:HS:FOURTH:ADV-ALG-FUNC:U01:L02";
    private const string FixturePath =
        "artifacts/curriculum-delivery/US-CCSS-MATH/coder-8/UsCcssHsADVALGFUNC2LESSONS-integration-acceptance-fixtures.json";

    [Fact]
    public void FixtureTracksExactlyTwoLessonIdentitiesAndTwentySixReviewableCases()
    {
        using var document = LoadFixtures();
        var root = document.RootElement;
        Assert.Equal("PROPOSED_EXPECTATIONS_NOT_RUNTIME_VERIFICATION",
            root.GetProperty("status").GetString());

        var course = root.GetProperty("course");
        Assert.Equal("ADV-ALG-FUNC", course.GetProperty("code").GetString());
        Assert.Equal("Fourth Courses", course.GetProperty("pathway").GetString());
        Assert.Equal(new[] { MatrixLesson, LogLesson },
            course.GetProperty("canonicalLessons").EnumerateArray()
                .Select(x => x.GetString()).ToArray());

        var cases = root.GetProperty("cases").EnumerateArray().ToArray();
        Assert.Equal(26, cases.Length);
        Assert.Equal(26, cases.Select(x => x.GetProperty("id").GetString())
            .Distinct(StringComparer.Ordinal).Count());
        Assert.Equal(11, cases.Count(x => x.GetProperty("lessonCode").GetString() == MatrixLesson));
        Assert.Equal(15, cases.Count(x => x.GetProperty("lessonCode").GetString() == LogLesson));

        Assert.All(cases, item =>
        {
            var isMatrix = item.GetProperty("lessonCode").GetString() == MatrixLesson;
            if (isMatrix)
                Assert.True(new[] { "CCSS:HSA-REI.C.8", "CCSS:HSA-REI.C.9" }
                    .Contains(item.GetProperty("officialTarget").GetString()));
            else
                Assert.Equal("CCSS:HSF-BF.B.5", item.GetProperty("officialTarget").GetString());
            Assert.False(string.IsNullOrWhiteSpace(item.GetProperty("expectedCanonical").GetString()));
            Assert.False(string.IsNullOrWhiteSpace(item.GetProperty("acceptedEquivalent").GetString()));
            Assert.False(string.IsNullOrWhiteSpace(item.GetProperty("rejectWrongOrMalformed").GetString()));
        });
        Assert.Contains(cases, x => x.GetProperty("category").GetString() == "singular-dependent");
        Assert.Contains(cases, x => x.GetProperty("category").GetString() == "singular-inconsistent");
        Assert.Contains(cases, x => x.GetProperty("category").GetString() == "log-decreasing");
        Assert.Contains(cases, x => x.GetProperty("category").GetString() == "invalid-input-negative");
        Assert.Contains(cases, x => x.GetProperty("category").GetString() == "log-product-justification");
    }

    [Fact]
    public void SourceGoalCasesRequireReviewedHumanRubricsRatherThanFakeNumericScoring()
    {
        using var d = LoadFixtures();
        var cases = d.RootElement.GetProperty("cases").EnumerateArray().ToArray();
        foreach (var id in new[] { "M11", "L13" })
        {
            var row = Assert.Single(cases, x => x.GetProperty("id").GetString() == id);
            Assert.Equal("HUMAN_RUBRIC", row.GetProperty("gradingPolicy").GetString());
        }
        var invalid = cases.Where(x => x.GetProperty("gradingPolicy").GetString()
            == "REJECT_INVALID_INPUT").ToArray();
        Assert.True(invalid.Length >= 5);
        Assert.Contains(cases, x => x.GetProperty("category").GetString() == "dimension-error");
        Assert.Contains(cases, x => x.GetProperty("category").GetString() == "invalid-base-one");
    }


    [Fact]
    public void OpenResponseRubricsMustRemainHumanScoredAndTiedToFixtures()
    {
        using var fixtureDocument = LoadFixtures();
        var fixtureCases = fixtureDocument.RootElement.GetProperty("cases")
            .EnumerateArray().ToDictionary(x => x.GetProperty("id").GetString()!);
        const string rubricPath =
            "artifacts/curriculum-delivery/US-CCSS-MATH/coder-8/UsCcssHsADVALGFUNC2LESSONS-human-scoring-rubrics.v1.json";
        using var rubricDocument = JsonDocument.Parse(File.ReadAllText(
            Path.Combine(FindRoot(), rubricPath.Replace('/', Path.DirectorySeparatorChar))));
        var root = rubricDocument.RootElement;
        Assert.Equal("PROPOSED_HUMAN_SCORING_NOT_RUNTIME_CERTIFIED",
            root.GetProperty("status").GetString());

        var rubrics = root.GetProperty("rubrics").EnumerateArray().ToArray();
        Assert.Equal(new[] { "L04", "L05", "L06", "L13", "M05", "M11" },
            rubrics.Select(x => x.GetProperty("caseId").GetString()).ToArray());
        foreach (var rubric in rubrics)
        {
            var id = rubric.GetProperty("caseId").GetString()!;
            Assert.True(fixtureCases.TryGetValue(id, out var fixtureCase));
            Assert.Equal(fixtureCase.GetProperty("lessonCode").GetString(),
                rubric.GetProperty("lessonCode").GetString());
            Assert.Equal(fixtureCase.GetProperty("gradingPolicy").GetString(),
                rubric.GetProperty("gradingPolicy").GetString());
            Assert.True(rubric.GetProperty("requiresHumanReview").GetBoolean());
            Assert.False(rubric.GetProperty("machineAutoGrade").GetBoolean());
            Assert.Equal(4, rubric.GetProperty("maxPoints").GetInt32());
            if (new[] { "L04", "L05", "L06" }.Contains(id))
                Assert.True(fixtureCase.GetProperty("explanationRequired").GetBoolean());
            var criteria = rubric.GetProperty("criteria").EnumerateArray().ToArray();
            Assert.Equal(4, criteria.Length);
            Assert.Equal(4, criteria.Sum(x => x.GetProperty("points").GetInt32()));
            Assert.All(criteria, criterion =>
            {
                Assert.Equal(1, criterion.GetProperty("points").GetInt32());
                Assert.False(string.IsNullOrWhiteSpace(
                    criterion.GetProperty("requirement").GetString()));
            });
            Assert.False(string.IsNullOrWhiteSpace(
                rubric.GetProperty("acceptedReasoning").GetString()));
            Assert.False(string.IsNullOrWhiteSpace(
                rubric.GetProperty("invalidExamples").GetString()));
        }

        var gates = root.GetProperty("releaseGates");
        foreach (var gate in new[] { "academicApproved", "teacherWorkflowVerified",
                     "twoSchoolIsolationVerified", "actualPersistedTeacherScoreVerified",
                     "productionReady" })
            Assert.False(gates.GetProperty(gate).GetBoolean());
    }


    [Theory]
    [InlineData("M01")]
    [InlineData("M02")]
    [InlineData("M03")]
    [InlineData("M04")]
    [InlineData("M05")]
    [InlineData("M06")]
    [InlineData("M07")]
    [InlineData("M08")]
    [InlineData("M09")]
    [InlineData("M10")]
    [InlineData("M11")]
    [InlineData("L01")]
    [InlineData("L02")]
    [InlineData("L03")]
    [InlineData("L04")]
    [InlineData("L05")]
    [InlineData("L06")]
    [InlineData("L07")]
    [InlineData("L08")]
    [InlineData("L09")]
    [InlineData("L10")]
    [InlineData("L11")]
    [InlineData("L12")]
    [InlineData("L13")]
    [InlineData("L14")]
    [InlineData("L15")]
    public void EveryFixtureHasSeparateWrongMalformedBoundaryAndEquivalentInputs(string id)
    {
        using var data = LoadFixtures();
        var item = Assert.Single(data.RootElement.GetProperty("cases").EnumerateArray(),
            x => x.GetProperty("id").GetString() == id);
        var canonical = item.GetProperty("expectedCanonical").GetString()!;
        var equivalent = item.GetProperty("acceptedEquivalent").GetString()!;
        var wrong = item.GetProperty("rejectWrongOrMalformed").GetString()!;
        var malformed = item.GetProperty("rejectMalformed").GetString()!;
        var boundary = item.GetProperty("boundaryOracle").GetString()!;
        Assert.False(string.IsNullOrWhiteSpace(canonical));
        Assert.False(string.IsNullOrWhiteSpace(equivalent));
        Assert.False(string.IsNullOrWhiteSpace(wrong));
        Assert.False(string.IsNullOrWhiteSpace(malformed));
        Assert.False(string.IsNullOrWhiteSpace(boundary));
        Assert.NotEqual(canonical, wrong);
        Assert.NotEqual(equivalent, wrong);
        Assert.NotEqual(canonical, malformed);
        Assert.NotEqual(equivalent, malformed);
        Assert.NotEqual(wrong, malformed);

        // A fixture is a design-time grading contract; this test must never
        // assert that an actual student runtime selected or verified the family.
        Assert.Equal("PROPOSED_EXPECTATIONS_NOT_RUNTIME_VERIFICATION",
            data.RootElement.GetProperty("status").GetString());
    }

    [Fact]
    public void CrossBaseLogComparisonCannotClaimUniversalPositiveInputOrdering()
    {
        Assert.True(Math.Log(3, 4) < Math.Log(3, 2));
        Assert.Equal(0, Math.Log(1, 4));
        Assert.Equal(0, Math.Log(1, 2));
        Assert.True(Math.Log(0.5, 4) > Math.Log(0.5, 2));
        // The L06 comparator specifically needs teacher-scored rationale,
        // since the same base-order claim changes sign across x=1.
        using var fixtures = LoadFixtures();
        var item = Assert.Single(fixtures.RootElement.GetProperty("cases").EnumerateArray(),
            x => x.GetProperty("id").GetString() == "L06");
        Assert.Equal("ORDER_RELATION_AND_RUBRIC", item.GetProperty("gradingPolicy").GetString());
        Assert.True(item.GetProperty("explanationRequired").GetBoolean());
        // The prompt contains TWO questions. The canonical answer must
        // give both the exact-base relation and the three domain regions;
        // otherwise a grader could accept a correct symbol-only first part.
        var expected = item.GetProperty("expectedCanonical").GetString()!;
        var equivalent = item.GetProperty("acceptedEquivalent").GetString()!;
        foreach (var answer in new[] { expected, equivalent })
        {
            Assert.Contains("log_4(3)", answer, StringComparison.Ordinal);
            Assert.Contains("log_2(3)", answer, StringComparison.Ordinal);
            Assert.Contains("x=1", answer, StringComparison.Ordinal);
            Assert.Contains("0<x<1", answer, StringComparison.Ordinal);
            Assert.Contains("x>1", answer, StringComparison.Ordinal);
        }
        Assert.Contains("false", expected, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void ExactMatrixOraclesCoverInvertibleSingularAndWrongDimensionCases()
    {
        var a2 = new int[,] { { 2, 1 }, { 1, 1 } };
        var inverse2 = new int[,] { { 1, -1 }, { -1, 2 } };
        Assert.Equal(1, Determinant2(a2));
        Assert.Equal(new[] { 2, 1 }, Multiply(inverse2, new[] { 5, 3 }));
        Assert.Equal(new[] { 5, 3 }, Multiply(a2, new[] { 2, 1 }));
        Assert.Equal(new[] { 1, 0 }, Multiply(a2, new[] { 1, -1 }));
        Assert.Equal(new[] { 0, 1 }, Multiply(a2, new[] { -1, 2 }));

        var a3 = new int[,] { { 1, 1, 1 }, { 2, -1, 1 }, { 1, 2, -1 } };
        var numerator3 = new int[,] { { -1, 3, 2 }, { 3, -2, 1 }, { 5, -1, -3 } };
        Assert.Equal(7, Determinant3(a3));
        Assert.Equal(new[] { 7, 14, 21 }, Multiply(numerator3, new[] { 6, 3, 2 }));
        Assert.Equal(new[] { 6, 3, 2 }, Multiply(a3, new[] { 1, 2, 3 }));
        for (var column = 0; column < 3; column++)
        {
            var unit = Enumerable.Range(0, 3).Select(i => i == column ? 1 : 0).ToArray();
            var numeratorColumn = Multiply(numerator3, unit);
            Assert.Equal(unit.Select(x => x * 7).ToArray(),
                Multiply(a3, numeratorColumn));
        }

        var singular = new int[,] { { 1, 1 }, { 2, 2 } };
        Assert.Equal(0, Determinant2(singular));
        // Exact augmented-row elimination separates dependent from contradictory systems.
        Assert.Equal(new[] { 0, 0, 0 },
            new[] { 2 - 2 * 1, 2 - 2 * 1, 4 - 2 * 2 });
        Assert.Equal(new[] { 0, 0, 1 },
            new[] { 2 - 2 * 1, 2 - 2 * 1, 5 - 2 * 2 });
        Assert.Throws<ArgumentException>(() => Multiply(a2, new[] { 5, 3, 1 }));
    }

    [Theory]
    [InlineData(2.0, 32.0, 5.0)]
    [InlineData(3.0, 0.1111111111111111, -2.0)]
    [InlineData(0.5, 4.0, -2.0)]
    [InlineData(2.0, 0.125, -3.0)]
    public void LogFixtureInverseOraclesAcceptPositiveDomains(double b, double x, double expected)
    {
        Assert.True(ValidRealLog(b, x));
        Assert.InRange(Math.Abs(Math.Log(x) / Math.Log(b) - expected), 0, 1e-10);
        Assert.InRange(Math.Abs(Math.Pow(b, expected) - x), 0, 1e-10);
    }

    [Fact]
    public void LogFixturesRejectInvalidInputsAndRespectMonotonicity()
    {
        foreach (var (b, x) in new[] { (1d, 5d), (0d, 3d), (-2d, 8d), (2d, 0d), (2d, -2d) })
            Assert.False(ValidRealLog(b, x));
        Assert.True(Math.Log(3, 2) < Math.Log(5, 2));
        Assert.True(Math.Log(3, 0.5) > Math.Log(5, 0.5));
        Assert.True(Math.Log(3, 4) < Math.Log(3, 2));
        Assert.InRange(Math.Abs(Math.Log(7) / Math.Log(2) - 2.807354922057604),
            0, 1e-11);
        Assert.True(Math.Abs(Math.Log(6, 2)
            - (Math.Log(2, 2) + Math.Log(4, 2))) > 0.1);
    }

    [Fact]
    public void LessonPracticeRegistryMustExposeActualMatrixAndLogCompetencies()
    {
        // Deliberately FAILS on the old shared official outcome Practice mapping.
        // Do not relax readiness or replace matrix systems with one-variable equations.
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(MatrixLesson, out var matrix));
        Assert.NotNull(matrix);
        Assert.Equal("READY_VERIFIED", matrix.Readiness);
        Assert.NotEmpty(matrix.AllowedQuestionFamilies);
        Assert.All(matrix.AllowedQuestionFamilies, family =>
            Assert.DoesNotContain("algebra.linear.ax_plus_b_equals_c", family, StringComparison.Ordinal));
        Assert.Contains(matrix.AllowedQuestionFamilies,
            family => family.Contains("matri", StringComparison.OrdinalIgnoreCase)
                   || family.Contains("matrix", StringComparison.OrdinalIgnoreCase));

        Assert.True(LessonPracticeCapabilityResolver.TryResolve(LogLesson, out var logs));
        Assert.NotNull(logs);
        Assert.Equal("READY_VERIFIED", logs.Readiness);
        Assert.Contains(logs.AllowedQuestionFamilies,
            family => family.Contains("logarith", StringComparison.OrdinalIgnoreCase));
    }

    private static int Determinant2(int[,] a) =>
        a[0, 0] * a[1, 1] - a[0, 1] * a[1, 0];

    private static int Determinant3(int[,] a) =>
        a[0, 0] * (a[1, 1] * a[2, 2] - a[1, 2] * a[2, 1])
        - a[0, 1] * (a[1, 0] * a[2, 2] - a[1, 2] * a[2, 0])
        + a[0, 2] * (a[1, 0] * a[2, 1] - a[1, 1] * a[2, 0]);

    private static int[] Multiply(int[,] a, int[] vector)
    {
        if (a.GetLength(1) != vector.Length)
            throw new ArgumentException("Matrix/vector dimensions must agree.", nameof(vector));
        return Enumerable.Range(0, a.GetLength(0))
            .Select(row => Enumerable.Range(0, a.GetLength(1))
                .Sum(col => a[row, col] * vector[col])).ToArray();
    }

    private static bool ValidRealLog(double b, double x) =>
        double.IsFinite(b) && double.IsFinite(x) && b > 0 && b != 1 && x > 0;

    private static JsonDocument LoadFixtures() =>
        JsonDocument.Parse(File.ReadAllText(Path.Combine(FindRoot(),
            FixturePath.Replace('/', Path.DirectorySeparatorChar))));

    private static string FindRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null)
        {
            if (File.Exists(Path.Combine(directory.FullName, "Edulytics.sln")))
                return directory.FullName;
            directory = directory.Parent;
        }
        throw new DirectoryNotFoundException("Edulytics solution root not found.");
    }
}

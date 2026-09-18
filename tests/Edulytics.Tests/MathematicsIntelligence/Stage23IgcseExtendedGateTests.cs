using System.Text.Json;
using Edulytics.Core.Mathematics.Planning;
using Edulytics.Core.Mathematics.Solving;
using Edulytics.Services.Mathematics.Contracts;
using Edulytics.Services.Mathematics.Difficulty;
using Edulytics.Services.Mathematics.Generation;
using Edulytics.Services.Mathematics.Planning;
using Edulytics.Services.Mathematics.Solving;
using Edulytics.Services.Mathematics.Verification;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class Stage23IgcseExtendedGateTests
{
    [Fact]
    public void GateInventoriesAllExtendedSupportingLessonsAndBlocksGlobalClaim()
    {
        var root = FindRoot();
        using var manifest = JsonDocument.Parse(File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Core/Mathematics/Curriculum/stage23-igcse-extended-gate-manifest.v1.json")));

        var lessons = manifest.RootElement.GetProperty("lessons").EnumerateArray().ToArray();
        Assert.Equal(140, lessons.Length);
        Assert.Equal(140, lessons.Select(x => x.GetProperty("lessonCode").GetString()).Distinct().Count());

        Assert.All(lessons, lesson =>
        {
            Assert.False(lesson.GetProperty("formalOutcomeMapped").GetBoolean());
            Assert.Empty(lesson.GetProperty("formalOutcomeCodes").EnumerateArray());
            Assert.Contains(
                lesson.GetProperty("status").GetString(),
                new[] { "CONTEXTUAL", "UNSUPPORTED" });
        });

        var gate = manifest.RootElement.GetProperty("gatePolicy");
        Assert.False(gate.GetProperty("globalIgcseExtendedCapabilityClaimAllowed").GetBoolean());
        Assert.False(gate.GetProperty("productRoutingEnabled").GetBoolean());

        var summary = manifest.RootElement.GetProperty("summary");
        Assert.Equal(0, summary.GetProperty("verified").GetInt32());
        Assert.Equal(28, summary.GetProperty("contextual").GetInt32());
        Assert.Equal(112, summary.GetProperty("unsupported").GetInt32());
        Assert.Equal(0, summary.GetProperty("formalOutcomeMapped").GetInt32());
    }

    [Fact]
    public void EveryContextualMappingReferencesBenchmarkCorpusAndNoTitleOnlyPromotionExists()
    {
        var root = FindRoot();
        using var manifest = JsonDocument.Parse(File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Core/Mathematics/Curriculum/stage23-igcse-extended-gate-manifest.v1.json")));
        using var corpus = JsonDocument.Parse(File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Core/Mathematics/Curriculum/stage23-igcse-extended-benchmark-corpus.v1.json")));

        var benchmarkIds = corpus.RootElement
            .GetProperty("benchmarks")
            .EnumerateArray()
            .Select(x => x.GetProperty("benchmarkId").GetString()!)
            .ToHashSet(StringComparer.Ordinal);

        foreach (var lesson in manifest.RootElement.GetProperty("lessons").EnumerateArray())
        {
            var status = lesson.GetProperty("status").GetString();
            var mappedBenchmarks = lesson.GetProperty("benchmarkIds")
                .EnumerateArray()
                .Select(x => x.GetString()!)
                .ToArray();

            if (status == "CONTEXTUAL")
            {
                Assert.NotEmpty(mappedBenchmarks);
                Assert.Equal("REQUIRED", lesson.GetProperty("academicReviewStatus").GetString());
                Assert.All(mappedBenchmarks, id => Assert.Contains(id, benchmarkIds));
            }
            else
            {
                Assert.Empty(mappedBenchmarks);
            }
        }

        Assert.True(
            manifest.RootElement.GetProperty("gatePolicy")
                .GetProperty("titleSimilarityAloneNeverPromotes")
                .GetBoolean());
    }

    [Fact]
    public void BenchmarkCorpusGeneratesSolvesVerifiesGradesAndCalibratesAllBands()
    {
        var cases = BenchmarkCases();
        Assert.Equal(16, cases.Count);

        var planner = new DeterministicMathematicsStrategyPlanner();
        var difficulty = new MathematicsDifficultyEngine();
        var grader = new MathematicsAnswerEquivalenceV2();

        foreach (var benchmark in cases)
        {
            for (var band = 1; band <= 3; band++)
            {
                var generated = benchmark.Generate(23000 + band * 97, band);

                Assert.Equal(benchmark.SkillId, generated.Skill.Value);
                Assert.Equal(benchmark.FamilyId, generated.QuestionFamilyId);
                Assert.Equal(MathematicsSolveStatus.Solved, generated.SolveResult.Status);
                Assert.True(generated.Verification.IsVerified);
                Assert.Equal(
                    band.ToString(System.Globalization.CultureInfo.InvariantCulture),
                    generated.Parameters["difficultyBand"]);

                var grade = grader.Evaluate(
                    generated.ExpectedAnswer,
                    generated.ExpectedAnswer,
                    new MathematicsAnswerEvaluationPolicy());
                Assert.True(grade.IsEquivalent);

                var plan = planner.Plan(new MathematicsPlanningRequest(
                    generated.Problem,
                    []));
                Assert.Equal(MathematicsPlanningStatus.Planned, plan.Status);

                var calibrated = difficulty.Assess(plan);
                Assert.InRange(calibrated.Score, 0, 200);
                Assert.NotEmpty(calibrated.Reasons);
            }
        }
    }

    [Fact]
    public void AcademicReviewGatePreventsVerifiedPromotionWithoutFormalMapping()
    {
        var root = FindRoot();
        using var manifest = JsonDocument.Parse(File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Core/Mathematics/Curriculum/stage23-igcse-extended-gate-manifest.v1.json")));

        var academic = manifest.RootElement.GetProperty("academicReview");
        Assert.Equal("REQUIRED", academic.GetProperty("currentState").GetString());
        Assert.Equal(0, academic.GetProperty("approvedVerifiedLessonCount").GetInt32());

        Assert.DoesNotContain(
            manifest.RootElement.GetProperty("lessons").EnumerateArray(),
            lesson => string.Equals(
                lesson.GetProperty("status").GetString(),
                "VERIFIED",
                StringComparison.Ordinal));
    }

    private static IReadOnlyList<BenchmarkCase> BenchmarkCases() =>
    [
        new("algebra.linear.solve", ExactLinearEquationQuestionFactory.FamilyId,
            (seed, band) => new ExactLinearEquationQuestionFactory(
                new ExactLinearEquationSolver(),
                new ExactLinearEquationVerifier()).Generate(seed, band)),

        new("algebra.linear.systems.two_by_two.solve", ExactLinearSystem2x2QuestionFactory.FamilyId,
            (seed, band) => new ExactLinearSystem2x2QuestionFactory(
                new ExactLinearSystem2x2Solver(),
                new ExactLinearSystem2x2Verifier()).Generate(seed, band)),

        new("algebra.linear.inequality.solve", ExactLinearInequalityQuestionFactory.FamilyId,
            (seed, band) => new ExactLinearInequalityQuestionFactory(
                new ExactLinearInequalitySolver(),
                new ExactLinearInequalityVerifier()).Generate(seed, band)),

        new("algebra.quadratic.solve.real_roots", ExactQuadraticQuestionFactory.FamilyId,
            (seed, band) => new ExactQuadraticQuestionFactory(
                new ExactQuadraticEquationSolver(),
                new ExactQuadraticEquationVerifier()).Generate(seed, band)),

        new("functions.evaluate.exact", ExactFunctionEvaluationQuestionFactory.FamilyId,
            (seed, band) => new ExactFunctionEvaluationQuestionFactory(
                new ExactFunctionEvaluationSolver(),
                new ExactFunctionEvaluationVerifier()).Generate(seed, band)),

        new("sequences.arithmetic.nth_term", ExactArithmeticSequenceQuestionFactory.FamilyId,
            (seed, band) => new ExactArithmeticSequenceQuestionFactory(
                new ExactSequenceTermSolver(),
                new ExactSequenceTermVerifier()).Generate(seed, band)),

        new("sequences.geometric.nth_term", ExactGeometricSequenceQuestionFactory.FamilyId,
            (seed, band) => new ExactGeometricSequenceQuestionFactory(
                new ExactSequenceTermSolver(),
                new ExactSequenceTermVerifier()).Generate(seed, band)),

        new("geometry.rectangle.area", ExactRectangleAreaQuestionFactory.FamilyId,
            (seed, band) => new ExactRectangleAreaQuestionFactory(
                new ExactRectangleAreaSolver(),
                new ExactRectangleAreaVerifier()).Generate(seed, band)),

        new("geometry.rectangle.perimeter", ExactRectanglePerimeterQuestionFactory.FamilyId,
            (seed, band) => new ExactRectanglePerimeterQuestionFactory(
                new ExactRectanglePerimeterSolver(),
                new ExactRectanglePerimeterVerifier()).Generate(seed, band)),

        new("geometry.triangle.area.base_height", ExactTriangleBaseHeightAreaQuestionFactory.FamilyId,
            (seed, band) => new ExactTriangleBaseHeightAreaQuestionFactory(
                new ExactTriangleBaseHeightAreaSolver(),
                new ExactTriangleBaseHeightAreaVerifier()).Generate(seed, band)),

        new("geometry.right_triangle.pythagorean", ExactPythagoreanQuestionFactory.FamilyId,
            (seed, band) => new ExactPythagoreanQuestionFactory(
                new ExactPythagoreanSolver(),
                new ExactPythagoreanVerifier()).Generate(seed, band)),

        new("trigonometry.special_angles.evaluate", ExactSpecialAngleTrigonometryQuestionFactory.FamilyId,
            (seed, band) => new ExactSpecialAngleTrigonometryQuestionFactory(
                new ExactSpecialAngleTrigonometrySolver(),
                new ExactSpecialAngleTrigonometryVerifier()).Generate(seed, band)),

        new("vectors.add.exact", ExactVectorAddQuestionFactory.FamilyId,
            (seed, band) => new ExactVectorAddQuestionFactory(
                new ExactVectorAddSolver(),
                new ExactVectorAddVerifier()).Generate(seed, band)),

        new("probability.simple.evaluate.exact", ExactSimpleProbabilityQuestionFactory.FamilyId,
            (seed, band) => new ExactSimpleProbabilityQuestionFactory(
                new ExactSimpleProbabilitySolver(),
                new ExactSimpleProbabilityVerifier()).Generate(seed, band)),

        new("statistics.mean.arithmetic.exact", ExactArithmeticMeanQuestionFactory.FamilyId,
            (seed, band) => new ExactArithmeticMeanQuestionFactory(
                new ExactArithmeticMeanSolver(),
                new ExactArithmeticMeanVerifier()).Generate(seed, band)),

        new("statistics.mean.frequency_table.exact", ExactFrequencyMeanQuestionFactory.FamilyId,
            (seed, band) => new ExactFrequencyMeanQuestionFactory(
                new ExactFrequencyMeanSolver(),
                new ExactFrequencyMeanVerifier()).Generate(seed, band))
    ];

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

    private sealed record BenchmarkCase(
        string SkillId,
        string FamilyId,
        Func<int, int, VerifiedGeneratedMathematicsProblem> Generate);
}

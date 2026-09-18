using System.Text.Json;
using Edulytics.Core.Mathematics.Planning;
using Edulytics.Core.Mathematics.Solving;
using Edulytics.Services.Mathematics.Contracts;
using Edulytics.Services.Mathematics.Difficulty;
using Edulytics.Services.Mathematics.Generation;
using Edulytics.Services.Mathematics.Planning;
using Edulytics.Services.Mathematics.Solving;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class Stage25IbAaHlStyleGateTests
{
    [Fact]
    public void GateDefinesAllSixDemandClassesAndFailsClosedForUnsupportedAdvancedClasses()
    {
        var root = FindRoot();
        using var manifest = JsonDocument.Parse(File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Core/Mathematics/Curriculum/stage25-ib-aa-hl-style-gate-manifest.v1.json")));

        var categories = manifest.RootElement.GetProperty("categoryCoverage").EnumerateArray().ToArray();
        Assert.Equal(6, categories.Length);
        Assert.Equal(
            new[] { "Modelling", "Multi-step", "Proof", "Reasoning", "Routine", "Unfamiliar transfer" },
            categories.Select(x => x.GetProperty("category").GetString()).OrderBy(x => x).ToArray());

        var byCategory = categories.ToDictionary(
            x => x.GetProperty("category").GetString()!,
            StringComparer.Ordinal);

        Assert.Equal("UNSUPPORTED", byCategory["Proof"].GetProperty("status").GetString());
        Assert.Equal(0, byCategory["Proof"].GetProperty("benchmarkCount").GetInt32());
        Assert.Equal("BLOCKED", byCategory["Proof"].GetProperty("capabilityClaim").GetString());

        Assert.Equal("UNSUPPORTED", byCategory["Unfamiliar transfer"].GetProperty("status").GetString());
        Assert.Equal(0, byCategory["Unfamiliar transfer"].GetProperty("benchmarkCount").GetInt32());
        Assert.Equal("BLOCKED", byCategory["Unfamiliar transfer"].GetProperty("capabilityClaim").GetString());

        var gate = manifest.RootElement.GetProperty("gatePolicy");
        Assert.False(gate.GetProperty("globalIbAaHlCapabilityClaimAllowed").GetBoolean());
        Assert.False(gate.GetProperty("productRoutingEnabled").GetBoolean());
        Assert.False(gate.GetProperty("officialIbCurriculumMappingPresent").GetBoolean());
        Assert.True(gate.GetProperty("categoryEvidenceDoesNotEqualCurriculumApproval").GetBoolean());
        Assert.True(gate.GetProperty("unsupportedCategoriesMustFailClosed").GetBoolean());
    }

    [Fact]
    public void ExecutableEngineEvidenceRunsFullChainAcrossAllDifficultyBands()
    {
        var root = FindRoot();
        using var corpus = JsonDocument.Parse(File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Core/Mathematics/Curriculum/stage25-ib-aa-hl-style-benchmark-corpus.v1.json")));

        var corpusRows = corpus.RootElement.GetProperty("benchmarks").EnumerateArray().ToArray();
        var cases = BenchmarkCases().ToDictionary(x => x.BenchmarkId, StringComparer.Ordinal);

        Assert.Equal(15, corpusRows.Length);
        Assert.Equal(15, cases.Count);

        var planner = new DeterministicMathematicsStrategyPlanner();
        var difficulty = new MathematicsDifficultyEngine();
        var grader = new MathematicsAnswerEquivalenceV2();

        foreach (var row in corpusRows)
        {
            var id = row.GetProperty("benchmarkId").GetString()!;
            Assert.True(cases.TryGetValue(id, out var benchmark), $"Missing executable Stage 25 benchmark {id}.");

            Assert.Equal("ENGINE_EVIDENCE_ONLY", row.GetProperty("evidenceLevel").GetString());
            Assert.False(row.GetProperty("officialIbMapping").GetBoolean());

            for (var band = 1; band <= 3; band++)
            {
                var generated = benchmark!.Generate(25000 + band * 149, band);

                Assert.Equal(row.GetProperty("skillId").GetString(), generated.Skill.Value);
                Assert.Equal(row.GetProperty("questionFamilyId").GetString(), generated.QuestionFamilyId);
                Assert.Equal(MathematicsSolveStatus.Solved, generated.SolveResult.Status);
                Assert.True(generated.Verification.IsVerified);

                var grade = grader.Evaluate(
                    generated.ExpectedAnswer,
                    generated.ExpectedAnswer,
                    new MathematicsAnswerEvaluationPolicy());
                Assert.True(grade.IsEquivalent);

                var plan = planner.Plan(new MathematicsPlanningRequest(generated.Problem, []));
                Assert.Equal(MathematicsPlanningStatus.Planned, plan.Status);

                var calibrated = difficulty.Assess(plan);
                Assert.InRange(calibrated.ComplexityScore, 0, 200);
                Assert.NotEmpty(calibrated.Reasons);
            }
        }
    }

    [Fact]
    public void ReasoningEvidenceRemainsNarrowAndDoesNotPromoteProof()
    {
        var root = FindRoot();
        using var manifest = JsonDocument.Parse(File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Core/Mathematics/Curriculum/stage25-ib-aa-hl-style-gate-manifest.v1.json")));

        var categories = manifest.RootElement.GetProperty("categoryCoverage").EnumerateArray().ToArray();
        var reasoning = categories.Single(x => x.GetProperty("category").GetString() == "Reasoning");
        var proof = categories.Single(x => x.GetProperty("category").GetString() == "Proof");

        Assert.Equal("EVIDENCED", reasoning.GetProperty("status").GetString());
        Assert.Equal(2, reasoning.GetProperty("benchmarkCount").GetInt32());
        Assert.Contains("does not establish general mathematical argument or proof capability",
            reasoning.GetProperty("limitation").GetString()!, StringComparison.Ordinal);

        Assert.Equal("UNSUPPORTED", proof.GetProperty("status").GetString());
    }

    [Fact]
    public void OfficialIbClaimsRemainBlockedWithoutFormalMappingAndAcademicApproval()
    {
        var root = FindRoot();
        using var manifest = JsonDocument.Parse(File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Core/Mathematics/Curriculum/stage25-ib-aa-hl-style-gate-manifest.v1.json")));

        var summary = manifest.RootElement.GetProperty("summary");
        Assert.Equal(0, summary.GetProperty("formalIbMappings").GetInt32());
        Assert.Equal(4, summary.GetProperty("evidencedCategories").GetInt32());
        Assert.Equal(2, summary.GetProperty("unsupportedCategories").GetInt32());

        var academic = manifest.RootElement.GetProperty("academicReview");
        Assert.Equal("REQUIRED", academic.GetProperty("currentState").GetString());
        Assert.Equal(0, academic.GetProperty("approvedVerifiedCategoryCount").GetInt32());

        var gate = manifest.RootElement.GetProperty("gatePolicy");
        Assert.False(gate.GetProperty("globalIbAaHlCapabilityClaimAllowed").GetBoolean());
        Assert.False(gate.GetProperty("productRoutingEnabled").GetBoolean());
    }

    private static IReadOnlyList<BenchmarkCase> BenchmarkCases()
    {
        var mechanicsSolver = new ExactMechanicsModelSolver();
        var mechanicsVerifier = new ExactMechanicsModelVerifier();

        return
        [
            new("ib-routine-quadratic", (s,b) => new ExactQuadraticQuestionFactory(new ExactQuadraticEquationSolver(), new ExactQuadraticEquationVerifier()).Generate(s,b)),
            new("ib-routine-function-eval", (s,b) => new ExactFunctionEvaluationQuestionFactory(new ExactFunctionEvaluationSolver(), new ExactFunctionEvaluationVerifier()).Generate(s,b)),
            new("ib-routine-vector-dot", (s,b) => new ExactVectorDotProductQuestionFactory(new ExactVectorDotProductSolver(), new ExactVectorDotProductVerifier()).Generate(s,b)),
            new("ib-routine-frequency-mean", (s,b) => new ExactFrequencyMeanQuestionFactory(new ExactFrequencyMeanSolver(), new ExactFrequencyMeanVerifier()).Generate(s,b)),
            new("ib-routine-arithmetic-sequence", (s,b) => new ExactArithmeticSequenceQuestionFactory(new ExactSequenceTermSolver(), new ExactSequenceTermVerifier()).Generate(s,b)),
            new("ib-multistep-derivative", (s,b) => new ExactPolynomialDerivativeQuestionFactory(new ExactPolynomialDerivativeSolver(), new ExactPolynomialDerivativeVerifier()).Generate(s,b)),
            new("ib-multistep-integral", (s,b) => new ExactPolynomialDefiniteIntegralQuestionFactory(new ExactPolynomialDefiniteIntegralSolver(), new ExactPolynomialDefiniteIntegralVerifier()).Generate(s,b)),
            new("ib-multistep-bisection", (s,b) => new ExactBisectionIterationQuestionFactory(new ExactBisectionIterationSolver(), new ExactBisectionIterationVerifier()).Generate(s,b)),
            new("ib-multistep-newton", (s,b) => new ExactNewtonIterationQuestionFactory(new ExactNewtonIterationSolver(), new ExactNewtonIterationVerifier()).Generate(s,b)),
            new("ib-modelling-velocity", (s,b) => new ExactMechanicsQuestionFactory(mechanicsSolver, mechanicsVerifier).Generate(ExactMechanicsQuestionFactory.VelocityFamily,s,b)),
            new("ib-modelling-displacement", (s,b) => new ExactMechanicsQuestionFactory(mechanicsSolver, mechanicsVerifier).Generate(ExactMechanicsQuestionFactory.DisplacementFamily,s,b)),
            new("ib-modelling-work-energy", (s,b) => new ExactMechanicsQuestionFactory(mechanicsSolver, mechanicsVerifier).Generate(ExactMechanicsQuestionFactory.WorkEnergyFamily,s,b)),
            new("ib-modelling-power", (s,b) => new ExactMechanicsQuestionFactory(mechanicsSolver, mechanicsVerifier).Generate(ExactMechanicsQuestionFactory.PowerFamily,s,b)),
            new("ib-reasoning-arithmetic-sequence", (s,b) => new ExactArithmeticSequenceQuestionFactory(new ExactSequenceTermSolver(), new ExactSequenceTermVerifier()).Generate(s,b)),
            new("ib-reasoning-geometric-sequence", (s,b) => new ExactGeometricSequenceQuestionFactory(new ExactSequenceTermSolver(), new ExactSequenceTermVerifier()).Generate(s,b))
        ];
    }

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
        string BenchmarkId,
        Func<int,int,VerifiedGeneratedMathematicsProblem> Generate);
}

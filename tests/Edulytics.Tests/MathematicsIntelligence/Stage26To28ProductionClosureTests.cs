using System.Numerics;
using System.Text.Json;
using Edulytics.Core.Mathematics.Ast;
using Edulytics.Core.Mathematics.Runtime;
using Edulytics.Services.Mathematics;
using Edulytics.Services.Mathematics.Rollout;
using Edulytics.Services.Mathematics.Runtime;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class Stage26To28ProductionClosureTests
{
    [Fact]
    public void ProductionResourceDefaultsCoverEveryRequiredSafetyBudget()
    {
        var limits = MathematicsResourceLimits.ProductionDefaults;

        Assert.Equal(4096, limits.MaxInputLength);
        Assert.Equal(512, limits.MaxTokenCount);
        Assert.Equal(64, limits.MaxAstDepth);
        Assert.Equal(4096, limits.MaxAstNodes);
        Assert.Equal(12, limits.MaxPolynomialDegree);
        Assert.Equal(12, limits.MaxMatrixDimension);
        Assert.Equal(TimeSpan.FromSeconds(2), limits.SolverTimeout);
        Assert.Equal(8L * 1024L * 1024L, limits.MaxEstimatedMemoryBytes);
        Assert.Equal(8, limits.MaxConcurrentOperations);
    }

    [Fact]
    public void ResourceGuardFailsClosedForInputDepthDegreeAndMatrixOverBudget()
    {
        var limits = MathematicsResourceLimits.ProductionDefaults;

        Assert.Throws<MathematicsResourceLimitException>(() =>
            MathematicsResourceGuard.ValidateInputText(new string('x', limits.MaxInputLength + 1)));

        MathNode deep = new SymbolNode("x");
        for (var i = 0; i <= limits.MaxAstDepth; i++)
            deep = new NegateNode(deep);

        Assert.Throws<MathematicsResourceLimitException>(() =>
            MathematicsResourceGuard.AnalyzeAst(deep));

        var highDegree = new PowerNode(
            new SymbolNode("x"),
            new IntegerNode(new BigInteger(limits.MaxPolynomialDegree + 1)));

        Assert.Throws<MathematicsResourceLimitException>(() =>
            MathematicsResourceGuard.AnalyzeAst(highDegree));

        var oversizedMatrix = new MatrixNode(
            Enumerable.Range(0, limits.MaxMatrixDimension + 1)
                .Select(_ => (IReadOnlyList<MathNode>)[new IntegerNode(BigInteger.One)])
                .ToArray());

        Assert.Throws<MathematicsResourceLimitException>(() =>
            MathematicsResourceGuard.AnalyzeAst(oversizedMatrix));
    }

    [Fact]
    public async Task ExecutionBudgetEnforcesConcurrencyAndTimeout()
    {
        var limits = MathematicsResourceLimits.ProductionDefaults with
        {
            MaxConcurrentOperations = 1,
            SolverTimeout = TimeSpan.FromMilliseconds(500)
        };
        var budget = new MathematicsExecutionBudget(limits);

        var active = 0;
        var maximum = 0;
        var sync = new object();

        async Task<int> Work(CancellationToken token)
        {
            var now = Interlocked.Increment(ref active);
            lock (sync)
                maximum = Math.Max(maximum, now);

            try
            {
                await Task.Delay(40, token);
                return 1;
            }
            finally
            {
                Interlocked.Decrement(ref active);
            }
        }

        await Task.WhenAll(
            budget.RunAsync(Work),
            budget.RunAsync(Work),
            budget.RunAsync(Work));

        Assert.Equal(1, maximum);

        var timeoutBudget = new MathematicsExecutionBudget(
            limits with { SolverTimeout = TimeSpan.FromMilliseconds(50) });

        await Assert.ThrowsAsync<TimeoutException>(() =>
            timeoutBudget.RunAsync<int>(async token =>
            {
                await Task.Delay(TimeSpan.FromSeconds(2), token);
                return 1;
            }));
    }

    [Fact]
    public void ObservabilityExposesEveryRequiredMetricAndExactKernelEmitsSuccess()
    {
        var expected = new[]
        {
            "edulytics.math.generation.success",
            "edulytics.math.solver.success",
            "edulytics.math.verification.failure",
            "edulytics.math.alignment.rejection",
            "edulytics.math.fallback",
            "edulytics.math.unsupported",
            "edulytics.math.timeout",
            "edulytics.math.answer_equivalence.disagreement",
            "edulytics.math.content_mapping.conflict"
        };

        var snapshot = MathematicsObservability.Snapshot();
        Assert.Equal(expected.OrderBy(x => x), snapshot.Keys.OrderBy(x => x));

        var beforeGeneration = MathematicsObservability.Current(MathematicsMetricKind.GenerationSuccess);
        var beforeSolver = MathematicsObservability.Current(MathematicsMetricKind.SolverSuccess);

        var items = new ExactSkillContractQuestionEngine().Generate(
            "stage28-test",
            "PED:CAMBRIDGE-INTL-MATH:S6:6F-3:BUILD",
            ["fractions.compare.unlike.common_denominator"],
            ExactSkillQuestionDifficulty.Standard,
            1,
            28001,
            []);

        Assert.Single(items);
        Assert.True(
            MathematicsObservability.Current(MathematicsMetricKind.GenerationSuccess) >
            beforeGeneration);
        Assert.True(
            MathematicsObservability.Current(MathematicsMetricKind.SolverSuccess) >
            beforeSolver);
    }

    [Fact]
    public void RolloutPolicySupportsFiveModesAndRequiresScopedExplicitPromotion()
    {
        Assert.Equal(
            new[] { "LegacyOnly", "ShadowV2", "V2VerifiedOnly", "V2Preferred", "V2Only" },
            Enum.GetNames<MathematicsRolloutMode>());

        var scope = new MathematicsRolloutScope(
            "fractions",
            "fractions.equivalent",
            "US-CCSS-MATH",
            4);

        var unscopedV2Only = MathematicsProductionRolloutPolicy.FromValues(
            "V2Only", null, null, null, null, legacyGrade16Enabled: false);

        var blockedGlobal = MathematicsProductionRolloutPolicy.Evaluate(
            scope,
            unscopedV2Only,
            isReadyVerified: true,
            hasProductionCapability: true);

        Assert.Equal(MathematicsRolloutRoute.Blocked, blockedGlobal.Route);

        var scopedVerified = MathematicsProductionRolloutPolicy.FromValues(
            "V2VerifiedOnly",
            "fractions",
            "fractions.equivalent",
            "US-CCSS-MATH",
            "4",
            legacyGrade16Enabled: false);

        Assert.Equal(
            MathematicsRolloutRoute.V2,
            MathematicsProductionRolloutPolicy.Evaluate(
                scope,
                scopedVerified,
                isReadyVerified: true,
                hasProductionCapability: true).Route);

        Assert.Equal(
            MathematicsRolloutRoute.Legacy,
            MathematicsProductionRolloutPolicy.Evaluate(
                scope with { Domain = "geometry" },
                scopedVerified,
                isReadyVerified: true,
                hasProductionCapability: true).Route);

        var shadow = MathematicsProductionRolloutPolicy.FromValues(
            "ShadowV2", "fractions", null, null, null, legacyGrade16Enabled: false);

        Assert.Equal(
            MathematicsRolloutRoute.ShadowV2,
            MathematicsProductionRolloutPolicy.Evaluate(
                scope,
                shadow,
                isReadyVerified: false,
                hasProductionCapability: true).Route);

        var scopedV2Only = MathematicsProductionRolloutPolicy.FromValues(
            "V2Only", "fractions", null, null, null, legacyGrade16Enabled: false);

        Assert.Equal(
            MathematicsRolloutRoute.Blocked,
            MathematicsProductionRolloutPolicy.Evaluate(
                scope,
                scopedV2Only,
                isReadyVerified: false,
                hasProductionCapability: false).Route);
    }

    [Fact]
    public void LegacyGrade16FlagRemainsBackwardCompatibleAndFailClosed()
    {
        var enabled = MathematicsProductionRolloutPolicy.FromValues(
            null, null, null, null, null, legacyGrade16Enabled: true);
        var disabled = MathematicsProductionRolloutPolicy.FromValues(
            null, null, null, null, null, legacyGrade16Enabled: false);

        Assert.False(enabled.ExplicitMode);
        Assert.Equal(MathematicsRolloutMode.V2VerifiedOnly, enabled.Mode);
        Assert.Equal(MathematicsRolloutMode.LegacyOnly, disabled.Mode);

        var invalidExplicit = MathematicsProductionRolloutPolicy.FromValues(
            "not-a-mode", "fractions", null, null, null, legacyGrade16Enabled: true);
        Assert.Equal(MathematicsRolloutMode.LegacyOnly, invalidExplicit.Mode);
    }

    [Fact]
    public void ClosureManifestKeepsAdvancedUnsupportedClaimsBlocked()
    {
        var root = FindRoot();
        using var manifest = JsonDocument.Parse(File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Core/Mathematics/Curriculum/stage26-28-production-rollout-manifest.v1.json")));

        var rollout = manifest.RootElement.GetProperty("productionRollout");
        Assert.False(rollout.GetProperty("globalV2OnlyEnabled").GetBoolean());
        Assert.False(rollout.GetProperty("advancedShadowVerifiedAutoPromotion").GetBoolean());
        Assert.True(rollout.GetProperty("explicitNonLegacyModeRequiresSelector").GetBoolean());
        Assert.True(rollout.GetProperty("v2OnlyBlocksUnsupportedInsteadOfFallingBack").GetBoolean());

        var boundary = manifest.RootElement.GetProperty("truthfulCapabilityBoundary");
        Assert.Equal("UNSUPPORTED", boundary.GetProperty("proof").GetString());
        Assert.Equal("UNSUPPORTED", boundary.GetProperty("unfamiliarTransfer").GetString());
        Assert.True(boundary.GetProperty("unsupportedCasesBlocked").GetBoolean());
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
}

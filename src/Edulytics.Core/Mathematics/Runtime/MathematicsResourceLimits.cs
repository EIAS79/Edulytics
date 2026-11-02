namespace Edulytics.Core.Mathematics.Runtime;

/// <summary>
/// Bounded production defaults for Mathematics Intelligence execution.
/// These limits are intentionally conservative and are enforced before
/// arbitrary or externally supplied mathematical structures reach a solver.
/// </summary>
public sealed record MathematicsResourceLimits(
    int MaxInputLength,
    int MaxTokenCount,
    int MaxAstDepth,
    int MaxAstNodes,
    int MaxPolynomialDegree,
    int MaxMatrixDimension,
    TimeSpan SolverTimeout,
    long MaxEstimatedMemoryBytes,
    int MaxConcurrentOperations)
{
    public static MathematicsResourceLimits ProductionDefaults { get; } = new(
        MaxInputLength: 4096,
        MaxTokenCount: 512,
        MaxAstDepth: 64,
        MaxAstNodes: 4096,
        MaxPolynomialDegree: 12,
        MaxMatrixDimension: 12,
        SolverTimeout: TimeSpan.FromSeconds(2),
        MaxEstimatedMemoryBytes: 8L * 1024L * 1024L,
        MaxConcurrentOperations: 8);
}

public sealed record MathematicsAstBudgetReport(
    int NodeCount,
    int MaxDepth,
    int? PolynomialDegree,
    int MaxMatrixRows,
    int MaxMatrixColumns,
    long EstimatedBytes);

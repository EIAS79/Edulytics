using Edulytics.Core.Mathematics.Ast;
using Edulytics.Core.Mathematics.Solving;
using Edulytics.Core.Mathematics.Verification;

namespace Edulytics.Services.Mathematics.Contracts;

/// <summary>
/// Stable service boundary for solver providers. Provider-specific CAS types must
/// not cross this interface.
/// </summary>
public interface IMathematicsSolver
{
    MathematicsSolveResult Solve(MathematicsSolveRequest request);
}

public interface IMathematicsVerifier
{
    MathematicsVerificationResult Verify(
        MathematicsSolveRequest request,
        MathematicsSolveResult result);
}

public interface IMathematicsRenderer
{
    string Render(MathNode node);
}

public interface IMathematicsAnswerEvaluator
{
    MathematicsAnswerEvaluation Evaluate(
        MathNode submitted,
        MathNode expected,
        MathematicsAnswerEvaluationPolicy policy);
}

public sealed record MathematicsAnswerEvaluationPolicy(
    bool RequireExact = true,
    decimal? AbsoluteTolerance = null,
    int? SignificantFigures = null,
    int? DecimalPlaces = null);

public sealed record MathematicsAnswerEvaluation(
    bool IsEquivalent,
    string Method,
    IReadOnlyList<string> Diagnostics);

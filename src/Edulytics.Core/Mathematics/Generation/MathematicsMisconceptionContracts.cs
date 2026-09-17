using Edulytics.Core.Mathematics.Ast;

namespace Edulytics.Core.Mathematics.Generation;

public sealed record GeneratedMathematicsDistractor(
    string MisconceptionId,
    MathNode Answer,
    string Rationale);

public interface IMathematicsMisconceptionEngine
{
    IReadOnlyList<GeneratedMathematicsDistractor> Generate(
        MathNode problem,
        MathNode correctAnswer,
        int maxDistractors = 3);
}

namespace Edulytics.Core.Mathematics.Difficulty;

/// <summary>
/// Internal multidimensional Mathematics difficulty representation. Existing
/// Easy/Medium/Challenging UI bands can be projected from this richer model.
/// </summary>
public sealed record MathematicsComplexityVector(
    int ArithmeticMagnitude = 0,
    int ExpressionNodeCount = 0,
    int ExpressionDepth = 0,
    int RequiredTransformationCount = 0,
    int RequiredStrategyCount = 0,
    int BranchCount = 0,
    int UnknownCount = 0,
    int PolynomialDegree = 0,
    int DomainConstraintCount = 0,
    int RepresentationSwitchCount = 0,
    int ExactArithmeticComplexity = 0,
    int AlgebraicManipulationDepth = 0,
    int ProofBurden = 0,
    int ModellingBurden = 0,
    int InterpretationBurden = 0,
    int CalculatorDependency = 0,
    int VisualReasoningBurden = 0,
    int UnfamiliarityLevel = 0,
    int MultiPartDependencyDepth = 0);

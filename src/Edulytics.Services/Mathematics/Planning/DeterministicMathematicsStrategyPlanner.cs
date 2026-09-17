using Edulytics.Core.Mathematics.Ast;
using Edulytics.Core.Mathematics.Difficulty;
using Edulytics.Core.Mathematics.Planning;

namespace Edulytics.Services.Mathematics.Planning;

/// <summary>
/// Deterministic, bounded strategy selection for Mathematics V2. The planner does
/// not solve a problem and never invents a capability. It classifies the declared
/// Math AST and returns a small ordered strategy set that downstream verified
/// solvers may execute.
/// </summary>
public sealed class DeterministicMathematicsStrategyPlanner : IMathematicsStrategyPlanner
{
    public MathematicsStrategyPlan Plan(MathematicsPlanningRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        if (request.MaxCandidateStrategies is < 1 or > 8
            || request.MaxAstNodes is < 1 or > 4096
            || request.MaxAstDepth is < 1 or > 128)
        {
            return Unsupported("Planner resource bounds are outside the supported safety envelope.");
        }

        if (!AstShape.TryAnalyze(request.Problem, request.MaxAstNodes, request.MaxAstDepth, out var shape))
        {
            return new MathematicsStrategyPlan(
                MathematicsPlanningStatus.ResourceLimit,
                null,
                [],
                new MathematicsComplexityVector(),
                ["Problem AST exceeds the deterministic strategy-planning node/depth budget."]);
        }

        var candidates = BuildCandidates(request.Problem)
            .OrderBy(candidate => candidate.Priority)
            .ThenBy(candidate => candidate.StrategyId, StringComparer.Ordinal)
            .Take(request.MaxCandidateStrategies)
            .ToArray();

        if (candidates.Length == 0)
        {
            return Unsupported("No reviewed strategy is registered for this Math AST shape.", shape);
        }

        var complexity = new MathematicsComplexityVector(
            ExpressionNodeCount: shape.NodeCount,
            ExpressionDepth: shape.Depth,
            RequiredTransformationCount: candidates[0].EstimatedStepCount,
            RequiredStrategyCount: candidates.Length,
            BranchCount: shape.BranchCount,
            UnknownCount: shape.SymbolCount,
            DomainConstraintCount: request.Assumptions.Count,
            RepresentationSwitchCount: shape.RepresentationSwitchCount,
            AlgebraicManipulationDepth: Math.Min(10, Math.Max(0, shape.Depth - 2)),
            ProofBurden: shape.ProofBurden,
            ModellingBurden: shape.ModellingBurden,
            InterpretationBurden: shape.InterpretationBurden,
            UnfamiliarityLevel: candidates.Length > 1 ? 1 : 0,
            MultiPartDependencyDepth: shape.MultiPartDependencyDepth);

        return new MathematicsStrategyPlan(
            MathematicsPlanningStatus.Planned,
            candidates[0].StrategyId,
            candidates,
            complexity,
            []);
    }

    private static IReadOnlyList<MathematicsStrategyCandidate> BuildCandidates(MathNode problem)
    {
        var result = new List<MathematicsStrategyCandidate>();
        switch (problem)
        {
            case EquationSystemNode system when system.Equations.Count == 2:
                result.Add(Candidate("systems.elimination.exact", "Eliminate one variable using exact arithmetic, then back-substitute and verify both original equations.", 4, 10));
                result.Add(Candidate("systems.substitution.exact", "Isolate one variable, substitute into the other equation, then verify the ordered pair in both originals.", 5, 20));
                break;
            case EquationNode:
                result.Add(Candidate("equations.normalize-classify-solve-verify", "Normalize both sides, classify the supported equation family, solve exactly, then substitute into the original equation.", 4, 10));
                break;
            case InequalityNode:
                result.Add(Candidate("inequalities.normalize-boundary-orientation-verify", "Normalize the affine relation, preserve/reverse orientation when required, and verify boundary plus representative points.", 4, 10));
                break;
            case DerivativeNode:
                result.Add(Candidate("calculus.derivative.symbolic-verify", "Differentiate symbolically under the supported exact rules and independently verify the derivative structure.", 3, 10));
                break;
            case IntegralNode integral when integral.LowerBound is not null && integral.UpperBound is not null:
                result.Add(Candidate("calculus.definite-integral-antiderivative-verify", "Construct an exact antiderivative, evaluate both bounds and independently verify from the original integrand.", 4, 10));
                break;
            case IntegralNode:
                result.Add(Candidate("calculus.indefinite-integral-symbolic", "Construct an exact symbolic antiderivative for the supported integrand family.", 3, 10));
                break;
            case MatrixNode:
                result.Add(Candidate("linear-algebra.matrix-exact", "Apply the declared bounded matrix operation using exact arithmetic and independently recompute the result.", 3, 10));
                break;
            case VectorNode:
                result.Add(Candidate("linear-algebra.vector-exact", "Apply the declared bounded vector operation componentwise and independently recompute it.", 3, 10));
                break;
            case FunctionCallNode call when call.FunctionName.StartsWith("mechanics_", StringComparison.Ordinal):
                result.Add(Candidate("modelling.mechanics-model-units-solve-verify", "Validate the declared physical model and SI dimensions, solve with exact arithmetic, then independently recompute the relationship.", 4, 10, "canonical SI quantities"));
                break;
            case FunctionCallNode call when call.FunctionName.Contains("bisection", StringComparison.Ordinal)
                                                || call.FunctionName.Contains("newton", StringComparison.Ordinal)
                                                || call.FunctionName.Contains("trapezoidal", StringComparison.Ordinal):
                result.Add(Candidate("numerical.fixed-process-recompute", "Execute the declared bounded numerical process for the fixed iteration/subdivision count and independently recompute every state transition.", 4, 10));
                break;
            case FunctionCallNode:
                result.Add(Candidate("function.declared-operation-exact", "Apply the reviewed declared function operation using exact arithmetic and verify from the original arguments.", 3, 10));
                break;
            default:
                result.Add(Candidate("expression.normalize-evaluate-exact", "Normalize the expression and evaluate only operations supported by the exact arithmetic kernel.", 2, 10));
                break;
        }
        return result;
    }

    private static MathematicsStrategyCandidate Candidate(string id, string rationale, int steps, int priority, params string[] preconditions) =>
        new(id, rationale, steps, priority, preconditions);

    private static MathematicsStrategyPlan Unsupported(string diagnostic, AstShape? shape = null) =>
        new(
            MathematicsPlanningStatus.Unsupported,
            null,
            [],
            shape is null
                ? new MathematicsComplexityVector()
                : new MathematicsComplexityVector(ExpressionNodeCount: shape.Value.NodeCount, ExpressionDepth: shape.Value.Depth, UnknownCount: shape.Value.SymbolCount),
            [diagnostic]);
}

internal readonly record struct AstShape(
    int NodeCount,
    int Depth,
    int SymbolCount,
    int BranchCount,
    int RepresentationSwitchCount,
    int ProofBurden,
    int ModellingBurden,
    int InterpretationBurden,
    int MultiPartDependencyDepth)
{
    public static bool TryAnalyze(MathNode root, int maxNodes, int maxDepth, out AstShape shape)
    {
        var nodes = 0;
        var depth = 0;
        var branches = 0;
        var representationSwitches = 0;
        var proof = 0;
        var modelling = 0;
        var interpretation = 0;
        var multipart = 0;
        var symbols = new HashSet<string>(StringComparer.Ordinal);
        var ok = Visit(root, 1);
        shape = new AstShape(nodes, depth, symbols.Count, branches, representationSwitches, proof, modelling, interpretation, multipart);
        return ok;

        bool Visit(MathNode node, int currentDepth)
        {
            nodes++;
            depth = Math.Max(depth, currentDepth);
            if (nodes > maxNodes || currentDepth > maxDepth)
            {
                return false;
            }

            switch (node)
            {
                case SymbolNode symbol:
                    symbols.Add(symbol.Name);
                    return true;
                case NegateNode negate:
                    return Visit(negate.Operand, currentDepth + 1);
                case AddNode add:
                    branches += Math.Max(0, add.Terms.Count - 1);
                    return add.Terms.All(x => Visit(x, currentDepth + 1));
                case MultiplyNode multiply:
                    branches += Math.Max(0, multiply.Factors.Count - 1);
                    return multiply.Factors.All(x => Visit(x, currentDepth + 1));
                case DivideNode divide:
                    return Visit(divide.Numerator, currentDepth + 1) && Visit(divide.Denominator, currentDepth + 1);
                case PowerNode power:
                    return Visit(power.Base, currentDepth + 1) && Visit(power.Exponent, currentDepth + 1);
                case RootNode rootNode:
                    return Visit(rootNode.Radicand, currentDepth + 1);
                case EquationNode equation:
                    proof = Math.Max(proof, 1);
                    return Visit(equation.Left, currentDepth + 1) && Visit(equation.Right, currentDepth + 1);
                case EquationSystemNode system:
                    branches += Math.Max(0, system.Equations.Count - 1);
                    multipart = Math.Max(multipart, system.Equations.Count);
                    proof = Math.Max(proof, 2);
                    return system.Equations.All(x => Visit(x, currentDepth + 1));
                case InequalityNode inequality:
                    proof = Math.Max(proof, 2);
                    branches++;
                    return Visit(inequality.Left, currentDepth + 1) && Visit(inequality.Right, currentDepth + 1);
                case FunctionCallNode call:
                    if (call.FunctionName.StartsWith("mechanics_", StringComparison.Ordinal))
                    {
                        modelling = Math.Max(modelling, 3);
                        interpretation = Math.Max(interpretation, 2);
                        representationSwitches++;
                    }
                    return call.Arguments.All(x => Visit(x, currentDepth + 1));
                case VectorNode vector:
                    representationSwitches++;
                    return vector.Components.All(x => Visit(x, currentDepth + 1));
                case MatrixNode matrix:
                    representationSwitches++;
                    branches += Math.Max(0, matrix.Rows.Count - 1);
                    return matrix.Rows.SelectMany(row => row).All(x => Visit(x, currentDepth + 1));
                case DerivativeNode derivative:
                    proof = Math.Max(proof, 2);
                    return Visit(derivative.Expression, currentDepth + 1) && Visit(derivative.Variable, currentDepth + 1);
                case IntegralNode integral:
                    proof = Math.Max(proof, 2);
                    return Visit(integral.Integrand, currentDepth + 1)
                        && Visit(integral.Variable, currentDepth + 1)
                        && (integral.LowerBound is null || Visit(integral.LowerBound, currentDepth + 1))
                        && (integral.UpperBound is null || Visit(integral.UpperBound, currentDepth + 1));
                default:
                    return true;
            }
        }
    }
}

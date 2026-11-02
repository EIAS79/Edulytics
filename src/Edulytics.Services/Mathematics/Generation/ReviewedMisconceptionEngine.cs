using System.Numerics;
using System.Text.Json;
using Edulytics.Core.Mathematics.Ast;
using Edulytics.Core.Mathematics.Domains;
using Edulytics.Core.Mathematics.Generation;
using Edulytics.Services.Mathematics.Contracts;
using Edulytics.Services.Mathematics.Verification;

namespace Edulytics.Services.Mathematics.Generation;

/// <summary>
/// Produces only reviewed deterministic misconception transforms. A candidate is
/// returned only after the shared answer-equivalence engine proves it is not the
/// correct answer. This prevents accidentally-valid multiple-choice distractors.
/// </summary>
public sealed class ReviewedMisconceptionEngine : IMathematicsMisconceptionEngine
{
    private readonly IMathematicsAnswerEvaluator evaluator;

    public ReviewedMisconceptionEngine(IMathematicsAnswerEvaluator? evaluator = null)
    {
        this.evaluator = evaluator ?? new MathematicsAnswerEquivalenceV2();
    }

    public IReadOnlyList<GeneratedMathematicsDistractor> Generate(MathNode problem, MathNode correctAnswer, int maxDistractors = 3)
    {
        ArgumentNullException.ThrowIfNull(problem);
        ArgumentNullException.ThrowIfNull(correctAnswer);
        if (maxDistractors is < 1 or > 6)
        {
            throw new ArgumentOutOfRangeException(nameof(maxDistractors));
        }

        var candidates = BuildCandidates(problem, correctAnswer);
        var accepted = new List<GeneratedMathematicsDistractor>();
        var fingerprints = new HashSet<string>(StringComparer.Ordinal);
        foreach (var candidate in candidates)
        {
            if (accepted.Count >= maxDistractors)
            {
                break;
            }

            var evaluation = evaluator.Evaluate(candidate.Answer, correctAnswer, new MathematicsAnswerEvaluationPolicy());
            if (evaluation.IsEquivalent)
            {
                continue;
            }

            var fingerprint = JsonSerializer.Serialize<MathNode>(candidate.Answer);
            if (!fingerprints.Add(fingerprint))
            {
                continue;
            }
            accepted.Add(candidate);
        }

        return accepted;
    }

    private static IEnumerable<GeneratedMathematicsDistractor> BuildCandidates(MathNode problem, MathNode correctAnswer)
    {
        if (TryScalar(correctAnswer, out var scalar, out var wrap))
        {
            if (!scalar.Numerator.IsZero)
            {
                yield return new GeneratedMathematicsDistractor(
                    "sign.reversal",
                    wrap(new ExactRational(BigInteger.Negate(scalar.Numerator), scalar.Denominator)),
                    "Applies the opposite sign to the final exact value.");
            }

            yield return new GeneratedMathematicsDistractor(
                "arithmetic.off_by_one",
                wrap(scalar + ExactRational.FromInteger(BigInteger.One)),
                "Introduces a one-unit arithmetic slip in the final exact value.");

            if (!scalar.Numerator.IsZero && BigInteger.Abs(scalar.Numerator) != scalar.Denominator)
            {
                yield return new GeneratedMathematicsDistractor(
                    "fraction.reciprocal",
                    wrap(new ExactRational(scalar.Denominator, scalar.Numerator)),
                    "Inverts numerator and denominator instead of preserving the solved ratio.");
            }
        }

        if (correctAnswer is VectorNode vector && vector.Components.Count >= 2)
        {
            yield return new GeneratedMathematicsDistractor(
                "ordered_result.component_order",
                new VectorNode(vector.Components.Reverse().ToArray()),
                "Reverses the order of an ordered/vector result.");
        }

        if (correctAnswer is MatrixNode matrix && matrix.Rows.Count > 0 && matrix.Rows.All(row => row.Count == matrix.Rows.Count))
        {
            var transposed = new List<IReadOnlyList<MathNode>>();
            for (var column = 0; column < matrix.Rows.Count; column++)
            {
                var row = new List<MathNode>();
                for (var sourceRow = 0; sourceRow < matrix.Rows.Count; sourceRow++)
                {
                    row.Add(matrix.Rows[sourceRow][column]);
                }
                transposed.Add(row);
            }
            yield return new GeneratedMathematicsDistractor(
                "matrix.transpose_confusion",
                new MatrixNode(transposed),
                "Returns the transposed arrangement instead of the required matrix result.");
        }

        if (problem is EquationNode && correctAnswer is VectorNode roots && roots.Components.Count == 2)
        {
            yield return new GeneratedMathematicsDistractor(
                "quadratic.forget_plus_minus",
                roots.Components[0],
                "Keeps only one branch of a two-root ± solution.");
        }
    }

    private static bool TryScalar(MathNode answer, out ExactRational value, out Func<ExactRational, MathNode> wrap)
    {
        if (MathematicsAnswerEquivalenceV2.TryEvaluateRational(answer, out value))
        {
            wrap = ToNode;
            return true;
        }

        if (answer is FunctionCallNode quantity
            && quantity.FunctionName == "quantity_si"
            && quantity.Arguments.Count == 2
            && quantity.Arguments[1] is SymbolNode unit
            && MathematicsAnswerEquivalenceV2.TryEvaluateRational(quantity.Arguments[0], out value))
        {
            wrap = scalar => new FunctionCallNode("quantity_si", [ToNode(scalar), new SymbolNode(unit.Name)]);
            return true;
        }

        wrap = _ => throw new InvalidOperationException();
        return false;
    }

    private static MathNode ToNode(ExactRational value) =>
        value.Denominator == BigInteger.One ? new IntegerNode(value.Numerator) : new RationalNode(value);
}

using System.Globalization;
using System.Numerics;
using Edulytics.Core.Mathematics.Ast;
using Edulytics.Core.Mathematics.Identifiers;
using Edulytics.Services.Mathematics.Contracts;

namespace Edulytics.Services.Mathematics.Generation;

/// <summary>
/// Deterministic generator for the first certified exact area-between-curves
/// family. Generated variants intentionally place both exact intersections
/// inside a wider requested interval so the solver must exercise piecewise
/// absolute integration rather than a cosmetic one-integral shortcut.
/// </summary>
public sealed class ExactAreaBetweenCurvesQuestionFactory
{
    public const string FamilyId =
        "calculus.area_between_curves.polynomial_exact";

    public static readonly SkillId Skill =
        new("calculus.area_between_curves");

    private readonly IMathematicsSolver solver;
    private readonly IMathematicsVerifier verifier;

    public ExactAreaBetweenCurvesQuestionFactory(
        IMathematicsSolver solver,
        IMathematicsVerifier verifier)
    {
        this.solver =
            solver ??
            throw new ArgumentNullException(nameof(solver));
        this.verifier =
            verifier ??
            throw new ArgumentNullException(nameof(verifier));
    }

    public VerifiedGeneratedMathematicsProblem Generate(
        int variantKey,
        int difficultyBand)
    {
        ExactFunctionsGraphsSequencesGeneration.ValidateDifficulty(
            difficultyBand);

        var state =
            ExactFunctionsGraphsSequencesGeneration.Seed(
                variantKey,
                0xA4EA2026u);

        var scale =
            ExactFunctionsGraphsSequencesGeneration.Next(
                ref state,
                1,
                difficultyBand + 1);

        var halfWidth =
            ExactFunctionsGraphsSequencesGeneration.Next(
                ref state,
                1 + difficultyBand,
                3 + difficultyBand);

        var centre =
            ExactFunctionsGraphsSequencesGeneration.Next(
                ref state,
                -3,
                3);

        var baseline =
            ExactFunctionsGraphsSequencesGeneration.Next(
                ref state,
                -2,
                3);

        var margin =
            ExactFunctionsGraphsSequencesGeneration.Next(
                ref state,
                1,
                1 + difficultyBand);

        // f(x) = baseline + scale * (halfWidth^2 - (x-centre)^2)
        // g(x) = baseline
        // Therefore intersections are exactly centre ± halfWidth and the
        // wider bounds force the ordering to switch twice.
        var firstA = -scale;
        var firstB = 2 * scale * centre;
        var firstC =
            baseline +
            scale *
            (halfWidth * halfWidth -
             centre * centre);

        var secondA = 0;
        var secondB = 0;
        var secondC = baseline;

        var leftIntersection =
            centre - halfWidth;
        var rightIntersection =
            centre + halfWidth;

        var lower =
            leftIntersection - margin;
        var upper =
            rightIntersection + margin;

        var problem =
            new AreaBetweenCurvesNode(
                Polynomial(
                    firstA,
                    firstB,
                    firstC),
                Polynomial(
                    secondA,
                    secondB,
                    secondC),
                new SymbolNode("x"),
                I(lower),
                I(upper));

        return ExactFunctionsGraphsSequencesGeneration
            .SolveVerifyAndBuild(
                FamilyId,
                Skill,
                problem,
                solver,
                verifier,
                [
                    new CapabilityId(
                        "rational.exact_arithmetic"),
                    new CapabilityId(
                        "algebra.polynomial.normalize.exact_rational"),
                    new CapabilityId(
                        "calculus.polynomial.definite_integral.exact"),
                    new CapabilityId(
                        "calculus.area_between_curves.exact"),
                    new CapabilityId(
                        "calculus.verify.area_between_curves.piecewise")
                ],
                variantKey,
                difficultyBand,
                new Dictionary<string, string>(
                    StringComparer.Ordinal)
                {
                    ["calculusKind"] =
                        "area-between-curves",
                    ["first.a"] =
                        firstA.ToString(
                            CultureInfo.InvariantCulture),
                    ["first.b"] =
                        firstB.ToString(
                            CultureInfo.InvariantCulture),
                    ["first.c"] =
                        firstC.ToString(
                            CultureInfo.InvariantCulture),
                    ["second.a"] =
                        secondA.ToString(
                            CultureInfo.InvariantCulture),
                    ["second.b"] =
                        secondB.ToString(
                            CultureInfo.InvariantCulture),
                    ["second.c"] =
                        secondC.ToString(
                            CultureInfo.InvariantCulture),
                    ["lower"] =
                        lower.ToString(
                            CultureInfo.InvariantCulture),
                    ["upper"] =
                        upper.ToString(
                            CultureInfo.InvariantCulture),
                    ["leftIntersection"] =
                        leftIntersection.ToString(
                            CultureInfo.InvariantCulture),
                    ["rightIntersection"] =
                        rightIntersection.ToString(
                            CultureInfo.InvariantCulture),
                    ["requiresPiecewiseAbsoluteArea"] =
                        "true"
                });
    }

    private static MathNode Polynomial(
        int quadratic,
        int linear,
        int constant)
    {
        var terms =
            new List<MathNode>();

        if (quadratic != 0)
        {
            terms.Add(
                quadratic == 1
                    ? new PowerNode(
                        new SymbolNode("x"),
                        I(2))
                    : new MultiplyNode(
                        [
                            I(quadratic),
                            new PowerNode(
                                new SymbolNode("x"),
                                I(2))
                        ]));
        }

        if (linear != 0)
        {
            terms.Add(
                linear == 1
                    ? new SymbolNode("x")
                    : new MultiplyNode(
                        [
                            I(linear),
                            new SymbolNode("x")
                        ]));
        }

        if (constant != 0 ||
            terms.Count == 0)
        {
            terms.Add(I(constant));
        }

        return terms.Count == 1
            ? terms[0]
            : new AddNode(terms);
    }

    private static IntegerNode I(
        int value) =>
        new(new BigInteger(value));
}

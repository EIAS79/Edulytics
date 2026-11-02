using System.Globalization;
using System.Numerics;
using Edulytics.Core.Mathematics.Ast;
using Edulytics.Core.Mathematics.Domains;
using Edulytics.Core.Mathematics.Identifiers;
using Edulytics.Services.Mathematics.Contracts;

namespace Edulytics.Services.Mathematics.Generation;

public sealed class ExactPolynomialDerivativeQuestionFactory
{
    public const string FamilyId = "calculus.polynomial.derivative.exact";
    public static readonly SkillId Skill = new("calculus.polynomial.differentiate");
    private readonly IMathematicsSolver solver;
    private readonly IMathematicsVerifier verifier;

    public ExactPolynomialDerivativeQuestionFactory(IMathematicsSolver solver, IMathematicsVerifier verifier)
    {
        this.solver = solver ?? throw new ArgumentNullException(nameof(solver));
        this.verifier = verifier ?? throw new ArgumentNullException(nameof(verifier));
    }

    public VerifiedGeneratedMathematicsProblem Generate(int variantKey, int difficultyBand)
    {
        ExactFunctionsGraphsSequencesGeneration.ValidateDifficulty(difficultyBand);
        var state = ExactFunctionsGraphsSequencesGeneration.Seed(variantKey, 0xC0FFEE11u);
        var degree = difficultyBand + 1;
        var expression = ExactCalculusGeneration.Polynomial(ref state, degree, difficultyBand, allowRational: difficultyBand == 3);
        var problem = new DerivativeNode(expression, new SymbolNode("x"), 1);
        return ExactFunctionsGraphsSequencesGeneration.SolveVerifyAndBuild(
            FamilyId,
            Skill,
            problem,
            solver,
            verifier,
            [
                new CapabilityId("rational.exact_arithmetic"),
                new CapabilityId("algebra.polynomial.normalize.exact_rational"),
                new CapabilityId("calculus.polynomial.derivative.exact"),
                new CapabilityId("calculus.verify.polynomial.derivative")
            ],
            variantKey,
            difficultyBand,
            new Dictionary<string, string>(StringComparer.Ordinal)
            {
                ["degree"] = degree.ToString(CultureInfo.InvariantCulture),
                ["calculusKind"] = "first-derivative"
            });
    }
}

public sealed class ExactPolynomialDefiniteIntegralQuestionFactory
{
    public const string FamilyId = "calculus.polynomial.definite_integral.exact";
    public static readonly SkillId Skill = new("calculus.polynomial.integrate.definite");
    private readonly IMathematicsSolver solver;
    private readonly IMathematicsVerifier verifier;

    public ExactPolynomialDefiniteIntegralQuestionFactory(IMathematicsSolver solver, IMathematicsVerifier verifier)
    {
        this.solver = solver ?? throw new ArgumentNullException(nameof(solver));
        this.verifier = verifier ?? throw new ArgumentNullException(nameof(verifier));
    }

    public VerifiedGeneratedMathematicsProblem Generate(int variantKey, int difficultyBand)
    {
        ExactFunctionsGraphsSequencesGeneration.ValidateDifficulty(difficultyBand);
        var state = ExactFunctionsGraphsSequencesGeneration.Seed(variantKey, 0x1A2B3C4Du);
        var degree = difficultyBand;
        var expression = ExactCalculusGeneration.Polynomial(ref state, degree, difficultyBand, allowRational: difficultyBand == 3);

        MathNode lower;
        MathNode upper;
        if (difficultyBand < 3)
        {
            var low = ExactFunctionsGraphsSequencesGeneration.Next(ref state, -3, 1);
            var high = ExactFunctionsGraphsSequencesGeneration.Next(ref state, low + 1, low + 5);
            lower = ExactFunctionsGraphsSequencesGeneration.I(low);
            upper = ExactFunctionsGraphsSequencesGeneration.I(high);
        }
        else
        {
            var lowNumerator = ExactFunctionsGraphsSequencesGeneration.Next(ref state, -5, 0);
            var highNumerator = ExactFunctionsGraphsSequencesGeneration.Next(ref state, 1, 6);
            lower = new RationalNode(new ExactRational(new BigInteger(lowNumerator), new BigInteger(2)));
            upper = new RationalNode(new ExactRational(new BigInteger(highNumerator), new BigInteger(2)));
        }

        var problem = new IntegralNode(expression, new SymbolNode("x"), lower, upper);
        return ExactFunctionsGraphsSequencesGeneration.SolveVerifyAndBuild(
            FamilyId,
            Skill,
            problem,
            solver,
            verifier,
            [
                new CapabilityId("rational.exact_arithmetic"),
                new CapabilityId("algebra.polynomial.normalize.exact_rational"),
                new CapabilityId("calculus.polynomial.definite_integral.exact"),
                new CapabilityId("calculus.verify.polynomial.definite_integral")
            ],
            variantKey,
            difficultyBand,
            new Dictionary<string, string>(StringComparer.Ordinal)
            {
                ["degree"] = degree.ToString(CultureInfo.InvariantCulture),
                ["calculusKind"] = "definite-integral"
            });
    }
}

internal static class ExactCalculusGeneration
{
    public static MathNode Polynomial(ref uint state, int degree, int difficultyBand, bool allowRational)
    {
        var terms = new List<MathNode>();
        for (var power = degree; power >= 0; power--)
        {
            var raw = ExactFunctionsGraphsSequencesGeneration.NonZeroSigned(ref state, difficultyBand == 1 ? 5 : 9);
            MathNode coefficient = allowRational && power == degree
                ? new RationalNode(new ExactRational(new BigInteger(raw), new BigInteger(2)))
                : ExactFunctionsGraphsSequencesGeneration.I(raw);

            if (power == 0)
            {
                terms.Add(coefficient);
                continue;
            }

            MathNode variablePower = power == 1
                ? new SymbolNode("x")
                : new PowerNode(new SymbolNode("x"), ExactFunctionsGraphsSequencesGeneration.I(power));
            terms.Add(new MultiplyNode([coefficient, variablePower]));
        }
        return terms.Count == 1 ? terms[0] : new AddNode(terms);
    }
}

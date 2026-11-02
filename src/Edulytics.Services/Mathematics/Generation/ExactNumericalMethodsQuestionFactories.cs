using System.Globalization;
using System.Numerics;
using Edulytics.Core.Mathematics.Ast;
using Edulytics.Core.Mathematics.Identifiers;
using Edulytics.Services.Mathematics.Contracts;

namespace Edulytics.Services.Mathematics.Generation;

public sealed class ExactBisectionIterationQuestionFactory
{
    public const string FamilyId = "numerical.roots.bisection.fixed_iterations.exact_rational";
    public static readonly SkillId Skill = new("numerical.roots.bisection.iterate");
    private readonly IMathematicsSolver solver;
    private readonly IMathematicsVerifier verifier;

    public ExactBisectionIterationQuestionFactory(IMathematicsSolver solver, IMathematicsVerifier verifier)
    {
        this.solver = solver ?? throw new ArgumentNullException(nameof(solver));
        this.verifier = verifier ?? throw new ArgumentNullException(nameof(verifier));
    }

    public VerifiedGeneratedMathematicsProblem Generate(int variantKey, int difficultyBand)
    {
        ExactFunctionsGraphsSequencesGeneration.ValidateDifficulty(difficultyBand);
        var state = ExactFunctionsGraphsSequencesGeneration.Seed(variantKey, 0xA4093822u);
        var targets = new[] { 2, 3, 5, 6, 7, 10 };
        var target = targets[ExactFunctionsGraphsSequencesGeneration.Next(ref state, 0, targets.Length - 1)];
        var lower = target < 4 ? 1 : target < 9 ? 2 : 3;
        var upper = lower + 1;
        var iterations = difficultyBand == 1 ? 2 : difficultyBand == 2 ? 4 : 6;
        var x = new SymbolNode("x");
        var polynomial = new AddNode([new PowerNode(x, I(2)), new NegateNode(I(target))]);
        var problem = new FunctionCallNode("numerical_bisection_fixed_exact", [polynomial, x, I(lower), I(upper), I(iterations)]);
        return ExactNumericalMethodsGeneration.Build(
            FamilyId, Skill, problem, solver, verifier,
            ["rational.exact_arithmetic", "numerical.roots.bisection.fixed_iterations.exact_rational", "numerical.verify.bisection.fixed_iterations"],
            variantKey, difficultyBand, "bisection-bracket");
    }

    private static IntegerNode I(int value) => new(new BigInteger(value));
}

public sealed class ExactNewtonIterationQuestionFactory
{
    public const string FamilyId = "numerical.roots.newton.fixed_iterations.exact_rational";
    public static readonly SkillId Skill = new("numerical.roots.newton.iterate");
    private readonly IMathematicsSolver solver;
    private readonly IMathematicsVerifier verifier;

    public ExactNewtonIterationQuestionFactory(IMathematicsSolver solver, IMathematicsVerifier verifier)
    {
        this.solver = solver ?? throw new ArgumentNullException(nameof(solver));
        this.verifier = verifier ?? throw new ArgumentNullException(nameof(verifier));
    }

    public VerifiedGeneratedMathematicsProblem Generate(int variantKey, int difficultyBand)
    {
        ExactFunctionsGraphsSequencesGeneration.ValidateDifficulty(difficultyBand);
        var state = ExactFunctionsGraphsSequencesGeneration.Seed(variantKey, 0x299F31D0u);
        var target = ExactFunctionsGraphsSequencesGeneration.Next(ref state, 2, difficultyBand == 1 ? 5 : 10);
        var start = target <= 4 ? 2 : 3;
        var iterations = difficultyBand + 1;
        var x = new SymbolNode("x");
        var polynomial = new AddNode([new PowerNode(x, I(2)), new NegateNode(I(target))]);
        var problem = new FunctionCallNode("numerical_newton_fixed_exact", [polynomial, x, I(start), I(iterations)]);
        return ExactNumericalMethodsGeneration.Build(
            FamilyId, Skill, problem, solver, verifier,
            ["rational.exact_arithmetic", "numerical.roots.newton.fixed_iterations.exact_rational", "numerical.verify.newton.fixed_iterations"],
            variantKey, difficultyBand, "newton-iterate");
    }

    private static IntegerNode I(int value) => new(new BigInteger(value));
}

public sealed class ExactTrapezoidalRuleQuestionFactory
{
    public const string FamilyId = "numerical.integration.trapezoidal.fixed_subdivisions.exact_rational";
    public static readonly SkillId Skill = new("numerical.integration.trapezoidal.estimate");
    private readonly IMathematicsSolver solver;
    private readonly IMathematicsVerifier verifier;

    public ExactTrapezoidalRuleQuestionFactory(IMathematicsSolver solver, IMathematicsVerifier verifier)
    {
        this.solver = solver ?? throw new ArgumentNullException(nameof(solver));
        this.verifier = verifier ?? throw new ArgumentNullException(nameof(verifier));
    }

    public VerifiedGeneratedMathematicsProblem Generate(int variantKey, int difficultyBand)
    {
        ExactFunctionsGraphsSequencesGeneration.ValidateDifficulty(difficultyBand);
        var state = ExactFunctionsGraphsSequencesGeneration.Seed(variantKey, 0x082EFA98u);
        var c = ExactFunctionsGraphsSequencesGeneration.Next(ref state, -3, 4);
        var d = ExactFunctionsGraphsSequencesGeneration.Next(ref state, -4, 5);
        var upper = difficultyBand == 1 ? 2 : 3;
        var subdivisions = difficultyBand == 1 ? 2 : difficultyBand == 2 ? 4 : 6;
        var x = new SymbolNode("x");
        var terms = new List<MathNode> { new PowerNode(x, I(2)) };
        if (c != 0) terms.Add(new MultiplyNode([I(c), x]));
        if (d != 0) terms.Add(I(d));
        var polynomial = terms.Count == 1 ? terms[0] : new AddNode(terms);
        var problem = new FunctionCallNode("numerical_trapezoidal_fixed_exact", [polynomial, x, I(0), I(upper), I(subdivisions)]);
        return ExactNumericalMethodsGeneration.Build(
            FamilyId, Skill, problem, solver, verifier,
            ["rational.exact_arithmetic", "numerical.integration.trapezoidal.fixed_subdivisions.exact_rational", "numerical.verify.trapezoidal.fixed_subdivisions"],
            variantKey, difficultyBand, "trapezoidal-estimate");
    }

    private static IntegerNode I(int value) => new(new BigInteger(value));
}

internal static class ExactNumericalMethodsGeneration
{
    public static VerifiedGeneratedMathematicsProblem Build(
        string familyId,
        SkillId skill,
        MathNode problem,
        IMathematicsSolver solver,
        IMathematicsVerifier verifier,
        IReadOnlyList<string> capabilities,
        int variantKey,
        int difficultyBand,
        string operation) =>
        ExactFunctionsGraphsSequencesGeneration.SolveVerifyAndBuild(
            familyId,
            skill,
            problem,
            solver,
            verifier,
            capabilities.Select(id => new CapabilityId(id)).ToArray(),
            variantKey,
            difficultyBand,
            new Dictionary<string, string>(StringComparer.Ordinal)
            {
                ["operation"] = operation,
                ["difficultyBand"] = difficultyBand.ToString(CultureInfo.InvariantCulture),
                ["resultSemantics"] = "exact-rational arithmetic applied to a fixed-step numerical method; method output remains an approximation unless the method lands on an exact root"
            });
}

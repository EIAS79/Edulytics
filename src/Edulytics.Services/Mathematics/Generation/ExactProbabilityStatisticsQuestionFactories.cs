using System.Globalization;
using System.Numerics;
using Edulytics.Core.Mathematics.Ast;
using Edulytics.Core.Mathematics.Domains;
using Edulytics.Core.Mathematics.Identifiers;
using Edulytics.Services.Mathematics.Contracts;

namespace Edulytics.Services.Mathematics.Generation;

public sealed class ExactSimpleProbabilityQuestionFactory
{
    public const string FamilyId = "probability.simple.favourable_over_total.exact";
    public static readonly SkillId Skill = new("probability.simple.evaluate.exact");
    private readonly IMathematicsSolver solver; private readonly IMathematicsVerifier verifier;
    public ExactSimpleProbabilityQuestionFactory(IMathematicsSolver solver, IMathematicsVerifier verifier) { this.solver = solver ?? throw new ArgumentNullException(nameof(solver)); this.verifier = verifier ?? throw new ArgumentNullException(nameof(verifier)); }
    public VerifiedGeneratedMathematicsProblem Generate(int variantKey, int difficultyBand)
    {
        ExactFunctionsGraphsSequencesGeneration.ValidateDifficulty(difficultyBand);
        var state = ExactFunctionsGraphsSequencesGeneration.Seed(variantKey, 0x243F6A88u);
        var total = ExactFunctionsGraphsSequencesGeneration.Next(ref state, difficultyBand == 1 ? 2 : 5, difficultyBand == 1 ? 12 : difficultyBand == 2 ? 30 : 80);
        var favourable = ExactFunctionsGraphsSequencesGeneration.Next(ref state, 0, total);
        var problem = new FunctionCallNode("probability_favourable_over_total_exact", [I(favourable), I(total)]);
        return Build(FamilyId, Skill, problem, solver, verifier,
            ["rational.exact_arithmetic", "probability.simple.exact_rational", "probability.verify.exact_events"], variantKey, difficultyBand, "simple-probability");
    }
    private static IntegerNode I(int value) => new(new BigInteger(value));
}

public sealed class ExactProbabilityComplementQuestionFactory
{
    public const string FamilyId = "probability.complement.exact";
    public static readonly SkillId Skill = new("probability.complement.evaluate.exact");
    private readonly IMathematicsSolver solver; private readonly IMathematicsVerifier verifier;
    public ExactProbabilityComplementQuestionFactory(IMathematicsSolver solver, IMathematicsVerifier verifier) { this.solver = solver ?? throw new ArgumentNullException(nameof(solver)); this.verifier = verifier ?? throw new ArgumentNullException(nameof(verifier)); }
    public VerifiedGeneratedMathematicsProblem Generate(int variantKey, int difficultyBand)
    {
        ExactFunctionsGraphsSequencesGeneration.ValidateDifficulty(difficultyBand);
        var state = ExactFunctionsGraphsSequencesGeneration.Seed(variantKey, 0x85A308D3u);
        var denominator = ExactFunctionsGraphsSequencesGeneration.Next(ref state, difficultyBand == 1 ? 2 : 3, difficultyBand == 1 ? 10 : difficultyBand == 2 ? 18 : 30);
        var numerator = ExactFunctionsGraphsSequencesGeneration.Next(ref state, 0, denominator);
        MathNode probability = numerator == 0 || numerator == denominator
            ? new IntegerNode(new BigInteger(numerator / denominator))
            : new RationalNode(new ExactRational(new BigInteger(numerator), new BigInteger(denominator)));
        var problem = new FunctionCallNode("probability_complement_exact", [probability]);
        return Build(FamilyId, Skill, problem, solver, verifier,
            ["rational.exact_arithmetic", "probability.complement.exact_rational", "probability.verify.exact_events"], variantKey, difficultyBand, "probability-complement");
    }
}

public sealed class ExactArithmeticMeanQuestionFactory
{
    public const string FamilyId = "statistics.mean.arithmetic.exact";
    public static readonly SkillId Skill = new("statistics.mean.arithmetic.exact");
    private readonly IMathematicsSolver solver; private readonly IMathematicsVerifier verifier;
    public ExactArithmeticMeanQuestionFactory(IMathematicsSolver solver, IMathematicsVerifier verifier) { this.solver = solver ?? throw new ArgumentNullException(nameof(solver)); this.verifier = verifier ?? throw new ArgumentNullException(nameof(verifier)); }
    public VerifiedGeneratedMathematicsProblem Generate(int variantKey, int difficultyBand)
    {
        ExactFunctionsGraphsSequencesGeneration.ValidateDifficulty(difficultyBand);
        var state = ExactFunctionsGraphsSequencesGeneration.Seed(variantKey, 0x13198A2Eu);
        var count = difficultyBand == 1 ? 3 : difficultyBand == 2 ? 5 : 7;
        var values = new MathNode[count];
        for (var i = 0; i < count; i++) values[i] = Scalar(ref state, difficultyBand);
        var problem = new FunctionCallNode("statistics_mean_exact", [new VectorNode(values)]);
        return Build(FamilyId, Skill, problem, solver, verifier,
            ["rational.exact_arithmetic", "statistics.mean.arithmetic.exact_rational", "statistics.verify.exact_means"], variantKey, difficultyBand, "arithmetic-mean");
    }
}

public sealed class ExactFrequencyMeanQuestionFactory
{
    public const string FamilyId = "statistics.mean.frequency_table.exact";
    public static readonly SkillId Skill = new("statistics.mean.frequency_table.exact");
    private readonly IMathematicsSolver solver; private readonly IMathematicsVerifier verifier;
    public ExactFrequencyMeanQuestionFactory(IMathematicsSolver solver, IMathematicsVerifier verifier) { this.solver = solver ?? throw new ArgumentNullException(nameof(solver)); this.verifier = verifier ?? throw new ArgumentNullException(nameof(verifier)); }
    public VerifiedGeneratedMathematicsProblem Generate(int variantKey, int difficultyBand)
    {
        ExactFunctionsGraphsSequencesGeneration.ValidateDifficulty(difficultyBand);
        var state = ExactFunctionsGraphsSequencesGeneration.Seed(variantKey, 0x03707344u);
        var rowCount = difficultyBand == 1 ? 3 : difficultyBand == 2 ? 4 : 6;
        var rows = new IReadOnlyList<MathNode>[rowCount];
        var anyPositive = false;
        for (var i = 0; i < rowCount; i++)
        {
            var value = Scalar(ref state, difficultyBand);
            var frequency = ExactFunctionsGraphsSequencesGeneration.Next(ref state, 0, difficultyBand == 1 ? 4 : 7);
            if (i == rowCount - 1 && !anyPositive) frequency = 1;
            anyPositive |= frequency > 0;
            rows[i] = [value, new IntegerNode(new BigInteger(frequency))];
        }
        var problem = new FunctionCallNode("statistics_frequency_mean_exact", [new MatrixNode(rows)]);
        return Build(FamilyId, Skill, problem, solver, verifier,
            ["rational.exact_arithmetic", "statistics.mean.frequency_table.exact_rational", "statistics.verify.exact_means"], variantKey, difficultyBand, "frequency-table-mean");
    }
}

internal static class ExactProbabilityStatisticsGeneration
{
    public static MathNode Scalar(ref uint state, int difficultyBand)
    {
        var numerator = ExactFunctionsGraphsSequencesGeneration.Next(ref state, -8 - difficultyBand * 3, 8 + difficultyBand * 3);
        if (difficultyBand < 3 || ExactFunctionsGraphsSequencesGeneration.Next(ref state, 0, 1) == 0)
            return new IntegerNode(new BigInteger(numerator));
        var denominator = ExactFunctionsGraphsSequencesGeneration.Next(ref state, 2, 5);
        return new RationalNode(new ExactRational(new BigInteger(numerator), new BigInteger(denominator)));
    }
}

file static class ProbabilityStatisticsFactoryHelpers
{
    public static MathNode Scalar(ref uint state, int difficultyBand) => ExactProbabilityStatisticsGeneration.Scalar(ref state, difficultyBand);
}

file static class ProbabilityStatisticsFactoryBuild
{
    public static VerifiedGeneratedMathematicsProblem Build(string familyId, SkillId skill, MathNode problem, IMathematicsSolver solver, IMathematicsVerifier verifier, IReadOnlyList<string> capabilities, int variantKey, int difficultyBand, string operation) =>
        ExactFunctionsGraphsSequencesGeneration.SolveVerifyAndBuild(
            familyId, skill, problem, solver, verifier,
            capabilities.Select(id => new CapabilityId(id)).ToArray(), variantKey, difficultyBand,
            new Dictionary<string, string>(StringComparer.Ordinal)
            {
                ["operation"] = operation,
                ["difficultyBand"] = difficultyBand.ToString(CultureInfo.InvariantCulture)
            });
}

file static VerifiedGeneratedMathematicsProblem Build(string familyId, SkillId skill, MathNode problem, IMathematicsSolver solver, IMathematicsVerifier verifier, IReadOnlyList<string> capabilities, int variantKey, int difficultyBand, string operation) =>
    ProbabilityStatisticsFactoryBuild.Build(familyId, skill, problem, solver, verifier, capabilities, variantKey, difficultyBand, operation);

file static MathNode Scalar(ref uint state, int difficultyBand) => ProbabilityStatisticsFactoryHelpers.Scalar(ref state, difficultyBand);

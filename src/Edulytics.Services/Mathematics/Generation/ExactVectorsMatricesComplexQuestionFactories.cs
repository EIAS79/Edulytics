using System.Globalization;
using System.Numerics;
using Edulytics.Core.Mathematics.Ast;
using Edulytics.Core.Mathematics.Domains;
using Edulytics.Core.Mathematics.Identifiers;
using Edulytics.Services.Mathematics.Contracts;

namespace Edulytics.Services.Mathematics.Generation;

public sealed class ExactVectorAddQuestionFactory
{
    public const string FamilyId = "vectors.add.exact_rational";
    public static readonly SkillId Skill = new("vectors.add.exact");
    private readonly IMathematicsSolver solver; private readonly IMathematicsVerifier verifier;
    public ExactVectorAddQuestionFactory(IMathematicsSolver solver, IMathematicsVerifier verifier) { this.solver = solver ?? throw new ArgumentNullException(nameof(solver)); this.verifier = verifier ?? throw new ArgumentNullException(nameof(verifier)); }
    public VerifiedGeneratedMathematicsProblem Generate(int variantKey, int difficultyBand)
    {
        ExactFunctionsGraphsSequencesGeneration.ValidateDifficulty(difficultyBand);
        var state = ExactFunctionsGraphsSequencesGeneration.Seed(variantKey, 0xA4093822u);
        var dimension = difficultyBand == 1 ? 2 : ExactFunctionsGraphsSequencesGeneration.Next(ref state, 2, Math.Min(4, difficultyBand + 1));
        var left = ExactLinearAlgebraGeneration.Vector(ref state, dimension, difficultyBand);
        var right = ExactLinearAlgebraGeneration.Vector(ref state, dimension, difficultyBand);
        return ExactLinearAlgebraGeneration.Build(FamilyId, Skill, new FunctionCallNode("vector_add_exact", [left, right]), solver, verifier,
            ["rational.exact_arithmetic", "vectors.add.exact_rational", "vectors.verify.exact_operations"], variantKey, difficultyBand, "vector-add");
    }
}

public sealed class ExactVectorDotProductQuestionFactory
{
    public const string FamilyId = "vectors.dot.exact_rational";
    public static readonly SkillId Skill = new("vectors.dot.exact");
    private readonly IMathematicsSolver solver; private readonly IMathematicsVerifier verifier;
    public ExactVectorDotProductQuestionFactory(IMathematicsSolver solver, IMathematicsVerifier verifier) { this.solver = solver ?? throw new ArgumentNullException(nameof(solver)); this.verifier = verifier ?? throw new ArgumentNullException(nameof(verifier)); }
    public VerifiedGeneratedMathematicsProblem Generate(int variantKey, int difficultyBand)
    {
        ExactFunctionsGraphsSequencesGeneration.ValidateDifficulty(difficultyBand);
        var state = ExactFunctionsGraphsSequencesGeneration.Seed(variantKey, 0x299F31D0u);
        var dimension = difficultyBand == 1 ? 2 : ExactFunctionsGraphsSequencesGeneration.Next(ref state, 2, Math.Min(4, difficultyBand + 1));
        var left = ExactLinearAlgebraGeneration.Vector(ref state, dimension, difficultyBand);
        var right = ExactLinearAlgebraGeneration.Vector(ref state, dimension, difficultyBand);
        return ExactLinearAlgebraGeneration.Build(FamilyId, Skill, new FunctionCallNode("vector_dot_exact", [left, right]), solver, verifier,
            ["rational.exact_arithmetic", "vectors.dot.exact_rational", "vectors.verify.exact_operations"], variantKey, difficultyBand, "vector-dot");
    }
}

public sealed class ExactMatrixMultiplyQuestionFactory
{
    public const string FamilyId = "matrices.multiply.exact_rational";
    public static readonly SkillId Skill = new("matrices.multiply.exact");
    private readonly IMathematicsSolver solver; private readonly IMathematicsVerifier verifier;
    public ExactMatrixMultiplyQuestionFactory(IMathematicsSolver solver, IMathematicsVerifier verifier) { this.solver = solver ?? throw new ArgumentNullException(nameof(solver)); this.verifier = verifier ?? throw new ArgumentNullException(nameof(verifier)); }
    public VerifiedGeneratedMathematicsProblem Generate(int variantKey, int difficultyBand)
    {
        ExactFunctionsGraphsSequencesGeneration.ValidateDifficulty(difficultyBand);
        var state = ExactFunctionsGraphsSequencesGeneration.Seed(variantKey, 0x082EFA98u);
        var size = difficultyBand == 3 && ExactFunctionsGraphsSequencesGeneration.Next(ref state, 0, 1) == 1 ? 3 : 2;
        var left = ExactLinearAlgebraGeneration.Matrix(ref state, size, size, difficultyBand);
        var right = ExactLinearAlgebraGeneration.Matrix(ref state, size, size, difficultyBand);
        return ExactLinearAlgebraGeneration.Build(FamilyId, Skill, new FunctionCallNode("matrix_multiply_exact", [left, right]), solver, verifier,
            ["rational.exact_arithmetic", "matrices.multiply.exact_rational", "matrices.verify.exact_operations"], variantKey, difficultyBand, "matrix-multiply");
    }
}

public sealed class ExactMatrixDeterminant2x2QuestionFactory
{
    public const string FamilyId = "matrices.determinant.2x2.exact";
    public static readonly SkillId Skill = new("matrices.determinant.2x2");
    private readonly IMathematicsSolver solver; private readonly IMathematicsVerifier verifier;
    public ExactMatrixDeterminant2x2QuestionFactory(IMathematicsSolver solver, IMathematicsVerifier verifier) { this.solver = solver ?? throw new ArgumentNullException(nameof(solver)); this.verifier = verifier ?? throw new ArgumentNullException(nameof(verifier)); }
    public VerifiedGeneratedMathematicsProblem Generate(int variantKey, int difficultyBand)
    {
        ExactFunctionsGraphsSequencesGeneration.ValidateDifficulty(difficultyBand);
        var state = ExactFunctionsGraphsSequencesGeneration.Seed(variantKey, 0xEC4E6C89u);
        var matrix = ExactLinearAlgebraGeneration.Matrix(ref state, 2, 2, difficultyBand);
        return ExactLinearAlgebraGeneration.Build(FamilyId, Skill, new FunctionCallNode("matrix_determinant_2x2_exact", [matrix]), solver, verifier,
            ["rational.exact_arithmetic", "matrices.determinant.2x2.exact", "matrices.verify.exact_operations"], variantKey, difficultyBand, "matrix-determinant-2x2");
    }
}

public sealed class ExactComplexAddQuestionFactory
{
    public const string FamilyId = "complex.add.exact_rational";
    public static readonly SkillId Skill = new("complex.add.exact");
    private readonly IMathematicsSolver solver; private readonly IMathematicsVerifier verifier;
    public ExactComplexAddQuestionFactory(IMathematicsSolver solver, IMathematicsVerifier verifier) { this.solver = solver ?? throw new ArgumentNullException(nameof(solver)); this.verifier = verifier ?? throw new ArgumentNullException(nameof(verifier)); }
    public VerifiedGeneratedMathematicsProblem Generate(int variantKey, int difficultyBand)
    {
        ExactFunctionsGraphsSequencesGeneration.ValidateDifficulty(difficultyBand);
        var state = ExactFunctionsGraphsSequencesGeneration.Seed(variantKey, 0x452821E6u);
        var left = ExactLinearAlgebraGeneration.Complex(ref state, difficultyBand);
        var right = ExactLinearAlgebraGeneration.Complex(ref state, difficultyBand);
        return ExactLinearAlgebraGeneration.Build(FamilyId, Skill, new FunctionCallNode("complex_add_exact", [left, right]), solver, verifier,
            ["rational.exact_arithmetic", "complex.add.exact_rational", "complex.verify.exact_operations"], variantKey, difficultyBand, "complex-add");
    }
}

public sealed class ExactComplexMultiplyQuestionFactory
{
    public const string FamilyId = "complex.multiply.exact_rational";
    public static readonly SkillId Skill = new("complex.multiply.exact");
    private readonly IMathematicsSolver solver; private readonly IMathematicsVerifier verifier;
    public ExactComplexMultiplyQuestionFactory(IMathematicsSolver solver, IMathematicsVerifier verifier) { this.solver = solver ?? throw new ArgumentNullException(nameof(solver)); this.verifier = verifier ?? throw new ArgumentNullException(nameof(verifier)); }
    public VerifiedGeneratedMathematicsProblem Generate(int variantKey, int difficultyBand)
    {
        ExactFunctionsGraphsSequencesGeneration.ValidateDifficulty(difficultyBand);
        var state = ExactFunctionsGraphsSequencesGeneration.Seed(variantKey, 0x38D01377u);
        var left = ExactLinearAlgebraGeneration.Complex(ref state, difficultyBand);
        var right = ExactLinearAlgebraGeneration.Complex(ref state, difficultyBand);
        return ExactLinearAlgebraGeneration.Build(FamilyId, Skill, new FunctionCallNode("complex_multiply_exact", [left, right]), solver, verifier,
            ["rational.exact_arithmetic", "complex.multiply.exact_rational", "complex.verify.exact_operations"], variantKey, difficultyBand, "complex-multiply");
    }
}

internal static class ExactLinearAlgebraGeneration
{
    public static VectorNode Vector(ref uint state, int dimension, int difficultyBand) => new(Enumerable.Range(0, dimension).Select(_ => Scalar(ref state, difficultyBand)).ToArray());
    public static MatrixNode Matrix(ref uint state, int rows, int columns, int difficultyBand)
    {
        var data = new IReadOnlyList<MathNode>[rows];
        for (var r = 0; r < rows; r++)
        {
            var row = new MathNode[columns];
            for (var c = 0; c < columns; c++) row[c] = Scalar(ref state, difficultyBand);
            data[r] = row;
        }
        return new MatrixNode(data);
    }
    public static FunctionCallNode Complex(ref uint state, int difficultyBand) => new("complex_exact", [Scalar(ref state, difficultyBand), Scalar(ref state, difficultyBand)]);
    private static MathNode Scalar(ref uint state, int difficultyBand)
    {
        var numerator = ExactFunctionsGraphsSequencesGeneration.Next(ref state, -6 - difficultyBand * 2, 6 + difficultyBand * 2);
        if (difficultyBand < 3 || ExactFunctionsGraphsSequencesGeneration.Next(ref state, 0, 1) == 0) return new IntegerNode(new BigInteger(numerator));
        var denominator = ExactFunctionsGraphsSequencesGeneration.Next(ref state, 2, 5);
        return new RationalNode(new ExactRational(new BigInteger(numerator), new BigInteger(denominator)));
    }
    public static VerifiedGeneratedMathematicsProblem Build(string familyId, SkillId skill, MathNode problem, IMathematicsSolver solver, IMathematicsVerifier verifier, IReadOnlyList<string> capabilityIds, int variantKey, int difficultyBand, string operation)
    {
        return ExactFunctionsGraphsSequencesGeneration.SolveVerifyAndBuild(
            familyId, skill, problem, solver, verifier, capabilityIds.Select(id => new CapabilityId(id)).ToArray(), variantKey, difficultyBand,
            new Dictionary<string, string>(StringComparer.Ordinal)
            {
                ["operation"] = operation,
                ["difficultyBand"] = difficultyBand.ToString(CultureInfo.InvariantCulture)
            });
    }
}

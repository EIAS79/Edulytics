using System.Numerics;
using System.Text.Json;
using Edulytics.Core.Mathematics.Ast;
using Edulytics.Core.Mathematics.Domains;
using Edulytics.Core.Mathematics.Solving;
using Edulytics.Services.Mathematics.Generation;
using Edulytics.Services.Mathematics.Solving;
using Edulytics.Services.Mathematics.Verification;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class ExactVectorsMatricesComplexSolverTests
{
    [Fact]
    public void VectorAddition_AndDotProduct_AreExactAndIndependentlyVerified()
    {
        var left = new VectorNode([R(1, 2), I(2), I(-3)]);
        var right = new VectorNode([R(3, 2), I(-1), I(4)]);

        var addRequest = Request(new FunctionCallNode("vector_add_exact", [left, right]));
        var addSolver = new ExactVectorAddSolver();
        var addVerifier = new ExactVectorAddVerifier();
        var add = addSolver.Solve(addRequest);
        Assert.Equal(MathematicsSolveStatus.Solved, add.Status);
        Assert.Equal(new VectorNode([I(2), I(1), I(1)]), add.ExactResult);
        Assert.True(addVerifier.Verify(addRequest, add).IsVerified);

        var dotRequest = Request(new FunctionCallNode("vector_dot_exact", [left, right]));
        var dotSolver = new ExactVectorDotProductSolver();
        var dotVerifier = new ExactVectorDotProductVerifier();
        var dot = dotSolver.Solve(dotRequest);
        Assert.Equal(R(-53, 4), dot.ExactResult);
        Assert.True(dotVerifier.Verify(dotRequest, dot).IsVerified);
    }

    [Fact]
    public void MatrixMultiplication_AndDeterminant_AreExactAndVerified()
    {
        var left = M([I(1), I(2)], [I(3), I(4)]);
        var right = M([I(2), I(0)], [I(1), I(2)]);
        var multiplyRequest = Request(new FunctionCallNode("matrix_multiply_exact", [left, right]));
        var multiplySolver = new ExactMatrixMultiplySolver();
        var multiplyVerifier = new ExactMatrixMultiplyVerifier();
        var product = multiplySolver.Solve(multiplyRequest);
        Assert.Equal(M([I(4), I(4)], [I(10), I(8)]), product.ExactResult);
        Assert.True(multiplyVerifier.Verify(multiplyRequest, product).IsVerified);

        var determinantRequest = Request(new FunctionCallNode("matrix_determinant_2x2_exact", [left]));
        var determinantSolver = new ExactMatrixDeterminant2x2Solver();
        var determinantVerifier = new ExactMatrixDeterminant2x2Verifier();
        var determinant = determinantSolver.Solve(determinantRequest);
        Assert.Equal(I(-2), determinant.ExactResult);
        Assert.True(determinantVerifier.Verify(determinantRequest, determinant).IsVerified);
    }

    [Fact]
    public void ComplexAddition_AndMultiplication_UseCanonicalExactRepresentation()
    {
        var left = C(R(1, 2), I(2));
        var right = C(R(3, 2), I(-1));

        var addRequest = Request(new FunctionCallNode("complex_add_exact", [left, right]));
        var addSolver = new ExactComplexAddSolver();
        var addVerifier = new ExactComplexAddVerifier();
        var add = addSolver.Solve(addRequest);
        Assert.Equal(C(I(2), I(1)), add.ExactResult);
        Assert.True(addVerifier.Verify(addRequest, add).IsVerified);

        var multiplyRequest = Request(new FunctionCallNode("complex_multiply_exact", [left, right]));
        var multiplySolver = new ExactComplexMultiplySolver();
        var multiplyVerifier = new ExactComplexMultiplyVerifier();
        var multiply = multiplySolver.Solve(multiplyRequest);
        Assert.Equal(C(R(11, 4), R(5, 2)), multiply.ExactResult);
        Assert.True(multiplyVerifier.Verify(multiplyRequest, multiply).IsVerified);
    }

    [Fact]
    public void Verifiers_RejectMutatedSolverAnswers()
    {
        var request = Request(new FunctionCallNode("vector_dot_exact", [new VectorNode([I(1), I(2)]), new VectorNode([I(3), I(4)])]));
        var solver = new ExactVectorDotProductSolver();
        var verifier = new ExactVectorDotProductVerifier();
        var result = solver.Solve(request);
        var wrong = result with { ExactResult = I(12), SolutionSet = new FiniteSolutionSet([I(12)]) };
        Assert.False(verifier.Verify(request, wrong).IsVerified);
    }

    [Fact]
    public void InvalidDimensions_AndNonCanonicalComplexValues_FailClosed()
    {
        var vectorMismatch = Request(new FunctionCallNode("vector_add_exact", [new VectorNode([I(1), I(2)]), new VectorNode([I(3)])]));
        Assert.Equal(MathematicsSolveStatus.Unsupported, new ExactVectorAddSolver().Solve(vectorMismatch).Status);

        var matrixMismatch = Request(new FunctionCallNode("matrix_multiply_exact", [M([I(1), I(2)]), M([I(1), I(2)])]));
        Assert.Equal(MathematicsSolveStatus.Unsupported, new ExactMatrixMultiplySolver().Solve(matrixMismatch).Status);

        var nonCanonicalComplex = Request(new FunctionCallNode("complex_add_exact", [new VectorNode([I(1), I(2)]), C(I(3), I(4))]));
        Assert.Equal(MathematicsSolveStatus.Unsupported, new ExactComplexAddSolver().Solve(nonCanonicalComplex).Status);
    }

    [Fact]
    public void MalformedAndOversizedScalars_FailClosedWithoutApproximation()
    {
        var malformed = Request(new FunctionCallNode("vector_dot_exact", [new VectorNode([new RationalNode(default(ExactRational))]), new VectorNode([I(1)])]));
        Assert.Equal(MathematicsSolveStatus.Unsupported, new ExactVectorDotProductSolver().Solve(malformed).Status);

        var huge = new IntegerNode(BigInteger.One << 4096);
        var oversized = Request(new FunctionCallNode("vector_add_exact", [new VectorNode([huge]), new VectorNode([I(1)])]));
        Assert.Equal(MathematicsSolveStatus.ResourceLimit, new ExactVectorAddSolver().Solve(oversized).Status);
    }

    private static MathematicsSolveRequest Request(MathNode problem) => new(problem, [], []);
    private static IntegerNode I(int value) => new(new BigInteger(value));
    private static RationalNode R(int numerator, int denominator) => new(new ExactRational(new BigInteger(numerator), new BigInteger(denominator)));
    private static FunctionCallNode C(MathNode real, MathNode imaginary) => new("complex_exact", [real, imaginary]);
    private static MatrixNode M(params MathNode[][] rows) => new(rows.Select(row => (IReadOnlyList<MathNode>)row).ToArray());
}

public sealed class ExactVectorsMatricesComplexQuestionFactoryTests
{
    [Theory]
    [InlineData(1)] [InlineData(2)] [InlineData(3)]
    public void VectorAddFactory_IsVerifiedAndDeterministic(int band) => AssertDeterministic(
        new ExactVectorAddQuestionFactory(new ExactVectorAddSolver(), new ExactVectorAddVerifier()), (f, s, b) => f.Generate(s, b), band,
        ExactVectorAddQuestionFactory.FamilyId, "vectors.add.exact");

    [Theory]
    [InlineData(1)] [InlineData(2)] [InlineData(3)]
    public void VectorDotFactory_IsVerifiedAndDeterministic(int band) => AssertDeterministic(
        new ExactVectorDotProductQuestionFactory(new ExactVectorDotProductSolver(), new ExactVectorDotProductVerifier()), (f, s, b) => f.Generate(s, b), band,
        ExactVectorDotProductQuestionFactory.FamilyId, "vectors.dot.exact");

    [Theory]
    [InlineData(1)] [InlineData(2)] [InlineData(3)]
    public void MatrixMultiplyFactory_IsVerifiedAndDeterministic(int band) => AssertDeterministic(
        new ExactMatrixMultiplyQuestionFactory(new ExactMatrixMultiplySolver(), new ExactMatrixMultiplyVerifier()), (f, s, b) => f.Generate(s, b), band,
        ExactMatrixMultiplyQuestionFactory.FamilyId, "matrices.multiply.exact");

    [Theory]
    [InlineData(1)] [InlineData(2)] [InlineData(3)]
    public void MatrixDeterminantFactory_IsVerifiedAndDeterministic(int band) => AssertDeterministic(
        new ExactMatrixDeterminant2x2QuestionFactory(new ExactMatrixDeterminant2x2Solver(), new ExactMatrixDeterminant2x2Verifier()), (f, s, b) => f.Generate(s, b), band,
        ExactMatrixDeterminant2x2QuestionFactory.FamilyId, "matrices.determinant.2x2");

    [Theory]
    [InlineData(1)] [InlineData(2)] [InlineData(3)]
    public void ComplexAddFactory_IsVerifiedAndDeterministic(int band) => AssertDeterministic(
        new ExactComplexAddQuestionFactory(new ExactComplexAddSolver(), new ExactComplexAddVerifier()), (f, s, b) => f.Generate(s, b), band,
        ExactComplexAddQuestionFactory.FamilyId, "complex.add.exact");

    [Theory]
    [InlineData(1)] [InlineData(2)] [InlineData(3)]
    public void ComplexMultiplyFactory_IsVerifiedAndDeterministic(int band) => AssertDeterministic(
        new ExactComplexMultiplyQuestionFactory(new ExactComplexMultiplySolver(), new ExactComplexMultiplyVerifier()), (f, s, b) => f.Generate(s, b), band,
        ExactComplexMultiplyQuestionFactory.FamilyId, "complex.multiply.exact");

    private static void AssertDeterministic<TFactory>(TFactory factory, Func<TFactory, int, int, VerifiedGeneratedMathematicsProblem> generate, int band, string family, string skill)
    {
        var first = generate(factory, 20260917, band);
        var second = generate(factory, 20260917, band);
        Assert.Equal(family, first.QuestionFamilyId);
        Assert.Equal(skill, first.Skill.Value);
        Assert.True(first.Verification.IsVerified);
        Assert.Equal(JsonSerializer.Serialize(first.Problem), JsonSerializer.Serialize(second.Problem));
        Assert.Equal(JsonSerializer.Serialize(first.ExpectedAnswer), JsonSerializer.Serialize(second.ExpectedAnswer));
    }
}

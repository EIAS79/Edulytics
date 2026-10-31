using Edulytics.Services.Mathematics;
using Xunit;
using Contract = Edulytics.Services.Mathematics.UsCcssHsAdvAlgFuncSourceResponseContract;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Course-owned exact source-response checks for Coder 8 A-C preparation.
/// No assertion here certifies READY_VERIFIED, authenticated Practice, or
/// teacher feedback persistence; M05/M11/L04/L05/L06/L13 require human review.
/// </summary>
public sealed class UsCcssHsAdvAlgFuncSourceResponseContractTests
{
    private static readonly long[,] A3 =
        { { 1, 1, 1 }, { 2, -1, 1 }, { 1, 2, -1 } };
    private static readonly long[,] Paint =
        { { 2, 0, 1 }, { 1, 4, 0 }, { 0, 2, 1 } };

    [Theory]
    [InlineData("A=[[1,1,1],[2,-1,1],[1,2,-1]];b=[6,3,2]", true)]
    [InlineData("A=[[1,1,1]];b=[6,3,2]", false)]
    [InlineData("A=[[1,1,1],[2,1,1],[1,2,-1]];b=[6,3,2]", false)]
    [InlineData("A=[[1,1,1],[2,-1,1],[1,2,-1]];b=[6,3,]", false)]
    [InlineData("A=<script>;b=[6,3,2]", false)]
    public void M01ChecksCompleteMatrixAndConstantsNotOneCorrectRow(string answer, bool expected)
    {
        Assert.Equal(expected, Contract.VerifyMatrixEquation(answer, A3, [6, 3, 2]));
    }

    [Theory]
    [InlineData("[2,-1,1]", true)]
    [InlineData("[2,-2/2,+1]", true)]
    [InlineData("[2,1,-1]", false)]
    [InlineData("[2,-,1]", false)]
    [InlineData("[2,-1]", false)]
    public void M09ChecksSignedCoefficientRow(string answer, bool expected)
    {
        Assert.Equal(expected, Contract.VerifyCoefficientRow(answer, [2, -1, 1]));
    }

    [Theory]
    [InlineData("A^-1=[[1,-1],[-1,2]];x=[2,1]", true)]
    [InlineData("A^-1=[[1,-1],[-1,2]];x=[4/2,2/2]", true)]
    [InlineData("x=[2,1]", false)]
    [InlineData("A^-1=[[1,0],[0,1]];x=[2,1]", false)]
    [InlineData("A^-1=[[1,-1],[-1,2]];x=[1,2]", false)]
    [InlineData("A^-1=[[1,-1],[-1,2]];x=[2/0,1]", false)]
    public void M02RequiresExactInverseAndSolution(string answer, bool expected)
    {
        Assert.Equal(expected,
            Contract.VerifyInverseAndSolution(answer, new long[,] { { 2, 1 }, { 1, 1 } }, [5, 3]));
    }

    [Theory]
    [InlineData("A^-1=[[2/5,1/5,-2/5],[-1/10,1/5,1/10],[1/5,-2/5,4/5]];x=[12,2,6]", true)]
    [InlineData("A^-1=[[4/10,2/10,-4/10],[-1/10,2/10,1/10],[2/10,-4/10,8/10]];x=[12,2,6]", true)]
    [InlineData("A^-1=[[2/5,1/5,-2/5],[-1/10,1/5,1/10],[1/5,-2/5,4/5]];x=[2,12,6]", false)]
    [InlineData("A^-1=[[1,0],[0,1]];x=[12,2,6]", false)]
    [InlineData("A^-1=[[2/0,1/5,-2/5],[-1/10,1/5,1/10],[1/5,-2/5,4/5]];x=[12,2,6]", false)]
    public void AllSystemsGoPaintingReceiptInverseAndAllPurchases(string answer, bool expected)
    {
        Assert.Equal(expected, Contract.VerifyInverseAndSolution(answer, Paint, [30, 20, 10]));
    }

    [Theory]
    [InlineData("A^-1=[[-1/7,3/7,2/7],[3/7,-2/7,1/7],[5/7,-1/7,-3/7]];x=[1,2,3]", true)]
    [InlineData("A^-1=[[-2/14,6/14,4/14],[6/14,-4/14,2/14],[10/14,-2/14,-6/14]];x=[7/7,14/7,21/7]", true)]
    [InlineData("A^-1=[[-1/7,3/7,2/7],[3/7,2/7,1/7],[5/7,-1/7,-3/7]];x=[1,2,3]", false)]
    [InlineData("A^-1=[[-1/7,3/7,2/7],[3/7,-2/7,1/7],[5/7,-1/7,-3/7]];x=[1,3,2]", false)]
    [InlineData("A^-1=[[-1/7,3/7,2/7],[3/7,-2/7,1/7]];x=[1,2,3]", false)]
    [InlineData("A^-1=[[-1/0,3/7,2/7],[3/7,-2/7,1/7],[5/7,-1/7,-3/7]];x=[1,2,3]", false)]
    [InlineData("x=[1,2,3]", false)]
    public void M03FixtureThreeByThreeInverseRequiresCompleteRationalMatrixAndSolution(
        string answer, bool expected)
    {
        // Exact M03 fixture A3; not the separately verified source painting example.
        // Author-only regression contract; shared selected Practice is still pending.
        Assert.Equal(expected, Contract.VerifyInverseAndSolution(answer, A3, [6, 3, 2]));
    }

    [Theory]
    [InlineData("A^-1=[[-5/3,2/3,4/3],[1/3,-1/3,1/3],[1,0,-1]];x=[2,4,3]", true)]
    [InlineData("A^-1=[[-10/6,4/6,8/6],[2/6,-2/6,2/6],[3/3,0,-3/3]];x=[4/2,8/2,9/3]", true)]
    [InlineData("A^-1=[[-5/3,2/3,4/3],[1/3,-1/3,1/3],[1,0,-1]];x=[2,3,4]", false)]
    [InlineData("A^-1=[[-5/3,2/3,4/3],[1/3,-1/3,1/3],[1,0,1]];x=[2,4,3]", false)]
    [InlineData("A^-1=[[-5/3,2/3,4/3],[1/3,-1/3,1/3]];x=[2,4,3]", false)]
    [InlineData("x=[2,4,3]", false)]
    public void AllSystemsGoOriginalTaskSixRequiresInverseAndAllThreeEquations(
        string answer, bool expected)
    {
        // MVP 10.6H original task 6 (teacher PDF p.51): technology-aided
        // inverse for x+2y+2z=16, 2x+y+3z=17, x+2y+z=13.
        var coefficients = new long[,]
        {
            { 1, 2, 2 },
            { 2, 1, 3 },
            { 1, 2, 1 }
        };
        Assert.Equal(expected,
            Contract.VerifyInverseAndSolution(answer, coefficients, [16, 17, 13]));
    }

    [Fact]
    public void LogDomainsAreRealAndCrossBaseOrderingChangesAtOne()
    {
        Assert.True(Contract.IsValidRealLog(0.5, 4));
        Assert.True(Contract.IsValidRealLog(2, 1.0 / 9));
        Assert.False(Contract.IsValidRealLog(1, 5));
        Assert.False(Contract.IsValidRealLog(0, 8));
        Assert.False(Contract.IsValidRealLog(-2, 8));
        Assert.False(Contract.IsValidRealLog(2, 0));
        Assert.False(Contract.IsValidRealLog(2, -2));
        Assert.Equal("<", Contract.CrossBaseTwoThreeRelation(0.5));
        Assert.Equal("=", Contract.CrossBaseTwoThreeRelation(1));
        Assert.Equal(">", Contract.CrossBaseTwoThreeRelation(3));
        Assert.True(Contract.VerifyL06ClosedRelations("<,<,=,>"));
        Assert.False(Contract.VerifyL06ClosedRelations("<,<,<,<"));
        Assert.False(Contract.VerifyL06ClosedRelations("<"));
    }

    [Fact]
    public void TeacherReasoningMustNotBePresentedAsAutomaticallyGraded()
    {
        foreach (var id in new[] { "M05", "M11", "L04", "L05", "L06", "L13" })
            Assert.True(Contract.RequiresTeacherReview(id), id);
        foreach (var id in new[] { "M01", "M09", "L01", "L03", "L15" })
            Assert.False(Contract.RequiresTeacherReview(id), id);
    }

    [Theory]
    [InlineData("x=[1,2,3]", true)]
    [InlineData("[7/7,14/7,21/7]", true)]
    [InlineData("x=[1,3,2]", false)]
    [InlineData("x=[1,2]", false)]
    [InlineData("[1,2,not-a-number]", false)]
    [InlineData("[1/0,2,3]", false)]
    public void M04RowReducedVectorMustSatisfyAllOriginalEquations(
        string answer, bool expected)
    {
        // The student explanation of each reversible row operation is
        // independently teacher-reviewed; this checks only the closed vector.
        Assert.Equal(expected, Contract.VerifySystemSolution(answer, A3, [6, 3, 2]));
    }

    [Fact]
    public void M05M06ClassificationsDistinguishDependentAndInconsistent()
    {
        var singular = new long[,] { { 1, 1 }, { 2, 2 } };
        Assert.True(Contract.VerifySystemClassification(
            "infinitely many", singular, [2, 4]));
        Assert.True(Contract.VerifySystemClassification(
            "no solution", singular, [2, 5]));
        Assert.False(Contract.VerifySystemClassification(
            "infinite", singular, [2, 5]));
        Assert.False(Contract.VerifySystemClassification(
            "unique", singular, [2, 4]));
        // M05 parameterisation and reasoning cannot be certified by this
        // classification answer; preserve the original 4-point human rubric.
    }

    [Fact]
    public void ZeroRankCoefficientBoundaryDoesNotMisclassifyAContradiction()
    {
        var zero = new long[,] { { 0, 0 }, { 0, 0 } };
        Assert.True(Contract.VerifySystemClassification(
            "none", zero, [0, 3]));
        Assert.False(Contract.VerifySystemClassification(
            "infinite", zero, [0, 3]));
        Assert.True(Contract.VerifySystemClassification(
            "infinite", zero, [0, 0]));
    }

    [Fact]
    public void Invertible2x2AndLargeCoefficientsUseExactIntegerProducts()
    {
        var invertible = new long[,] { { 2, 1 }, { 1, 1 } };
        Assert.True(Contract.VerifySystemClassification(
            "unique", invertible, [5, 3]));
        Assert.True(Contract.VerifyInverseEligibility(
            "invertible", invertible, rhsLength: 2));
        var large = new long[,] { { long.MaxValue, 1 }, { 1, long.MaxValue } };
        Assert.True(Contract.VerifyInverseEligibility(
            "invertible", large));
    }

    [Fact]
    public void M07M08M10InverseEligibilityUsesDistinctFailureCategories()
    {
        var singular = new long[,] { { 1, 2 }, { 2, 4 } };
        var square = new long[,] { { 2, 1 }, { 1, 1 } };
        var nonsquare = new long[,] { { 1, 2, 3 }, { 4, 5, 6 } };
        Assert.True(Contract.VerifyInverseEligibility("singular", singular));
        Assert.False(Contract.VerifyInverseEligibility("invertible", singular));
        Assert.True(Contract.VerifyInverseEligibility(
            "invalid dimensions", square, rhsLength: 3));
        Assert.False(Contract.VerifyInverseEligibility(
            "invertible", square, rhsLength: 3));
        Assert.True(Contract.VerifyInverseEligibility("not_square", nonsquare));
        Assert.False(Contract.VerifyInverseEligibility("singular", nonsquare));
        Assert.False(Contract.VerifyInverseEligibility("not_square", square));
        Assert.False(Contract.VerifyInverseEligibility(
            "<script>", square));
    }

    [Fact]
    public void ThreeByThreeInverseEligibilityDistinguishesSingularity()
    {
        Assert.True(Contract.VerifyInverseEligibility("invertible", Paint));
        var singular = new long[,]
        {
            { 1, 2, 3 },
            { 2, 4, 6 },
            { 0, 1, 1 }
        };
        Assert.True(Contract.VerifyInverseEligibility("singular", singular));
        Assert.False(Contract.VerifyInverseEligibility("invertible", singular));
    }

    [Fact]
    public void L01ExactMatrixContractRemainsAuditOnlyUntilApproved()
    {
        const string lesson = "PED:US-CCSS-MATH:HS:FOURTH:ADV-ALG-FUNC:U01:L01";
        Assert.True(
            Edulytics.Core.Mathematics.Practice.LessonPracticeContractRegistry.TryResolve(
                lesson, out var pending));
        Assert.NotNull(pending);
        Assert.Equal("INTEGRATION_PENDING", pending!.Readiness);
        Assert.Equal("CODER8_L01_MATRIX_FULL_SYSTEM_AND_TEACHER_REASONING",
            pending.Mechanic);
        Assert.Contains("usccss.hs.matrix.inverse_eligibility",
            pending.AllowedQuestionFamilies);
        Assert.DoesNotContain(pending.AllowedQuestionFamilies,
            family => family.Contains("one-step-formula-equation",
                StringComparison.Ordinal));
        Assert.Single(Edulytics.Core.Mathematics.Practice
            .LessonPracticeContractRegistry.Coder8L01PendingForAudit);
        Assert.DoesNotContain(Edulytics.Core.Mathematics.Practice
            .LessonPracticeContractRegistry.All,
            item => item.LessonCode == lesson);
        Assert.False(Edulytics.Core.Mathematics.Practice
            .LessonPracticeCapabilityResolver.TryResolve(lesson, out var selected));
        Assert.Null(selected);
    }

    [Fact]
    public void Coder8PendingContractDoesNotChangeExistingGradeOneReadyRoute()
    {
        Assert.True(Edulytics.Core.Mathematics.Practice
            .LessonPracticeCapabilityResolver.TryResolve(
                "PED:US-CCSS-MATH:G1:U01:L01", out var existing));
        Assert.NotNull(existing);
        Assert.Equal("READY_VERIFIED", existing!.Readiness);
    }
}

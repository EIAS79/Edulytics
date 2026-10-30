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
    public void M03SourcePaintingInverseAndAllReceipts(string answer, bool expected)
    {
        Assert.Equal(expected, Contract.VerifyInverseAndSolution(answer, Paint, [30, 20, 10]));
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
}

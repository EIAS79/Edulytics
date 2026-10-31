using Edulytics.Core.Mathematics.Practice;

namespace Edulytics.Tests.Acceptance;

public sealed class UsCcssAlg2SequencePracticeMathTests
{
    [Fact]
    public void Hanoi_UsesExactBigIntegerAndSafeEquivalentExpressions()
    {
        Assert.True(UsCcssAlg2SequencePracticeMath.GradeHanoiMinimumMoves(3, "7"));
        Assert.True(UsCcssAlg2SequencePracticeMath.GradeHanoiMinimumMoves(5, "2^5-1"));
        Assert.True(UsCcssAlg2SequencePracticeMath.GradeHanoiMinimumMoves(5, " 2 ^ 5 - 1 "));
        Assert.True(UsCcssAlg2SequencePracticeMath.GradeHanoiMinimumMoves(5, "1+2+4+8+16"));
        Assert.False(UsCcssAlg2SequencePracticeMath.GradeHanoiMinimumMoves(5, "3 1"));
        Assert.False(UsCcssAlg2SequencePracticeMath.GradeHanoiMinimumMoves(4, "1 5"));
        Assert.False(UsCcssAlg2SequencePracticeMath.GradeHanoiMinimumMoves(64, "18 446744073709551615"));
        Assert.True(UsCcssAlg2SequencePracticeMath.GradeHanoiMinimumMoves(64, "18446744073709551615"));
        Assert.False(UsCcssAlg2SequencePracticeMath.GradeHanoiMinimumMoves(64, "18446744073709552000"));
        Assert.False(UsCcssAlg2SequencePracticeMath.GradeHanoiMinimumMoves(5, "2^4-1"));
        Assert.False(UsCcssAlg2SequencePracticeMath.GradeHanoiMinimumMoves(5, "31.0"));
        Assert.False(UsCcssAlg2SequencePracticeMath.GradeHanoiMinimumMoves(5, "Infinity"));
        Assert.False(UsCcssAlg2SequencePracticeMath.GradeHanoiMinimumMoves(0, "0"));
        Assert.False(UsCcssAlg2SequencePracticeMath.GradeHanoiMinimumMoves(65, "0"));
        Assert.False(UsCcssAlg2SequencePracticeMath.GradeHanoiMinimumMoves(5, "1+2+abc+8+16"));
    }

    [Fact]
    public void Checker_AcceptsBoundedEquivalentExpressionAndRejectsInvalidResponses()
    {
        Assert.True(UsCcssAlg2SequencePracticeMath.GradeCheckerMinimumMoves(1, "3"));
        Assert.True(UsCcssAlg2SequencePracticeMath.GradeCheckerMinimumMoves(3, "15"));
        Assert.True(UsCcssAlg2SequencePracticeMath.GradeCheckerMinimumMoves(3, "3*(3+2)"));
        Assert.True(UsCcssAlg2SequencePracticeMath.GradeCheckerMinimumMoves(3, "3 ( 3 + 2 )"));
        Assert.True(UsCcssAlg2SequencePracticeMath.GradeCheckerMinimumMoves(7, "63"));
        Assert.False(UsCcssAlg2SequencePracticeMath.GradeCheckerMinimumMoves(3, "7"));
        Assert.False(UsCcssAlg2SequencePracticeMath.GradeCheckerMinimumMoves(3, "3*(4+2)"));
        Assert.False(UsCcssAlg2SequencePracticeMath.GradeCheckerMinimumMoves(3, "4*(4+2)"));
        Assert.False(UsCcssAlg2SequencePracticeMath.GradeCheckerMinimumMoves(3, "3*(3+2);alert(1)"));
        Assert.False(UsCcssAlg2SequencePracticeMath.GradeCheckerMinimumMoves(3, "15.5"));
        Assert.False(UsCcssAlg2SequencePracticeMath.GradeCheckerMinimumMoves(0, "0"));
    }

    [Fact]
    public void RationalGeometric_HandlesSourceHalvingAndEquivalentFractions()
    {
        Assert.True(UsCcssAlg2SequencePracticeMath.GradeGeometricTerm("1/2", "1/2", 1, 4, "1/16"));
        Assert.True(UsCcssAlg2SequencePracticeMath.GradeGeometricTerm("1/2", "1/2", 1, 4, "0.0625"));
        Assert.True(UsCcssAlg2SequencePracticeMath.GradeGeometricTerm("1/2", "1/2", 1, 4, "2/32"));
        Assert.True(UsCcssAlg2SequencePracticeMath.GradeGeometricTerm("80", "1/2", 0, 6, "1.25"));
        Assert.True(UsCcssAlg2SequencePracticeMath.GradeGeometricTerm("1", "2", 0, 6, "64"));
        Assert.False(UsCcssAlg2SequencePracticeMath.GradeGeometricTerm("1/2", "1/2", 1, 4, "1/8"));
        Assert.False(UsCcssAlg2SequencePracticeMath.GradeGeometricTerm("1", "2", 0, -1, "1/2"));
        Assert.False(UsCcssAlg2SequencePracticeMath.GradeGeometricTerm("1", "0/0", 0, 2, "0"));
        Assert.False(UsCcssAlg2SequencePracticeMath.GradeGeometricTerm("1", "2", 0, 2, "NaN"));
    }

    [Fact]
    public void Affine_BackwardAndForwardIndexing()
    {
        Assert.True(UsCcssAlg2SequencePracticeMath.GradeAffineRecurrenceTerm("-7", 4, 1, -2, 3, "2"));
        Assert.True(UsCcssAlg2SequencePracticeMath.GradeAffineRecurrenceTerm("-7", 4, 2, -2, 3, "-1"));
        Assert.True(UsCcssAlg2SequencePracticeMath.GradeAffineRecurrenceTerm("-7", 4, 3, -2, 3, "5"));
        Assert.True(UsCcssAlg2SequencePracticeMath.GradeAffineRecurrenceTerm("-7", 4, 5, -2, 3, "17"));
        Assert.True(UsCcssAlg2SequencePracticeMath.GradeAffineRecurrenceTerm("-7", 4, 7, -2, 3, "65"));
        Assert.False(UsCcssAlg2SequencePracticeMath.GradeAffineRecurrenceTerm("-7", 4, 3, -2, 3, "-5"));
        Assert.False(UsCcssAlg2SequencePracticeMath.GradeAffineRecurrenceTerm("-7", 4, 1, 0, 3, "2"));
        Assert.False(UsCcssAlg2SequencePracticeMath.GradeAffineRecurrenceTerm("-7", 4, 7, -2, 3, "1/0"));
    }

    [Fact]
    public void ArithmeticSequence_WithFractionalAndSkippedIndices()
    {
        Assert.True(UsCcssAlg2SequencePracticeMath.GradeArithmeticTerm("1", "-3/4", 1, 5, "-2"));
        Assert.True(UsCcssAlg2SequencePracticeMath.GradeArithmeticTerm("1", "-0.75", 1, 5, "-8/4"));
        Assert.True(UsCcssAlg2SequencePracticeMath.GradeArithmeticTerm("2", "4", 1, 5, "18"));
        Assert.False(UsCcssAlg2SequencePracticeMath.GradeArithmeticTerm("1", "-3/4", 1, 5, "-1.25"));
        Assert.False(UsCcssAlg2SequencePracticeMath.GradeArithmeticTerm("1", "0/0", 1, 5, "2"));
    }

    [Fact]
    public void MonthlyInterest_UsesExactSourceIndexAndDeclaredDisplayRounding()
    {
        Assert.True(UsCcssAlg2SequencePracticeMath.GradeUnroundedMonthlyInterestAtDisplay(1, "1000"));
        Assert.True(UsCcssAlg2SequencePracticeMath.GradeUnroundedMonthlyInterestAtDisplay(3, "1040.40"));
        Assert.True(UsCcssAlg2SequencePracticeMath.GradeUnroundedMonthlyInterestAtDisplay(8, "1148.69"));
        Assert.False(UsCcssAlg2SequencePracticeMath.GradeUnroundedMonthlyInterestAtDisplay(8, "1148.68"));
        Assert.False(UsCcssAlg2SequencePracticeMath.GradeUnroundedMonthlyInterestAtDisplay(0, "1000"));
    }

    [Fact]
    public void Sierpinski_SourceStepZeroThroughFive()
    {
        Assert.True(UsCcssAlg2SequencePracticeMath.GradeSierpinskiCount(0, "1"));
        Assert.True(UsCcssAlg2SequencePracticeMath.GradeSierpinskiCount(5, "243"));
        Assert.True(UsCcssAlg2SequencePracticeMath.GradeSierpinskiPieceArea(0, "256"));
        Assert.True(UsCcssAlg2SequencePracticeMath.GradeSierpinskiPieceArea(1, "64"));
        Assert.True(UsCcssAlg2SequencePracticeMath.GradeSierpinskiPieceArea(5, "0.25"));
        Assert.True(UsCcssAlg2SequencePracticeMath.GradeSierpinskiPieceArea(5, "1/4"));
        Assert.False(UsCcssAlg2SequencePracticeMath.GradeSierpinskiPieceArea(5, "256"));
        Assert.False(UsCcssAlg2SequencePracticeMath.GradeSierpinskiCount(13, "1594323"));
        Assert.False(UsCcssAlg2SequencePracticeMath.GradeSierpinskiPieceArea(-1, "1024"));
    }
}

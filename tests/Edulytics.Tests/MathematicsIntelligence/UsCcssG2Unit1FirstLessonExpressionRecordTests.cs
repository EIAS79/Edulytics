using Edulytics.Core.Mathematics.Practice;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Stage C independent authoring tests. NOT executed until Stage D.
/// </summary>
public sealed class UsCcssG2Unit1FirstLessonExpressionRecordTests
{
    [Theory]
    [InlineData(0, "9 - 9")]
    [InlineData(0, "0 + 0")]
    [InlineData(5, "10 - 5")]
    [InlineData(5, "3 + 2")]
    [InlineData(5, "1 + 4")]
    [InlineData(10, "7 + 3")]
    [InlineData(10, "10 + 0")]
    [InlineData(10, "0 + 10")]
    [InlineData(3, "10 − 7")]
    public void DiverseSourceAcceptableExpressionsMatchTheSameMarkedTarget(
        int target, string input)
    {
        var result=UsCcssG2Unit1FirstLessonExpressionRecord.Check(target,input);
        Assert.True(result.ExpressionValid);
        Assert.True(result.TargetMatched);
        Assert.Equal(target,result.ComputedValue);
    }

    [Theory]
    [InlineData(5, "4 + 2")]
    [InlineData(3, "2 + 4")]
    [InlineData(8, "10 - 7")]
    [InlineData(4, "10 + 5")]
    [InlineData(5, "3 - 7")]
    public void ValidButWrongTargetOrOutOfRangeOperationsAreNotAccepted(
        int target, string input)
    {
        var result=UsCcssG2Unit1FirstLessonExpressionRecord.Check(target,input);
        Assert.False(result.TargetMatched);
    }

    [Theory]
    [InlineData("11 + 0")]
    [InlineData("8 +")]
    [InlineData("8 x 2")]
    [InlineData("8 + 2 + 0")]
    [InlineData("2 - 20")]
    [InlineData("x + 1")]
    public void OnlyExactTwoCardArithmeticCanBeVerified(string invalid)
    {
        Assert.False(UsCcssG2Unit1FirstLessonExpressionRecord
            .Check(5,invalid).ExpressionValid);
    }

    [Fact]
    public void MultipleExpressionsForOneTargetAreAllKeptInsteadOfSingleAnswer()
    {
        var results=UsCcssG2Unit1FirstLessonExpressionRecord.CheckAll(5,
            new[]{"10 - 5","3 + 2","1 + 4","5 + 0"});
        Assert.Equal(4,results.Count);
        Assert.All(results,item=>Assert.True(item.TargetMatched));
    }

    [Fact]
    public void TargetMustBelongToOriginalCheckOffBoard()
    {
        Assert.False(UsCcssG2Unit1FirstLessonExpressionRecord
            .Check(-1,"1 + 0").ExpressionValid);
        Assert.False(UsCcssG2Unit1FirstLessonExpressionRecord
            .Check(11,"10 + 1").ExpressionValid);
    }
}

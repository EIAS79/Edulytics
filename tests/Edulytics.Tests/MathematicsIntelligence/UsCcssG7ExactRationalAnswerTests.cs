using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// G7 Stage D regression: verify rational equality without rounding a
/// recurring fraction and without accepting malformed or out-of-bounds input.
/// </summary>
public sealed class UsCcssG7ExactRationalAnswerTests
{
    [Theory]
    [InlineData("1/12", 1, 12)]
    [InlineData("2/24", 1, 12)]
    [InlineData("5000/21", 5000, 21)]
    [InlineData("25000000/311", 25000000, 311)]
    [InlineData("4.50", 9, 2)]
    [InlineData("5/3", 5, 3)]
    [InlineData("0.125", 1, 8)]
    public void ExactMathematicalValuesAreAccepted(
        string answer, long numerator, long denominator)
    {
        Assert.True(G7ExactRationalAnswer.Matches(answer, numerator, denominator));
    }

    [Theory]
    [InlineData("1/0", 1, 12)]
    [InlineData("1/13", 1, 12)]
    [InlineData("0.0833333333333333333333333333", 1, 12)]
    [InlineData("-1/12", 1, 12)]
    [InlineData("1/-12", 1, 12)]
    [InlineData("1/12/1", 1, 12)]
    [InlineData("1e0", 1, 1)]
    [InlineData("100000001", 100000001, 1)]
    [InlineData("25cm", 25, 1)]
    public void IncorrectOrUnboundedInputsAreRejected(
        string answer, long numerator, long denominator)
    {
        Assert.False(G7ExactRationalAnswer.Matches(answer, numerator, denominator));
    }
}
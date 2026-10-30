using System.Reflection;
using Edulytics.Core.Mathematics.Practice;

namespace Edulytics.Tests.Acceptance;

/// <summary>
/// Course-owned adapter tests are authored, NOT RUN in the Unit-First A-C cycle.
/// These inspect the internal-only adapter without exposing teacher answers
/// or amending the central registry.
/// </summary>
public sealed class UsCcssAlg2U01L01PracticeAdapterTests
{
    private static readonly Type Adapter =
        typeof(UsCcssAlg2SequencePracticeMath).Assembly.GetType(
            "Edulytics.Core.Mathematics.Practice.UsCcssAlg2U01L01PracticeAdapter",
            throwOnError: true)!;
    private static readonly Type Variant =
        Adapter.GetNestedType("Variant", BindingFlags.NonPublic)!;
    private static readonly MethodInfo Create =
        Adapter.GetMethod("Create", BindingFlags.NonPublic | BindingFlags.Static)!;
    private static readonly MethodInfo Evaluate =
        Adapter.GetMethod("Evaluate", BindingFlags.NonPublic | BindingFlags.Static)!;

    private static object Item(string familyVariant, int parameter) =>
        Create.Invoke(null, [Enum.Parse(Variant, familyVariant), parameter])!;

    private static string Verdict(object item, string answer) =>
        Evaluate.Invoke(null, [item, answer])!.ToString()!;

    private static string? Expected(object item) =>
        item.GetType().GetProperty("ExpectedAnswer")!.GetValue(item)?.ToString();

    [Theory]
    [InlineData("HanoiMoves", 1, "1")]
    [InlineData("HanoiMoves", 5, "31")]
    [InlineData("HanoiMoves", 64, "18446744073709551615")]
    [InlineData("CheckerMovesOptional", 7, "63")]
    [InlineData("DoubleMinusOne", 1, "3")]
    [InlineData("DoubleMinusOne", 5, "33")]
    [InlineData("Halve", 4, "1/16")]
    [InlineData("AffineReverse", 1, "2")]
    [InlineData("AffineReverse", 7, "65")]
    [InlineData("Fibonacci", 1, "0")]
    [InlineData("Fibonacci", 10, "34")]
    public void DeterministicSourceQuestions_ProduceCorrectCanonicalAnswers(
        string variant, int parameter, string expected)
    {
        object item = Item(variant, parameter);
        Assert.Equal(expected, Expected(item));
        Assert.Equal("Correct", Verdict(item, expected));
    }

    [Fact]
    public void RejectsRoundedOrMalformedAndAcceptsMathEquivalentResponses()
    {
        Assert.Equal("Incorrect", Verdict(Item("HanoiMoves", 64), "18446744073709552000"));
        Assert.Equal("Incorrect", Verdict(Item("HanoiMoves", 5), "NaN"));
        Assert.Equal("Correct", Verdict(Item("HanoiMoves", 5), "2^5-1"));
        Assert.Equal("Correct", Verdict(Item("HanoiMoves", 5), "1+2+4+8+16"));
        Assert.Equal("Correct", Verdict(Item("Halve", 4), "0.0625"));
        Assert.Equal("Correct", Verdict(Item("Halve", 4), "2/32"));
        Assert.Equal("Incorrect", Verdict(Item("AffineReverse", 1), "-2"));
    }

    [Theory]
    [InlineData("JadaReasoning")]
    [InlineData("NoahReasoningOptional")]
    [InlineData("TwoDistinctRules")]
    public void OpenReasoningRequiresTeacherReview(string variant)
    {
        object item = Item(variant, 1);
        Assert.Null(Expected(item));
        Assert.Equal("TeacherReviewRequired", Verdict(item, "a learner justification"));
    }

    [Fact]
    public void InvalidDomainCannotGenerate()
    {
        Assert.Throws<TargetInvocationException>(() => Item("HanoiMoves", 65));
        Assert.Throws<TargetInvocationException>(() => Item("HanoiMoves", 0));
        Assert.Throws<TargetInvocationException>(() => Item("CheckerMovesOptional", 8));
        Assert.Throws<TargetInvocationException>(() => Item("Fibonacci", 11));
    }
}

using System.Reflection;
using System.Text.Json;
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

    [Fact]
    public void LearnerProjectionAndDirectCandidateJson_DoNotLeakCanonicalAnswerOrTeacherRubric()
    {
        object candidate = Item("HanoiMoves", 64);
        var projection = Adapter.GetMethod("ProjectForLearner",
            BindingFlags.NonPublic | BindingFlags.Static)!;
        object learner = projection.Invoke(null, [candidate])!;
        string studentJson = JsonSerializer.Serialize(learner);
        string accidentalCandidateJson = JsonSerializer.Serialize(candidate);
        string accidentalRecordLog = candidate.ToString()!;

        foreach (var json in new[] { studentJson, accidentalCandidateJson, accidentalRecordLog })
        {
            Assert.DoesNotContain("ExpectedAnswer", json, StringComparison.Ordinal);
            Assert.DoesNotContain("TeacherRubric", json, StringComparison.Ordinal);
            Assert.DoesNotContain("18446744073709551615", json, StringComparison.Ordinal);
        }
        Assert.Contains("Prompt", studentJson, StringComparison.Ordinal);
        Assert.Contains("FamilyId", studentJson, StringComparison.Ordinal);
        Assert.Contains("Parameters", studentJson, StringComparison.Ordinal);
        Assert.Equal("64", learner.GetType()
            .GetProperty("Parameters")!
            .GetValue(learner) is IReadOnlyDictionary<string, int> parameters
                ? parameters["n"].ToString()
                : null);
    }

    [Theory]
    [InlineData("usccss.alg2.u01.l01.hanoi_min_moves", 0, 64)]
    [InlineData("usccss.alg2.u01.l01.hanoi_min_moves", 1, 1)]
    [InlineData("usccss.alg2.u01.l01.recursive_sequence", 0, 5)]
    [InlineData("usccss.alg2.u01.l01.recursive_sequence", 1, 4)]
    [InlineData("usccss.alg2.u01.l01.recursive_sequence", 2, 7)]
    [InlineData("usccss.alg2.u01.l01.recursive_sequence", 3, 10)]
    [InlineData("usccss.alg2.u01.l01.recursive_sequence", 4, 1)]
    [InlineData("usccss.alg2.u01.l01.checker_jump_optional", 0, 7)]
    [InlineData("usccss.alg2.u01.l01.checker_jump_optional", 1, 1)]
    public void StringFamilyAndIntegerVariantDispatch_UsesExactL01Scope(
        string family, int slot, int parameter)
    {
        var dispatch = Adapter.GetMethod("TryCreateForFamily",
            BindingFlags.NonPublic | BindingFlags.Static)!;
        object?[] args = [family, slot, parameter, null];
        Assert.True((bool)dispatch.Invoke(null, args)!);
        Assert.NotNull(args[3]);
        Assert.Equal(family, args[3]!.GetType()
            .GetProperty("FamilyId")!.GetValue(args[3]));
    }

    [Theory]
    [InlineData("usccss.alg2.u01.l01.hanoi_min_moves", 1)]
    [InlineData("usccss.alg2.u01.l01.checker_jump_optional", 1)]
    [InlineData("usccss.alg2.u01.l01.recursive_sequence", 4)]
    public void TeacherScoredItems_CannotEnterExactAutoGrader(string family, int slot)
    {
        var onlyAuto = Adapter.GetMethod("TryCreateAutoGradableForFamily",
            BindingFlags.NonPublic | BindingFlags.Static)!;
        object?[] args = [family, slot, 1, null];
        Assert.False((bool)onlyAuto.Invoke(null, args)!);
        Assert.Null(args[3]);
        args = [family, 0, 1, null];
        Assert.True((bool)onlyAuto.Invoke(null, args)!);
        Assert.NotNull(args[3]);
    }

    [Fact]
    public void ForgedQuestionFamilyOrExpectedAnswer_IsRejected()
    {
        object canonical = Item("HanoiMoves", 5);
        Type type = canonical.GetType();
        var constructor = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance)
            .Single(x => x.GetParameters().Length == 9);
        object?[] values = constructor.GetParameters()
            .Select(p => type.GetProperty(p.Name!)!.GetValue(canonical)).ToArray();
        values[1] = "usccss.alg2.u01.l99.hanoi_min_moves";
        Assert.Equal("Incorrect", Verdict(constructor.Invoke(values), "31"));
        values[1] = "usccss.alg2.u01.l01.hanoi_min_moves";
        values[6] = "999";
        Assert.Equal("Incorrect", Verdict(constructor.Invoke(values), "31"));
    }

    [Fact]
    public void UnknownFamilyOrInvalidParameter_FailsClosed()
    {
        var dispatch = Adapter.GetMethod("TryCreateForFamily",
            BindingFlags.NonPublic | BindingFlags.Static)!;
        foreach (object?[] args in new object?[][]
        {
            ["usccss.alg2.u01.l01.hanoi_min_moves", 0, 65, null],
            ["usccss.alg2.u01.l01.recursive_sequence", 3, 11, null],
            ["usccss.alg2.u01.l01.recursive_sequence", 10, 1, null],
            ["usccss.alg2.u01.l99.hanoi_min_moves", 0, 5, null]
        })
        {
            Assert.False((bool)dispatch.Invoke(null, args)!);
            Assert.Null(args[3]);
        }
    }

    [Theory]
    [InlineData("usccss.alg2.u01.l01.hanoi_min_moves", 0, 64,
        "18446744073709551615")]
    [InlineData("usccss.alg2.u01.l01.checker_jump_optional", 0, 7, "63")]
    [InlineData("usccss.alg2.u01.l01.recursive_sequence", 0, 5, "33")]
    [InlineData("usccss.alg2.u01.l01.recursive_sequence", 1, 4, "1/16")]
    [InlineData("usccss.alg2.u01.l01.recursive_sequence", 2, 1, "2")]
    [InlineData("usccss.alg2.u01.l01.recursive_sequence", 3, 10, "34")]
    public void ServiceCompatibleNumericInterface_GeneratesAndGradesExact(
        string family, int slot, int parameter, string expected)
    {
        Assert.True(UsCcssAlg2U01L01PracticeAdapter.SupportsFamily(family));
        Assert.True(UsCcssAlg2U01L01PracticeAdapter.TryPrepareExactNumericQuestion(
            family, slot, parameter, out string prompt, out string correct,
            out var parameters));
        Assert.NotEmpty(prompt);
        Assert.Equal(expected, correct);
        Assert.Equal(parameter, parameters["n"]);
        Assert.Equal(2, parameters.Count);
        Assert.True(UsCcssAlg2U01L01PracticeAdapter.VerifyExactNumericQuestion(
            family, parameters, expected));
        Assert.False(UsCcssAlg2U01L01PracticeAdapter.VerifyExactNumericQuestion(
            family, parameters, "not-a-number"));
    }

    [Fact]
    public void CheckerEquivalentExpression_IsAcceptedWithCanonicalServerAnswerKeptNumeric()
    {
        const string checker = "usccss.alg2.u01.l01.checker_jump_optional";
        Assert.True(UsCcssAlg2U01L01PracticeAdapter.TryPrepareExactNumericQuestion(
            checker, 0, 3, out _, out string canonical, out var parameters));
        Assert.Equal("15", canonical);
        Assert.True(UsCcssAlg2U01L01PracticeAdapter.VerifyExactNumericQuestion(
            checker, parameters, "3*(3+2)"));
        Assert.False(UsCcssAlg2U01L01PracticeAdapter.VerifyExactNumericQuestion(
            checker, parameters, "3*(4+2)"));
    }

    [Fact]
    public void ServiceCompatibleNumericInterface_IsFailClosedForTeacherOrForgedFamily()
    {
        const string hanoi = "usccss.alg2.u01.l01.hanoi_min_moves";
        Assert.False(UsCcssAlg2U01L01PracticeAdapter.TryPrepareExactNumericQuestion(
            hanoi, 1, 1, out _, out _, out _));
        Assert.False(UsCcssAlg2U01L01PracticeAdapter.TryPrepareExactNumericQuestion(
            hanoi, 0, 65, out _, out _, out _));
        Assert.False(UsCcssAlg2U01L01PracticeAdapter.SupportsFamily(
            "usccss.alg2.u01.l02.hanoi_min_moves"));
        Assert.False(UsCcssAlg2U01L01PracticeAdapter.VerifyExactNumericQuestion(
            hanoi, new Dictionary<string, int> { ["n"] = 64, ["variant"] = 2 },
            "18446744073709551615"));
        Assert.False(UsCcssAlg2U01L01PracticeAdapter.VerifyExactNumericQuestion(
            hanoi, new Dictionary<string, int> { ["n"] = 64, ["variant"] = 0,
                ["expected"] = 1 }, "18446744073709551615"));
        Assert.False(UsCcssAlg2U01L01PracticeAdapter.VerifyExactNumericQuestion(
            hanoi, new Dictionary<string, int> { ["n"] = 65, ["variant"] = 0 },
            "36893488147419103231"));
        Assert.False(UsCcssAlg2U01L01PracticeAdapter.VerifyExactNumericQuestion(
            hanoi, new Dictionary<string, int> { ["n"] = 64, ["variant"] = 0 },
            "18446744073709552000"));
    }

    [Fact]
    public void AuthoredRubrics_HaveIndependentMinimalityReasoningAndDistinctRules()
    {
        string hanoiRubric = (string)Item("JadaReasoning", 1).GetType()
            .GetProperty("TeacherRubric")!.GetValue(Item("JadaReasoning", 1))!;
        Assert.Contains("7 + 1 + 7 = 15", hanoiRubric, StringComparison.Ordinal);
        string checkerRubric = (string)Item("CheckerMovesOptional", 7).GetType()
            .GetProperty("TeacherRubric")!.GetValue(Item("CheckerMovesOptional", 7))!;
        Assert.Contains("n^2", checkerRubric, StringComparison.Ordinal);
        string rulesRubric = (string)Item("TwoDistinctRules", 1).GetType()
            .GetProperty("TeacherRubric")!.GetValue(Item("TwoDistinctRules", 1))!;
        Assert.Contains("two genuinely distinct rules", rulesRubric, StringComparison.Ordinal);
        Assert.Contains("10,15,20", rulesRubric, StringComparison.Ordinal);
        Assert.Contains("15,30,50", rulesRubric, StringComparison.Ordinal);
    }
}

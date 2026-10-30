using System.Globalization;
using System.Numerics;

namespace Edulytics.Core.Mathematics.Practice;

/// <summary>
/// Course-owned, deliberately NOT runtime-registered, U01:L01 question adapter.
/// Never serialize ExpectedAnswer to learners. Registration requires exclusive
/// central registry ownership, validation and an authenticated route later.
/// </summary>
internal static class UsCcssAlg2U01L01PracticeAdapter
{
    internal const string LessonCode = "PED:US-CCSS-MATH:HS:TRAD:ALG2:IM-1E:U01:L01";
    internal const string HanoiFamily = "usccss.alg2.u01.l01.hanoi_min_moves";
    internal const string RecurrenceFamily = "usccss.alg2.u01.l01.recursive_sequence";
    internal const string CheckerFamily = "usccss.alg2.u01.l01.checker_jump_optional";

    internal enum Variant
    {
        HanoiMoves,
        CheckerMovesOptional,
        DoubleMinusOne,
        Halve,
        AffineReverse,
        Fibonacci,
        JadaReasoning,
        NoahReasoningOptional,
        TwoDistinctRules
    }

    // Internal-only candidate. A future API must project only Prompt/ItemId,
    // never ExpectedAnswer or teacher metadata into student response DTOs.
    internal sealed record Candidate(
        string LessonCode,
        string FamilyId,
        string ItemId,
        Variant Variant,
        int Parameter,
        string Prompt,
        string? ExpectedAnswer,
        bool TeacherScored,
        string TeacherRubric);

    internal enum Verdict { Correct, Incorrect, TeacherReviewRequired }

    internal static Candidate Create(Variant variant, int parameter)
    {
        string id = "alg2-u01-l01/" + variant.ToString().ToLowerInvariant() + "/" +
                    parameter.ToString(CultureInfo.InvariantCulture);
        const string explanationRubric = "Check the initial term and the repeated rule, " +
            "then require mathematical justification rather than only a guess.";

        return variant switch
        {
            Variant.HanoiMoves when parameter is >= 1 and <= 64 =>
                new(LessonCode, HanoiFamily, id, variant, parameter,
                    "For " + parameter + " discs and three pegs, how many legal moves " +
                    "are minimally required? Give an exact integer or an exact expression.",
                    ((BigInteger.One << parameter) - BigInteger.One)
                        .ToString(CultureInfo.InvariantCulture),
                    false,
                    "The largest disc must move exactly once between two complete " +
                    "(n-1)-disc transfers. Explain the construction and lower bound."),

            Variant.CheckerMovesOptional when parameter is >= 1 and <= 7 =>
                new(LessonCode, CheckerFamily, id, variant, parameter,
                    "In the optional two-color forward-only checker-swap puzzle with " +
                    parameter + " checkers of each color, give the minimum legal moves.",
                    (parameter * (parameter + 2)).ToString(CultureInfo.InvariantCulture),
                    false,
                    "Demonstrate legal slides and opposite-color jumps; reject " +
                    "backwards moves or same-color jumps. Require a minimality argument."),

            Variant.DoubleMinusOne when parameter is >= 1 and <= 8 =>
                new(LessonCode, RecurrenceFamily, id, variant, parameter,
                    "The first term is 3; double the previous term and subtract 1. " +
                    "What is term " + parameter + "?",
                    ((BigInteger.One << parameter) + BigInteger.One)
                        .ToString(CultureInfo.InvariantCulture),
                    false, explanationRubric),

            Variant.Halve when parameter is >= 1 and <= 8 =>
                new(LessonCode, RecurrenceFamily, id, variant, parameter,
                    "The first term is 1/2 and each term is half the preceding one. " +
                    "Give term " + parameter + " as a fraction or decimal.",
                    "1/" + (BigInteger.One << parameter).ToString(CultureInfo.InvariantCulture),
                    false, explanationRubric),

            Variant.AffineReverse when parameter is >= 1 and <= 8 =>
                new(LessonCode, RecurrenceFamily, id, variant, parameter,
                    "For a_(n+1) = -2a_n + 3 and a_4 = -7, find a_" + parameter +
                    " (reverse the rule when n < 4).",
                    AffineTerm(parameter).ToString(CultureInfo.InvariantCulture),
                    false,
                    "For backwards indexing use a_n = (3-a_(n+1))/2, not the " +
                    "forward recurrence. Check by forward substitution."),

            Variant.Fibonacci when parameter is >= 1 and <= 10 =>
                new(LessonCode, RecurrenceFamily, id, variant, parameter,
                    "The first two terms are 0 and 1; each later term is the sum " +
                    "of the previous two. Find term " + parameter + ".",
                    FibonacciTerm(parameter).ToString(CultureInfo.InvariantCulture),
                    false,
                    "Both initial terms and term numbering must be stated."),

            Variant.JadaReasoning when parameter == 1 =>
                new(LessonCode, HanoiFamily, id, variant, parameter,
                    "Explain how a legal three-disc Tower of Hanoi solution can " +
                    "be reused to solve the four-disc puzzle.",
                    null, true,
                    "Teacher scores a valid 3 + 1 + 3 sequence with correct peg " +
                    "transfers, legal moves and reasoning for minimality."),

            Variant.NoahReasoningOptional when parameter == 1 =>
                new(LessonCode, CheckerFamily, id, variant, parameter,
                    "For the optional checker-swap puzzle, describe how a smaller " +
                    "legal arrangement helps solve a larger one.",
                    null, true,
                    "Teacher checks forward-only slides, jumps over opposite colors, " +
                    "legal finish state and evidence supporting the claimed minimum."),

            Variant.TwoDistinctRules when parameter == 1 =>
                new(LessonCode, RecurrenceFamily, id, variant, parameter,
                    "The first two terms are 0 and 5. Give two different complete " +
                    "rules and the next three terms for each rule.",
                    null, true,
                    "Teacher scores two valid, genuinely distinct rules (not only " +
                    "equivalent descriptions) with consistent next terms and seeds."),

            _ => throw new ArgumentOutOfRangeException(nameof(parameter),
                "Unsupported variant/parameter for this source-scoped lesson")
        };
    }

    internal static Verdict Evaluate(Candidate item, string? response)
    {
        if (item.LessonCode != LessonCode ||
            !item.ItemId.StartsWith("alg2-u01-l01/", StringComparison.Ordinal))
            return Verdict.Incorrect;
        if (item.TeacherScored)
            return Verdict.TeacherReviewRequired;

        bool correct = item.Variant switch
        {
            Variant.HanoiMoves =>
                UsCcssAlg2SequencePracticeMath.GradeHanoiMinimumMoves(item.Parameter, response),
            Variant.CheckerMovesOptional =>
                UsCcssAlg2SequencePracticeMath.GradeCheckerMinimumMoves(item.Parameter, response),
            Variant.DoubleMinusOne =>
                UsCcssAlg2SequencePracticeMath.GradeAffineRecurrenceTerm(
                    "3", 1, item.Parameter, 2, -1, response),
            Variant.Halve =>
                UsCcssAlg2SequencePracticeMath.GradeGeometricTerm(
                    "1/2", "1/2", 1, item.Parameter, response),
            Variant.AffineReverse =>
                UsCcssAlg2SequencePracticeMath.GradeAffineRecurrenceTerm(
                    "-7", 4, item.Parameter, -2, 3, response),
            Variant.Fibonacci =>
                response is { Length: > 0 and <= 128 } &&
                BigInteger.TryParse(response, NumberStyles.Integer,
                    CultureInfo.InvariantCulture, out var number) &&
                number == FibonacciTerm(item.Parameter),
            _ => false
        };

        return correct ? Verdict.Correct : Verdict.Incorrect;
    }

    private static BigInteger AffineTerm(int index)
    {
        BigInteger value = -7;
        if (index >= 4)
            for (int n = 4; n < index; n++) value = -2 * value + 3;
        else
            for (int n = 4; n > index; n--) value = (3 - value) / 2;
        return value;
    }

    private static BigInteger FibonacciTerm(int index)
    {
        if (index <= 2) return index - 1;
        BigInteger a = 0, b = 1;
        for (int n = 3; n <= index; n++)
            (a, b) = (b, a + b);
        return b;
    }
}

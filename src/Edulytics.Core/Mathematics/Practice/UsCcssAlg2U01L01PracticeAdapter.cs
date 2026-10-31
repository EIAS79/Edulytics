using System.Globalization;
using System.Numerics;
using System.Text.Json.Serialization;

namespace Edulytics.Core.Mathematics.Practice;

/// <summary>
/// Course-owned, deliberately NOT runtime-registered, U01:L01 question adapter.
/// Never serialize ExpectedAnswer to learners. Registration requires exclusive
/// central registry ownership, validation and an authenticated route later.
/// </summary>
public static class UsCcssAlg2U01L01PracticeAdapter
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
    internal sealed record LearnerQuestion(
        string LessonCode,
        string FamilyId,
        string ItemId,
        string Prompt,
        IReadOnlyDictionary<string, int> Parameters);

    internal sealed record Candidate(
        string LessonCode,
        string FamilyId,
        string ItemId,
        Variant Variant,
        int Parameter,
        string Prompt,
        [property: JsonIgnore] string? ExpectedAnswer,
        bool TeacherScored,
        [property: JsonIgnore] string TeacherRubric)
    {
        // Record.ToString otherwise prints every positional property,
        // including the secret numeric answer and teacher-only rubric.
        public override string ToString() =>
            $"ALG2-L01-Candidate({LessonCode};{FamilyId};{ItemId};TeacherScored={TeacherScored})";
    }

    internal enum Verdict { Correct, Incorrect, TeacherReviewRequired }

    // Bound the length of a submitted proof without automatically scoring its
    // mathematical merits. Blank or unreasonably large submissions must not
    // create a teacher review item.
    internal const int MaximumTeacherExplanationLength = 4000;

    // Exact family dispatch allows a future central owner to bind the
    // existing engine's string family / variant / int parameter contract
    // without duplicating Algebra 2 generation or changing global behavior.
    public static bool SupportsFamily(string? family) =>
        family is HanoiFamily or RecurrenceFamily or CheckerFamily;

    internal static bool TryCreateForFamily(
        string? family, int variantSlot, int parameter, out Candidate? candidate)
    {
        candidate = null;
        if (!SupportsFamily(family))
            return false;

        Variant? variant = family switch
        {
            HanoiFamily => variantSlot switch
            {
                0 => Variant.HanoiMoves,
                1 => Variant.JadaReasoning,
                _ => null
            },
            CheckerFamily => variantSlot switch
            {
                0 => Variant.CheckerMovesOptional,
                1 => Variant.NoahReasoningOptional,
                _ => null
            },
            RecurrenceFamily => variantSlot switch
            {
                0 => Variant.DoubleMinusOne,
                1 => Variant.Halve,
                2 => Variant.AffineReverse,
                3 => Variant.Fibonacci,
                4 => Variant.TwoDistinctRules,
                _ => null
            },
            _ => null
        };
        if (variant is null)
            return false;
        try
        {
            candidate = Create(variant.Value, parameter);
            return true;
        }
        catch (ArgumentOutOfRangeException)
        {
            return false;
        }
    }

    // Only numeric variants can enter the existing exact grading engine.
    // Teacher-scored variants must use a separately approved teacher rubric
    // workflow and must NEVER be marked as machine-verified READY.
    internal static bool TryCreateAutoGradableForFamily(
        string? family, int variantSlot, int parameter, out Candidate? candidate)
    {
        if (!TryCreateForFamily(family, variantSlot, parameter, out candidate) ||
            candidate is null || candidate.TeacherScored)
        {
            candidate = null;
            return false;
        }
        return true;
    }

    // A separate, course-local server-side teacher lane. The candidate
    // includes a private rubric and must never be serialized to learners:
    // ProjectForLearner is the only valid student projection. No human proof
    // can enter the exact numeric verifier or receive an automatic score.
    internal static bool TryCreateTeacherReviewForFamily(
        string? family, int variantSlot, int parameter, out Candidate? candidate)
    {
        if (!TryCreateForFamily(family, variantSlot, parameter, out candidate) ||
            candidate is null || !candidate.TeacherScored ||
            candidate.ExpectedAnswer is not null ||
            string.IsNullOrWhiteSpace(candidate.TeacherRubric))
        {
            candidate = null;
            return false;
        }
        return true;
    }

    // Stable Services-compatible numeric hooks for a FUTURE exclusively
    // coordinated engine dispatch. The caller MUST keep correctAnswer private,
    // exactly as it already does for ExactSkillGeneratedQuestion.CorrectAnswer.
    // A public API does not mean this family is registered or READY.
    public static bool TryPrepareExactNumericQuestion(
        string? family, int variantSlot, int termOrDiscCount,
        out string prompt, out string correctAnswer,
        out IReadOnlyDictionary<string, int> parameters)
    {
        prompt = string.Empty;
        correctAnswer = string.Empty;
        parameters = new Dictionary<string, int>();

        if (!TryCreateAutoGradableForFamily(
                family, variantSlot, termOrDiscCount, out var candidate) ||
            candidate?.ExpectedAnswer is not { Length: > 0 })
            return false;

        prompt = candidate.Prompt;
        correctAnswer = candidate.ExpectedAnswer;
        parameters = new Dictionary<string, int>
        {
            ["n"] = candidate.Parameter,
            ["variant"] = (int)candidate.Variant
        };
        return true;
    }

    // Exact verifier shape matches the shared kernel's Verify(family,
    // IReadOnlyDictionary<string,int>, answer) signature. No fallback to
    // generic sequences or user-provided expected answers is permitted.
    public static bool VerifyExactNumericQuestion(
        string? family, IReadOnlyDictionary<string, int>? parameters,
        string? submittedAnswer)
    {
        if (!SupportsFamily(family) || parameters is null ||
            parameters.Count != 2 ||
            !parameters.TryGetValue("n", out int parameter) ||
            !parameters.TryGetValue("variant", out int variantValue) ||
            !Enum.IsDefined(typeof(Variant), variantValue))
            return false;

        Variant variant = (Variant)variantValue;
        int slot = variant switch
        {
            Variant.HanoiMoves or Variant.CheckerMovesOptional or
                Variant.DoubleMinusOne => 0,
            Variant.JadaReasoning or Variant.NoahReasoningOptional or
                Variant.Halve => 1,
            Variant.AffineReverse => 2,
            Variant.Fibonacci => 3,
            Variant.TwoDistinctRules => 4,
            _ => -1
        };
        if (!TryCreateAutoGradableForFamily(
                family, slot, parameter, out var candidate) ||
            candidate is null || candidate.Variant != variant)
            return false;

        return Evaluate(candidate, submittedAnswer) == Verdict.Correct;
    }

    // This is the ONLY course-owned projection intended for a learner.
    // Answer and teacher rubric stay server-side. The dictionary uses only
    // integer parameters, matching ExactSkillGeneratedQuestion.Parameters;
    // the future engine must keep CorrectAnswer/Solution private.
    internal static LearnerQuestion ProjectForLearner(Candidate item)
    {
        if (!IsCanonicalCandidate(item))
            throw new InvalidOperationException("Invalid ALG2 L01 question candidate.");
        return new LearnerQuestion(
            item.LessonCode, item.FamilyId, item.ItemId, item.Prompt,
            new Dictionary<string, int>
            {
                ["n"] = item.Parameter,
                ["variant"] = (int)item.Variant
            });
    }

    private static bool IsCanonicalCandidate(Candidate? item)
    {
        if (item is null ||
            !string.Equals(item.LessonCode, LessonCode, StringComparison.Ordinal) ||
            !item.ItemId.StartsWith("alg2-u01-l01/", StringComparison.Ordinal) ||
            !SupportsFamily(item.FamilyId))
            return false;
        try
        {
            return item == Create(item.Variant, item.Parameter);
        }
        catch (ArgumentOutOfRangeException)
        {
            return false;
        }
    }

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
                    "The largest disc must move after at least T(n-1) legal moves " +
                    "and before another T(n-1), yielding T(n) >= 2*T(n-1)+1. " +
                    "Exhibit the same construction to attain equality, with " +
                    "T(1)=1. For 64 discs report exact 2^64-1, never a double."),

            Variant.CheckerMovesOptional when parameter is >= 1 and <= 7 =>
                new(LessonCode, CheckerFamily, id, variant, parameter,
                    "In the optional two-color forward-only checker-swap puzzle with " +
                    parameter + " checkers of each color, give the minimum legal moves.",
                    (parameter * (parameter + 2)).ToString(CultureInfo.InvariantCulture),
                    false,
                    "Teacher checks a legal construction and minimum: each of the n^2 " +
                    "opposite-color pairs must cross once via a jump. Those jumps " +
                    "supply 2*n^2 units of required 2*n*(n+1) total forward " +
                    "displacement, leaving at least 2*n single-cell slides, " +
                    "hence n^2+2*n moves. Reject backwards or same-color jumps."),

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
                    "Teacher scores a valid 7 + 1 + 7 = 15 move construction: " +
                    "move three smaller discs, move the largest once, then " +
                    "transfer the three smaller discs again. Check legal pegs " +
                    "and require reasoning for the minimum."),

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
                    "Teacher scores two genuinely distinct rules with seeds 0,5 and " +
                    "three consistent next terms for each: adding 5 gives " +
                    "10,15,20, while a quadratic continuation n(n-1)*5/2 " +
                    "gives 15,30,50 for terms n=3,4,5. Reject two formulas " +
                    "that encode the same rule or differ only in wording."),

            _ => throw new ArgumentOutOfRangeException(nameof(parameter),
                "Unsupported variant/parameter for this source-scoped lesson")
        };
    }

    internal static Verdict Evaluate(Candidate item, string? response)
    {
        if (!IsCanonicalCandidate(item))
            return Verdict.Incorrect;
        if (item.TeacherScored)
            return string.IsNullOrWhiteSpace(response) ||
                   response.Length > MaximumTeacherExplanationLength
                ? Verdict.Incorrect
                : Verdict.TeacherReviewRequired;

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

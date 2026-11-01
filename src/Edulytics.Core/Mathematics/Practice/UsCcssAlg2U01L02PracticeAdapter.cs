using System.Globalization;
using System.Numerics;

namespace Edulytics.Core.Mathematics.Practice;

/// <summary>
/// Algebra 2 IM First Edition U01:L02 source-bounded numeric Practice adapter.
/// Lesson-local only: registration requires separately authorized shared routes.
/// Paper/triangle graph explanations and credit-card rounding remain teacher
/// review; no automatic credit-card grading until a rounding policy is approved.
/// </summary>
public static class UsCcssAlg2U01L02PracticeAdapter
{
    public const string LessonCode = "PED:US-CCSS-MATH:HS:TRAD:ALG2:IM-1E:U01:L02";
    public const string GeometricFamily = "usccss.alg2.u01.l02.geometric_fill_ratio";
    public const string PaperFamily = "usccss.alg2.u01.l02.paper_conservation";
    public const string SierpinskiFamily = "usccss.alg2.u01.l02.sierpinski_shaded";

    public static bool SupportsFamily(string? family) =>
        family is GeometricFamily or PaperFamily or SierpinskiFamily;

    // The contract is (source family, variant, n). No user-controlled rules,
    // decimal arithmetic, unbounded exponents, or mathematical evaluation.
    public static bool TryPrepareExactNumericQuestion(
        string? family, int variant, int n,
        out string prompt, out string correctAnswer,
        out IReadOnlyDictionary<string, int> parameters)
    {
        prompt = string.Empty;
        correctAnswer = string.Empty;
        parameters = new Dictionary<string, int>();
        if (!SupportsFamily(family))
            return false;

        BigInteger numerator;
        BigInteger denominator = BigInteger.One;
        string first;
        string ratio;
        int firstIndex;

        switch (family)
        {
            case GeometricFamily when (variant is >= 0 and <= 3) &&
                                      n >= (variant == 1 ? 1 : 0) && n <= 6:
                (first, ratio, firstIndex, numerator, denominator) = variant switch
                {
                    0 => ("1", "2", 0, BigInteger.One << n, BigInteger.One),
                    1 => ("1/2", "1/2", 1, BigInteger.One, BigInteger.One << n),
                    2 => ("1", "3", 0, BigInteger.Pow(3, n), BigInteger.One),
                    _ => ("1", "3/4", 0, BigInteger.Pow(3, n), BigInteger.Pow(4, n))
                };
                prompt = $"A geometric sequence starts at index {firstIndex} with term {first} " +
                         $"and multiplies by {ratio} each step. Give term {n} exactly.";
                break;

            case PaperFamily when (variant is 0 or 1) && (n is >= 0 and <= 6):
                numerator = variant == 0 ? BigInteger.One << n : 80;
                denominator = variant == 0 ? BigInteger.One : BigInteger.One << n;
                prompt = variant == 0
                    ? $"An 8 by 10 inch sheet is cut so the number of pieces doubles " +
                      $"at every step. How many pieces are there at step {n} (step 0 has 1)?"
                    : $"An 8 by 10 inch sheet is cut into twice as many equal pieces " +
                      $"at every step. What is the area of ONE piece at step {n} in square inches?";
                break;

            case SierpinskiFamily when (variant is 0 or 1) && (n is >= 0 and <= 5):
                numerator = variant == 0 ? BigInteger.Pow(3, n) : 256;
                denominator = variant == 0 ? BigInteger.One : BigInteger.Pow(4, n);
                prompt = variant == 0
                    ? $"A Sierpinski triangle has one shaded triangle at step 0; " +
                      $"the shaded count triples per step. How many at step {n}?"
                    : $"In the source Sierpinski task, each shaded triangle has area " +
                      $"256 at step 0 and one fourth the area each step. " +
                      $"Find the area of ONE shaded triangle at step {n}.";
                break;

            default:
                return false;
        }

        BigInteger gcd = BigInteger.GreatestCommonDivisor(numerator, denominator);
        numerator /= gcd;
        denominator /= gcd;
        correctAnswer = denominator == BigInteger.One
            ? numerator.ToString(CultureInfo.InvariantCulture)
            : numerator.ToString(CultureInfo.InvariantCulture) + "/" +
              denominator.ToString(CultureInfo.InvariantCulture);
        parameters = new Dictionary<string, int>
        {
            ["n"] = n,
            ["variant"] = variant
        };
        return true;
    }

    public static bool VerifyExactNumericQuestion(
        string? family, IReadOnlyDictionary<string, int>? parameters,
        string? submittedAnswer)
    {
        if (parameters is null || parameters.Count != 2 ||
            !parameters.TryGetValue("n", out int n) ||
            !parameters.TryGetValue("variant", out int variant) ||
            !TryPrepareExactNumericQuestion(
                family, variant, n, out _, out _, out _))
            return false;

        return family switch
        {
            GeometricFamily => variant switch
            {
                0 => UsCcssAlg2SequencePracticeMath.GradeGeometricTerm(
                    "1", "2", 0, n, submittedAnswer),
                1 => UsCcssAlg2SequencePracticeMath.GradeGeometricTerm(
                    "1/2", "1/2", 1, n, submittedAnswer),
                2 => UsCcssAlg2SequencePracticeMath.GradeGeometricTerm(
                    "1", "3", 0, n, submittedAnswer),
                3 => UsCcssAlg2SequencePracticeMath.GradeGeometricTerm(
                    "1", "3/4", 0, n, submittedAnswer),
                _ => false
            },
            PaperFamily => UsCcssAlg2SequencePracticeMath.GradeGeometricTerm(
                variant == 0 ? "1" : "80",
                variant == 0 ? "2" : "1/2",
                0, n, submittedAnswer),
            SierpinskiFamily => variant == 0
                ? UsCcssAlg2SequencePracticeMath.GradeSierpinskiCount(n, submittedAnswer)
                : UsCcssAlg2SequencePracticeMath.GradeSierpinskiPieceArea(n, submittedAnswer),
            _ => false
        };
    }
}

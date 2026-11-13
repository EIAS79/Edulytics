using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

// UAE Grade 5 Common U01 L01: extend the existing exact-place-value
// Practice architecture from millions to billions (Int64 answers).
// Keep G4 Int32 skills and behaviour unchanged.
internal static class UaeGrade5Unit1Lesson01Questions
{
    internal const string DigitValue = "uae.g5.u01.l01.digit_value_to_billions";
    internal const string Compose = "uae.g5.u01.l01.compose_ten_digits";
    internal const string Expanded = "uae.g5.u01.l01.expanded_to_standard";

    internal sealed record Item(
        string Family, string Prompt, string Solution,
        AssessmentItemType ItemType, IReadOnlyDictionary<string, int> Parameters);

    private static readonly long[] Multipliers =
        [1_000_000_000L, 100_000_000L, 10_000_000L, 1_000_000L,
         100_000L, 10_000L, 1_000L, 100L, 10L, 1L];

    private static readonly string[] Positions =
        ["آحاد البلايين", "مئات الملايين", "عشرات الملايين", "آحاد الملايين",
         "مئات الألوف", "عشرات الألوف", "آحاد الألوف", "المئات", "العشرات", "الآحاد"];

    internal static bool Supports(string? family) =>
        family is DigitValue or Compose or Expanded;

    internal static Item Build(string family, Random random)
    {
        if (!Supports(family))
            throw new InvalidOperationException("Unsupported G5 place-value family.");

        long number = random.NextInt64(1_000_000_000L, 10_000_000_000L);
        int[] digits = Multipliers.Select(m => (int)(number / m % 10L)).ToArray();
        int position = family == DigitValue ? random.Next(0, digits.Length) : -1;
        string prompt = family switch
        {
            DigitValue =>
                $"في العدد {number:N0}، ما قيمة الرقم في منزلة {Positions[position]}؟",
            Compose =>
                $"كوّن العدد بالصيغة القياسية من أرقام المنازل، بدءًا من البلايين: {string.Join("، ", digits)}.",
            Expanded =>
                $"اكتب العدد بالصيغة القياسية: {string.Join(" + ",
                    digits.Zip(Multipliers, (digit, multiplier) => digit * multiplier)
                          .Where(value => value != 0))}.",
            _ => throw new InvalidOperationException("Unknown Grade 5 family.")
        };

        var parameters = new Dictionary<string, int> { ["pos"] = position };
        for (int i = 0; i < digits.Length; i++)
            parameters[$"d{i}"] = digits[i];

        return new Item(family, prompt,
            "استخدم مخطط القيمة المكانية؛ حافظ على خانات الأصفار، وتحقق من قيمة كل منزلة.",
            AssessmentItemType.Numeric, parameters);
    }

    private static bool TryGetValidatedDigits(
        string family, IReadOnlyDictionary<string, int> parameters,
        out int[] digits, out int position)
    {
        digits = [];
        position = -1;
        if (!Supports(family) || parameters.Count != 11 ||
            !parameters.TryGetValue("pos", out position) ||
            (family == DigitValue && (position < 0 || position > 9)) ||
            (family != DigitValue && position != -1))
            return false;

        var result = new int[10];
        for (int i = 0; i < result.Length; i++)
        {
            if (!parameters.TryGetValue($"d{i}", out int digit) ||
                digit < 0 || digit > 9 || (i == 0 && digit == 0))
                return false;
            result[i] = digit;
        }

        digits = result;
        return true;
    }

    internal static string Solve(string family, IReadOnlyDictionary<string, int> parameters)
    {
        if (!TryGetValidatedDigits(family, parameters, out int[] digits, out int position))
            throw new InvalidOperationException("Invalid Grade 5 place-value parameters.");

        // The solver uses a weighted positional sum.
        long number = 0;
        for (int i = 0; i < digits.Length; i++)
            number = checked(number + digits[i] * Multipliers[i]);

        long value = family == DigitValue
            ? checked(digits[position] * Multipliers[position])
            : number;
        return value.ToString(CultureInfo.InvariantCulture);
    }

    internal static bool Verify(
        string family, IReadOnlyDictionary<string, int> parameters, string? answer)
    {
        if (string.IsNullOrWhiteSpace(answer) ||
            !TryGetValidatedDigits(family, parameters, out int[] digits, out int position))
            return false;

        // Independent check: reconstruct by repeated base-ten accumulation,
        // rather than invoking the weighted-sum solver or trusting its output.
        long reconstructed = 0;
        foreach (int digit in digits)
            reconstructed = checked(reconstructed * 10L + digit);

        long power = 1;
        if (family == DigitValue)
        {
            for (int i = 9; i > position; i--)
                power = checked(power * 10L);
        }
        long expected = family == DigitValue ? digits[position] * power : reconstructed;

        string normalized = new(answer.Trim()
            .Where(ch => ch != '٬' && ch != ',' && ch != ' ' &&
                         ch != '\u00A0' && ch != '\u202F')
            .Select(ch =>
                ch is >= '٠' and <= '٩' ? (char)('0' + ch - '٠') :
                ch is >= '۰' and <= '۹' ? (char)('0' + ch - '۰') : ch)
            .ToArray());

        return long.TryParse(normalized, NumberStyles.None,
                   CultureInfo.InvariantCulture, out long provided)
               && provided == expected;
    }
}

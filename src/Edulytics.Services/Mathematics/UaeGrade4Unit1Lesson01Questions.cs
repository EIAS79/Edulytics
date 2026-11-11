using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

// UAE Grade 4 U01 L01: extend the existing Grade 3 exact place-value approach
// from four-digit numbers to the seven-digit million range.
// Original Edulytics questions; no source artwork or publisher text is copied.
internal static class UaeGrade4Unit1Lesson01Questions
{
    internal const string DigitValue = "uae.g4.u01.l01.digit_value_to_millions";
    internal const string Compose = "uae.g4.u01.l01.compose_seven_digits";
    internal const string Expanded = "uae.g4.u01.l01.expanded_to_standard";
    internal sealed record Item(string Family, string Prompt, string Solution,
        AssessmentItemType ItemType, IReadOnlyDictionary<string,int> Parameters);

    private static readonly int[] Multipliers =
        [1_000_000, 100_000, 10_000, 1_000, 100, 10, 1];

    private static readonly string[] Positions =
        ["الملايين", "مئات الألوف", "عشرات الألوف", "آحاد الألوف",
         "المئات", "العشرات", "الآحاد"];

    internal static bool Supports(string? family) =>
        family is DigitValue or Compose or Expanded;

    public static Item Build(string family, Random rng)
    {
        if (!Supports(family)) throw new InvalidOperationException("Unknown G4 place-value family");
        int number = rng.Next(1_000_000, 10_000_000);
        int pos = family == DigitValue ? rng.Next(0, 7) : -1;
        int[] digits = Multipliers.Select(m => number / m % 10).ToArray();
        int result = pos >= 0 ? digits[pos] * Multipliers[pos] : number;
        string prompt = family switch
        {
            DigitValue => $"في العدد {number}، ما القيمة المكانية للرقم في منزلة {Positions[pos]}؟",
            Compose => $"كوّن العدد بالصيغة القياسية من الأرقام وفق ترتيب المنازل من الملايين إلى الآحاد: {string.Join("، ",digits)}.",
            _ => $"اكتب العدد بالصيغة القياسية: {string.Join(" + ",digits.Zip(Multipliers, (d,m) => d*m).Where(v => v != 0))}."
        };
        return new Item(family,prompt,
            "استخدم مخطط الملايين والألوف والآحاد ثم تحقق من كل منزلة.",
            AssessmentItemType.Numeric,
            new Dictionary<string,int>{{"number",number},{"pos",pos},{"result",result}});
    }

    private static bool Valid(string family, IReadOnlyDictionary<string,int> p, out int expected)
    {
        expected = 0;
        if (!Supports(family) || p.Count!=3 ||
            !p.TryGetValue("number",out int number) ||
            number < 1_000_000 || number > 9_999_999 ||
            !p.TryGetValue("pos",out int pos) ||
            !p.TryGetValue("result",out int supplied) ||
            (family == DigitValue && (pos<0 || pos>6)) ||
            (family != DigitValue && pos!=-1)) return false;
        int value = family == DigitValue
            ? (number / Multipliers[pos] % 10) * Multipliers[pos]
            : number;
        expected = value;
        return supplied == value;
    }

    internal static string Solve(string family, IReadOnlyDictionary<string,int> p)
    {
        if (!Valid(family,p,out int expected))
            throw new InvalidOperationException("Invalid G4 place-value parameters");
        return expected.ToString(CultureInfo.InvariantCulture);
    }

    internal static bool Verify(string family, IReadOnlyDictionary<string,int> p, string? answer)
    {
        if (!Valid(family,p,out int expected) || string.IsNullOrWhiteSpace(answer)) return false;
        string normalized = new(answer.Trim()
            .Where(c => c != '٬' && c != ',' && c != ' ' && c != '\u00a0')
            .Select(c => c is >= '٠' and <= '٩' ? (char)('0'+c-'٠') :
                          c is >= '۰' and <= '۹' ? (char)('0'+c-'۰') : c).ToArray());
        return int.TryParse(normalized, NumberStyles.None,
            CultureInfo.InvariantCulture, out int value) && value == expected;
    }
}

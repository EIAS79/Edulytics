using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

// UAE Grade 4 U01 L01: reuse the existing G3 exact-engine architecture,
// extending place value to millions. Original examples, no publisher artwork.
internal static class UaeGrade4Unit1Lesson01Questions
{
    internal const string DigitValue = "uae.g4.u01.l01.digit_value_to_millions";
    internal const string Compose = "uae.g4.u01.l01.compose_seven_digits";
    internal const string Expanded = "uae.g4.u01.l01.expanded_to_standard";
    internal sealed record Item(string Family, string Prompt, string Solution,
        AssessmentItemType ItemType, IReadOnlyDictionary<string, int> Parameters);

    private static readonly int[] Multipliers =
        [1_000_000, 100_000, 10_000, 1_000, 100, 10, 1];
    private static readonly string[] Positions =
        ["الملايين", "مئات الألوف", "عشرات الألوف", "آحاد الألوف",
         "المئات", "العشرات", "الآحاد"];

    internal static bool Supports(string? family) =>
        family is DigitValue or Compose or Expanded;

    public static Item Build(string family, Random rng)
    {
        if (!Supports(family))
            throw new InvalidOperationException("Unknown G4 place-value family");
        int number = rng.Next(1_000_000, 10_000_000);
        int[] digits = Multipliers.Select(m => number / m % 10).ToArray();
        int pos = family == DigitValue ? rng.Next(0, 7) : -1;
        string prompt = family switch
        {
            DigitValue =>
                $"في العدد {number}، ما قيمة الرقم في منزلة {Positions[pos]}؟",
            Compose =>
                $"كوّن العدد بالصيغة القياسية من الأرقام حسب ترتيب المنازل: {string.Join("، ", digits)}.",
            _ => $"اكتب الصيغة القياسية: {string.Join(" + ", digits.Zip(Multipliers, (d, m) => d*m).Where(v=>v!=0))}."
        };
        var p = new Dictionary<string,int>{{"pos",pos}};
        for(int i=0;i<7;i++)p[$"d{i}"]=digits[i];
        return new Item(family,prompt,
            "استخدم جدول دورات الملايين والألوف والآحاد واحسب قيمة كل رقم.",
            AssessmentItemType.Numeric,p);
    }

    private static bool Valid(string family,
        IReadOnlyDictionary<string,int> p,out int expected)
    {
        expected = 0;
        if (!Supports(family) || p.Count != 8 ||
            !p.TryGetValue("pos",out int pos) ||
            (family == DigitValue && (pos < 0 || pos > 6)) ||
            (family != DigitValue && pos != -1)) return false;
        int number = 0;
        var digits = new int[7];
        for(int i=0;i<7;i++)
        {
            if(!p.TryGetValue($"d{i}",out int digit) ||
               digit<0 || digit>9 ||
               (i==0 && digit==0)) return false;
            digits[i]=digit;
            number=checked(number+digit*Multipliers[i]);
        }
        expected=family==DigitValue?digits[pos]*Multipliers[pos]:number;
        return true;
    }

    internal static string Solve(string family,
        IReadOnlyDictionary<string,int> p)
    {
        if (!Valid(family,p,out int expected))
            throw new InvalidOperationException("Invalid G4 place-value parameters");
        return expected.ToString(CultureInfo.InvariantCulture);
    }

    internal static bool Verify(string family,
        IReadOnlyDictionary<string,int> p,string? answer)
    {
        if (!Valid(family,p,out int expected) ||
            string.IsNullOrWhiteSpace(answer)) return false;
        string normalized = new(answer.Trim()
            .Where(c => c != '٬' && c != ',' && c != ' ' && c != '\u00a0')
            .Select(c => c is >= '٠' and <= '٩' ? (char)('0'+c-'٠') :
                          c is >= '۰' and <= '۹' ? (char)('0'+c-'۰') : c).ToArray());
        return int.TryParse(normalized,NumberStyles.None,
            CultureInfo.InvariantCulture,out int parsed) && parsed==expected;
    }
}

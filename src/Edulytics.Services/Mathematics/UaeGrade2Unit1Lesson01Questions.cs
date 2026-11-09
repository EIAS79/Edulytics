using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// First-party UAE Grade2 U01 L01. Extends the existing exact Practice
/// engine with only two genuinely missing lesson-specific contract families.
/// The 2025–2026 student source is used only for skill parity, not copy.
/// </summary>
internal static class UaeGrade2Unit1Lesson01Questions
{
    public const string Commutative = "uae.g2.u01.l01.commutative_add";
    public const string Identity = "uae.g2.u01.l01.additive_zero";
    public sealed record Question(string Family, string Prompt, string Solution,
        AssessmentItemType ItemType, IReadOnlyDictionary<string, int> Parameters);
    public static bool Supports(string? f) => f is Commutative or Identity;

    public static Question Build(string family, Random rng)
    {
        if (!Supports(family)) throw new InvalidOperationException("Not UAE G2 U01 L01");
        int a = rng.Next(1, 10), b = family == Identity ? 0 : rng.Next(1, 10);
        int mode = rng.Next(0, 2);
        int sum = a + b;
        string dotsA = new('●', a), dotsB = new('○', b);
        string prompt = family == Identity
            ? mode == 0
                ? $"المجموعة الأولى {dotsA} والثانية فارغة ∅. أكمل: {a} + 0 = ؟"
                : $"مجموعة فارغة ∅ وبعدها المجموعة {dotsA}. أكمل: 0 + {a} = ؟"
            : mode == 0
                ? $"المجموعتان {dotsA} | {dotsB}. إذا كان {a} + {b} = {sum}، فأكمل {b} + {a} = ؟"
                : $"بدلت ترتيب المجموعتين {dotsB} | {dotsA}. احسب: {b} + {a} = ؟ ثم تحقق من {a} + {b}.";
        var p = new Dictionary<string, int>(StringComparer.Ordinal)
        { ["a"]=a, ["b"]=b, ["mode"]=mode, ["result"]=sum };
        return new Question(family,prompt,
            family == Identity ? "الصفر لا يضيف قطعًا، فيبقى العدد نفسه."
                               : "تبديل ترتيب الحدين لا يغير العدد الكلي.",
            AssessmentItemType.Numeric,p);
    }

    private static bool Valid(string? f, IReadOnlyDictionary<string, int> p, out int total)
    {
        total = 0;
        if (!Supports(f) || p.Count != 4 ||
            !p.TryGetValue("a", out int a) || a is < 1 or > 9 ||
            !p.TryGetValue("b", out int b) ||
            !p.TryGetValue("mode", out int mode) || mode is < 0 or > 1 ||
            !p.TryGetValue("result", out int result)) return false;
        if (f == Identity ? b != 0 : b is < 1 or > 9) return false;
        total = a + b;
        return total == result;
    }

    public static string Solve(string family, IReadOnlyDictionary<string, int> p)
    {
        if (!Valid(family,p,out int v)) throw new InvalidOperationException("Invalid G2 L1 parameters");
        return v.ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family, IReadOnlyDictionary<string, int> p, string answer)
    {
        if (!Valid(family,p,out int v) || string.IsNullOrWhiteSpace(answer)) return false;
        string digits = new(answer.Trim().Select(c =>
            c is >= '٠' and <= '٩' ? (char)('0'+c-'٠') :
            c is >= '۰' and <= '۹' ? (char)('0'+c-'۰') : c).ToArray());
        return int.TryParse(digits, NumberStyles.Integer, CultureInfo.InvariantCulture,
            out int entered) && entered == v;
    }
}

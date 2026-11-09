using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

// G3 Unit 2 Lesson 9: extends the existing exact Practice engine; source wording
// and artwork are not used. Numeric estimation already reuses L05's family.
// Stage D runtime/CI checks have NOT RUN.
internal static class UaeGrade3Unit2Lesson09Questions
{
    internal const string Family = "uae.g3.u02.l09.reasonable_estimate_verdict";

    internal sealed record Item(string Family, string Prompt, string Solution,
        AssessmentItemType ItemType, IReadOnlyDictionary<string, int> Parameters);

    public static bool Supports(string? family) => family == Family;
    private static int RoundHundred(int n) => ((n + 50) / 100) * 100;

    public static Item Build(string family, Random rng)
    {
        if (!Supports(family))
            throw new InvalidOperationException("Unsupported Grade 3 reasonableness family");

        // Two or three positive addends; every number remains in the grade-3 range.
        int mode = rng.Next(2);
        int a = rng.Next(200, 1900);
        int b = rng.Next(200, 1900);
        int c = mode == 1 ? rng.Next(200, 1900) : 0;
        int expected = RoundHundred(a) + RoundHundred(b) + RoundHundred(c);
        int[] options = expected >= 1000
            ? [0, -500, 500, -1000, 1000]
            : [0, 500, 1000];
        int delta = options[rng.Next(options.Length)];
        int candidate = expected + delta;
        int result = delta == 0 ? 1 : 0;

        string terms = mode == 0 ? $"{a} و{b}" : $"{a} و{b} و{c}";
        string prompt = $"جمعت مجموعة مقادير {terms}، وقدّر أحد الطلاب مجموعها بنحو {candidate}. " +
            "إذا قرّبت كل مقدار إلى أقرب مئة ثم جمعت، هل يطابق تقديرُه الناتج؟ أجب بنعم أو لا.";

        return new Item(family, prompt,
            "أقرّب كل عدد إلى أقرب مئة، أجمع التقديرات، ثم أقارن الناتج بالتقدير المقترح. " +
            "أشرح للمعلم سبب الحكم، فالتقدير ليس ناتج الجمع الدقيق.",
            AssessmentItemType.ShortAnswer,
            new Dictionary<string, int>
            {
                ["a"] = a, ["b"] = b, ["c"] = c,
                ["mode"] = mode, ["delta"] = delta,
                ["candidate"] = candidate, ["result"] = result
            });
    }

    private static bool Valid(string? family, IReadOnlyDictionary<string, int> p,
        out int result)
    {
        result = 0;
        if (!Supports(family) || p.Count != 7 ||
            !p.TryGetValue("a", out int a) || a is < 200 or > 1899 ||
            !p.TryGetValue("b", out int b) || b is < 200 or > 1899 ||
            !p.TryGetValue("c", out int c) ||
            !p.TryGetValue("mode", out int mode) || mode is < 0 or > 1 ||
            !p.TryGetValue("delta", out int delta) ||
            !p.TryGetValue("candidate", out int candidate) ||
            !p.TryGetValue("result", out int stored))
            return false;

        if (mode == 0 && c != 0 || mode == 1 && c is < 200 or > 1899)
            return false;

        int sum = RoundHundred(a) + RoundHundred(b) + RoundHundred(c);
        if (delta is not (0 or 500 or -500 or 1000 or -1000) ||
            delta < 0 && sum < 1000 ||
            candidate < 0 || candidate != sum + delta)
            return false;

        result = delta == 0 ? 1 : 0;
        return stored == result;
    }

    public static string Solve(string family, IReadOnlyDictionary<string, int> p)
    {
        if (!Valid(family, p, out int answer))
            throw new InvalidOperationException("Invalid UAE G3 reasonableness parameters");
        return answer == 1 ? "نعم" : "لا";
    }

    public static bool Verify(string family, IReadOnlyDictionary<string, int> p,
        string answer)
    {
        if (!Valid(family, p, out int correct) || string.IsNullOrWhiteSpace(answer))
            return false;
        string normalized = answer.Trim().ToLowerInvariant();
        return correct == 1
            ? normalized is "نعم" or "yes" or "صح" or "صحيح"
            : normalized is "لا" or "no" or "خطأ" or "غير صحيح";
    }
}

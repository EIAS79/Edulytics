using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

// UAE G5 Common U01 L02: reuses the proven comparison/ordering exact
// Practice pattern, extending the source range to nine-digit numbers.
// Grade 4 generators remain unchanged.
internal static class UaeGrade5Unit1Lesson02Questions
{
    internal const string Relation = "uae.g5.u01.l02.compare_to_millions";
    internal const string Greater = "uae.g5.u01.l02.select_greater";
    internal const string Ascending = "uae.g5.u01.l02.order_four_ascending";
    internal const string Descending = "uae.g5.u01.l02.order_four_descending";
    internal sealed record Item(string Family, string Prompt, string Solution,
        AssessmentItemType ItemType, IReadOnlyDictionary<string, int> Parameters);

    private const int Min = 100_000;
    private const int MaxExclusive = 1_000_000_000;

    internal static bool Supports(string? family) =>
        family is Relation or Greater or Ascending or Descending;

    internal static Item Build(string family, Random random)
    {
        if (!Supports(family))
            throw new InvalidOperationException("Unsupported G5 comparison family.");

        if (family is Ascending or Descending)
        {
            var values = new HashSet<int>();
            while (values.Count != 4)
                values.Add(random.Next(Min, MaxExclusive));
            int[] originals = values.ToArray();
            var parameters = new Dictionary<string, int>
            {
                ["a"] = originals[0], ["b"] = originals[1],
                ["c"] = originals[2], ["d"] = originals[3]
            };
            string direction = family == Ascending ? "الأصغر إلى الأكبر" : "الأكبر إلى الأصغر";
            return new Item(family,
                $"رتب الأعداد الآتية من {direction}، وافصل بينها بفاصلة منقوطة: {string.Join("، ", originals)}.",
                "قارن عدد الخانات ثم المنازل بدءًا من الملايين.",
                AssessmentItemType.ShortAnswer, parameters);
        }

        int a = random.Next(Min, MaxExclusive);
        int b = random.Next(Min, MaxExclusive);
        if (family == Relation && random.Next(6) == 0)
            b = a;
        else if (family == Greater && b == a)
            b = a == MaxExclusive - 1 ? a - 1 : a + 1;

        var pair = new Dictionary<string, int> { ["a"] = a, ["b"] = b };
        string prompt = family == Relation
            ? $"ضع > أو < أو = في الفراغ: {a:N0} □ {b:N0}."
            : $"ما العدد الأكبر بين {a:N0} و{b:N0}؟";
        return new Item(family, prompt,
            "انظر إلى أول منزلة مختلفة من جهة اليسار لتحديد العلاقة.",
            family == Relation ? AssessmentItemType.ShortAnswer : AssessmentItemType.Numeric,
            pair);
    }

    private static bool TryPair(string family, IReadOnlyDictionary<string, int> p,
        out int a, out int b)
    {
        a = b = 0;
        return (family is Relation or Greater) && p.Count == 2 &&
            p.TryGetValue("a", out a) && p.TryGetValue("b", out b) &&
            a >= Min && a < MaxExclusive && b >= Min && b < MaxExclusive &&
            (family == Relation || a != b);
    }

    private static bool TrySet(IReadOnlyDictionary<string, int> p, out int[] values)
    {
        values = [];
        if (p.Count != 4 || !p.TryGetValue("a", out int a) ||
            !p.TryGetValue("b", out int b) ||
            !p.TryGetValue("c", out int c) ||
            !p.TryGetValue("d", out int d))
            return false;

        int[] candidate = [a, b, c, d];
        if (candidate.Any(x => x < Min || x >= MaxExclusive) ||
            candidate.Distinct().Count() != 4)
            return false;
        values = candidate;
        return true;
    }

    private static string NormalizeNumber(string answer) =>
        new(answer.Trim()
            .Where(ch => ch != ',' && ch != '٬' && ch != ' ' &&
                         ch != '\u00A0' && ch != '\u202F')
            .Select(ch =>
                ch is >= '٠' and <= '٩' ? (char)('0' + ch - '٠') :
                ch is >= '۰' and <= '۹' ? (char)('0' + ch - '۰') : ch)
            .ToArray());

    internal static string Solve(string family, IReadOnlyDictionary<string, int> p)
    {
        if (family is Ascending or Descending)
        {
            if (!TrySet(p, out int[] originals))
                throw new InvalidOperationException("Invalid G5 order parameters.");
            Array.Sort(originals);
            if (family == Descending)
                Array.Reverse(originals);
            return string.Join(";", originals);
        }
        if (!TryPair(family, p, out int a, out int b))
            throw new InvalidOperationException("Invalid G5 comparison parameters.");
        return family == Relation
            ? (a > b ? ">" : a < b ? "<" : "=")
            : Math.Max(a, b).ToString(CultureInfo.InvariantCulture);
    }

    internal static bool Verify(
        string family, IReadOnlyDictionary<string, int> p, string? answer)
    {
        if (!Supports(family) || string.IsNullOrWhiteSpace(answer))
            return false;
        if (family is Ascending or Descending)
        {
            if (!TrySet(p, out int[] originals))
                return false;
            string[] tokens = answer.Split(';', StringSplitOptions.TrimEntries);
            if (tokens.Length != 4)
                return false;
            var provided = new int[4];
            for (int i = 0; i < 4; i++)
                if (!int.TryParse(NormalizeNumber(tokens[i]), NumberStyles.None,
                    CultureInfo.InvariantCulture, out provided[i]))
                    return false;
            var independentlySorted = family == Ascending
                ? originals.OrderBy(x => x).ToArray()
                : originals.OrderByDescending(x => x).ToArray();
            return provided.SequenceEqual(independentlySorted);
        }

        if (!TryPair(family, p, out int a, out int b))
            return false;
        if (family == Relation)
            return answer.Trim() == (a < b ? "<" : a > b ? ">" : "=");

        return int.TryParse(NormalizeNumber(answer), NumberStyles.None,
                   CultureInfo.InvariantCulture, out int providedValue)
               && providedValue == (a >= b ? a : b);
    }
}

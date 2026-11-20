using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Grade 9 UAE General lesson 7-6. Exact integer-root cases for monic quadratics.
/// This mathematical mechanism cannot be represented by the existing linear
/// equations or monomial-factor families; explanations remain teacher-reviewed.
/// </summary>
internal static class UaeG9MonicQuadraticQuestions
{
    internal const string Distinct = "uae.g9.general.u07.l06.monic_distinct_roots";
    internal const string Repeated = "uae.g9.general.u07.l06.monic_repeated_root";
    internal sealed record Item(
        string Family, string Prompt, string Solution,
        AssessmentItemType ItemType, IReadOnlyDictionary<string, int> Parameters);

    internal static bool Supports(string? family) => family is Distinct or Repeated;

    private static bool Valid(string family, IReadOnlyDictionary<string, int> p)
    {
        if (!Supports(family) || p.Count != 4 ||
            !p.TryGetValue("r1", out int r1) || !p.TryGetValue("r2", out int r2) ||
            !p.TryGetValue("b", out int b) || !p.TryGetValue("c", out int c) ||
            r1 is < -8 or > 8 || r2 is < -8 or > 8 ||
            b != -(r1 + r2) || c != r1 * r2) return false;
        return family == Distinct ? r1 < r2 : r1 == r2;
    }

    private static string Signed(int value) => value < 0
        ? value.ToString(CultureInfo.InvariantCulture)
        : "+" + value.ToString(CultureInfo.InvariantCulture);

    internal static Item Build(string family, Random random)
    {
        if (!Supports(family)) throw new InvalidOperationException("Unsupported G9 quadratic family.");
        ArgumentNullException.ThrowIfNull(random);
        int a = random.Next(-7, 8), b = family == Repeated ? a : random.Next(-7, 8);
        if (family == Distinct)
        {
            while (a == b) b = random.Next(-7, 8);
            if (a > b) (a, b) = (b, a);
        }
        int linear = -(a + b), constant = a * b;
        var parameters = new Dictionary<string, int>
        {
            ["r1"] = a, ["r2"] = b, ["b"] = linear, ["c"] = constant
        };
        string prompt = family == Distinct
            ? $"حل المعادلة س²{Signed(linear)}س{Signed(constant)}=٠ بالتحليل إلى عاملين. اكتب الجذرين تصاعديًا مفصولين بفاصلة."
            : $"حل المعادلة س²{Signed(linear)}س{Signed(constant)}=٠ بالتحليل؛ اكتب الجذر المكرر مرة واحدة.";
        return new Item(family, prompt,
            "ابحث عن عددين مجموعهما معامل س وحاصل ضربهما الحد الثابت، ثم طبّق خاصية حاصل الضرب الصفري وتحقق بالتعويض.",
            AssessmentItemType.ShortAnswer, parameters);
    }

    internal static string Solve(string family, IReadOnlyDictionary<string, int> p)
    {
        if (!Valid(family, p)) throw new InvalidOperationException("Invalid monic quadratic parameters.");
        return family == Distinct
            ? FormattableString.Invariant($"{p["r1"]},{p["r2"]}")
            : p["r1"].ToString(CultureInfo.InvariantCulture);
    }

    private static string Normalize(string? value)
    {
        var chars = (value ?? "").Trim()
            .Replace('−', '-').Replace('–', '-').Replace('،', ',').Replace('؛', ',')
            .Where(c => !char.IsWhiteSpace(c) && c is not '\u200E' and not '\u200F' and not '\u061C')
            .Select(c => c switch
            {
                >= '٠' and <= '٩' => (char)('0' + c - '٠'),
                >= '۰' and <= '۹' => (char)('0' + c - '۰'),
                _ => c
            }).ToArray();
        return new string(chars);
    }

    internal static bool Verify(string family, IReadOnlyDictionary<string, int> p, string? answer)
    {
        if (!Valid(family, p) || string.IsNullOrWhiteSpace(answer) || answer.Length > 60) return false;
        string[] parts = Normalize(answer).Split(',');
        if (parts.Length != (family == Repeated ? 1 : 2)) return false;
        var parsed = new int[parts.Length];
        for (int i = 0; i < parts.Length; i++)
        {
            if (!int.TryParse(parts[i], NumberStyles.AllowLeadingSign,
                    CultureInfo.InvariantCulture, out parsed[i])) return false;
        }
        return family == Repeated ? parsed[0] == p["r1"] :
            parsed[0] == p["r1"] && parsed[1] == p["r2"];
    }
}

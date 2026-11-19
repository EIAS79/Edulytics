using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Exact numeric weighted-mean skill extension, not an unweighted mean proxy.
/// Arabic learner prompts; Stage D must run the generator, verifier and semantic gate.
/// </summary>
internal static class UaeGrade9WeightedMeanQuestions
{
    internal const string WeightedGrades = "uae.g9.u02.l09.weighted_grades";
    internal const string WeightedGroups = "uae.g9.u02.l09.weighted_group_sizes";

    internal sealed record Question(
        string Family, string Prompt, string Solution,
        AssessmentItemType ItemType, IReadOnlyDictionary<string, int> Parameters);

    internal static bool Supports(string? family) =>
        family is WeightedGrades or WeightedGroups;

    internal static Question Build(string family, Random random)
    {
        if (!Supports(family))
            throw new ArgumentException("Unsupported G9 weighted-mean family.", nameof(family));
        // A 20% / 30% / 50% weighted calculation, with integer score
        // outputs and exact invariant-culture grading.
        int first = random.Next(5, 10) * 10;
        int second = random.Next(5, 10) * 10;
        int third = random.Next(5, 10) * 10;
        var values = new Dictionary<string, int>
        {
            ["v1"] = first, ["v2"] = second, ["v3"] = third,
            ["w1"] = 2, ["w2"] = 3, ["w3"] = 5
        };
        string prompt = family == WeightedGrades
            ? $"أوجد المتوسط المرجح لدرجات {first} و{second} و{third} بأوزان ٢٠٪ و٣٠٪ و٥٠٪ على الترتيب."
            : $"ثلاث مجموعات متوسطاتها {first} و{second} و{third}، وأعدادها ٢ و٣ و٥ على الترتيب. أوجد المتوسط الكلي لجميع العناصر.";
        return new Question(
            family, prompt,
            "اضرب كل متوسط في وزنه، ثم اجمع النواتج واقسم على مجموع الأوزان. لا تستعمل المتوسط غير المرجح.",
            AssessmentItemType.ShortAnswer, values);
    }

    internal static string Solve(string family, IReadOnlyDictionary<string, int> p)
    {
        if (!Supports(family) || !Valid(p))
            throw new ArgumentException("Invalid weighted mean inputs.", nameof(p));
        long weighted = (long)p["v1"] * p["w1"]
                      + (long)p["v2"] * p["w2"]
                      + (long)p["v3"] * p["w3"];
        int sum = p["w1"] + p["w2"] + p["w3"];
        if (weighted % sum != 0)
            throw new InvalidOperationException("Generated weighted mean is non-integral.");
        return (weighted / sum).ToString(CultureInfo.InvariantCulture);
    }

    internal static bool Verify(string family, IReadOnlyDictionary<string, int> p, string? answer)
    {
        if (!Supports(family) || !Valid(p) || string.IsNullOrWhiteSpace(answer))
            return false;
        var normalized = string.Concat(answer.Trim().Select(c => c switch
        {
            >= '٠' and <= '٩' => (char)('0' + c - '٠'),
            >= '۰' and <= '۹' => (char)('0' + c - '۰'),
            '−' or '–' => '-',
            _ => c
        }));
        if (!long.TryParse(normalized, NumberStyles.Integer,
                          CultureInfo.InvariantCulture, out long candidate))
            return false;
        long deviation = (candidate - p["v1"]) * p["w1"]
                       + (candidate - p["v2"]) * p["w2"]
                       + (candidate - p["v3"]) * p["w3"];
        return deviation == 0;
    }

    private static bool Valid(IReadOnlyDictionary<string, int> p)
    {
        if (!p.TryGetValue("v1", out int v1) ||
            !p.TryGetValue("v2", out int v2) ||
            !p.TryGetValue("v3", out int v3) ||
            !p.TryGetValue("w1", out int w1) ||
            !p.TryGetValue("w2", out int w2) ||
            !p.TryGetValue("w3", out int w3))
            return false;
        return v1 is >= 0 and <= 100 && v2 is >= 0 and <= 100 &&
               v3 is >= 0 and <= 100 && w1 > 0 && w2 > 0 && w3 > 0 &&
               (long)w1 + w2 + w3 <= 10000;
    }
}

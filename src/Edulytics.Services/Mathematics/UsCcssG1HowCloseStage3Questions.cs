using System.Globalization;
using System.Text.RegularExpressions;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// VERSIONED, source-specific U06 L04 digital Practice family.
/// The legacy center_add_compare family is kept unchanged for historical items.
/// This family grades only the four-card arithmetic, not physical center play.
/// </summary>
public static class UsCcssG1HowCloseStage3Questions
{
    public const string FamilyId = "usccss.g1.u06.l04.how_close_stage3.v2";

    public sealed record Problem(string Family, string Prompt, string Explanation,
        AssessmentItemType ItemType, IReadOnlyDictionary<string, int> Parameters);

    public static bool Supports(string? family) =>
        string.Equals(family, FamilyId, StringComparison.Ordinal);

    public static bool Valid(IReadOnlyDictionary<string, int> parameters)
    {
        if (parameters.Count != 7) return false;
        var cards = new int[7];
        for (int i = 0; i < cards.Length; i++)
        {
            if (!parameters.TryGetValue("d" + i.ToString(CultureInfo.InvariantCulture),
                out cards[i])) return false;
        }
        return UsCcssG1HowCloseStage3Mathematics.ValidCards(cards);
    }

    private static int[] Cards(IReadOnlyDictionary<string, int> parameters) =>
        Enumerable.Range(0, 7)
            .Select(i => parameters["d" + i.ToString(CultureInfo.InvariantCulture)])
            .ToArray();

    public static Problem Build(Random random)
    {
        var cards = UsCcssG1HowCloseStage3Mathematics.DrawSeven(random);
        var parameters = Enumerable.Range(0, 7)
            .ToDictionary(i => "d" + i.ToString(CultureInfo.InvariantCulture),
                i => cards[i], StringComparer.Ordinal);
        if (!Valid(parameters))
            throw new InvalidOperationException("Invalid seven-card source task.");

        var prompt = "Original 2021 How Close? Stage 3 DIGITAL MATH: Your seven " +
            "number cards are " + string.Join(", ", cards) + ". Choose FOUR different " +
            "drawn cards to build TWO two-digit numbers. Add them to get AS CLOSE " +
            "AS POSSIBLE TO 100 WITHOUT GOING OVER. Use each card only once, " +
            "and do not put zero in the tens place. Write " +
            "NUMBERS=first,second;TOTAL=sum (any mathematically optimal pair is accepted).";

        return new(FamilyId, prompt,
            "Calculate the total from four distinct available cards. " +
            "An independent mathematical checker verifies no overshoot and " +
            "accepts every equally optimal pair. Real card play, partner turns, " +
            "recording sheets and discussion remain teacher-facilitated.",
            AssessmentItemType.ShortAnswer, parameters);
    }

    public static string Solve(IReadOnlyDictionary<string, int> parameters)
    {
        if (!Valid(parameters))
            throw new InvalidOperationException("Invalid seven-card source-task parameters.");

        var best = UsCcssG1HowCloseStage3Mathematics.FindOptimalPairs(Cards(parameters))[0];
        return $"NUMBERS={best.First},{best.Second};TOTAL={best.Total}";
    }

    public static bool Verify(IReadOnlyDictionary<string, int> parameters, string? answer)
    {
        if (!Valid(parameters) || string.IsNullOrWhiteSpace(answer) ||
            answer.Length > 100) return false;

        var compact = string.Concat(answer.Where(c => !char.IsWhiteSpace(c)))
            .ToUpperInvariant();
        var match = Regex.Match(compact,
            @"\ANUMBERS=(\d{2}),(\d{2});TOTAL=(\d{1,3})\z",
            RegexOptions.CultureInvariant, TimeSpan.FromMilliseconds(100));
        if (!match.Success) return false;

        if (!int.TryParse(match.Groups[1].Value, NumberStyles.None,
                CultureInfo.InvariantCulture, out int first) ||
            !int.TryParse(match.Groups[2].Value, NumberStyles.None,
                CultureInfo.InvariantCulture, out int second) ||
            !int.TryParse(match.Groups[3].Value, NumberStyles.None,
                CultureInfo.InvariantCulture, out int statedTotal))
            return false;

        var grade = UsCcssG1HowCloseStage3Mathematics.GradePair(
            Cards(parameters), first, second);
        return grade.Optimal && grade.Total == statedTotal;
    }
}

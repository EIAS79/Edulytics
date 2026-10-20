namespace Edulytics.Services.Mathematics;

/// <summary>
/// Original IM K–5 2021 G1 U06 L04 Stage 3: seven distinct cards from 0–9,
/// choose four once to form two two-digit numbers, add WITHOUT exceeding 100.
/// Pure source-goal mathematics for a future VERSIONED Practice adapter.
/// This intentionally does not alter historical center_add_compare seeds.
/// Partner play, physical cards and strategy narration remain teacher-led.
/// </summary>
public static class UsCcssG1HowCloseStage3Mathematics
{
    public sealed record Pair(int First, int Second, int Total, int Gap);
    public sealed record Grade(bool ValidSelection, bool Optimal, int? Total,
        int? BestTotal, string Reason);

    public static bool ValidCards(IReadOnlyList<int>? cards) =>
        cards is { Count: 7 } && cards.All(x => x is >= 0 and <= 9) &&
        cards.Distinct().Count() == 7;

    /// <summary>Returns EVERY optimal pair, not just one arbitrary key.
    /// Interchanging the two addends does not create a new answer.</summary>
    public static IReadOnlyList<Pair> FindOptimalPairs(IReadOnlyList<int> cards)
    {
        if (!ValidCards(cards))
            throw new ArgumentException("Exactly seven distinct cards numbered 0–9 required.", nameof(cards));

        var optimal = new List<Pair>();
        int best = -1;
        for (int i = 0; i < 7; i++)
        for (int j = 0; j < 7; j++)
        for (int k = 0; k < 7; k++)
        for (int l = 0; l < 7; l++)
        {
            if (i == j || i == k || i == l || j == k || j == l || k == l)
                continue;
            if (cards[i] == 0 || cards[k] == 0) continue; // two-digit addends

            int first = cards[i] * 10 + cards[j];
            int second = cards[k] * 10 + cards[l];
            if (first > second) continue; // canonical unordered addition

            int sum = first + second;
            if (sum > 100 || sum < best) continue; // original no-overshoot rule
            if (sum > best)
            {
                best = sum;
                optimal.Clear();
            }
            optimal.Add(new Pair(first, second, sum, 100 - sum));
        }

        if (optimal.Count == 0)
            throw new InvalidOperationException("No permitted two-addend sum could be constructed.");

        return optimal.OrderBy(x => x.First).ThenBy(x => x.Second).ToArray();
    }

    public static Grade GradePair(IReadOnlyList<int> cards, int first, int second)
    {
        if (!ValidCards(cards))
            throw new ArgumentException("Exactly seven distinct cards numbered 0–9 required.", nameof(cards));

        if (first is < 10 or > 99 || second is < 10 or > 99)
            return new(false, false, null, null, "TWO_DIGIT_NUMBERS_REQUIRED");

        int[] digits = [first / 10, first % 10, second / 10, second % 10];
        if (digits.Distinct().Count() != 4 || digits.Any(d => !cards.Contains(d)))
            return new(false, false, null, null, "CARDS_NOT_AVAILABLE_OR_REUSED");

        int total = first + second;
        if (total > 100)
            return new(false, false, total, null, "SUM_EXCEEDS_100");

        int optimalTotal = FindOptimalPairs(cards)[0].Total;
        return total == optimalTotal
            ? new(true, true, total, optimalTotal, "OPTIMAL_VALID_SUM")
            : new(true, false, total, optimalTotal, "CLOSER_SUM_EXISTS");
    }

    /// <summary>Draws seven distinct 0–9 cards from a shuffled deck.</summary>
    public static IReadOnlyList<int> DrawSeven(Random random)
    {
        ArgumentNullException.ThrowIfNull(random);
        int[] deck = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9];
        for (int i = deck.Length - 1; i > 0; i--)
        {
            int j = random.Next(i + 1);
            (deck[i], deck[j]) = (deck[j], deck[i]);
        }
        return deck.Take(7).ToArray();
    }
}

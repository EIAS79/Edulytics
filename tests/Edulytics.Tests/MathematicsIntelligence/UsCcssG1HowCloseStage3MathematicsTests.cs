using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssG1HowCloseStage3MathematicsTests
{
    [Fact]
    public void TwoDifferentOptimalCardSelectionsAreBothAccepted()
    {
        int[] cards = [0, 1, 2, 4, 5, 7, 9];
        var optimal = UsCcssG1HowCloseStage3Mathematics.FindOptimalPairs(cards);
        Assert.Contains(optimal, p => p.First == 49 && p.Second == 51 && p.Total == 100);
        Assert.Contains(optimal, p => p.First == 41 && p.Second == 59 && p.Total == 100);

        // Unlike string-only grading, the mathematics accepts equivalent
        // optimal answers even when the selected four cards differ.
        var a = UsCcssG1HowCloseStage3Mathematics.GradePair(cards, 51, 49);
        var b = UsCcssG1HowCloseStage3Mathematics.GradePair(cards, 41, 59);
        Assert.True(a.ValidSelection && a.Optimal);
        Assert.True(b.ValidSelection && b.Optimal);
        Assert.Equal("OPTIMAL_VALID_SUM", a.Reason);
        Assert.Equal(100, b.BestTotal);
    }

    [Fact]
    public void OriginalWithoutGoingOverRuleRejectsOverAndNonoptimalTotals()
    {
        int[] cards = [0, 1, 2, 4, 5, 7, 9];

        var over = UsCcssG1HowCloseStage3Mathematics.GradePair(cards, 92, 51);
        Assert.False(over.ValidSelection);
        Assert.False(over.Optimal);
        Assert.Equal(143, over.Total);
        Assert.Equal("SUM_EXCEEDS_100", over.Reason);

        var nonoptimal = UsCcssG1HowCloseStage3Mathematics.GradePair(cards, 20, 47);
        Assert.True(nonoptimal.ValidSelection);
        Assert.False(nonoptimal.Optimal);
        Assert.Equal(67, nonoptimal.Total);
        Assert.Equal(100, nonoptimal.BestTotal);
        Assert.Equal("CLOSER_SUM_EXISTS", nonoptimal.Reason);
    }

    [Theory]
    [InlineData(55, 49, "CARDS_NOT_AVAILABLE_OR_REUSED")]
    [InlineData(38, 41, "CARDS_NOT_AVAILABLE_OR_REUSED")]
    [InlineData(9, 51, "TWO_DIGIT_NUMBERS_REQUIRED")]
    public void OriginalRulesRejectCardReuseUnavailableCardsAndOneDigitValues(
        int left, int right, string expectedReason)
    {
        int[] cards = [0, 1, 2, 4, 5, 7, 9];
        var result = UsCcssG1HowCloseStage3Mathematics.GradePair(cards, left, right);
        Assert.False(result.ValidSelection);
        Assert.False(result.Optimal);
        Assert.Equal(expectedReason, result.Reason);
    }

    [Fact]
    public void EachDrawContainsSevenDistinctSourcePermittedCardsAndAtLeastOneOptimalAnswer()
    {
        for (int seed = 0; seed < 160; seed++)
        {
            var cards = UsCcssG1HowCloseStage3Mathematics.DrawSeven(new Random(seed * 97 + 11));
            Assert.True(UsCcssG1HowCloseStage3Mathematics.ValidCards(cards));
            Assert.DoesNotContain(10, cards); // teacher removes number-10 cards

            var solutions = UsCcssG1HowCloseStage3Mathematics.FindOptimalPairs(cards);
            Assert.NotEmpty(solutions);
            int best = solutions[0].Total;
            Assert.InRange(best, 1, 100);
            Assert.All(solutions, p =>
            {
                Assert.Equal(best, p.Total);
                Assert.Equal(100 - best, p.Gap);
                Assert.True(p.First <= p.Second);
                Assert.True(UsCcssG1HowCloseStage3Mathematics.GradePair(
                    cards, p.Second, p.First).Optimal);
            });
        }
    }

    [Fact]
    public void InvalidSevenCardPoolsFailClosed()
    {
        Assert.False(UsCcssG1HowCloseStage3Mathematics.ValidCards([0, 0, 1, 2, 3, 4, 5]));
        Assert.False(UsCcssG1HowCloseStage3Mathematics.ValidCards([0, 1, 2, 3, 4, 5, 10]));
        Assert.False(UsCcssG1HowCloseStage3Mathematics.ValidCards([0, 1, 2]));
        Assert.Throws<ArgumentException>(() =>
            UsCcssG1HowCloseStage3Mathematics.FindOptimalPairs([0, 0, 1, 2, 3, 4, 5]));
    }
}

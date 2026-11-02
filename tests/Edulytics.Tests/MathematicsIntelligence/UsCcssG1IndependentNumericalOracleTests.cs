using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Independent arithmetic oracles for selected Grade 1 source-bound contracts.
/// These compare generated canonical results to independently computed numerical
/// invariants; unlike round-trip tests they do not derive expected results from Solve.
/// They do NOT certify classroom activities, authentic UI, source-task parity or release.
/// </summary>
public sealed class UsCcssG1IndependentNumericalOracleTests
{
    [Fact]
    public void AllTwoDigitPairsHaveCorrectGreaterLessEqualTruthTable()
    {
        string[] families =
        [
            "usccss.g1.u04.l16.write_comparison",
            "usccss.g1.u04.l21.compare_nonstandard_forms"
        ];
        foreach (var family in families)
        {
            for (int first = 20; first <= 99; first++)
            for (int second = 20; second <= 99; second++)
            {
                var p = new Dictionary<string, int>
                {
                    ["a"] = first, ["b"] = second, ["c"] = 0, ["mode"] = 0
                };
                if (family == families[0])
                    p["c"] = 20; // comparison-symbol contract also requires c in range.

                var sign = first < second ? "<" : first > second ? ">" : "=";
                var expected = "SYMBOL=" + sign;
                Assert.Equal(expected, UsCcssG1FullGradeUnit4FinalQuestions.Solve(family, p));
                Assert.True(UsCcssG1FullGradeUnit4FinalQuestions.Verify(family, p, expected));

                string wrong = sign == ">" ? "<" : ">";
                Assert.False(UsCcssG1FullGradeUnit4FinalQuestions.Verify(
                    family, p, "SYMBOL=" + wrong));
            }
        }
    }

    [Fact]
    public void EveryTwoDigitRegroupingPreservesQuantityExactly()
    {
        const string family = "usccss.g1.u04.l19.regroup_one_ten";
        for (int value = 20; value <= 99; value++)
        {
            var p = new Dictionary<string, int>
            {
                ["a"] = value, ["b"] = 0, ["c"] = 0, ["mode"] = 2
            };
            int ones = value % 10 + 10;
            int tens = value / 10 - 1;
            Assert.Equal(value, tens * 10 + ones);
            Assert.Equal($"TENS={tens};ONES={ones}",
                UsCcssG1FullGradeUnit4FinalQuestions.Solve(family, p));
            Assert.False(UsCcssG1FullGradeUnit4FinalQuestions.Verify(
                family, p, $"TENS={tens + 1};ONES={ones}"));
        }
    }

    [Fact]
    public void AllClockHoursUseCorrectWholeAndHalfHourOutcomes()
    {
        // Each original-source family now has distinct response contracts by mode.
        // Verify all twelve hours and all four modes without downgrading newly
        // introduced analog/digital comparisons, word descriptions or 5-minute hops.
        string[] names =
        [
            "one", "two", "three", "four", "five", "six",
            "seven", "eight", "nine", "ten", "eleven", "twelve"
        ];
        const string hourFamily = "usccss.g1.u07.l13.hour_clock";
        const string halfFamily = "usccss.g1.u07.l14.half_hour_clock";
        const string writeFamily = "usccss.g1.u07.l15.write_hours_halfhours";

        for (int hour = 1; hour <= 12; hour++)
        for (int mode = 0; mode < 4; mode++)
        {
            int differentDigitalHour = hour == 12 ? 1 : hour + 1;
            var p = new Dictionary<string, int>
            {
                ["a"] = hour, ["b"] = differentDigitalHour, ["c"] = 0, ["mode"] = mode
            };
            string word = names[hour - 1];

            string hourExpected = mode switch
            {
                0 or 1 => $"TIME={hour}:00",
                2 => $"SAME=YES;TIME={hour}:00",
                _ => $"SAME=NO;TIME={hour}:00"
            };
            Assert.True(UsCcssG1FullGradeUnit7GeometryTimeQuestions.Valid(hourFamily, p));
            Assert.Equal(hourExpected,
                UsCcssG1FullGradeUnit7GeometryTimeQuestions.Solve(hourFamily, p));
            Assert.True(UsCcssG1FullGradeUnit7GeometryTimeQuestions.Verify(
                hourFamily, p, hourExpected));
            Assert.False(UsCcssG1FullGradeUnit7GeometryTimeQuestions.Verify(
                hourFamily, p, mode <= 1 ? $"TIME={hour}:30" :
                    $"SAME={(mode == 2 ? "NO" : "YES")};TIME={hour}:00"));

            string halfExpected = mode switch
            {
                0 or 1 => $"MINUTES=30;HOUR={hour}",
                2 => $"COLOR=BLUE;TIME={hour}:00;WORDS={word}_oclock",
                _ => $"COLOR=RED;TIME={hour}:30;WORDS=half_past_{word}"
            };
            Assert.Equal(halfExpected,
                UsCcssG1FullGradeUnit7GeometryTimeQuestions.Solve(halfFamily, p));
            Assert.True(UsCcssG1FullGradeUnit7GeometryTimeQuestions.Verify(
                halfFamily, p, halfExpected));
            Assert.False(UsCcssG1FullGradeUnit7GeometryTimeQuestions.Verify(
                halfFamily, p, halfExpected + ";FAKE=1"));

            string writeExpected = mode <= 1
                ? $"TIME={hour}:" + (mode == 0 ? "00" : "30")
                : "HOPS=6;MINUTES=30;STOP_AT=6";
            Assert.Equal(writeExpected,
                UsCcssG1FullGradeUnit7GeometryTimeQuestions.Solve(writeFamily, p));
            Assert.True(UsCcssG1FullGradeUnit7GeometryTimeQuestions.Verify(
                writeFamily, p, writeExpected));
            Assert.False(UsCcssG1FullGradeUnit7GeometryTimeQuestions.Verify(
                writeFamily, p, mode <= 1 ? writeExpected + ";UNVERIFIED=1" :
                    "HOPS=5;MINUTES=25;STOP_AT=5"));

            if (mode == 3)
            {
                // A digital card equal to the analog time cannot represent
                // the intentionally different-clock variant.
                p["b"] = hour;
                Assert.False(UsCcssG1FullGradeUnit7GeometryTimeQuestions.Valid(hourFamily, p));
            }
        }
    }
}

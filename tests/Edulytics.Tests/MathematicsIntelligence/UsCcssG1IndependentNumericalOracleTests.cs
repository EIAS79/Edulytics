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
                ["a"] = value, ["b"] = 0, ["c"] = 0, ["mode"] = 0
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
        for (int hour = 1; hour <= 12; hour++)
        for (int mode = 0; mode < 4; mode++)
        {
            var p = new Dictionary<string, int>
            {
                ["a"] = hour, ["b"] = 2, ["c"] = 0, ["mode"] = mode
            };

            Assert.Equal($"TIME={hour}:00",
                UsCcssG1FullGradeUnit7GeometryTimeQuestions.Solve(
                    "usccss.g1.u07.l13.hour_clock", p));

            Assert.Equal($"MINUTES=30;HOUR={hour}",
                UsCcssG1FullGradeUnit7GeometryTimeQuestions.Solve(
                    "usccss.g1.u07.l14.half_hour_clock", p));

            var clockExpected = $"TIME={hour}:" + (mode % 2 == 0 ? "00" : "30");
            const string clockFamily = "usccss.g1.u07.l15.write_hours_halfhours";
            Assert.Equal(clockExpected,
                UsCcssG1FullGradeUnit7GeometryTimeQuestions.Solve(clockFamily, p));
            Assert.True(UsCcssG1FullGradeUnit7GeometryTimeQuestions.Verify(
                clockFamily, p, clockExpected));
            Assert.False(UsCcssG1FullGradeUnit7GeometryTimeQuestions.Verify(
                clockFamily, p, clockExpected + ";UNVERIFIED=1"));
        }
    }
}

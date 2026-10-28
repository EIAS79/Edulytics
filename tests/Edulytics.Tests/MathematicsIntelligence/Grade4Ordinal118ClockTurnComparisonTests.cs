using System;
using System.Collections.Generic;
using System.Linq;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

// Stage C authored-only evidence: no .NET or GitHub Actions execution before Stage D.
public sealed class Grade4Ordinal118ClockTurnComparisonTests
{
    private const string Family = "usccss.g4.u07.l07.clock_hour_angle_turn_compare";

    [Fact]
    public void ExactHourGeneratedTurnsCompareCorrectlyAndRejectWrongMalformedOrTamperedInputs()
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(Family));
        var engine = new ExactSkillContractQuestionEngine();
        foreach (var seed in new[] { 20210707, 20261027, 118149 })
        {
            var items = engine.Generate("g4-2021-u07-l07", Family, [Family],
                ExactSkillQuestionDifficulty.Standard, 12, seed, []);
            Assert.Equal(12, items.Count);
            foreach (var item in items)
            {
                var p = item.Parameters;
                Assert.InRange(p["caseType"], 0, 7);
                Assert.InRange(p["variant"], 0, 2);
                Assert.InRange(p["questionKind"], 0, 1);
                Assert.InRange(p["hourA"], 1, 11);
                Assert.InRange(p["hourB"], 1, 11);
                Assert.Equal(Math.Min(p["hourA"], 12 - p["hourA"]), p["stepA"]);
                Assert.Equal(Math.Min(p["hourB"], 12 - p["hourB"]), p["stepB"]);
                var expected = p["questionKind"] == 0
                    ? (p["stepA"] > p["stepB"] ? 1 : p["stepA"] < p["stepB"] ? 2 : 3)
                    : Math.Abs(p["stepA"] - p["stepB"]);
                Assert.Equal(expected.ToString(), item.CorrectAnswer);
                Assert.True(ExactSkillContractQuestionEngine.Verify(Family, p, expected.ToString()));
                Assert.False(ExactSkillContractQuestionEngine.Verify(Family, p, (expected + 10).ToString()));
                Assert.False(ExactSkillContractQuestionEngine.Verify(Family, p, "?"));
                Assert.False(ExactSkillContractQuestionEngine.Verify(Family, p, ""));
                var altered = p.ToDictionary(x => x.Key, x => x.Value, StringComparer.Ordinal);
                altered["hourA"] = 0;
                Assert.False(ExactSkillContractQuestionEngine.Verify(Family, altered, expected.ToString()));
                altered["hourA"] = p["hourA"];
                altered["stepA"] = 7;
                Assert.False(ExactSkillContractQuestionEngine.Verify(Family, altered, expected.ToString()));
                altered["stepA"] = p["stepA"];
                altered["variant"] = 9;
                Assert.False(ExactSkillContractQuestionEngine.Verify(Family, altered, expected.ToString()));
                altered["variant"] = p["variant"];
                altered["caseType"] = p["caseType"] < 7 ? p["caseType"] + 1 : 0;
                Assert.False(ExactSkillContractQuestionEngine.Verify(Family, altered, expected.ToString()));
            }
        }
    }

    [Theory]
    [InlineData(0, 3, 5, 2, 2)]
    [InlineData(1, 8, 2, 1, 2)]
    [InlineData(2, 3, 9, 3, 0)]
    [InlineData(3, 6, 5, 1, 1)]
    [InlineData(4, 10, 4, 2, 2)]
    [InlineData(5, 7, 8, 1, 1)]
    [InlineData(6, 1, 11, 3, 0)]
    [InlineData(7, 4, 6, 2, 2)]
    public void EightExactHourSourceGoalFixturesCompareAndMeasureTheDifference(
        int mode, int hourA, int hourB, int expectedLarger, int expectedDifference)
    {
        var a = Math.Min(hourA, 12 - hourA);
        var b = Math.Min(hourB, 12 - hourB);
        Assert.Equal(expectedLarger, a > b ? 1 : a < b ? 2 : 3);
        Assert.Equal(expectedDifference, Math.Abs(a - b));
        var p = new Dictionary<string, int>(StringComparer.Ordinal)
        {
            ["caseType"] = mode, ["variant"] = 1,
            ["hourA"] = hourA, ["hourB"] = hourB,
            ["stepA"] = a, ["stepB"] = b, ["questionKind"] = 0
        };
        Assert.True(ExactSkillContractQuestionEngine.Verify(Family, p, expectedLarger.ToString()));
        p["questionKind"] = 1;
        Assert.True(ExactSkillContractQuestionEngine.Verify(Family, p, expectedDifference.ToString()));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family, p, (expectedDifference + 1).ToString()));
    }

    [Fact]
    public void SourceAndreQuarterTurnAndExampleClockAnglesAreIndependentlyCorrect()
    {
        Assert.Equal(3, Math.Min(3, 12 - 3)); // at exactly 3:00
        Assert.Equal(5, Math.Min(5, 12 - 5)); // at exactly 5:00
        Assert.Equal(6, Math.Min(6, 12 - 6)); // at exactly 6:00
        Assert.Equal(4, Math.Min(8, 12 - 8)); // at exactly 8:00
        Assert.Equal(90, 3 * 30);
        Assert.Equal(150, 5 * 30);
        Assert.Equal(180, 6 * 30);
        Assert.Equal(120, 4 * 30);
        Assert.Equal(30, (5 - 4) * 30);
    }
}

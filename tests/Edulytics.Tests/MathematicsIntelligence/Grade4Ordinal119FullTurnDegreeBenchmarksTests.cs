using System;
using System.Collections.Generic;
using System.Linq;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

// Stage C AUTHORING ONLY. All execution is deferred to Stage D.
public sealed class Grade4Ordinal119FullTurnDegreeBenchmarksTests
{
    private const string Family = "usccss.g4.u07.l08.full_turn_degree_benchmarks";

    [Fact]
    public void GeneratedDegreeBenchmarkItemsRejectWrongMalformedAndModifiedData()
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(Family));
        var engine = new ExactSkillContractQuestionEngine();
        foreach (var seed in new[] { 20210708, 20261027, 119149 })
        {
            var items = engine.Generate("g4-2021-u07-l08", Family, [Family],
                ExactSkillQuestionDifficulty.Standard, 12, seed, []);
            Assert.Equal(12, items.Count);
            foreach (var item in items)
            {
                var p = item.Parameters;
                Assert.InRange(p["caseType"], 0, 7);
                Assert.InRange(p["variant"], 0, 2);
                Assert.InRange(p["questionKind"], 0, 2);
                Assert.Equal(360 * p["numerator"], p["degrees"] * p["denominator"]);
                var expected = p["questionKind"] == 0 ? p["numerator"] :
                    p["questionKind"] == 1 ? p["denominator"] : 360 - p["degrees"];
                Assert.Equal(expected.ToString(), item.CorrectAnswer);
                Assert.True(ExactSkillContractQuestionEngine.Verify(Family, p, expected.ToString()));
                Assert.False(ExactSkillContractQuestionEngine.Verify(Family, p, (expected + 11).ToString()));
                Assert.False(ExactSkillContractQuestionEngine.Verify(Family, p, ""));
                Assert.False(ExactSkillContractQuestionEngine.Verify(Family, p, "?"));
                var modified = p.ToDictionary(x => x.Key, x => x.Value, StringComparer.Ordinal);
                modified["degrees"] = 359;
                Assert.False(ExactSkillContractQuestionEngine.Verify(Family, modified, expected.ToString()));
                modified["degrees"] = p["degrees"];
                modified["denominator"] = 0;
                Assert.False(ExactSkillContractQuestionEngine.Verify(Family, modified, expected.ToString()));
                modified["denominator"] = p["denominator"];
                modified["variant"] = 10;
                Assert.False(ExactSkillContractQuestionEngine.Verify(Family, modified, expected.ToString()));
                modified["variant"] = p["variant"];
                modified["questionKind"] = 6;
                Assert.False(ExactSkillContractQuestionEngine.Verify(Family, modified, expected.ToString()));
            }
        }
    }

    [Theory]
    [InlineData(0, 45, 1, 8)]
    [InlineData(1, 60, 1, 6)]
    [InlineData(2, 90, 1, 4)]
    [InlineData(3, 120, 1, 3)]
    [InlineData(4, 180, 1, 2)]
    [InlineData(5, 240, 2, 3)]
    [InlineData(6, 270, 3, 4)]
    [InlineData(7, 360, 1, 1)]
    public void EightIndependentReducedFractionAndFullTurnChecks(
        int mode, int degrees, int numerator, int denominator)
    {
        Assert.Equal(360 * numerator, degrees * denominator);
        var p = new Dictionary<string, int>(StringComparer.Ordinal)
        {
            ["caseType"] = mode, ["variant"] = 1, ["questionKind"] = 0,
            ["degrees"] = degrees, ["numerator"] = numerator, ["denominator"] = denominator
        };
        Assert.True(ExactSkillContractQuestionEngine.Verify(Family, p, numerator.ToString()));
        p["questionKind"] = 1;
        Assert.True(ExactSkillContractQuestionEngine.Verify(Family, p, denominator.ToString()));
        p["questionKind"] = 2;
        Assert.True(ExactSkillContractQuestionEngine.Verify(Family, p, (360 - degrees).ToString()));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family, p, (361 - degrees).ToString()));
    }

    [Fact]
    public void SourceFullQuarterHalfThreeQuarterAndThirdTurnsAreConsistent()
    {
        Assert.Equal(360, 4 * 90);
        Assert.Equal(360, 2 * 180);
        Assert.Equal(360, 3 * 120);
        Assert.Equal(360, 270 + 90);
        Assert.Equal(120, 360 / 3);
        Assert.Equal(90, 360 / 4);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

// Stage C authored only. DO NOT EXECUTE until Stage D.
public sealed class Grade4Ordinal114ParallelIntersectingLinesTests
{
    private const string Family = "usccss.g4.u07.l03.dot_grid_parallel_intersect";

    [Fact]
    public void GeneratedLinePairsRetainExactRelationAndRejectInvalidResponses()
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(Family));
        var engine = new ExactSkillContractQuestionEngine();
        foreach (var seed in new[] { 20210703, 20261027, 713014 })
        {
            var items = engine.Generate("g4-2021-u07-l03", Family, [Family],
                ExactSkillQuestionDifficulty.Standard, 8, seed, []);
            Assert.Equal(8, items.Count);
            foreach (var item in items)
            {
                var p = item.Parameters;
                Assert.InRange(p["caseType"], 0, 7);
                Assert.InRange(p["variant"], 0, 2);
                var expected = p["caseType"] < 4 ? "1" : "2";
                Assert.Equal(expected, item.CorrectAnswer);
                Assert.True(ExactSkillContractQuestionEngine.Verify(Family, p, expected));
                Assert.False(ExactSkillContractQuestionEngine.Verify(Family, p, expected == "1" ? "2" : "1"));
                Assert.False(ExactSkillContractQuestionEngine.Verify(Family, p, "?"));
                Assert.False(ExactSkillContractQuestionEngine.Verify(Family, p, ""));
                var invalid = p.ToDictionary(x => x.Key, x => x.Value, StringComparer.Ordinal);
                invalid["variant"] = 9;
                Assert.False(ExactSkillContractQuestionEngine.Verify(Family, invalid, expected));
                invalid["variant"] = p["variant"];
                invalid["r1"] = 0;
                Assert.False(ExactSkillContractQuestionEngine.Verify(Family, invalid, expected));
                invalid["r1"] = p["r1"];
                invalid["r2"] = invalid["r1"];
                invalid["c2"] = invalid["c1"];
                Assert.False(ExactSkillContractQuestionEngine.Verify(Family, invalid, expected));
            }
        }
    }

    // Fixtures are independently authored from the math of each dot-grid
    // pair; the family solver must not serve as its own answer oracle.
    [Theory]
    [InlineData(0, 1, 1, 1, 4, 3, 1, 3, 4, "1")]
    [InlineData(1, 1, 1, 4, 1, 1, 3, 4, 3, "1")]
    [InlineData(2, 1, 1, 3, 3, 1, 3, 3, 5, "1")]
    [InlineData(3, 1, 5, 3, 3, 1, 3, 3, 1, "1")]
    [InlineData(4, 2, 1, 2, 5, 1, 3, 4, 3, "2")]
    [InlineData(5, 2, 1, 2, 5, 1, 1, 4, 4, "2")]
    [InlineData(6, 1, 3, 4, 3, 1, 5, 4, 2, "2")]
    [InlineData(7, 1, 1, 4, 4, 1, 4, 4, 1, "2")]
    public void EightSourceGoalCasesHaveIndependentlyExpectedParallelOrIntersectingClass(
        int mode, int r1, int c1, int r2, int c2,
        int r3, int c3, int r4, int c4, string expected)
    {
        var p = new Dictionary<string, int>(StringComparer.Ordinal)
        {
            ["caseType"] = mode, ["variant"] = 0,
            ["r1"] = r1, ["c1"] = c1, ["r2"] = r2, ["c2"] = c2,
            ["r3"] = r3, ["c3"] = c3, ["r4"] = r4, ["c4"] = c4
        };
        var cross = (r2 - r1) * (c4 - c3) - (c2 - c1) * (r4 - r3);
        Assert.Equal(expected == "1", cross == 0);
        Assert.True(ExactSkillContractQuestionEngine.Verify(Family, p, expected));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family, p, expected == "1" ? "2" : "1"));
        var wrongRelation = p.ToDictionary(x => x.Key, x => x.Value, StringComparer.Ordinal);
        wrongRelation["caseType"] = mode < 4 ? 7 : 0;
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family, wrongRelation, expected));
    }

    [Fact]
    public void CoincidentOrMismatchedLineRelationCannotMasqueradeAsParallel()
    {
        var coincident = new Dictionary<string, int>(StringComparer.Ordinal)
        {
            ["caseType"] = 0, ["variant"] = 0,
            ["r1"] = 1, ["c1"] = 1, ["r2"] = 1, ["c2"] = 4,
            ["r3"] = 1, ["c3"] = 2, ["r4"] = 1, ["c4"] = 5
        };
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family, coincident, "1"));
        coincident["r3"] = 2;
        coincident["r4"] = 3;
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family, coincident, "1"));
    }
}

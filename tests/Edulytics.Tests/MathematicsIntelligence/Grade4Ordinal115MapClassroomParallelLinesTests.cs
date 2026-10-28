using System;
using System.Collections.Generic;
using System.Linq;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

// Stage C only: authored source-goal regression, intentionally NOT RUN.
public sealed class Grade4Ordinal115MapClassroomParallelLinesTests
{
    private const string Family = "usccss.g4.u07.l03.dot_grid_parallel_intersect";

    [Fact]
    public void ReusedExactLineRelationshipFamilySupportsLesson115WithoutAutoGradingMaps()
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(Family));
        var engine = new ExactSkillContractQuestionEngine();
        foreach (var seed in new[] { 20210704, 20261027 })
        {
            var items = engine.Generate("g4-2021-u07-l04", Family, [Family],
                ExactSkillQuestionDifficulty.Standard, 8, seed, []);
            Assert.Equal(8, items.Count);
            foreach (var item in items)
            {
                var p = item.Parameters;
                Assert.InRange(p["caseType"], 0, 7);
                var expected = p["caseType"] < 4 ? "1" : "2";
                Assert.Equal(expected, item.CorrectAnswer);
                Assert.True(ExactSkillContractQuestionEngine.Verify(Family, p, expected));
                Assert.False(ExactSkillContractQuestionEngine.Verify(Family, p, expected == "1" ? "2" : "1"));
                Assert.False(ExactSkillContractQuestionEngine.Verify(Family, p, "unverified map"));
                var damaged = p.ToDictionary(x => x.Key, x => x.Value, StringComparer.Ordinal);
                damaged["c3"] = 0;
                Assert.False(ExactSkillContractQuestionEngine.Verify(Family, damaged, expected));
            }
        }
    }

    [Fact]
    public void TransversalCrossesBothVerticalLinesWithoutMakingThoseParallelLinesMeet()
    {
        var originalParallelPair = new Dictionary<string, int>(StringComparer.Ordinal)
        {
            ["caseType"] = 1, ["variant"] = 1,
            ["r1"] = 1, ["c1"] = 1, ["r2"] = 4, ["c2"] = 1,
            ["r3"] = 1, ["c3"] = 4, ["r4"] = 4, ["c4"] = 4
        };
        Assert.True(ExactSkillContractQuestionEngine.Verify(Family, originalParallelPair, "1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family, originalParallelPair, "2"));

        // Third horizontal line through row 3 crosses the first vertical line.
        // This tests the OTHER pair: vertical + transversal, not a change to
        // the two distinct original vertical parallel lines.
        var verticalAndTransversal = new Dictionary<string, int>(StringComparer.Ordinal)
        {
            ["caseType"] = 4, ["variant"] = 1,
            ["r1"] = 1, ["c1"] = 1, ["r2"] = 4, ["c2"] = 1,
            ["r3"] = 3, ["c3"] = 1, ["r4"] = 3, ["c4"] = 4
        };
        Assert.True(ExactSkillContractQuestionEngine.Verify(Family, verticalAndTransversal, "2"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family, verticalAndTransversal, "1"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Family, originalParallelPair, "1"));
    }
}

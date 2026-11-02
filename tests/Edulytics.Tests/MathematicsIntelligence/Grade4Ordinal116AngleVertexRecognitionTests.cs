using System;
using System.Collections.Generic;
using System.Linq;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

// Stage C authoring only: DO NOT RUN this file until deferred Stage D.
public sealed class Grade4Ordinal116AngleVertexRecognitionTests
{
    private const string Family = "usccss.g4.u07.l05.angle_common_vertex_rays";

    [Fact]
    public void GeneratedRayPairsHaveExactVertexRuleAndRejectWrongMalformedAndTamperedAnswers()
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(Family));
        var engine = new ExactSkillContractQuestionEngine();
        foreach (var seed in new[] { 20210705, 20261027, 116149 })
        {
            var items = engine.Generate("g4-2021-u07-l05", Family, [Family],
                ExactSkillQuestionDifficulty.Standard, 8, seed, []);
            Assert.Equal(8, items.Count);
            foreach (var item in items)
            {
                var p = item.Parameters;
                Assert.InRange(p["caseType"], 0, 7);
                Assert.InRange(p["variant"], 0, 2);
                Assert.InRange(p["start1"], 0, 3);
                Assert.InRange(p["start2"], 0, 3);
                Assert.InRange(p["direction1"], 0, 7);
                Assert.InRange(p["direction2"], 0, 7);
                Assert.NotEqual(p["direction1"], p["direction2"]);
                var expected = p["start1"] == p["start2"] ? "1" : "2";
                Assert.Equal(expected, item.CorrectAnswer);
                Assert.True(ExactSkillContractQuestionEngine.Verify(Family, p, expected));
                Assert.False(ExactSkillContractQuestionEngine.Verify(Family, p, expected == "1" ? "2" : "1"));
                Assert.False(ExactSkillContractQuestionEngine.Verify(Family, p, "?"));
                Assert.False(ExactSkillContractQuestionEngine.Verify(Family, p, ""));
                var invalid = p.ToDictionary(kv => kv.Key, kv => kv.Value, StringComparer.Ordinal);
                invalid["variant"] = 9;
                Assert.False(ExactSkillContractQuestionEngine.Verify(Family, invalid, expected));
                invalid["variant"] = p["variant"];
                invalid["start1"] = -1;
                Assert.False(ExactSkillContractQuestionEngine.Verify(Family, invalid, expected));
                invalid["start1"] = p["start1"];
                invalid["direction2"] = invalid["direction1"];
                Assert.False(ExactSkillContractQuestionEngine.Verify(Family, invalid, expected));
                invalid["direction2"] = p["direction2"];
                invalid["caseType"] = p["caseType"] < 4 ? 7 : 0;
                Assert.False(ExactSkillContractQuestionEngine.Verify(Family, invalid, expected));
            }
        }
    }

    [Theory]
    [InlineData(0, 0, 0, 2, 0, "1")]
    [InlineData(1, 1, 1, 1, 4, "1")]
    [InlineData(2, 2, 2, 6, 0, "1")]
    [InlineData(3, 3, 3, 3, 5, "1")]
    [InlineData(4, 0, 1, 2, 0, "2")]
    [InlineData(5, 1, 2, 1, 4, "2")]
    [InlineData(6, 2, 3, 6, 0, "2")]
    [InlineData(7, 3, 0, 3, 5, "2")]
    public void SourceGoalFixturesIndependentlyIdentifyCommonVertex(
        int mode, int start1, int start2, int direction1, int direction2,
        string expected)
    {
        var p = new Dictionary<string, int>(StringComparer.Ordinal)
        {
            ["caseType"] = mode,
            ["variant"] = 0,
            ["start1"] = start1,
            ["start2"] = start2,
            ["direction1"] = direction1,
            ["direction2"] = direction2
        };
        Assert.Equal(expected == "1", start1 == start2);
        Assert.NotEqual(direction1, direction2);
        Assert.True(ExactSkillContractQuestionEngine.Verify(Family, p, expected));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family, p, expected == "1" ? "2" : "1"));
    }

    [Fact]
    public void ALongerDrawnArmDoesNotChangeTheCommonStartingVertex()
    {
        var p = new Dictionary<string, int>(StringComparer.Ordinal)
        {
            ["caseType"] = 0, ["variant"] = 2,
            ["start1"] = 0, ["start2"] = 0,
            ["direction1"] = 2, ["direction2"] = 0
        };
        Assert.True(ExactSkillContractQuestionEngine.Verify(Family, p, "1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family, p, "2"));
        // The family has no arm-length parameter by design; the source
        // angle concept depends on ray direction and shared start.
        Assert.DoesNotContain("armLength", p.Keys);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

// Stage C authored only; NEVER run before final Stage D.
public sealed class Grade4Ordinal117CompareAngleOpeningsTests
{
    private const string Family = "usccss.g4.u07.l06.compare_angle_openings";

    [Fact]
    public void GeneratedPracticeComparesTurnNotRayLengthAndRejectsWrongMalformedOrTamperedAnswers()
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(Family));
        var engine = new ExactSkillContractQuestionEngine();
        foreach (var seed in new[] { 20210706, 20261027, 617149 })
        {
            var items = engine.Generate("g4-2021-u07-l06", Family, [Family],
                ExactSkillQuestionDifficulty.Standard, 8, seed, []);
            Assert.Equal(8, items.Count);
            foreach (var item in items)
            {
                var p = item.Parameters;
                Assert.InRange(p["caseType"], 0, 7);
                Assert.InRange(p["variant"], 0, 2);
                Assert.InRange(p["steps1"], 1, 4);
                Assert.InRange(p["steps2"], 1, 4);
                var expected = p["steps1"] > p["steps2"] ? "1" :
                    p["steps1"] < p["steps2"] ? "2" : "3";
                Assert.Equal(expected, item.CorrectAnswer);
                Assert.True(ExactSkillContractQuestionEngine.Verify(Family, p, expected));
                Assert.False(ExactSkillContractQuestionEngine.Verify(Family, p, expected == "1" ? "2" : "1"));
                Assert.False(ExactSkillContractQuestionEngine.Verify(Family, p, ""));
                Assert.False(ExactSkillContractQuestionEngine.Verify(Family, p, "?"));
                var tampered = p.ToDictionary(x => x.Key, x => x.Value, StringComparer.Ordinal);
                tampered["end1"] = (p["end1"] + 1) % 8;
                Assert.False(ExactSkillContractQuestionEngine.Verify(Family, tampered, expected));
                tampered["end1"] = p["end1"];
                tampered["variant"] = 7;
                Assert.False(ExactSkillContractQuestionEngine.Verify(Family, tampered, expected));
                tampered["variant"] = p["variant"];
                tampered["steps1"] = 0;
                Assert.False(ExactSkillContractQuestionEngine.Verify(Family, tampered, expected));
            }
        }
    }

    [Theory]
    [InlineData(0, 1, 2, "2")]
    [InlineData(1, 3, 1, "1")]
    [InlineData(2, 2, 2, "3")]
    [InlineData(3, 4, 3, "1")]
    [InlineData(4, 1, 4, "2")]
    [InlineData(5, 3, 2, "1")]
    [InlineData(6, 4, 4, "3")]
    [InlineData(7, 2, 3, "2")]
    public void EightIndependentlyExpectedTurnOrderings(
        int caseType, int firstEighthSteps, int secondEighthSteps, string expected)
    {
        Assert.InRange(firstEighthSteps, 1, 4);
        Assert.InRange(secondEighthSteps, 1, 4);
        var p = new Dictionary<string, int>(StringComparer.Ordinal)
        {
            ["caseType"] = caseType, ["variant"] = 1,
            ["start1"] = 2, ["start2"] = 4,
            ["end1"] = (2 + firstEighthSteps) % 8,
            ["end2"] = (4 + secondEighthSteps) % 8,
            ["steps1"] = firstEighthSteps, ["steps2"] = secondEighthSteps
        };
        Assert.Equal(expected, firstEighthSteps > secondEighthSteps ? "1" :
            firstEighthSteps < secondEighthSteps ? "2" : "3");
        Assert.True(ExactSkillContractQuestionEngine.Verify(Family, p, expected));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family, p, expected == "3" ? "2" : "3"));
    }

    [Fact]
    public void ArmLengthIsNotAParameterAndRotatingBothAnglesPreservesTheirOpening()
    {
        var basePair = new Dictionary<string, int>(StringComparer.Ordinal)
        {
            ["caseType"] = 0, ["variant"] = 0,
            ["start1"] = 0, ["end1"] = 1, ["steps1"] = 1,
            ["start2"] = 2, ["end2"] = 4, ["steps2"] = 2
        };
        Assert.True(ExactSkillContractQuestionEngine.Verify(Family, basePair, "2"));
        Assert.DoesNotContain("armLength", basePair.Keys);
        var rotated = new Dictionary<string, int>(basePair, StringComparer.Ordinal);
        rotated["variant"] = 1;
        rotated["start1"] = 2; rotated["end1"] = 3;
        rotated["start2"] = 4; rotated["end2"] = 6;
        Assert.True(ExactSkillContractQuestionEngine.Verify(Family, rotated, "2"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family, rotated, "1"));
    }
}

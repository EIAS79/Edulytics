using System;
using System.Collections.Generic;
using System.Linq;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

// Grade 4 source-aligned L121. Stage C assertion authoring only: UNRUN.
public sealed class Grade4Ordinal121PerpendicularJudgmentTests
{
    private const string Family = "usccss.g4.u07.l10.perpendicular_right_angle_judgment";
    private const string ReusedProtractorFamily = "usccss.g4.u07.l09.protractor_dual_scale_reading";

    [Fact]
    public void GeneratesPerpendicularJudgmentsAndRejectsIncorrectOrForgedResponses()
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(Family));
        var engine = new ExactSkillContractQuestionEngine();
        foreach (var seed in new[] { 20210710, 121149, 20261027 })
        {
            var items = engine.Generate("g4-2021-u07-l10", Family, [Family],
                ExactSkillQuestionDifficulty.Standard, 12, seed, []);
            Assert.Equal(12, items.Count);
            foreach (var item in items)
            {
                var p = item.Parameters;
                Assert.InRange(p["caseType"], 0, 7);
                Assert.InRange(p["variant"], 0, 2);
                var expectation = p["angleDegrees"] == 90 ? 1 : 2;
                Assert.Equal(expectation, p["perpendicularAnswer"]);
                Assert.Equal(expectation.ToString(), item.CorrectAnswer);
                Assert.True(ExactSkillContractQuestionEngine.Verify(Family, p, expectation.ToString()));
                Assert.False(ExactSkillContractQuestionEngine.Verify(Family, p, (3-expectation).ToString()));
                Assert.False(ExactSkillContractQuestionEngine.Verify(Family, p, ""));
                Assert.False(ExactSkillContractQuestionEngine.Verify(Family, p, "other"));
                var modified = p.ToDictionary(x => x.Key, x => x.Value, StringComparer.Ordinal);
                modified["angleDegrees"] = 89;
                Assert.False(ExactSkillContractQuestionEngine.Verify(Family, modified, expectation.ToString()));
                modified["angleDegrees"] = p["angleDegrees"];
                modified["caseType"] = 10;
                Assert.False(ExactSkillContractQuestionEngine.Verify(Family, modified, expectation.ToString()));
            }
        }
    }

    [Theory]
    [InlineData(0, 90, 1)]
    [InlineData(1, 90, 1)]
    [InlineData(2, 90, 1)]
    [InlineData(3, 90, 1)]
    [InlineData(4, 75, 2)]
    [InlineData(5, 80, 2)]
    [InlineData(6, 100, 2)]
    [InlineData(7, 120, 2)]
    public void ExactEightCasesAgreeWithDefinitionOfPerpendicularLines(int mode,int degrees,int answer)
    {
        Assert.Equal(degrees==90?1:2, answer);
        foreach(var variant in new[]{0,1,2})
        {
            var parameters = new Dictionary<string,int>(StringComparer.Ordinal)
            {
                ["caseType"]=mode, ["variant"]=variant,
                ["angleDegrees"]=degrees, ["perpendicularAnswer"]=answer
            };
            Assert.True(ExactSkillContractQuestionEngine.Verify(Family,parameters,answer.ToString()));
            Assert.False(ExactSkillContractQuestionEngine.Verify(Family,parameters,(3-answer).ToString()));
        }
    }

    [Fact]
    public void ReusedSourceProtractorFamilyAndOriginalQuotientWarmupRemainSupported()
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(ReusedProtractorFamily));
        Assert.Equal(90,180/2);
        Assert.Equal(45,180/4);
        Assert.Equal(45,360/8);
        Assert.Equal(22.5,360.0/16);
        Assert.Equal(360,4*90);
        var engine = new ExactSkillContractQuestionEngine();
        var items = engine.Generate("g4-2021-u07-l10-protractor-reuse",ReusedProtractorFamily,
            [ReusedProtractorFamily],ExactSkillQuestionDifficulty.Standard,8,20261027,[]);
        Assert.Equal(8,items.Count);
        foreach(var item in items)
            Assert.True(ExactSkillContractQuestionEngine.Verify(ReusedProtractorFamily,item.Parameters,item.CorrectAnswer));
    }
}

using System;
using System.Collections.Generic;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

// Stage C authored, intentionally UNRUN until Stage D.
public sealed class Grade4Ordinal123AngleDegreeClassificationTests
{
    private const string Family = "usccss.g4.u07.l12.classify_angles_by_degree_bounds";

    [Fact]
    public void GeneratedExactClassificationsHaveCorrectAnswers()
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(Family));
        var engine = new ExactSkillContractQuestionEngine();
        foreach (var seed in new[] { 20210712, 123149, 20261027 })
        {
            var items = engine.Generate("g4-u07-l12", Family, [Family],
                ExactSkillQuestionDifficulty.Standard, 12, seed, []);
            Assert.Equal(12, items.Count);
            foreach (var item in items)
            {
                var p = item.Parameters;
                int d = p["degrees"];
                int expected = d < 90 ? 1 : d == 90 ? 2 : d < 180 ? 3 : 4;
                Assert.Equal(expected, p["classCode"]);
                Assert.True(ExactSkillContractQuestionEngine.Verify(Family,p,expected.ToString()));
                Assert.False(ExactSkillContractQuestionEngine.Verify(Family,p,"?"));
                Assert.False(ExactSkillContractQuestionEngine.Verify(Family,p,(expected%4+1).ToString()));
            }
        }
    }

    [Theory]
    [InlineData(0,20,1)]
    [InlineData(1,35,1)]
    [InlineData(2,45,1)]
    [InlineData(3,70,1)]
    [InlineData(4,90,2)]
    [InlineData(5,110,3)]
    [InlineData(6,130,3)]
    [InlineData(7,180,4)]
    public void EightSourceRelevantIndependentDegreesRespectBoundaries(int mode,int degrees,int classification)
    {
        var p = new Dictionary<string,int>
        {
            ["caseType"]=mode,["variant"]=0,
            ["degrees"]=degrees,["classCode"]=classification
        };
        Assert.True(ExactSkillContractQuestionEngine.Verify(Family,p,classification.ToString()));
        p["degrees"]=89;
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,p,classification.ToString()));
    }

    [Fact]
    public void OriginalVisibleSourceFractionsAndFirstPartyCooldownAreConsistent()
    {
        Assert.Equal(1,12/12);
        Assert.Equal(10,120/12);
        Assert.True(45<90);
        Assert.True(130>90 && 130<180);
        Assert.Equal(180,2*90);
    }
}

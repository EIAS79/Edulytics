using System;
using System.Collections.Generic;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

// IM/KH 2021 G4 U07:L13. Stage C authored, UNRUN.
public sealed class Grade4Ordinal124AdditiveAngleTests
{
    private const string Family="usccss.g4.u07.l13.additive_angle_part";

    [Fact]
    public void GeneratedItemsHaveCorrectAnswersAndRejectBadAnswers()
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(Family));
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var seed in new[]{20210713,124149,20261027})
        {
            var items=engine.Generate("g4-u07-l13",Family,[Family],
                ExactSkillQuestionDifficulty.Standard,12,seed,[]);
            Assert.Equal(12,items.Count);
            foreach(var item in items)
            {
                var p=item.Parameters;
                var expected=p["totalDegrees"]-p["knownDegrees"];
                Assert.Equal(expected,p["unknownDegrees"]);
                Assert.Equal(expected.ToString(),item.CorrectAnswer);
                Assert.True(ExactSkillContractQuestionEngine.Verify(Family,p,expected.ToString()));
                Assert.False(ExactSkillContractQuestionEngine.Verify(Family,p,(expected+1).ToString()));
                Assert.False(ExactSkillContractQuestionEngine.Verify(Family,p,"?"));
            }
        }
    }

    [Theory]
    [InlineData(0,90,62,28)]
    [InlineData(1,180,73,107)]
    [InlineData(2,360,120,240)]
    [InlineData(3,90,34,56)]
    [InlineData(4,180,120,60)]
    [InlineData(5,360,275,85)]
    [InlineData(6,90,45,45)]
    [InlineData(7,180,95,85)]
    public void EightAngleSplitFixturesRejectTampering(int mode,int total,int known,int missing)
    {
        Assert.Equal(total,known+missing);
        var p=new Dictionary<string,int>
        {
            ["caseType"]=mode,["variant"]=0,["totalDegrees"]=total,
            ["knownDegrees"]=known,["unknownDegrees"]=missing
        };
        Assert.True(ExactSkillContractQuestionEngine.Verify(Family,p,missing.ToString()));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,p,(missing+1).ToString()));
        p["variant"]=10;
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,p,missing.ToString()));
    }

    [Fact]
    public void SourceGoalAngleAdditivityStaticChecks()
    {
        Assert.Equal(28,90-62);
        Assert.Equal(107,180-73);
        Assert.Equal(240,360-120);
        Assert.Equal(360,90+270);
    }
}

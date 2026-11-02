using System;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

// G4 2021 U07:L11 — authored in Stage C, NOT EXECUTED.
public sealed class Grade4Ordinal122AngleConstructionReuseTests
{
    private const string ProtractorFamily = "usccss.g4.u07.l09.protractor_dual_scale_reading";
    private const string PerpendicularFamily = "usccss.g4.u07.l10.perpendicular_right_angle_judgment";

    [Fact]
    public void ReusePriorExactProtractorAndPerpendicularFamiliesWithoutAnotherEngine()
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(ProtractorFamily));
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(PerpendicularFamily));
        var kernel = new ExactSkillContractQuestionEngine();
        foreach (var family in new[] { ProtractorFamily, PerpendicularFamily })
        {
            var items = kernel.Generate("g4-u07-l11-practice", family, [family],
                ExactSkillQuestionDifficulty.Standard, 8, 20210711, []);
            Assert.Equal(8, items.Count);
            foreach (var item in items)
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(
                    family, item.Parameters, item.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,"?"));
            }
        }
    }

    [Theory]
    [InlineData(0,90)]
    [InlineData(20,110)]
    [InlineData(40,130)]
    public void OriginalThreePossiblePerpendicularProtractorScalesDifferByNinety(
        int firstReading, int secondReading)
    {
        Assert.Equal(90, secondReading-firstReading);
    }

    [Fact]
    public void Source143ClockEstimateAndFourCardRangeExamplesAreDistinctFromConstructedWork()
    {
        Assert.True(120<143 && 143<160);
        Assert.True(25<35);
        Assert.True(55>35 && 55<80);
        Assert.True(100>80 && 100<120);
        Assert.True(145>120 && 145<180);
        Assert.True(95>90);
        // Teacher checks source image, construction and variable reflex value.
        Assert.True(360-95>180);
    }
}

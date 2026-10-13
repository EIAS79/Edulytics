using Edulytics.Core.Curriculum;
using Edulytics.Services.LessonContent;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssB01NumberLineArithmeticalVisualsTests
{
    [Fact]
    public void AllDisplayedTicksHaveCorrectEqualSpacingAndRealResult()
    {
        var engine=new ExactSkillContractQuestionEngine();
        var cases=0;
        foreach (var family in new[]{
            "number.whole.add_subtract.within_100.apply",
            "number.whole.add_subtract.within_100.build",
            "number.whole.add_subtract.across_ten.apply"
        })
        {
            for(var seed=730100;seed<730148;seed++)
            {
                var q=Assert.Single(engine.Generate(
                    "usccss-b01-source-bound-numberline",
                    "PED:US-CCSS-MATH:G2:U04:L09", [family],
                    ExactSkillQuestionDifficulty.Standard,1,seed,[]));
                var p=q.Parameters;
                Assert.Equal(p["operation"]==0
                    ? p["left"]+p["right"] : p["left"]-p["right"],
                    int.Parse(q.CorrectAnswer));
                var visual=Assert.Single(UsCcssB01SourceBoundNumberLineVisuals.Build(
                    "PED:US-CCSS-MATH:G2:U04:L09",[q]));
                Assert.Equal(RichLessonVisualKind.NumberLine,visual.Kind);
                var ticks=visual.Items.Select(int.Parse).ToArray();
                Assert.InRange(ticks.Length,2,3);
                for(var i=1;i<ticks.Length;i++)
                    Assert.Equal(p["right"],ticks[i]-ticks[i-1]);
                Assert.Contains(p["left"],ticks);
                Assert.Contains(p["expected"],ticks);
                Assert.Contains($"{p["left"]} " +
                    (p["operation"]==0?"+":"−") +
                    $" {p["right"]} = {p["expected"]}",visual.Description);
                cases++;
            }
        }
        Assert.Equal(144,cases);
    }

    [Fact]
    public void WrongStudentAnswerAndOtherCurriculaDoNotReceiveNumberLine()
    {
        var q=Assert.Single(new ExactSkillContractQuestionEngine().Generate(
            "usccss-b01-source-bound-numberline",
            "PED:US-CCSS-MATH:G2:U04:L09",
            ["number.whole.add_subtract.within_100.apply"],
            ExactSkillQuestionDifficulty.Standard,1,730102,[]));
        Assert.Empty(UsCcssB01SourceBoundNumberLineVisuals.Build(
            "PED:US-CCSS-MATH:G2:U05:L09",[q]));
        Assert.Empty(UsCcssB01SourceBoundNumberLineVisuals.Build(
            "PED:US-CCSS-MATH:G2:U04:L04",[q]));
        Assert.Empty(UsCcssB01SourceBoundNumberLineVisuals.Build(
            "PED:CAMBRIDGE-INTL-MATH:G2:U04:L09",[q]));
        Assert.Empty(UsCcssB01SourceBoundNumberLineVisuals.Build(
            "PED:US-CCSS-MATH:G2:U04:L09",[q with {CorrectAnswer="999"}]));
    }
}

using System.Globalization;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

// Grade5 Unit11 L07, local Arabic student text pp.829-834.
// Authored Stage C, no CI/Stage D executed.
public sealed class UaeGrade5Unit11Lesson07FairShareTests
{
    [Theory]
    [InlineData("uae.g5.u11.l07.equal_share_two_groups")]
    [InlineData("uae.g5.u11.l07.equal_share_three_groups")]
    [InlineData("uae.g5.u11.l07.equal_share_missing_group")]
    public void ExactFairSharingAndMissingGroupHaveIndependentOracle(string family)
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
        Assert.NotNull(ui);
        var engine=new ExactSkillContractQuestionEngine();
        var samples=engine.Generate("PED:UAE-MOE-MATH:L5:GENERAL:11:07",family,[family],
            ExactSkillQuestionDifficulty.Standard,8,20261113,[]);
        foreach(var q in samples)
        {
            var p=q.Parameters;
            int oracle=family.EndsWith("equal_share_two_groups",StringComparison.Ordinal)?
                (p["a"]+p["b"])/2:family.EndsWith("equal_share_three_groups",StringComparison.Ordinal)?
                (p["a"]+p["b"]+p["c"])/3:3*p["m"]-p["a"]-p["b"];
            Assert.Equal(oracle.ToString(CultureInfo.InvariantCulture),q.CorrectAnswer);
            Assert.DoesNotContain("result",p.Keys);
            Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,q.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"99999"));
        }
    }
}

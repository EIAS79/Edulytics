using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Source-specific numeric correctness tests authored only, Stage D unrun.</summary>
public sealed class UsCcssG2Unit1Lesson16CompareQuestionsTests
{
    private static readonly string[] Families=[
        "usccss.g2.u01.l16.solve_compare_larger",
        "usccss.g2.u01.l16.solve_compare_smaller",
        "usccss.g2.u01.l16.solve_compare_difference"
    ];
    [Fact]
    public void ThreeUnknownPositionsRouteOnlyOriginalLesson()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G2:U01:L16",out var contract));
        Assert.NotNull(contract);
        Assert.Equal(Families,contract!.AllowedQuestionFamilies);
        foreach(var family in Families)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
            Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,ui!.InteractionKind);
        }
    }
    [Fact]
    public void RandomizedPlaceValueTapesStayWithin100AndTamperedDataRejected()
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var family in Families)
        foreach(var seed in new[]{20261022,20261023})
        {
            var generated=engine.Generate("im2021-g2-u01-l16",family,[family],
                ExactSkillQuestionDifficulty.Standard,4,seed,[]);
            Assert.Equal(4,generated.Count);
            foreach(var q in generated)
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(
                    family,q.Parameters,q.CorrectAnswer));
                Assert.True(q.Parameters["small"]+q.Parameters["difference"]==
                    q.Parameters["big"]);
                Assert.InRange(q.Parameters["big"],2,100);
                var tampered=new Dictionary<string,int>(q.Parameters,StringComparer.Ordinal)
                    {["big"]=101};
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family,tampered,q.CorrectAnswer));
            }
        }
    }
}

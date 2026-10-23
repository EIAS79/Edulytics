using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade2 L14 authored during Stage C, execution held for D.</summary>
public sealed class UsCcssG2Unit1Lesson14TapeDiagramQuestionsTests
{
    private static readonly string[] Families=[
        "usccss.g2.u01.l14.tape_unknown_gap",
        "usccss.g2.u01.l14.tape_addition_missing_part",
        "usccss.g2.u01.l14.tape_subtraction_missing_part"
    ];
    [Fact]
    public void ThreeSourceTapeNumericalFamiliesRouteOnlyL14()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G2:U01:L14",out var contract));
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
    public void EachGeneratedTapeGapWithin20ValidatesAndRejectsChangedValues()
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var family in Families)
        foreach(var seed in new[]{20261022,20261023})
        {
            var items=engine.Generate("im2021-g2-u01-l14",family,[family],
                ExactSkillQuestionDifficulty.Standard,5,seed,[]);
            Assert.Equal(5,items.Count);
            foreach(var item in items)
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(
                    family,item.Parameters,item.CorrectAnswer));
                var a=item.Parameters["small"];
                var b=item.Parameters["big"];
                Assert.InRange(a,1,19);
                Assert.InRange(b,2,20);
                Assert.True(b>a);
                var invalid=new Dictionary<string,int>(item.Parameters,StringComparer.Ordinal)
                    {["big"]=21};
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family,invalid,item.CorrectAnswer));
            }
        }
    }
}

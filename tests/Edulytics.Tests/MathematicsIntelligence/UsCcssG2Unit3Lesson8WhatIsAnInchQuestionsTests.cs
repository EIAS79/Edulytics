using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG2Unit3Lesson8WhatIsAnInchQuestionsTests
{
    [Fact]
    public void FourSourceInchSkillsResolveWithIntegerWholeInchAnswer()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G2:U03:L08",out var c));
        Assert.NotNull(c);
        Assert.Equal(4,c!.AllowedQuestionFamilies.Count);
        foreach(var f in c.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var interaction));
            Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,interaction!.InteractionKind);
        }
    }
    [Fact]
    public void RandomVirtualEndpointsNeverRequireHalfInchesAndUseExactWholeChoices()
    {
        var engine=new ExactSkillContractQuestionEngine();
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G2:U03:L08",out var c));
        foreach(var family in c!.AllowedQuestionFamilies)
        foreach(var seed in new[]{20261022,20261023})
        {
            var questions=engine.Generate("im2021-g2-u03-l08",family,[family],
                ExactSkillQuestionDifficulty.Standard,4,seed,[]);
            Assert.Equal(4,questions.Count);
            foreach(var q in questions)
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(
                    family,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family,q.Parameters,"-1"));
                if(family.EndsWith("below_half",StringComparison.Ordinal))
                    Assert.InRange(q.Parameters["tenths"],1,4);
                else if(family.EndsWith("above_half",StringComparison.Ordinal))
                    Assert.InRange(q.Parameters["tenths"],6,9);
                else Assert.Equal(0,q.Parameters["tenths"]);
                var altered=new Dictionary<string,int>(q.Parameters,StringComparer.Ordinal)
                    { ["whole"]=99 };
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family,altered,q.CorrectAnswer));
            }
        }
    }
}

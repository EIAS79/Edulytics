using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Four exact zoo question families, no-ten regroup tested at D.</summary>
public sealed class UsCcssG2Unit2Lesson3ZooStoryQuestionsTests
{
    [Fact]
    public void FourSourceStructuresUseNumericEntryInsteadOfRelationSymbols()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G2:U02:L03",out var c));
        Assert.NotNull(c);
        Assert.Equal(4,c!.AllowedQuestionFamilies.Count);
        foreach(var family in c.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var interaction));
            Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,interaction!.InteractionKind);
        }
    }
    [Fact]
    public void RandomizedProblemsStayUnder100AndNeverBorrowOrCarry()
    {
        var engine=new ExactSkillContractQuestionEngine();
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G2:U02:L03",out var contract));
        foreach(var family in contract!.AllowedQuestionFamilies)
        foreach(var seed in new[]{20261022,20261023})
        {
            var questions=engine.Generate("im2021-g2-u02-l03",family,[family],
                ExactSkillQuestionDifficulty.Standard,4,seed,[]);
            Assert.Equal(4,questions.Count);
            foreach(var q in questions)
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(
                    family,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family,q.Parameters,"-1"));
                var altered=new Dictionary<string,int>(q.Parameters,StringComparer.Ordinal);
                if(family.EndsWith("start_unknown",StringComparison.Ordinal)||
                   family.EndsWith("part_unknown",StringComparison.Ordinal))
                {
                    Assert.Equal(q.Parameters["part"]+q.Parameters["other"],q.Parameters["total"]);
                    Assert.True(q.Parameters["part"]%10+q.Parameters["other"]%10<10);
                    altered["total"]=101;
                }
                else
                {
                    Assert.True(q.Parameters["bigger"]%10>=q.Parameters["smaller"]%10);
                    altered["bigger"]=101;
                }
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family,altered,q.CorrectAnswer));
            }
        }
    }
}

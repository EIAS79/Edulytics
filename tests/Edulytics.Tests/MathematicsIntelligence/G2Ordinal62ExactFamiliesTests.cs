using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal62ExactFamiliesTests
{
    [Fact]
    public void FourSourceLimitedFamiliesHaveExactStudentNumericalInteractions()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:US-CCSS-MATH:G2:U04:L10",out var c));
        Assert.NotNull(c);
        Assert.Equal(4,c!.AllowedQuestionFamilies.Count);
        foreach(var family in c.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
            Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,ui!.InteractionKind);
        }
    }
    [Fact]
    public void GeneratedCalculationsAreRecomputedAndTamperedInputsFail()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:US-CCSS-MATH:G2:U04:L10",out var c));
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var family in c!.AllowedQuestionFamilies)
        foreach(var seed in new[]{20261022,20261023})
        {
            var questions=engine.Generate("g2-grade-original-ordinal-62",family,[family],
                ExactSkillQuestionDifficulty.Standard,4,seed,[]);
            Assert.Equal(4,questions.Count);
            foreach(var q in questions)
            {
                Assert.Equal(10,q.Parameters["lesson"]);
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"-1"));
                var invalid=q.Parameters.ToDictionary(x=>x.Key,x=>x.Value,StringComparer.Ordinal);
                invalid["tens"]=7;
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,invalid,q.CorrectAnswer));
                invalid["tens"]=q.Parameters["tens"];
                invalid["lesson"]=16;
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,invalid,q.CorrectAnswer));
            }
        }
    }
}

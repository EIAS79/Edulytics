using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Four exact mathematical families, randomized tens guards, D unrun.</summary>
public sealed class UsCcssG2Unit2Lesson1NoRegroupQuestionsTests
{
    [Fact]
    public void SourceOnlyLessonRoutesFourExactIntegerFamilies()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G2:U02:L01",out var contract));
        Assert.NotNull(contract);
        Assert.Equal(4,contract!.AllowedQuestionFamilies.Count);
        foreach(var family in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
            Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,ui!.InteractionKind);
        }
    }
    [Fact]
    public void NoGeneratedVariantBorrowsOrCarriesATen()
    {
        var engine=new ExactSkillContractQuestionEngine();
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G2:U02:L01",out var contract));
        foreach(var family in contract!.AllowedQuestionFamilies)
        foreach(var seed in new[]{20261022,20261023,20261024})
        {
            var questions=engine.Generate("im2021-g2-u02-l01",family,[family],
                ExactSkillQuestionDifficulty.Standard,4,seed,[]);
            Assert.Equal(4,questions.Count);
            foreach(var q in questions)
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(
                    family,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family,q.Parameters,"-1"));
                var wrong=new Dictionary<string,int>(q.Parameters,StringComparer.Ordinal);
                if(family.Contains("movie_snacks",StringComparison.Ordinal))
                {
                    var a=q.Parameters["nachos"];var p=q.Parameters["popcorn"];
                    var t=q.Parameters["pretzels"];
                    Assert.True((p%10)+(t%10)<10);
                    Assert.True((a%10)>=(p%10));
                    wrong["popcorn"]=29;
                }
                else
                {
                    var big=q.Parameters["larger"];var small=q.Parameters["smaller"];
                    Assert.True(big%10+small%10<10);
                    Assert.True(big%10>=small%10);
                    wrong["larger"]=59;
                }
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family,wrong,q.CorrectAnswer));
            }
        }
    }
}

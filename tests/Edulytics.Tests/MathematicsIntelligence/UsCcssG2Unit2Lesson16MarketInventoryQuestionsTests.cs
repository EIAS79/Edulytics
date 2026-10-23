using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG2Unit2Lesson16MarketInventoryQuestionsTests
{
    [Fact]
    public void FourReviewedMarketFamiliesAreAllNumericAndRoutable()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G2:U02:L16",out var contract));
        Assert.NotNull(contract);
        Assert.Equal(4,contract!.AllowedQuestionFamilies.Count);
        foreach(var f in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var kind));
            Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,kind!.InteractionKind);
        }
    }
    [Fact]
    public void RandomInventoryNeverOversellsAndAllArithmeticRecomputes()
    {
        var generator=new ExactSkillContractQuestionEngine();
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G2:U02:L16",out var contract));
        foreach(var f in contract!.AllowedQuestionFamilies)
        foreach(var seed in new[]{20261022,20261023})
        {
            var q=generator.Generate("im2021-g2-u02-l16",f,[f],
                ExactSkillQuestionDifficulty.Standard,4,seed,[]);
            Assert.Equal(4,q.Count);
            foreach(var item in q)
            {
                var p=item.Parameters;
                Assert.Equal(100,p["small"]+p["second"]+p["third"]);
                Assert.InRange(p["small"],0,9);
                Assert.True(p["second"]>10&&p["third"]>10);
                Assert.InRange(p["saleSmall"],0,p["small"]);
                Assert.InRange(p["saleSecond"],0,p["second"]);
                Assert.InRange(p["saleThird"],0,p["third"]);
                Assert.True(ExactSkillContractQuestionEngine.Verify(f,p,item.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,p,"-1"));
                var altered=new Dictionary<string,int>(p,StringComparer.Ordinal); altered["third"]=1000;
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,altered,item.CorrectAnswer));
            }
        }
    }
}

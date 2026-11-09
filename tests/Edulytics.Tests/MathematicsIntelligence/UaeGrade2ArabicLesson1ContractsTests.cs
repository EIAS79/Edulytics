using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Written in Stage A-B-C; intentionally NOT_RUN until Stage D.</summary>
public sealed class UaeGrade2ArabicLesson1ContractsTests
{
    [Theory]
    [InlineData("uae.g2.u01.l01.commutative_add")]
    [InlineData("uae.g2.u01.l01.additive_zero")]
    public void FirstPartyFamilies_AreSupportedAndIndependentlyVerified(string family)
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        var engine = new ExactSkillContractQuestionEngine();
        for (int seed = 1; seed <= 24; seed++)
        {
            var item = Assert.Single(engine.Generate(
                "uae-g2-first-unit",family,[family],
                ExactSkillQuestionDifficulty.Standard,1,seed,[]));
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                item.Family,item.Parameters,item.CorrectAnswer));
            var wrong = (int.Parse(item.CorrectAnswer)+1).ToString();
            Assert.False(ExactSkillContractQuestionEngine.Verify(
                item.Family,item.Parameters,wrong));
        }
    }

    [Fact]
    public void FakeResult_MustBeRejected_AndArabicDigitsAccepted()
    {
        var p = new Dictionary<string,int>
        { ["a"]=4, ["b"]=3, ["mode"]=0, ["result"]=8 };
        const string family = "uae.g2.u01.l01.commutative_add";
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"8"));
        p["result"]=7;
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,"٧"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"8"));
    }
}

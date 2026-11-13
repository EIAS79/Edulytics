using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Grade5 Arabic student printed 137–142; written for StageD. NOT RUN in A-B-C.
public sealed class UaeGrade5Unit2Lesson10TwoDigitMultiplicationTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L5:GENERAL:02:10";
    [Fact]
    public void ExistingPartialProductsEngineSupportsGrade5WithoutAnswerLeakage()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var c));
        Assert.Equal("number.whole.multiply.two_digit_partial_products",c!.SkillId);
        Assert.Equal(5,c.AllowedQuestionFamilies.Count);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var family in c.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
            Assert.NotNull(ui);
            foreach(var q in engine.Generate(Lesson,family,[family],
                         ExactSkillQuestionDifficulty.Standard,2,20261113,[]))
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"-1"));
                Assert.DoesNotContain("answer",q.Parameters.Keys);
            }
        }
    }
    [Fact]
    public void TwoDigitPartialSumsAndThreeDigitChallengeComputeExactly()
    {
        var p=new Dictionary<string,int>{{"first",165},{"second",31}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u02.l10.multiply_two_digit_factor",p,"5115"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u02.l10.ones_partial",p,"165"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u02.l10.tens_partial",p,"4950"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u02.l10.estimate_product",p,"6000"));
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g5.u02.l10.multiply_two_digit_factor",p,"16531"));
        var challenge=new Dictionary<string,int>{{"first",235},{"second",124}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u02.l10.three_digit_challenge",challenge,"٢٩٬١٤٠"));
    }
}

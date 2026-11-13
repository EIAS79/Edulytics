using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Grade5 owner-selected Arabic book printed pp157–162. StageD tests NOT_RUN.
public sealed class UaeGrade5Unit3Lesson01FactFamilyTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L5:GENERAL:03:01";
    [Fact]
    public void MultiplicationDivisionInverseFamiliesUseReviewedExactPractice()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var c));
        Assert.Equal("number.whole.divide",c!.SkillId);
        Assert.Equal(4,c.AllowedQuestionFamilies.Count);
        var e=new ExactSkillContractQuestionEngine();
        foreach(var family in c.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
            Assert.NotNull(ui);
            foreach(var q in e.Generate(Lesson,family,[family],
                ExactSkillQuestionDifficulty.Standard,2,20261113,[]))
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"-1"));
                Assert.DoesNotContain("answer",q.Parameters.Keys);
            }
        }
    }
    [Fact]
    public void FactFamilyRecomputesCorrectInverseAndRejectsInvalidFactor()
    {
        var p=new Dictionary<string,int>{{"groups",6},{"each",8}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u03.l01.quotient_from_fact",p,"٨"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u03.l01.missing_multiplier",p,"8"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u03.l01.missing_divisor",p,"6"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u03.l01.related_product",p,"٤٨"));
        p["groups"]=0;
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g5.u03.l01.related_product",p,"0"));
    }
}

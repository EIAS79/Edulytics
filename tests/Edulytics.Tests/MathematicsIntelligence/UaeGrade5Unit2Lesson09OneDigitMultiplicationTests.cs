using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Grade 5 source pp131-136; written in A-B-C for Stage D, NOT RUN.
public sealed class UaeGrade5Unit2Lesson09OneDigitMultiplicationTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L5:GENERAL:02:09";
    [Fact]
    public void ExistingG4ExactMultiplicationEngineAcceptsGrade5PracticeFamilies()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var c));
        Assert.Equal("number.multiply_multidigit_by_one",c!.SkillId);
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
                Assert.DoesNotContain("result",q.Parameters.Keys);
            }
        }
    }
    [Fact]
    public void RegroupAndPartialPlaceValuesHaveExactAnswers()
    {
        var p=new Dictionary<string,int>{{"factor",7},{"number",246}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u02.l09.exact_one_digit_product",p,"١٬٧٢٢"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u02.l09.ones_partial",p,"42"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u02.l09.tens_partial",p,"280"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u02.l09.round_and_estimate",p,"1400"));
        p["factor"]=0;
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g5.u02.l09.exact_one_digit_product",p,"0"));
    }
}

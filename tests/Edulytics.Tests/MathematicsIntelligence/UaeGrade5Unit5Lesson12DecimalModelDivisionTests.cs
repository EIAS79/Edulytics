using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

// G5 Arabic Student printed pp373–378; Stage D tests authored but NOT_RUN in A–B–C.
public sealed class UaeGrade5Unit5Lesson12DecimalModelDivisionTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L5:GENERAL:05:12";
    [Fact] public void LocalModelSkillsRouteToSharedExactEngine()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal("number.division_model_word_problem",contract!.SkillId);
        Assert.Equal(2,contract.AllowedQuestionFamilies.Count);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var f in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
            Assert.NotNull(ui);
            foreach(var q in engine.Generate(Lesson,f,[f],ExactSkillQuestionDifficulty.Standard,2,20261113,[]))
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,"-42"));
                Assert.DoesNotContain("answer",q.Parameters.Keys);
            }
        }
    }
    [Fact] public void GridCountAndEqualGroupsUseHundredthsConsistently()
    {
        var p=new Dictionary<string,int>{{"dividendHundredths",30},{"divisorHundredths",6}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u05.l12.model_decimal_equal_groups",p,"٥"));
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g5.u05.l12.model_decimal_equal_groups",p,"0.5"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u05.l12.model_decimal_hundredths_count",p,"٣٠"));
        p["dividendHundredths"]=240;p["divisorHundredths"]=80;
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u05.l12.model_decimal_equal_groups",p,"3"));
        p["divisorHundredths"]=0;
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g5.u05.l12.model_decimal_equal_groups",p,"3"));
    }
}

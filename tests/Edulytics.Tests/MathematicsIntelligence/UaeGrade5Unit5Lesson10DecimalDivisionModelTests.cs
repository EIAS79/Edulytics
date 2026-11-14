using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Local G5 student pp361–366. Written tests NOT_RUN.
public sealed class UaeGrade5Unit5Lesson10DecimalDivisionModelTests
{
  private const string Lesson="PED:UAE-MOE-MATH:L5:GENERAL:05:10";
  [Fact] public void AllThreeModelFamiliesUseExactRegisteredEngine()
  {
    Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
    Assert.Equal("number.division_model_word_problem",contract!.SkillId);
    Assert.Equal(3,contract.AllowedQuestionFamilies.Count);
    var engine=new ExactSkillContractQuestionEngine();
    foreach(var f in contract.AllowedQuestionFamilies)
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
        Assert.NotNull(ui);
        foreach(var q in engine.Generate(Lesson,f,[f],ExactSkillQuestionDifficulty.Standard,2,20261113,[]))
            Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
    }
  }
  [Fact] public void TenthsRebundlingAndZeroPartAreCorrect()
  {
    var p=new Dictionary<string,int>{{"dividendTenths",36},{"groups",3}};
    Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u05.l10.tenths_equal_group_quotient",p,"١٫٢"));
    Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u05.l10.tenths_per_group",p,"12"));
    p["dividendTenths"]=24;p["groups"]=4;
    Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u05.l10.tenths_equal_group_quotient",p,"0.6"));
    Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g5.u05.l10.tenths_equal_group_quotient",p,"6"));
    // Maximum generated case: eight groups of 29 tenths. Guard against
    // a mismatch between the generator and its independent verifier.
    p["dividendTenths"]=232;p["groups"]=8;
    Assert.True(ExactSkillContractQuestionEngine.Verify(
        "uae.g5.u05.l10.tenths_equal_group_quotient",p,"2.9"));
    Assert.True(ExactSkillContractQuestionEngine.Verify(
        "uae.g5.u05.l10.tenths_per_group",p,"29"));
    Assert.True(ExactSkillContractQuestionEngine.Verify(
        "uae.g5.u05.l10.whole_units_per_group",p,"2"));
    p["dividendTenths"]=240;
    Assert.False(ExactSkillContractQuestionEngine.Verify(
        "uae.g5.u05.l10.tenths_equal_group_quotient",p,"3"));
    p["groups"]=0;
    Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g5.u05.l10.tenths_equal_group_quotient",p,"0.6"));
  }
}
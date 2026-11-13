using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// UAE G5 local Arabic student printed pp327–332; Stage D NOT_RUN.
public sealed class UaeGrade5Unit5Lesson05DecimalMultiplyTests
{
  private const string Lesson="PED:UAE-MOE-MATH:L5:GENERAL:05:05";
  [Fact] public void AllThreeDecimalMultiplyFamiliesRouteToSharedEngine()
  {
    Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
    Assert.Equal("supporting.decimals.core",contract!.SkillId);
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
  [Fact] public void FourDecimalPlacesAreNotRoundedAway()
  {
    const string f="uae.g5.u05.l05.multiply_decimal_factors";
    var p=new Dictionary<string,int>{{"leftHundredths",47},{"rightHundredths",18}};
    Assert.True(ExactSkillContractQuestionEngine.Verify(f,p,"٠٫٠٨٤٦"));
    Assert.False(ExactSkillContractQuestionEngine.Verify(f,p,"0.08"));
    p["leftHundredths"]=50;p["rightHundredths"]=5;
    Assert.True(ExactSkillContractQuestionEngine.Verify(f,p,"0.025"));
    Assert.False(ExactSkillContractQuestionEngine.Verify(f,p,"0.03"));
    p["leftHundredths"]=0;
    Assert.False(ExactSkillContractQuestionEngine.Verify(f,p,"0"));
  }
}
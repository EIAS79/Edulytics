using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// G5 Arabic local pp335–340, written NOT_RUN.
public sealed class UaeGrade5Unit5Lesson06PowersOfTenTests
{
  private const string Lesson="PED:UAE-MOE-MATH:L5:GENERAL:05:06";
  [Fact] public void ThreeRegisteredFamiliesRouteThroughSharedDecimalEngine()
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
  [Fact] public void DecimalPowersUseOneTwoThreePositions()
  {
    var p=new Dictionary<string,int>{{"thousandths",853},{"pow",3}};
    Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u05.l06.decimal_times_power_ten",p,"853"));
    Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u05.l06.decimal_shift_positions",p,"٣"));
    Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g5.u05.l06.decimal_times_power_ten",p,"0.853000"));
    p["thousandths"]=24700;p["pow"]=2;
    Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u05.l06.decimal_times_power_ten",p,"2470"));
    p["pow"]=4;
    Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g5.u05.l06.decimal_times_power_ten",p,"247000"));
  }
}
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Local Arabic student printed pp321–326, Stage D NOT_RUN.
public sealed class UaeGrade5Unit5Lesson04AreaModelTests
{
  private const string Lesson="PED:UAE-MOE-MATH:L5:GENERAL:05:04";
  [Fact] public void AllSourceAreaFamiliesAreRouted()
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
  [Fact] public void AreaTenthsAndThousandthsStayExact()
  {
    var p=new Dictionary<string,int>{{"aHundredths",30},{"bHundredths",70}};
    Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u05.l04.area_tenths_product",p,"٠٫٢١"));
    Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u05.l04.area_shaded_hundredths",p,"21"));
    p["aHundredths"]=11;p["bHundredths"]=50;
    Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u05.l04.area_extended_product",p,"٠٫٠٥٥"));
    Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g5.u05.l04.area_extended_product",p,"0.06"));
    p["aHundredths"]=241;
    Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g5.u05.l04.area_extended_product",p,"1.205"));
  }
}
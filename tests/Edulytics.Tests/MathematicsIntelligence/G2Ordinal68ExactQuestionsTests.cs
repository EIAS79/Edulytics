using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal68ExactQuestionsTests
{
 [Fact]
 public void FourSourceSpecificMathFamiliesRegisteredAsExactNumeric()
 {
  Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:US-CCSS-MATH:G2:U05:L01",out var contract));
  Assert.NotNull(contract);Assert.Equal(4,contract!.AllowedQuestionFamilies.Count);
  foreach(var f in contract.AllowedQuestionFamilies)
  {
   Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
   Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
   Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,ui!.InteractionKind);
  }
 }
 [Fact]
 public void TwoIndependentSeedsRoundTripAndRejectTampering()
 {
  Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:US-CCSS-MATH:G2:U05:L01",out var contract));
  var engine=new ExactSkillContractQuestionEngine();
  foreach(var f in contract!.AllowedQuestionFamilies)
  foreach(var seed in new[]{20261022,20261023})
  {
   var generated=engine.Generate("im2021-g2-u05-l01",f,[f],
       ExactSkillQuestionDifficulty.Standard,4,seed,[]);
   Assert.Equal(4,generated.Count);
   foreach(var q in generated)
   {
    Assert.Equal(1,q.Parameters["lesson"]);
    Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
    Assert.False(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,"-1"));
    var modified=q.Parameters.ToDictionary(x=>x.Key,x=>x.Value,StringComparer.Ordinal);
    modified["lesson"]=99;
    Assert.False(ExactSkillContractQuestionEngine.Verify(f,modified,q.CorrectAnswer));
    modified["lesson"]=1;modified["h"]=8;
    Assert.False(ExactSkillContractQuestionEngine.Verify(f,modified,q.CorrectAnswer));
   }
  }
 }
}

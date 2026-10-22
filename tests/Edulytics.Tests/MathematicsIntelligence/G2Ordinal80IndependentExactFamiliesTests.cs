using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal80IndependentExactFamiliesTests
{
 [Fact] public void FourOriginalSkillFamiliesUsePrivateExactNumericKeypad()
 {
  Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:US-CCSS-MATH:G2:U05:L13",out var contract));
  Assert.NotNull(contract);Assert.Equal(4,contract!.AllowedQuestionFamilies.Count);
  foreach(var f in contract.AllowedQuestionFamilies)
  {
   Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
   Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
   Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,ui!.InteractionKind);
  }
 }
 [Fact] public void TwoSeedsAndFourVariantsRequireExactRecomputationAndUntamperedParameters()
 {
  Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:US-CCSS-MATH:G2:U05:L13",out var contract));
  var engine=new ExactSkillContractQuestionEngine();
  foreach(var f in contract!.AllowedQuestionFamilies)
  foreach(var seed in new[]{20261022,20261023})
  {
   var questions=engine.Generate("im2021-g2-u05-l13",f,[f],
      ExactSkillQuestionDifficulty.Standard,4,seed,[]);
   Assert.Equal(4,questions.Count);
   foreach(var q in questions)
   {
    Assert.Equal(13,q.Parameters["lesson"]);
    Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
    Assert.False(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,"-1"));
    var changed=q.Parameters.ToDictionary(x=>x.Key,x=>x.Value,StringComparer.Ordinal);
    changed["lesson"]=15;
    Assert.False(ExactSkillContractQuestionEngine.Verify(f,changed,q.CorrectAnswer));
    changed["lesson"]=13;changed["step"]=9;
    Assert.False(ExactSkillContractQuestionEngine.Verify(f,changed,q.CorrectAnswer));
   }
  }
 }
}

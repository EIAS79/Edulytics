using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal85ExactPracticeFamiliesTests
{
 [Fact] public void FourOriginalLessonFamiliesAllHaveExactNumericInteraction()
 {
  Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:US-CCSS-MATH:G2:U06:L04",out var contract));
  Assert.NotNull(contract);Assert.Equal(4,contract!.AllowedQuestionFamilies.Count);
  foreach(var family in contract.AllowedQuestionFamilies)
  {
   Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
   Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
   Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,ui!.InteractionKind);
  }
 }
 [Fact] public void FirstPartySeedsRecomputeExactCorrectAnswerAndRejectChangedInput()
 {
  Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:US-CCSS-MATH:G2:U06:L04",out var contract));
  var engine=new ExactSkillContractQuestionEngine();
  foreach(var family in contract!.AllowedQuestionFamilies)
  foreach(var seed in new[]{20261022,20261023})
  {
   var tasks=engine.Generate("im2021-g2-unit6-l04",family,[family],
     ExactSkillQuestionDifficulty.Standard,4,seed,[]);
   Assert.Equal(4,tasks.Count);
   foreach(var q in tasks)
   {
    Assert.Equal(4,q.Parameters["lesson"]);
    Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
    Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"-1"));
    var changed=q.Parameters.ToDictionary(t=>t.Key,t=>t.Value,StringComparer.Ordinal);
    changed["lesson"]=99;Assert.False(ExactSkillContractQuestionEngine.Verify(family,changed,q.CorrectAnswer));
    changed["lesson"]=4;changed["a"]=10;
    Assert.False(ExactSkillContractQuestionEngine.Verify(family,changed,q.CorrectAnswer));
   }
  }
 }
}

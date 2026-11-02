using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal92ExactPracticeFamiliesTests
{
 [Fact] public void OnlyFourRegisteredUnit6LessonFamiliesAreAllowed()
 {
  Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:US-CCSS-MATH:G2:U06:L11",out var contract));
  Assert.NotNull(contract);Assert.Equal(4,contract!.AllowedQuestionFamilies.Count);
  foreach(var family in contract.AllowedQuestionFamilies)
  {
   Assert.StartsWith("usccss.g2.u06.l11.",family);
   Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
   Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
   Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,ui!.InteractionKind);
  }
 }
 [Fact] public void GeneratedQuestionsVerifyExactAnswersAndRejectMalformedParameters()
 {
  Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:US-CCSS-MATH:G2:U06:L11",out var contract));
  var engine=new ExactSkillContractQuestionEngine();
  foreach(var family in contract!.AllowedQuestionFamilies)
  foreach(var seed in new[]{20261022,20261023})
  {
   var items=engine.Generate("im2021-g2-unit6-l11",family,[family],
     ExactSkillQuestionDifficulty.Standard,4,seed,[]);
   Assert.Equal(4,items.Count);
   foreach(var q in items)
   {
    Assert.Equal(11,q.Parameters["lesson"]);
    Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
    Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"-1"));
    var invalid=q.Parameters.ToDictionary(k=>k.Key,v=>v.Value,StringComparer.Ordinal);
    invalid["lesson"]=99;
    Assert.False(ExactSkillContractQuestionEngine.Verify(family,invalid,q.CorrectAnswer));
    invalid["lesson"]=11;invalid["a"]=10;
    Assert.False(ExactSkillContractQuestionEngine.Verify(family,invalid,q.CorrectAnswer));
   }
  }
 }
}

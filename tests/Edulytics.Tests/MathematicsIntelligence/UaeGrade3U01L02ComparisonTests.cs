using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Written Stage C — NOT RUN.
public sealed class UaeGrade3U01L02ComparisonTests
{
 [Fact] public void ThreeComparisonFamiliesMapToAppropriateRealInteractions()
 {
  Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:UAE-MOE-MATH:L3:COMMON:01:02",out var contract));
  Assert.Equal(3,contract!.AllowedQuestionFamilies.Count);
  foreach(var f in contract.AllowedQuestionFamilies)
  {
   Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
   Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var interaction));
   Assert.Equal(f.EndsWith("relation")?LessonPracticeInteractionKind.RelationChoice:
       LessonPracticeInteractionKind.IntegerKeypad,interaction!.InteractionKind);
  }
 }
 [Fact] public void ComparisonVerificationAcceptsTrueAndRejectsFalseAndTampering()
 {
  var engine=new ExactSkillContractQuestionEngine();
  Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:UAE-MOE-MATH:L3:COMMON:01:02",out var contract));
  foreach(string family in contract!.AllowedQuestionFamilies)
  foreach(int seed in new[]{20261109,20261110})
  foreach(var item in engine.Generate("g3-u01-l02",family,[family],ExactSkillQuestionDifficulty.Standard,4,seed,[]))
  {
   Assert.True(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,item.CorrectAnswer));
   var bad=item.Parameters.ToDictionary(x=>x.Key,x=>x.Value);
   bad["result"]+=1;
   Assert.False(ExactSkillContractQuestionEngine.Verify(family,bad,item.CorrectAnswer));
   Assert.False(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,"INVALID"));
  }
 }
}

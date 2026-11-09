using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Written only: not executed in Stage A–B–C.
public sealed class UaeGrade3Unit1Lesson03OrderingTests
{
 [Fact] public void OrderingThreeFamiliesHaveCorrectLearnerInteraction()
 {
  Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:UAE-MOE-MATH:L3:COMMON:01:03",out var c));
  Assert.Equal(3,c!.AllowedQuestionFamilies.Count);
  foreach(var f in c.AllowedQuestionFamilies)
  {
   Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
   Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
   Assert.Equal(f.EndsWith("number")?LessonPracticeInteractionKind.IntegerKeypad:LessonPracticeInteractionKind.ContractDriven,ui!.InteractionKind);
  }
 }
 [Fact] public void EngineVerifiesOrderingAndRejectsAlteredParameters()
 {
  var e=new ExactSkillContractQuestionEngine();
  Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:UAE-MOE-MATH:L3:COMMON:01:03",out var c));
  foreach(var f in c!.AllowedQuestionFamilies)
  foreach(var q in e.Generate("uae-g3-u01-l03",f,[f],ExactSkillQuestionDifficulty.Standard,4,20261109,[]))
  {
   Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
   Assert.False(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,"x"));
   var altered=q.Parameters.ToDictionary(x=>x.Key,x=>x.Value);
   altered["a"]=10000;
   Assert.False(ExactSkillContractQuestionEngine.Verify(f,altered,q.CorrectAnswer));
  }
 }
}

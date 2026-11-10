using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Written at A–B–C, deliberately NOT_RUN pending Stage D.
public sealed class UaeGrade3Unit5Lesson02EqualPartsTests
{
 [Fact] public void EqualPartsReuseSingleProvenQuestionFamily()
 {
  Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:UAE-MOE-MATH:L3:COMMON:05:02",out var contract));
  var f=Assert.Single(contract!.AllowedQuestionFamilies);
  Assert.Equal("uae.g3.u05.l01.model_equal_groups_division",f);
  Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
  Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
  Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,ui!.InteractionKind);
 }
 [Fact] public void DistributionModelProducesExactIntAndRejectsTamperedTotals()
 {
  var engine=new ExactSkillContractQuestionEngine();
  const string f="uae.g3.u05.l01.model_equal_groups_division";
  foreach(var q in engine.Generate("uae-g3-u05-l02",f,[f],ExactSkillQuestionDifficulty.Standard,8,20261109,[]))
  {
   Assert.Equal(q.Parameters["groups"]*q.Parameters["each"],q.Parameters["total"]);
   Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
   var broken=q.Parameters.ToDictionary(x=>x.Key,x=>x.Value);
   broken["groups"]=0;
   Assert.False(ExactSkillContractQuestionEngine.Verify(f,broken,q.CorrectAnswer));
  }
 }
}

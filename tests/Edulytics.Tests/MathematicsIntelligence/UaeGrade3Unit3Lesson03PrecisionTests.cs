using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Authored Stage C, NOT_RUN.
public sealed class UaeGrade3Unit3Lesson03PrecisionTests
{
 [Fact] public void ClassifiedPrecisionIsContractDrivenAndExistingFamiliesReused()
 {
  Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:UAE-MOE-MATH:L3:COMMON:03:03",out var c));
  Assert.Equal(3,c!.AllowedQuestionFamilies.Count);
  foreach(var f in c.AllowedQuestionFamilies)
  {
   Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
   Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var interaction));
  }
  Assert.True(LessonPracticeInteractionRegistry.TryResolve("uae.g3.u03.l03.choose_estimate_or_exact",out var ui));
  Assert.Equal(LessonPracticeInteractionKind.ContractDriven,ui!.InteractionKind);
 }
 [Fact] public void ClassificationAnswerAndParameterIntegrity()
 {
  var e=new ExactSkillContractQuestionEngine();
  const string f="uae.g3.u03.l03.choose_estimate_or_exact";
  foreach(var q in e.Generate("g3-u03-l03",f,[f],ExactSkillQuestionDifficulty.Standard,8,20261109,[]))
  {
   Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
   var altered=q.Parameters.ToDictionary(x=>x.Key,x=>x.Value);
   altered["result"]=1-altered["result"];
   Assert.False(ExactSkillContractQuestionEngine.Verify(f,altered,q.CorrectAnswer));
  }
 }
}

using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Test source written only, NOT_RUN until Stage D.
public sealed class UaeG3U06L02MultiplyTwoTests
{
 [Fact] public void MultiplyingTwoUsesExistingEngineAndKeypad()
 {
   Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:UAE-MOE-MATH:L3:COMMON:06:02",out var contract));
   const string f="uae.g3.u06.l02.multiply_by_two";
   Assert.Contains(f,contract!.AllowedQuestionFamilies);
   Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
   Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
   Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,ui!.InteractionKind);
 }
 [Fact] public void SourceCasesRecomputeAndTamperedAnswersFail()
 {
   var engine=new ExactSkillContractQuestionEngine();
   const string f="uae.g3.u06.l02.multiply_by_two";
   foreach(var q in engine.Generate("g3-u06-l02",f,[f],ExactSkillQuestionDifficulty.Standard,10,20261109,[]))
   {
     Assert.Equal(q.Parameters["b"]*2,int.Parse(q.CorrectAnswer));
     Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
     var t=q.Parameters.ToDictionary(x=>x.Key,x=>x.Value);t["result"]++;
     Assert.False(ExactSkillContractQuestionEngine.Verify(f,t,q.CorrectAnswer));
   }
 }
}

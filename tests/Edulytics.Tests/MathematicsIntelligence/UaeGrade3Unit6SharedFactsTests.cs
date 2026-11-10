using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// New source test; intentionally NOT_RUN during A–B–C.
public sealed class UaeGrade3Unit6SharedFactsTests
{
 [Fact] public void MultiplicationTableContractIsVisibleInExistingEngine()
 {
   Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:UAE-MOE-MATH:L3:COMMON:06:01",out var c));
   Assert.Single(c!.AllowedQuestionFamilies);
   const string family="uae.g3.u06.l01.multiplication_table_pattern";
   Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
   Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
   Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,ui!.InteractionKind);
 }
 [Fact] public void NumericalFamiliesRecomputeAndRejectCorruptedParameters()
 {
   var engine=new ExactSkillContractQuestionEngine();
   const string family="uae.g3.u06.l01.multiplication_table_pattern";
   foreach(var q in engine.Generate("g3-u06-l01",family,[family],ExactSkillQuestionDifficulty.Standard,8,20261109,[]))
   {
     Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
     var p=q.Parameters.ToDictionary(k=>k.Key,v=>v.Value);
     p["result"]++;
     Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,q.CorrectAnswer));
   }
 }
}

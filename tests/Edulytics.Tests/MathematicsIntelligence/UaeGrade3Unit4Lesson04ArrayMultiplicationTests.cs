using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Tests authored for Stage D, intentionally NOT_RUN in A-B-C.
public sealed class UaeGrade3Unit4Lesson04ArrayMultiplicationTests
{
 [Fact] public void ExistingArrayFamilyAndOneMissingFactorExtensionAreAvailable()
 {
  Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:UAE-MOE-MATH:L3:COMMON:04:04",out var contract));
  Assert.Equal(2,contract!.AllowedQuestionFamilies.Count);
  Assert.Contains("uae.g2.u02.l05.repeated_add_arrays",contract.AllowedQuestionFamilies);
  Assert.Contains("uae.g3.u04.l04.find_missing_array_factor",contract.AllowedQuestionFamilies);
  foreach(var family in contract.AllowedQuestionFamilies)
  {
   Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
   Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
   Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,ui!.InteractionKind);
  }
 }
 [Fact] public void MissingFactorAnswersAreVerifiedAndRecomputeOnParameterTampering()
 {
  var engine=new ExactSkillContractQuestionEngine();
  const string f="uae.g3.u04.l04.find_missing_array_factor";
  foreach(var q in engine.Generate("g3-u04-l04",f,[f],ExactSkillQuestionDifficulty.Standard,12,20261109,[]))
  {
   Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
   Assert.InRange(q.Parameters["rows"],2,10);
   Assert.InRange(q.Parameters["columns"],2,10);
   var altered=q.Parameters.ToDictionary(x=>x.Key,x=>x.Value);altered["result"]+=1;
   Assert.False(ExactSkillContractQuestionEngine.Verify(f,altered,q.CorrectAnswer));
   Assert.False(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,"not a number"));
  }
 }
}

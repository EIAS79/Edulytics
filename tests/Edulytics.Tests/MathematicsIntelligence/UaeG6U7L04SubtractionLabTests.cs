using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG6U7L04SubtractionLabTests
{
 const string L="PED:UAE-MOE-MATH:L6:GENERAL:07:04:STUDENT-AR-128";
 [Fact] public void ArabicLabAndReuse()
 {
  var rich=RichLessonContentV2Registry.Find(L,"ar");Assert.NotNull(rich);
  Assert.Equal(10,rich!.WorkedExamples.Count);
  Assert.All(rich.WorkedExamples,e=>Assert.True(e.Steps.Count>=3));
  Assert.True(LessonPracticeCapabilityResolver.TryResolve(L,out var c));
  Assert.Equal(4,c!.AllowedQuestionFamilies.Count);
  Assert.Contains("uae.g6.u07.l01.subtract",c.AllowedQuestionFamilies);
  foreach(var f in c.AllowedQuestionFamilies)Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
 }
 [Fact] public void WritingAndInverseKeepSubtractionDirection()
 {
  var p=new Dictionary<string,int>{{"removed",5},{"remaining",41}};
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u07.l04.write_subtraction_model",p,"س−٥=٤١"));
  Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g6.u07.l04.write_subtraction_model",p,"5-س=41"));
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u07.l04.inverse_addition_statement",p,"س=٤١+٥"));
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u07.l04.inverse_addition_statement",p,"س=46"));
  p["remaining"]=0;
  Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g6.u07.l04.write_subtraction_model",p,"س−٥=٠"));
 }
}

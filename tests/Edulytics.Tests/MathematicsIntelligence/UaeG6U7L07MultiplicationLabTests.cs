using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG6U7L07MultiplicationLabTests
{
 const string L="PED:UAE-MOE-MATH:L6:GENERAL:07:07:STUDENT-AR-128";
 [Fact] public void ArabicRichAndReusedExactPractice()
 {
  var rich=RichLessonContentV2Registry.Find(L,"ar");
  Assert.NotNull(rich);Assert.Equal(10,rich!.WorkedExamples.Count);
  Assert.All(rich.WorkedExamples,e=>Assert.True(e.Steps.Count>=3));
  Assert.True(LessonPracticeCapabilityResolver.TryResolve(L,out var c));
  Assert.Equal(5,c!.AllowedQuestionFamilies.Count);
  Assert.Contains("uae.g6.u07.l01.multiply",c.AllowedQuestionFamilies);
  foreach(var fam in c.AllowedQuestionFamilies)Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(fam));
 }
 [Fact] public void TapeEquationAndInverseDivisionAreBounded()
 {
  var p=new Dictionary<string,int>{{"groups",5},{"total",10}};
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u07.l07.write_equal_groups",p,"٥س=١٠"));
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u07.l07.write_equal_groups",p,"5×س=10"));
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u07.l07.inverse_division_statement",p,"س=10/5"));
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u07.l07.inverse_division_statement",p,"س=2"));
  Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g6.u07.l07.write_equal_groups",p,"س+5=10"));
  p["groups"]=0;
  Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g6.u07.l07.write_equal_groups",p,"٠س=١٠"));
 }
}

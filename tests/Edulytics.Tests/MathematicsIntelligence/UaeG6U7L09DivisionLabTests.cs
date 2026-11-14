using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG6U7L09DivisionLabTests
{
 const string L="PED:UAE-MOE-MATH:L6:GENERAL:07:09:STUDENT-AR-128";
 [Fact] public void ArabicLabAndFourExactFamilies()
 {
  var rich=RichLessonContentV2Registry.Find(L,"ar");
  Assert.NotNull(rich);Assert.Equal(10,rich!.WorkedExamples.Count);
  Assert.All(rich.WorkedExamples,e=>Assert.True(e.Steps.Count>=3));
  Assert.True(LessonPracticeCapabilityResolver.TryResolve(L,out var c));
  Assert.Equal(4,c!.AllowedQuestionFamilies.Count);
  foreach(var f in c.AllowedQuestionFamilies)Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
 }
 [Fact] public void DivisionEquationAndInverseAreExact()
 {
  var p=new Dictionary<string,int>{{"groups",4},{"share",35}};
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u07.l09.write_quotient_equation",p,"س÷٤=٣٥"));
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u07.l09.write_quotient_equation",p,"س/4=35"));
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u07.l09.inverse_multiplication",p,"س=140"));
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u07.l09.inverse_multiplication",p,"س=٤×٣٥"));
  Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g6.u07.l09.write_quotient_equation",p,"س/35=4"));
  p["groups"]=0;
  Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g6.u07.l09.write_quotient_equation",p,"س/0=35"));
 }
}

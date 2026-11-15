using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG6U7L02AdditionLabTests
{
 const string L="PED:UAE-MOE-MATH:L6:GENERAL:07:02:STUDENT-AR-128";
 [Fact] public void ArabicLabAndReusedAddition()
 {
  var rich=RichLessonContentV2Registry.Find(L,"ar");
  Assert.NotNull(rich);Assert.Equal(10,rich!.WorkedExamples.Count);
  Assert.All(rich.WorkedExamples,e=>Assert.True(e.Steps.Count>=3));
  Assert.True(LessonPracticeCapabilityResolver.TryResolve(L,out var c));
  Assert.Equal(4,c!.AllowedQuestionFamilies.Count);
  Assert.Contains("uae.g6.u07.l01.add",c.AllowedQuestionFamilies);
  foreach(var f in c.AllowedQuestionFamilies)
    Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
 }
 [Fact] public void TapeWritingAcceptsEquivalentOrderOnly()
 {
  var p=new Dictionary<string,int>{{"known",3},{"total",7}};
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u07.l02.write_tape_equation",p,"س+٣=٧"));
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u07.l02.write_tape_equation",p,"3+س=7"));
  Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g6.u07.l02.write_tape_equation",p,"س+7=3"));
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u07.l02.subtraction_form",p,"4"));
 }
 [Fact] public void DecimalMoneyIsExactAndInputValidated()
 {
  var p=new Dictionary<string,int>{{"totalCents",13500},{"spentCents",2595}};
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u07.l02.solve_decimal_money",p,"109.05"));
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u07.l02.solve_decimal_money",p,"١٠٩٫٠٥"));
  Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g6.u07.l02.solve_decimal_money",p,"110"));
  p["spentCents"]=13500;
  Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g6.u07.l02.solve_decimal_money",p,"0"));
 }
}

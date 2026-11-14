using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG6U8L01FunctionTablesTests
{
 const string L="PED:UAE-MOE-MATH:L6:GENERAL:08:01:STUDENT-AR-128";
 [Fact] public void RichArabicFunctionTablesAndFiveFamilies()
 {
  var rich=RichLessonContentV2Registry.Find(L,"ar");
  Assert.NotNull(rich);Assert.Equal(10,rich!.WorkedExamples.Count);
  Assert.All(rich.WorkedExamples,e=>Assert.True(e.Steps.Count>=3));
  Assert.True(LessonPracticeCapabilityResolver.TryResolve(L,out var c));
  Assert.Equal(5,c!.AllowedQuestionFamilies.Count);
  foreach(var f in c.AllowedQuestionFamilies)Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
 }
 [Fact] public void LinearOutputAndInputExact()
 {
  var p=new Dictionary<string,int>{{"a",52},{"b",0},{"x",6}};
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u08.l01.linear_output",p,"٣١٢"));
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u08.l01.linear_input",p,"6"));
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u08.l01.ordered_pair",p,"(٦،٣١٢)"));
  Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g6.u08.l01.ordered_pair",p,"(312,6)"));
 }
 [Fact] public void IndependentDependentAndNonzeroDivision()
 {
  var independent=new Dictionary<string,int>{{"mode",0}};
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u08.l01.classify_variable",independent,"مستقل"));
  independent["mode"]=1;
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u08.l01.classify_variable",independent,"تابع"));
  var p=new Dictionary<string,int>{{"total",30},{"x",6}};
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u08.l01.division_output",p,"٥"));
  p["x"]=0;
  Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g6.u08.l01.division_output",p,"0"));
 }
}

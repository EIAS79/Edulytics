using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG6U7L03AdditionEquationTests
{
 const string L="PED:UAE-MOE-MATH:L6:GENERAL:07:03:STUDENT-AR-128";
 [Fact] public void ArabicRichAndSevenFamilies()
 {
  var rich=RichLessonContentV2Registry.Find(L,"ar");
  Assert.NotNull(rich);Assert.Equal(10,rich!.WorkedExamples.Count);
  Assert.All(rich.WorkedExamples,e=>Assert.True(e.Steps.Count>=3));
  Assert.True(LessonPracticeCapabilityResolver.TryResolve(L,out var c));
  Assert.Equal(7,c!.AllowedQuestionFamilies.Count);
  foreach(var f in c.AllowedQuestionFamilies)Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
 }
 [Fact] public void FractionAdditionAndEquivalentFractionForms()
 {
  var p=new Dictionary<string,int>{{"known",1},{"total",5},{"denominator",10}};
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u07.l03.fraction_addition_unknown",p,"٢/٥"));
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u07.l03.fraction_addition_unknown",p,"4/10"));
  Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g6.u07.l03.fraction_addition_unknown",p,"1/5"));
  p["denominator"]=0;
  Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g6.u07.l03.fraction_addition_unknown",p,"0"));
 }
 [Fact] public void ExactDecimalAndSubtractionProperty()
 {
  var p=new Dictionary<string,int>{{"totalCents",24550},{"partCents",13200}};
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u07.l03.decimal_addition_unknown",p,"113.50"));
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u07.l03.decimal_addition_unknown",p,"١١٣٫٥"));
  Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g6.u07.l03.decimal_addition_unknown",p,"114"));
  var q=new Dictionary<string,int>{{"mode",0}};
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u07.l03.identify_subtraction_property",q,"نعم"));
  q["mode"]=1;
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u07.l03.identify_subtraction_property",q,"لا"));
 }
}

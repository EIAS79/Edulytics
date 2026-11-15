using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG6U7L10DivisionEquationsTests
{
 const string L="PED:UAE-MOE-MATH:L6:GENERAL:07:10:STUDENT-AR-128";
 [Fact] public void ArabicLessonUnitSevenHasSevenFamilies()
 {
  var rich=RichLessonContentV2Registry.Find(L,"ar");
  Assert.NotNull(rich);Assert.Equal(10,rich!.WorkedExamples.Count);
  Assert.All(rich.WorkedExamples,e=>Assert.True(e.Steps.Count>=3));
  Assert.True(LessonPracticeCapabilityResolver.TryResolve(L,out var c));
  Assert.Equal(7,c!.AllowedQuestionFamilies.Count);
  foreach(var f in c.AllowedQuestionFamilies)Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
 }
 [Fact] public void ExactDecimalAndFractionalDivision()
 {
  var d=new Dictionary<string,int>{{"divisor100",850},{"quotient",16}};
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u07.l10.decimal_divisor",d,"١٣٦"));
  Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g6.u07.l10.decimal_divisor",d,"135"));
  var p=new Dictionary<string,int>{{"num",2},{"den",5},{"q",3}};
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u07.l10.rational_divisor",p,"6/5"));
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u07.l10.rational_divisor",p,"12/10"));
  p["den"]=0;
  Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g6.u07.l10.rational_divisor",p,"0"));
 }
 [Fact] public void UnknownDenominatorAndMultiplicationProperty()
 {
  var p=new Dictionary<string,int>{{"numerator",16},{"quotient",8}};
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u07.l10.variable_denominator",p,"٢"));
  p["quotient"]=0;
  Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g6.u07.l10.variable_denominator",p,"0"));
  var prop=new Dictionary<string,int>{{"mode",0}};
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u07.l10.multiplication_property",prop,"نعم"));
  prop["mode"]=1;
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u07.l10.multiplication_property",prop,"لا"));
 }
}

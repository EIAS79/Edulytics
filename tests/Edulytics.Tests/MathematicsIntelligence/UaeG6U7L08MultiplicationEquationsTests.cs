using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG6U7L08MultiplicationEquationsTests
{
 const string L="PED:UAE-MOE-MATH:L6:GENERAL:07:08:STUDENT-AR-128";
 [Fact] public void ArabicContentAndSixContractFamilies()
 {
  var rich=RichLessonContentV2Registry.Find(L,"ar");
  Assert.NotNull(rich);Assert.Equal(10,rich!.WorkedExamples.Count);
  Assert.All(rich.WorkedExamples,e=>Assert.True(e.Steps.Count>=3));
  Assert.True(LessonPracticeCapabilityResolver.TryResolve(L,out var c));
  Assert.Equal(6,c!.AllowedQuestionFamilies.Count);
  foreach(var f in c.AllowedQuestionFamilies)Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
 }
 [Fact] public void DecimalCoefficientExactAndSafe()
 {
  var p=new Dictionary<string,int>{{"coef100",328},{"value",6}};
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u07.l08.decimal_coefficient",p,"٦"));
  Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g6.u07.l08.decimal_coefficient",p,"5"));
  p["coef100"]=0;
  Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g6.u07.l08.decimal_coefficient",p,"6"));
 }
 [Fact] public void FractionCoefficientAndNonzeroDivisionProperty()
 {
  var p=new Dictionary<string,int>{{"numerator",2},{"denominator",5},{"value",3}};
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u07.l08.fraction_coefficient",p,"3"));
  p["denominator"]=0;
  Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g6.u07.l08.fraction_coefficient",p,"3"));
  var prop=new Dictionary<string,int>{{"mode",0}};
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u07.l08.division_property",prop,"نعم"));
  prop["mode"]=1;
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u07.l08.division_property",prop,"لا"));
 }
}

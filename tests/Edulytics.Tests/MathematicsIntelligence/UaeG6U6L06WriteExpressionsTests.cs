using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG6U6L06WriteExpressionsTests
{
 const string L="PED:UAE-MOE-MATH:L6:GENERAL:06:06:STUDENT-AR-128";
 [Fact] public void ArabicRichAndTenFamilies()
 {
  var rich=RichLessonContentV2Registry.Find(L,"ar");
  Assert.NotNull(rich); Assert.Equal(10,rich!.WorkedExamples.Count);
  Assert.All(rich.WorkedExamples,e=>Assert.True(e.Steps.Count>=3));
  Assert.True(LessonPracticeCapabilityResolver.TryResolve(L,out var c));
  Assert.Equal(10,c!.AllowedQuestionFamilies.Count);
  foreach(var family in c.AllowedQuestionFamilies)
    Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
 }
 [Fact] public void CorrectSubtractionAndGrouping()
 {
  var p=new Dictionary<string,int>{{"a",3},{"b",5}};
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l06.multiple_minus",p,"٣س−٥"));
  Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l06.multiple_minus",p,"5-3س"));
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l06.multiple_plus",p,"3×س+5"));
  var grouped=new Dictionary<string,int>{{"q",3},{"fee",15}};
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l06.grouped_ticket_cost",grouped,"٣(س+١٥)"));
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l06.grouped_ticket_cost",grouped,"3*(س+15)"));
  Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l06.grouped_ticket_cost",grouped,"3س+15"));
 }
 [Fact] public void AccurateDecimalUnitAndPercentage()
 {
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l06.inches_to_centimeters",
    new Dictionary<string,int>(),"٢٫٥٤س"));
  var p=new Dictionary<string,int>{{"pct",20}};
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l06.percentage_markup",p,"١٫٢س"));
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l06.percentage_markup",p,"س+٠٫٢س"));
  p["pct"]=30;
  Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l06.percentage_markup",p,"1.3س"));
 }
}

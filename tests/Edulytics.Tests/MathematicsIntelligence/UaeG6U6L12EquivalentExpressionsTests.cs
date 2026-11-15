using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG6U6L12EquivalentExpressionsTests
{
 const string L="PED:UAE-MOE-MATH:L6:GENERAL:06:12:STUDENT-AR-128";
 [Fact] public void SourceAlignedRichAndEightExactPracticeFamilies()
 {
  var rich=RichLessonContentV2Registry.Find(L,"ar");
  Assert.NotNull(rich);Assert.Equal(10,rich!.WorkedExamples.Count);
  Assert.All(rich.WorkedExamples,e=>Assert.True(e.Steps.Count>=3));
  Assert.True(LessonPracticeCapabilityResolver.TryResolve(L,out var c));
  Assert.Equal(8,c!.AllowedQuestionFamilies.Count);
  foreach(var f in c.AllowedQuestionFamilies)
   Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
 }
 [Fact] public void TwoVariablesRemainSeparated()
 {
  var p=new Dictionary<string,int>{{"a",2},{"b",5},{"c",7},{"d",4}};
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l12.combine_two_variables",p,"٩س+٩ص"));
  Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l12.combine_two_variables",p,"18س"));
  var q=new Dictionary<string,int>{{"k",4},{"a",2},{"b",1}};
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l12.distribute_two_variables",q,"٨س+٤ص"));
  Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l12.distribute_two_variables",q,"8س+ص"));
 }
 [Fact] public void GcfFactoringAndGroupedCostAreExact()
 {
  var p=new Dictionary<string,int>{{"ax",27},{"by",18}};
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l12.factor_two_variables",p,"٩(٣س+٢ص)"));
  p["by"]=19;
  Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l12.factor_two_variables",p,"٩(٣س+٢ص)"));
  var q=new Dictionary<string,int>{{"q",3},{"fee",10}};
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l12.grouped_tickets_cost",q,"٤س+٣٠"));
  Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l12.grouped_tickets_cost",q,"3س+30"));
 }
}

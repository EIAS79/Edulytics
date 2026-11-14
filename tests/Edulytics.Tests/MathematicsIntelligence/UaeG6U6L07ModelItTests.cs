using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG6U6L07ModelItTests
{
 const string L="PED:UAE-MOE-MATH:L6:GENERAL:06:07:STUDENT-AR-128";
 [Fact] public void ArabicInquiryHasTenExamplesAndFiveNumericFamilies()
 {
  var rich=RichLessonContentV2Registry.Find(L,"ar");
  Assert.NotNull(rich);Assert.Equal(10,rich!.WorkedExamples.Count);
  Assert.All(rich.WorkedExamples,e=>Assert.True(e.Steps.Count>=3));
  Assert.True(LessonPracticeCapabilityResolver.TryResolve(L,out var c));
  Assert.Equal(5,c!.AllowedQuestionFamilies.Count);
  foreach(var f in c.AllowedQuestionFamilies)Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
 }
 [Fact] public void TablesAndTriangularSquaresMatchModels()
 {
  var p=new Dictionary<string,int>{{"tables",4}};
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l07.table_seats",p,"١٨"));
  Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l07.table_seats",p,"24"));
  var q=new Dictionary<string,int>{{"step",5}};
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l07.staircase_squares",q,"15"));
  q["step"]=26;
  Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l07.staircase_squares",q,"351"));
 }
 [Fact] public void SavingsAreComputedInExactCents()
 {
  var p=new Dictionary<string,int>{{"initialCents",538},{"weekCents",293},{"weeks",5}};
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l07.savings_after_weeks",p,"٢٠٫٠٣"));
  Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l07.savings_after_weeks",p,"20"));
 }
 [Fact] public void EqualGroupsAndVoteFractionsCheckBoundaries()
 {
  var p=new Dictionary<string,int>{{"people",24},{"teams",4}};
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l07.equal_team_size",p,"6"));
  p["teams"]=5;Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l07.equal_team_size",p,"4.8"));
  var v=new Dictionary<string,int>{{"people",24},{"numerator",1},{"denominator",4}};
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l07.fractional_votes",v,"٦"));
  v["denominator"]=0;Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l07.fractional_votes",v,"0"));
 }
}

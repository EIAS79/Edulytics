using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG6U7L06GuessCheckTests
{
 const string L="PED:UAE-MOE-MATH:L6:GENERAL:07:06:STUDENT-AR-128";
 [Fact] public void ArabicInquiryHasFiveObjectiveFamilies()
 {
  var rich=RichLessonContentV2Registry.Find(L,"ar");
  Assert.NotNull(rich);Assert.Equal(10,rich!.WorkedExamples.Count);
  Assert.All(rich.WorkedExamples,e=>Assert.True(e.Steps.Count>=3));
  Assert.True(LessonPracticeCapabilityResolver.TryResolve(L,out var c));
  Assert.Equal(5,c!.AllowedQuestionFamilies.Count);
  foreach(var f in c.AllowedQuestionFamilies)Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
 }
 [Fact] public void BanknotesAndSavingsEnforceAllParameters()
 {
  var notes=new Dictionary<string,int>{{"n",8},{"twenties",2}};
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u07.l06.banknotes",notes,"٢"));
  Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g6.u07.l06.banknotes",notes,"3"));
  var savings=new Dictionary<string,int>{{"initial",134},{"month",18},{"months",7}};
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u07.l06.monthly_savings",savings,"7"));
  savings["months"]=25;
  Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g6.u07.l06.monthly_savings",savings,"25"));
 }
 [Fact] public void VaryingPuzzleParametersPreserveUniqueAnswersAndOrder()
 {
  var comics=new Dictionary<string,int>{{"newGroups",2},{"usedGroups",2}};
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u07.l06.comic_sets",comics,"٢،٢"));
  Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g6.u07.l06.comic_sets",comics,"٣،١"));
  comics["newGroups"]=3;
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u07.l06.comic_sets",comics,"٣،٢"));
  var quiz=new Dictionary<string,int>{{"twoPoint",3},{"fourPoint",3}};
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u07.l06.quiz_points",quiz,"٣،٣"));
  quiz["fourPoint"]=4;
  Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g6.u07.l06.quiz_points",quiz,"٣،٣"));
  var operation=new Dictionary<string,int>{{"a",3},{"b",4},{"c",6},{"d",1}};
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u07.l06.operator_check",operation,"٣×٤+٦÷١"));
  Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g6.u07.l06.operator_check",operation,"3+4*6/1"));
  operation["a"]=5;
  Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g6.u07.l06.operator_check",operation,"٣×٤+٦÷١"));
 }
}

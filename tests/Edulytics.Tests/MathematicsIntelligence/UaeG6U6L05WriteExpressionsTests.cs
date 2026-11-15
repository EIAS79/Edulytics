using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG6U6L05WriteExpressionsTests
{
 const string L="PED:UAE-MOE-MATH:L6:GENERAL:06:05:STUDENT-AR-128";
 [Fact] public void ArabicBarModelsAndScopedContract()
 {
   var rich=RichLessonContentV2Registry.Find(L,"ar");
   Assert.NotNull(rich);Assert.Equal(10,rich!.WorkedExamples.Count);
   Assert.All(rich.WorkedExamples,e=>Assert.True(e.Steps.Count>=3));
   Assert.Equal(3,rich.Visuals.Count(v=>v.Kind==RichLessonVisualKind.ExpressionStripBars));
   Assert.True(LessonPracticeCapabilityResolver.TryResolve(L,out var c));
   Assert.Equal(6,c!.AllowedQuestionFamilies.Count);
   foreach(var f in c.AllowedQuestionFamilies)
     Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
 }
 [Fact] public void MoreLessAndQuotientKeepTheirOrder()
 {
   var p=new Dictionary<string,int>{{"n",6}};
   Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l05.more_than",p,"س+٦"));
   Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l05.more_than",p,"6+س"));
   Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l05.less_than",p,"س−٦"));
   Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l05.less_than",p,"6-س"));
   var q=new Dictionary<string,int>{{"d",3}};
   Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l05.part_of",q,"س÷٣"));
   Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l05.part_of",q,"3س"));
 }
 [Fact] public void RepeatedAndCombinedBarGroupsAreExact()
 {
   var p=new Dictionary<string,int>{{"times",4}};
   Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l05.repeated",p,"٤س"));
   Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l05.repeated",p,"4×س"));
   Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l05.repeated",p,"س+٤"));
   var q=new Dictionary<string,int>{{"times",3},{"add",3}};
   Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l05.groups_plus",q,"٣س+٣"));
   Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l05.groups_plus",q,"3*س+3"));
   Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l05.groups_plus",q,"6س"));
 }
 [Fact] public void ClassificationRejectsWrongMode()
 {
   var p=new Dictionary<string,int>{{"mode",2}};
   Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l05.operation_type",p,"حاصل ضرب"));
   Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l05.operation_type",p,"مجموع"));
   p["mode"]=4;
   Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l05.operation_type",p,"حاصل ضرب"));
 }
}

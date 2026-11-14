using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG6U8L07InequalitiesTests
{
 const string L="PED:UAE-MOE-MATH:L6:GENERAL:08:07:STUDENT-AR-128",
 M="uae.g6.u08.l06.candidate_membership";
 [Fact] public void ArabicRichAndReusedPractice()
 {
  var lesson=RichLessonContentV2Registry.Find(L,"ar");
  Assert.NotNull(lesson);Assert.Equal(10,lesson!.WorkedExamples.Count);
  Assert.All(lesson.WorkedExamples,e=>Assert.True(e.Steps.Count>=3));
  Assert.True(LessonPracticeCapabilityResolver.TryResolve(L,out var c));
  Assert.Equal(3,c!.AllowedQuestionFamilies.Count);
  foreach(var f in c.AllowedQuestionFamilies)Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
 }
 [Fact] public void StrictVsInclusiveSolutionCheckAndSignedValues()
 {
  var p=new Dictionary<string,int>{{"op",0},{"bound",7},{"candidate",7}};
  Assert.True(ExactSkillContractQuestionEngine.Verify(M,p,"لا"));
  p["candidate"]=6;Assert.True(ExactSkillContractQuestionEngine.Verify(M,p,"نعم"));
  p["op"]=1;p["candidate"]=7;
  Assert.True(ExactSkillContractQuestionEngine.Verify(M,p,"نعم"));
  p["bound"]=-2;p["candidate"]=-4;p["op"]=0;
  Assert.True(ExactSkillContractQuestionEngine.Verify(M,p,"نعم"));
 }
}

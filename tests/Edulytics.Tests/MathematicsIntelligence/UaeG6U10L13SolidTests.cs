using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG6U10L13SolidTests
{
 private const string L="PED:UAE-MOE-MATH:L6:GENERAL:10:13:STUDENT-AR-128";
 [Fact] public void ArabicContentAndScopedContract()
 {
   var lesson=RichLessonContentV2Registry.Find(L,"ar");
   Assert.NotNull(lesson);
   Assert.Equal(8,lesson!.WorkedExamples.Count);
   Assert.True(lesson.Visuals.Count>=3);
   Assert.True(LessonPracticeCapabilityResolver.TryResolve(L,out var contract));
   Assert.Equal(2,contract!.AllowedQuestionFamilies.Count);
   foreach(var f in contract.AllowedQuestionFamilies) Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
 }
 [Fact] public void CheckCorrectArabicAnswerAndRejectWrongValue()
 {
   var p=new Dictionary<string,int>{{"l1",3},{"w1",2},{"h1",4},{"l2",3},{"w2",4},{"h2",2}};
   const string f="uae.g6.u10.l14.composite_volume";
   Assert.True(ExactSkillContractQuestionEngine.Verify(f,p,"٤٨"));
   Assert.False(ExactSkillContractQuestionEngine.Verify(f,p,"49"));
 }
}

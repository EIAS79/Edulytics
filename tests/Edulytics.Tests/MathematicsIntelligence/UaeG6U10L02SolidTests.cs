using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG6U10L02SolidTests
{
 private const string L="PED:UAE-MOE-MATH:L6:GENERAL:10:02:STUDENT-AR-128";
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
   var p=new Dictionary<string,int>{{"l",3},{"w",2},{"h",4}};
   const string f="uae.g6.u10.l02.rect_prism_volume";
   Assert.True(ExactSkillContractQuestionEngine.Verify(f,p,"٢٤"));
   Assert.False(ExactSkillContractQuestionEngine.Verify(f,p,"25"));
 }
}

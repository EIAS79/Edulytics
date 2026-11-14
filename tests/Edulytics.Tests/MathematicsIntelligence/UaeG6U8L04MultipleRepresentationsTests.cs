using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG6U8L04MultipleRepresentationsTests
{
 const string L="PED:UAE-MOE-MATH:L6:GENERAL:08:04:STUDENT-AR-128";
 [Fact] public void OriginalArabicMultipleRepresentationsAndReusedContract()
 {
   var lesson=RichLessonContentV2Registry.Find(L,"ar");
   Assert.NotNull(lesson);
   Assert.Equal(10,lesson!.WorkedExamples.Count);
   Assert.All(lesson.WorkedExamples,e=>Assert.True(e.Steps.Count>=3));
   Assert.Equal(2,lesson.Visuals.Count(v=>v.Kind==RichLessonVisualKind.CartesianPlot));
   Assert.True(LessonPracticeCapabilityResolver.TryResolve(L,out var c));
   Assert.Equal(5,c!.AllowedQuestionFamilies.Count);
   foreach(var family in c.AllowedQuestionFamilies)
     Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
 }
 [Fact] public void AuthoringPreservesSourceWordTableEquationAndGraphSkills()
 {
   var lesson=RichLessonContentV2Registry.Find(L,"ar")!;
   Assert.Contains(lesson.ExplanationParagraphs,s=>s.Contains("٩س"));
   Assert.Contains(lesson.ExplanationParagraphs,s=>s.Contains("٢٠+٥س"));
   Assert.Contains(lesson.Visuals,v=>v.Kind==RichLessonVisualKind.RatioTable);
   Assert.Contains(lesson.Visuals,v=>v.Kind==RichLessonVisualKind.CartesianPlot);
 }
}

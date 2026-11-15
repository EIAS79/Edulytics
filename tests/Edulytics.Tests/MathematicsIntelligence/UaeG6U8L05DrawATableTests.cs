using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG6U8L05DrawATableTests
{
 const string L="PED:UAE-MOE-MATH:L6:GENERAL:08:05:STUDENT-AR-128";
 [Fact] public void ArabicInquiryIsBoundToExistingExactPatterns()
 {
   var lesson=RichLessonContentV2Registry.Find(L,"ar");
   Assert.NotNull(lesson);Assert.Equal(10,lesson!.WorkedExamples.Count);
   Assert.All(lesson.WorkedExamples,e=>Assert.True(e.Steps.Count>=3));
   Assert.True(LessonPracticeCapabilityResolver.TryResolve(L,out var c));
   Assert.Equal(4,c!.AllowedQuestionFamilies.Count);
   foreach(var f in c.AllowedQuestionFamilies)
       Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
 }
 [Fact] public void TableVisualsRepresentGrowthComparisonAndSavings()
 {
    var lesson=RichLessonContentV2Registry.Find(L,"ar")!;
    Assert.True(lesson.Visuals.Count(v=>v.Kind==RichLessonVisualKind.RatioTable)>=3);
    Assert.Contains(lesson.ExplanationParagraphs,p=>p.Contains("٢٥")&&p.Contains("٢٠"));
 }
}

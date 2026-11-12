using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit14Lesson02MeasurementTests
{
 private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:14:02";
 [Fact] public void PracticeQuestionsAreGeneratedVerifiedAndRegistered()
 {
  Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
  Assert.Equal(2,contract!.AllowedQuestionFamilies.Count);
  var engine=new ExactSkillContractQuestionEngine();
  foreach(var family in contract.AllowedQuestionFamilies)
  {
   Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
   Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
   Assert.NotNull(ui);
   foreach(var q in engine.Generate(Lesson,family,[family],ExactSkillQuestionDifficulty.Standard,3,20261112,[]))
   {
    Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
    Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"-1"));
   }
  }
 }
 [Theory]
 [InlineData(2,3,1)]
 [InlineData(4,5,3)]
 [InlineData(8,6,2)]
 public void BothFamiliesHaveCorrectDeterministicAnswers(int a,int b,int c)
 {
  var p=new Dictionary<string,int>{{"a",a},{"b",b},{"c",c}};
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u14.l02.dot_plot_total",p,Expected1(a,b,c).ToString(System.Globalization.CultureInfo.InvariantCulture)));
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u14.l02.dot_plot_most",p,Expected2(a,b,c).ToString(System.Globalization.CultureInfo.InvariantCulture)));
 }
 private static int Expected1(int a,int b,int c)=>a+b+c;
 private static int Expected2(int a,int b,int c)=>Math.Max(a,Math.Max(b,c));
 [Fact] public void RichArabicContentHasExpectedLessons()
 {
  var lesson=RichLessonContentV2Registry.Find(Lesson,"ar");
  Assert.NotNull(lesson);
  Assert.Equal("عرض بيانات القياس في التمثيل البياني بالنقاط المجمعة",lesson!.Title);
  Assert.True(lesson.WorkedExamples.Count>=4);
  Assert.True(lesson.KeyConcepts.Count>=3);
  Assert.Equal(RichLessonVisualKind.EquationSet,Assert.Single(lesson.Visuals).Kind);
 }
}

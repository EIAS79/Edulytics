using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit15Lesson04GeometryTests
{
 private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:15:04";
 [Fact] public void GeneratedGeometryPracticeIsRegisteredAndCorrect()
 {
  Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
  Assert.Equal(2,contract!.AllowedQuestionFamilies.Count);
  var engine=new ExactSkillContractQuestionEngine();
  foreach(var f in contract.AllowedQuestionFamilies)
  {
   Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
   Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
   Assert.NotNull(ui);
   foreach(var q in engine.Generate(Lesson,f,[f],ExactSkillQuestionDifficulty.Standard,3,20261112,[]))
   {
    Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
    Assert.False(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,"-1"));
   }
  }
 }
 [Theory]
 [InlineData(40,30,20)]
 [InlineData(60,40,20)]
 [InlineData(50,25,15)]
 public void SolvesDeterministicGeometryProblems(int a,int b,int c)
 {
  var p=new Dictionary<string,int>{{"a",a},{"b",b},{"c",c}};
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u15.l04.draw_degrees",p,(a).ToString(System.Globalization.CultureInfo.InvariantCulture)));
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u15.l04.draw_supplement",p,(180-a).ToString(System.Globalization.CultureInfo.InvariantCulture)));
 }
 [Fact] public void InvalidShapeInputsAreRejected()
 {
  var p=new Dictionary<string,int>{{"a",-1},{"b",20},{"c",30}};
  Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g4.u15.l04.draw_degrees",p,"0"));
 }
 [Fact] public void AuthoredArabicContentContainsWorkedExamples()
 {
  var l=RichLessonContentV2Registry.Find(Lesson,"ar");
  Assert.NotNull(l);
  Assert.Equal("رسم الزوايا",l!.Title);
  Assert.True(l.WorkedExamples.Count>=4);
  Assert.Equal(RichLessonVisualKind.EquationSet,Assert.Single(l.Visuals).Kind);
 }
}

using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit12Lesson04GeometryOneTests
{
 private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:12:04";
 [Fact] public void GenerateAndVerifyBothLessonFamilies()
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
 [InlineData(2,3,2,4,6)]
 [InlineData(4,3,2,2,6)]
 [InlineData(5,4,2,1,6)]
 public void SourceAlignedExamplesHaveExactAnswers(int a,int b,int c,int one,int two)
 {
   var p=new Dictionary<string,int>{{"a",a},{"b",b},{"c",c}};
   Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u12.l04.net_missing_faces",p,one.ToString(System.Globalization.CultureInfo.InvariantCulture)));
   Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u12.l04.net_total_faces",p,two.ToString(System.Globalization.CultureInfo.InvariantCulture)));
 }
 [Fact] public void InvalidParametersAreRejected()
 {
   var p=new Dictionary<string,int>{{"a",-1},{"b",3},{"c",2}};
   Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g4.u12.l04.net_missing_faces",p,"0"));
 }
 [Fact] public void OriginalArabicTeachingContentIsWired()
 {
   var lesson=RichLessonContentV2Registry.Find(Lesson,"ar");
   Assert.NotNull(lesson);
   Assert.Equal("نشاط عملي: تكوين أشكال ثلاثية الأبعاد",lesson!.Title);
   Assert.True(lesson.WorkedExamples.Count>=4);
   Assert.True(lesson.KeyConcepts.Count>=3);
   Assert.Equal(RichLessonVisualKind.EquationSet,Assert.Single(lesson.Visuals).Kind);
 }
}

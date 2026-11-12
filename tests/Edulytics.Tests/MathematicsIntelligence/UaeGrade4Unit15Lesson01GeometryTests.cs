using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit15Lesson01GeometryTests
{
 private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:15:01";
 [Fact] public void PracticeFamiliesGenerateAndVerify()
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
 [InlineData(3,5)]
 [InlineData(4,4)]
 [InlineData(6,2)]
 public void DeterministicPerimeterAndAreaChecks(int a,int b)
 {
    var p=new Dictionary<string,int>{{"a",a},{"b",b},{"c",0}};
    Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u15.l01.perimeter_rectangle",p,(2*(a+b)).ToString(System.Globalization.CultureInfo.InvariantCulture)));
    Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u15.l01.perimeter_square",p,(4*a).ToString(System.Globalization.CultureInfo.InvariantCulture)));
 }
 [Fact] public void InvalidShapeIsRejected()
 {
  var p=new Dictionary<string,int>{{"a",-1},{"b",3},{"c",0}};
  Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g4.u15.l01.perimeter_rectangle",p,"0"));
 }
 [Fact] public void ArabicRichContentIncludesModels()
 {
  var l=RichLessonContentV2Registry.Find(Lesson,"ar");
  Assert.NotNull(l);
  Assert.Equal("حساب المحيط",l!.Title);
  Assert.True(l.WorkedExamples.Count>=4);
  Assert.Equal(RichLessonVisualKind.EquationSet,Assert.Single(l.Visuals).Kind);
 }
}

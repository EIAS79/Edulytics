using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG6U8L08WriteAndGraphInequalitiesTests
{
 const string L="PED:UAE-MOE-MATH:L6:GENERAL:08:08:STUDENT-AR-128";
 const string W="uae.g6.u08.l08.write_word_inequality";
 [Fact] public void RichArabicLessonHasActualInequalityLineVisuals()
 {
  var lesson=RichLessonContentV2Registry.Find(L,"ar");
  Assert.NotNull(lesson);Assert.Equal(10,lesson!.WorkedExamples.Count);
  Assert.All(lesson.WorkedExamples,e=>Assert.True(e.Steps.Count>=3));
  Assert.True(lesson.Visuals.Count(v=>v.Kind==RichLessonVisualKind.InequalityNumberLine)>=3);
  Assert.True(LessonPracticeCapabilityResolver.TryResolve(L,out var c));
  Assert.Equal(4,c!.AllowedQuestionFamilies.Count);
  foreach(var family in c.AllowedQuestionFamilies)
    Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
 }
 [Theory]
 [InlineData(0,142,"س<١٤٫٢")]
 [InlineData(1,600,"س≤٦٠")]
 [InlineData(2,120,"س>١٢")]
 [InlineData(3,180,"س≥١٨")]
 public void WordToInequalityWritesExactArabicWithTenths(int op,int bound10,string response)
 {
  var p=new Dictionary<string,int>{{"op",op},{"bound10",bound10}};
  Assert.True(ExactSkillContractQuestionEngine.Verify(W,p,response));
  Assert.False(ExactSkillContractQuestionEngine.Verify(W,p,"س=١"));
 }
 [Fact] public void InvalidBoundCannotBeGraded()
 {
  var p=new Dictionary<string,int>{{"op",3},{"bound10",201}};
  Assert.False(ExactSkillContractQuestionEngine.Verify(W,p,"س≥20.1"));
 }
}

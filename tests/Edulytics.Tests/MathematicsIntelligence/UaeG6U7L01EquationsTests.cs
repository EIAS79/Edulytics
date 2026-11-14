using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG6U7L01EquationsTests
{
 const string L="PED:UAE-MOE-MATH:L6:GENERAL:07:01:STUDENT-AR-128";
 [Fact] public void RichArabicLessonAndSixExactFamilies()
 {
  var lesson=RichLessonContentV2Registry.Find(L,"ar");
  Assert.NotNull(lesson);Assert.Equal(10,lesson!.WorkedExamples.Count);
  Assert.All(lesson.WorkedExamples,x=>Assert.True(x.Steps.Count>=3));
  Assert.True(LessonPracticeCapabilityResolver.TryResolve(L,out var c));
  Assert.Equal(6,c!.AllowedQuestionFamilies.Count);
  foreach(var family in c.AllowedQuestionFamilies)
    Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
 }
 [Theory]
 [InlineData("uae.g6.u07.l01.add")]
 [InlineData("uae.g6.u07.l01.subtract")]
 [InlineData("uae.g6.u07.l01.result_minus_unknown")]
 [InlineData("uae.g6.u07.l01.multiply")]
 [InlineData("uae.g6.u07.l01.divide")]
 public void OneStepEquationsWithInverseOperations(string family)
 {
  var p=new Dictionary<string,int>{{"x",4},{"a",7}};
  Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,"٤"));
  Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"5"));
  p["a"]=0;
  Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"4"));
 }
 [Fact] public void CandidateMustSatisfyEquation()
 {
  var p=new Dictionary<string,int>{{"x",4},{"a",7},{"option",4}};
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u07.l01.test_candidate",p,"نعم"));
  p["option"]=5;
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u07.l01.test_candidate",p,"لا"));
  p["option"]=42;
  Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g6.u07.l01.test_candidate",p,"لا"));
 }
}

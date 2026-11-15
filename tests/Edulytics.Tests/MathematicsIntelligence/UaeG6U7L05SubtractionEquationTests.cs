using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG6U7L05SubtractionEquationTests
{
 const string L="PED:UAE-MOE-MATH:L6:GENERAL:07:05:STUDENT-AR-128";
 [Fact] public void ArabicLessonAndReuse()
 {
  var rich=RichLessonContentV2Registry.Find(L,"ar");
  Assert.NotNull(rich);Assert.Equal(10,rich!.WorkedExamples.Count);
  Assert.All(rich.WorkedExamples,e=>Assert.True(e.Steps.Count>=3));
  Assert.True(LessonPracticeCapabilityResolver.TryResolve(L,out var c));
  Assert.Equal(6,c!.AllowedQuestionFamilies.Count);
  foreach(var f in c.AllowedQuestionFamilies)Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
 }
 [Fact] public void DecimalsAndFractionsRemainExact()
 {
  var p=new Dictionary<string,int>{{"removedCents",7025},{"remainCents",4350}};
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u07.l05.decimal_subtraction",p,"١١٣٫٧٥"));
  Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g6.u07.l05.decimal_subtraction",p,"113.74"));
  var q=new Dictionary<string,int>{{"denominator",3},{"removed",1},{"remain",2}};
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u07.l05.fraction_subtraction",q,"1"));
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u07.l05.fraction_subtraction",q,"3/3"));
  q["denominator"]=0;Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g6.u07.l05.fraction_subtraction",q,"1"));
 }
 [Fact] public void AdditionPropertyRequiresEqualOperations()
 {
  var p=new Dictionary<string,int>{{"mode",0}};
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u07.l05.addition_property",p,"نعم"));
  p["mode"]=1;
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u07.l05.addition_property",p,"لا"));
 }
}

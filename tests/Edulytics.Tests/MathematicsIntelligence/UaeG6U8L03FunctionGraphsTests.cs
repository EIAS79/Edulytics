using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG6U8L03FunctionGraphsTests
{
 const string L="PED:UAE-MOE-MATH:L6:GENERAL:08:03:STUDENT-AR-128";
 [Fact] public void RichContentHasThreeCartesianPlotsAndFiveFamilies()
 {
  var rich=RichLessonContentV2Registry.Find(L,"ar");
  Assert.NotNull(rich);Assert.Equal(10,rich!.WorkedExamples.Count);
  Assert.All(rich.WorkedExamples,e=>Assert.True(e.Steps.Count>=3));
  Assert.True(rich.Visuals.Count(v=>v.Kind==RichLessonVisualKind.CartesianPlot)>=3);
  Assert.True(LessonPracticeCapabilityResolver.TryResolve(L,out var c));
  Assert.Equal(5,c!.AllowedQuestionFamilies.Count);
  foreach(var f in c.AllowedQuestionFamilies)Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
 }
 [Fact] public void EquationAndInterceptAreExact()
 {
  var p=new Dictionary<string,int>{{"a",3},{"b",2}};
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u08.l03.equation_from_line",p,"ص=٣س+٢"));
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u08.l03.y_intercept",p,"٢"));
  Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g6.u08.l03.equation_from_line",p,"ص=٣س"));
 }
 [Fact] public void CorrectLinearityClassification()
 {
  var p=new Dictionary<string,int>{{"mode",0}};
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u08.l03.linear_vs_square",p,"خطية"));
  p["mode"]=1;
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u08.l03.linear_vs_square",p,"غير خطية"));
  p["mode"]=3;
  Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g6.u08.l03.linear_vs_square",p,"خطية"));
 }
}

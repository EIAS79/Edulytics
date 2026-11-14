using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG6U8L10SolveInequalityTests
{
 const string L="PED:UAE-MOE-MATH:L6:GENERAL:08:10:STUDENT-AR-128";
 [Fact] public void CompleteArabicLessonAndContract()
 {
  var r=RichLessonContentV2Registry.Find(L,"ar");
  Assert.NotNull(r);Assert.Equal(10,r!.WorkedExamples.Count);
  Assert.True(r.Visuals.Any(v=>v.Kind==RichLessonVisualKind.InequalityNumberLine));
  Assert.True(LessonPracticeCapabilityResolver.TryResolve(L,out var c));
  Assert.Equal(5,c!.AllowedQuestionFamilies.Count);
  foreach(var x in c.AllowedQuestionFamilies)Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(x));
 }
 [Fact] public void MultiplyDivideByPositiveKeepsInequalityDirection()
 {
  var p=new Dictionary<string,int>{{"op",1},{"factor",5},{"b",9}};
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u08.l10.solve_multiply_inequality",p,"س≤٩"));
  p["op"]=2;p["factor"]=8;p["b"]=3;
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u08.l10.solve_divide_inequality",p,"س>٢٤"));
  p["factor"]=0;
  Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g6.u08.l10.solve_divide_inequality",p,"س>٠"));
 }
}

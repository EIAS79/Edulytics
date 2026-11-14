using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG6U8L09SolveInequalityLabTests
{
 const string L="PED:UAE-MOE-MATH:L6:GENERAL:08:09:STUDENT-AR-128";
 const string A="uae.g6.u08.l09.solve_add_inequality", S="uae.g6.u08.l09.solve_subtract_inequality";
 [Fact] public void SourceArabicLabHasChartsAndExactAdapter()
 {
  var lesson=RichLessonContentV2Registry.Find(L,"ar");
  Assert.NotNull(lesson);Assert.Equal(10,lesson!.WorkedExamples.Count);
  Assert.All(lesson.WorkedExamples,e=>Assert.True(e.Steps.Count>=3));
  Assert.True(lesson.Visuals.Count(v=>v.Kind==RichLessonVisualKind.InequalityNumberLine)>=3);
  Assert.True(LessonPracticeCapabilityResolver.TryResolve(L,out var c));
  Assert.Equal(4,c!.AllowedQuestionFamilies.Count);
  foreach(var f in c.AllowedQuestionFamilies)Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
 }
 [Fact] public void AddAndSubtractSameAmountDoNotFlipInequality()
 {
  var a=new Dictionary<string,int>{{"op",0},{"b",126},{"change",9}};
  Assert.False(ExactSkillContractQuestionEngine.Verify(A,a,"س<١١٧")); // bounded generator excludes >25
  a["b"]=18;a["change"]=9;
  Assert.True(ExactSkillContractQuestionEngine.Verify(A,a,"س<٩"));
  var s=new Dictionary<string,int>{{"op",1},{"b",1},{"change",5}};
  Assert.True(ExactSkillContractQuestionEngine.Verify(S,s,"س≤٦"));
  s["op"]=3;s["b"]=8;s["change"]=3;
  Assert.True(ExactSkillContractQuestionEngine.Verify(S,s,"س≥١١"));
 }
}

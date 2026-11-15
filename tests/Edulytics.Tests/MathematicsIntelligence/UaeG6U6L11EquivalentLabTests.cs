using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG6U6L11EquivalentLabTests
{
 const string L="PED:UAE-MOE-MATH:L6:GENERAL:06:11:STUDENT-AR-128";
 [Fact] public void OriginalArabicTilesAndSixContractFamilies()
 {
  var rich=RichLessonContentV2Registry.Find(L,"ar");
  Assert.NotNull(rich);Assert.Equal(10,rich!.WorkedExamples.Count);
  Assert.All(rich.WorkedExamples,e=>Assert.True(e.Steps.Count>=3));
  Assert.Equal(2,rich.Visuals.Count(v=>v.Kind==RichLessonVisualKind.ExpressionStripBars));
  Assert.True(LessonPracticeCapabilityResolver.TryResolve(L,out var c));
  Assert.Equal(6,c!.AllowedQuestionFamilies.Count);
  foreach(var f in c.AllowedQuestionFamilies)
   Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
 }
 [Fact] public void LikeTermCombinationPreservesConstants()
 {
  var p=new Dictionary<string,int>{{"a",2},{"b",4},{"c",3}};
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l11.combine_like_terms",p,"٥س+٤"));
  Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l11.combine_like_terms",p,"9س"));
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l11.count_variable_tiles",p,"٥"));
  p["a"]=0;
  Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l11.count_variable_tiles",p,"3"));
 }
 [Fact] public void PairEquivalenceDetectsDifferentConstants()
 {
  var p=new Dictionary<string,int>{{"a",2},{"b",4},{"c",3},{"mode",0}};
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l11.equivalent_pair",p,"نعم"));
  p["mode"]=1;
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l11.equivalent_pair",p,"لا"));
  Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l11.equivalent_pair",p,"نعم"));
 }
}

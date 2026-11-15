using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG6U6L09DistributionLabTests
{
 const string L="PED:UAE-MOE-MATH:L6:GENERAL:06:09:STUDENT-AR-128";
 [Fact] public void ArabicLabHasSourceAlignedModelsAndReuse()
 {
  var rich=RichLessonContentV2Registry.Find(L,"ar");
  Assert.NotNull(rich);Assert.Equal(10,rich!.WorkedExamples.Count);
  Assert.All(rich.WorkedExamples,e=>Assert.True(e.Steps.Count>=3));
  Assert.True(LessonPracticeCapabilityResolver.TryResolve(L,out var c));
  Assert.Equal(5,c!.AllowedQuestionFamilies.Count);
  Assert.Contains("uae.g4.u04.l07.distributive_sum",c.AllowedQuestionFamilies);
  foreach(var f in c.AllowedQuestionFamilies)
    Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
 }
 [Fact] public void NumericDistributionAndTilesExact()
 {
  var p=new Dictionary<string,int>{{"a",3},{"b",6},{"c",22}};
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l09.evaluate_distributive",p,"٨٤"));
  Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l09.evaluate_distributive",p,"66"));
  var q=new Dictionary<string,int>{{"a",2},{"b",1}};
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l09.count_variable_tiles",q,"2"));
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l09.expand_variable",q,"٢س+٢"));
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l09.expand_variable",q,"2×س+2"));
  Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l09.expand_variable",q,"2س+1"));
  q["a"]=13;Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l09.expand_variable",q,"13س+13"));
 }
}

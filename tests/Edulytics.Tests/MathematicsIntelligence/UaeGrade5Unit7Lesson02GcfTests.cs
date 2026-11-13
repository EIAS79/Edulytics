using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Grade5 Arabic student pp492–497. Written for StageD; NOT_RUN.
public sealed class UaeGrade5Unit7Lesson02GcfTests
{
 private const string Lesson="PED:UAE-MOE-MATH:L5:GENERAL:07:02";
 [Fact] public void GcfThreeFamiliesReuseExistingFactorEngine()
 {
  Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
  Assert.Equal("supporting.number.factors_primes",contract!.SkillId);
  Assert.Equal(3,contract.AllowedQuestionFamilies.Count);
  var engine=new ExactSkillContractQuestionEngine();
  foreach(var f in contract.AllowedQuestionFamilies)
  {
    Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
    Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
    Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,ui!.InteractionKind);
    foreach(var q in engine.Generate(Lesson,f,[f],ExactSkillQuestionDifficulty.Standard,2,20261113,[]))
      Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
  }
 }
 [Fact] public void GcfMathFixturesMatchTripleTextbookSkill()
 {
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u07.l02.gcf_three_numbers",new Dictionary<string,int>{{"a",60},{"b",96},{"c",144}},"١٢"));
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u07.l02.gcf_two_numbers",new Dictionary<string,int>{{"a",18},{"b",24},{"c",24}},"6"));
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u07.l02.test_common_factor",new Dictionary<string,int>{{"a",45},{"b",70},{"c",5}},"1"));
  Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g5.u07.l02.gcf_two_numbers",new Dictionary<string,int>{{"a",18},{"b",24},{"c",24}},"9"));
 }
}

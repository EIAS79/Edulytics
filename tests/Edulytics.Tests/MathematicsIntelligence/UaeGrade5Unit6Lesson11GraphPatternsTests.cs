using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Source-aimed Stage A-B-C tests WRITTEN_NOT_RUN.
public sealed class UaeGrade5Unit6Lesson11GraphPatternsTests
{
 private const string Lesson="PED:UAE-MOE-MATH:L5:GENERAL:06:11";
 [Fact] public void G5LinearGraphFamiliesReuseCoreGraphEngine()
 {
  Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
  Assert.Equal("data.graphs",contract!.SkillId);
  Assert.Equal(3,contract.AllowedQuestionFamilies.Count);
  var engine=new ExactSkillContractQuestionEngine();
  foreach(var f in contract.AllowedQuestionFamilies)
  {
   Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
   Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
   Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,ui!.InteractionKind);
   foreach(var q in engine.Generate(Lesson,f,[f],ExactSkillQuestionDifficulty.Standard,2,20261113,[]))
   {
    Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
    Assert.False(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,"-999"));
   }
  }
 }
 [Fact] public void GraphRatesMatchOriginalArithmetic()
 {
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u06.l11.graph_value_at_hour",new Dictionary<string,int>{{"rate",5},{"h",4}},"20"));
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u06.l11.compare_linear_rentals",new Dictionary<string,int>{{"a",5},{"b",10},{"h",3}},"١٥"));
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u06.l11.graph_linear_increase",new Dictionary<string,int>{{"rate",4},{"from",1},{"to",4}},"12"));
 }
}

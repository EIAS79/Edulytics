using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Stage A-B-C source-aligned tests WRITTEN_NOT_RUN.
public sealed class UaeGrade5Unit6Lesson09MapLocationsTests
{
 private const string Lesson="PED:UAE-MOE-MATH:L5:GENERAL:06:09";
 [Fact] public void G5GridMapReusesGraphEngineAndNumericInteraction()
 {
  Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
  Assert.Equal("data.graphs",contract!.SkillId);
  Assert.Equal(3,contract.AllowedQuestionFamilies.Count);
  var engine=new ExactSkillContractQuestionEngine();
  foreach(var family in contract.AllowedQuestionFamilies)
  {
   Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
   Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
   Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,ui!.InteractionKind);
   foreach(var q in engine.Generate(Lesson,family,[family],ExactSkillQuestionDifficulty.Standard,2,20261113,[]))
   {
    Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
    Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"-999"));
   }
  }
 }
 [Fact] public void ExplicitGridRouteMathPreservesOrientation()
 {
  var p=new Dictionary<string,int>{{"sx",1},{"sy",1},{"tx",4},{"ty",5}};
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u06.l09.map_horizontal_steps",p,"٣"));
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u06.l09.map_vertical_steps",p,"4"));
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u06.l09.map_shortest_grid_route",p,"7"));
  Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g5.u06.l09.map_shortest_grid_route",p,"5"));
 }
}

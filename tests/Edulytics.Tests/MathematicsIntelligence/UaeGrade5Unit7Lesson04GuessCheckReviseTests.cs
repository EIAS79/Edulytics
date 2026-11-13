using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// StageD tests authored, NOT_RUN.
public sealed class UaeGrade5Unit7Lesson04GuessCheckReviseTests
{
 private const string Lesson="PED:UAE-MOE-MATH:L5:GENERAL:07:04";
 [Fact] public void GuessFamiliesRouteAndVerify()
 {
  Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var c));
  Assert.Equal(3,c!.AllowedQuestionFamilies.Count);
  var engine=new ExactSkillContractQuestionEngine();
  foreach(var f in c.AllowedQuestionFamilies)
  {
   Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
   Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out _));
   foreach(var q in engine.Generate(Lesson,f,[f],ExactSkillQuestionDifficulty.Standard,2,20261113,[]))
      Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
  }
 }
 [Fact] public void ChecksEveryConstraint()
 {
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u07.l04.cars_from_total_wheels",new Dictionary<string,int>{{"total",11},{"wheels",34}},"٦"));
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u07.l04.adults_from_ticket_revenue",new Dictionary<string,int>{{"tickets",12},{"amount",168}},"4"));
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u07.l04.validate_guess_wheels",new Dictionary<string,int>{{"total",9},{"wheels",26},{"guess",5}},"0"));
 }
}
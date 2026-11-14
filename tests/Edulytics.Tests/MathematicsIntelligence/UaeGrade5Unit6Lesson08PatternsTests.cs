using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Stage A-B-C authored fixtures; deliberately NOT_RUN.
public sealed class UaeGrade5Unit6Lesson08PatternsTests
{
 private const string Lesson="PED:UAE-MOE-MATH:L5:GENERAL:06:08";
 [Fact] public void FamiliesHaveExactPracticeContractsAndVerifiedGeneratedAnswers()
 {
  Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
  Assert.Equal("number.numeric_sequences_identify_extend",contract!.SkillId);
  Assert.Equal(3,contract.AllowedQuestionFamilies.Count);
  var engine=new ExactSkillContractQuestionEngine();
  foreach(var f in contract.AllowedQuestionFamilies)
  {
   Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
   Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var interaction));
   Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,interaction!.InteractionKind);
   foreach(var q in engine.Generate(Lesson,f,[f],ExactSkillQuestionDifficulty.Standard,2,20261113,[]))
   {
    Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
    Assert.False(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,"-1"));
   }
  }
 }
 [Fact] public void SourceAlignedAdditiveAndGeometricFixtures()
 {
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u06.l08.next_additive_term",new Dictionary<string,int>{{"start",4},{"step",4},{"terms",4}},"٢٠"));
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u06.l08.next_multiplicative_term",new Dictionary<string,int>{{"start",2},{"step",3},{"terms",4}},"162"));
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u06.l08.compare_weekly_growth",new Dictionary<string,int>{{"a",3},{"b",6},{"weeks",5}},"15"));
 }
}

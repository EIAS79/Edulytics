using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal93ExactPracticeFamiliesTests
{
 [Fact] public void FourExactLessonFamiliesWithNumberEntryAreRegistered()
 {
  Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:US-CCSS-MATH:G2:U06:L12",out var contract));
  Assert.NotNull(contract);Assert.Equal(4,contract!.AllowedQuestionFamilies.Count);
  foreach(var family in contract.AllowedQuestionFamilies)
  {
   Assert.StartsWith("usccss.g2.u06.l12.",family);
   Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
   Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
   Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,ui!.InteractionKind);
  }
 }
 [Fact] public void NumericSeedsPassVerifierAndTamperedParametersFail()
 {
  Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:US-CCSS-MATH:G2:U06:L12",out var contract));
  var engine=new ExactSkillContractQuestionEngine();
  foreach(var family in contract!.AllowedQuestionFamilies)
  foreach(var seed in new[]{20261022,20261023})
  {
   var questions=engine.Generate("im2021-g2-unit6-l12",family,[family],
     ExactSkillQuestionDifficulty.Standard,4,seed,[]);
   Assert.Equal(4,questions.Count);
   foreach(var q in questions)
   {
    Assert.Equal(12,q.Parameters["lesson"]);
    Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
    Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"-1"));
    var tampered=q.Parameters.ToDictionary(x=>x.Key,x=>x.Value,StringComparer.Ordinal);
    tampered["lesson"]=99;Assert.False(ExactSkillContractQuestionEngine.Verify(family,tampered,q.CorrectAnswer));
    tampered["lesson"]=12;tampered["a"]=10;
    Assert.False(ExactSkillContractQuestionEngine.Verify(family,tampered,q.CorrectAnswer));
   }
  }
 }
}

using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal74IndependentExactFamiliesTests
{
 [Fact] public void FourSeparateOriginalSkillMathFamiliesUseIntegerKeypad()
 {
  Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:US-CCSS-MATH:G2:U05:L07",out var contract));
  Assert.NotNull(contract);Assert.Equal(4,contract!.AllowedQuestionFamilies.Count);
  foreach(var family in contract.AllowedQuestionFamilies)
  {
   Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
   Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
   Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,ui!.InteractionKind);
  }
 }
 [Fact] public void ExactGenerationAndVerificationRejectIncorrectOrTamperedFields()
 {
  Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:US-CCSS-MATH:G2:U05:L07",out var contract));
  var engine=new ExactSkillContractQuestionEngine();
  foreach(var family in contract!.AllowedQuestionFamilies)
  foreach(var seed in new[]{20261022,20261023})
  {
   var problems=engine.Generate("source-im2021-u05-l07",family,[family],
      ExactSkillQuestionDifficulty.Standard,4,seed,[]);
   Assert.Equal(4,problems.Count);
   foreach(var p in problems)
   {
    Assert.Equal(7,p.Parameters["lesson"]);
    Assert.True(ExactSkillContractQuestionEngine.Verify(family,p.Parameters,p.CorrectAnswer));
    Assert.False(ExactSkillContractQuestionEngine.Verify(family,p.Parameters,"-1"));
    var corrupt=p.Parameters.ToDictionary(t=>t.Key,t=>t.Value,StringComparer.Ordinal);
    corrupt["lesson"]=15;
    Assert.False(ExactSkillContractQuestionEngine.Verify(family,corrupt,p.CorrectAnswer));
    corrupt["lesson"]=7;corrupt["step"]=9;
    Assert.False(ExactSkillContractQuestionEngine.Verify(family,corrupt,p.CorrectAnswer));
   }
  }
 }
}

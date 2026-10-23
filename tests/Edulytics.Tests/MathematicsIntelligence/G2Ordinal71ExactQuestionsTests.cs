using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal71ExactQuestionsTests
{
 [Fact] public void FourSpecificGeneratedFamiliesAndNumericKeypadAvailable()
 {
  Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:US-CCSS-MATH:G2:U05:L04",out var contract));
  Assert.NotNull(contract);Assert.Equal(4,contract!.AllowedQuestionFamilies.Count);
  foreach(var fam in contract.AllowedQuestionFamilies)
  {Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(fam));
   Assert.True(LessonPracticeInteractionRegistry.TryResolve(fam,out var ui));
   Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,ui!.InteractionKind);}
 }
 [Fact] public void AllGeneratedVariantsRejectIncorrectResponsesAndWrongContract()
 {
  Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:US-CCSS-MATH:G2:U05:L04",out var contract));
  var engine=new ExactSkillContractQuestionEngine();
  foreach(var fam in contract!.AllowedQuestionFamilies)
  foreach(var seed in new[]{20261022,20261023})
  {
   var qs=engine.Generate("g2-u05-l04",fam,[fam],
       ExactSkillQuestionDifficulty.Standard,4,seed,[]);
   Assert.Equal(4,qs.Count);
   foreach(var q in qs)
   {Assert.True(ExactSkillContractQuestionEngine.Verify(fam,q.Parameters,q.CorrectAnswer));
    Assert.False(ExactSkillContractQuestionEngine.Verify(fam,q.Parameters,"-1"));
    var bad=q.Parameters.ToDictionary(t=>t.Key,t=>t.Value,StringComparer.Ordinal);
    bad["lesson"]=99;
    Assert.False(ExactSkillContractQuestionEngine.Verify(fam,bad,q.CorrectAnswer));
    bad["lesson"]=4;bad["d1"]=0;
    Assert.False(ExactSkillContractQuestionEngine.Verify(fam,bad,q.CorrectAnswer));
   }
  }
 }
}

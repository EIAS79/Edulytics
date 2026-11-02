using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal130ExactPracticeFamiliesTests {
 [Fact] public void FourAllowedFamiliesGenerateVerifyAndRejectTampering(){
  Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:US-CCSS-MATH:G2:U08:L10",out var contract));
  Assert.NotNull(contract);Assert.Equal(4,contract!.AllowedQuestionFamilies.Count);
  var engine=new ExactSkillContractQuestionEngine();
  foreach(var family in contract.AllowedQuestionFamilies){
   Assert.StartsWith("usccss.g2.u08.l10.",family);
   Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
   Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
   Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,ui!.InteractionKind);
   var questions=engine.Generate("g2-unit8-10",family,[family],ExactSkillQuestionDifficulty.Standard,4,20261022,[]);
   Assert.Equal(4,questions.Count);
   foreach(var q in questions){
    Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
    Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"-1"));
    var changed=q.Parameters.ToDictionary(x=>x.Key,x=>x.Value,StringComparer.Ordinal);
    changed["lesson"]=99;Assert.False(ExactSkillContractQuestionEngine.Verify(family,changed,q.CorrectAnswer));
    changed["lesson"]=10;changed["a"]=10000;Assert.False(ExactSkillContractQuestionEngine.Verify(family,changed,q.CorrectAnswer));
   }
  }
 }
}
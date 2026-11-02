using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal135ExactPracticeFamiliesTests {
 [Fact] public void EveryFirstPartyFamilyIsMathematicallyExactAndTamperResistant(){
  Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:US-CCSS-MATH:G2:U09:L02",out var contract));
  Assert.NotNull(contract);Assert.Equal(4,contract!.AllowedQuestionFamilies.Count);
  var engine=new ExactSkillContractQuestionEngine();
  foreach(var family in contract.AllowedQuestionFamilies){
   Assert.StartsWith("usccss.g2.u09.l02.",family);
   Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
   Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
   Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,ui!.InteractionKind);
   var questions=engine.Generate("g2-u09-l2",family,[family],ExactSkillQuestionDifficulty.Standard,4,20261022,[]);
   Assert.Equal(4,questions.Count);
   foreach(var item in questions){
    Assert.Equal(2,item.Parameters["lesson"]);
    Assert.True(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,item.CorrectAnswer));
    Assert.False(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,"-1"));
    var tampered=item.Parameters.ToDictionary(x=>x.Key,x=>x.Value,StringComparer.Ordinal);
    tampered["lesson"]=99;Assert.False(ExactSkillContractQuestionEngine.Verify(family,tampered,item.CorrectAnswer));
    tampered["lesson"]=2;tampered["a"]=10000;Assert.False(ExactSkillContractQuestionEngine.Verify(family,tampered,item.CorrectAnswer));
   }
  }
 }
}
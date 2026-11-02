using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal142ExactPracticeFamiliesTests {
 [Fact] public void FourIndependentFamiliesGenerateAndVerifyByExactIntegerTruth(){
  Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:US-CCSS-MATH:G2:U09:L09",out var contract));
  Assert.NotNull(contract);Assert.Equal(4,contract!.AllowedQuestionFamilies.Count);
  var engine=new ExactSkillContractQuestionEngine();
  foreach(var f in contract.AllowedQuestionFamilies){
   Assert.StartsWith("usccss.g2.u09.l09.",f);
   Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
   Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
   Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,ui!.InteractionKind);
   var qs=engine.Generate("g2-u09-9",f,[f],ExactSkillQuestionDifficulty.Standard,4,20261022,[]);
   Assert.Equal(4,qs.Count);
   foreach(var q in qs){
    Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
    Assert.False(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,"-1"));
    var changed=q.Parameters.ToDictionary(x=>x.Key,x=>x.Value,StringComparer.Ordinal);
    changed["lesson"]=99;Assert.False(ExactSkillContractQuestionEngine.Verify(f,changed,q.CorrectAnswer));
    changed["lesson"]=9;changed["a"]=100;Assert.False(ExactSkillContractQuestionEngine.Verify(f,changed,q.CorrectAnswer));
   }
  }
 }
}
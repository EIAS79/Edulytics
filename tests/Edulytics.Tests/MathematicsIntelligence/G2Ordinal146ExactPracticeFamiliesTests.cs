using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal146ExactPracticeFamiliesTests{
 [Fact] public void FourExactIndependentFamiliesForFinalOriginalLesson(){
  Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:US-CCSS-MATH:G2:U09:L13",out var contract));
  Assert.NotNull(contract);Assert.Equal(4,contract!.AllowedQuestionFamilies.Count);
  var engine=new ExactSkillContractQuestionEngine();
  foreach(var f in contract.AllowedQuestionFamilies){
   Assert.StartsWith("usccss.g2.u09.l13.",f);
   Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
   Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
   Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,ui!.InteractionKind);
   var questions=engine.Generate("im2021-g2-u09-l13",f,[f],ExactSkillQuestionDifficulty.Standard,4,20261022,[]);
   Assert.Equal(4,questions.Count);
   foreach(var q in questions){
    Assert.Equal(13,q.Parameters["lesson"]);
    Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
    Assert.False(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,"-1"));
    var altered=q.Parameters.ToDictionary(x=>x.Key,x=>x.Value,StringComparer.Ordinal);
    altered["lesson"]=99;Assert.False(ExactSkillContractQuestionEngine.Verify(f,altered,q.CorrectAnswer));
    altered["lesson"]=13;altered["a"]=10000;Assert.False(ExactSkillContractQuestionEngine.Verify(f,altered,q.CorrectAnswer));
   }
  }
 }
}
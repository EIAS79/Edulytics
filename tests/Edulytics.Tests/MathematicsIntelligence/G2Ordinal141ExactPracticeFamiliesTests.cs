using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal141ExactPracticeFamiliesTests {
 [Fact] public void FourApprovedExactFamiliesGenerateSolveAndFailClosed(){
  Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:US-CCSS-MATH:G2:U09:L08",out var contract));
  Assert.NotNull(contract);Assert.Equal(4,contract!.AllowedQuestionFamilies.Count);
  var engine=new ExactSkillContractQuestionEngine();
  foreach(var f in contract.AllowedQuestionFamilies){
   Assert.StartsWith("usccss.g2.u09.l08.",f);
   Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
   Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var interaction));
   Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,interaction!.InteractionKind);
   var qs=engine.Generate("source-g2-u09-l8",f,[f],ExactSkillQuestionDifficulty.Standard,4,20261022,[]);
   Assert.Equal(4,qs.Count);
   foreach(var question in qs){
    Assert.True(ExactSkillContractQuestionEngine.Verify(f,question.Parameters,question.CorrectAnswer));
    Assert.False(ExactSkillContractQuestionEngine.Verify(f,question.Parameters,"-1"));
    var change=question.Parameters.ToDictionary(x=>x.Key,x=>x.Value,StringComparer.Ordinal);
    change["lesson"]=99;Assert.False(ExactSkillContractQuestionEngine.Verify(f,change,question.CorrectAnswer));
    change["lesson"]=8;change["a"]=100;Assert.False(ExactSkillContractQuestionEngine.Verify(f,change,question.CorrectAnswer));
   }
  }
 }
}
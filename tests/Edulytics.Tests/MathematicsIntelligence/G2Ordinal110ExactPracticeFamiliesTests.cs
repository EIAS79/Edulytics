using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal110ExactPracticeFamiliesTests
{
 [Fact] public void FourContractedFamiliesAreExactAndTamperResistant()
 {
  Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:US-CCSS-MATH:G2:U07:L08",out var contract));
  Assert.NotNull(contract);Assert.Equal(4,contract!.AllowedQuestionFamilies.Count);
  var engine=new ExactSkillContractQuestionEngine();
  foreach(var f in contract.AllowedQuestionFamilies)
  {
   Assert.StartsWith("usccss.g2.u07.l08.",f);
   Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
   Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
   Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,ui!.InteractionKind);
   var questions=engine.Generate("im2021-g2-u07-l8",f,[f],
     ExactSkillQuestionDifficulty.Standard,4,20261022,[]);
   Assert.Equal(4,questions.Count);
   foreach(var item in questions)
   {
    Assert.True(ExactSkillContractQuestionEngine.Verify(f,item.Parameters,item.CorrectAnswer));
    Assert.False(ExactSkillContractQuestionEngine.Verify(f,item.Parameters,"-1"));
    var bad=item.Parameters.ToDictionary(x=>x.Key,x=>x.Value,StringComparer.Ordinal);
    bad["lesson"]=99;Assert.False(ExactSkillContractQuestionEngine.Verify(f,bad,item.CorrectAnswer));
    bad["lesson"]=8;bad["a"]=100;Assert.False(ExactSkillContractQuestionEngine.Verify(f,bad,item.CorrectAnswer));
   }
  }
 }
}
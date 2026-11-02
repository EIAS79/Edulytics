using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal96ExactPracticeFamiliesTests
{
 [Fact] public void FourFirstPartyQuestionFamiliesResolveToNumericInput()
 {
  Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:US-CCSS-MATH:G2:U06:L15",out var contract));
  Assert.NotNull(contract);Assert.Equal(4,contract!.AllowedQuestionFamilies.Count);
  foreach(var f in contract.AllowedQuestionFamilies)
  {
   Assert.StartsWith("usccss.g2.u06.l15.",f);
   Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
   Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
   Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,ui!.InteractionKind);
  }
 }
 [Fact] public void GeneratedNumericProblemsVerifyExactlyAndRejectTampering()
 {
  Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:US-CCSS-MATH:G2:U06:L15",out var contract));
  var engine=new ExactSkillContractQuestionEngine();
  foreach(var f in contract!.AllowedQuestionFamilies)
  foreach(var seed in new[]{20261022,20261023})
  {
   var items=engine.Generate("im2021-g2-unit6-l15",f,[f],
     ExactSkillQuestionDifficulty.Standard,4,seed,[]);
   Assert.Equal(4,items.Count);
   foreach(var item in items)
   {
    Assert.Equal(15,item.Parameters["lesson"]);
    Assert.True(ExactSkillContractQuestionEngine.Verify(f,item.Parameters,item.CorrectAnswer));
    Assert.False(ExactSkillContractQuestionEngine.Verify(f,item.Parameters,"-1"));
    var bad=item.Parameters.ToDictionary(x=>x.Key,x=>x.Value,StringComparer.Ordinal);
    bad["lesson"]=99;Assert.False(ExactSkillContractQuestionEngine.Verify(f,bad,item.CorrectAnswer));
    bad["lesson"]=15;bad["a"]=10;
    Assert.False(ExactSkillContractQuestionEngine.Verify(f,bad,item.CorrectAnswer));
   }
  }
 }
}

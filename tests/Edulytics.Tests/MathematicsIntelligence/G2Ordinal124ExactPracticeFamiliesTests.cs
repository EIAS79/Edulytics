using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal124ExactPracticeFamiliesTests{
 [Fact] public void FourSeparateLessonFamiliesGenerateAndResistTampering(){
  Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:US-CCSS-MATH:G2:U08:L04",out var con));
  Assert.NotNull(con);Assert.Equal(4,con!.AllowedQuestionFamilies.Count);
  var engine=new ExactSkillContractQuestionEngine();
  foreach(var family in con.AllowedQuestionFamilies){
   Assert.StartsWith("usccss.g2.u08.l04.",family);
   Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
   Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
   Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,ui!.InteractionKind);
   var qs=engine.Generate("im2021-g2-u08-l4",family,[family],ExactSkillQuestionDifficulty.Standard,4,20261022,[]);
   Assert.Equal(4,qs.Count);
   foreach(var item in qs){
    Assert.True(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,item.CorrectAnswer));
    Assert.False(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,"-1"));
    var bad=item.Parameters.ToDictionary(x=>x.Key,x=>x.Value,StringComparer.Ordinal);
    bad["lesson"]=99;Assert.False(ExactSkillContractQuestionEngine.Verify(family,bad,item.CorrectAnswer));
    bad["lesson"]=4;bad["a"]=10000;Assert.False(ExactSkillContractQuestionEngine.Verify(family,bad,item.CorrectAnswer));
   }
  }
 }
}
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal103ExactPracticeFamiliesTests
{
 [Fact] public void FourDistinctFirstPartyFamiliesHaveCorrectLearnerContracts()
 {
  Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:US-CCSS-MATH:G2:U07:L01",out var con));
  Assert.NotNull(con);Assert.Equal(4,con!.AllowedQuestionFamilies.Count);
  foreach(var f in con.AllowedQuestionFamilies)
  {
   Assert.StartsWith("usccss.g2.u07.l01.",f);
   Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
   Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
   Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,ui!.InteractionKind);
  }
 }
 [Fact] public void GenerationSolvingAndTamperedParametersFailClosed()
 {
  Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:US-CCSS-MATH:G2:U07:L01",out var con));
  var engine=new ExactSkillContractQuestionEngine();
  foreach(var f in con!.AllowedQuestionFamilies)
  foreach(var seed in new[]{20261022,20261023})
  {
   var q=engine.Generate("im2021-g2-u07-1",f,[f],
     ExactSkillQuestionDifficulty.Standard,4,seed,[]);
   Assert.Equal(4,q.Count);
   foreach(var item in q)
   {
    Assert.Equal(1,item.Parameters["lesson"]);
    Assert.True(ExactSkillContractQuestionEngine.Verify(f,item.Parameters,item.CorrectAnswer));
    Assert.False(ExactSkillContractQuestionEngine.Verify(f,item.Parameters,"-1"));
    var broken=item.Parameters.ToDictionary(x=>x.Key,x=>x.Value,StringComparer.Ordinal);
    broken["lesson"]=99;Assert.False(ExactSkillContractQuestionEngine.Verify(f,broken,item.CorrectAnswer));
    broken["lesson"]=1;broken["a"]=9999;
    Assert.False(ExactSkillContractQuestionEngine.Verify(f,broken,item.CorrectAnswer));
   }
  }
 }
}
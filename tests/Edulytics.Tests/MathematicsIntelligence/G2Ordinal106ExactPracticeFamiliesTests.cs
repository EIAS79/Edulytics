using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal106ExactPracticeFamiliesTests{
 [Fact] public void FourExactSourceAlignedFirstPartyFamilies(){
 Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:US-CCSS-MATH:G2:U07:L04",out var con));
 Assert.NotNull(con);Assert.Equal(4,con!.AllowedQuestionFamilies.Count);
 var engine=new ExactSkillContractQuestionEngine();
 foreach(var f in con.AllowedQuestionFamilies){
 Assert.StartsWith("usccss.g2.u07.l04.",f);
 Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
 Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
 Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,ui!.InteractionKind);
 var qs=engine.Generate("im2021-g2-u07-l04",f,[f],ExactSkillQuestionDifficulty.Standard,4,20261022,[]);
 Assert.Equal(4,qs.Count);
 foreach(var q in qs){
 Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
 Assert.False(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,"-1"));
 var bad=q.Parameters.ToDictionary(x=>x.Key,x=>x.Value,StringComparer.Ordinal);
 bad["lesson"]=99;Assert.False(ExactSkillContractQuestionEngine.Verify(f,bad,q.CorrectAnswer));
 bad["lesson"]=4;bad["a"]=10000;Assert.False(ExactSkillContractQuestionEngine.Verify(f,bad,q.CorrectAnswer));
 }
 }
 }
}
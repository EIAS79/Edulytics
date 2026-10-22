using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal115ExactPracticeFamiliesTests
{
 [Fact] public void FourOriginalSkillAlignedFirstPartyMathFamiliesGenerateAndRejectTampering()
 {
  Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:US-CCSS-MATH:G2:U07:L13",out var c));
  Assert.NotNull(c);Assert.Equal(4,c!.AllowedQuestionFamilies.Count);
  var engine=new ExactSkillContractQuestionEngine();
  foreach(var f in c.AllowedQuestionFamilies)
  {
   Assert.StartsWith("usccss.g2.u07.l13.",f);
   Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
   Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
   Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,ui!.InteractionKind);
   var questions=engine.Generate("im2021-g2-u07-13",f,[f],ExactSkillQuestionDifficulty.Standard,4,20261023,[]);
   Assert.Equal(4,questions.Count);
   foreach(var item in questions)
   {
    Assert.Equal(13,item.Parameters["lesson"]);
    Assert.True(ExactSkillContractQuestionEngine.Verify(f,item.Parameters,item.CorrectAnswer));
    Assert.False(ExactSkillContractQuestionEngine.Verify(f,item.Parameters,"-1"));
    var bad=item.Parameters.ToDictionary(x=>x.Key,x=>x.Value,StringComparer.Ordinal);
    bad["lesson"]=77;Assert.False(ExactSkillContractQuestionEngine.Verify(f,bad,item.CorrectAnswer));
    bad["lesson"]=13;bad["a"]=99;Assert.False(ExactSkillContractQuestionEngine.Verify(f,bad,item.CorrectAnswer));
   }
  }
 }
}
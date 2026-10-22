using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal105ExactPracticeFamiliesTests
{
 [Fact] public void FourFamiliesAreIntegratedIntoExactEngineAndIntegerInteraction()
 {
  Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:US-CCSS-MATH:G2:U07:L03",out var lesson));
  Assert.NotNull(lesson);Assert.Equal(4,lesson!.AllowedQuestionFamilies.Count);
  var engine=new ExactSkillContractQuestionEngine();
  foreach(var family in lesson.AllowedQuestionFamilies)
  {
   Assert.StartsWith("usccss.g2.u07.l03.",family);
   Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
   Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var interaction));
   Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,interaction!.InteractionKind);
   var q=engine.Generate("im2021-g2-u07-3",family,[family],ExactSkillQuestionDifficulty.Standard,4,20261022,[]);
   Assert.Equal(4,q.Count);
   foreach(var item in q)
   {
    Assert.True(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,item.CorrectAnswer));
    Assert.False(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,"-1"));
    var altered=item.Parameters.ToDictionary(x=>x.Key,x=>x.Value,StringComparer.Ordinal);
    altered["lesson"]=99;Assert.False(ExactSkillContractQuestionEngine.Verify(family,altered,item.CorrectAnswer));
    altered["lesson"]=3;altered["a"]=10000;
    Assert.False(ExactSkillContractQuestionEngine.Verify(family,altered,item.CorrectAnswer));
   }
  }
 }
}
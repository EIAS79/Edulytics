using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Stage C written NOT_RUN.
public sealed class UaeG3U06L06PatternTests
{
 [Fact] public void ExactSharedHandlerUnderstandsFivePatternModes()
 {
  const string f="uae.g3.u06.l06.find_mul_div_pattern";
  Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:UAE-MOE-MATH:L3:COMMON:06:06",out var contract));
  Assert.Contains(f,contract!.AllowedQuestionFamilies);
  Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
  Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
  Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,ui!.InteractionKind);
  var e=new ExactSkillContractQuestionEngine();
  foreach(int seed in new[]{1,2,3,4,5,6,7,8,9,10})
  foreach(var item in e.Generate("uae-g3-u06-l06",f,[f],ExactSkillQuestionDifficulty.Standard,8,seed,[]))
  {
   int a=item.Parameters["a"],b=item.Parameters["b"],mode=item.Parameters["mode"];
   int expected=mode==3?1<<(b+3):mode==4?1<<(b-3):(b+3)*a;
   Assert.Equal(expected,int.Parse(item.CorrectAnswer));
   Assert.True(ExactSkillContractQuestionEngine.Verify(f,item.Parameters,item.CorrectAnswer));
   var bad=item.Parameters.ToDictionary(x=>x.Key,x=>x.Value);bad["result"]++;
   Assert.False(ExactSkillContractQuestionEngine.Verify(f,bad,item.CorrectAnswer));
  }
 }
}

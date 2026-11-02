using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal66ExactFamiliesTests
{
 [Fact] public void FourSourceRestrictedFamiliesUseExactStudentKeypad()
 {
  Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:US-CCSS-MATH:G2:U04:L14",out var contract));
  Assert.NotNull(contract);Assert.Equal(4,contract!.AllowedQuestionFamilies.Count);
  foreach(var f in contract.AllowedQuestionFamilies)
  {
   Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
   Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
   Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,ui!.InteractionKind);
  }
 }
 [Fact] public void SeededExactGenerationRejectsTamperedContractParameters()
 {
  Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:US-CCSS-MATH:G2:U04:L14",out var contract));
  var gen=new ExactSkillContractQuestionEngine();
  foreach(var f in contract!.AllowedQuestionFamilies)
  foreach(var seed in new[]{20261022,20261023})
  {
   var qs=gen.Generate("im2021-g2-original66",f,[f],ExactSkillQuestionDifficulty.Standard,4,seed,[]);
   Assert.Equal(4,qs.Count);
   foreach(var q in qs)
   {
    Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
    Assert.False(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,"-1"));
    var bad=q.Parameters.ToDictionary(p=>p.Key,p=>p.Value,StringComparer.Ordinal);
    bad["lesson"]=99;
    Assert.False(ExactSkillContractQuestionEngine.Verify(f,bad,q.CorrectAnswer));
   }
  }
 }
}

using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Stage A/B/C authored; NOT_RUN until Stage D.
public sealed class UaeGrade5Unit8Lesson02LikeFractionTests
{
 [Fact] public void ExpandedLikeFractionAdditionIsExact()
 {
  const string f="uae.g5.u08.l02.add_like_extended_fraction";
  var p=new Dictionary<string,int>{{"a",13},{"b",2},{"denominator",16}};
  Assert.True(ExactSkillContractQuestionEngine.Verify(f,p,"١٥/١٦"));
  p["a"]=15;p["b"]=9;p["denominator"]=18;
  Assert.True(ExactSkillContractQuestionEngine.Verify(f,p,"4/3"));
  Assert.False(ExactSkillContractQuestionEngine.Verify(f,p,"24/18"));
 }
 [Fact] public void GradeFiveReusesGradeFourExactFamilies()
 {
  Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:UAE-MOE-MATH:L5:GENERAL:08:02",out var c));
  Assert.Equal("fractions.add",c!.SkillId);
  Assert.Equal(4,c.AllowedQuestionFamilies.Count);
  foreach(var f in c.AllowedQuestionFamilies)
  {
   Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
   Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
   Assert.NotNull(ui);
  }
 }
}
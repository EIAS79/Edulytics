using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Source-aligned tests written for Stage D; NOT RUN.
public sealed class UaeGrade5Unit7Lesson01FractionsDivisionTests
{
 private const string Lesson="PED:UAE-MOE-MATH:L5:GENERAL:07:01";
 [Fact] public void Grade5DivisionAsFractionHasExactFamilyAndRouting()
 {
   Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
   Assert.Equal("number.fractions.division_as_fraction",contract!.SkillId);
   Assert.Equal(3,contract.AllowedQuestionFamilies.Count);
   foreach(var family in contract.AllowedQuestionFamilies)
   {
      Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
      Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var interaction));
      Assert.NotNull(interaction);
   }
 }
 [Fact] public void EqualSharingProducesExactFractionAndWholeRemainder()
 {
   var p=new Dictionary<string,int>{{"n",5},{"d",4}};
   Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u07.l01.division_as_fraction",p,"٥/٤"));
   Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u07.l01.quotient_whole_part",p,"1"));
   Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u07.l01.quotient_remainder",p,"1"));
   Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g5.u07.l01.division_as_fraction",p,"4/5"));
 }
}

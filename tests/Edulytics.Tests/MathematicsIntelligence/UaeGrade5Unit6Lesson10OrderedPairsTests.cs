using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Grade5 local Arabic pages461–466. WRITTEN_NOT_RUN at A–B–C.
public sealed class UaeGrade5Unit6Lesson10OrderedPairsTests
{
 private const string Lesson="PED:UAE-MOE-MATH:L5:GENERAL:06:10";
 [Fact] public void XandYAndOrderedPairHaveDistinctAnswerInterfaces()
 {
  Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
  Assert.Equal("data.graphs",contract!.SkillId);
  Assert.Equal(3,contract.AllowedQuestionFamilies.Count);
  foreach(var f in contract.AllowedQuestionFamilies)
  {
   Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
   Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
   Assert.NotNull(ui);
  }
  Assert.True(LessonPracticeInteractionRegistry.TryResolve("uae.g5.u06.l10.write_ordered_pair",out var pair));
  Assert.Equal(LessonPracticeInteractionKind.StructuredEntry,pair!.InteractionKind);
 }
 [Fact] public void CoordinatesAreOrderedAndArabicDigitsAreAccepted()
 {
  var p=new Dictionary<string,int>{{"x",3},{"y",5}};
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u06.l10.coordinate_x",p,"٣"));
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u06.l10.coordinate_y",p,"5"));
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u06.l10.write_ordered_pair",p,"(٣،٥)"));
  Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g5.u06.l10.write_ordered_pair",p,"(٥،٣)"));
 }
}

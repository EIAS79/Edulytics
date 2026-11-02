using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal71MathAndTeacherBoundaryTests
{
 [Fact] public void OriginalNumericQuestionsAreAccurateButPublisherFiguresNeedTeacher()
 {
  var s=UsCcssG2Unit5FirstSixOriginal.Source(4,"672","253","509","180");
  Assert.True(s.Correct);Assert.True(s.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit5FirstSixOriginal.Source(4,"999","253","509","180").Correct);
  Assert.NotEmpty(UsCcssG2Unit5FirstSixOriginal.TeacherRubrics[4]);
 }
 [Fact] public void FirstPartyIndependentChecksCannotImpersonateStudentWork()
 {
  var s=UsCcssG2Unit5FirstSixOriginal.FirstParty(4,"507","609","482","30");
  Assert.True(s.Correct);Assert.False(s.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit5FirstSixOriginal.FirstParty(4,"999","609","482","30").Correct);
 }
}

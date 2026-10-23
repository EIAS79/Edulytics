using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal73MathAndTeacherBoundaryTests
{
 [Fact] public void OriginalNumericQuestionsAreAccurateButPublisherFiguresNeedTeacher()
 {
  var s=UsCcssG2Unit5FirstSixOriginal.Source(6,"248","562","433","951");
  Assert.True(s.Correct);Assert.True(s.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit5FirstSixOriginal.Source(6,"999","562","433","951").Correct);
  Assert.NotEmpty(UsCcssG2Unit5FirstSixOriginal.TeacherRubrics[6]);
 }
 [Fact] public void FirstPartyIndependentChecksCannotImpersonateStudentWork()
 {
  var s=UsCcssG2Unit5FirstSixOriginal.FirstParty(6,"248","562","433","951");
  Assert.True(s.Correct);Assert.False(s.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit5FirstSixOriginal.FirstParty(6,"999","562","433","951").Correct);
 }
}

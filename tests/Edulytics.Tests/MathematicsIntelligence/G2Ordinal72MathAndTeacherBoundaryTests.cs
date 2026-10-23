using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal72MathAndTeacherBoundaryTests
{
 [Fact] public void OriginalNumericQuestionsAreAccurateButPublisherFiguresNeedTeacher()
 {
  var s=UsCcssG2Unit5FirstSixOriginal.Source(5,"357","192","238","316");
  Assert.True(s.Correct);Assert.True(s.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit5FirstSixOriginal.Source(5,"999","192","238","316").Correct);
  Assert.NotEmpty(UsCcssG2Unit5FirstSixOriginal.TeacherRubrics[5]);
 }
 [Fact] public void FirstPartyIndependentChecksCannotImpersonateStudentWork()
 {
  var s=UsCcssG2Unit5FirstSixOriginal.FirstParty(5,"465","500","654","456");
  Assert.True(s.Correct);Assert.False(s.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit5FirstSixOriginal.FirstParty(5,"999","500","654","456").Correct);
 }
}

using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal70MathAndTeacherBoundaryTests
{
 [Fact]
 public void OriginalSourceNumericTasksStillNeedRealClassroomReview()
 {
  var source=UsCcssG2Unit5FirstSixOriginal.Source(3,"84","495","188","283");
  Assert.True(source.Correct);Assert.True(source.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit5FirstSixOriginal.Source(3,"999","495","188","283").Correct);
  Assert.NotEmpty(UsCcssG2Unit5FirstSixOriginal.TeacherRubrics[3]);
 }
 [Fact]
 public void NewEdulyticsAnswersMustNotCertifySourcePictures()
 {
  var own=UsCcssG2Unit5FirstSixOriginal.FirstParty(3,"495","4","9","5");
  Assert.True(own.Correct);Assert.False(own.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit5FirstSixOriginal.FirstParty(3,"999","4","9","5").Correct);
 }
}

using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal113SourceAndNumericBoundaryTests
{
 [Fact] public void OriginalStudentWorkAlwaysRemainsTeacherReviewed()
 {
  var s=UsCcssG2Unit7OriginalActivities.Source(11,"137","179","137","200");
  Assert.True(s.Correct);Assert.True(s.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit7OriginalActivities.Source(11,"999","179","137","200").Correct);
  Assert.NotEmpty(UsCcssG2Unit7OriginalActivities.AuthenticTasks[11]);
  Assert.Equal(4,UsCcssG2Unit7OriginalActivities.LastSourceNumericExamples[11].Length);
 }
 [Fact] public void FirstPartyNumericCorrectionDoesNotCertifyPublisherImages()
 {
  var s=UsCcssG2Unit7OriginalActivities.NewMath(11,"140","150","125","790");
  Assert.True(s.Correct);Assert.False(s.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit7OriginalActivities.NewMath(11,"999","150","125","790").Correct);
 }
}
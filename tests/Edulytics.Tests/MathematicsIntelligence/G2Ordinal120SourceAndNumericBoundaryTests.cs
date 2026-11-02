using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal120SourceAndNumericBoundaryTests
{
 [Fact] public void OriginalStudentWorkAlwaysRemainsTeacherReviewed()
 {
  var s=UsCcssG2Unit7OriginalActivities.Source(18,"0","0","0","0");
  Assert.Null(s.Correct);Assert.True(s.TeacherReviewNeeded);
  Assert.Contains("educator",s.Feedback,StringComparison.OrdinalIgnoreCase);
  Assert.Equal(4,UsCcssG2Unit7OriginalActivities.LastSourceNumericExamples[18].Length);
 }
 [Fact] public void FirstPartyNumericCorrectionDoesNotCertifyPublisherImages()
 {
  var s=UsCcssG2Unit7OriginalActivities.NewMath(18,"15","255","15","2");
  Assert.True(s.Correct);Assert.False(s.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit7OriginalActivities.NewMath(18,"999","255","15","2").Correct);
 }
}
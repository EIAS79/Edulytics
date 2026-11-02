using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal119SourceAndNumericBoundaryTests
{
 [Fact] public void OriginalStudentWorkAlwaysRemainsTeacherReviewed()
 {
  var s=UsCcssG2Unit7OriginalActivities.Source(17,"25","335","269","337");
  Assert.True(s.Correct);Assert.True(s.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit7OriginalActivities.Source(17,"999","335","269","337").Correct);
  Assert.NotEmpty(UsCcssG2Unit7OriginalActivities.AuthenticTasks[12]);
  Assert.Equal(4,UsCcssG2Unit7OriginalActivities.LastSourceNumericExamples[17].Length);
 }
 [Fact] public void FirstPartyNumericCorrectionDoesNotCertifyPublisherImages()
 {
  var s=UsCcssG2Unit7OriginalActivities.NewMath(17,"275","400","710","480");
  Assert.True(s.Correct);Assert.False(s.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit7OriginalActivities.NewMath(17,"999","400","710","480").Correct);
 }
}
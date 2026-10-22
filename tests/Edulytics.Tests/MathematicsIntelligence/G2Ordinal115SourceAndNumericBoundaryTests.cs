using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal115SourceAndNumericBoundaryTests
{
 [Fact] public void OriginalStudentWorkAlwaysRemainsTeacherReviewed()
 {
  var s=UsCcssG2Unit7OriginalActivities.Source(13,"284","216","192","180");
  Assert.True(s.Correct);Assert.True(s.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit7OriginalActivities.Source(13,"999","216","192","180").Correct);
  Assert.NotEmpty(UsCcssG2Unit7OriginalActivities.AuthenticTasks[12]);
  Assert.Equal(4,UsCcssG2Unit7OriginalActivities.LastSourceNumericExamples[13].Length);
 }
 [Fact] public void FirstPartyNumericCorrectionDoesNotCertifyPublisherImages()
 {
  var s=UsCcssG2Unit7OriginalActivities.NewMath(13,"284","216","192","217");
  Assert.True(s.Correct);Assert.False(s.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit7OriginalActivities.NewMath(13,"999","216","192","217").Correct);
 }
}
using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal114SourceAndNumericBoundaryTests
{
 [Fact] public void OriginalStudentWorkAlwaysRemainsTeacherReviewed()
 {
  var s=UsCcssG2Unit7OriginalActivities.Source(12,"347","318","306","226");
  Assert.True(s.Correct);Assert.True(s.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit7OriginalActivities.Source(12,"999","318","306","226").Correct);
  Assert.NotEmpty(UsCcssG2Unit7OriginalActivities.AuthenticTasks[12]);
  Assert.Equal(4,UsCcssG2Unit7OriginalActivities.LastSourceNumericExamples[12].Length);
 }
 [Fact] public void FirstPartyNumericCorrectionDoesNotCertifyPublisherImages()
 {
  var s=UsCcssG2Unit7OriginalActivities.NewMath(12,"346","225","216","136");
  Assert.True(s.Correct);Assert.False(s.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit7OriginalActivities.NewMath(12,"999","225","216","136").Correct);
 }
}
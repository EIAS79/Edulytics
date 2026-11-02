using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal86SourceAndNumericBoundaryTests
{
 [Fact] public void ExactOriginalNumericFactsStillNeedTeacherRealOriginalEvidence()
 {
  var src=UsCcssG2Unit6OriginalActivities.Source(5,"40","85","90","535");
  Assert.True(src.Correct);Assert.True(src.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit6OriginalActivities.Source(5,"999","85","90","535").Correct);
  Assert.NotEmpty(UsCcssG2Unit6OriginalActivities.AuthenticTasks[5]);
 }
 [Fact] public void FirstPartyChecksDoNotAssertPublisherDiagramSuccess()
 {
  var own=UsCcssG2Unit6OriginalActivities.NewMath(5,"65","40","4","325");
  Assert.True(own.Correct);Assert.False(own.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit6OriginalActivities.NewMath(5,"999","40","4","325").Correct);
 }
}

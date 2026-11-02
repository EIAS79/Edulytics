using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal83SourceAndNumericBoundaryTests
{
 [Fact] public void ExactOriginalNumericFactsStillNeedTeacherRealOriginalEvidence()
 {
  var src=UsCcssG2Unit6OriginalActivities.Source(2,"4","5","6","1");
  Assert.True(src.Correct);Assert.True(src.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit6OriginalActivities.Source(2,"999","5","6","1").Correct);
  Assert.NotEmpty(UsCcssG2Unit6OriginalActivities.AuthenticTasks[2]);
 }
 [Fact] public void FirstPartyChecksDoNotAssertPublisherDiagramSuccess()
 {
  var own=UsCcssG2Unit6OriginalActivities.NewMath(2,"4","5","6","6");
  Assert.True(own.Correct);Assert.False(own.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit6OriginalActivities.NewMath(2,"999","5","6","6").Correct);
 }
}

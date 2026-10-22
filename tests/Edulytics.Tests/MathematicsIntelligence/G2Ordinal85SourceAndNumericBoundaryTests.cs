using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal85SourceAndNumericBoundaryTests
{
 [Fact] public void ExactOriginalNumericFactsStillNeedTeacherRealOriginalEvidence()
 {
  var src=UsCcssG2Unit6OriginalActivities.Source(4,"6","8","12","5");
  Assert.True(src.Correct);Assert.True(src.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit6OriginalActivities.Source(4,"999","8","12","5").Correct);
  Assert.NotEmpty(UsCcssG2Unit6OriginalActivities.AuthenticTasks[4]);
 }
 [Fact] public void FirstPartyChecksDoNotAssertPublisherDiagramSuccess()
 {
  var own=UsCcssG2Unit6OriginalActivities.NewMath(4,"6","8","12","5");
  Assert.True(own.Correct);Assert.False(own.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit6OriginalActivities.NewMath(4,"999","8","12","5").Correct);
 }
}

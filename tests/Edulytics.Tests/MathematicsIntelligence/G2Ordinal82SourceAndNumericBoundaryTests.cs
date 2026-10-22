using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal82SourceAndNumericBoundaryTests
{
 [Fact] public void ExactOriginalNumericFactsStillNeedTeacherRealOriginalEvidence()
 {
  var src=UsCcssG2Unit6OriginalActivities.Source(1,"3","4","5","6");
  Assert.True(src.Correct);Assert.True(src.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit6OriginalActivities.Source(1,"999","4","5","6").Correct);
  Assert.NotEmpty(UsCcssG2Unit6OriginalActivities.AuthenticTasks[1]);
 }
 [Fact] public void FirstPartyChecksDoNotAssertPublisherDiagramSuccess()
 {
  var own=UsCcssG2Unit6OriginalActivities.NewMath(1,"3","4","5","6");
  Assert.True(own.Correct);Assert.False(own.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit6OriginalActivities.NewMath(1,"999","4","5","6").Correct);
 }
}

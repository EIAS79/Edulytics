using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal87SourceAndNumericBoundaryTests
{
 [Fact] public void ExactOriginalNumericFactsStillNeedTeacherRealOriginalEvidence()
 {
  var src=UsCcssG2Unit6OriginalActivities.Source(6,"2","3","4","4");
  Assert.True(src.Correct);Assert.True(src.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit6OriginalActivities.Source(6,"999","3","4","4").Correct);
  Assert.NotEmpty(UsCcssG2Unit6OriginalActivities.AuthenticTasks[6]);
 }
 [Fact] public void FirstPartyChecksDoNotAssertPublisherDiagramSuccess()
 {
  var own=UsCcssG2Unit6OriginalActivities.NewMath(6,"6","2","4","10");
  Assert.True(own.Correct);Assert.False(own.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit6OriginalActivities.NewMath(6,"999","2","4","10").Correct);
 }
}

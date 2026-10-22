using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal89SourceAndNumericBoundaryTests
{
 [Fact] public void OriginalSourceNumericFactsRemainHumanReviewed()
 {
  var source=UsCcssG2Unit6OriginalActivities.Source(8,"10","25","40","25");
  Assert.True(source.Correct);Assert.True(source.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit6OriginalActivities.Source(8,"999","25","40","25").Correct);
  Assert.Contains("equal",UsCcssG2Unit6OriginalActivities.AuthenticTasks[8].ToLowerInvariant());
 }
 [Fact] public void FirstPartyNumericExamplesDoNotProveOriginalVisualCorrectness()
 {
  var own=UsCcssG2Unit6OriginalActivities.NewMath(8,"2","4","8","45");
  Assert.True(own.Correct);Assert.False(own.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit6OriginalActivities.NewMath(8,"999","4","8","45").Correct);
 }
}

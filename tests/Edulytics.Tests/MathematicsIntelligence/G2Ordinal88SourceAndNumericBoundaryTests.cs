using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal88SourceAndNumericBoundaryTests
{
 [Fact] public void SourceNumericalFactsCannotApproveRealEqualAreaFolds()
 {
  var original=UsCcssG2Unit6OriginalActivities.Source(7,"2","3","4","3");
  Assert.True(original.Correct);Assert.True(original.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit6OriginalActivities.Source(7,"999","3","4","3").Correct);
  Assert.Contains("equal-area",UsCcssG2Unit6OriginalActivities.AuthenticTasks[7]);
 }
 [Fact] public void SeparateFirstPartyNumbersNeverApprovePublisherPaper()
 {
  var own=UsCcssG2Unit6OriginalActivities.NewMath(7,"2","3","4","9");
  Assert.True(own.Correct);Assert.False(own.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit6OriginalActivities.NewMath(7,"999","3","4","9").Correct);
 }
}

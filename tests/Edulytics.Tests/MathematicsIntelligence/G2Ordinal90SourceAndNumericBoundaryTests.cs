using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal90SourceAndNumericBoundaryTests
{
 [Fact] public void OriginalSourceNumericFactsRemainHumanReviewed()
 {
  var source=UsCcssG2Unit6OriginalActivities.Source(9,"45","55","50","55");
  Assert.True(source.Correct);Assert.True(source.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit6OriginalActivities.Source(9,"999","55","50","55").Correct);
  Assert.Contains("pizza",UsCcssG2Unit6OriginalActivities.AuthenticTasks[9].ToLowerInvariant());
 }
 [Fact] public void FirstPartyNumericExamplesDoNotProveOriginalVisualCorrectness()
 {
  var own=UsCcssG2Unit6OriginalActivities.NewMath(9,"3","2","4","1");
  Assert.True(own.Correct);Assert.False(own.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit6OriginalActivities.NewMath(9,"999","2","4","1").Correct);
 }
}

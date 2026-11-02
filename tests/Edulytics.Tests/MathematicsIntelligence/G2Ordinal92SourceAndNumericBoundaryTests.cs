using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal92SourceAndNumericBoundaryTests
{
 [Fact] public void OriginalSourceNumericFactsRemainHumanReviewed()
 {
  var source=UsCcssG2Unit6OriginalActivities.Source(11,"0","15","30","45");
  Assert.True(source.Correct);Assert.True(source.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit6OriginalActivities.Source(11,"999","15","30","45").Correct);
  Assert.Contains("clock",UsCcssG2Unit6OriginalActivities.AuthenticTasks[11].ToLowerInvariant());
 }
 [Fact] public void FirstPartyNumericExamplesDoNotProveOriginalVisualCorrectness()
 {
  var own=UsCcssG2Unit6OriginalActivities.NewMath(11,"15","30","45","25");
  Assert.True(own.Correct);Assert.False(own.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit6OriginalActivities.NewMath(11,"999","30","45","25").Correct);
 }
}

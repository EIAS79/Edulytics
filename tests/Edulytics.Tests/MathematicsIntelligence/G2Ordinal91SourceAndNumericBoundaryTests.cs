using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal91SourceAndNumericBoundaryTests
{
 [Fact] public void OriginalSourceNumericFactsRemainHumanReviewed()
 {
  var source=UsCcssG2Unit6OriginalActivities.Source(10,"25","50","75","100");
  Assert.True(source.Correct);Assert.True(source.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit6OriginalActivities.Source(10,"999","50","75","100").Correct);
  Assert.Contains("optional",UsCcssG2Unit6OriginalActivities.AuthenticTasks[10].ToLowerInvariant());
 }
 [Fact] public void FirstPartyNumericExamplesDoNotProveOriginalVisualCorrectness()
 {
  var own=UsCcssG2Unit6OriginalActivities.NewMath(10,"75","50","100","80");
  Assert.True(own.Correct);Assert.False(own.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit6OriginalActivities.NewMath(10,"999","50","100","80").Correct);
 }
}

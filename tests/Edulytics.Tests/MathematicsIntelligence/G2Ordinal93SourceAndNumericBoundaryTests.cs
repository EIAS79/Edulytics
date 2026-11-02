using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal93SourceAndNumericBoundaryTests
{
 [Fact] public void OriginalSourceFactsNeverSubstituteForHumanReviewedActivity()
 {
  var original=UsCcssG2Unit6OriginalActivities.Source(12,"5","30","40","55");
  Assert.True(original.Correct);Assert.True(original.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit6OriginalActivities.Source(12,"999","30","40","55").Correct);
  Assert.Contains("clock",UsCcssG2Unit6OriginalActivities.AuthenticTasks[12].ToLowerInvariant());
 }
 [Fact] public void FirstPartyNumericExamplesRemainSeparateFromOriginalPublisherMedia()
 {
  var independent=UsCcssG2Unit6OriginalActivities.NewMath(12,"5","30","45","10");
  Assert.True(independent.Correct);Assert.False(independent.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit6OriginalActivities.NewMath(12,"999","30","45","10").Correct);
 }
}

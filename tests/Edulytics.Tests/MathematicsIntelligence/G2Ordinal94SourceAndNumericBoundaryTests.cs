using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal94SourceAndNumericBoundaryTests
{
 [Fact] public void OriginalSourceFactsNeverSubstituteForHumanReviewedActivity()
 {
  var original=UsCcssG2Unit6OriginalActivities.Source(13,"15","30","45","5");
  Assert.True(original.Correct);Assert.True(original.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit6OriginalActivities.Source(13,"999","30","45","5").Correct);
  Assert.Contains("routine",UsCcssG2Unit6OriginalActivities.AuthenticTasks[13].ToLowerInvariant());
 }
 [Fact] public void FirstPartyNumericExamplesRemainSeparateFromOriginalPublisherMedia()
 {
  var independent=UsCcssG2Unit6OriginalActivities.NewMath(13,"15","30","45","20");
  Assert.True(independent.Correct);Assert.False(independent.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit6OriginalActivities.NewMath(13,"999","30","45","20").Correct);
 }
}

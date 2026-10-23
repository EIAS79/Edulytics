using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal98SourceAndNumericBoundaryTests
{
 [Fact] public void OriginalNumericalFactsAreNotClaimsAboutPublisherPictures()
 {
  var source=UsCcssG2Unit6OriginalActivities.Source(17,"50","50","75","100");
  Assert.True(source.Correct);Assert.True(source.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit6OriginalActivities.Source(17,"999","50","75","100").Correct);
  Assert.Contains("coin",UsCcssG2Unit6OriginalActivities.AuthenticTasks[17].ToLowerInvariant());
 }
 [Fact] public void IndependentEdulyticsNumericalFactsNeverCertifyOriginalMedia()
 {
  var independent=UsCcssG2Unit6OriginalActivities.NewMath(17,"4","10","20","25");
  Assert.True(independent.Correct);Assert.False(independent.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit6OriginalActivities.NewMath(17,"999","10","20","25").Correct);
 }
}

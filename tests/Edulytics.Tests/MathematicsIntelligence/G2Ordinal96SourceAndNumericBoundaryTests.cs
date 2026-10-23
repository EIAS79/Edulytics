using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal96SourceAndNumericBoundaryTests
{
 [Fact] public void OriginalNumericalFactsAreNotClaimsAboutPublisherPictures()
 {
  var source=UsCcssG2Unit6OriginalActivities.Source(15,"1","5","10","15");
  Assert.True(source.Correct);Assert.True(source.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit6OriginalActivities.Source(15,"999","5","10","15").Correct);
  Assert.Contains("coin",UsCcssG2Unit6OriginalActivities.AuthenticTasks[15].ToLowerInvariant());
 }
 [Fact] public void IndependentEdulyticsNumericalFactsNeverCertifyOriginalMedia()
 {
  var independent=UsCcssG2Unit6OriginalActivities.NewMath(15,"4","10","20","34");
  Assert.True(independent.Correct);Assert.False(independent.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit6OriginalActivities.NewMath(15,"999","10","20","34").Correct);
 }
}

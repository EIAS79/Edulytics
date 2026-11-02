using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal97SourceAndNumericBoundaryTests
{
 [Fact] public void OriginalNumericalFactsAreNotClaimsAboutPublisherPictures()
 {
  var source=UsCcssG2Unit6OriginalActivities.Source(16,"25","50","75","5");
  Assert.True(source.Correct);Assert.True(source.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit6OriginalActivities.Source(16,"999","50","75","5").Correct);
  Assert.Contains("coin",UsCcssG2Unit6OriginalActivities.AuthenticTasks[16].ToLowerInvariant());
 }
 [Fact] public void IndependentEdulyticsNumericalFactsNeverCertifyOriginalMedia()
 {
  var independent=UsCcssG2Unit6OriginalActivities.NewMath(16,"25","60","75","5");
  Assert.True(independent.Correct);Assert.False(independent.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit6OriginalActivities.NewMath(16,"999","60","75","5").Correct);
 }
}

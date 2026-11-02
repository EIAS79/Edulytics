using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal95SourceAndNumericBoundaryTests
{
 [Fact] public void OriginalNumericalFactsAreNotClaimsAboutPublisherPictures()
 {
  var source=UsCcssG2Unit6OriginalActivities.Source(14,"55","55","65","60");
  Assert.True(source.Correct);Assert.True(source.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit6OriginalActivities.Source(14,"999","55","65","60").Correct);
  Assert.Contains("optional",UsCcssG2Unit6OriginalActivities.AuthenticTasks[14].ToLowerInvariant());
 }
 [Fact] public void IndependentEdulyticsNumericalFactsNeverCertifyOriginalMedia()
 {
  var independent=UsCcssG2Unit6OriginalActivities.NewMath(14,"35","15","70","4");
  Assert.True(independent.Correct);Assert.False(independent.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit6OriginalActivities.NewMath(14,"999","15","70","4").Correct);
 }
}

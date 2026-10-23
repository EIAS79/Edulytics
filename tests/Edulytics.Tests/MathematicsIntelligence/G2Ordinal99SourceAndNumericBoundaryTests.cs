using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal99SourceAndNumericBoundaryTests
{
 [Fact] public void SourceNumericalFactsDoNotCertifyActualPublisherPictures()
 {
  var source=UsCcssG2Unit6OriginalActivities.Source(18,"75","35","45","18");
  Assert.True(source.Correct);Assert.True(source.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit6OriginalActivities.Source(18,"999","35","45","18").Correct);
  Assert.Contains("price",UsCcssG2Unit6OriginalActivities.AuthenticTasks[18].ToLowerInvariant());
 }
 [Fact] public void FirstPartyNumericalAnswersRemainSeparateFromOriginalSourceAcceptance()
 {
  var independent=UsCcssG2Unit6OriginalActivities.NewMath(18,"75","20","60","53");
  Assert.True(independent.Correct);Assert.False(independent.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit6OriginalActivities.NewMath(18,"999","20","60","53").Correct);
 }
}

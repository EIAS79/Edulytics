using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal101SourceAndNumericBoundaryTests
{
 [Fact] public void SourceNumericalFactsDoNotCertifyActualPublisherPictures()
 {
  var source=UsCcssG2Unit6OriginalActivities.Source(20,"55","65","71","72");
  Assert.True(source.Correct);Assert.True(source.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit6OriginalActivities.Source(20,"999","65","71","72").Correct);
  Assert.Contains("optional",UsCcssG2Unit6OriginalActivities.AuthenticTasks[20].ToLowerInvariant());
 }
 [Fact] public void FirstPartyNumericalAnswersRemainSeparateFromOriginalSourceAcceptance()
 {
  var independent=UsCcssG2Unit6OriginalActivities.NewMath(20,"55","75","80","25");
  Assert.True(independent.Correct);Assert.False(independent.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit6OriginalActivities.NewMath(20,"999","75","80","25").Correct);
 }
}

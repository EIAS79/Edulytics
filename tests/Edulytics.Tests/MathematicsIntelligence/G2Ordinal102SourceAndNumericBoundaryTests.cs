using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal102SourceAndNumericBoundaryTests
{
 [Fact] public void SourceNumericalFactsDoNotCertifyActualPublisherPictures()
 {
  var source=UsCcssG2Unit6OriginalActivities.Source(21,"4","1","2","2");
  Assert.True(source.Correct);Assert.True(source.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit6OriginalActivities.Source(21,"999","1","2","2").Correct);
  Assert.Contains("block",UsCcssG2Unit6OriginalActivities.AuthenticTasks[21].ToLowerInvariant());
 }
 [Fact] public void FirstPartyNumericalAnswersRemainSeparateFromOriginalSourceAcceptance()
 {
  var independent=UsCcssG2Unit6OriginalActivities.NewMath(21,"4","1","19","6");
  Assert.True(independent.Correct);Assert.False(independent.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit6OriginalActivities.NewMath(21,"999","1","19","6").Correct);
 }
}

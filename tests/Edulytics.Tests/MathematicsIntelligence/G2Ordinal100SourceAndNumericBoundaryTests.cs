using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal100SourceAndNumericBoundaryTests
{
 [Fact] public void SourceNumericalFactsDoNotCertifyActualPublisherPictures()
 {
  var source=UsCcssG2Unit6OriginalActivities.Source(19,"50","60","62","73");
  Assert.True(source.Correct);Assert.True(source.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit6OriginalActivities.Source(19,"999","60","62","73").Correct);
  Assert.Contains("tape",UsCcssG2Unit6OriginalActivities.AuthenticTasks[19].ToLowerInvariant());
 }
 [Fact] public void FirstPartyNumericalAnswersRemainSeparateFromOriginalSourceAcceptance()
 {
  var independent=UsCcssG2Unit6OriginalActivities.NewMath(19,"29","75","70","15");
  Assert.True(independent.Correct);Assert.False(independent.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit6OriginalActivities.NewMath(19,"999","75","70","15").Correct);
 }
}

using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal108SourceAndNumericBoundaryTests
{
 [Fact] public void ActualSourceNumericMatchesWithoutApprovingOriginalMedia()
 {
  var result=UsCcssG2Unit7OriginalActivities.Source(6,"30","40","70","90");
  Assert.True(result.Correct);Assert.True(result.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit7OriginalActivities.Source(6,"999","40","70","90").Correct);
  Assert.Equal(4,UsCcssG2Unit7OriginalActivities.NextSourceNumericExamples[6].Length);
  Assert.NotEmpty(UsCcssG2Unit7OriginalActivities.AuthenticTasks[6]);
 }
 [Fact] public void IndependentEdulyticsNumericAnswersAreNotOriginalAnswers()
 {
  var result=UsCcssG2Unit7OriginalActivities.NewMath(6,"130","280","520","7");
  Assert.True(result.Correct);Assert.False(result.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit7OriginalActivities.NewMath(6,"999","280","520","7").Correct);
 }
}
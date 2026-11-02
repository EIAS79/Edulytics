using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal109SourceAndNumericBoundaryTests
{
 [Fact] public void ActualSourceNumericMatchesWithoutApprovingOriginalMedia()
 {
  var result=UsCcssG2Unit7OriginalActivities.Source(7,"353","392","446","470");
  Assert.True(result.Correct);Assert.True(result.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit7OriginalActivities.Source(7,"999","392","446","470").Correct);
  Assert.Equal(4,UsCcssG2Unit7OriginalActivities.NextSourceNumericExamples[7].Length);
  Assert.NotEmpty(UsCcssG2Unit7OriginalActivities.AuthenticTasks[6]);
 }
 [Fact] public void IndependentEdulyticsNumericAnswersAreNotOriginalAnswers()
 {
  var result=UsCcssG2Unit7OriginalActivities.NewMath(7,"365","450","1","1");
  Assert.True(result.Correct);Assert.False(result.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit7OriginalActivities.NewMath(7,"999","450","1","1").Correct);
 }
}
using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal107SourceAndNumericBoundaryTests
{
 [Fact] public void ActualSourceNumericMatchesWithoutApprovingOriginalMedia()
 {
  var result=UsCcssG2Unit7OriginalActivities.Source(5,"254","474","676","5");
  Assert.True(result.Correct);Assert.True(result.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit7OriginalActivities.Source(5,"999","474","676","5").Correct);
  Assert.Equal(4,UsCcssG2Unit7OriginalActivities.NextSourceNumericExamples[5].Length);
  Assert.NotEmpty(UsCcssG2Unit7OriginalActivities.AuthenticTasks[5]);
 }
 [Fact] public void IndependentEdulyticsNumericAnswersAreNotOriginalAnswers()
 {
  var result=UsCcssG2Unit7OriginalActivities.NewMath(5,"468","35","604","12");
  Assert.True(result.Correct);Assert.False(result.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit7OriginalActivities.NewMath(5,"999","35","604","12").Correct);
 }
}
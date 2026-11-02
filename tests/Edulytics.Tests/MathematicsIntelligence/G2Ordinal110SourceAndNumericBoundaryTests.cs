using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal110SourceAndNumericBoundaryTests
{
 [Fact] public void ActualSourceNumericMatchesWithoutApprovingOriginalMedia()
 {
  var result=UsCcssG2Unit7OriginalActivities.Source(8,"291","354","361","7");
  Assert.True(result.Correct);Assert.True(result.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit7OriginalActivities.Source(8,"999","354","361","7").Correct);
  Assert.Equal(4,UsCcssG2Unit7OriginalActivities.NextSourceNumericExamples[8].Length);
  Assert.NotEmpty(UsCcssG2Unit7OriginalActivities.AuthenticTasks[6]);
 }
 [Fact] public void IndependentEdulyticsNumericAnswersAreNotOriginalAnswers()
 {
  var result=UsCcssG2Unit7OriginalActivities.NewMath(8,"310","450","560","110");
  Assert.True(result.Correct);Assert.False(result.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit7OriginalActivities.NewMath(8,"999","450","560","110").Correct);
 }
}
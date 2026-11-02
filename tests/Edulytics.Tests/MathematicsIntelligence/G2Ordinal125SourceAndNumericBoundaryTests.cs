using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal125SourceAndNumericBoundaryTests {
 [Fact] public void OriginalPublisherImagesOrClassroomGamesNeverGetNumericCertification(){
  var check=UsCcssG2Unit8OriginalActivities.Source(5,"0","0","0","0");
  Assert.Null(check.Correct);Assert.True(check.TeacherReviewNeeded);
  Assert.NotEmpty(UsCcssG2Unit8OriginalActivities.AuthenticTasks[5]);
 }
 [Fact] public void NewEdulyticsNumbersAreCheckedSeparately(){
  var good=UsCcssG2Unit8OriginalActivities.NewMath(5,"1","0","12","13");
  Assert.True(good.Correct);Assert.False(good.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit8OriginalActivities.NewMath(5,"999","0","12","13").Correct);
  Assert.Equal(4,UsCcssG2Unit8OriginalActivities.IndependentNumericExamples[5].Length);
 }
}
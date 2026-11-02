using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal129SourceAndNumericBoundaryTests {
 [Fact] public void OriginalPublisherImagesOrClassroomGamesNeverGetNumericCertification(){
  var check=UsCcssG2Unit8OriginalActivities.Source(9,"0","0","0","0");
  Assert.Null(check.Correct);Assert.True(check.TeacherReviewNeeded);
  Assert.NotEmpty(UsCcssG2Unit8OriginalActivities.AuthenticTasks[9]);
 }
 [Fact] public void NewEdulyticsNumbersAreCheckedSeparately(){
  var good=UsCcssG2Unit8OriginalActivities.NewMath(9,"20","12","3","4");
  Assert.True(good.Correct);Assert.False(good.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit8OriginalActivities.NewMath(9,"999","12","3","4").Correct);
  Assert.Equal(4,UsCcssG2Unit8OriginalActivities.IndependentNumericExamples[9].Length);
 }
}
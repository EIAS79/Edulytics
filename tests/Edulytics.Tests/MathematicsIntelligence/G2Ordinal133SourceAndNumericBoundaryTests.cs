using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal133SourceAndNumericBoundaryTests {
 [Fact] public void OriginalPublisherImagesOrClassroomGamesNeverGetNumericCertification(){
  var check=UsCcssG2Unit8OriginalActivities.Source(13,"0","0","0","0");
  Assert.Null(check.Correct);Assert.True(check.TeacherReviewNeeded);
  Assert.NotEmpty(UsCcssG2Unit8OriginalActivities.AuthenticTasks[13]);
 }
 [Fact] public void NewEdulyticsNumbersAreCheckedSeparately(){
  var good=UsCcssG2Unit8OriginalActivities.NewMath(13,"8","6","2","14");
  Assert.True(good.Correct);Assert.False(good.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit8OriginalActivities.NewMath(13,"999","6","2","14").Correct);
  Assert.Equal(4,UsCcssG2Unit8OriginalActivities.IndependentNumericExamples[13].Length);
 }
}
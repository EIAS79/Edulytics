using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal130SourceAndNumericBoundaryTests {
 [Fact] public void OriginalPublisherImagesOrClassroomGamesNeverGetNumericCertification(){
  var check=UsCcssG2Unit8OriginalActivities.Source(10,"0","0","0","0");
  Assert.Null(check.Correct);Assert.True(check.TeacherReviewNeeded);
  Assert.NotEmpty(UsCcssG2Unit8OriginalActivities.AuthenticTasks[10]);
 }
 [Fact] public void NewEdulyticsNumbersAreCheckedSeparately(){
  var good=UsCcssG2Unit8OriginalActivities.NewMath(10,"15","15","5","3");
  Assert.True(good.Correct);Assert.False(good.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit8OriginalActivities.NewMath(10,"999","15","5","3").Correct);
  Assert.Equal(4,UsCcssG2Unit8OriginalActivities.IndependentNumericExamples[10].Length);
 }
}
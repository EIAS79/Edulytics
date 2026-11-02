using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal132SourceAndNumericBoundaryTests {
 [Fact] public void OriginalPublisherImagesOrClassroomGamesNeverGetNumericCertification(){
  var check=UsCcssG2Unit8OriginalActivities.Source(12,"0","0","0","0");
  Assert.Null(check.Correct);Assert.True(check.TeacherReviewNeeded);
  Assert.NotEmpty(UsCcssG2Unit8OriginalActivities.AuthenticTasks[12]);
 }
 [Fact] public void NewEdulyticsNumbersAreCheckedSeparately(){
  var good=UsCcssG2Unit8OriginalActivities.NewMath(12,"12","2","3","12");
  Assert.True(good.Correct);Assert.False(good.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit8OriginalActivities.NewMath(12,"999","2","3","12").Correct);
  Assert.Equal(4,UsCcssG2Unit8OriginalActivities.IndependentNumericExamples[12].Length);
 }
}
using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal134SourceAndNumericBoundaryTests {
 [Fact] public void GenuinePublisherSourceMediaAndOpenTasksRequireTeacher(){
  var original=UsCcssG2Unit9OriginalActivities.Source(1,"0","0","0","0");
  Assert.Null(original.Correct);Assert.True(original.TeacherReviewNeeded);
  Assert.NotEmpty(UsCcssG2Unit9OriginalActivities.AuthenticTasks[1]);
 }
 [Fact] public void FirstPartyNumericAnswersNeverCertifyOriginalSource(){
  var own=UsCcssG2Unit9OriginalActivities.NewMath(1,"6","15","7","15");
  Assert.True(own.Correct);Assert.False(own.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit9OriginalActivities.NewMath(1,"999","15","7","15").Correct);
  Assert.Equal(4,UsCcssG2Unit9OriginalActivities.IndependentNumericExamples[1].Length);
 }
}
using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal137SourceAndNumericBoundaryTests {
 [Fact] public void GenuinePublisherSourceMediaAndOpenTasksRequireTeacher(){
  var original=UsCcssG2Unit9OriginalActivities.Source(4,"0","0","0","0");
  Assert.Null(original.Correct);Assert.True(original.TeacherReviewNeeded);
  Assert.NotEmpty(UsCcssG2Unit9OriginalActivities.AuthenticTasks[4]);
 }
 [Fact] public void FirstPartyNumericAnswersNeverCertifyOriginalSource(){
  var own=UsCcssG2Unit9OriginalActivities.NewMath(4,"5","12","3","53");
  Assert.True(own.Correct);Assert.False(own.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit9OriginalActivities.NewMath(4,"999","12","3","53").Correct);
  Assert.Equal(4,UsCcssG2Unit9OriginalActivities.IndependentNumericExamples[4].Length);
 }
}
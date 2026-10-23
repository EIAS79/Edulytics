using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal136SourceAndNumericBoundaryTests {
 [Fact] public void GenuinePublisherSourceMediaAndOpenTasksRequireTeacher(){
  var original=UsCcssG2Unit9OriginalActivities.Source(3,"0","0","0","0");
  Assert.Null(original.Correct);Assert.True(original.TeacherReviewNeeded);
  Assert.NotEmpty(UsCcssG2Unit9OriginalActivities.AuthenticTasks[3]);
 }
 [Fact] public void FirstPartyNumericAnswersNeverCertifyOriginalSource(){
  var own=UsCcssG2Unit9OriginalActivities.NewMath(3,"9","4","12","7");
  Assert.True(own.Correct);Assert.False(own.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit9OriginalActivities.NewMath(3,"999","4","12","7").Correct);
  Assert.Equal(4,UsCcssG2Unit9OriginalActivities.IndependentNumericExamples[3].Length);
 }
}
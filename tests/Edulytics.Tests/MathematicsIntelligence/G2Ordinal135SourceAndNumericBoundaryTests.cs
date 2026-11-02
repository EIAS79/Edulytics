using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal135SourceAndNumericBoundaryTests {
 [Fact] public void GenuinePublisherSourceMediaAndOpenTasksRequireTeacher(){
  var original=UsCcssG2Unit9OriginalActivities.Source(2,"0","0","0","0");
  Assert.Null(original.Correct);Assert.True(original.TeacherReviewNeeded);
  Assert.NotEmpty(UsCcssG2Unit9OriginalActivities.AuthenticTasks[2]);
 }
 [Fact] public void FirstPartyNumericAnswersNeverCertifyOriginalSource(){
  var own=UsCcssG2Unit9OriginalActivities.NewMath(2,"13","7","8","9");
  Assert.True(own.Correct);Assert.False(own.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit9OriginalActivities.NewMath(2,"999","7","8","9").Correct);
  Assert.Equal(4,UsCcssG2Unit9OriginalActivities.IndependentNumericExamples[2].Length);
 }
}
using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal145SourceAndNumericBoundaryTests {
 [Fact] public void OriginalPupilAuthoredQuestionsAndDiagramsRequireTeacherReview(){
  var s=UsCcssG2Unit9OriginalActivities.Source(12,"0","0","0","0");
  Assert.Null(s.Correct);Assert.True(s.TeacherReviewNeeded);
  Assert.NotEmpty(UsCcssG2Unit9OriginalActivities.AuthenticTasks[12]);
 }
 [Fact] public void IndependentEdulyticsNumericExercisesAreCheckedSeparately(){
  var own=UsCcssG2Unit9OriginalActivities.NewMath(12,"40","23","25","19");
  Assert.True(own.Correct);Assert.False(own.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit9OriginalActivities.NewMath(12,"999","23","25","19").Correct);
  Assert.Equal(4,UsCcssG2Unit9OriginalActivities.IndependentNumericExamples[12].Length);
 }
}
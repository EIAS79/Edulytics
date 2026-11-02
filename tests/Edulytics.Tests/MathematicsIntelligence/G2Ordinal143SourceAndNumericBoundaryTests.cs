using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal143SourceAndNumericBoundaryTests {
 [Fact] public void OriginalPupilAuthoredQuestionsAndDiagramsRequireTeacherReview(){
  var s=UsCcssG2Unit9OriginalActivities.Source(10,"0","0","0","0");
  Assert.Null(s.Correct);Assert.True(s.TeacherReviewNeeded);
  Assert.NotEmpty(UsCcssG2Unit9OriginalActivities.AuthenticTasks[10]);
 }
 [Fact] public void IndependentEdulyticsNumericExercisesAreCheckedSeparately(){
  var own=UsCcssG2Unit9OriginalActivities.NewMath(10,"34","22","11","19");
  Assert.True(own.Correct);Assert.False(own.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit9OriginalActivities.NewMath(10,"999","22","11","19").Correct);
  Assert.Equal(4,UsCcssG2Unit9OriginalActivities.IndependentNumericExamples[10].Length);
 }
}
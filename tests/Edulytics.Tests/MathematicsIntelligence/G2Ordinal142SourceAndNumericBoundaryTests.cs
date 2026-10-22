using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal142SourceAndNumericBoundaryTests {
 [Fact] public void OriginalPupilAuthoredQuestionsAndDiagramsRequireTeacherReview(){
  var s=UsCcssG2Unit9OriginalActivities.Source(9,"0","0","0","0");
  Assert.Null(s.Correct);Assert.True(s.TeacherReviewNeeded);
  Assert.NotEmpty(UsCcssG2Unit9OriginalActivities.AuthenticTasks[9]);
 }
 [Fact] public void IndependentEdulyticsNumericExercisesAreCheckedSeparately(){
  var own=UsCcssG2Unit9OriginalActivities.NewMath(9,"57","26","80","28");
  Assert.True(own.Correct);Assert.False(own.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit9OriginalActivities.NewMath(9,"999","26","80","28").Correct);
  Assert.Equal(4,UsCcssG2Unit9OriginalActivities.IndependentNumericExamples[9].Length);
 }
}
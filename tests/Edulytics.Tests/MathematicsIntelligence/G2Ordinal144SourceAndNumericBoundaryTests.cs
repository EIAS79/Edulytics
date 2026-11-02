using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal144SourceAndNumericBoundaryTests {
 [Fact] public void OriginalPupilAuthoredQuestionsAndDiagramsRequireTeacherReview(){
  var s=UsCcssG2Unit9OriginalActivities.Source(11,"0","0","0","0");
  Assert.Null(s.Correct);Assert.True(s.TeacherReviewNeeded);
  Assert.NotEmpty(UsCcssG2Unit9OriginalActivities.AuthenticTasks[11]);
 }
 [Fact] public void IndependentEdulyticsNumericExercisesAreCheckedSeparately(){
  var own=UsCcssG2Unit9OriginalActivities.NewMath(11,"60","36","54","72");
  Assert.True(own.Correct);Assert.False(own.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit9OriginalActivities.NewMath(11,"999","36","54","72").Correct);
  Assert.Equal(4,UsCcssG2Unit9OriginalActivities.IndependentNumericExamples[11].Length);
 }
}
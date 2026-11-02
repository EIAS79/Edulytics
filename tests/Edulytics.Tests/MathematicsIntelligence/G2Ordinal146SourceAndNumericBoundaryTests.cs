using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal146SourceAndNumericBoundaryTests{
 [Fact] public void OriginalStudentStoryAndGalleryPosterNeedHumanEvaluation(){
  var original=UsCcssG2Unit9OriginalActivities.Source(13,"0","0","0","0");
  Assert.Null(original.Correct);Assert.True(original.TeacherReviewNeeded);
  Assert.Contains("poster",UsCcssG2Unit9OriginalActivities.AuthenticTasks[13],StringComparison.OrdinalIgnoreCase);
 }
 [Fact] public void FirstPartyPosterNumericExamplesAreIndependent(){
  var answer=UsCcssG2Unit9OriginalActivities.NewMath(13,"39","49","59","89");
  Assert.True(answer.Correct);Assert.False(answer.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit9OriginalActivities.NewMath(13,"999","49","59","89").Correct);
  Assert.Equal(4,UsCcssG2Unit9OriginalActivities.IndependentNumericExamples[13].Length);
 }
}
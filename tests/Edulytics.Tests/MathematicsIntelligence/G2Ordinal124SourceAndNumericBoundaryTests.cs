using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal124SourceAndNumericBoundaryTests{
 [Fact] public void OriginalPublisherPhysicalAnswersAreNotAutomaticallyCertified(){
  var result=UsCcssG2Unit8OriginalActivities.Source(4,"0","0","0","0");
  Assert.Null(result.Correct);Assert.True(result.TeacherReviewNeeded);
  Assert.NotEmpty(UsCcssG2Unit8OriginalActivities.AuthenticTasks[4]);
 }
 [Fact] public void IndependentNumericalExamplesCheckRightAndWrongValues(){
  var q=UsCcssG2Unit8OriginalActivities.NewMath(4,"8","9","4","1");
  Assert.True(q.Correct);Assert.False(q.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit8OriginalActivities.NewMath(4,"999","9","4","1").Correct);
  Assert.Equal(4,UsCcssG2Unit8OriginalActivities.IndependentNumericExamples[4].Length);
 }
}
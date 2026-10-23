using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal122SourceAndNumericBoundaryTests{
 [Fact] public void OriginalPublisherPhysicalAnswersAreNotAutomaticallyCertified(){
  var result=UsCcssG2Unit8OriginalActivities.Source(2,"0","0","0","0");
  Assert.Null(result.Correct);Assert.True(result.TeacherReviewNeeded);
  Assert.NotEmpty(UsCcssG2Unit8OriginalActivities.AuthenticTasks[2]);
 }
 [Fact] public void IndependentNumericalExamplesCheckRightAndWrongValues(){
  var q=UsCcssG2Unit8OriginalActivities.NewMath(2,"9","10","1","7");
  Assert.True(q.Correct);Assert.False(q.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit8OriginalActivities.NewMath(2,"999","10","1","7").Correct);
  Assert.Equal(4,UsCcssG2Unit8OriginalActivities.IndependentNumericExamples[2].Length);
 }
}
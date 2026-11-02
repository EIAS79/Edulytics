using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal121SourceAndNumericBoundaryTests{
 [Fact] public void OriginalPublisherPhysicalAnswersAreNotAutomaticallyCertified(){
  var result=UsCcssG2Unit8OriginalActivities.Source(1,"0","0","0","0");
  Assert.Null(result.Correct);Assert.True(result.TeacherReviewNeeded);
  Assert.NotEmpty(UsCcssG2Unit8OriginalActivities.AuthenticTasks[1]);
 }
 [Fact] public void IndependentNumericalExamplesCheckRightAndWrongValues(){
  var q=UsCcssG2Unit8OriginalActivities.NewMath(1,"5","1","4","0");
  Assert.True(q.Correct);Assert.False(q.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit8OriginalActivities.NewMath(1,"999","1","4","0").Correct);
  Assert.Equal(4,UsCcssG2Unit8OriginalActivities.IndependentNumericExamples[1].Length);
 }
}
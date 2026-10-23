using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal140SourceAndNumericBoundaryTests {
 [Fact] public void AuthenticSourceCardsAndRepresentationsRequireEducator(){
  var s=UsCcssG2Unit9OriginalActivities.Source(7,"0","0","0","0");
  Assert.Null(s.Correct);Assert.True(s.TeacherReviewNeeded);
  Assert.NotEmpty(UsCcssG2Unit9OriginalActivities.AuthenticTasks[7]);
 }
 [Fact] public void FirstPartyMathIsIndependentAndRejectsWrongAnswer(){
  var valid=UsCcssG2Unit9OriginalActivities.NewMath(7,"443","261","104","964");
  Assert.True(valid.Correct);Assert.False(valid.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit9OriginalActivities.NewMath(7,"999","261","104","964").Correct);
  Assert.Equal(4,UsCcssG2Unit9OriginalActivities.IndependentNumericExamples[7].Length);
 }
}
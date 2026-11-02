using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal138SourceAndNumericBoundaryTests {
 [Fact] public void AuthenticSourceCardsAndRepresentationsRequireEducator(){
  var s=UsCcssG2Unit9OriginalActivities.Source(5,"0","0","0","0");
  Assert.Null(s.Correct);Assert.True(s.TeacherReviewNeeded);
  Assert.NotEmpty(UsCcssG2Unit9OriginalActivities.AuthenticTasks[5]);
 }
 [Fact] public void FirstPartyMathIsIndependentAndRejectsWrongAnswer(){
  var valid=UsCcssG2Unit9OriginalActivities.NewMath(5,"356","356","356","308");
  Assert.True(valid.Correct);Assert.False(valid.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit9OriginalActivities.NewMath(5,"999","356","356","308").Correct);
  Assert.Equal(4,UsCcssG2Unit9OriginalActivities.IndependentNumericExamples[5].Length);
 }
}
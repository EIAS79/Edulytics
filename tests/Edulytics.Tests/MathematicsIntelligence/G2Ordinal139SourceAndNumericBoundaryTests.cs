using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal139SourceAndNumericBoundaryTests {
 [Fact] public void AuthenticSourceCardsAndRepresentationsRequireEducator(){
  var s=UsCcssG2Unit9OriginalActivities.Source(6,"0","0","0","0");
  Assert.Null(s.Correct);Assert.True(s.TeacherReviewNeeded);
  Assert.NotEmpty(UsCcssG2Unit9OriginalActivities.AuthenticTasks[6]);
 }
 [Fact] public void FirstPartyMathIsIndependentAndRejectsWrongAnswer(){
  var valid=UsCcssG2Unit9OriginalActivities.NewMath(6,"527","527","527","527");
  Assert.True(valid.Correct);Assert.False(valid.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit9OriginalActivities.NewMath(6,"999","527","527","527").Correct);
  Assert.Equal(4,UsCcssG2Unit9OriginalActivities.IndependentNumericExamples[6].Length);
 }
}
using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal141SourceAndNumericBoundaryTests {
 [Fact] public void AuthenticSourceCardsAndRepresentationsRequireEducator(){
  var s=UsCcssG2Unit9OriginalActivities.Source(8,"0","0","0","0");
  Assert.Null(s.Correct);Assert.True(s.TeacherReviewNeeded);
  Assert.NotEmpty(UsCcssG2Unit9OriginalActivities.AuthenticTasks[8]);
 }
 [Fact] public void FirstPartyMathIsIndependentAndRejectsWrongAnswer(){
  var valid=UsCcssG2Unit9OriginalActivities.NewMath(8,"64","36","14","36");
  Assert.True(valid.Correct);Assert.False(valid.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit9OriginalActivities.NewMath(8,"999","36","14","36").Correct);
  Assert.Equal(4,UsCcssG2Unit9OriginalActivities.IndependentNumericExamples[8].Length);
 }
}
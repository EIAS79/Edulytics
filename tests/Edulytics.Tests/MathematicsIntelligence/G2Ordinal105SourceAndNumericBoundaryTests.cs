using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal105SourceAndNumericBoundaryTests
{
 [Fact] public void SourceMathRequiresHumanEvidence()
 {
  var actual=UsCcssG2Unit7OriginalActivities.Source(3,"351","116","601","460");
  Assert.True(actual.Correct);Assert.True(actual.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit7OriginalActivities.Source(3,"999","116","601","460").Correct);
  Assert.Equal(4,UsCcssG2Unit7OriginalActivities.SourceNumericExamples[3].Length);
 }
 [Fact] public void IndependentAnswersAreSeparateAndDoNotAcceptBadKeys()
 {
  var firstParty=UsCcssG2Unit7OriginalActivities.NewMath(3,"101","130","340","70");
  Assert.True(firstParty.Correct);Assert.False(firstParty.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit7OriginalActivities.NewMath(3,"999","130","340","70").Correct);
  Assert.Equal(4,UsCcssG2Unit7OriginalActivities.IndependentNumericExamples[3].Length);
 }
}
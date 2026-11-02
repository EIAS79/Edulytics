using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal76SourceMathematicsTests
{
 [Fact] public void PublisherMathChecksRequireActualTeacherPictureReview()
 {
  var row=UsCcssG2Unit5ComparisonOriginal.Original(9,"371","317","483","443");
  Assert.True(row.Correct);Assert.True(row.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit5ComparisonOriginal.Original(9,"999","317","483","443").Correct);
  Assert.NotEmpty(UsCcssG2Unit5ComparisonOriginal.SourceHumanRubrics[9]);
 }
 [Fact] public void FictionalFirstPartyValuesAreDistinctFromRealSourceEvidence()
 {
  var row=UsCcssG2Unit5ComparisonOriginal.FirstParty(9,"483","443","40","1");
  Assert.True(row.Correct);Assert.False(row.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit5ComparisonOriginal.FirstParty(9,"999","443","40","1").Correct);
 }
}

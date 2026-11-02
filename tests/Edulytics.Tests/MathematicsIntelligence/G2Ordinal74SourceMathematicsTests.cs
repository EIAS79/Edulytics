using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal74SourceMathematicsTests
{
 [Fact] public void PublisherMathChecksRequireActualTeacherPictureReview()
 {
  var row=UsCcssG2Unit5ComparisonOriginal.Original(7,"1","0","1","1");
  Assert.True(row.Correct);Assert.True(row.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit5ComparisonOriginal.Original(7,"999","0","1","1").Correct);
  Assert.NotEmpty(UsCcssG2Unit5ComparisonOriginal.SourceHumanRubrics[7]);
 }
 [Fact] public void FictionalFirstPartyValuesAreDistinctFromRealSourceEvidence()
 {
  var row=UsCcssG2Unit5ComparisonOriginal.FirstParty(7,"100","101","0","1");
  Assert.True(row.Correct);Assert.False(row.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit5ComparisonOriginal.FirstParty(7,"999","101","0","1").Correct);
 }
}

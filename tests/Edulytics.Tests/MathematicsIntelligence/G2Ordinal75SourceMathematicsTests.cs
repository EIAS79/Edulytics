using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal75SourceMathematicsTests
{
 [Fact] public void PublisherMathChecksRequireActualTeacherPictureReview()
 {
  var row=UsCcssG2Unit5ComparisonOriginal.Original(8,"3","30","300","700");
  Assert.True(row.Correct);Assert.True(row.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit5ComparisonOriginal.Original(8,"999","30","300","700").Correct);
  Assert.NotEmpty(UsCcssG2Unit5ComparisonOriginal.SourceHumanRubrics[8]);
 }
 [Fact] public void FictionalFirstPartyValuesAreDistinctFromRealSourceEvidence()
 {
  var row=UsCcssG2Unit5ComparisonOriginal.FirstParty(8,"234","450","678","901");
  Assert.True(row.Correct);Assert.False(row.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit5ComparisonOriginal.FirstParty(8,"999","450","678","901").Correct);
 }
}

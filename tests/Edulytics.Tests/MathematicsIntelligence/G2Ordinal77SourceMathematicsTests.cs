using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal77SourceMathematicsTests
{
 [Fact] public void PublisherMathChecksRequireActualTeacherPictureReview()
 {
  var row=UsCcssG2Unit5ComparisonOriginal.Original(10,"76","76","99","99");
  Assert.True(row.Correct);Assert.True(row.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit5ComparisonOriginal.Original(10,"999","76","99","99").Correct);
  Assert.NotEmpty(UsCcssG2Unit5ComparisonOriginal.SourceHumanRubrics[10]);
 }
 [Fact] public void FictionalFirstPartyValuesAreDistinctFromRealSourceEvidence()
 {
  var row=UsCcssG2Unit5ComparisonOriginal.FirstParty(10,"142","274","330","300");
  Assert.True(row.Correct);Assert.False(row.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit5ComparisonOriginal.FirstParty(10,"999","274","330","300").Correct);
 }
}

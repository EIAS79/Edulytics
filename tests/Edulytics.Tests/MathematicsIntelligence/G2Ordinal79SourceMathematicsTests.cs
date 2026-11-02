using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal79SourceMathematicsTests
{
 [Fact] public void ExactPublisherQuantitiesStillNeedHumanReviewForRealActivities()
 {
  var answer=UsCcssG2Unit5ComparisonOriginal.Original(12,"30","37","36","36");
  Assert.True(answer.Correct);Assert.True(answer.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit5ComparisonOriginal.Original(12,"999","37","36","36").Correct);
  Assert.NotEmpty(UsCcssG2Unit5ComparisonOriginal.SourceHumanRubrics[12]);
 }
 [Fact] public void FirstPartyPracticeNeverCertifiesPublisherImagesOrRealCards()
 {
  var answer=UsCcssG2Unit5ComparisonOriginal.FirstParty(12,"638","681","672","644");
  Assert.True(answer.Correct);Assert.False(answer.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit5ComparisonOriginal.FirstParty(12,"999","681","672","644").Correct);
 }
}

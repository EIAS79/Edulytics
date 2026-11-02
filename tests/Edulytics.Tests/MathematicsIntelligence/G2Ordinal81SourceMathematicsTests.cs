using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal81SourceMathematicsTests
{
 [Fact] public void ExactPublisherQuantitiesStillNeedHumanReviewForRealActivities()
 {
  var answer=UsCcssG2Unit5ComparisonOriginal.Original(14,"529","529","529","529");
  Assert.True(answer.Correct);Assert.True(answer.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit5ComparisonOriginal.Original(14,"999","529","529","529").Correct);
  Assert.NotEmpty(UsCcssG2Unit5ComparisonOriginal.SourceHumanRubrics[14]);
 }
 [Fact] public void FirstPartyPracticeNeverCertifiesPublisherImagesOrRealCards()
 {
  var answer=UsCcssG2Unit5ComparisonOriginal.FirstParty(14,"320","452","671","805");
  Assert.True(answer.Correct);Assert.False(answer.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit5ComparisonOriginal.FirstParty(14,"999","452","671","805").Correct);
 }
}

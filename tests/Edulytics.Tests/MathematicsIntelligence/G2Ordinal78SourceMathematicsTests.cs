using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal78SourceMathematicsTests
{
 [Fact] public void ExactPublisherQuantitiesStillNeedHumanReviewForRealActivities()
 {
  var answer=UsCcssG2Unit5ComparisonOriginal.Original(11,"1","1","0","1");
  Assert.True(answer.Correct);Assert.True(answer.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit5ComparisonOriginal.Original(11,"999","1","0","1").Correct);
  Assert.NotEmpty(UsCcssG2Unit5ComparisonOriginal.SourceHumanRubrics[11]);
 }
 [Fact] public void FirstPartyPracticeNeverCertifiesPublisherImagesOrRealCards()
 {
  var answer=UsCcssG2Unit5ComparisonOriginal.FirstParty(11,"413","415","732","721");
  Assert.True(answer.Correct);Assert.False(answer.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit5ComparisonOriginal.FirstParty(11,"999","415","732","721").Correct);
 }
}

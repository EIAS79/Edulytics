using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal80SourceMathematicsTests
{
 [Fact] public void ExactPublisherQuantitiesStillNeedHumanReviewForRealActivities()
 {
  var answer=UsCcssG2Unit5ComparisonOriginal.Original(13,"604","406","64","604");
  Assert.True(answer.Correct);Assert.True(answer.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit5ComparisonOriginal.Original(13,"999","406","64","604").Correct);
  Assert.NotEmpty(UsCcssG2Unit5ComparisonOriginal.SourceHumanRubrics[13]);
 }
 [Fact] public void FirstPartyPracticeNeverCertifiesPublisherImagesOrRealCards()
 {
  var answer=UsCcssG2Unit5ComparisonOriginal.FirstParty(13,"701","502","641","400");
  Assert.True(answer.Correct);Assert.False(answer.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit5ComparisonOriginal.FirstParty(13,"999","502","641","400").Correct);
 }
}

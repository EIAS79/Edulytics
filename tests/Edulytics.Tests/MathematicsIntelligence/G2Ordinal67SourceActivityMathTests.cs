using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal67SourceActivityMathTests
{
 [Fact] public void OriginalCalculationsRequireEducatorReview()
 {
  var r=UsCcssG2Unit4FinalSixSourcePractice.Source(15,"12","39","15","68");
  Assert.True(r.Correct);Assert.True(r.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit4FinalSixSourcePractice.Source(15,"999","39","15","68").Correct);
  Assert.NotEmpty(UsCcssG2Unit4FinalSixSourcePractice.TeacherRubrics[15]);
 }
 [Fact] public void SeparatePracticeNeverCertifiesPublisherArtwork()
 {
  var r=UsCcssG2Unit4FinalSixSourcePractice.FirstParty(15,"15","36","23","40");
  Assert.True(r.Correct);Assert.False(r.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit4FinalSixSourcePractice.FirstParty(15,"999","36","23","40").Correct);
 }
}

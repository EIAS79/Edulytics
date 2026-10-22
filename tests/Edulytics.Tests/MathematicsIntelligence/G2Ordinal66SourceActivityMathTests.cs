using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal66SourceActivityMathTests
{
 [Fact] public void OriginalCalculationsRequireEducatorReview()
 {
  var r=UsCcssG2Unit4FinalSixSourcePractice.Source(14,"50","49","60","2");
  Assert.True(r.Correct);Assert.True(r.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit4FinalSixSourcePractice.Source(14,"999","49","60","2").Correct);
  Assert.NotEmpty(UsCcssG2Unit4FinalSixSourcePractice.TeacherRubrics[14]);
 }
 [Fact] public void SeparatePracticeNeverCertifiesPublisherArtwork()
 {
  var r=UsCcssG2Unit4FinalSixSourcePractice.FirstParty(14,"50","1","0","1");
  Assert.True(r.Correct);Assert.False(r.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit4FinalSixSourcePractice.FirstParty(14,"999","1","0","1").Correct);
 }
}

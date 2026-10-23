using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal69MathAndTeacherBoundaryTests
{
 [Fact]
 public void OriginalSourceNumericTasksStillNeedRealClassroomReview()
 {
  var source=UsCcssG2Unit5FirstSixOriginal.Source(2,"9","11","15","30");
  Assert.True(source.Correct);Assert.True(source.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit5FirstSixOriginal.Source(2,"999","11","15","30").Correct);
  Assert.NotEmpty(UsCcssG2Unit5FirstSixOriginal.TeacherRubrics[2]);
 }
 [Fact]
 public void NewEdulyticsAnswersMustNotCertifySourcePictures()
 {
  var own=UsCcssG2Unit5FirstSixOriginal.FirstParty(2,"30","7","300","29");
  Assert.True(own.Correct);Assert.False(own.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit5FirstSixOriginal.FirstParty(2,"999","7","300","29").Correct);
 }
}

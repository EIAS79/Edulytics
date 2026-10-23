using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal68MathAndTeacherBoundaryTests
{
 [Fact]
 public void OriginalSourceNumericTasksStillNeedRealClassroomReview()
 {
  var source=UsCcssG2Unit5FirstSixOriginal.Source(1,"96","100","100","111");
  Assert.True(source.Correct);Assert.True(source.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit5FirstSixOriginal.Source(1,"999","100","100","111").Correct);
  Assert.NotEmpty(UsCcssG2Unit5FirstSixOriginal.TeacherRubrics[1]);
 }
 [Fact]
 public void NewEdulyticsAnswersMustNotCertifySourcePictures()
 {
  var own=UsCcssG2Unit5FirstSixOriginal.FirstParty(1,"10","100","96","111");
  Assert.True(own.Correct);Assert.False(own.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit5FirstSixOriginal.FirstParty(1,"999","100","96","111").Correct);
 }
}

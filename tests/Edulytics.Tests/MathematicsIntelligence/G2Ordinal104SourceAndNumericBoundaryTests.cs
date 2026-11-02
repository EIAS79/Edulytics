using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal104SourceAndNumericBoundaryTests
{
 [Fact] public void PublisherNumericCheckAlwaysNeedsEducatorForSourceWork()
 {
  var answers=new[]{"623","228","182","958"};
  var s=UsCcssG2Unit7OriginalActivities.Source(2,answers[0],answers[1],answers[2],answers[3]);
  Assert.True(s.Correct);Assert.True(s.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit7OriginalActivities.Source(2,"999",answers[1],answers[2],answers[3]).Correct);
  Assert.Equal(4,UsCcssG2Unit7OriginalActivities.SourceNumericExamples[2].Length);
  Assert.NotEmpty(UsCcssG2Unit7OriginalActivities.AuthenticTasks[2]);
 }
 [Fact] public void IndependentFirstPartyMathNeverCertifiesOriginalSource()
 {
  var values=new[]{"500","492","548","528"};
  var check=UsCcssG2Unit7OriginalActivities.NewMath(2,values[0],values[1],values[2],values[3]);
  Assert.True(check.Correct);Assert.False(check.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit7OriginalActivities.NewMath(2,"999",values[1],values[2],values[3]).Correct);
  Assert.Equal(4,UsCcssG2Unit7OriginalActivities.IndependentNumericExamples[2].Length);
 }
}
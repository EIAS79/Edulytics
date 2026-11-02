using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal103SourceAndNumericBoundaryTests
{
 [Fact] public void PublisherNumericCheckAlwaysNeedsEducatorForSourceWork()
 {
  var answers=new[]{"2","5","7","2"};
  var s=UsCcssG2Unit7OriginalActivities.Source(1,answers[0],answers[1],answers[2],answers[3]);
  Assert.True(s.Correct);Assert.True(s.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit7OriginalActivities.Source(1,"999",answers[1],answers[2],answers[3]).Correct);
  Assert.Equal(4,UsCcssG2Unit7OriginalActivities.SourceNumericExamples[1].Length);
  Assert.NotEmpty(UsCcssG2Unit7OriginalActivities.AuthenticTasks[1]);
 }
 [Fact] public void IndependentFirstPartyMathNeverCertifiesOriginalSource()
 {
  var values=new[]{"5","534","434","383"};
  var check=UsCcssG2Unit7OriginalActivities.NewMath(1,values[0],values[1],values[2],values[3]);
  Assert.True(check.Correct);Assert.False(check.TeacherReviewNeeded);
  Assert.False(UsCcssG2Unit7OriginalActivities.NewMath(1,"999",values[1],values[2],values[3]).Correct);
  Assert.Equal(4,UsCcssG2Unit7OriginalActivities.IndependentNumericExamples[1].Length);
 }
}
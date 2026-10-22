using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG2Unit3FifthLessonMetersTests
{
    [Fact]
    public void SourceABCDHaveOriginalPreparedExactTapeLengths()
    {
        var r=UsCcssG2Unit3FifthLessonMeters.SourceTapeABCD("58","72","44","180");
        Assert.True(r.Correct);Assert.True(r.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit3FifthLessonMeters.SourceTapeABCD("58","72","44","200").Correct);
    }
    [Fact]
    public void SourceEFGAndKomodo1m80AndDistinctIndependentExitAreChecked()
    {
        Assert.True(UsCcssG2Unit3FifthLessonMeters.SourceTapeEFG("336","190","282").Correct);
        Assert.False(UsCcssG2Unit3FifthLessonMeters.SourceTapeEFG("336","190","280").Correct);
        Assert.True(UsCcssG2Unit3FifthLessonMeters.DInMeters("1","80").Correct);
        Assert.False(UsCcssG2Unit3FifthLessonMeters.DInMeters("2","0").Correct);
        var r=UsCcssG2Unit3FifthLessonMeters.Independent("247","65","412");
        Assert.True(r.Correct);Assert.False(r.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit3FifthLessonMeters.Independent("247","60","412").Correct);
        Assert.True(UsCcssG2Unit3FifthLessonMeters.TeacherRubric.Count>=6);
    }
}

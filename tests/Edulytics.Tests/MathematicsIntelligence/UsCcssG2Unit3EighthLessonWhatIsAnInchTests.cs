using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG2Unit3EighthLessonWhatIsAnInchTests
{
    [Fact]
    public void InchTileSideMatchesExactlyOneRulerUnit()
    {
        var v=UsCcssG2Unit3EighthLessonWhatIsAnInch.InchTile("1","1");
        Assert.True(v.Correct);Assert.True(v.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit3EighthLessonWhatIsAnInch.InchTile("1","2").Correct);
    }
    [Fact]
    public void FirstPartyIllustrationsAreClearlySeparateFromPhysicalRulerMeasurements()
    {
        var r=UsCcssG2Unit3EighthLessonWhatIsAnInch.OriginalVirtual("4","7");
        Assert.True(r.Correct);Assert.False(r.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit3EighthLessonWhatIsAnInch.OriginalVirtual("5","7").Correct);
        var rounding=UsCcssG2Unit3EighthLessonWhatIsAnInch.NearestWholeVirtual("5","6");
        Assert.True(rounding.Correct);Assert.False(rounding.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit3EighthLessonWhatIsAnInch.NearestWholeVirtual("6","5").Correct);
        Assert.True(UsCcssG2Unit3EighthLessonWhatIsAnInch.TeacherRubric.Count>=6);
    }
}

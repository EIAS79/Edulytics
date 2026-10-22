using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG2Unit3FirstLessonStandardUnitsTests
{
    [Fact]
    public void SameSourceLengthHasFourLongOrEightShortOrFortyCubes()
    {
        var v=UsCcssG2Unit3FirstLessonStandardUnits.SourceUnits("4","8","40");
        Assert.True(v.Correct);Assert.True(v.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit3FirstLessonStandardUnits.SourceUnits("8","4","40").Correct);
        Assert.False(UsCcssG2Unit3FirstLessonStandardUnits.SourceUnits("4","8","4").Correct);
        Assert.True(UsCcssG2Unit3FirstLessonStandardUnits.TeacherRubric.Count>=6);
    }
    [Fact]
    public void IndependentThirtyCentimeterNumericAnalogyDoesNotProveActualMeasurement()
    {
        var r=UsCcssG2Unit3FirstLessonStandardUnits.FirstParty("3","6","30");
        Assert.True(r.Correct);Assert.False(r.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit3FirstLessonStandardUnits.FirstParty("6","3","30").Correct);
        Assert.False(UsCcssG2Unit3FirstLessonStandardUnits.FirstParty("3","6","40").Correct);
    }
}

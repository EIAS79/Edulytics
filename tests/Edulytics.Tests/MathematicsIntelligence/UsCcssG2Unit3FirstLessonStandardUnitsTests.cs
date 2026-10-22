using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG2Unit3FirstLessonStandardUnitsTests
{
    [Fact]
    public void FortyCmNeedsFourLongEightShortAndFortyRealOneCmEdges()
    {
        var s=UsCcssG2Unit3FirstLessonStandardUnits.SourceUnits("4","8","40");
        Assert.True(s.Correct);Assert.True(s.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit3FirstLessonStandardUnits.SourceUnits("8","4","40").Correct);
        Assert.False(UsCcssG2Unit3FirstLessonStandardUnits.SourceUnits("4","8","4").Correct);
    }
    [Fact]
    public void ThirtyCmFirstPartyIsPurelyNumerical()
    {
        var s=UsCcssG2Unit3FirstLessonStandardUnits.FirstParty("3","6","30");
        Assert.True(s.Correct);Assert.False(s.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit3FirstLessonStandardUnits.FirstParty("3","6","3").Correct);
        Assert.True(UsCcssG2Unit3FirstLessonStandardUnits.TeacherRubric.Count>=6);
    }
}

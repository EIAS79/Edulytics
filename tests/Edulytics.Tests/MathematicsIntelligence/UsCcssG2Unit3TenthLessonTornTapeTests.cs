using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG2Unit3TenthLessonTornTapeTests
{
    [Fact]
    public void FourWarmupComparisonsRespectConstantDifference()
    {
        var result=UsCcssG2Unit3TenthLessonTornTape.SourceTrueFalse("True","True","False","True");
        Assert.True(result.Correct);Assert.True(result.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit3TenthLessonTornTape.SourceTrueFalse("True","True","True","True").Correct);
    }
    [Fact]
    public void SourceNotebookAndDeskAreLengthsNotEndTicks()
    {
        Assert.True(UsCcssG2Unit3TenthLessonTornTape.SourceRuler("2","10","8","16","34","18").Correct);
        Assert.False(UsCcssG2Unit3TenthLessonTornTape.SourceRuler("2","10","10","16","34","18").Correct);
        Assert.False(UsCcssG2Unit3TenthLessonTornTape.SourceRuler("2","10","8","16","34","34").Correct);
    }
    [Fact]
    public void FirstPartyVirtualMeasurementsAreIndependentFromPublisherImages()
    {
        var r=UsCcssG2Unit3TenthLessonTornTape.Independent("8","9","12","9","13");
        Assert.True(r.Correct);Assert.False(r.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit3TenthLessonTornTape.Independent("8","9","12","9","26").Correct);
        Assert.True(UsCcssG2Unit3TenthLessonTornTape.TeacherRubric.Count>=6);
    }
}

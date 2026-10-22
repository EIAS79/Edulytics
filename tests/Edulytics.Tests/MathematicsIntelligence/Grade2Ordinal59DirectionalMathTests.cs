using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class Grade2Ordinal59DirectionalMathTests
{
    [Fact]
    public void SourceValuesNeedTeacherToConfirmRealArrowsAndStudentWork()
    {
        var r=UsCcssG2Unit4ArrowLessons.Source(7,"5","8","10","10");
        Assert.True(r.Correct);
        Assert.True(r.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit4ArrowLessons.Source(7,"999","8","10","10").Correct);
        Assert.NotEmpty(UsCcssG2Unit4ArrowLessons.TeacherRubrics[7]);
    }
    [Fact]
    public void IndependentNumbersCannotCountAsOriginalPaperDiagramEvidence()
    {
        var r=UsCcssG2Unit4ArrowLessons.Independent(7,"9","9","27","12");
        Assert.True(r.Correct);
        Assert.False(r.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit4ArrowLessons.Independent(7,"999","9","27","12").Correct);
    }
}

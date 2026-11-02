using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class Grade2Ordinal60DirectionalMathTests
{
    [Fact]
    public void SourceValuesNeedTeacherToConfirmRealArrowsAndStudentWork()
    {
        var r=UsCcssG2Unit4ArrowLessons.Source(8,"22","12","53","24");
        Assert.True(r.Correct);
        Assert.True(r.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit4ArrowLessons.Source(8,"999","12","53","24").Correct);
        Assert.NotEmpty(UsCcssG2Unit4ArrowLessons.TeacherRubrics[8]);
    }
    [Fact]
    public void IndependentNumbersCannotCountAsOriginalPaperDiagramEvidence()
    {
        var r=UsCcssG2Unit4ArrowLessons.Independent(8,"7","14","5","5");
        Assert.True(r.Correct);
        Assert.False(r.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit4ArrowLessons.Independent(8,"999","14","5","5").Correct);
    }
}

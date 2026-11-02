using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class Grade2Ordinal61DirectionalMathTests
{
    [Fact]
    public void SourceValuesNeedTeacherToConfirmRealArrowsAndStudentWork()
    {
        var r=UsCcssG2Unit4ArrowLessons.Source(9,"18","3","46","3");
        Assert.True(r.Correct);
        Assert.True(r.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit4ArrowLessons.Source(9,"999","3","46","3").Correct);
        Assert.NotEmpty(UsCcssG2Unit4ArrowLessons.TeacherRubrics[9]);
    }
    [Fact]
    public void IndependentNumbersCannotCountAsOriginalPaperDiagramEvidence()
    {
        var r=UsCcssG2Unit4ArrowLessons.Independent(9,"5","34","7","23");
        Assert.True(r.Correct);
        Assert.False(r.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit4ArrowLessons.Independent(9,"999","34","7","23").Correct);
    }
}

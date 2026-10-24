using Edulytics.Web.Curriculum.Grade3;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Stage C source contract authored, intentionally NOT EXECUTED.</summary>
public sealed class UsCcssG3Unit1Lesson19UnknownProductTests
{
    private const string Code="PED:US-CCSS-MATH:G3:U01:L19";
    [Theory]
    [InlineData("7x5=□",true)]
    [InlineData("5×7=?",true)]
    [InlineData("35=7×□",false)]
    [InlineData("7×5=35",false)]
    [InlineData("35",false)]
    [InlineData("7+5=?",false)]
    [InlineData("7×6=?",false)]
    public void CrayonTaskRequiresSymbolicUnknownProduct(string value,bool expected)
    {
        var q=new Grade3OriginalQuestion {
            AnswerType="multiplication-unknown-equation",Expected="7x5=□"
        };
        var f=Grade3SourceCompanionCatalogue.Grade(q,value);
        Assert.Equal(expected,f.NumericCorrect);
        Assert.True(f.TeacherReview);
    }
    [Fact]
    public void FiveCrayonRowsAndBothCoconutGrovesRequireSymbolAndSolution()
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code,out var lesson));
        Assert.NotNull(lesson);
        Assert.False(lesson.ReadyForStageD);
        Assert.Equal(12,lesson.SourceSubtasks.Count);
        foreach(var pair in new[]{("G3-U01-L19-A1-01","7x5=□","35"),("G3-U01-L19-A2-01","5x9=□","45"),("G3-U01-L19-A2-02","2x4=□","8")})
        {
            var symbolic=lesson.SourceSubtasks.Single(q=>q.Id==pair.Item1+"-SYMBOL");
            var numeric=lesson.SourceSubtasks.Single(q=>q.Id==pair.Item1);
            Assert.Equal("multiplication-unknown-equation",symbolic.AnswerType);
            Assert.True(Grade3SourceCompanionCatalogue.Grade(symbolic,pair.Item2).NumericCorrect);
            Assert.True(Grade3SourceCompanionCatalogue.Grade(numeric,pair.Item3).NumericCorrect);
            Assert.False(Grade3SourceCompanionCatalogue.Grade(symbolic,pair.Item3).NumericCorrect);
        }
        Assert.Equal("teacher",lesson.SourceSubtasks.Single(
            q=>q.Id=="G3-U01-L19-A1-COCRAFT").AnswerType);
        Assert.Equal(new[]{"20","18","16","14"},
            lesson.SourceSubtasks.Take(4).Select(q=>q.Expected));
        Assert.False(lesson.OriginalStudentTaskPdfRead);
    }
}

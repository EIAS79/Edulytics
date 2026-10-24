using Edulytics.Web.Curriculum.Grade3;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>2021 G3 U01 L20 Stage C contracts — authored, NOT EXECUTED.</summary>
public sealed class UsCcssG3Unit1Lesson20CommutativeSourceTests
{
    private const string Code="PED:US-CCSS-MATH:G3:U01:L20";
    [Theory]
    [InlineData("2×5=10;5×2=10",true)]
    [InlineData("5*2=10;2x5=10",true)]
    [InlineData("2×5=10",false)]
    [InlineData("10",false)]
    [InlineData("2×5=10;2×5=10",false)]
    [InlineData("2×5=10;5×2=11",false)]
    [InlineData("5×2=10;3×5=15",false)]
    public void BothSourceArraysRequireFullSwappedEquations(string answer,bool expected)
    {
        var q=new Grade3OriginalQuestion {
            AnswerType="commutative-equation-pair",Expected="2x5=10;5x2=10"
        };
        var feedback=Grade3SourceCompanionCatalogue.Grade(q,answer);
        Assert.Equal(expected,feedback.NumericCorrect);
        Assert.True(feedback.TeacherReview);
    }
    [Fact]
    public void SourceContainsSubtractionWarmupAndSeparateMLR1Explanation()
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code,out var l));
        Assert.NotNull(l);
        Assert.False(l.ReadyForStageD);
        Assert.Equal(new[]{"60","58","58","56"},l.SourceSubtasks.Take(4).Select(x=>x.Expected));
        Assert.Equal(2,l.SourceSubtasks.Count(x=>x.AnswerType=="commutative-equation-pair"));
        Assert.True(l.SourceSubtasks.Any(x=>x.Id=="G3-U01-L20-A1-02" && x.AnswerType=="teacher"));
        Assert.True(l.SourceSubtasks.Any(x=>x.Id=="G3-U01-L20-A2-02" && x.AnswerType=="teacher"));
        Assert.True(l.SourceSubtasks.Any(x=>x.Id=="G3-U01-L20-SYNTH" && x.AnswerType=="teacher"));
        Assert.True(Grade3SourceCompanionCatalogue.Grade(
            l.SourceSubtasks.Single(x=>x.Id=="G3-U01-L20-A2-01"),"6×3=18;3×6=18").NumericCorrect);
        Assert.False(Grade3SourceCompanionCatalogue.Grade(
            l.SourceSubtasks.Single(x=>x.Id=="G3-U01-L20-A2-01"),"18").NumericCorrect);
        Assert.False(l.OriginalStudentTaskPdfRead);
    }
}

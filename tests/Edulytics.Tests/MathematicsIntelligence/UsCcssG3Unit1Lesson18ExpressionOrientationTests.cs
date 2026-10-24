using Edulytics.Web.Curriculum.Grade3;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Stage C code contracts only — NOT EXECUTED.</summary>
public sealed class UsCcssG3Unit1Lesson18ExpressionOrientationTests
{
    private const string Code="PED:US-CCSS-MATH:G3:U01:L18";
    [Theory]
    [InlineData("3x5","3×5",true)]
    [InlineData("3x5","5×3",true)]
    [InlineData("3x5","15",false)]
    [InlineData("3x5","3+5",false)]
    [InlineData("3x5","3×6",false)]
    [InlineData("3x5","5×3=16",false)]
    [InlineData("2x6","6×2",true)]
    [InlineData("2x6","2×6",true)]
    [InlineData("2x6","12",false)]
    [InlineData("2x10","10×2",true)]
    public void SourceAllowsAnyJustifiedArrayOrientationWithoutScalarOnly(
        string expected,string answer,bool success)
    {
        var q=new Grade3OriginalQuestion {
            AnswerType="multiplication-expression-either-order",Expected=expected
        };
        var result=Grade3SourceCompanionCatalogue.Grade(q,answer);
        Assert.Equal(success,result.NumericCorrect);
        Assert.True(result.TeacherReview);
    }
    [Fact]
    public void AllFourSourceExpressionCasesAndFiveHumanArrayReviewTasksArePresent()
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code,out var lesson));
        Assert.NotNull(lesson);
        Assert.False(lesson.ReadyForStageD);
        Assert.Equal(10,lesson.SourceSubtasks.Count);
        Assert.Equal(4,lesson.SourceSubtasks.Count(q=>
            q.AnswerType=="multiplication-expression-either-order"));
        Assert.Equal(5,lesson.SourceSubtasks.Count(q=>q.AnswerType=="teacher"));
        Assert.Equal(new[]{"3x5","4x5","2x6","2x10"},
            lesson.SourceSubtasks.Where(q=>q.Id.Contains("-A1-")).Select(q=>q.Expected));
        Assert.Equal("teacher",lesson.OriginalEdulyticsCoolDown!.AnswerType);
        Assert.False(lesson.OriginalStudentTaskPdfRead);
    }
}

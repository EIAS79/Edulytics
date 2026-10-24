using Edulytics.Core.Curriculum;
using Edulytics.Web.Curriculum.Grade3;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Stage A–C authored tests only; NOT executed.</summary>
public sealed class UsCcssG3Unit1Lesson16PhysicalArraysTests
{
    private const string Code = "PED:US-CCSS-MATH:G3:U01:L16";

    [Fact]
    public void SourceTasksRequirePhysicalTeacherJudgmentNotJustCorrectArithmetic()
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code,out var lesson));
        Assert.NotNull(lesson);
        Assert.Equal(7,lesson.SourceSubtasks.Count);
        Assert.All(lesson.SourceSubtasks,q =>
        {
            Assert.Equal("teacher",q.AnswerType);
            var feedback=Grade3SourceCompanionCatalogue.Grade(q,"24");
            Assert.Null(feedback.NumericCorrect);
            Assert.True(feedback.TeacherReview);
        });
        Assert.Equal("teacher",lesson.IndependentPractice!.AnswerType);
        Assert.Equal("teacher",lesson.OriginalEdulyticsCoolDown!.AnswerType);
        Assert.Null(Grade3SourceCompanionCatalogue.Grade(
            lesson.OriginalEdulyticsCoolDown,"24").NumericCorrect);
        Assert.False(lesson.ReadyForStageD);
    }

    [Fact]
    public void SourceSpecificRichV2PreservesTwentyFourCubeNonExhaustiveInstruction()
    {
        var rich=RichLessonContentV2Registry.Find(Code,"en");
        Assert.NotNull(rich);
        var content=string.Join(" ",rich.ExplanationParagraphs);
        Assert.Contains("12 eggs",content);
        Assert.Contains("four circles",content);
        Assert.Contains("six groups of five",content);
        Assert.Contains("60 connecting cubes",content);
        Assert.Contains("all 20 cubes",content);
        Assert.Contains("all 24 cubes",content);
        Assert.Contains("NOT require finding every possible factor pair",content);
        Assert.Contains(rich.CommonMistakes,q=>q.Mistake.Contains("Requiring all possible arrays"));
        Assert.Contains(rich.SummaryPoints,p=>p.Contains("human-judged"));
    }
}

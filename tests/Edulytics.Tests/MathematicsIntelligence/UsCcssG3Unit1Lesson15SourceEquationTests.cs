using Edulytics.Web.Curriculum.Grade3;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Stage A–C contract authored only. DO NOT RUN until Stage D authorization.</summary>
public sealed class UsCcssG3Unit1Lesson15SourceEquationTests
{
    private const string Code = "PED:US-CCSS-MATH:G3:U01:L15";

    [Fact]
    public void SourceHasFourTensThreeSymbolicPairsAndFiveMixedUnknownRoleStories()
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code, out var lesson));
        Assert.NotNull(lesson);
        Assert.False(lesson.ReadyForStageD);
        Assert.Equal(15, lesson.SourceSubtasks.Count);
        Assert.Equal(3, lesson.SourceSubtasks.Count(x =>
            x.Id.EndsWith("-SYMBOL", StringComparison.Ordinal) &&
            x.AnswerType == "multiplication-unknown-equation"));
        Assert.Equal(new[] { "10", "20", "30", "40" },
            lesson.SourceSubtasks.Take(4).Select(x => x.Expected));
        Assert.Equal(new[] { "8", "16", "2", "45", "10" },
            lesson.SourceSubtasks.Where(x => x.Id.Contains("-A2-"))
                .Select(x => x.Expected));
        Assert.False(lesson.OriginalStudentTaskPdfRead);
    }

    [Theory]
    [InlineData("G3-U01-L15-A1-01-SYMBOL", "□×5=15", true)]
    [InlineData("G3-U01-L15-A1-01-SYMBOL", "15=5×?", true)]
    [InlineData("G3-U01-L15-A1-01-SYMBOL", "3", false)]
    [InlineData("G3-U01-L15-A1-01-SYMBOL", "3×5=15", false)]
    [InlineData("G3-U01-L15-A1-01-SYMBOL", "□×5=20", false)]
    [InlineData("G3-U01-L15-A1-02-SYMBOL", "6×2=□", true)]
    [InlineData("G3-U01-L15-A1-02-SYMBOL", "□×6=2", false)]
    [InlineData("G3-U01-L15-A1-02-SYMBOL", "12", false)]
    [InlineData("G3-U01-L15-A1-03-SYMBOL", "□×10=60", true)]
    [InlineData("G3-U01-L15-A1-03-SYMBOL", "6×10=60", false)]
    [InlineData("G3-U01-L15-A1-03-SYMBOL", "□×10=80", false)]
    public void SourceActivityOneMustExpressTheUnknownNotJustTheNumber(
        string id, string answer, bool expected)
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code, out var lesson));
        var question = lesson!.SourceSubtasks.Single(x => x.Id == id);
        var feedback = Grade3SourceCompanionCatalogue.Grade(question, answer);
        Assert.Equal(expected, feedback.NumericCorrect);
        Assert.True(feedback.TeacherReview);
    }

    [Fact]
    public void ActivityTwoPreservesThreeUnknownRolesWithoutAutogradingReasoning()
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code, out var lesson));
        var activityTwo = lesson!.SourceSubtasks.Where(x => x.Id.Contains("-A2-")).ToArray();
        Assert.Equal(5, activityTwo.Length);
        foreach (var q in activityTwo)
        {
            Assert.Equal("integer", q.AnswerType);
            Assert.True(Grade3SourceCompanionCatalogue.Grade(q, q.Expected).NumericCorrect);
            Assert.True(Grade3SourceCompanionCatalogue.Grade(q, q.Expected).TeacherReview);
        }
    }

    [Theory]
    [InlineData("1 × 10", "10")]
    [InlineData("2 × 10", "20")]
    [InlineData("3 × 10", "30")]
    [InlineData("4 × 10", "40")]
    public void WarmupNumericAnchorDoesNotCertifyOralReasoning(string expression,string expected)
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code,out var lesson));
        var question=lesson!.SourceSubtasks.Single(x=>x.Prompt.StartsWith(expression,StringComparison.Ordinal));
        Assert.Equal(expected,question.Expected);
        var feedback=Grade3SourceCompanionCatalogue.Grade(question,expected);
        Assert.True(feedback.NumericCorrect);
        Assert.True(feedback.TeacherReview);
    }

    [Fact]
    public void IndependentPracticeAndCoolDownDoNotClaimToBePublisherSourceItems()
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code,out var lesson));
        Assert.NotNull(lesson);
        Assert.Equal("integer",lesson.IndependentPractice!.AnswerType);
        Assert.Equal("6",lesson.IndependentPractice.Expected);
        Assert.Equal("integer",lesson.OriginalEdulyticsCoolDown!.AnswerType);
        Assert.Equal("12",lesson.OriginalEdulyticsCoolDown.Expected);
        Assert.False(lesson.OriginalStudentTaskPdfRead);
        Assert.False(lesson.FullArtworkVerified);
        Assert.False(lesson.PublishedRichV2Compared);
        Assert.False(lesson.SourceToFirstPartyParityVerified);
    }

    [Fact]
    public void FirstPartyVisualAndRichV2ReflectEightTwoSockGroupsAndOriginalLanguageSupports()
    {
        // Written-only source-file contract. No .NET runtime or Razor execution.
        static string FindSource(string relative)
        {
            DirectoryInfo? root = new(AppContext.BaseDirectory);
            while(root is not null)
            {
                var path=Path.Combine(root.FullName,relative.Replace('/',Path.DirectorySeparatorChar));
                if(File.Exists(path))return File.ReadAllText(path);
                root=root.Parent;
            }
            throw new FileNotFoundException(relative);
        }
        var razor=FindSource("src/Edulytics.Web/Views/Grade3SourceCompanion/Index.cshtml");
        var rich=FindSource("src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g3-u01-l15-2021.rich-lesson-v2.json");
        Assert.Contains("Eight equal boxes, each with the number two",razor);
        Assert.Contains("for(var group=0;group<8;group++)",razor);
        Assert.Contains("Capture Squares, Stage 5",razor);
        Assert.Contains("Five in a Row, Stage 8",razor);
        Assert.Contains("MLR8 Discussion Supports",razor);
        Assert.Contains("g3-u01-l15-2021-source-specific-rich-v2-authoring-20261024-v2",rich);
        Assert.Contains("Teacher reflection",rich);
        Assert.Contains("until a future unit",rich);
        Assert.Contains("source original figure",razor, StringComparison.OrdinalIgnoreCase);
    }
}

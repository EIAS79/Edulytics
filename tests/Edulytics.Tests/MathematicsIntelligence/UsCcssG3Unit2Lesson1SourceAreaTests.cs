using Edulytics.Web.Curriculum.Grade3;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>IM K–5 2021 first-edition Grade 3 Unit 2 Lesson 1 source contracts;
/// authored only; tests intentionally NOT RUN before Stage D.</summary>
public sealed class UsCcssG3Unit2Lesson1SourceAreaTests
{
    private const string Code="PED:US-CCSS-MATH:G3:U02:L01";

    [Theory]
    [InlineData("G3-U02-L01-WU-ORIGINAL-01","8","7")]
    [InlineData("G3-U02-L01-WU-02","12","21")]
    [InlineData("G3-U02-L01-WU-03","20","16")]
    public void ThreeVisuallyInspectedWarmupFiguresRequireCorrectDotCounts(
        string questionId,string right,string wrong)
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code,out var lesson));
        var q=lesson!.SourceSubtasks.Single(x=>x.Id==questionId);
        Assert.Equal("integer",q.AnswerType);
        Assert.True(Grade3SourceCompanionCatalogue.Grade(q,right).NumericCorrect);
        Assert.False(Grade3SourceCompanionCatalogue.Grade(q,wrong).NumericCorrect);
        Assert.True(Grade3SourceCompanionCatalogue.Grade(q,right).TeacherReview);
    }

    [Fact]
    public void OriginalShapeAndPatternBlockActivitiesRequireHumanAssessment()
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code,out var lesson));
        Assert.NotNull(lesson);
        Assert.True(lesson.ReadyForStageD);
        Assert.True(lesson.OriginalStudentTaskPdfRead);
        Assert.Equal(10,lesson.SourceSubtasks.Count);
        Assert.Equal(7,lesson.SourceSubtasks.Count(x=>x.AnswerType=="teacher"));
        foreach(var q in lesson.SourceSubtasks.Where(x=>x.AnswerType=="teacher"))
        {
            foreach(var submission in new[]{"0","12","20","left","right","C","same",""})
            {
                var result=Grade3SourceCompanionCatalogue.Grade(q,submission);
                Assert.Null(result.NumericCorrect);
                Assert.True(result.TeacherReview);
            }
        }
        Assert.Equal("teacher",lesson.IndependentPractice!.AnswerType);
        Assert.Equal("teacher",lesson.OriginalEdulyticsCoolDown!.AnswerType);
    }

    [Fact]
    public void CanonicalRichV2AndIndependentVisualsPreserveOriginalSourceSequence()
    {
        static string Read(string relative)
        {
            DirectoryInfo? dir=new(AppContext.BaseDirectory);
            while(dir is not null)
            {
                var path=Path.Combine(dir.FullName,relative.Replace('/',Path.DirectorySeparatorChar));
                if(File.Exists(path)) return File.ReadAllText(path);
                dir=dir.Parent;
            }
            throw new FileNotFoundException(relative);
        }
        var rich=Read("src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g3-u02-l01-2021.rich-lesson-v2.json");
        var bridge=Read("src/Edulytics.Data/Seeding/CommonCoreG3Unit2Lesson1RichV2.cs");
        var hook=Read("src/Edulytics.Data/Seeding/CommonCoreG3SourceTeachingCorrections.cs");
        var project=Read("src/Edulytics.Data/Edulytics.Data.csproj");
        var view=Read("src/Edulytics.Web/Views/Grade3SourceCompanion/Index.cshtml");
        Assert.Contains("g3-u02-l01-2021-source-abc-firstparty-20261024-v1",rich);
        Assert.Contains("g3-u02-l01-2021-source-abc-firstparty-20261024-v1",bridge);
        Assert.Contains("CommonCoreG3Unit2Lesson1RichV2.Apply(lesson)",hook);
        Assert.Contains("Edulytics.Data.Grade3.Unit2Lesson1RichV2",project);
        Assert.Contains("for(var i=0;i<8;i++)",view);
        Assert.Contains("for(var i=0;i<12;i++)",view);
        Assert.Contains("for(var i=0;i<20;i++)",view);
        Assert.Contains("Pattern blocks",rich,StringComparison.OrdinalIgnoreCase);
        Assert.Contains("Rectangle",rich,StringComparison.OrdinalIgnoreCase);
        Assert.Contains("2 rows of 4",rich);
    }
}

using Edulytics.Web.Curriculum.Grade3;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Grade 3 IM 2021 source A–B–C area tiling contracts. Tests are written
/// but INTENTIONALLY NOT EXECUTED until Stage D.
/// </summary>
public sealed class UsCcssG3Unit2Lesson2SourceAreaTests
{
    private const string Code="PED:US-CCSS-MATH:G3:U02:L02";

    [Theory]
    [InlineData(1,"9","90")]
    [InlineData(2,"18","8")]
    [InlineData(3,"13","31")]
    [InlineData(4,"30","5")]
    [InlineData(5,"36","30")]
    [InlineData(6,"21","20")]
    public void SixOriginalHandoutFiguresHaveDistinctVerifiedSquareUnitCounts(
        int figure,string correct,string incorrect)
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code,out var lesson));
        Assert.NotNull(lesson);
        var q=lesson.SourceSubtasks.Single(x=>x.Id=="G3-U02-L02-A2-"+figure.ToString("00"));
        Assert.Equal("integer",q.AnswerType);
        Assert.Equal(correct,q.Expected);
        var right=Grade3SourceCompanionCatalogue.Grade(q,correct);
        var wrong=Grade3SourceCompanionCatalogue.Grade(q,incorrect);
        Assert.True(right.NumericCorrect);
        Assert.False(wrong.NumericCorrect);
        Assert.True(right.TeacherReview);
        Assert.True(wrong.TeacherReview);
    }

    [Fact]
    public void OriginalWarmupAndShapeConstructionRemainHumanReviewed()
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code,out var lesson));
        Assert.NotNull(lesson);
        Assert.True(lesson.ReadyForStageD);
        Assert.True(lesson.OriginalStudentTaskPdfRead);
        Assert.Equal(13,lesson.SourceSubtasks.Count);
        Assert.Equal(6,lesson.SourceSubtasks.Count(x=>x.AnswerType=="integer"));
        Assert.Equal(7,lesson.SourceSubtasks.Count(x=>x.AnswerType=="teacher"));
        foreach(var original in lesson.SourceSubtasks.Where(q=>q.AnswerType=="teacher"))
        {
            foreach(var submitted in new[]{"0","12","C","9","36",""})
            {
                var f=Grade3SourceCompanionCatalogue.Grade(original,submitted);
                Assert.Null(f.NumericCorrect);
                Assert.True(f.TeacherReview);
            }
        }
    }

    [Fact]
    public void SourceSpecificRazorVisualsAndRichContentAreCanonical()
    {
        static string Source(string relative)
        {
            DirectoryInfo? current=new(AppContext.BaseDirectory);
            while(current is not null)
            {
                var candidate=Path.Combine(current.FullName,relative.Replace('/',Path.DirectorySeparatorChar));
                if(File.Exists(candidate))return File.ReadAllText(candidate);
                current=current.Parent;
            }
            throw new FileNotFoundException(relative);
        }
        var rich=Source("src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g3-u02-l02-2021.rich-lesson-v2.json");
        var bridge=Source("src/Edulytics.Data/Seeding/CommonCoreG3Unit2Lesson2RichV2.cs");
        var embedded=Source("src/Edulytics.Data/Edulytics.Data.csproj");
        var canonical=Source("src/Edulytics.Data/Seeding/CommonCoreG3SourceTeachingCorrections.cs");
        var razor=Source("src/Edulytics.Web/Views/Grade3SourceCompanion/Index.cshtml");
        Assert.Contains("g3-u02-l02-2021-source-abc-firstparty-20261024-v1",rich);
        Assert.Contains("g3-u02-l02-2021-source-abc-firstparty-20261024-v1",bridge);
        Assert.Contains("Edulytics.Data.Grade3.Unit2Lesson2RichV2",embedded);
        Assert.Contains("CommonCoreG3Unit2Lesson2RichV2.Apply(lesson)",canonical);
        Assert.Contains("9, 18, 13, 30, 36 and 21",razor);
        Assert.Contains("tile-grid model",razor);
        Assert.Contains("Activity 2: six individual 2021 area figures",razor);
    }
}

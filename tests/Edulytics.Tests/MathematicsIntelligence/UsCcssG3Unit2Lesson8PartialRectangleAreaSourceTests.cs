using Edulytics.Web.Curriculum.Grade3;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>First-edition U02 L08 contract tests AUTHORED but NOT EXECUTED.</summary>
public sealed class UsCcssG3Unit2Lesson8PartialRectangleAreaSourceTests
{
    private const string Code="PED:US-CCSS-MATH:G3:U02:L08";

    [Theory]
    [InlineData("A1-AREA","90 in²",true)]
    [InlineData("A1-AREA","90 square inches",true)]
    [InlineData("A1-AREA","90",false)]
    [InlineData("A1-AREA","90 cm2",false)]
    [InlineData("A1-AREA","90 in",false)]
    [InlineData("A2-01","32 m²",true)]
    [InlineData("A2-01","32 square meters",true)]
    [InlineData("A2-01","32 ft2",false)]
    [InlineData("A2-01","32",false)]
    [InlineData("A2-02","18 m2",true)]
    [InlineData("A2-02","18 square metres",true)]
    [InlineData("A2-02","15 m2",false)]
    [InlineData("A2-02","18 in2",false)]
    public void OriginalAreaChecksRequireCorrectNumberAndSquaredUnit(string suffix,string answer,bool correct)
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code,out var lesson));
        var q=lesson!.SourceSubtasks.Single(x=>x.Id=="G3-U02-L08-"+suffix);
        Assert.Equal("area-with-unit",q.AnswerType);
        var feedback=Grade3SourceCompanionCatalogue.Grade(q,answer);
        Assert.Equal(correct,feedback.NumericCorrect);
        Assert.True(feedback.TeacherReview);
    }

    [Theory]
    [InlineData("WU-01","20")]
    [InlineData("WU-02","24")]
    [InlineData("WU-03","16")]
    public void OriginalWarmupCountUsesViewedPicturesNotWrongSecondAlt(string suffix,string total)
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code,out var lesson));
        var q=lesson!.SourceSubtasks.Single(x=>x.Id=="G3-U02-L08-"+suffix);
        Assert.Equal(total,q.Expected);
        Assert.True(Grade3SourceCompanionCatalogue.Grade(q,total).NumericCorrect);
        Assert.False(Grade3SourceCompanionCatalogue.Grade(q,"30").NumericCorrect);
    }

    [Fact]
    public void SourceRemainsHumanReviewedForTileDrawingsAndReasoning()
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code,out var lesson));
        Assert.NotNull(lesson);
        Assert.True(lesson.ReadyForStageD);
        Assert.True(lesson.OriginalStudentTaskPdfRead);
        Assert.Equal(10,lesson.SourceSubtasks.Count);
        foreach(var q in lesson.SourceSubtasks.Where(x=>x.AnswerType=="teacher"))
        {
            Assert.Null(Grade3SourceCompanionCatalogue.Grade(q,"90").NumericCorrect);
            Assert.True(Grade3SourceCompanionCatalogue.Grade(q,"90").TeacherReview);
        }
    }

    [Fact]
    public void RichContentAndFirstPartyVisualAreWiredOnlyAtCanonicalGuardedPoint()
    {
        static string Read(string relative)
        {
            DirectoryInfo? dir=new(AppContext.BaseDirectory);
            while(dir is not null)
            {
                var path=Path.Combine(dir.FullName,relative.Replace('/',Path.DirectorySeparatorChar));
                if(File.Exists(path))return File.ReadAllText(path);
                dir=dir.Parent;
            }
            throw new FileNotFoundException(relative);
        }
        var rich=Read("src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g3-u02-l08-2021.rich-lesson-v2.json");
        var bridge=Read("src/Edulytics.Data/Seeding/CommonCoreG3Unit2Lesson8RichV2.cs");
        var project=Read("src/Edulytics.Data/Edulytics.Data.csproj");
        var hook=Read("src/Edulytics.Data/Seeding/CommonCoreG3SourceTeachingCorrections.cs");
        var view=Read("src/Edulytics.Web/Views/Grade3SourceCompanion/Index.cshtml");
        Assert.Contains("g3-u02-l08-2021-source-abc-firstparty-20261024-v1",rich);
        Assert.Contains("g3-u02-l08-2021-source-abc-firstparty-20261024-v1",bridge);
        Assert.Contains("CommonCoreG3Unit2Lesson8RichV2.Apply(lesson)",hook);
        Assert.Contains("Edulytics.Data.Grade3.Unit2Lesson8RichV2",project);
        Assert.Contains("original second image has FOUR clusters",view);
        Assert.Contains("for(var i=0;i<90;i++)",view);
        Assert.Contains("source original SVG",rich,StringComparison.OrdinalIgnoreCase);
    }
}

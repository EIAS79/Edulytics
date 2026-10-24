using Edulytics.Web.Curriculum.Grade3;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>G3 U02 L07 first-edition source A–B–C contracts, authored NOT EXECUTED.</summary>
public sealed class UsCcssG3Unit2Lesson7SquareUnitSourceTests
{
    private const string Code="PED:US-CCSS-MATH:G3:U02:L07";

    [Theory]
    [InlineData("CARD", "9 in2", true)]
    [InlineData("CARD", "9 square inches", true)]
    [InlineData("CARD", "9", false)]
    [InlineData("CARD", "9 ft2", false)]
    [InlineData("DESK", "3 ft²", true)]
    [InlineData("DESK", "3 square feet", true)]
    [InlineData("DESK", "3 in2", false)]
    [InlineData("NOTE", "55 cm²", true)]
    [InlineData("NOTE", "55 meters2", false)]
    [InlineData("ROOM", "55 square meters", true)]
    [InlineData("ROOM", "55 cm2", false)]
    [InlineData("ROOM", "55", false)]
    public void AllFourPublisherSourceMatchItemsRequireMagnitudeAndSquaredUnit(
        string suffix, string response, bool correct)
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code, out var lesson));
        var q=lesson!.SourceSubtasks.Single(x=>x.Id=="G3-U02-L07-A2-02-"+suffix);
        Assert.Equal("area-quantity-match",q.AnswerType);
        var result=Grade3SourceCompanionCatalogue.Grade(q,response);
        Assert.Equal(correct,result.NumericCorrect);
        Assert.True(result.TeacherReview);
    }

    [Fact]
    public void SourceContainsAllSixQualitativeCasesAndBothDocumentedOptionalVersions()
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code,out var lesson));
        Assert.NotNull(lesson);
        Assert.True(lesson.ReadyForStageD);
        Assert.True(lesson.OriginalStudentTaskPdfRead);
        Assert.Equal(16,lesson.SourceSubtasks.Count);
        Assert.Equal(6,lesson.SourceSubtasks.Count(x=>x.Id.Contains("A2-01-")));
        Assert.Equal(4,lesson.SourceSubtasks.Count(x=>x.AnswerType=="area-quantity-match"));
        var pdf=lesson.SourceSubtasks.Single(x=>x.Id.EndsWith("PDF-YARDS"));
        var html=lesson.SourceSubtasks.Single(x=>x.Id.EndsWith("HTML-METERS"));
        Assert.Equal("teacher",pdf.AnswerType);
        Assert.Equal("teacher",html.AnswerType);
        Assert.Contains("YARDS",pdf.Prompt);
        Assert.Contains("METERS",html.Prompt);
        foreach(var q in lesson.SourceSubtasks.Where(x=>x.AnswerType=="teacher"))
        {
            foreach(var answer in new[]{"55","meter","square yard",""})
            {
                var feedback=Grade3SourceCompanionCatalogue.Grade(q,answer);
                Assert.Null(feedback.NumericCorrect);
                Assert.True(feedback.TeacherReview);
            }
        }
    }

    [Fact]
    public void CanonicalFirstPartyRichV2AndIndependentVisualMustBeConnected()
    {
        static string Read(string file)
        {
            DirectoryInfo? d=new(AppContext.BaseDirectory);
            while(d is not null)
            {
                var candidate=Path.Combine(d.FullName,file.Replace('/',Path.DirectorySeparatorChar));
                if(File.Exists(candidate)) return File.ReadAllText(candidate);
                d=d.Parent;
            }
            throw new FileNotFoundException(file);
        }
        var rich=Read("src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g3-u02-l07-2021.rich-lesson-v2.json");
        var bridge=Read("src/Edulytics.Data/Seeding/CommonCoreG3Unit2Lesson7RichV2.cs");
        var materializer=Read("src/Edulytics.Data/Seeding/CommonCoreG3SourceTeachingCorrections.cs");
        var project=Read("src/Edulytics.Data/Edulytics.Data.csproj");
        var razor=Read("src/Edulytics.Web/Views/Grade3SourceCompanion/Index.cshtml");
        Assert.Contains("g3-u02-l07-2021-source-abc-firstparty-20261024-v1",rich);
        Assert.Contains("g3-u02-l07-2021-source-abc-firstparty-20261024-v1",bridge);
        Assert.Contains("CommonCoreG3Unit2Lesson7RichV2.Apply(lesson)",materializer);
        Assert.Contains("Edulytics.Data.Grade3.Unit2Lesson7RichV2",project);
        Assert.Contains("l.UnitNumber == 2 && l.LessonNumber == 7",razor);
        Assert.Contains("square YARDS",razor);
        Assert.Contains("area-quantity-match",
            Read("src/Edulytics.Web/Curriculum/Grade3/Grade3SourceCompanionCatalogue.cs"));
    }
}

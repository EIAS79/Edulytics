using Edulytics.Web.Curriculum.Grade3;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>2021 first-edition U02 L09 A–B–C contracts; AUTHORED, NOT RUN.</summary>
public sealed class UsCcssG3Unit2Lesson9MeasuredRectangleSourceTests
{
    private const string Code = "PED:US-CCSS-MATH:G3:U02:L09";

    [Theory]
    [InlineData("A1-A-NOMINAL", "18 cm²", true)]
    [InlineData("A1-A-NOMINAL", "18 cm2", true)]
    [InlineData("A1-A-NOMINAL", "18", false)]
    [InlineData("A1-A-NOMINAL", "18 ft2", false)]
    [InlineData("A1-B-NOMINAL", "20 cm²", true)]
    [InlineData("A1-B-NOMINAL", "20 cm", false)]
    [InlineData("A1-B-NOMINAL", "30 cm²", false)]
    [InlineData("A1-C-NOMINAL", "30 cm²", true)]
    [InlineData("A1-C-NOMINAL", "30", false)]
    [InlineData("A1-C-NOMINAL", "18 cm²", false)]
    public void NominalSourceDimensionsUseSquaredCentimetersAndTeacherConfirmation(string id, string submitted, bool correct)
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code, out var lesson));
        var q = lesson!.SourceSubtasks.Single(x => x.Id == "G3-U02-L09-" + id);
        Assert.Equal("area-with-unit", q.AnswerType);
        var result = Grade3SourceCompanionCatalogue.Grade(q, submitted);
        Assert.Equal(correct, result.NumericCorrect);
        Assert.True(result.TeacherReview); // Nominal dimension ≠ proof of ruler use.
    }

    [Fact]
    public void SourcePhysicalTapeAssignmentCannotBeAutoPassed()
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code, out var lesson));
        Assert.NotNull(lesson);
        Assert.True(lesson.ReadyForStageD);
        Assert.True(lesson.OriginalStudentTaskPdfRead);
        Assert.Equal(15, lesson.SourceSubtasks.Count);
        var expected = new[] {4, 6, 9, 10, 12, 16};
        foreach (var area in expected)
        {
            var q = lesson.SourceSubtasks.Single(x =>
                x.Id == "G3-U02-L09-A2-TAPE-" + area);
            Assert.Equal("teacher", q.AnswerType);
            foreach (var attempt in new[] {area.ToString(), area + " ft²", "3x4", ""})
            {
                var result = Grade3SourceCompanionCatalogue.Grade(q, attempt);
                Assert.Null(result.NumericCorrect);
                Assert.True(result.TeacherReview);
            }
        }
        Assert.Equal(3,lesson.SourceSubtasks.Count(x=>x.AnswerType=="area-with-unit"));
    }

    [Fact]
    public void ZeroRulerAndCanonicalRichV2AreExplicitlySourceBound()
    {
        static string Read(string relative)
        {
            DirectoryInfo? dir = new(AppContext.BaseDirectory);
            while (dir is not null)
            {
                var p = Path.Combine(dir.FullName, relative.Replace('/', Path.DirectorySeparatorChar));
                if (File.Exists(p)) return File.ReadAllText(p);
                dir = dir.Parent;
            }
            throw new FileNotFoundException(relative);
        }

        var rich = Read("src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g3-u02-l09-2021.rich-lesson-v2.json");
        var bridge = Read("src/Edulytics.Data/Seeding/CommonCoreG3Unit2Lesson9RichV2.cs");
        var hook = Read("src/Edulytics.Data/Seeding/CommonCoreG3SourceTeachingCorrections.cs");
        var project = Read("src/Edulytics.Data/Edulytics.Data.csproj");
        var view = Read("src/Edulytics.Web/Views/Grade3SourceCompanion/Index.cshtml");
        Assert.Contains("g3-u02-l09-2021-source-abc-firstparty-20261024-v2", rich);
        Assert.Contains("g3-u02-l09-2021-source-abc-firstparty-20261024-v2", bridge);
        Assert.Contains("CommonCoreG3Unit2Lesson9RichV2.Apply(lesson)", hook);
        Assert.Contains("Edulytics.Data.Grade3.Unit2Lesson9RichV2", project);
        Assert.Contains("A — 6 × 3 = 18 cm²", view);
        Assert.Contains("B — 4 × 5 = 20 cm²", view);
        Assert.Contains("C — 10 × 3 = 30 cm²", view);
        Assert.Contains("at ZERO", view);
        Assert.Contains("physically", rich, StringComparison.OrdinalIgnoreCase);
    }
}

using Edulytics.Web.Curriculum.Grade3;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Grade 3 2021 Unit 2 Lesson 9 original academic source task contracts.
/// AUTHORING ONLY: the tests are written but MUST NOT be run before Stage D.
/// </summary>
public sealed class UsCcssG3Unit2Lesson9MeasuredRectangleTests
{
    private const string Code = "PED:US-CCSS-MATH:G3:U02:L09";

    [Theory]
    [InlineData("G3-U02-L09-A1-A-NOMINAL", "18 cm²", true)]
    [InlineData("G3-U02-L09-A1-A-NOMINAL", "18", false)]
    [InlineData("G3-U02-L09-A1-A-NOMINAL", "18 in²", false)]
    [InlineData("G3-U02-L09-A1-A-NOMINAL", "20 cm²", false)]
    [InlineData("G3-U02-L09-A1-B-NOMINAL", "20 cm²", true)]
    [InlineData("G3-U02-L09-A1-B-NOMINAL", "20 ft²", false)]
    [InlineData("G3-U02-L09-A1-C-NOMINAL", "30 cm²", true)]
    [InlineData("G3-U02-L09-A1-C-NOMINAL", "30", false)]
    public void ThreeOriginalNominalCentimetreDimensionsRequireSquareAreaUnits(string id,string answer,bool correct)
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code,out var lesson));
        Assert.NotNull(lesson);
        var item=lesson.SourceSubtasks.Single(q=>q.Id==id);
        Assert.Equal("area-with-unit",item.AnswerType);
        var feedback=Grade3SourceCompanionCatalogue.Grade(item,answer);
        Assert.Equal(correct,feedback.NumericCorrect);
        Assert.True(feedback.TeacherReview);
    }

    [Fact]
    public void AllSixPossibleTapedFloorTargetsRequireHumanPhysicalReview()
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code,out var lesson));
        Assert.NotNull(lesson);
        Assert.True(lesson.ReadyForStageD);
        Assert.True(lesson.OriginalStudentTaskPdfRead);
        foreach(var n in new[]{4,6,9,10,12,16})
        {
            var q=lesson.SourceSubtasks.Single(x=>x.Id==$"G3-U02-L09-A2-TAPE-{n}");
            Assert.Equal("teacher",q.AnswerType);
            foreach(var candidate in new[]{n.ToString(),$"{n} ft²","built rectangle",""})
            {
                var grade=Grade3SourceCompanionCatalogue.Grade(q,candidate);
                Assert.Null(grade.NumericCorrect);
                Assert.True(grade.TeacherReview);
            }
        }
        Assert.Equal(15,lesson.SourceSubtasks.Count);
    }

    [Fact]
    public void GuardedCanonicalRichV2AndIndependentPublishedModelsRetainThreeSourceProportions()
    {
        static string GetSource(string path)
        {
            DirectoryInfo? root=new(AppContext.BaseDirectory);
            while(root is not null)
            {
                var file=Path.Combine(root.FullName,path.Replace('/',Path.DirectorySeparatorChar));
                if(File.Exists(file)) return File.ReadAllText(file);
                root=root.Parent;
            }
            throw new FileNotFoundException(path);
        }
        var rich=GetSource("src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g3-u02-l09-2021.rich-lesson-v2.json");
        var bridge=GetSource("src/Edulytics.Data/Seeding/CommonCoreG3Unit2Lesson9RichV2.cs");
        var project=GetSource("src/Edulytics.Data/Edulytics.Data.csproj");
        var materializer=GetSource("src/Edulytics.Data/Seeding/CommonCoreG3SourceTeachingCorrections.cs");
        var razor=GetSource("src/Edulytics.Web/Views/Grade3SourceCompanion/Index.cshtml");
        Assert.Contains("g3-u02-l09-2021-source-abc-firstparty-20261024-v2",rich);
        Assert.Contains("g3-u02-l09-2021-source-abc-firstparty-20261024-v2",bridge);
        Assert.Contains("Edulytics.Data.Grade3.Unit2Lesson9RichV2",project);
        Assert.Contains("CommonCoreG3Unit2Lesson9RichV2.Apply(lesson)",materializer);
        Assert.Contains("A — 6 × 3 = 18 cm²",razor);
        Assert.Contains("B — 4 × 5 = 20 cm²",razor);
        Assert.Contains("C — 10 × 3 = 30 cm²",razor);
        Assert.Contains("width:144px;height:72px",razor);
        Assert.Contains("width:96px;height:120px",razor);
        Assert.Contains("width:240px;height:72px",razor);
    }
}

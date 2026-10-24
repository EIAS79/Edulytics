using Edulytics.Web.Curriculum.Grade3;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>2021 source unit and physical measurement contracts, authored NOT EXECUTED in A–B–C.</summary>
public sealed class UsCcssG3Unit2Lesson6SourceUnitMeasurementTests
{
    private const string Code="PED:US-CCSS-MATH:G3:U02:L06";

    [Fact]
    public void SourcePhysicalMeasurementsAndOriginalEstimationsRequireTeacherEvaluation()
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code,out var lesson));
        Assert.True(lesson!.ReadyForStageD);
        Assert.True(lesson.OriginalStudentTaskPdfRead);
        Assert.Equal(10,lesson.SourceSubtasks.Count);
        Assert.All(lesson.SourceSubtasks,q=>{
            Assert.Equal("teacher",q.AnswerType);
            foreach(var answer in new[]{"4","25","25.8064","4 in²","26 cm²",""})
            {
                var feedback=Grade3SourceCompanionCatalogue.Grade(q,answer);
                Assert.Null(feedback.NumericCorrect);
                Assert.True(feedback.TeacherReview);
            }
        });
    }

    [Theory]
    [InlineData("21 cm²",true)]
    [InlineData("21 in²",false)]
    [InlineData("21",false)]
    [InlineData("24 cm²",false)]
    [InlineData("25 cm²",false)]
    public void NewIndependentPracticeRequiresBothCorrectCountAndSquareCentimeterUnit(
        string answer,bool expected)
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code,out var lesson));
        Assert.Equal("area-with-unit",lesson!.IndependentPractice!.AnswerType);
        var result=Grade3SourceCompanionCatalogue.Grade(lesson.IndependentPractice,answer);
        Assert.Equal(expected,result.NumericCorrect);
        Assert.True(result.TeacherReview);
    }

    [Theory]
    [InlineData("24 in²",true)]
    [InlineData("24 cm²",false)]
    [InlineData("24",false)]
    [InlineData("21 in²",false)]
    public void FirstPartyCoolDownRequiresCorrectSquareInchUnit(string answer,bool expected)
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code,out var lesson));
        Assert.Equal("area-with-unit",lesson!.OriginalEdulyticsCoolDown!.AnswerType);
        var result=Grade3SourceCompanionCatalogue.Grade(lesson.OriginalEdulyticsCoolDown,answer);
        Assert.Equal(expected,result.NumericCorrect);
        Assert.True(result.TeacherReview);
    }

    [Fact]
    public void SourceFourDifferentSquareCountsPreserveOnePhysicalRectangleAndCanonicalRichV2()
    {
        static string Read(string rel)
        {
            DirectoryInfo? dir=new(AppContext.BaseDirectory);
            while(dir is not null)
            {
                var candidate=Path.Combine(dir.FullName,rel.Replace('/',Path.DirectorySeparatorChar));
                if(File.Exists(candidate))return File.ReadAllText(candidate);
                dir=dir.Parent;
            }
            throw new FileNotFoundException(rel);
        }
        var rich=Read("src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g3-u02-l06-2021.rich-lesson-v2.json");
        var bridge=Read("src/Edulytics.Data/Seeding/CommonCoreG3Unit2Lesson6RichV2.cs");
        var hook=Read("src/Edulytics.Data/Seeding/CommonCoreG3SourceTeachingCorrections.cs");
        var project=Read("src/Edulytics.Data/Edulytics.Data.csproj");
        var razor=Read("src/Edulytics.Web/Views/Grade3SourceCompanion/Index.cshtml");
        Assert.Contains("g3-u02-l06-2021-source-abc-firstparty-20261024-v1",rich);
        Assert.Contains("g3-u02-l06-2021-source-abc-firstparty-20261024-v1",bridge);
        Assert.Contains("Edulytics.Data.Grade3.Unit2Lesson6RichV2",project);
        Assert.Contains("CommonCoreG3Unit2Lesson6RichV2.Apply(lesson)",hook);
        Assert.Contains("5×10=50",razor);
        Assert.Contains("3×6=18",razor);
        Assert.Contains("2×4=8",razor);
        Assert.Contains("1×2=2",razor);
        Assert.Contains("width:210px;height:105px",razor);
        Assert.Contains("NOT exactly 25",razor);
        Assert.Contains("MLR2",rich);
    }
}

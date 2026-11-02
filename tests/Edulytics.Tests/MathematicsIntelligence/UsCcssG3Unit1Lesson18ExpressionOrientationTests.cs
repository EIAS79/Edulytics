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
        Assert.True(lesson.ReadyForStageD);
        Assert.Equal(10,lesson.SourceSubtasks.Count);
        Assert.Equal(4,lesson.SourceSubtasks.Count(q=>
            q.AnswerType=="multiplication-expression-either-order"));
        Assert.Equal(5,lesson.SourceSubtasks.Count(q=>q.AnswerType=="teacher"));
        Assert.Equal(new[]{"3x5","4x5","2x6","2x10"},
            lesson.SourceSubtasks.Where(q=>q.Id.Contains("-A1-")).Select(q=>q.Expected));
        Assert.Equal("teacher",lesson.OriginalEdulyticsCoolDown!.AnswerType);
        Assert.True(lesson.OriginalStudentTaskPdfRead);
    }

    [Fact]
    public void CanonicalRichV2AndFirstPartyTriangleVisualAreSourceBound()
    {
        static string Read(string path)
        {
            DirectoryInfo? root=new(AppContext.BaseDirectory);
            while(root is not null)
            {
                var full=Path.Combine(root.FullName,path.Replace('/',Path.DirectorySeparatorChar));
                if(File.Exists(full))return File.ReadAllText(full);
                root=root.Parent;
            }
            throw new FileNotFoundException(path);
        }
        var rich=Read("src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g3-u01-l18-2021.rich-lesson-v2.json");
        var razor=Read("src/Edulytics.Web/Views/Grade3SourceCompanion/Index.cshtml");
        var source=Read("src/Edulytics.Data/Seeding/CommonCoreG3Unit1Lesson18RichV2.cs");
        var materializer=Read("src/Edulytics.Data/Seeding/CommonCoreG3SourceTeachingCorrections.cs");
        var project=Read("src/Edulytics.Data/Edulytics.Data.csproj");
        Assert.Contains("g3-u01-l18-first-edition-source-specific-abc-20261024-v2",rich);
        Assert.Contains("g3-u01-l18-first-edition-source-specific-abc-20261024-v2",source);
        Assert.Contains("CommonCoreG3Unit1Lesson18RichV2.Apply(lesson)",materializer);
        Assert.Contains("Edulytics.Data.Grade3.Unit1Lesson18RichV2",project);
        Assert.Contains("for(var triangle=0;triangle<20;triangle++)",razor);
        Assert.Contains("clip-path:polygon(50% 0%,100% 100%,0% 100%)",razor);
        Assert.Contains("four source learner subtask",rich,StringComparison.OrdinalIgnoreCase);
    }
}

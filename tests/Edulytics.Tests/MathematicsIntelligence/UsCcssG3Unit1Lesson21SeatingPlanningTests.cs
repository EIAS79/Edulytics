using Edulytics.Web.Curriculum.Grade3;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade 3 2021 optional model-and-graph source; authored tests NOT RUN.</summary>
public sealed class UsCcssG3Unit1Lesson21SeatingPlanningTests
{
    [Fact]
    public void AllAuthenticPlanningTasksRemainTeacherReviewedNotUniqueNumeric()
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(
            "PED:US-CCSS-MATH:G3:U01:L21",out var lesson));
        Assert.NotNull(lesson);
        Assert.True(lesson.ReadyForStageD);
        Assert.True(lesson.OriginalStudentTaskPdfRead);;
        Assert.Equal(5,lesson.SourceSubtasks.Count);
        Assert.All(lesson.SourceSubtasks,q=>
        {
            Assert.Equal("teacher",q.AnswerType);
            var f=Grade3SourceCompanionCatalogue.Grade(q,"16");
            Assert.Null(f.NumericCorrect);
            Assert.True(f.TeacherReview);
        });
        Assert.Equal("teacher",lesson.IndependentPractice!.AnswerType);
        Assert.Equal("teacher",lesson.OriginalEdulyticsCoolDown!.AnswerType);
        Assert.Contains("16",lesson.SourceSubtasks.Single(q=>q.Id.Contains("ASSUMPTIONS")).Prompt);
        Assert.Contains("2, 4, 5 or 10",lesson.SourceSubtasks.Single(q=>q.Id.Contains("ASSUMPTIONS")).Prompt);
        Assert.Contains("GREATER than 1",lesson.SourceSubtasks.Single(q=>q.Id=="G3-U01-L21-A2-01").Prompt);
    }

    [Fact]
    public void SixTableSourceWarmupIsSeparateFromSixteenTableDesignProject()
    {
        static string Read(string relative)
        {
            DirectoryInfo? root=new(AppContext.BaseDirectory);
            while(root is not null)
            {
                var path=Path.Combine(root.FullName,relative.Replace('/',Path.DirectorySeparatorChar));
                if(File.Exists(path))return File.ReadAllText(path);
                root=root.Parent;
            }
            throw new FileNotFoundException(relative);
        }
        var view=Read("src/Edulytics.Web/Views/Grade3SourceCompanion/Index.cshtml");
        var rich=Read("src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g3-u01-l21-2021.rich-lesson-v2.json");
        var bridge=Read("src/Edulytics.Data/Seeding/CommonCoreG3Unit1Lesson21RichV2.cs");
        var hook=Read("src/Edulytics.Data/Seeding/CommonCoreG3SourceTeachingCorrections.cs");
        var project=Read("src/Edulytics.Data/Edulytics.Data.csproj");
        Assert.Contains("for(var table=0;table<6;table++)",view);
        Assert.Contains("Teacher-controlled reveal",view);
        Assert.Contains("six-table",rich,StringComparison.OrdinalIgnoreCase);
        Assert.Contains("sixteen square tables",rich);
        Assert.Contains("g3-u01-l21-2021-source-abc-firstparty-20261024-v2",rich);
        Assert.Contains("g3-u01-l21-2021-source-abc-firstparty-20261024-v2",bridge);
        Assert.Contains("Edulytics.Data.Grade3.Unit1Lesson21RichV2",project);
        Assert.Contains("CommonCoreG3Unit1Lesson21RichV2.Apply(lesson)",hook);
    }

    [Theory]
    [InlineData("16")]
    [InlineData("64")]
    [InlineData("4")]
    [InlineData("")]
    [InlineData("Game A:4;Game B:8;Game C:10;Game D:20")]
    public void NoPlannerResponseCanBeAutoApproved(string submitted)
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(
            "PED:US-CCSS-MATH:G3:U01:L21",out var lesson));
        foreach(var q in lesson!.SourceSubtasks.Concat(new[]{
            lesson.IndependentPractice!,lesson.OriginalEdulyticsCoolDown!}))
        {
            var f=Grade3SourceCompanionCatalogue.Grade(q,submitted);
            Assert.Null(f.NumericCorrect);
            Assert.True(f.TeacherReview);
        }
    }
}

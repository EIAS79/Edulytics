using Edulytics.Web.Curriculum.Grade3;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// First edition 2021 G3 U02 L04 authoring contracts.
/// Written during A–B–C; intentionally NOT EXECUTED until Stage D.
/// </summary>
public sealed class UsCcssG3Unit2Lesson4RectangleAreaSourceTests
{
    private const string Code="PED:US-CCSS-MATH:G3:U02:L04";

    [Theory]
    [InlineData("G3-U02-L04-A2-01", "18", "19")]
    [InlineData("G3-U02-L04-A2-02", "30", "24")]
    [InlineData("G3-U02-L04-A2-03", "60", "50")]
    [InlineData("G3-U02-L04-A2-04", "45", "40")]
    public void FourOriginalSourceRectanglesRequireIndividuallyCorrectArea(
        string taskId,string correct,string wrong)
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code,out var lesson));
        var q=Assert.Single(lesson!.SourceSubtasks, x=>x.Id==taskId);
        Assert.Equal("integer",q.AnswerType);
        Assert.Equal(correct,q.Expected);
        var pass=Grade3SourceCompanionCatalogue.Grade(q,correct);
        var fail=Grade3SourceCompanionCatalogue.Grade(q,wrong);
        Assert.True(pass.NumericCorrect);
        Assert.False(fail.NumericCorrect);
        Assert.True(pass.TeacherReview);
        Assert.True(fail.TeacherReview);
    }

    [Fact]
    public void HiddenPartnerReconstructionAndReasoningCannotBeMachineCertified()
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code,out var lesson));
        Assert.True(lesson!.OriginalStudentTaskPdfRead);
        Assert.True(lesson.ReadyForStageD);
        Assert.Equal(11,lesson.SourceSubtasks.Count);
        foreach(var id in new[]{"G3-U02-L04-A1-FIRST","G3-U02-L04-A1-SWAP","G3-U02-L04-SYNTH"})
        {
            var q=Assert.Single(lesson.SourceSubtasks,x=>x.Id==id);
            Assert.Equal("teacher",q.AnswerType);
            foreach(var response in new[]{"60","4 rows and 5 columns",""})
            {
                var result=Grade3SourceCompanionCatalogue.Grade(q,response);
                Assert.Null(result.NumericCorrect);
                Assert.True(result.TeacherReview);
            }
        }
    }

    [Fact]
    public void FirstPartyRichV2BridgeAndThirdOriginalDrawingRemainCorrect()
    {
        static string Read(string relative)
        {
            DirectoryInfo? dir=new(AppContext.BaseDirectory);
            while(dir is not null)
            {
                var p=Path.Combine(dir.FullName,
                    relative.Replace('/',Path.DirectorySeparatorChar));
                if(File.Exists(p)) return File.ReadAllText(p);
                dir=dir.Parent;
            }
            throw new FileNotFoundException(relative);
        }
        var rich=Read("src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g3-u02-l04-2021.rich-lesson-v2.json");
        var bridge=Read("src/Edulytics.Data/Seeding/CommonCoreG3Unit2Lesson4RichV2.cs");
        var pack=Read("src/Edulytics.Data/Edulytics.Data.csproj");
        var materializer=Read("src/Edulytics.Data/Seeding/CommonCoreG3SourceTeachingCorrections.cs");
        var razor=Read("src/Edulytics.Web/Views/Grade3SourceCompanion/Index.cshtml");
        Assert.Contains("g3-u02-l04-2021-source-abc-firstparty-20261024-v1",rich);
        Assert.Contains("g3-u02-l04-2021-source-abc-firstparty-20261024-v1",bridge);
        Assert.Contains("Edulytics.Data.Grade3.Unit2Lesson4RichV2",pack);
        Assert.Contains("CommonCoreG3Unit2Lesson4RichV2.Apply(lesson)",materializer);
        Assert.Contains("6 × 10 = 60",razor);
        Assert.Contains("rc.r*rc.c",razor);
        Assert.DoesNotContain("third image has no independently verified",razor);
    }
}

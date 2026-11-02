using Edulytics.Web.Curriculum.Grade3;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// First-party 2021 Grade 3 Unit 2 Lesson 5 Stage A–B–C authoring contracts.
/// Tests intentionally written but not run until Stage D.
/// </summary>
public sealed class UsCcssG3Unit2Lesson5SourceCardsAndAreasTests
{
    private const string Code="PED:US-CCSS-MATH:G3:U02:L05";

    [Theory]
    [InlineData("G3-U02-L05-WU-01","15","18")]
    [InlineData("G3-U02-L05-WU-02","18","15")]
    [InlineData("G3-U02-L05-WU-03","24","18")]
    [InlineData("G3-U02-L05-A2-01","12","6")]
    [InlineData("G3-U02-L05-A2-02","24","20")]
    [InlineData("G3-U02-L05-A2-03","18","24")]
    [InlineData("G3-U02-L05-A2-04","28","24")]
    [InlineData("G3-U02-L05-A2-05","6","12")]
    public void NumericallyCorrectCountOrAreaNeverAutoCertifiesDiagram(
        string id,string expected,string incorrect)
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code,out var l));
        var q=Assert.Single(l!.SourceSubtasks,x=>x.Id==id);
        Assert.Equal("integer",q.AnswerType);
        Assert.Equal(expected,q.Expected);
        var good=Grade3SourceCompanionCatalogue.Grade(q,expected);
        var bad=Grade3SourceCompanionCatalogue.Grade(q,incorrect);
        Assert.True(good.NumericCorrect);
        Assert.False(bad.NumericCorrect);
        Assert.True(good.TeacherReview);
        Assert.True(bad.TeacherReview);
    }

    [Theory]
    [InlineData("G3-U02-L05-A1-01","F","B")]
    [InlineData("G3-U02-L05-A1-02","C","A")]
    [InlineData("G3-U02-L05-A1-03","E","F")]
    [InlineData("G3-U02-L05-A1-04","D","G")]
    [InlineData("G3-U02-L05-A1-05","A","C")]
    [InlineData("G3-U02-L05-A1-06","G","E")]
    [InlineData("G3-U02-L05-A1-07","B","F")]
    public void EveryOriginalExpressionMapsToItsSourceEquivalentCard(
        string id,string correct,string wrong)
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code,out var l));
        var q=Assert.Single(l!.SourceSubtasks,x=>x.Id==id);
        Assert.Equal("source-card-letter",q.AnswerType);
        Assert.Equal(correct,q.Expected);
        // The original publisher card geometry is a physical/source-art task.
        // Retain the inspected answer-key label without awarding a screen-only
        // numeric pass before the teacher verifies the actual match.
        foreach (var response in new[] { correct, correct.ToLowerInvariant(), wrong, "", "15" })
        {
            var review=Grade3SourceCompanionCatalogue.Grade(q,response);
            Assert.Null(review.NumericCorrect);
            Assert.True(review.TeacherReview);
        }
    }

    [Fact]
    public void TeacherDialogueMustNotBeReplacedWithAnAutomaticProductScore()
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code,out var l));
        Assert.True(l!.OriginalStudentTaskPdfRead);
        Assert.True(l.ReadyForStageD);
        Assert.Equal(16,l.SourceSubtasks.Count);
        var synth=Assert.Single(l.SourceSubtasks,x=>x.Id=="G3-U02-L05-SYNTH");
        foreach(var response in new[]{"14","2×7=14","area in square units",""})
        {
            var mark=Grade3SourceCompanionCatalogue.Grade(synth,response);
            Assert.Null(mark.NumericCorrect);
            Assert.True(mark.TeacherReview);
        }
    }

    [Fact]
    public void GuardedRichV2AndSevenNewFirstPartyCardsMustRemainConnected()
    {
        static string Read(string path)
        {
            DirectoryInfo? dir=new(AppContext.BaseDirectory);
            while(dir is not null)
            {
                var p=Path.Combine(dir.FullName,path.Replace('/',Path.DirectorySeparatorChar));
                if(File.Exists(p)) return File.ReadAllText(p);
                dir=dir.Parent;
            }
            throw new FileNotFoundException(path);
        }
        var rich=Read("src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g3-u02-l05-2021.rich-lesson-v2.json");
        var source=Read("src/Edulytics.Data/Seeding/CommonCoreG3Unit2Lesson5RichV2.cs");
        var project=Read("src/Edulytics.Data/Edulytics.Data.csproj");
        var hook=Read("src/Edulytics.Data/Seeding/CommonCoreG3SourceTeachingCorrections.cs");
        var view=Read("src/Edulytics.Web/Views/Grade3SourceCompanion/Index.cshtml");
        var grader=Read("src/Edulytics.Web/Curriculum/Grade3/Grade3SourceCompanionCatalogue.cs");
        Assert.Contains("g3-u02-l05-2021-source-abc-firstparty-20261024-v1",rich);
        Assert.Contains("g3-u02-l05-2021-source-abc-firstparty-20261024-v1",source);
        Assert.Contains("Edulytics.Data.Grade3.Unit2Lesson5RichV2",project);
        Assert.Contains("CommonCoreG3Unit2Lesson5RichV2.Apply(lesson)",hook);
        Assert.Contains("source-card-letter",grader);
        Assert.Contains("letter:\"A\",rows:2,cols:6",view);
        Assert.Contains("letter:\"E\",rows:7,cols:10",view);
        Assert.Contains("SIX groups of FOUR = 24",view);
        Assert.DoesNotContain("third source dot picture requires teacher image inspection",view);
    }
}

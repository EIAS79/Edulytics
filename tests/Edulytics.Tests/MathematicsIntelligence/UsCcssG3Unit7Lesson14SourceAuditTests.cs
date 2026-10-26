using System;
using System.Linq;
using System.Text.Json;
using Edulytics.Web.Curriculum.Grade3;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssG3Unit7Lesson14SourceAuditTests
{
    private const string Code = "PED:US-CCSS-MATH:G3:U07:L14";
    [Fact]
    public void EveryAuthoredSourceElementHasATeacherAwareCompanion()
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code, out var lesson));
        Assert.NotNull(lesson);
        Assert.Equal(5, lesson!.SourceSubtasks.Count);
        Assert.Equal(5,
            lesson.SourceSubtasks.Count(q => q.AnswerType == "teacher"));
        foreach (var q in lesson.SourceSubtasks)
        {
            Assert.StartsWith("G3-U07-L14-SOURCE-", q.Id);
            if (q.AnswerType == "teacher")
            {
                Assert.Null(q.Expected);
                var evaluation = Grade3SourceCompanionCatalogue.Grade(q, "123");
                Assert.Null(evaluation.NumericCorrect);
                Assert.True(evaluation.TeacherReview);
            }
        }
    }
    [Fact]
    public void OriginalEditionContentVersionIsEmbeddedAndIdentityPinned()
    {
        var assembly=System.Reflection.Assembly.Load("Edulytics.Data");
        using var stream=assembly.GetManifestResourceStream("Edulytics.Data.Grade3.Unit7Lesson14RichV2");
        Assert.NotNull(stream);
        using var doc=JsonDocument.Parse(stream!);
        Assert.Equal("g3-u07-l14-2021-wax-print-repeated-quadrilateral-and-bundle-info-gap-v1", doc.RootElement.GetProperty("ContentVersion").GetString());
        Assert.Equal(Code, doc.RootElement.GetProperty("Lessons")[0].GetProperty("LessonCode").GetString());
    }
}

using System.Text.Json;
using System.Text.Json.Serialization;
using Edulytics.Core.Curriculum;
using Edulytics.Data.Seeding;
using Edulytics.Web.Curriculum.Grade3;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Unrun Grade 3 Unit 3 Lesson 6 authoring contracts. Not Stage D.</summary>
public sealed class UsCcssG3Unit3Lesson6FlexibleAdditionSourceTests
{
    const string Code="PED:US-CCSS-MATH:G3:U03:L06";
    static Grade3CompanionLesson Lesson(){Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code,out var l));return Assert.IsType<Grade3CompanionLesson>(l);}
    static Grade3OriginalQuestion Task(string id)=>Lesson().SourceSubtasks.Single(q=>q.Id.EndsWith(id,StringComparison.Ordinal));
    [Theory]
    [InlineData("WU-1","456",true)]
    [InlineData("WU-2","455",true)]
    [InlineData("WU-3","459",true)]
    [InlineData("WU-4","580",true)]
    [InlineData("A1-0","943",true)]
    [InlineData("A1-1","781",true)]
    [InlineData("A1-2","654",true)]
    [InlineData("A1-3","802",true)]
    [InlineData("A1-4","710",true)]
    [InlineData("A2-1","547",true)]
    [InlineData("A2-2","623",true)]
    [InlineData("A2-3","278",true)]
    [InlineData("A2-4","514",true)]
    [InlineData("A2-5","898",true)]
    [InlineData("A1-0","843",false)]
    [InlineData("A1-3","792",false)]
    [InlineData("A2-5","900",false)]
    [InlineData("A2-5","898.0",false)]
    [InlineData("A2-5","",false)]
    public void SourceAdditionIsExactAndMissedCompositionsFail(string id,string answer,bool expected)
    {
        var r=Grade3SourceCompanionCatalogue.Grade(Task(id),answer);
        Assert.Equal(expected,r.NumericCorrect);Assert.True(r.TeacherReview);
    }
    [Fact]
    public void DiscussionAndOriginalCoolDownRemainTeacherReviewed()
    {
        var l=Lesson();Assert.False(l.ReadyForStageD);Assert.False(l.OriginalStudentTaskPdfRead);
        Assert.Equal(18,l.SourceSubtasks.Count);
        foreach(var q in l.SourceSubtasks.Where(x=>x.AnswerType=="teacher"))
        {
            Assert.Null(Grade3SourceCompanionCatalogue.Grade(q,"943").NumericCorrect);
            Assert.True(Grade3SourceCompanionCatalogue.Grade(q,"943").TeacherReview);
        }
    }
    [Fact]
    public void GuardedMaterializedTeachingIncludesBothMethodsAndSourceSums()
    {
        var a=typeof(CanonicalLessonContentPackDocument).Assembly;
        var resource=a.GetManifestResourceNames().Single(x=>x.EndsWith("us-ccss-math-g3-phase29-v1.lesson-content-pack.json",StringComparison.Ordinal));
        using var stream=a.GetManifestResourceStream(resource)!;
        var opts=new JsonSerializerOptions(JsonSerializerDefaults.Web);
        opts.Converters.Add(new JsonStringEnumConverter());
        var pack=JsonSerializer.Deserialize<CanonicalLessonContentPackDocument>(stream,opts)!;
        var lesson=pack.Lessons.Single(x=>x.LessonCode==Code);
        CommonCoreG3SourceTeachingCorrections.Apply(pack);
        var en=lesson.Translations.Single(x=>x.CultureCode=="en");
        foreach(var s in new[]{"300+156=456","657+286=943","602+179=781","438+364=802","199+348=547","399+499=898","MLR8","hundreds columns"})
            Assert.Contains(s,en.Explanation);
        lesson.CanonicalBodySha256=new string('0',64);
        Assert.False(CommonCoreG3SourceTeachingCorrections.IsTarget(pack,lesson));
    }
}

using System.Text.Json;
using System.Text.Json.Serialization;
using Edulytics.Core.Curriculum;
using Edulytics.Data.Seeding;
using Edulytics.Web.Curriculum.Grade3;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Authoring-only Grade 3 Unit 3 Lesson 3 checks; NOT EXECUTED.</summary>
public sealed class UsCcssG3Unit3Lesson3AddYourWaySourceTests
{
    const string Code = "PED:US-CCSS-MATH:G3:U03:L03";
    private static Grade3CompanionLesson Lesson()
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code,out var lesson));
        return Assert.IsType<Grade3CompanionLesson>(lesson);
    }
    private static Grade3OriginalQuestion Task(string id)=>
        Lesson().SourceSubtasks.Single(q=>q.Id.EndsWith(id,StringComparison.Ordinal));

    [Theory]
    [InlineData("WU-1","247",true)]
    [InlineData("WU-2","352",true)]
    [InlineData("WU-3","652",true)]
    [InlineData("WU-4","647",true)]
    [InlineData("A1-1","427",true)]
    [InlineData("A1-2","353",true)]
    [InlineData("A1-3","394",true)]
    [InlineData("A1-4","603",true)]
    [InlineData("WU-3","642",false)]
    [InlineData("A1-3","384",false)]
    [InlineData("A1-4","605",false)]
    [InlineData("A1-4","603.0",false)]
    [InlineData("A1-4","",false)]
    public void SourceArithmeticAnswersAreExact(string id,string answer,bool expected)
    {
        var r=Grade3SourceCompanionCatalogue.Grade(Task(id),answer);
        Assert.Equal(expected,r.NumericCorrect);
        Assert.True(r.TeacherReview);
    }

    [Fact]
    public void ExplanationAndChoiceCannotBeAutoPassed()
    {
        Assert.Equal(14,Lesson().SourceSubtasks.Count);
        Assert.False(Lesson().OriginalStudentTaskPdfRead);
        Assert.False(Lesson().ReadyForStageD);
        foreach(var q in Lesson().SourceSubtasks.Where(q=>q.AnswerType=="teacher"))
        {
            var r=Grade3SourceCompanionCatalogue.Grade(q,"394");
            Assert.Null(r.NumericCorrect);
            Assert.True(r.TeacherReview);
        }
        Assert.Contains("choice of three",Lesson().TeacherReviewNote,StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GuardedTeachingUsesBothPlaceValueOrdersWithoutImposingAnAlgorithm()
    {
        var a=typeof(CanonicalLessonContentPackDocument).Assembly;
        var res=a.GetManifestResourceNames().Single(x=>x.EndsWith("us-ccss-math-g3-phase29-v1.lesson-content-pack.json",StringComparison.Ordinal));
        using var s=a.GetManifestResourceStream(res)!;
        var opts=new JsonSerializerOptions(JsonSerializerDefaults.Web);
        opts.Converters.Add(new JsonStringEnumConverter());
        var pack=JsonSerializer.Deserialize<CanonicalLessonContentPackDocument>(s,opts)!;
        var item=pack.Lessons.Single(x=>x.LessonCode==Code);
        CommonCoreG3SourceTeachingCorrections.Apply(pack);
        var en=item.Translations.Single(x=>x.CultureCode=="en");
        foreach(var value in new[]{"200+40+7=247","40+600+12=652","325+102=427","298+305=603","300+80+14=394","14+80+300=394","MLR6"})
            Assert.Contains(value,en.Explanation);
        Assert.Contains("No single algorithm is imposed",en.Explanation);
        item.CanonicalBodySha256=new string('0',64);
        Assert.False(CommonCoreG3SourceTeachingCorrections.IsTarget(pack,item));
    }
}

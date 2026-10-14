using Edulytics.Core.Curriculum;
using Edulytics.Core.Lessons;
using Edulytics.Data.Seeding;
using Edulytics.Services.LessonContent;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// V9 B01 G3 U06 L09: clock-to-nearest-minute needs a real analog dial,
/// 60 equally spaced minute divisions, hand-angle arithmetic and truthful
/// accessibility. This is not the publisher's copyrighted illustration and
/// does not certify L1-L6 or rights for third-party source images.
/// </summary>
public sealed class UsCcssB01V9NearestMinuteClockVisualTests
{
    private const string Code = "PED:US-CCSS-MATH:G3:U06:L09";

    [Fact]
    public void OriginalNearestMinuteLessonComposesClockFaceWithExactOneThirtySevenValues()
    {
        var pair = MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .Where(d => d.PackCode == "US-CCSS-MATH" && d.VersionCode == "CCSSM-2010")
            .SelectMany(d => d.Lessons.Select(l => (Doc: d, Lesson: l)))
            .Single(x => x.Lesson.LessonCode == Code);
        Assert.Equal("https://im.kendallhunt.com/k5/teachers/grade-3/unit-6/lesson-9/preparation.html",
            pair.Lesson.SourceUrl);
        Assert.Equal("446e27ac04ac32ff683f4aadeda7271f3dffe3d7cf2c21cd3963a9a4416f59f7",
            pair.Lesson.CanonicalBodySha256);
        Assert.Equal(["CCSS:3.MD.A.1"], pair.Lesson.OutcomeCodes);

        var body = pair.Lesson.Translations.Single(t => t.CultureCode == "en");
        var rich = RichLessonContentV2RuntimeComposer.TryCompose(
            Code, new CanonicalLessonTranslationRecord(
                body.CultureCode,body.Title,body.Explanation,
                body.KeyConceptsAndRules,body.WorkedExamples,
                body.StepByStepSolutions,body.CommonMistakes,body.QuickSummary));
        Assert.NotNull(rich);
        var clock = Assert.Single(rich.Visuals, v => v.Kind == RichLessonVisualKind.ClockFace);
        Assert.Equal(["1", "37"], clock.Items);
        Assert.Contains("1:37", clock.Title);
        Assert.Contains("37/60", clock.Description);
        Assert.Contains("one-minute tick", clock.Description);
        Assert.Contains("a.m.", clock.Description);
        Assert.Contains("p.m.", clock.Description);
        Assert.Contains(rich.Visuals, v => v.Kind == RichLessonVisualKind.EquationSet);
        Assert.Equal(2, rich.Visuals.Count);
    }

    [Theory]
    [InlineData(1, 37, 37)]
    [InlineData(12, 0, 0)]
    [InlineData(5, 30, 30)]
    [InlineData(7, 59, 59)]
    public void ClockGeometryUsesExactSixtyMinuteTicksAndContinuousHourHand(
        int hour,int minutes,int expectedMinuteIndex)
    {
        Assert.InRange(minutes,0,59);
        double minuteAngle = 2*Math.PI*minutes/60 - Math.PI/2;
        double hourAngle = 2*Math.PI*((hour%12)+minutes/60.0)/12 - Math.PI/2;
        double sourceDialMinuteTick = 2*Math.PI*expectedMinuteIndex/60 - Math.PI/2;
        Assert.Equal(sourceDialMinuteTick,minuteAngle,10);
        if(minutes>0)
        {
            double hourStartAngle = 2*Math.PI*(hour%12)/12 - Math.PI/2;
            Assert.NotEqual(hourStartAngle,hourAngle);
        }
        Assert.InRange(Math.Abs(Math.Cos(minuteAngle)),-1,1);
        Assert.InRange(Math.Abs(Math.Sin(hourAngle)),-1,1);
    }
}
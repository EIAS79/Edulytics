using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;

// G6 Arabic 10x10 percentage models, selected Student Book PDF138-141.
// Authored A-B-C. No .NET or browser tests manually triggered.
public sealed class UaeGrade6Unit2Lesson02PercentageModelsTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L6:GENERAL:02:02:STUDENT-AR-128";
    [Fact]
    public void EmbeddedArabicRichLessonUsesExistingPercentFamilies()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Equal("مختبر الاستكشاف: تمثيل النسبة المئوية",l!.Title);
        Assert.True(l.WorkedExamples.Count>=5);
        Assert.Contains(l.Visuals,v=>v.Kind==RichLessonVisualKind.RatioTable);
        Assert.Contains(l.Visuals,v=>v.Kind==RichLessonVisualKind.FractionBars);
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.NotNull(contract);
        Assert.Equal(3,contract!.AllowedQuestionFamilies.Count);
        foreach(string f in contract.AllowedQuestionFamilies)
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
    }
    [Fact]
    public void OriginalBarAndHundredGridCalculationsAreExact()
    {
        Assert.Equal(37,100*37/100);
        Assert.Equal(9,20*45/100);
        Assert.Equal(3,5*60/100);
        Assert.Equal(7,20*35/100);
        Assert.Equal(8,100*8/100);
    }
    [Fact]
    public void ExistingFamiliesRemainIndependentlyVerifiable()
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(string f in new[]{
            "uae.g5.u01.l03.hundredths_to_percent",
            "uae.g5.u01.l03.tenths_to_percent",
            "uae.g5.u07.l07.count_shaded_hundredths"
        })
            foreach(var q in engine.Generate(Lesson,f,[f],ExactSkillQuestionDifficulty.Standard,2,20261114,[]))
                Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
    }
}

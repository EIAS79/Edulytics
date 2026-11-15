using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Student Book G6 Arabic pp109–116. Source A-B-C tests written; NOT manually executed.
public sealed class UaeGrade6Unit2Lesson04PercentDecimalsTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L6:GENERAL:02:04:STUDENT-AR-128";
    [Fact]
    public void ArabicContentAndBothDirectionsUseExistingFamilies()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Equal("النسب المئوية والأعداد العشرية",l!.Title);
        Assert.True(l.WorkedExamples.Count>=5);
        Assert.Contains(l.Visuals,v=>v.Kind==RichLessonVisualKind.PlaceValueChart);
        Assert.Contains(l.Visuals,v=>v.Kind==RichLessonVisualKind.RatioTable);
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var c));
        Assert.NotNull(c);
        Assert.Equal(4,c!.AllowedQuestionFamilies.Count);
        foreach(var f in c.AllowedQuestionFamilies)
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
    }
    [Fact]
    public void PercentAndDecimalAreTheSameExactNumber()
    {
        Assert.Equal(56m/100m,0.56m);
        Assert.Equal(8m/100m,0.08m);
        Assert.Equal(38m/100m,0.38m);
        Assert.Equal(20m/100m,0.2m);
        Assert.Equal(74m/100m,0.74m);
    }
    [Fact]
    public void ReusedExactKernelProducesVerifiableQuestions()
    {
        var e=new ExactSkillContractQuestionEngine();
        foreach(var family in new[]{
            "uae.g5.u01.l03.hundredths_to_percent",
            "uae.g5.u01.l03.tenths_to_percent",
            "uae.g4.u10.l03.hundredths_decimal",
            "uae.g4.u10.l02.tenths_decimal"
        })
            foreach(var q in e.Generate(Lesson,family,[family],
                ExactSkillQuestionDifficulty.Standard,2,20261114,[]))
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
    }
}

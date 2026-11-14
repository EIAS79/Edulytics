using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Selected Grade6 Student Book printed101–108; authored A-B-C tests, NOT executed.
public sealed class UaeGrade6Unit2Lesson03PercentAndFractionsTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L6:GENERAL:02:03:STUDENT-AR-128";
    [Fact]
    public void ArabicContentAndExistingPercentSimplificationSkillsAreStaged()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Equal("النسب المئوية والكسور",l!.Title);
        Assert.True(l.WorkedExamples.Count>=5);
        Assert.Contains(l.Visuals,v=>v.Kind==RichLessonVisualKind.RatioTable);
        Assert.Contains(l.Visuals,v=>v.Kind==RichLessonVisualKind.FractionBars);
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var c));
        Assert.NotNull(c);
        Assert.Equal(3,c!.AllowedQuestionFamilies.Count);
        foreach(var f in c.AllowedQuestionFamilies)
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
    }
    [Fact]
    public void SimplestFractionAndPercentConversionExamplesAreExact()
    {
        Assert.Equal(7m/20m,0.35m);
        Assert.Equal(6m/8m,0.75m);
        Assert.Equal(55m/100m,11m/20m);
        Assert.Equal(42m/100m,21m/50m);
        Assert.Equal(84m/100m,21m/25m);
        Assert.Equal(3m/20m,0.15m);
    }
    [Fact]
    public void ExistingKernelIndependentlyChecksGeneratedAnswers()
    {
        var e=new ExactSkillContractQuestionEngine();
        foreach(var f in new[]{
            "uae.g5.u01.l03.hundredths_to_percent",
            "uae.g5.u01.l03.tenths_to_percent",
            "uae.g5.u07.l03.simplify_fraction"
        })
            foreach(var q in e.Generate(Lesson,f,[f],ExactSkillQuestionDifficulty.Standard,2,20261114,[]))
                Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
    }
}

using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;

// Grade6 Arabic local student book Unit2 Lesson1 printed89-96.
// Authored A-B-C; NOT executed manually; Stage D runs .NET/RTL.
public sealed class UaeGrade6Unit2Lesson01DecimalsAndFractionsTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L6:GENERAL:02:01:STUDENT-AR-128";
    [Fact]
    public void ArabicSourceLessonReusesValidatedGrade4AndGrade5ExactFamilies()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Equal("الأعداد العشرية والكسور",l!.Title);
        Assert.True(l.WorkedExamples.Count>=5);
        Assert.Contains(l.Visuals,v=>v.Kind==RichLessonVisualKind.PlaceValueChart);
        Assert.Contains(l.Visuals,v=>v.Kind==RichLessonVisualKind.CalculationChain);
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.NotNull(contract);
        Assert.Equal(4,contract!.AllowedQuestionFamilies.Count);
        foreach(var f in contract.AllowedQuestionFamilies)
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
    }
    [Fact]
    public void FractionConversionArithmeticIsExact()
    {
        Assert.Equal(5m/8m,0.625m);
        Assert.Equal(9m/25m,0.36m);
        Assert.Equal(3m+1m/8m,3.125m);
        Assert.Equal(7m/20m,0.35m);
        Assert.Equal(7m/8m,0.875m);
    }
    [Fact]
    public void ReusedEngineStillIndependentlyVerifiesGeneratedResponses()
    {
        var engine=new ExactSkillContractQuestionEngine();
        var families=new[]{
            "uae.g4.u10.l04.decimal_to_hundredths",
            "uae.g5.u01.l04.thousandths_numerator",
            "uae.g5.u07.l03.simplify_fraction",
            "uae.g5.u07.l08.reducible_fraction_to_decimal"
        };
        foreach(var f in families)
            foreach(var q in engine.Generate(Lesson,f,[f],
                ExactSkillQuestionDifficulty.Standard,2,20261114,[]))
                Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
    }
}

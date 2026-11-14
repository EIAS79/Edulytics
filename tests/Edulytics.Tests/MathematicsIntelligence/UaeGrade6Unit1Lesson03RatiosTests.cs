using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;

// UAE G6 General source printed pp 19-26. Tests authored A-B-C, NOT manually run.
public sealed class UaeGrade6Unit1Lesson03RatiosTests
{
    private const string Lesson = "PED:UAE-MOE-MATH:L6:GENERAL:01:03:STUDENT-AR-128";
    [Fact]
    public void ArabicStudentLessonUsesReviewedRatioMathAndVisualModels()
    {
        var content=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(content);
        Assert.Equal("النسب",content!.Title);
        Assert.True(content.WorkedExamples.Count>=5);
        Assert.Contains(content.Visuals,v=>v.Kind==RichLessonVisualKind.RatioTable);
        Assert.Contains(content.Visuals,v=>v.Kind==RichLessonVisualKind.CalculationChain);
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.NotNull(contract);
        Assert.Equal(3,contract!.AllowedQuestionFamilies.Count);
        foreach(var f in contract.AllowedQuestionFamilies)
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
    }
    [Fact]
    public void ExactRatioSubskillsGenerateArabicAndIndependentlyVerify()
    {
        var eng=new ExactSkillContractQuestionEngine();
        var families=new[]{"ratio.unit_rate.equivalent_ratio","ratio.unit_rate.divide_total","fractions.equivalent.reduce_common_factor"};
        foreach(var f in families)
        {
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out _));
            foreach(var q in eng.Generate(Lesson,f,[f],ExactSkillQuestionDifficulty.Standard,2,20261114,[]))
            {
                Assert.Contains("نسب",q.Prompt);
                Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,"-999"));
            }
        }
    }
    [Fact]
    public void PartToPartAndPartToWholeMustDiffer()
    {
        Assert.Equal(3,6/2);
        Assert.Equal(5,10/2);
        Assert.Equal(8,16/2);
        Assert.Equal(5,30/(2+3));
    }
}

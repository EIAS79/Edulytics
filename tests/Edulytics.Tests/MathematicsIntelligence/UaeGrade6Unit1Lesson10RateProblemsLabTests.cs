using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;

// G6 local student edition Unit1 exploration lab 3 pp67–70.
// Source A-B-C tests authored, not manually executed; browser/CI evidence Stage D.
public sealed class UaeGrade6Unit1Lesson10RateProblemsLabTests
{
    private const string Lesson = "PED:UAE-MOE-MATH:L6:GENERAL:01:10:STUDENT-AR-128";
    [Fact]
    public void ContentAndExistingExactRateFamiliesAreStagedForArabicStudent()
    {
        var content=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(content);
        Assert.Equal("مختبر الاستكشاف: مسائل النسب والمعدلات",content!.Title);
        Assert.True(content.WorkedExamples.Count >= 5);
        Assert.Contains(content.Visuals,v=>v.Kind==RichLessonVisualKind.RatioTable);
        Assert.Contains(content.Visuals,v=>v.Kind==RichLessonVisualKind.DoubleNumberLine);
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.NotNull(contract);
        Assert.Equal(2,contract!.AllowedQuestionFamilies.Count);
        foreach(var family in contract.AllowedQuestionFamilies)
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
    }
    [Fact]
    public void ExistingNumericVerifiersRespectUnitRateAndScale()
    {
        const string unitFamily="ratio.unit_rate.direct";
        var rate=new Dictionary<string,int>{{"total",144},{"quantity",3}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(unitFamily,rate,"48"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(unitFamily,rate,"52"));
        const string equivalent="ratio.unit_rate.equivalent_ratio";
        var scaling=new Dictionary<string,int>{{"baseFirst",48},{"baseSecond",1},{"targetSecond",5}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(equivalent,scaling,"240"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(equivalent,scaling,"245"));
    }
    [Fact]
    public void ReusedGeneratorEmitsLocalizedQuestionsOnlyForThisGrade6Lesson()
    {
        var generator=new ExactSkillContractQuestionEngine();
        foreach(string family in new[]{"ratio.unit_rate.direct","ratio.unit_rate.equivalent_ratio"})
        {
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out _));
            foreach(var item in generator.Generate(Lesson,family,[family],ExactSkillQuestionDifficulty.Standard,2,20261114,[]))
            {
                Assert.True(item.Prompt.Contains("الوحدة")||item.Prompt.Contains("نسب"));
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,item.CorrectAnswer));
            }
        }
    }
}

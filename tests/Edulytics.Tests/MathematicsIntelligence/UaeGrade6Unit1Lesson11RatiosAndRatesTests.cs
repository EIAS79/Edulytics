using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;

// Source-aligned G6 student-book native Lesson7, indexed element11: PDF112–119.
// Authored A-B-C; no manual .NET invocation, acceptance remains Stage D.
public sealed class UaeGrade6Unit1Lesson11RatiosAndRatesTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L6:GENERAL:01:11:STUDENT-AR-128";
    [Fact]
    public void SourceOrderedArabicLessonUsesOriginalVisualsAndExistingExactPractice()
    {
        var content=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(content);
        Assert.Equal("مسائل النسب والمعدلات",content!.Title);
        Assert.True(content.WorkedExamples.Count>=5);
        Assert.Contains(content.Visuals,v=>v.Kind==RichLessonVisualKind.RatioTable);
        Assert.Contains(content.Visuals,v=>v.Kind==RichLessonVisualKind.DoubleNumberLine);
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.NotNull(contract);
        Assert.Equal(3,contract!.AllowedQuestionFamilies.Count);
        foreach(string f in contract.AllowedQuestionFamilies)
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
    }
    [Theory]
    [InlineData("ratio.unit_rate.direct",150,5,30)]
    [InlineData("ratio.unit_rate.direct",108,3,36)]
    public void ExistingPerUnitVerificationRejectsOffByOne(
        string family,int total,int quantity,int answer)
    {
        var parameters=new Dictionary<string,int>{{"total",total},{"quantity",quantity}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,parameters,answer.ToString()));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,parameters,(answer+1).ToString()));
    }
    [Fact]
    public void ArabicScopedGenerationPreservesExactVerification()
    {
        var generator=new ExactSkillContractQuestionEngine();
        foreach(var family in new[]{"ratio.unit_rate.direct","ratio.unit_rate.equivalent_ratio","ratio.unit_rate.divide_total"})
        {
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out _));
            foreach(var q in generator.Generate(Lesson,family,[family],
                ExactSkillQuestionDifficulty.Standard,2,20261114,[]))
            {
                Assert.True(q.Prompt.Contains("وحدة") || q.Prompt.Contains("نسب"));
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
            }
        }
    }
}

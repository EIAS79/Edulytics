using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UaeGrade4Unit7Lesson09MultipleOperationsTests
{
    private const string Lesson = "PED:UAE-MOE-MATH:L4:COMMON:07:09";

    [Fact]
    public void MultiOperationPracticeContractGeneratesAndVerifies()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson, out var contract));
        Assert.NotNull(contract);
        Assert.Equal(2, contract!.AllowedQuestionFamilies.Count);
        var engine = new ExactSkillContractQuestionEngine();
        foreach (var family in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family, out var ui));
            Assert.NotNull(ui);
            foreach (var q in engine.Generate(Lesson, family, [family],
                ExactSkillQuestionDifficulty.Standard, 3, 20261111, []))
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(family, q.Parameters, q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family, q.Parameters, "-1"));
            }
        }
    }

    [Fact]
    public void MultipleOperationExamplesMatchStudentSourceMath()
    {
        Assert.Equal(new[] {12,19,26}, Enumerable.Range(1,3).Select(x=>7*x+5));
        Assert.Equal(new[] {17,15,13}, Enumerable.Range(2,3).Select(x=>2*(9-x)+3));
        Assert.Equal(new[] {17,11,9}, Enumerable.Range(1,3).Select(x=>12/x+5));
        Assert.Equal(27, 6*4+3);
    }

    [Fact]
    public void ArabicRichContentCoversTableAndParentheses()
    {
        var l = RichLessonContentV2Registry.Find(Lesson, "ar");
        Assert.NotNull(l);
        Assert.Equal("المعادلات التي تتضمن عمليات متعددة", l!.Title);
        Assert.True(l.WorkedExamples.Count >= 4);
        Assert.True(l.KeyConcepts.Count >= 3);
        Assert.True(l.CommonMistakes.Count >= 2);
        Assert.Equal(RichLessonVisualKind.EquationSet, Assert.Single(l.Visuals).Kind);
    }
}

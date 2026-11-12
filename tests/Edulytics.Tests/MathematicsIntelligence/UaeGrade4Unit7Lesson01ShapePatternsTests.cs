using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit7Lesson01ShapePatternsTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:07:01";
    [Fact]
    public void PatternFamiliesGenerateAndVerify()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal(2,contract!.AllowedQuestionFamilies.Count);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var family in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
            Assert.NotNull(ui);
            foreach(var q in engine.Generate(Lesson,family,[family],ExactSkillQuestionDifficulty.Standard,3,20261111,[]))
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"-1"));
            }
        }
    }
    [Fact]
    public void RepeatingStarsAndMoonsHaveThreeSymbolUnit()
    {
        var p=new Dictionary<string,int>{{"period",3},{"position",7}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u07.l01.repeating_pattern_unit_length",p,"٣"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u07.l01.repeating_pattern_symbol_position",p,"١"));
        p["position"]=8;
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u07.l01.repeating_pattern_symbol_position",p,"٢"));
    }
    [Fact]
    public void RichArabicShowsSymbolicPatterns()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Equal("الأنماط غير العددية",l!.Title);
        Assert.True(l.WorkedExamples.Count>=3);
        Assert.Equal(RichLessonVisualKind.EquationSet,Assert.Single(l.Visuals).Kind);
    }
}

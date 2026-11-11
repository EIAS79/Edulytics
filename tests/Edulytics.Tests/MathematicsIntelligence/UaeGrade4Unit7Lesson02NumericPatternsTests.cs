using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit7Lesson02NumericPatternsTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:07:02";
    [Fact]
    public void NumericPatternFamiliesGenerateAndVerify()
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
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"-2"));
            }
        }
    }
    [Fact]
    public void AlternatingAndConstantPatternsHaveExpectedNextTerms()
    {
        var p=new Dictionary<string,int>{{"start",26},{"step",4},{"index",6}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u07.l02.constant_numeric_step",p,"٤٦"));
        p["start"]=32;p["step"]=5;p["index"]=7;
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u07.l02.alternating_numeric_rule",p,"٣٨"));
    }
    [Fact]
    public void ArabicNumericalPatternsRichContentExists()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Equal("الأنماط العددية",l!.Title);
        Assert.True(l.WorkedExamples.Count>=3);
        Assert.Equal(RichLessonVisualKind.EquationSet,Assert.Single(l.Visuals).Kind);
    }
}

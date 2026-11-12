using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit7Lesson05AddSubtractRulesTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:07:05";
    [Fact]
    public void InputOutputRulesGenerateAndValidate()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal(2,contract!.AllowedQuestionFamilies.Count);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var f in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
            Assert.NotNull(ui);
            foreach(var q in engine.Generate(Lesson,f,[f],ExactSkillQuestionDifficulty.Standard,3,20261111,[]))
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,"-1"));
            }
        }
    }
    [Fact]
    public void StudentInputOutputExamplesCorrect()
    {
        var p=new Dictionary<string,int>{{"input",21},{"change",7},{"factor",3}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u07.l05.input_add_rule",p,"٢٨"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u07.l05.input_subtract_rule",p,"١٤"));
        p["input"]=24;
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u07.l05.input_add_rule",p,"٣١"));
    }
    [Fact]
    public void RichArabicContentDefinesRuleTables()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Equal("قواعد الجمع والطرح",l!.Title);
        Assert.True(l.WorkedExamples.Count>=3);
        Assert.Equal(RichLessonVisualKind.EquationSet,Assert.Single(l.Visuals).Kind);
    }
}

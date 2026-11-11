using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit7Lesson06MultiplyDivideRulesTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:07:06";
    [Fact]
    public void MultiplicationAndDivisionRulesGenerate()
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
    public void StudentValuesMatchRules()
    {
        var p=new Dictionary<string,int>{{"input",24},{"change",3},{"factor",6}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u07.l06.input_multiply_rule",p,"١٤٤"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u07.l06.input_divide_rule",p,"٢٤"));
    }
    [Fact]
    public void RichContentListsTables()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Equal("قواعد الضرب والقسمة",l!.Title);
        Assert.True(l.WorkedExamples.Count>=3);
        Assert.Equal(RichLessonVisualKind.EquationSet,Assert.Single(l.Visuals).Kind);
    }
}

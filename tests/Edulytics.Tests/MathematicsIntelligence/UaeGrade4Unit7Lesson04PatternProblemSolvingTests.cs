using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit7Lesson04PatternProblemSolvingTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:07:04";
    [Fact]
    public void ReusedNumericFamiliesWorkForProblemSolving()
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
    public void BookDailyTicketPatternMatchesModel()
    {
        var p=new Dictionary<string,int>{{"start",32},{"step",4},{"index",5}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u07.l02.constant_numeric_step",p,"٤٨"));
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g4.u07.l02.constant_numeric_step",p,"٤٤"));
    }
    [Fact]
    public void RichArabicIncludesStoryProblems()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Contains("البحث عن نمط",l!.Title);
        Assert.True(l.WorkedExamples.Count>=3);
        Assert.Equal(RichLessonVisualKind.EquationSet,Assert.Single(l.Visuals).Kind);
    }
}

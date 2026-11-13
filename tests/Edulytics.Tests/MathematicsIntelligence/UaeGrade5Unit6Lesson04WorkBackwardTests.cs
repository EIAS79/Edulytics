using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Grade5 Arabic student 02-1.pdf printed423–428; authored StageD tests not run.
public sealed class UaeGrade5Unit6Lesson04WorkBackwardTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L5:GENERAL:06:04";
    [Fact] public void WorkBackwardReusesThreeExistingExactFourStepFamilies()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal("problem_solving.four_steps_with_whole_numbers",contract!.SkillId);
        Assert.Equal(3,contract.AllowedQuestionFamilies.Count);
        var e=new ExactSkillContractQuestionEngine();
        foreach(var f in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
            Assert.NotNull(ui);
            foreach(var q in e.Generate(Lesson,f,[f],ExactSkillQuestionDifficulty.Standard,2,20261113,[]))
                Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
        }
    }
    [Fact] public void InverseAndStartingQuantityContractsMatchStudentObjective()
    {
        var p=new Dictionary<string,int>{{"a",3},{"b",6},{"c",4},{"d",13}};
        const string f="uae.g5.u01.l09.undo_four_steps";
        Assert.True(ExactSkillContractQuestionEngine.Verify(f,p,"٣٣"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(f,p,"11"));
        var initial=new Dictionary<string,int>{{"a",24},{"b",18},{"c",27},{"d",15}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u01.l09.find_starting_total",initial,"٨٤"));
    }
}
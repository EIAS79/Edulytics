using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Stage ABC tests WRITTEN_NOT_RUN; local Arabic student printed 105–110.
public sealed class UaeGrade5Unit2Lesson05MakeTableTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L5:GENERAL:02:05";
    [Fact] public void AllThreeSourceTableFamiliesRouteToExistingEngine()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal("problem_solving.four_steps_with_whole_numbers",contract!.SkillId);
        Assert.Equal(3,contract.AllowedQuestionFamilies.Count);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var family in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
            Assert.NotNull(ui);
            foreach(var q in engine.Generate(Lesson,family,[family],ExactSkillQuestionDifficulty.Standard,2,20261113,[]))
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"-1"));
                Assert.DoesNotContain("result",q.Parameters.Keys);
            }
        }
    }
    [Fact] public void TableExamplesRecomputeCorrectly()
    {
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u02.l05.table_population_scale",new Dictionary<string,int>{{"a",63},{"b",100},{"c",10000}},"٦٣٠٠"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u02.l05.table_weekly_doubling",new Dictionary<string,int>{{"a",1},{"b",7},{"c",2}},"64"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u02.l05.table_constant_rate",new Dictionary<string,int>{{"a",160},{"b",10},{"c",60}},"960"));
    }
}

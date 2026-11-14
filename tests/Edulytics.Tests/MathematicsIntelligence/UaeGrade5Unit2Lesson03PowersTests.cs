using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Source Grade5 Arabic student pp93–98; StageD tests NOT_RUN.
public sealed class UaeGrade5Unit2Lesson03PowersTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L5:GENERAL:02:03";
    [Fact] public void PowerFamiliesRouteToSharedExactEngine()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal("supporting.indices.roots_standard_form",contract!.SkillId);
        Assert.Equal(3,contract.AllowedQuestionFamilies.Count);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var f in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));Assert.NotNull(ui);
            foreach(var q in engine.Generate(Lesson,f,[f],ExactSkillQuestionDifficulty.Standard,2,20261113,[]))
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
                Assert.DoesNotContain("result",q.Parameters.Keys);
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,"-1"));
            }
        }
    }
    [Fact] public void PowersAndCountAndCubeMatchSource()
    {
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u02.l03.evaluate_integer_power",new Dictionary<string,int>{{"base",3},{"exp",4},{"mode",0}},"٨١"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u02.l03.count_repeated_factor",new Dictionary<string,int>{{"base",3},{"product",81},{"mode",1}},"٤"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u02.l03.cube_volume",new Dictionary<string,int>{{"base",18},{"mode",2}},"٥٨٣٢"));
    }
}

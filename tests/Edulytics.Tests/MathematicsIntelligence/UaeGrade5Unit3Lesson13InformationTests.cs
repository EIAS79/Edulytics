using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Grade5 Arabic source pp233–238; Stage A-B-C authored, Stage D NOT_RUN.
public sealed class UaeGrade5Unit3Lesson13InformationTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L5:GENERAL:03:13";
    [Fact]
    public void ExtraOrMissingFactsRouteThroughExistingFourStepEngine()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal("problem_solving.identify_extra_or_missing_information",contract!.SkillId);
        Assert.Equal(3,contract.AllowedQuestionFamilies.Count);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var family in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
            Assert.NotNull(ui);
            foreach(var item in engine.Generate(Lesson,family,[family],ExactSkillQuestionDifficulty.Standard,2,20261113,[]))
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,item.CorrectAnswer));
        }
    }
    [Fact]
    public void ExtraFactsDoNotAlterAnswerAndMissingStudentCountIsRecognized()
    {
        var p=new Dictionary<string,int>{{"a",8},{"b",12},{"c",5}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u03.l13.ignore_extra_and_divide",p,"8"));
        p["c"]=15;
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u03.l13.ignore_extra_and_divide",p,"8"));
        var q=new Dictionary<string,int>{{"a",0},{"b",20},{"c",2}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u03.l13.classify_sufficient_information",q,"0"));
        q["a"]=1;
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u03.l13.classify_sufficient_information",q,"1"));
    }
}

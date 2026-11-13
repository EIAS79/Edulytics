using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Written only at Stage A-B-C. Not run.
public sealed class UaeGrade5Unit9Lesson09UnitFractionDivisionTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L5:GENERAL:09:09";
    [Fact] public void ExactArabicUnitFractionsHaveValidRoutingAndVerification()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal(2,contract!.AllowedQuestionFamilies.Count);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var f in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
            Assert.NotNull(ui);
            foreach(var q in engine.Generate(Lesson,f,[f],ExactSkillQuestionDifficulty.Standard,2,20261113,[]))
            {
                Assert.DoesNotContain("result",q.Parameters.Keys);
                Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,"-999"));
            }
        }
    }
    [Fact] public void SourceShapeIntegerAndUnitDivisionExamples()
    {
        var p=new Dictionary<string,int>{{"n",4},{"d",2}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u09.l09.whole_divide_unit_fraction",p,"٨"));
        p["n"]=2;p["d"]=5;
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u09.l09.unit_fraction_divide_whole",p,"١/١٠"));
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g5.u09.l09.unit_fraction_divide_whole",p,"2/10"));
        p["d"]=0;
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g5.u09.l09.whole_divide_unit_fraction",p,"0"));
    }
}
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Source: local Arabic student 02-1.pdf printed405–410. Tests written for StageD; NOT_RUN in A–B–C.
public sealed class UaeGrade5Unit6Lesson01NumericalExpressionTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L5:GENERAL:06:01";
    [Fact] public void ThreeNumericBarFamiliesRouteThroughExistingAlgebraEngine()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var c));
        Assert.Equal("number.algebra.write_expressions",c!.SkillId);
        Assert.Equal(3,c.AllowedQuestionFamilies.Count);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var f in c.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
            Assert.NotNull(ui);
            foreach(var q in engine.Generate(Lesson,f,[f],ExactSkillQuestionDifficulty.Standard,2,20261113,[]))
                Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
        }
    }
    [Fact] public void BarModelMatchesArithmeticAndRejectsCorruptedParameters()
    {
        const string f="uae.g5.u06.l01.two_group_bar_total";
        var p=new Dictionary<string,int>{{"n",2},{"a",5},{"m",2},{"b",4}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(f,p,"١٨"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(f,p,"14"));
        p["n"]=0;
        Assert.False(ExactSkillContractQuestionEngine.Verify(f,p,"18"));
        const string paired="uae.g5.u06.l01.paired_group_expression_value";
        p["n"]=3;p["a"]=5;p["m"]=3;p["b"]=4;
        Assert.True(ExactSkillContractQuestionEngine.Verify(paired,p,"27"));
        p["m"]=2;
        Assert.False(ExactSkillContractQuestionEngine.Verify(paired,p,"23"));
    }
}
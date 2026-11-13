using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// UAE Grade5 student 02-1.pdf printed pp411–416. Authored StageD test; NOT_RUN in A-B-C.
public sealed class UaeGrade5Unit6Lesson02OrderOperationsTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L5:GENERAL:06:02";
    [Fact] public void AllThreeG5OrderingFamiliesRouteToReusedAlgebraEngine()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var c));
        Assert.Equal("number.algebra.evaluate_expressions",c!.SkillId);
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
    [Fact] public void OrderPowersAndNestedBracketsDoNotPermitWrongAnswers()
    {
        var p=new Dictionary<string,int>{{"a",20},{"b",4},{"c",8},{"d",0}};
        const string basic="uae.g5.u06.l02.multiply_before_add";
        Assert.True(ExactSkillContractQuestionEngine.Verify(basic,p,"٥٢"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(basic,p,"128"));
        p["a"]=20;p["b"]=4;p["c"]=10;p["d"]=2;
        const string nested="uae.g5.u06.l02.nested_parentheses";
        Assert.True(ExactSkillContractQuestionEngine.Verify(nested,p,"11"));
        p["d"]=0;
        Assert.False(ExactSkillContractQuestionEngine.Verify(nested,p,"11"));
        p["a"]=4;p["b"]=2;p["c"]=2;p["d"]=8;
        const string power="uae.g5.u06.l02.exponents_before_products";
        Assert.True(ExactSkillContractQuestionEngine.Verify(power,p,"32"));
    }
}
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Grade 5 local Arabic printed pp347–352; authored Stage D tests NOT_RUN.
public sealed class UaeGrade5Unit5Lesson08MultiplicationPropertiesTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L5:GENERAL:05:08";
    [Fact] public void PropertiesFamiliesReuseRegisteredEngine()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal("number.mult_div_properties",contract!.SkillId);
        Assert.Equal(3,contract.AllowedQuestionFamilies.Count);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var f in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
            Assert.NotNull(ui);
            foreach(var q in engine.Generate(Lesson,f,[f],ExactSkillQuestionDifficulty.Standard,2,20261113,[]))
                Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
        }
    }
    [Fact] public void DecimalAssociativityCommutativityIdentityAreCorrect()
    {
        var p=new Dictionary<string,int>{{"aTenths",17},{"b",5},{"c",2}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u05.l08.associative_decimal_product",p,"١٧"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u05.l08.commutative_decimal_factor",p,"5"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u05.l08.multiplicative_identity",p,"1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g5.u05.l08.multiplicative_identity",p,"0"));
        p["aTenths"]=0;
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g5.u05.l08.associative_decimal_product",p,"0"));
    }
}
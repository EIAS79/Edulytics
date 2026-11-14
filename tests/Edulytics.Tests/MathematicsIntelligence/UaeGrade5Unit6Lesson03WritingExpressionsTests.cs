using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Grade5 Arabic Student 02-1.pdf printed417–422; StageD tests authored NOT_RUN.
public sealed class UaeGrade5Unit6Lesson03WritingExpressionsTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L5:GENERAL:06:03";
    [Fact] public void NumericExpressionFamiliesUseStructuredAnswerContract()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal("number.algebra.write_expressions",contract!.SkillId);
        Assert.Equal(3,contract.AllowedQuestionFamilies.Count);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var f in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var interaction));
            Assert.Equal(LessonPracticeInteractionKind.ContractDriven,interaction!.InteractionKind);
            foreach(var q in engine.Generate(Lesson,f,[f],ExactSkillQuestionDifficulty.Standard,2,20261113,[]))
                Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
        }
    }
    [Fact] public void ExactExpressionsPreserveStructureAndAcceptArabicDigits()
    {
        const string s="uae.g5.u06.l03.write_multiply_then_subtract";
        var p=new Dictionary<string,int>{{"a",4},{"b",7},{"c",3}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(s,p,"٤×٧−٣"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(s,p,"7*4-3"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(s,p,"4*(7-3)"));
        const string d="uae.g5.u06.l03.write_sum_then_divide";
        p["a"]=6;p["b"]=10;p["c"]=4;
        Assert.True(ExactSkillContractQuestionEngine.Verify(d,p,"(٦+١٠)÷٤"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(d,p,"6+10/4"));
        p["c"]=0;
        Assert.False(ExactSkillContractQuestionEngine.Verify(d,p,"(6+10)/0"));
    }
}
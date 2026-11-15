using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG6U6L03NumericalExpressionsTests
{
    const string L="PED:UAE-MOE-MATH:L6:GENERAL:06:03:STUDENT-AR-128";
    [Fact] public void RichContentAndContractsIncludeReusedAndNewFamilies()
    {
        var rich=RichLessonContentV2Registry.Find(L,"ar");
        Assert.NotNull(rich);
        Assert.Equal(10,rich!.WorkedExamples.Count);
        Assert.All(rich.WorkedExamples,x=>Assert.True(x.Steps.Count>=3));
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(L,out var c));
        Assert.Equal(7,c!.AllowedQuestionFamilies.Count);
        Assert.Contains("uae.g4.u07.l07.order_multiply_before_add",c.AllowedQuestionFamilies);
        foreach(var family in c.AllowedQuestionFamilies)
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
    }
    [Fact] public void AdditionSubtractionDoNotReorder()
    {
        var p=new Dictionary<string,int>{{"a",10},{"b",2},{"c",8}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l03.sum_sub_left_right",p,"16"));
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l03.sum_sub_left_right",p,"0"));
    }
    [Fact] public void DivisionMultiplicationAreEqualPriority()
    {
        var p=new Dictionary<string,int>{{"q",8},{"b",2},{"c",4}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l03.division_multiplication_left_right",p,"32"));
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l03.division_multiplication_left_right",p,"2"));
    }
    [Fact] public void BracketsExponentsAndDecimalCostsAreExact()
    {
        var p=new Dictionary<string,int>{{"a",5},{"b",8},{"c",9},{"d",6}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l03.parenthesis_first",p,"29"));
        var exp=new Dictionary<string,int>{{"a",4},{"b",6},{"e",2},{"m",3}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l03.exponent_then_multiply",exp,"١١٢"));
        var cost=new Dictionary<string,int>{{"q1",4},{"p1",50},{"q2",3},{"p2",225}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l03.two_item_cost",cost,"8.75"));
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l03.two_item_cost",cost,"19.25"));
    }
    [Fact] public void FirstOperationClassificationRejectsWrongPriority()
    {
        var p=new Dictionary<string,int>{{"mode",0}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l03.first_operation",p,"الأقواس"));
        p["mode"]=1;
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l03.first_operation",p,"الأسس"));
        p["mode"]=4;
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l03.first_operation",p,"الأسس"));
    }
}

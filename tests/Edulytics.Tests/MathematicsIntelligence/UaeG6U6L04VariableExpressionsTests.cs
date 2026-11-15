using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG6U6L04VariableExpressionsTests
{
    const string L="PED:UAE-MOE-MATH:L6:GENERAL:06:04:STUDENT-AR-128";
    [Fact] public void ArabicLessonExamplesAndContractsExist()
    {
        var rich=RichLessonContentV2Registry.Find(L,"ar");
        Assert.NotNull(rich);
        Assert.Equal(10,rich!.WorkedExamples.Count);
        Assert.All(rich.WorkedExamples,e=>Assert.True(e.Steps.Count>=3));
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(L,out var c));
        Assert.Equal(9,c!.AllowedQuestionFamilies.Count);
        foreach(var family in c.AllowedQuestionFamilies)
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
    }
    [Fact] public void OneVariableAdditionAndSubtractionAreExact()
    {
        var p=new Dictionary<string,int>{{"x",16},{"a",8}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l04.substitute_add",p,"٢٤"));
        p=new(){{"x",64},{"y",27}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l04.substitute_difference",p,"37"));
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l04.substitute_difference",p,"91"));
    }
    [Fact] public void MultiplicationDivisionLinearAndTwoVariableWork()
    {
        var p=new Dictionary<string,int>{{"x",5},{"a",6}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l04.substitute_product",p,"٣٠"));
        var div=new Dictionary<string,int>{{"x",20},{"d",4}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l04.substitute_division",div,"5"));
        div["x"]=7;div["d"]=3;
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l04.substitute_division",div,"2.333333"));
        div["x"]=20;div["d"]=4;
        var lin=new Dictionary<string,int>{{"x",3},{"a",5},{"b",4}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l04.substitute_linear",lin,"19"));
        var two=new Dictionary<string,int>{{"x",4},{"y",9},{"cx",3},{"cy",2}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l04.two_variable_expression",two,"30"));
    }
    [Fact] public void FractionalSquaresAndCubesRetainExactForm()
    {
        var p=new Dictionary<string,int>{{"n",1},{"d",8}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l04.fraction_squared",p,"1/16"));
        p["d"]=2;
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l04.cube_volume",p,"1/8"));
        p["d"]=0;
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l04.cube_volume",p,"0"));
    }
    [Fact] public void ContextCostAndInvalidParametersAreSafe()
    {
        var p=new Dictionary<string,int>{{"initial",200},{"price",30},{"q",4}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l04.money_remaining",p,"٨٠"));
        p["q"]=11;
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l04.money_remaining",p,"-130"));
    }
}

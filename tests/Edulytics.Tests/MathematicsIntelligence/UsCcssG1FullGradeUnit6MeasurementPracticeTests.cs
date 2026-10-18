using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG1FullGradeUnit6MeasurementPracticeTests
{
    [Fact]
    public void SeventeenSourceGoalsHaveDigitalSubskillContracts()
    {
        Assert.Equal(17,UsCcssG1FullGradeUnit6MeasurementQuestions.Families.Count);
        for(int n=1;n<=17;n++)
        {
            var code=$"PED:US-CCSS-MATH:G1:U06:L{n:00}";
            var contract=LessonPracticeContractRegistry.All.Single(x=>x.LessonCode==code);
            var f=Assert.Single(contract.AllowedQuestionFamilies);
            Assert.True(UsCcssG1FullGradeUnit6MeasurementQuestions.Supports(f));
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var choice));
            Assert.Equal(LessonPracticeInteractionKind.ContractDriven,choice!.InteractionKind);
        }
    }
    [Fact]
    public void CorrectAndIncorrectMeasurementStoryAndCountingMathAreVerified()
    {
        foreach(var family in UsCcssG1FullGradeUnit6MeasurementQuestions.Families)
            for(int seed=0;seed<110;seed++)
            {
                var item=UsCcssG1FullGradeUnit6MeasurementQuestions.Build(family,new Random(seed*19+73));
                var answer=UsCcssG1FullGradeUnit6MeasurementQuestions.Solve(family,item.Parameters);
                Assert.True(UsCcssG1FullGradeUnit6MeasurementQuestions.Valid(family,item.Parameters));
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,answer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,answer+";WRONG=1"));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,"DIFFERENCE=-4"));
            }
    }
    [Fact]
    public void ChangingUnitSizeChangesTheMeasureCountNotThePhysicalLength()
    {
        const string f="usccss.g1.u06.l07.unit_length_matters";
        var p=new Dictionary<string,int>{{"a",6},{"b",2},{"c",0},{"mode",0}};
        Assert.Equal("SMALL_UNITS=12",UsCcssG1FullGradeUnit6MeasurementQuestions.Solve(f,p));
        Assert.True(UsCcssG1FullGradeUnit6MeasurementQuestions.Verify(f,p,"SMALL_UNITS=12"));
        Assert.False(UsCcssG1FullGradeUnit6MeasurementQuestions.Verify(f,p,"SMALL_UNITS=6"));
    }
    [Fact]
    public void CenterExpressionsCompareDifferentValuesWithin20()
    {
        const string f="usccss.g1.u06.l04.center_add_compare";
        var p=new Dictionary<string,int>{{"a",31},{"b",6},{"c",0},{"mode",1}};
        Assert.Equal("RELATION=GREATER",UsCcssG1FullGradeUnit6MeasurementQuestions.Solve(f,p));
        var q=UsCcssG1FullGradeUnit6MeasurementQuestions.Build(f,new Random(412));
        // The random branch may choose an addition variant. Both remain
        // individually source-scoped, with a correct and rejectable key.
        Assert.True(UsCcssG1FullGradeUnit6MeasurementQuestions.Valid(f,q.Parameters));
        p["a"]=65;p["b"]=16;
        Assert.Equal("RELATION=LESS",UsCcssG1FullGradeUnit6MeasurementQuestions.Solve(f,p));
        p["a"]=16;p["b"]=1;
        Assert.Equal("RELATION=EQUAL",UsCcssG1FullGradeUnit6MeasurementQuestions.Solve(f,p));
        Assert.False(UsCcssG1FullGradeUnit6MeasurementQuestions.Verify(f,p,"RELATION=GREATER"));
    }
    [Fact]
    public void EngineSupportsTenDistinctDigitalPromptsPerSourceSkill()
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var f in UsCcssG1FullGradeUnit6MeasurementQuestions.Families)
        {
            var rows=engine.Generate("grade1-u06",f,[f],ExactSkillQuestionDifficulty.Standard,10,20261019,[]);
            Assert.Equal(10,rows.Count);
            Assert.Equal(10,rows.Select(x=>x.Prompt).Distinct(StringComparer.Ordinal).Count());
        }
    }
}

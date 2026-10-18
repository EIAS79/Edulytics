using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG1FullGradeUnit4FinalPracticeTests
{
    [Fact]
    public void AllEightLessonFamiliesHaveDedicatedDigitallyGradeableContracts()
    {
        Assert.Equal(8,UsCcssG1FullGradeUnit4FinalQuestions.Families.Count);
        for(int i=16;i<=23;i++)
        {
            var c=LessonPracticeContractRegistry.All.Single(x=>x.LessonCode==$"PED:US-CCSS-MATH:G1:U04:L{i:00}");
            var f=Assert.Single(c.AllowedQuestionFamilies);
            Assert.True(UsCcssG1FullGradeUnit4FinalQuestions.Supports(f));
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var interaction));
            Assert.Equal(LessonPracticeInteractionKind.ContractDriven,interaction!.InteractionKind);
        }
    }
    [Fact]
    public void DigitalTensAndOnesFractionsAndComparisonsRejectFalseResults()
    {
        foreach(var family in UsCcssG1FullGradeUnit4FinalQuestions.Families)
            for(int seed=0;seed<110;seed++)
            {
                var problem=UsCcssG1FullGradeUnit4FinalQuestions.Build(family,new Random(103*seed+19));
                var answer=UsCcssG1FullGradeUnit4FinalQuestions.Solve(family,problem.Parameters);
                Assert.True(UsCcssG1FullGradeUnit4FinalQuestions.Valid(family,problem.Parameters));
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,problem.Parameters,answer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,problem.Parameters,"ANSWER=999"));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,problem.Parameters,answer+";FAKE=1"));
            }
    }
    [Fact]
    public void ANonstandardRegroupingKeepsTheSameTwoDigitQuantity()
    {
        const string f="usccss.g1.u04.l19.regroup_one_ten";
        var p=new Dictionary<string,int>{{"a",43},{"b",0},{"c",0},{"mode",0}};
        Assert.Equal("TENS=3;ONES=13",UsCcssG1FullGradeUnit4FinalQuestions.Solve(f,p));
        Assert.True(UsCcssG1FullGradeUnit4FinalQuestions.Verify(f,p,"TENS=3;ONES=13"));
        Assert.False(UsCcssG1FullGradeUnit4FinalQuestions.Verify(f,p,"TENS=4;ONES=13"));
    }
    [Fact]
    public void EngineCanGenerateTenDifferentIndependentItemsPerFamily()
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var family in UsCcssG1FullGradeUnit4FinalQuestions.Families)
        {
            var rows=engine.Generate("g1-u04-complete",family,[family],
                ExactSkillQuestionDifficulty.Standard,10,20261019,[]);
            Assert.Equal(10,rows.Count);
            Assert.Equal(10,rows.Select(x=>x.Prompt).Distinct(StringComparer.Ordinal).Count());
        }
    }
}

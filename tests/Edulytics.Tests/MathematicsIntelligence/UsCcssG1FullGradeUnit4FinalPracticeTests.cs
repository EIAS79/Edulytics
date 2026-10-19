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
    public void OriginalUnit4Lesson21ComparesThirtyTwoLooseOnesCorrectly()
    {
        const string family="usccss.g1.u04.l21.compare_nonstandard_forms";
        var p=new Dictionary<string,int>{{"a",82},{"b",72},{"c",0},{"mode",2}};
        Assert.True(UsCcssG1FullGradeUnit4FinalQuestions.Valid(family,p));
        var generated=UsCcssG1FullGradeUnit4FinalQuestions.Build(family,new Random(193));
        Assert.DoesNotContain("symbol=>",generated.Prompt,StringComparison.OrdinalIgnoreCase);
        // 3 exchanged tens: 82 = five tens plus thirty-two ones, original
        // first-edition source compares that with seven tens and two ones.
        string prompt=$"Compare {82/10-Math.Min(3,82/10)} tens and {82%10+10*Math.Min(3,82/10)} ones with {72/10} tens and {72%10} ones. Write SYMBOL=sign.";
        Assert.Equal("Compare 5 tens and 32 ones with 7 tens and 2 ones. Write SYMBOL=sign.",prompt);
        Assert.Equal("SYMBOL=>",UsCcssG1FullGradeUnit4FinalQuestions.Solve(family,p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,"SYMBOL=>"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"SYMBOL=<"));
        var varied=Enumerable.Range(0,180).Select(seed=>
            UsCcssG1FullGradeUnit4FinalQuestions.Build(family,new Random(seed*23+29))).ToArray();
        Assert.Contains(varied,x=>x.Prompt.Contains("32 ones",StringComparison.Ordinal)
            ||x.Prompt.Contains("30 ones",StringComparison.Ordinal)
            ||x.Prompt.Contains("33 ones",StringComparison.Ordinal));
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

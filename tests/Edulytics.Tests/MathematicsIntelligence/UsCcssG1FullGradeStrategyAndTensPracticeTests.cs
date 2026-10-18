using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssG1FullGradeStrategyAndTensPracticeTests
{
    [Fact]
    public void NineNewFamiliesHaveRuntimeInteractionContractsAndOnlyScopedLessons()
    {
        Assert.Equal(9,UsCcssG1FullGradeStrategyAndTensQuestions.Families.Count);
        foreach(var family in UsCcssG1FullGradeStrategyAndTensQuestions.Families)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family),family);
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var interaction),family);
            Assert.Equal(LessonPracticeInteractionKind.ContractDriven,interaction!.InteractionKind);
        }
        var reviewed=LessonPracticeContractRegistry.All.Where(x=>
           x.LessonCode.StartsWith("PED:US-CCSS-MATH:G1:U03:L2",StringComparison.Ordinal)||
           x.LessonCode.StartsWith("PED:US-CCSS-MATH:G1:U04:L0",StringComparison.Ordinal))
           .Where(x=>x.AllowedQuestionFamilies.Any(f=>UsCcssG1FullGradeStrategyAndTensQuestions.Supports(f))).ToArray();
        Assert.Equal(9,reviewed.Length);
        Assert.DoesNotContain(reviewed,x=>x.LessonCode=="PED:US-CCSS-MATH:G1:U03:L28");
    }
    [Fact]
    public void NineFamiliesRejectWrongAndMalformedValuesOnHundredsOfSeededInstances()
    {
        foreach(var family in UsCcssG1FullGradeStrategyAndTensQuestions.Families)
        {
            for(var seed=0;seed<160;seed++)
            {
                var item=UsCcssG1FullGradeStrategyAndTensQuestions.Build(family,new Random(seed*43+11));
                Assert.True(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(family,item.Parameters));
                var answer=UsCcssG1FullGradeStrategyAndTensQuestions.Solve(family,item.Parameters);
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,answer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,answer+";INVALID=1"));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,"TOTAL=-1"));
                Assert.False(UsCcssG1FullGradeStrategyAndTensQuestions.GradePersistedAnswer(family,answer,""));
            }
        }
    }
    [Fact]
    public void StoryModeSupportsStartChangeResultAndSubtractionUnknowns()
    {
        for(int mode=0;mode<6;mode++)
        {
            var p=new Dictionary<string,int>{{"a",7},{"b",5},{"c",0},{"mode",mode}};
            var answer=UsCcssG1FullGradeStrategyAndTensQuestions.Solve(
                UsCcssG1FullGradeStrategyAndTensQuestions.StoryUnknowns,p);
            var expected=mode switch
            {
                0=>"UNKNOWN=12;EQUATION=7+5=12",
                1=>"UNKNOWN=5;EQUATION=7+5=12",
                2=>"UNKNOWN=7;EQUATION=7+5=12",
                3=>"UNKNOWN=7;EQUATION=12-5=7",
                4=>"UNKNOWN=5;EQUATION=12-5=7",
                _=>"UNKNOWN=12;EQUATION=12-5=7"
            };
            Assert.Equal(expected,answer);
            Assert.True(UsCcssG1FullGradeStrategyAndTensQuestions.Verify(
                UsCcssG1FullGradeStrategyAndTensQuestions.StoryUnknowns,p,expected));
            Assert.False(UsCcssG1FullGradeStrategyAndTensQuestions.Verify(
                UsCcssG1FullGradeStrategyAndTensQuestions.StoryUnknowns,p,"UNKNOWN=99;EQUATION=7+5=12"));
        }
    }
    [Fact]
    public void TensAreNeverMistakenForSingleOnesOrNegativeOne()
    {
        var add=new Dictionary<string,int>{{"a",4},{"b",1},{"c",0},{"mode",0}};
        Assert.Equal("TENS=5;TOTAL=50",UsCcssG1FullGradeStrategyAndTensQuestions.Solve(
          UsCcssG1FullGradeStrategyAndTensQuestions.AddRemoveTen,add));
        add["mode"]=1;
        Assert.Equal("TENS=3;TOTAL=30",UsCcssG1FullGradeStrategyAndTensQuestions.Solve(
          UsCcssG1FullGradeStrategyAndTensQuestions.AddRemoveTen,add));
        Assert.False(UsCcssG1FullGradeStrategyAndTensQuestions.Verify(
          UsCcssG1FullGradeStrategyAndTensQuestions.AddRemoveTen,add,"TENS=3;TOTAL=3"));
        Assert.False(UsCcssG1FullGradeStrategyAndTensQuestions.Valid(
          UsCcssG1FullGradeStrategyAndTensQuestions.AddRemoveTen,
          new Dictionary<string,int>{{"a",2},{"b",3},{"c",0},{"mode",1}}));
    }
    [Fact]
    public void RealEngineEmitsFiveDistinctQuestionsPerFamily()
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var family in UsCcssG1FullGradeStrategyAndTensQuestions.Families)
        {
            var items=engine.Generate("G1-real-numeric-61-70",family,[family],
                ExactSkillQuestionDifficulty.Standard,10,20261018,[]);
            Assert.Equal(10,items.Count);
            Assert.Equal(10,items.Select(x=>x.Prompt).Distinct(StringComparer.Ordinal).Count());
            foreach(var item in items)
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,item.CorrectAnswer));
        }
    }
}

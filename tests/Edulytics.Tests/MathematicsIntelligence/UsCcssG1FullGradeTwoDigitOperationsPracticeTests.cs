using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssG1FullGradeTwoDigitOperationsPracticeTests
{
    [Fact]
    public void SourceGoalFamiliesAreRegisteredForNewG1U04L11ToL15()
    {
        Assert.Equal(5,UsCcssG1FullGradeTwoDigitOperationsQuestions.Families.Count);
        for(int n=11;n<=15;n++)
        {
            var code=$"PED:US-CCSS-MATH:G1:U04:L{n:00}";
            var row=LessonPracticeContractRegistry.All.Single(x=>x.LessonCode==code);
            Assert.Single(row.AllowedQuestionFamilies);
            var fam=row.AllowedQuestionFamilies.Single();
            Assert.True(UsCcssG1FullGradeTwoDigitOperationsQuestions.Supports(fam));
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(fam));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(fam,out var interaction));
            Assert.Equal(LessonPracticeInteractionKind.ContractDriven,interaction!.InteractionKind);
        }
    }
    [Fact]
    public void RandomizedAnswerVerificationRejectsWrongAndMalformedSubmissions()
    {
        foreach(var fam in UsCcssG1FullGradeTwoDigitOperationsQuestions.Families)
            for(int seed=0;seed<180;seed++)
            {
                var problem=UsCcssG1FullGradeTwoDigitOperationsQuestions.Build(fam,new Random(seed*23+5));
                var ans=UsCcssG1FullGradeTwoDigitOperationsQuestions.Solve(fam,problem.Parameters);
                Assert.True(UsCcssG1FullGradeTwoDigitOperationsQuestions.Valid(fam,problem.Parameters));
                Assert.True(ExactSkillContractQuestionEngine.Verify(fam,problem.Parameters,ans));
                Assert.False(ExactSkillContractQuestionEngine.Verify(fam,problem.Parameters,ans+";FAKE=0"));
                Assert.False(ExactSkillContractQuestionEngine.Verify(fam,problem.Parameters,"RESULT=-99"));
            }
    }
    [Fact]
    public void WordComparisonPrecedesOfficialSymbols()
    {
        var p=new Dictionary<string,int>{{"a",64},{"b",57},{"mode",1},{"unused",0}};
        Assert.Equal("RELATION=GREATER",UsCcssG1FullGradeTwoDigitOperationsQuestions.Solve(
          UsCcssG1FullGradeTwoDigitOperationsQuestions.CompareWords,p));
        Assert.Equal("SYMBOL=>",UsCcssG1FullGradeTwoDigitOperationsQuestions.Solve(
          UsCcssG1FullGradeTwoDigitOperationsQuestions.CompareSymbols,p));
        p["a"]=36;p["b"]=63;
        Assert.Equal("RELATION=LESS",UsCcssG1FullGradeTwoDigitOperationsQuestions.Solve(
          UsCcssG1FullGradeTwoDigitOperationsQuestions.CompareWords,p));
        Assert.Equal("SYMBOL=<",UsCcssG1FullGradeTwoDigitOperationsQuestions.Solve(
          UsCcssG1FullGradeTwoDigitOperationsQuestions.CompareSymbols,p));
        p["b"]=36;
        Assert.Equal("SYMBOL==",UsCcssG1FullGradeTwoDigitOperationsQuestions.Solve(
          UsCcssG1FullGradeTwoDigitOperationsQuestions.CompareSymbols,p));
    }
    [Fact]
    public void LiveEngineEmitsTenDifferentValidQuestionsPerFamily()
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var family in UsCcssG1FullGradeTwoDigitOperationsQuestions.Families)
        {
            var q=engine.Generate("grade1-76-to-80",family,[family],
                ExactSkillQuestionDifficulty.Standard,10,20261018,[]);
            Assert.Equal(10,q.Count);
            Assert.Equal(10,q.Select(x=>x.Prompt).Distinct(StringComparer.Ordinal).Count());
        }
    }
}

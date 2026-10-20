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
    public void Lesson76OriginalFiveTensAdditionTasksRespectActualResultOnes()
    {
        const string f=UsCcssG1FullGradeTwoDigitOperationsQuestions.AddTens;
        var source=new[]{(a:37,b:20,sum:57,ones:7),(a:60,b:23,sum:83,ones:3),
            (a:48,b:50,sum:98,ones:8),(a:54,b:20,sum:74,ones:4),
            (a:30,b:65,sum:95,ones:5)};
        foreach(var (a,b,sum,ones) in source)
        {
            var p=new Dictionary<string,int>{{"a",a},{"b",b},{"mode",4},{"unused",0}};
            Assert.True(UsCcssG1FullGradeTwoDigitOperationsQuestions.Valid(f,p));
            Assert.Equal($"TOTAL={sum};ONES={ones}",
                UsCcssG1FullGradeTwoDigitOperationsQuestions.Solve(f,p));
            Assert.True(ExactSkillContractQuestionEngine.Verify(f,p,
                $"TOTAL={sum};ONES={ones}"));
            Assert.False(ExactSkillContractQuestionEngine.Verify(f,p,
                $"TOTAL={sum};ONES={(ones+1)%10}"));
        }
        var invalid=new Dictionary<string,int>{{"a",60},{"b",24},{"mode",4},{"unused",0}};
        Assert.False(UsCcssG1FullGradeTwoDigitOperationsQuestions.Valid(f,invalid));
        var commuted=new Dictionary<string,int>{{"a",30},{"b",65},{"mode",4},{"unused",0}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(f,commuted,"TOTAL=95;ONES=5"));
    }

    [Fact]
    public void Lesson76SourceTrueFalseAndIndependentMissingDigitMathAreVerified()
    {
        const string f=UsCcssG1FullGradeTwoDigitOperationsQuestions.AddTens;
        var tests=new[]{
            (m:5,a:80,b:5,answer:"TRUE=YES;LEFT=85;RIGHT=85"),
            (m:6,a:70,b:1,answer:"TRUE=NO;LEFT=71;RIGHT=81"),
            (m:7,a:20,b:6,answer:"TRUE=NO;LEFT=26;RIGHT=36"),
            (m:8,a:46,b:30,answer:"TOTAL=76;ONES=6"),
            (m:9,a:45,b:20,answer:"DIGIT=5;EQUATION=45+20=65;TRUE=YES"),
            (m:10,a:43,b:20,answer:"TRUE=NO;MAX=69;TARGET=75")
        };
        foreach(var (m,a,b,answer) in tests)
        {
            var p=new Dictionary<string,int>{{"a",a},{"b",b},{"mode",m},{"unused",0}};
            Assert.True(UsCcssG1FullGradeTwoDigitOperationsQuestions.Valid(f,p));
            Assert.Equal(answer,UsCcssG1FullGradeTwoDigitOperationsQuestions.Solve(f,p));
            Assert.True(ExactSkillContractQuestionEngine.Verify(f,p,answer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(f,p,answer+";OTHER=1"));
            p["b"]=b+1;
            Assert.False(UsCcssG1FullGradeTwoDigitOperationsQuestions.Valid(f,p));
        }
        var generated=Enumerable.Range(0,990).Select(i=>
            UsCcssG1FullGradeTwoDigitOperationsQuestions.Build(f,new Random(i*73+17))).ToArray();
        foreach(var mode in new[]{4,5,6,7,8,9,10})
            Assert.True(generated.Count(q=>q.Parameters["mode"]==mode)>35);
        Assert.All(generated,q=>Assert.True(ExactSkillContractQuestionEngine.Verify(
            f,q.Parameters,UsCcssG1FullGradeTwoDigitOperationsQuestions.Solve(f,q.Parameters))));
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

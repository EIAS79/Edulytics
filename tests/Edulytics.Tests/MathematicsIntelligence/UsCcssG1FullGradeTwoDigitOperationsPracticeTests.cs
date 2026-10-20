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
            (m:10,a:43,b:20,answer:"TRUE=NO;MAX=69;TARGET=75"),
            (m:11,a:56,b:40,answer:"DIGIT=4;EQUATION=56+40=96;TRUE=YES"),
            (m:12,a:3,b:30,answer:"TRUE=NO;ONES_LEFT=3;ONES_RIGHT=4")
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
        foreach(var mode in new[]{4,5,6,7,8,9,10,11,12})
            Assert.True(generated.Count(q=>q.Parameters["mode"]==mode)>35);
        Assert.All(generated,q=>Assert.True(ExactSkillContractQuestionEngine.Verify(
            f,q.Parameters,UsCcssG1FullGradeTwoDigitOperationsQuestions.Solve(f,q.Parameters))));
    }

    [Fact]
    public void Lesson76SourceImageAccessibilityDescriptionsProveExactMissingTensAndImpossibleOnes()
    {
        const string family=UsCcssG1FullGradeTwoDigitOperationsQuestions.AddTens;
        var truePuzzle=new Dictionary<string,int>
            {["a"]=56,["b"]=40,["mode"]=11,["unused"]=0};
        const string actual="DIGIT=4;EQUATION=56+40=96;TRUE=YES";
        Assert.True(UsCcssG1FullGradeTwoDigitOperationsQuestions.Valid(family,truePuzzle));
        Assert.Equal(actual,UsCcssG1FullGradeTwoDigitOperationsQuestions.Solve(family,truePuzzle));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,truePuzzle,actual));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,truePuzzle,
            "DIGIT=3;EQUATION=56+30=86;TRUE=YES"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,truePuzzle,
            "DIGIT=4;EQUATION=56+40=95;TRUE=YES"));
        truePuzzle["b"]=50;
        Assert.False(UsCcssG1FullGradeTwoDigitOperationsQuestions.Valid(family,truePuzzle));

        var impossible=new Dictionary<string,int>
            {["a"]=3,["b"]=30,["mode"]=12,["unused"]=0};
        const string noDigit="TRUE=NO;ONES_LEFT=3;ONES_RIGHT=4";
        Assert.True(UsCcssG1FullGradeTwoDigitOperationsQuestions.Valid(family,impossible));
        Assert.Equal(noDigit,UsCcssG1FullGradeTwoDigitOperationsQuestions.Solve(family,impossible));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,impossible,noDigit));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,impossible,
            "TRUE=YES;ONES_LEFT=3;ONES_RIGHT=4"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,impossible,
            "TRUE=NO;ONES_LEFT=3;ONES_RIGHT=3"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,impossible,
            "TRUE=NO;ONES_LEFT=3;ONES_RIGHT=4;DIGIT=4"));
        impossible["a"]=4;
        Assert.False(UsCcssG1FullGradeTwoDigitOperationsQuestions.Valid(family,impossible));
        Assert.False(UsCcssG1FullGradeTwoDigitOperationsQuestions.Valid(
            UsCcssG1FullGradeTwoDigitOperationsQuestions.CompareWords,
            new Dictionary<string,int>{{"a",56},{"b",40},{"mode",11},{"unused",0}}));
        var questions=Enumerable.Range(0,1040)
            .Select(i=>UsCcssG1FullGradeTwoDigitOperationsQuestions.Build(
                family,new Random(i*47+29))).ToArray();
        foreach(var mode in new[]{11,12})
        {
            var modeQuestions=questions.Where(q=>q.Parameters["mode"]==mode).ToArray();
            Assert.True(modeQuestions.Length>40);
            Assert.All(modeQuestions,q=>
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,
                    UsCcssG1FullGradeTwoDigitOperationsQuestions.Solve(family,q.Parameters)));
                Assert.Contains("SOURCE-ALIGNED FIRST-PARTY",q.Prompt);
            });
        }
    }

    [Fact]
    public void Lesson77SourceRepeatedTenSequencesAndMixedOperationsRejectWrongAnswers()
    {
        const string family=UsCcssG1FullGradeTwoDigitOperationsQuestions.TenMoreLess;
        foreach(var (mode,start,increment,expected) in new[]{
            (2,67,30,"RESULT=97"),
            (2,99,30,"RESULT=69"),
            (3,45,20,"PLUS=65;MINUS=25;RETURN=45")
        })
        {
            var p=new Dictionary<string,int>{{"a",start},{"b",increment},{"mode",mode},{"unused",0}};
            Assert.True(UsCcssG1FullGradeTwoDigitOperationsQuestions.Valid(family,p));
            Assert.Equal(expected,UsCcssG1FullGradeTwoDigitOperationsQuestions.Solve(family,p));
            Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,expected));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,expected+";EXTRA=0"));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"RESULT=87"));
            p["b"]=10;
            Assert.False(UsCcssG1FullGradeTwoDigitOperationsQuestions.Valid(family,p));
            p["b"]=increment;
            p["a"]=start-1;
            Assert.False(UsCcssG1FullGradeTwoDigitOperationsQuestions.Valid(family,p));
        }
        var generated=Enumerable.Range(0,1200).Select(i=>
            UsCcssG1FullGradeTwoDigitOperationsQuestions.Build(family,new Random(i*31+19))).ToArray();
        Assert.Contains(generated,q=>q.Parameters["mode"]==2&&q.Prompt.Contains("67+10+10+10"));
        Assert.Contains(generated,q=>q.Parameters["mode"]==2&&q.Prompt.Contains("99-10-10-10"));
        Assert.Contains(generated,q=>q.Parameters["mode"]==3&&q.Prompt.Contains("45+10+10"));
        Assert.All(generated,q=>Assert.True(ExactSkillContractQuestionEngine.Verify(
            family,q.Parameters,UsCcssG1FullGradeTwoDigitOperationsQuestions.Solve(family,q.Parameters))));
    }

    [Fact]
    public void Lesson77EachRepeatedTenStepRequiresAllThreeSourceAlignedValues()
    {
        const string family=UsCcssG1FullGradeTwoDigitOperationsQuestions.TenMoreLess;
        foreach(var (mode,start,answer,wrong) in new[]{
            (9,67,"STEP1=77;STEP2=87;STEP3=97","STEP1=77;STEP2=97;STEP3=97"),
            (10,99,"STEP1=89;STEP2=79;STEP3=69","STEP1=89;STEP2=79;STEP3=79")
        })
        {
            var p=new Dictionary<string,int>{{"a",start},{"b",30},{"mode",mode},{"unused",0}};
            Assert.True(UsCcssG1FullGradeTwoDigitOperationsQuestions.Valid(family,p));
            Assert.Equal(answer,UsCcssG1FullGradeTwoDigitOperationsQuestions.Solve(family,p));
            Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,answer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,wrong));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"STEP1=77;STEP3=97"));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,answer+";EXTRA=0"));
            p["b"]=20;
            Assert.False(UsCcssG1FullGradeTwoDigitOperationsQuestions.Valid(family,p));
            p["b"]=30;
            p["a"]=start+1;
            Assert.False(UsCcssG1FullGradeTwoDigitOperationsQuestions.Valid(family,p));
        }
        var generated=Enumerable.Range(0,1000).Select(i=>
            UsCcssG1FullGradeTwoDigitOperationsQuestions.Build(family,new Random(i*29+31))).ToArray();
        foreach(var mode in new[]{9,10})
            Assert.Contains(generated,q=>q.Parameters["mode"]==mode&&q.Prompt.Contains("THREE-STEP CHECK"));
        Assert.All(generated,q=>Assert.True(ExactSkillContractQuestionEngine.Verify(
            family,q.Parameters,UsCcssG1FullGradeTwoDigitOperationsQuestions.Solve(family,q.Parameters))));
    }

    [Fact]
    public void Lesson78OriginalCenterNumericTasksAndDistinctCardChoiceAreVerified()
    {
        const string family=UsCcssG1FullGradeTwoDigitOperationsQuestions.CenterReview;
        foreach(var (mode,a,b,expected) in new[]{
            (4,70,20,"ADD=90;SUBTRACT=50"),
            (5,8,7,"TOTAL=20;CARDS=8+7+5"),
            (6,9,6,"ADD=15;SUBTRACT=9")
        })
        {
            var p=new Dictionary<string,int>{{"a",a},{"b",b},{"mode",mode},{"unused",0}};
            Assert.True(UsCcssG1FullGradeTwoDigitOperationsQuestions.Valid(family,p));
            Assert.Equal(expected,UsCcssG1FullGradeTwoDigitOperationsQuestions.Solve(family,p));
            Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,expected));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,expected+";EXTRA=1"));
            p["a"]=a+1;
            Assert.False(UsCcssG1FullGradeTwoDigitOperationsQuestions.Valid(family,p));
        }
        var questions=Enumerable.Range(0,1400).Select(i=>
            UsCcssG1FullGradeTwoDigitOperationsQuestions.Build(family,new Random(i*43+11))).ToArray();
        foreach(var mode in new[]{4,5,6})
            Assert.True(questions.Count(q=>q.Parameters["mode"]==mode)>100);
        Assert.All(questions,q=>Assert.True(ExactSkillContractQuestionEngine.Verify(
            family,q.Parameters,UsCcssG1FullGradeTwoDigitOperationsQuestions.Solve(family,q.Parameters))));
    }

    [Fact]
    public void Lesson77RepeatedTenSequencesRejectWrongOraclesAndParameters()
    {
        const string family=UsCcssG1FullGradeTwoDigitOperationsQuestions.TenMoreLess;
        foreach(var (mode,a,b,answer) in new[]{
            (2,67,30,"RESULT=97"),
            (2,99,30,"RESULT=69"),
            (3,45,20,"PLUS=65;MINUS=25;RETURN=45")
        })
        {
            var p=new Dictionary<string,int>{{"a",a},{"b",b},{"mode",mode},{"unused",0}};
            Assert.True(UsCcssG1FullGradeTwoDigitOperationsQuestions.Valid(family,p));
            Assert.Equal(answer,UsCcssG1FullGradeTwoDigitOperationsQuestions.Solve(family,p));
            Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,answer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,answer+";EXTRA=0"));
            p["b"]=b+10;
            Assert.False(UsCcssG1FullGradeTwoDigitOperationsQuestions.Valid(family,p));
        }
    }

    [Fact]
    public void Lesson77AllFourOriginalTenChangePairsHaveStrictOracles()
    {
        const string family=UsCcssG1FullGradeTwoDigitOperationsQuestions.TenMoreLess;
        foreach(var (mode,a,plus,minus) in new[]{
            (5,67,77,57),(6,39,49,29),(7,52,62,42),(8,75,85,65)
        })
        {
            var p=new Dictionary<string,int>{{"a",a},{"b",10},{"mode",mode},{"unused",0}};
            var answer=$"PLUS={plus};MINUS={minus}";
            Assert.True(UsCcssG1FullGradeTwoDigitOperationsQuestions.Valid(family,p));
            Assert.Equal(answer,UsCcssG1FullGradeTwoDigitOperationsQuestions.Solve(family,p));
            Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,answer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,$"PLUS={plus};MINUS={minus+10}"));
            p["a"]=a+1;
            Assert.False(UsCcssG1FullGradeTwoDigitOperationsQuestions.Valid(family,p));
        }
        var generated=Enumerable.Range(0,700).Select(i=>
            UsCcssG1FullGradeTwoDigitOperationsQuestions.Build(family,new Random(i*17+11))).ToArray();
        foreach(var mode in new[]{5,6,7,8})
            Assert.Contains(generated,q=>q.Parameters["mode"]==mode);
    }

    [Fact]
    public void Lesson77IndependentExitAssessesBothDirectionsWithoutPublisherClaim()
    {
        const string family=UsCcssG1FullGradeTwoDigitOperationsQuestions.TenMoreLess;
        var p=new Dictionary<string,int>{{"a",46},{"b",10},{"mode",4},{"unused",0}};
        Assert.True(UsCcssG1FullGradeTwoDigitOperationsQuestions.Valid(family,p));
        Assert.Equal("PLUS=56;MINUS=36",UsCcssG1FullGradeTwoDigitOperationsQuestions.Solve(family,p));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,"PLUS=56;MINUS=36"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"PLUS=56;MINUS=46"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"PLUS=56"));
        p["a"]=47;
        Assert.False(UsCcssG1FullGradeTwoDigitOperationsQuestions.Valid(family,p));
        var generated=Enumerable.Range(0,200).Select(i=>
            UsCcssG1FullGradeTwoDigitOperationsQuestions.Build(family,new Random(i*31+3))).ToArray();
        Assert.Contains(generated,q=>q.Parameters["mode"]==4&&q.Prompt.Contains("INDEPENDENT EDULYTICS EXIT"));
    }

    [Fact]
    public void Lesson79OriginalElenaAndNoahCasesHaveStrictMathOracles()
    {
        const string family=UsCcssG1FullGradeTwoDigitOperationsQuestions.CompareWords;
        foreach(var (mode,a,b,answer) in new[]{
            (4,75,65,"RELATION=GREATER"),
            (5,39,41,"RELATION=LESS"),
            (6,93,26,"RELATION=GREATER"),
            (7,62,64,"RELATION=LESS")
        })
        {
            var p=new Dictionary<string,int>{{"a",a},{"b",b},{"mode",mode},{"unused",0}};
            Assert.True(UsCcssG1FullGradeTwoDigitOperationsQuestions.Valid(family,p));
            Assert.Equal(answer,UsCcssG1FullGradeTwoDigitOperationsQuestions.Solve(family,p));
            Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,answer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"RELATION=EQUAL"));
            p["a"]=a+1;
            Assert.False(UsCcssG1FullGradeTwoDigitOperationsQuestions.Valid(family,p));
        }
        var generated=Enumerable.Range(0,1200).Select(i=>
            UsCcssG1FullGradeTwoDigitOperationsQuestions.Build(family,new Random(i*37+17))).ToArray();
        Assert.Contains(generated,q=>q.Parameters["mode"]==4&&q.Prompt.Contains("ELENA"));
        Assert.Contains(generated,q=>q.Parameters["mode"]==5&&q.Prompt.Contains("NOAH"));
        foreach(var mode in new[]{6,7})
        {
            var sourceCases=generated.Where(q=>q.Parameters["mode"]==mode).ToArray();
            Assert.True(sourceCases.Length>70);
            Assert.All(sourceCases,q=>Assert.Contains("SOURCE TEACHER DISCUSSION",q.Prompt));
        }
    }

    [Fact]
    public void Lesson80OriginalSymbolComparisonsAreSourceExact()
    {
        const string family=UsCcssG1FullGradeTwoDigitOperationsQuestions.CompareSymbols;
        foreach(var (mode,a,b,answer) in new[]{
            (4,17,27,"SYMBOL=<"),
            (5,34,36,"SYMBOL=<"),
            (6,17,47,"SYMBOL=<")
        })
        {
            var p=new Dictionary<string,int>{{"a",a},{"b",b},{"mode",mode},{"unused",0}};
            Assert.True(UsCcssG1FullGradeTwoDigitOperationsQuestions.Valid(family,p));
            Assert.Equal(answer,UsCcssG1FullGradeTwoDigitOperationsQuestions.Solve(family,p));
            Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,answer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"SYMBOL=>"));
            p["b"]=b+1;
            Assert.False(UsCcssG1FullGradeTwoDigitOperationsQuestions.Valid(family,p));
        }
        var generated=Enumerable.Range(0,600).Select(i=>
            UsCcssG1FullGradeTwoDigitOperationsQuestions.Build(family,new Random(i*37+19))).ToArray();
        foreach(var mode in new[]{4,5,6})
            Assert.Contains(generated,q=>q.Parameters["mode"]==mode);
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

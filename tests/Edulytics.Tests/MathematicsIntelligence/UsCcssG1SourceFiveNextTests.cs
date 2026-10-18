using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssG1SourceFiveNextTests
{
    private static readonly string[] Codes=
    [
        "PED:US-CCSS-MATH:G1:U03:L04",
        "PED:US-CCSS-MATH:G1:U03:L05",
        "PED:US-CCSS-MATH:G1:U03:L06",
        "PED:US-CCSS-MATH:G1:U03:L07",
        "PED:US-CCSS-MATH:G1:U03:L08"
    ];

    [Fact]
    public void FiveCanonicalLessonsHaveExactlyTwoSourceSpecificContractsEach()
    {
        var contracts=Codes.Select(code=>LessonPracticeContractRegistry.All
            .Single(x=>x.LessonCode==code)).ToArray();
        Assert.Equal(5,contracts.Length);
        Assert.All(contracts,c=>Assert.Equal(2,c.AllowedQuestionFamilies.Count));
        Assert.Equal("usccss.g1.ten_as_unit",contracts[4].SkillId);
        Assert.Contains(UsCcssG1SourceFiveNextQuestions.StartUnknown,
            contracts[2].AllowedQuestionFamilies);
        Assert.Contains(UsCcssG1SourceFiveNextQuestions.CompareCards,
            contracts[3].AllowedQuestionFamilies);
        Assert.Contains(UsCcssG1SourceFiveNextQuestions.FourTeens,
            contracts[4].AllowedQuestionFamilies);
        foreach(var c in contracts)
            Assert.All(c.AllowedQuestionFamilies,f=>
            {
                Assert.True(UsCcssG1SourceFiveNextQuestions.Supports(f));
                Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var interaction));
                Assert.Equal(LessonPracticeInteractionKind.ContractDriven,interaction!.InteractionKind);
            });
    }

    [Fact]
    public void TenExactFamiliesGenerateValidateRenderAndRejectWrongAnswers()
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var (contract,index) in Codes.Select((code,index)=>
            (LessonPracticeContractRegistry.All.Single(c=>c.LessonCode==code),index)))
        foreach(var family in contract.AllowedQuestionFamilies)
        {
            var questions=engine.Generate("g1009-tests",family,[family],
                ExactSkillQuestionDifficulty.Standard,5,12401+index,[]);
            Assert.Equal(5,questions.Count);
            Assert.Equal(5,questions.Select(q=>q.Prompt)
                .Distinct(StringComparer.OrdinalIgnoreCase).Count());
            foreach(var q in questions)
            {
                Assert.Equal(family,q.Family);
                Assert.True(UsCcssG1SourceFiveNextQuestions.Valid(family,q.Parameters));
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
                Assert.True(UsCcssG1SourceFiveNextQuestions.GradePersistedAnswer(family,
                    q.CorrectAnswer,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"999"));
                Assert.False(UsCcssG1SourceFiveNextQuestions.GradePersistedAnswer(family,
                    q.CorrectAnswer,"999"));
                var svg=Edulytics.Web.Presentation.PracticeMathVisualRenderer.RenderSvg(
                    family,System.Text.Json.JsonSerializer.Serialize(new{parameters=q.Parameters}));
                Assert.NotNull(svg);
                Assert.Contains("<svg ",svg);
                Assert.DoesNotContain("<image ",svg);
                Assert.DoesNotContain(q.CorrectAnswer,svg);
            }
        }
    }

    [Fact]
    public void AllGenerationSeedsPreserveConstraintsAndProduceVerifiedMath()
    {
        foreach(var f in UsCcssG1SourceFiveNextQuestions.Families)
        {
            for(var seed=0;seed<220;seed++)
            {
                var q=UsCcssG1SourceFiveNextQuestions.Build(f,new Random(seed*47+11));
                Assert.True(UsCcssG1SourceFiveNextQuestions.Valid(f,q.Parameters));
                var expected=UsCcssG1SourceFiveNextQuestions.Solve(f,q.Parameters);
                Assert.True(UsCcssG1SourceFiveNextQuestions.Verify(f,q.Parameters,expected));
                Assert.True(UsCcssG1SourceFiveNextQuestions.GradePersistedAnswer(
                    f,expected,expected));
            }
        }
    }

    [Fact]
    public void AcrossSessionsEqualVisibleTasksKeepOneExposureFingerprint()
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var family in UsCcssG1SourceFiveNextQuestions.Families)
        {
            var seen=new Dictionary<string,string>(StringComparer.Ordinal);
            for(var seed=1;seed<=150;seed++)
            {
                var q=Assert.Single(engine.Generate("g1009-history",family,[family],
                    ExactSkillQuestionDifficulty.Standard,1,seed,[]));
                if(seen.TryGetValue(q.Prompt,out var previous))
                    Assert.Equal(previous,q.ExposureFingerprint);
                else
                    seen[q.Prompt]=q.ExposureFingerprint;
            }
            Assert.True(seen.Count>=5,"Too small G1-009 task pool: "+family);
            var first=Assert.Single(engine.Generate("g1009-history",family,[family],
                ExactSkillQuestionDifficulty.Standard,1,17,[]));
            var next=Assert.Single(engine.Generate("g1009-history",family,[family],
                ExactSkillQuestionDifficulty.Standard,1,17,[first.ExposureFingerprint]));
            Assert.NotEqual(first.Prompt,next.Prompt);
        }
    }

    [Fact]
    public void TargetedEducationalNegativeControlsCannotPass()
    {
        var p=new Dictionary<string,int>{{"a",6},{"b",2},{"c",1},{"mode",0}};
        Assert.True(UsCcssG1SourceFiveNextQuestions.Verify(
            UsCcssG1SourceFiveNextQuestions.MakeTen,p,
            "YELLOW=4;FIRST=6+4=10;SECOND=4+6=10"));
        Assert.False(UsCcssG1SourceFiveNextQuestions.Verify(
            UsCcssG1SourceFiveNextQuestions.MakeTen,p,
            "YELLOW=4;FIRST=6+4=10;SECOND=4+5=10"));
        Assert.True(UsCcssG1SourceFiveNextQuestions.Verify(
            UsCcssG1SourceFiveNextQuestions.MakeTen,p,
            "YELLOW=4;FIRST=4+6=10;SECOND=6+4=10"));
        Assert.False(UsCcssG1SourceFiveNextQuestions.GradePersistedAnswer(
            UsCcssG1SourceFiveNextQuestions.MakeTen,
            "YELLOW=4;FIRST=6+4=10;SECOND=4+6=10",
            "YELLOW=4;FIRST=6+4=10;SECOND=6+4=10"));
        Assert.True(UsCcssG1SourceFiveNextQuestions.GradePersistedAnswer(
            UsCcssG1SourceFiveNextQuestions.RelatedDifference,
            "DIFF=4;CHECK=2+4=6",
            "DIFF=4;CHECK=4+2=6"));
        Assert.False(UsCcssG1SourceFiveNextQuestions.GradePersistedAnswer(
            UsCcssG1SourceFiveNextQuestions.RelatedDifference,
            "DIFF=4;CHECK=2+4=6",
            "DIFF=4;CHECK=1+5=6"));
        Assert.True(UsCcssG1SourceFiveNextQuestions.Verify(
            UsCcssG1SourceFiveNextQuestions.StartUnknown,p,
            "START=4;EQUATION=4+2=6"));
        Assert.False(UsCcssG1SourceFiveNextQuestions.Verify(
            UsCcssG1SourceFiveNextQuestions.StartUnknown,p,
            "START=8;EQUATION=6+2=8"));
        var teen=new Dictionary<string,int>{{"a",16},{"b",2},{"c",1},{"mode",0}};
        Assert.True(UsCcssG1SourceFiveNextQuestions.Verify(
            UsCcssG1SourceFiveNextQuestions.TenUnit,teen,
            "TENS=1;ONES=6;TOTAL=16"));
        Assert.False(UsCcssG1SourceFiveNextQuestions.Verify(
            UsCcssG1SourceFiveNextQuestions.TenUnit,teen,
            "TENS=10;ONES=6;TOTAL=16"));
        Assert.True(UsCcssG1SourceFiveNextQuestions.Verify(
            UsCcssG1SourceFiveNextQuestions.FourTeens,teen,
            "ONES=6,7,8,9"));
        Assert.False(UsCcssG1SourceFiveNextQuestions.Verify(
            UsCcssG1SourceFiveNextQuestions.FourTeens,teen,
            "ONES=6,7,8,10"));
    }
}

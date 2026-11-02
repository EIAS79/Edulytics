using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssG1NextSourceFiveTests
{
    private static readonly string[] Codes=
    [
        "PED:US-CCSS-MATH:G1:U02:L21",
        "PED:US-CCSS-MATH:G1:U02:L22",
        "PED:US-CCSS-MATH:G1:U03:L01",
        "PED:US-CCSS-MATH:G1:U03:L02",
        "PED:US-CCSS-MATH:G1:U03:L03"
    ];

    [Fact]
    public void ExactlyFiveSourceSpecificLessonsEachUseBothAppropriateSkillFamilies()
    {
        var contracts=Codes.Select(code=>LessonPracticeContractRegistry.All.Single(x=>x.LessonCode==code)).ToArray();
        Assert.Equal(5,contracts.Length);
        Assert.All(contracts,c=>Assert.Equal(2,c.AllowedQuestionFamilies.Count));
        Assert.Contains(UsCcssG1NextSourceFiveQuestions.Puzzle,contracts[0].AllowedQuestionFamilies);
        Assert.Contains(UsCcssG1NextSourceFiveQuestions.Survey,contracts[1].AllowedQuestionFamilies);
        Assert.Contains(UsCcssG1NextSourceFiveQuestions.Sums,contracts[2].AllowedQuestionFamilies);
        Assert.Contains(UsCcssG1NextSourceFiveQuestions.Commute,contracts[3].AllowedQuestionFamilies);
        Assert.Contains(UsCcssG1NextSourceFiveQuestions.Equal,contracts[4].AllowedQuestionFamilies);
    }

    [Fact]
    public void EveryFamilyHasExactParametersVerifiableAnswersAndFirstPartyVisual()
    {
        var engine=new ExactSkillContractQuestionEngine();
        for(int lesson=0;lesson<Codes.Length;lesson++)
        {
            var contract=LessonPracticeContractRegistry.All.Single(x=>x.LessonCode==Codes[lesson]);
            foreach(var family in contract.AllowedQuestionFamilies)
            {
                Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var interaction));
                Assert.Equal(LessonPracticeInteractionKind.ContractDriven,interaction!.InteractionKind);
                var examples=engine.Generate("G1-008",family,[family],
                    ExactSkillQuestionDifficulty.Standard,6,20261018+lesson,[]);
                Assert.Equal(6,examples.Count);
                foreach(var item in examples)
                {
                    Assert.Equal(family,item.Family);
                    Assert.True(UsCcssG1NextSourceFiveQuestions.Valid(family,item.Parameters));
                    Assert.True(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,item.CorrectAnswer));
                    Assert.False(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,"999"));
                    Assert.False(UsCcssG1NextSourceFiveQuestions.GradePersistedAnswer(family,item.CorrectAnswer,"999"));
                    var svg=Edulytics.Web.Presentation.PracticeMathVisualRenderer.RenderSvg(
                        family,System.Text.Json.JsonSerializer.Serialize(new{parameters=item.Parameters}));
                    Assert.NotNull(svg);
                    Assert.Contains("<svg ",svg);
                    Assert.DoesNotContain("<image ",svg);
                    Assert.DoesNotContain(item.CorrectAnswer,svg);
                }
            }
        }
    }

    [Fact]
    public void SourceSkillSpecificFailuresAreNotMislabeledAsCorrect()
    {
        var sums=UsCcssG1NextSourceFiveQuestions.Sums;
        var p=new Dictionary<string,int>{{"a",4},{"b",3},{"c",2},{"mode",0}};
        Assert.True(UsCcssG1NextSourceFiveQuestions.Verify(sums,p,"SUM=7;SELF=GOT_IT"));
        Assert.True(UsCcssG1NextSourceFiveQuestions.Verify(sums,p,"SUM=7;SELF=NOT_YET"));
        Assert.True(UsCcssG1NextSourceFiveQuestions.GradePersistedAnswer(sums,
            "SUM=7;SELF=GOT_IT","SUM=7;SELF=NOT_YET"));
        Assert.False(UsCcssG1NextSourceFiveQuestions.Verify(sums,p,"SUM=8;SELF=GOT_IT"));
        Assert.False(UsCcssG1NextSourceFiveQuestions.Verify(sums,p,"SUM=7;SELF=MAYBE"));
        var favorite=UsCcssG1NextSourceFiveQuestions.Favorite;
        Assert.True(UsCcssG1NextSourceFiveQuestions.Verify(favorite,p,
            "EQUATION=4+2=6;REASON=COUNT_ON"));
        Assert.True(UsCcssG1NextSourceFiveQuestions.GradePersistedAnswer(favorite,
            "EQUATION=4+3=7;REASON=COUNT_ON",
            "EQUATION=4+2=6;REASON=COUNT_ON"));
        Assert.False(UsCcssG1NextSourceFiveQuestions.Verify(favorite,p,
            "EQUATION=4+4=8;REASON=COUNT_ON"));
        var puzzle=UsCcssG1NextSourceFiveQuestions.Puzzle;
        var q=new Dictionary<string,int>{{"a",4},{"b",2},{"c",3},{"mode",0}};
        Assert.True(UsCcssG1NextSourceFiveQuestions.Verify(puzzle,q,"FIRST=2;SECOND=3"));
        Assert.False(UsCcssG1NextSourceFiveQuestions.Verify(puzzle,q,"FIRST=2;SECOND=2"));
        var survey=UsCcssG1NextSourceFiveQuestions.Survey;
        var r=new Dictionary<string,int>{{"a",2},{"b",3},{"c",1},{"mode",0}};
        Assert.True(UsCcssG1NextSourceFiveQuestions.Verify(survey,r,
            "2+3=5;5+1=6;6-2=4"));
        Assert.False(UsCcssG1NextSourceFiveQuestions.Verify(survey,r,
            "2+3=5;5+1=6;1+1=2"));
        var equal=UsCcssG1NextSourceFiveQuestions.Equal;
        var e=new Dictionary<string,int>{{"a",2},{"b",3},{"c",4},{"otherB",1},{"mode",0}};
        Assert.True(UsCcssG1NextSourceFiveQuestions.Verify(equal,e,
            "LEFT=5;RIGHT=5;EQUAL=YES"));
        Assert.False(UsCcssG1NextSourceFiveQuestions.Verify(equal,e,
            "LEFT=5;RIGHT=5;EQUAL=NO"));
    }

    [Fact]
    public void SeedSweepForAllTenFamiliesMustAlwaysCreateValidatedProblems()
    {
        foreach(var family in Codes.SelectMany(code=>LessonPracticeContractRegistry.All
            .Single(x=>x.LessonCode==code).AllowedQuestionFamilies))
        {
            var modes=new HashSet<int>();
            for(int seed=0;seed<240;seed++)
            {
                var q=UsCcssG1NextSourceFiveQuestions.Build(family,new Random(seed*47+19));
                modes.Add(q.Parameters["mode"]);
                Assert.True(UsCcssG1NextSourceFiveQuestions.Valid(family,q.Parameters));
                var reference=UsCcssG1NextSourceFiveQuestions.Solve(family,q.Parameters);
                Assert.True(UsCcssG1NextSourceFiveQuestions.Verify(family,q.Parameters,reference));
                Assert.True(UsCcssG1NextSourceFiveQuestions.GradePersistedAnswer(family,reference,reference));
            }
            Assert.NotEmpty(modes);
        }
    }

    [Fact]
    public void IdenticalVisibleTasksHaveStableExposureAcrossUnusedParametersAndSessions()
    {
        var engine=new ExactSkillContractQuestionEngine();
        var familyList=Codes.SelectMany(code=>LessonPracticeContractRegistry.All
            .Single(c=>c.LessonCode==code).AllowedQuestionFamilies).ToArray();
        Assert.Equal(10,familyList.Length);
        foreach(var family in familyList)
        {
            var seen=new Dictionary<string,(string ExposureFingerprint,string Parameters)>(
                StringComparer.Ordinal);
            for(var seed=1;seed<=300;seed++)
            {
                var q=Assert.Single(engine.Generate(
                    "G1-008-exposures",family,[family],
                    ExactSkillQuestionDifficulty.Standard,1,seed,[]));
                var fingerprintParameters=string.Join(";",
                    q.Parameters.OrderBy(kv=>kv.Key,StringComparer.Ordinal)
                        .Select(kv=>$"{kv.Key}={kv.Value}"));
                if(seen.TryGetValue(q.Prompt,out var previous))
                {
                    // The identity is the task a child can actually see:
                    // irrelevant c/mode must not change longitudinal exposure.
                    Assert.Equal(previous.ExposureFingerprint,q.ExposureFingerprint);
                }
                else
                    seen[q.Prompt]=(q.ExposureFingerprint,fingerprintParameters);
            }
            Assert.True(seen.Count>4,"Expected a meaningful task pool for "+family);
            var batch=engine.Generate("G1-008-exposures",family,[family],
                ExactSkillQuestionDifficulty.Standard,5,20261018,[]);
            Assert.Equal(5,batch.Select(q=>q.Prompt)
                .Distinct(StringComparer.OrdinalIgnoreCase).Count());
            var initial=Assert.Single(engine.Generate("G1-008-exposures",
                family,[family],ExactSkillQuestionDifficulty.Standard,1,99,[]));
            var withHistory=Assert.Single(engine.Generate("G1-008-exposures",
                family,[family],ExactSkillQuestionDifficulty.Standard,1,99,
                [initial.ExposureFingerprint]));
            Assert.NotEqual(initial.Prompt,withHistory.Prompt);
        }
    }
}

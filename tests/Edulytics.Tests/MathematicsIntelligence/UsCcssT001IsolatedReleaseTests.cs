using System.Globalization;
using System.Reflection;
using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Data.Seeding;
using Edulytics.Services.Mathematics;
using Edulytics.Services.LessonContent;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Real-engine T001 checks. Not a substitute for academic L1-L6 evidence.</summary>
public sealed class UsCcssT001IsolatedReleaseTests
{
    private const string Prefix="PED:US-CCSS-MATH:G1:U01:L";

    [Fact]
    public void AllTenPracticeContractsStayWithinPublisherUnitOne()
    {
        for(var n=1;n<=10;n++)
        {
            var code=Prefix+n.ToString("D2");
            Assert.True(LessonPracticeCapabilityResolver.TryResolve(code,out var contract));
            Assert.NotNull(contract);
            Assert.Equal("READY_VERIFIED",contract!.Readiness);
            Assert.NotEmpty(contract.AllowedQuestionFamilies);
            foreach(var family in contract.AllowedQuestionFamilies)
                Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family),
                    "Unsupported family in "+code+": "+family);
            if(n!=1)
            {
                Assert.All(contract.AllowedQuestionFamilies,
                    x=>Assert.StartsWith("usccss.t001.g1.",x));
            }
        }
    }

    [Fact]
    public void L08ConstructedGraphIsAnAppliedProductionTaskNotAGenericCalculation()
    {
        var capability=Assert.Single(PracticeQuestionFormCapabilityRegistry.Resolve(
            "usccss.t001.g1.construct_shape_graph"));
        Assert.Equal(PracticeQuestionForm.Apply,capability.Form);
        Assert.Equal(PracticeCognitiveOperation.Apply,capability.CognitiveOperation);
        Assert.Equal(PracticeCognitiveDifficulty.Standard,capability.MinimumDifficulty);
        Assert.Equal(PracticeCognitiveDifficulty.Standard,capability.MaximumDifficulty);
    }

    [Fact]
    public void SurveyPersistedGraderNeverAcceptsTheInternalSolverSentinel()
    {
        var type=typeof(ExactSkillContractQuestionEngine).Assembly.GetType(
            "Edulytics.Services.Mathematics.UsCcssT001AuthoredActivities",
            throwOnError:true);
        Assert.NotNull(type);
        var grade=type!.GetMethod("GradePersistedAnswer",BindingFlags.Public|BindingFlags.Static);
        Assert.NotNull(grade);
        const string metadata="{\"parameters\":{\"minimumVotes\":5,\"maximumVotes\":10,\"topic\":0}}";
        const string eightVotes="{\"parameters\":{\"minimumVotes\":8,\"maximumVotes\":10,\"topic\":3}}";
        bool Check(string actual,string expected="survey-entry-protocol-v1",
            string parameters=metadata)=>
            (bool)grade!.Invoke(null,[
                "usccss.t001.g1.collect_survey_graph",expected,actual,parameters])!;
        Assert.False(Check("survey-entry-protocol-v1"));
        Assert.True(Check("v:01201|g:2,2,1"));
        Assert.False(Check("v:01201|g:2,1,2"));
        Assert.False(Check("v:01201|g:2,2,1","fake-sentinel"));
        Assert.False(Check("v:01201|g:2,2,1",parameters:eightVotes));
        Assert.False(Check("v:01201|g:2,2,1",parameters:"{\"parameters\":{}}"));
        Assert.False(Check("v:012|g:1,1,1"));
        Assert.False(Check("v:0120x|g:2,1,2"));
        Assert.False(Check("v:01201|g:02,2,1"));
        Assert.False(Check("v:01201|g:2,2,1|teacher-approved"));
    }

    [Fact]
    public void SourceExactQuestionsGenerateAndGradeAcrossDifficultyAndSeeds()
    {
        var engine=new ExactSkillContractQuestionEngine();
        var checkedCount=0;
        for(var n=2;n<=10;n++)
        {
            var code=Prefix+n.ToString("D2");
            Assert.True(LessonPracticeCapabilityResolver.TryResolve(code,out var contract));
            foreach(var family in contract!.AllowedQuestionFamilies)
                foreach(var difficulty in new[]
                {
                    ExactSkillQuestionDifficulty.Standard,
                    ExactSkillQuestionDifficulty.Stretch,
                    ExactSkillQuestionDifficulty.Challenge
                })
                    for(var sample=0;sample<10;sample++)
                    {
                        var q=Assert.Single(engine.Generate(
                            "usccss-t001-independent-math-oracle",code,[family],
                            difficulty,1,1881100+checkedCount,[]));
                        Assert.Equal(family,q.Family);
                        // Arithmetic oracle is intentionally separate from
                        // the production generator/solver and verifier.
                        var p=q.Parameters;
                        if(family is
                            "usccss.t001.g1.write_dot_expression" or
                            "usccss.t001.g1.construct_tool_sort" or
                            "usccss.t001.g1.collect_survey_graph")
                        {
                            Assert.Equal(Edulytics.Core.Enums.AssessmentItemType.ShortAnswer,q.ItemType);
                            Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,q.CorrectAnswer));
                            if(family=="usccss.t001.g1.write_dot_expression")
                            {
                                Assert.Equal($"{p["left"]}+{p["right"]}={p["left"]+p["right"]}",q.CorrectAnswer);
                                Assert.Contains("Dot cards: ",q.Prompt);
                                Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,
                                    $"{p["right"]}+{p["left"]}={p["left"]+p["right"]+1}"));
                                Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,
                                    (p["left"]+p["right"]).ToString(CultureInfo.InvariantCulture)));
                            }
                            else if(family=="usccss.t001.g1.construct_tool_sort")
                            {
                                var tokens=q.Prompt.Split("Items: ",StringSplitOptions.None)[1]
                                    .Split(". Sort EVERY",StringSplitOptions.None)[0]
                                    .Split(", ",StringSplitOptions.None);
                                var kinds=new[]{"counter","pattern block","inch tile"};
                                Assert.Equal(p["length"],tokens.Length);
                                for(var position=0;position<tokens.Length;position++)
                                    Assert.Equal(kinds[p["item"+position]],tokens[position]);
                                Assert.Equal(string.Join(",",Enumerable.Range(0,p["length"])
                                    .Select(i=>p["item"+i])),q.CorrectAnswer);
                                Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,
                                    "0,0,0,0,0,0,0,0,0"));
                            }
                            else
                            {
                                Assert.Equal("survey-entry-protocol-v1",q.CorrectAnswer);
                                Assert.Contains("REAL CLASS SURVEY",q.Prompt);
                                Assert.Contains("teacher",q.Prompt,StringComparison.OrdinalIgnoreCase);
                                Assert.InRange(p["topic"],0,5);
                                Assert.InRange(p["minimumVotes"],5,8);
                                var sampleVotes=Enumerable.Range(0,p["minimumVotes"])
                                    .Select(i=>i%3).ToArray();
                                var computedCounts=Enumerable.Range(0,3)
                                    .Select(i=>sampleVotes.Count(v=>v==i)).ToArray();
                                var validSurvey="v:"+string.Concat(sampleVotes.Select(i=>i.ToString(
                                    CultureInfo.InvariantCulture)))+"|g:"+
                                    string.Join(",",computedCounts);
                                Assert.True(ExactSkillContractQuestionEngine.Verify(
                                    family,p,validSurvey));
                                Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,
                                    "v:"+string.Concat(sampleVotes)+"|g:0,0,0"));
                                Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,
                                    "v:0120|g:2,1,1"));
                                Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,
                                    "v:0120x|g:2,1,2"));
                            }
                            checkedCount++;
                            continue;
                        }
                        if(family=="usccss.t001.g1.construct_shape_graph")
                        {
                            var fullGraph=string.Join(",",new[]{p["first"],p["second"],p["third"]});
                            Assert.Equal(Edulytics.Core.Enums.AssessmentItemType.ShortAnswer,q.ItemType);
                            Assert.Equal(fullGraph,q.CorrectAnswer);
                            Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,fullGraph));
                            Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"0,0,0"));
                            Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"5,5,5"));
                            Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"1,2"));
                            Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"1|2|3"));
                            var cards=q.Prompt.Split("Cards: ",StringSplitOptions.None)[1]
                                .Split(". ",StringSplitOptions.None)[0]
                                .Split(", ",StringSplitOptions.None);
                            Assert.InRange(cards.Length,3,10);
                            Assert.Equal(p["first"],cards.Count(x=>x=="▲"));
                            Assert.Equal(p["second"],cards.Count(x=>x=="■"));
                            Assert.Equal(p["third"],cards.Count(x=>x=="●"));
                            checkedCount++;
                            continue;
                        }
                        var independentlyComputed=family switch
                        {
                            "usccss.t001.g1.dot_pair_sum"=>p["left"]+p["right"],
                            "usccss.t001.g1.add_one_or_two"=>p["start"]+p["step"],
                            "usccss.t001.g1.subtract_one_or_two"=>p["start"]-p["step"],
                            "usccss.t001.g1.mixed_add_subtract"=>
                                p["operation"]==0?p["left"]+p["right"]:p["left"]-p["right"],
                            "usccss.t001.g1.make_ten_missing_part"=>10-p["known"],
                            "usccss.t001.g1.choose_dot_expression" or
                            "usccss.t001.g1.choose_tool_sort" or
                            "usccss.t001.g1.choose_shape_chart" or
                            "usccss.t001.g1.choose_survey_table"=>
                                1+((p["first"]+2*p["second"]+3*p["third"])%3),
                            "usccss.t001.g1.sort_math_tools" or
                            "usccss.t001.g1.sort_shape_cards" or
                            "usccss.t001.g1.class_survey_categories"=>
                                p["category"] switch
                                {
                                    0=>p["first"],1=>p["second"],2=>p["third"],
                                    _=>throw new InvalidOperationException("Unreviewed category")
                                },
                            _=>throw new InvalidOperationException("Unreviewed T001 family")
                        };
                        Assert.Equal(independentlyComputed.ToString(CultureInfo.InvariantCulture),
                            q.CorrectAnswer);
                        Assert.True(ExactSkillContractQuestionEngine.Verify(
                            family,q.Parameters,q.CorrectAnswer));
                        var a=int.Parse(q.CorrectAnswer,CultureInfo.InvariantCulture);
                        Assert.False(ExactSkillContractQuestionEngine.Verify(
                            family,q.Parameters,(a+11).ToString(CultureInfo.InvariantCulture)));
                        Assert.InRange(a,0,10);
                        if(family.Contains("choose_",StringComparison.Ordinal))
                        {
                            Assert.InRange(a,1,3);
                            Assert.Contains("Options:",q.Prompt);
                            Assert.Contains("1)",q.Prompt);
                            Assert.Contains("2)",q.Prompt);
                            Assert.Contains("3)",q.Prompt);
                            var choices=q.Prompt.Split(" Options: ",StringSplitOptions.None);
                            Assert.Equal(2,choices.Length);
                            var displayed=choices[1].Split(" | ",StringSplitOptions.None);
                            Assert.Equal(3,displayed.Length);
                            Assert.Equal(3,displayed.Distinct(StringComparer.Ordinal).Count());
                            Assert.StartsWith(a.ToString(CultureInfo.InvariantCulture)+") ",
                                displayed[a-1]);
                            if(family=="usccss.t001.g1.choose_dot_expression")
                                Assert.EndsWith(
                                    p["first"]+" + "+p["second"],displayed[a-1]);
                            else
                            {
                                string[] names=family switch
                                {
                                    "usccss.t001.g1.choose_tool_sort"=>
                                        ["Counters","Pattern blocks","Inch tiles"],
                                    "usccss.t001.g1.choose_shape_chart"=>
                                        ["Triangles","Squares","Circles"],
                                    _=>["Apples","Bananas","Oranges"]
                                };
                                for(var labelIndex=0;labelIndex<3;labelIndex++)
                                {
                                    var amount=labelIndex switch
                                    {
                                        0=>p["first"],1=>p["second"],_=>p["third"]
                                    };
                                    Assert.Contains(
                                        names[labelIndex]+": "+new string('●',amount),
                                        displayed[a-1]);
                                }
                            }
                            Assert.False(ExactSkillContractQuestionEngine.Verify(
                                family,p,(a%3+1).ToString(CultureInfo.InvariantCulture)));
                        }

                        if(n==3)Assert.InRange(q.Parameters["step"],1,2);
                        if(n==4)
                        {
                            Assert.InRange(q.Parameters["step"],1,2);
                            Assert.True(q.Parameters["start"]>=q.Parameters["step"]);
                        }
                        if((n==6 || n==10) && family.EndsWith("make_ten_missing_part",
                                StringComparison.Ordinal))
                            Assert.Equal(10,q.Parameters["known"]+q.Parameters["missing"]);
                        // Detect a dishonest "sorting" prompt that already
                        // displays the category count and requires no classification.
                        if((n is 7 or 8 or 9) &&
                           !family.Contains("choose_",StringComparison.Ordinal))
                        {
                            var label=n==7?"Tools: ":n==8?"Cards: ":"Unsorted votes: ";
                            var begin=q.Prompt.IndexOf(label,StringComparison.Ordinal);
                            Assert.True(begin>=0,"Missing unsorted items for "+code);
                            begin+=label.Length;
                            var end=q.Prompt.IndexOf(". ",begin,StringComparison.Ordinal);
                            Assert.True(end>begin,"Unsorted items not delimited");
                            var listed=q.Prompt[begin..end]
                                .Split(", ",StringSplitOptions.None);
                            string[] tokens=n switch
                            {
                                7=>["counter","pattern block","inch tile"],
                                8=>["▲","■","●"],
                                _=>["apple","banana","orange"]
                            };
                            Assert.Equal(q.Parameters["first"]+
                                q.Parameters["second"]+q.Parameters["third"],listed.Length);
                            Assert.Equal(q.Parameters["first"],listed.Count(x=>x==tokens[0]));
                            Assert.Equal(q.Parameters["second"],listed.Count(x=>x==tokens[1]));
                            Assert.Equal(q.Parameters["third"],listed.Count(x=>x==tokens[2]));
                            Assert.All(listed,x=>Assert.Contains(x,tokens));
                        }

                        checkedCount++;
                    }
        }
        Assert.Equal(570,checkedCount); // 19 family slots * 3 difficulties * 10 seeds
    }

    [Fact]
    public void NineLearnerRichV2LessonsTeachTheSpecificPublisherGoalAndPracticalActivities()
    {
        // Validate the actual learner-facing enhancement on the Rich V2 model.
        // The browser integration separately proves this model is rendered on
        // real logged-in student pages (not just present in helper tests).
        var append=typeof(RichLessonContentV2RuntimeComposer).GetMethod(
            "AddT001LearnerTeaching",BindingFlags.NonPublic|BindingFlags.Static);
        Assert.NotNull(append);
        static RichLessonContentV2Lesson Empty(string code) => new()
        {
            LessonCode=code,CultureCode="en",Title="T001 independent model",
            ExplanationParagraphs=[],KeyConcepts=[],WorkedExamples=[],
            CommonMistakes=[],SummaryPoints=[],Visuals=[],Videos=[]
        };
        for(var n=2;n<=10;n++)
        {
            var code=Prefix+n.ToString("D2");
            var lesson=Empty(code);
            append!.Invoke(null,[code,lesson]);
            Assert.StartsWith("Learning goal: ",lesson.ExplanationParagraphs[0]);
            Assert.StartsWith("Try",lesson.ExplanationParagraphs[1]);
            Assert.Equal("Worked model — lesson skill",lesson.WorkedExamples[0].Title);
            Assert.NotEmpty(lesson.WorkedExamples[0].Steps);
            Assert.NotEmpty(lesson.WorkedExamples[0].Check);
            Assert.NotEmpty(lesson.CommonMistakes[0].Correction);
            if(n==6)
            {
                Assert.Contains("Find the Pair",lesson.ExplanationParagraphs[1],
                    StringComparison.OrdinalIgnoreCase);
                Assert.Contains("10 - 7 = 3",lesson.WorkedExamples[0].Check);
            }
            if(n is 7 or 8)
            {
                Assert.Contains("sort",lesson.ExplanationParagraphs[1],
                    StringComparison.OrdinalIgnoreCase);
                Assert.Contains(n==7?"once":"one",lesson.CommonMistakes[0].Correction,
                    StringComparison.OrdinalIgnoreCase);
            }
            if(n==9)
            {
                Assert.Contains("real survey",lesson.ExplanationParagraphs[1],
                    StringComparison.OrdinalIgnoreCase);
                Assert.Contains("fictional votes",lesson.ExplanationParagraphs[1],
                    StringComparison.OrdinalIgnoreCase);
            }
        }
        var pilot=Empty(Prefix+"01");
        append!.Invoke(null,[Prefix+"01",pilot]);
        Assert.Empty(pilot.WorkedExamples);
        Assert.Empty(pilot.ExplanationParagraphs);
    }

    [Theory]
    [InlineData("usccss.t001.g1.choose_tool_sort")]
    [InlineData("usccss.t001.g1.choose_shape_chart")]
    [InlineData("usccss.t001.g1.choose_survey_table")]
    public void EachCategorizationRepresentationFamilySupportsTenDistinctWithinTenChoices(
        string family)
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var seed in new[]{913456,913457,913458})
        {
            var questions=engine.Generate(
                "t001-full-category-session-capacity",family,[family],
                ExactSkillQuestionDifficulty.Standard,10,seed,[]);
            Assert.Equal(10,questions.Count);
            Assert.Equal(10,questions.Select(x=>x.ExposureFingerprint)
                .Distinct(StringComparer.Ordinal).Count());
            Assert.Equal(10,questions.Select(x=>
                string.Join(",",new[]{
                    x.Parameters["first"],x.Parameters["second"],x.Parameters["third"]
                })).Distinct(StringComparer.Ordinal).Count());
            foreach(var q in questions)
            {
                var p=q.Parameters;
                Assert.All(new[]{"first","second","third"},
                    label=>Assert.InRange(p[label],1,6));
                Assert.Equal(3,new[]{p["first"],p["second"],p["third"]}
                    .Distinct().Count());
                Assert.InRange(p["first"]+p["second"]+p["third"],6,10);
                Assert.True(ExactSkillContractQuestionEngine.Verify(
                    family,p,q.CorrectAnswer));
            }
        }
    }

    [Fact]
    public void CanonicalL05AndL06HaveCorrectedWorkedMathAndDoNotModifyPilot()
    {
        var pairs=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(doc=>doc.Lessons.Select(lesson=>(Doc:doc,Lesson:lesson)))
            .Where(x=>x.Lesson.LessonCode.StartsWith(Prefix,StringComparison.Ordinal))
            .ToArray();
        Assert.Contains(pairs,x=>x.Lesson.LessonCode==Prefix+"01");
        var l05=pairs.Single(x=>x.Lesson.LessonCode==Prefix+"05");
        Assert.Contains("3 + 2 = 5",l05.Lesson.Translations.Single().WorkedExamples);
        Assert.Contains("6 - 2 = 4",l05.Lesson.Translations.Single().WorkedExamples);
        Assert.Equal(CommonCoreT001MixedAddSubtractContentCorrection.CorrectedVersion,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(l05.Doc,l05.Lesson));
        var l06=pairs.Single(x=>x.Lesson.LessonCode==Prefix+"06");
        Assert.Contains("6 + 4 = 10",l06.Lesson.Translations.Single().WorkedExamples);
        Assert.DoesNotContain("flash cards",l06.Lesson.Translations.Single().WorkedExamples,
            StringComparison.OrdinalIgnoreCase);
        Assert.Equal(CommonCoreT001FindThePairContentCorrection.CorrectedVersion,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(l06.Doc,l06.Lesson));
    }
}

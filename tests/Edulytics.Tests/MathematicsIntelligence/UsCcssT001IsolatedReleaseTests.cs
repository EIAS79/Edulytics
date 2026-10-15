using System.Globalization;
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
                        Assert.True(ExactSkillContractQuestionEngine.Verify(
                            family,q.Parameters,q.CorrectAnswer));
                        var a=int.Parse(q.CorrectAnswer,CultureInfo.InvariantCulture);
                        Assert.False(ExactSkillContractQuestionEngine.Verify(
                            family,q.Parameters,(a+11).ToString(CultureInfo.InvariantCulture)));
                        Assert.InRange(a,0,10);
                        if(n==3)Assert.InRange(q.Parameters["step"],1,2);
                        if(n==4)
                        {
                            Assert.InRange(q.Parameters["step"],1,2);
                            Assert.True(q.Parameters["start"]>=q.Parameters["step"]);
                        }
                        if(n==6 || (n==10 && family.EndsWith("make_ten_missing_part",
                                StringComparison.Ordinal)))
                            Assert.Equal(10,q.Parameters["known"]+q.Parameters["missing"]);
                        // Detect a dishonest "sorting" prompt that already
                        // displays the category count and requires no classification.
                        if(n is 7 or 8 or 9)
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
                                8=>["triangle","square","circle"],
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
        Assert.Equal(300,checkedCount); // 10 family slots across L02-L10 * 3 * 10
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

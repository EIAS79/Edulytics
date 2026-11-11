using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit1Lesson03CompareTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:01:03";

    [Fact]
    public void G4ComparisonThreeFamiliesUseSharedExactPractice()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal(3,contract!.AllowedQuestionFamilies.Count);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var family in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
            Assert.NotNull(ui);
            for(int seed=1;seed<=6;seed++)
            foreach(var q in engine.Generate(Lesson,family,[family],
                ExactSkillQuestionDifficulty.Standard,2,seed,[]))
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"not an answer"));
                Assert.Equal(2,q.Parameters.Count);
                Assert.DoesNotContain("result",q.Parameters.Keys);
                var modified=q.Parameters.ToDictionary(x=>x.Key,x=>x.Value);
                modified["a"]=10_000_000;
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,modified,q.CorrectAnswer));
            }
        }
    }

    [Fact]
    public void GreaterSmallerAndRelationRejectWrongNumbers()
    {
        var p=new Dictionary<string,int>{{"a",6_304_820},{"b",6_304_280}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u01.l03.compare_whole_numbers",p,">"));
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g4.u01.l03.compare_whole_numbers",p,"<"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u01.l03.select_greater",p,"٦٬٣٠٤٬٨٢٠"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u01.l03.select_smaller",p,"6304280"));
        p["b"]=p["a"];
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u01.l03.compare_whole_numbers",p,"="));
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g4.u01.l03.select_greater",p,"6304820"));
    }

    [Fact]
    public void ArabicRichLessonUsesOriginalNumberLineAndThreeExamples()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Equal("مقارنة الأعداد",l!.Title);
        Assert.True(l.WorkedExamples.Count>=3);
        Assert.True(l.KeyConcepts.Count>=3);
        Assert.Equal(RichLessonVisualKind.NumberLine,Assert.Single(l.Visuals).Kind);
        Assert.Contains(l.CommonMistakes,x=>x.Mistake.Contains("الآحاد"));
    }
}

using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit2Lesson06SubtractionTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:02:06";
    [Fact]
    public void ThreeSubtractionFamiliesRouteAndValidateParameters()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal(3,contract!.AllowedQuestionFamilies.Count);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var f in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
            Assert.NotNull(ui);
            for(int seed=1;seed<=5;seed++)
            foreach(var q in engine.Generate(Lesson,f,[f],
                ExactSkillQuestionDifficulty.Standard,1,seed,[]))
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,"-999"));
                Assert.Equal(2,q.Parameters.Count);
                Assert.DoesNotContain("result",q.Parameters.Keys);
                var altered=q.Parameters.ToDictionary(x=>x.Key,x=>x.Value);
                altered["a"]=1;
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,altered,q.CorrectAnswer));
            }
        }
    }
    [Fact]
    public void BorrowAndMissingSubtrahendAgreeWithIndependentArithmetic()
    {
        var p=new Dictionary<string,int>{{"a",3214},{"b",957}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u02.l06.subtract_four_digits",p,"٢٬٢٥٧"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u02.l06.missing_subtrahend",p,"957"));
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g4.u02.l06.subtract_four_digits",p,"2256"));
        p["a"]=4275;p["b"]=1345;
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u02.l06.subtract_four_digits",p,"2930"));
        p["a"]=632500;p["b"]=284100;
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u02.l06.subtract_six_digits",p,"348400"));
    }
    [Fact]
    public void StudentArabicRichLessonCoversSubtractionAndPlaceValue()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Equal("طرح الأعداد الكلية",l!.Title);
        Assert.True(l.WorkedExamples.Count>=3);
        Assert.Equal(RichLessonVisualKind.PlaceValueChart,Assert.Single(l.Visuals).Kind);
        Assert.Contains(l.CommonMistakes,x=>x.Mistake.Contains("إنقاص"));
    }
}

using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit4Lesson08ColumnRegroupingTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:04:08";
    [Fact]
    public void RegroupingFamiliesGenerateAndVerify()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal(2,contract!.AllowedQuestionFamilies.Count);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var f in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
            Assert.NotNull(ui);
            foreach(var q in engine.Generate(Lesson,f,[f],ExactSkillQuestionDifficulty.Standard,3,20261111,[]))
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,"-4"));
                Assert.Equal(4,q.Parameters.Count);
            }
        }
    }
    [Fact]
    public void RegroupedTextbookExamplesAreCorrect()
    {
        var p=new Dictionary<string,int>{{"multiplier",5},{"tens",3},{"ones",7}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u04.l03.model_two_digit_product",p,"١٨٥"));
        p["multiplier"]=6;p["ones"]=8;
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u04.l03.model_two_digit_product",p,"٢٢٨"));
        p["multiplier"]=7;p["tens"]=6;
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u04.l03.model_two_digit_product",p,"٤٧٦"));
    }
    [Fact]
    public void ArabicRichLessonSupportsCarrying()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Equal("الضرب مع إعادة التجميع",l!.Title);
        Assert.True(l.WorkedExamples.Count>=3);
        Assert.Equal(RichLessonVisualKind.EquationSet,Assert.Single(l.Visuals).Kind);
    }
}

using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit4Lesson06RegroupingTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:04:06";
    [Fact]
    public void RegroupingLessonReusesExactBaseTenFamilies()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal(2,contract!.AllowedQuestionFamilies.Count);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var family in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
            Assert.NotNull(ui);
            foreach(var q in engine.Generate(Lesson,family,[family],ExactSkillQuestionDifficulty.Standard,2,20261111,[]))
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"-3"));
                Assert.Equal(4,q.Parameters.Count);
            }
        }
    }
    [Fact]
    public void TextbookRegroupingExamplesHaveExpectedAnswers()
    {
        var p=new Dictionary<string,int>{{"multiplier",3},{"tens",2},{"ones",6}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u04.l03.model_two_digit_product",p,"٧٨"));
        p["multiplier"]=4;p["tens"]=3;p["ones"]=1;
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u04.l03.model_two_digit_product",p,"١٢٤"));
    }
    [Fact]
    public void RichArabicStudentLessonMatchesRegrouping()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Equal("نشاط عملي: تمثيل إعادة التجميع",l!.Title);
        Assert.True(l.WorkedExamples.Count>=3);
        Assert.Equal(RichLessonVisualKind.EquationSet,Assert.Single(l.Visuals).Kind);
    }
}

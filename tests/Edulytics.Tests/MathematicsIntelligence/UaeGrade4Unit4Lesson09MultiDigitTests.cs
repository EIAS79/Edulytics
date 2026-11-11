using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit4Lesson09MultiDigitTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:04:09";
    [Fact]
    public void MultidigitFamiliesGenerateWithExactAnswers()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal(3,contract!.AllowedQuestionFamilies.Count);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var f in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
            Assert.NotNull(ui);
            foreach(var q in engine.Generate(Lesson,f,[f],ExactSkillQuestionDifficulty.Standard,3,20261111,[]))
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,"-9"));
                Assert.Equal(2,q.Parameters.Count);
                var invalid=q.Parameters.ToDictionary(x=>x.Key,x=>x.Value);
                invalid["number"]=0;
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,invalid,q.CorrectAnswer));
            }
        }
    }
    [Fact]
    public void TextbookExamplesOfThreeAndFourDigitMultiplicationAreCorrect()
    {
        var p=new Dictionary<string,int>{{"factor",9},{"number",365}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u04.l09.multidigit_product",p,"٣٬٢٨٥"));
        p["factor"]=3;p["number"]=2763;
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u04.l09.multidigit_product",p,"٨٢٨٩"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u04.l09.partial_hundreds",p,"٢١٠٠"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u04.l09.round_estimate",p,"٩٠٠٠"));
    }
    [Fact]
    public void ArabicRichLessonContainsMultidigitWorkedExamples()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Equal("الضرب في عدد متعدد الأرقام",l!.Title);
        Assert.True(l.WorkedExamples.Count>=3);
        Assert.Equal(RichLessonVisualKind.EquationSet,Assert.Single(l.Visuals).Kind);
    }
}

using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit9Lesson04SubtractionTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:09:04";
    [Fact]
    public void SubtractionFamiliesGenerateAndVerify()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal(3,contract!.AllowedQuestionFamilies.Count);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var family in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
            Assert.NotNull(ui);
            foreach(var q in engine.Generate(Lesson,family,[family],ExactSkillQuestionDifficulty.Standard,3,20261111,[]))
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"-1"));
            }
        }
    }
    [Theory]
    [InlineData(10,7,5,"2","1","5")]
    [InlineData(8,7,2,"5","5","8")]
    [InlineData(6,5,3,"2","1","3")]
    [InlineData(12,9,3,"6","1","2")]
    public void DifferenceAndSimplifiedResultAreCorrect(int d,int a,int b,string raw,string n,string den)
    {
        var p=new Dictionary<string,int>{{"denominator",d},{"a",a},{"b",b}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u09.l04.difference_numerator",p,raw));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u09.l04.reduced_difference_numerator",p,n));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u09.l04.reduced_difference_denominator",p,den));
    }
    [Fact]
    public void RejectsInvalidSubtraction()
    {
        var p=new Dictionary<string,int>{{"denominator",8},{"a",3},{"b",5}};
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g4.u09.l04.difference_numerator",p,"-2"));
    }
    [Fact]
    public void ArabicRichSourceIncludesReductionAndReverseCheck()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Equal("طرح الكسور المتشابهة",l!.Title);
        Assert.True(l.WorkedExamples.Count>=4);
        Assert.True(l.KeyConcepts.Count>=3);
        Assert.Equal(RichLessonVisualKind.EquationSet,Assert.Single(l.Visuals).Kind);
    }
}

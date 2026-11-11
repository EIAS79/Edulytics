using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit8Lesson03EquivalentFractionModelsTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:08:03";
    [Fact]
    public void FractionModelsAndNumberLinesGenerateAndVerify()
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
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,"0"));
            }
        }
    }
    [Theory]
    [InlineData(1,3,2,"2")]
    [InlineData(1,4,3,"3")]
    [InlineData(2,5,4,"8")]
    public void EqualAreaAndNumberLineMarkHaveSameScaledNumerator(int n,int d,int scale,string expected)
    {
        var p=new Dictionary<string,int>{{"numerator",n},{"denominator",d},{"factor",scale}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u08.l03.fraction_strip_equivalence",p,expected));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u08.l03.number_line_equivalence",p,expected));
    }
    [Fact]
    public void InvalidScalingAndMismatchAreRejected()
    {
        var p=new Dictionary<string,int>{{"numerator",3},{"denominator",3},{"factor",2}};
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g4.u08.l03.fraction_strip_equivalence",p,"6"));
    }
    [Fact]
    public void ArabicRichLessonContainsOriginalFractionModelExamples()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Equal("نشاط عملي: تمثيل الكسور المكافئة",l!.Title);
        Assert.True(l.WorkedExamples.Count>=4);
        Assert.True(l.KeyConcepts.Count>=3);
        Assert.Equal(RichLessonVisualKind.EquationSet,Assert.Single(l.Visuals).Kind);
    }
}

using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit8Lesson06FractionComparisonTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:08:06";
    [Fact]
    public void ComparisonFamiliesGenerateVerifyAndRejectWrongAnswers()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal(2,contract!.AllowedQuestionFamilies.Count);
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
    [InlineData(3,4,1,2,"2","2")]
    [InlineData(2,5,2,3,"0","1")]
    [InlineData(2,6,1,3,"1","0")]
    [InlineData(1,3,3,4,"0","1")]
    public void ComparisonsMatchCrossMultiplication(int n1,int d1,int n2,int d2,string sign,string smaller)
    {
        var p=new Dictionary<string,int>{{"numerator1",n1},{"denominator1",d1},{"numerator2",n2},{"denominator2",d2}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u08.l06.compare_fraction_sign",p,sign));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u08.l06.select_smaller_fraction",p,smaller));
    }
    [Fact]
    public void InvalidDenominatorIsRejected()
    {
        var p=new Dictionary<string,int>{{"numerator1",1},{"denominator1",0},{"numerator2",1},{"denominator2",3}};
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g4.u08.l06.compare_fraction_sign",p,"0"));
    }
    [Fact]
    public void ArabicRichContainsEquivalentDenominatorAndOrderingExamples()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Equal("مقارنة الكسور وترتيبها",l!.Title);
        Assert.True(l.WorkedExamples.Count>=4);
        Assert.True(l.KeyConcepts.Count>=3);
        Assert.Equal(RichLessonVisualKind.EquationSet,Assert.Single(l.Visuals).Kind);
    }
}

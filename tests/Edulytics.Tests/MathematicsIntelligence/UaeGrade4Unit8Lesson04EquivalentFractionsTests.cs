using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit8Lesson04EquivalentFractionsTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:08:04";
    [Fact]
    public void EquivalentFractionFamiliesGenerateAndVerify()
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
    [InlineData(3,4,2,"6")]
    [InlineData(3,4,3,"9")]
    [InlineData(2,5,4,"8")]
    public void ScalingBothPartsPreservesFractionValue(int numerator,int denominator,int factor,string answer)
    {
        var p=new Dictionary<string,int>{{"numerator",numerator},{"denominator",denominator},{"factor",factor}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u08.l04.multiply_fraction_pair",p,answer));
        Assert.Equal(numerator*(denominator*factor),int.Parse(answer)*denominator);
    }
    [Fact]
    public void OnlyValidScalingIsAccepted()
    {
        var p=new Dictionary<string,int>{{"numerator",2},{"denominator",4},{"factor",0}};
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g4.u08.l04.multiply_fraction_pair",p,"0"));
    }
    [Fact]
    public void RichArabicLessonIsSourceAligned()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Equal("الكسور المكافئة",l!.Title);
        Assert.True(l.WorkedExamples.Count>=4);
        Assert.True(l.KeyConcepts.Count>=3);
        Assert.Equal(RichLessonVisualKind.EquationSet,Assert.Single(l.Visuals).Kind);
    }
}

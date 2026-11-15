using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;

// Grade6 general Arabic student PDF100-107. Tests written, not manually run A-B-C.
public sealed class UaeGrade6Unit1Lesson09EquivalentRatiosTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L6:GENERAL:01:09:STUDENT-AR-128";
    [Fact] public void ArabicRichContentHasEquivalentAndNonEquivalentExamples()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Equal("النسب المتكافئة",l!.Title);
        Assert.True(l.WorkedExamples.Count>=5);
        Assert.Contains(l.Visuals,v=>v.Kind==RichLessonVisualKind.RatioTable);
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.NotNull(contract);
        Assert.Equal(3,contract!.AllowedQuestionFamilies.Count);
    }
    [Fact] public void ExactExistingFamiliesVerifyEquivalentRatiosAndArabicPrompts()
    {
        var eng=new ExactSkillContractQuestionEngine();
        foreach(var f in new[]{
            "ratio.unit_rate.direct",
            "ratio.unit_rate.equivalent_ratio",
            "fractions.equivalent.reduce_common_factor"})
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out _));
            foreach(var q in eng.Generate(Lesson,Lesson,[f],ExactSkillQuestionDifficulty.Standard,3,20261114,[]))
            {
                Assert.True(q.Prompt.Contains("نسب") || q.Prompt.Contains("وحدة"));
                Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,"-999"));
            }
        }
    }
    [Theory]
    [InlineData(10,2,30,6,true)]
    [InlineData(20,5,45,9,false)]
    [InlineData(3,45,5,75,true)]
    [InlineData(60,3,90,6,false)]
    public void IndependentCrossProductsCheckRealWorldCases(
        int first,int second,int third,int fourth,bool same)
        => Assert.Equal(same,first*fourth==second*third);
}

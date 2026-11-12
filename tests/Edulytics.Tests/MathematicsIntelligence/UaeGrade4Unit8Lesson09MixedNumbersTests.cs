using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit8Lesson09MixedNumbersTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:08:09";
    [Fact]
    public void MixedNumberPracticeGeneratesAndVerifies()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var c));
        Assert.Equal(2,c!.AllowedQuestionFamilies.Count);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var f in c.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
            Assert.NotNull(ui);
            foreach(var q in engine.Generate(Lesson,f,[f],ExactSkillQuestionDifficulty.Standard,3,20261111,[]))
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,"-1"));
            }
        }
    }
    [Theory]
    [InlineData(2,3,4,"11")]
    [InlineData(1,1,4,"5")]
    [InlineData(3,2,3,"11")]
    [InlineData(5,1,2,"11")]
    public void MixedNumberConvertsToUnitFractionCount(int whole,int numerator,int denominator,string answer)
    {
        var p=new Dictionary<string,int>{{"whole",whole},{"numerator",numerator},{"denominator",denominator}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u08.l09.mixed_unit_fraction_count",p,answer));
    }
    [Fact]
    public void FourthsFamilyRequiresDenominatorFour()
    {
        var valid=new Dictionary<string,int>{{"whole",2},{"numerator",3},{"denominator",4}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u08.l09.mixed_as_fourths",valid,"١١"));
        valid["denominator"]=5;
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g4.u08.l09.mixed_as_fourths",valid,"13"));
    }
    [Fact]
    public void ImproperFractionParametersAreRejected()
    {
        var p=new Dictionary<string,int>{{"whole",1},{"numerator",4},{"denominator",4}};
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g4.u08.l09.mixed_unit_fraction_count",p,"8"));
    }
    [Fact]
    public void RichArabicLessonIsValid()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Equal("الأعداد الكسرية",l!.Title);
        Assert.True(l.WorkedExamples.Count>=4);
        Assert.True(l.KeyConcepts.Count>=3);
        Assert.Equal(RichLessonVisualKind.EquationSet,Assert.Single(l.Visuals).Kind);
    }
}

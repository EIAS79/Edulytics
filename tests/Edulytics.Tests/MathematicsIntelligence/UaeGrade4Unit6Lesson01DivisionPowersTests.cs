using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit6Lesson01DivisionPowersTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:06:01";
    [Fact]
    public void PlaceValueDivisionFamiliesGenerateAndVerify()
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
                Assert.Equal(3,q.Parameters.Count);
            }
        }
    }
    [Fact]
    public void MultiplesOfHundredAndThousandKeepPlaceValue()
    {
        var p=new Dictionary<string,int>{{"factor",5},{"digits",3},{"zeros",2}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u06.l01.divide_powers_10_100_1000",p,"٣٠٠"));
        p["factor"]=4;p["digits"]=6;
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u06.l01.divide_powers_10_100_1000",p,"٦٠٠"));
        p["zeros"]=3;
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u06.l01.missing_quotient_place_value",p,"٦٠٠٠"));
    }
    [Fact]
    public void RichArabicLessonShowsDivisionPatterns()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Contains("قسمة مضاعفات",l!.Title);
        Assert.True(l.WorkedExamples.Count>=3);
        Assert.Equal(RichLessonVisualKind.EquationSet,Assert.Single(l.Visuals).Kind);
    }
}

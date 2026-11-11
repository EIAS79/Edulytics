using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

// Source: grade 4 student 04-01.pdf, printed pp. 11-16.
// Written during A-B-C. Test execution is recorded separately.
public sealed class UaeGrade4Unit1Lesson01PlaceValueTests
{
    private const string Lesson = "PED:UAE-MOE-MATH:L4:COMMON:01:01";

    [Fact]
    public void ExactThreeFamiliesRouteToG4LearnerContract()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson, out var contract));
        Assert.Equal(3, contract!.AllowedQuestionFamilies.Count);
        Assert.Equal("number.place_value.to_millions", contract.SkillId);
        foreach(var family in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family, out var ui));
            Assert.NotNull(ui);
        }
    }

    [Fact]
    public void PlaceValueToMillionsGeneratesSolvesAndVerifiesAcrossSeeds()
    {
        var engine = new ExactSkillContractQuestionEngine();
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson, out var contract));
        foreach(var family in contract!.AllowedQuestionFamilies)
        for(int seed=1;seed<=10;seed++)
        foreach(var item in engine.Generate(Lesson, family, [family],
                    ExactSkillQuestionDifficulty.Standard, 2, seed, []))
        {
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                family, item.Parameters, item.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(
                family, item.Parameters, "-999"));
            Assert.False(ExactSkillContractQuestionEngine.Verify(
                family, item.Parameters, "1.5"));
            var arabic = new string(item.CorrectAnswer.Select(x =>
                x is >= '0' and <= '9' ? (char)('٠' + x - '0') : x).ToArray());
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                family, item.Parameters, arabic));
            var corrupted = item.Parameters.ToDictionary(x=>x.Key,x=>x.Value);
            corrupted["result"] = -9;
            Assert.False(ExactSkillContractQuestionEngine.Verify(
                family, corrupted, item.CorrectAnswer));
        }
    }

    [Fact]
    public void SevenDigitBoundariesAndZeroPlaceHoldWithoutOverflow()
    {
        const string f = "uae.g4.u01.l01.digit_value_to_millions";
        var p = new Dictionary<string,int>{{"number",9_000_005},{"pos",0},{"result",9_000_000}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(f,p,"٩٬٠٠٠٬٠٠٠"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(f,p,"900000"));
        p["pos"]=1;p["result"]=0;
        Assert.True(ExactSkillContractQuestionEngine.Verify(f,p,"٠"));
        p["number"]=10_000_000;
        Assert.False(ExactSkillContractQuestionEngine.Verify(f,p,"0"));
    }
}

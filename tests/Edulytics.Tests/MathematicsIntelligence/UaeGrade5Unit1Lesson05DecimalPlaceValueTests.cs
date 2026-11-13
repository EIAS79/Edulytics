using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

// UAE G5 Arabic Student, printed pp37–42. Stage C test source, NOT_RUN.
public sealed class UaeGrade5Unit1Lesson05DecimalPlaceValueTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L5:GENERAL:01:05";
    private const string Digit="uae.g5.u01.l05.decimal_digit_place_value";
    private const string Factor="uae.g5.u01.l05.place_factor_ten";

    [Fact]
    public void Grade5ReusesG4PlaceValueAndRegistersTwoExactExtensions()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal("decimals.place_value",contract!.SkillId);
        Assert.Equal(3,contract.AllowedQuestionFamilies.Count);
        foreach(var family in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
            Assert.NotNull(ui);
        }
    }

    [Fact]
    public void RepeatedDigitsHaveDifferentValuesAtDifferentPlaces()
    {
        var p=new Dictionary<string,int>
        { ["numerator"]=555,["denominator"]=1000,["place"]=1 };
        Assert.True(ExactSkillContractQuestionEngine.Verify(Digit,p,"٠٫٥"));
        p["place"]=2;
        Assert.True(ExactSkillContractQuestionEngine.Verify(Digit,p,"0.05"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Digit,p,"0.5"));
        p["place"]=3;
        Assert.True(ExactSkillContractQuestionEngine.Verify(Digit,p,"0.005"));
        p["place"]=4;
        Assert.False(ExactSkillContractQuestionEngine.Verify(Digit,p,"0.0005"));
    }

    [Fact]
    public void OnePlaceRatioIsTenAndTwoPlacesIsOneHundred()
    {
        var p=new Dictionary<string,int>
        { ["digit"]=7,["largerPlace"]=1,["smallerPlace"]=2 };
        Assert.True(ExactSkillContractQuestionEngine.Verify(Factor,p,"١٠"));
        p["smallerPlace"]=3;
        Assert.True(ExactSkillContractQuestionEngine.Verify(Factor,p,"100"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Factor,p,"10"));
        p["largerPlace"]=3;
        Assert.False(ExactSkillContractQuestionEngine.Verify(Factor,p,"1"));
    }

    [Fact]
    public void ExactGrade5PlaceFamiliesGenerateValidQuestionsAcrossSeeds()
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var family in new[]{Digit,Factor})
        for(int seed=1;seed<=8;seed++)
        foreach(var item in engine.Generate(Lesson,family,[family],
            ExactSkillQuestionDifficulty.Standard,2,seed,[]))
        {
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                family,item.Parameters,item.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,"-123"));
        }
    }
}

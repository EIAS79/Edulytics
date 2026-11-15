using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG6U6L02PowersTests
{
    const string L="PED:UAE-MOE-MATH:L6:GENERAL:06:02:STUDENT-AR-128";
    [Fact] public void ArabicRichLessonAndExactPracticeHaveSevenFamilies()
    {
        var rich=RichLessonContentV2Registry.Find(L,"ar");
        Assert.NotNull(rich);
        Assert.Equal(10,rich!.WorkedExamples.Count);
        Assert.All(rich.WorkedExamples,e=>Assert.True(e.Steps.Count>=3));
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(L,out var c));
        Assert.Equal(7,c!.AllowedQuestionFamilies.Count);
        foreach(var f in c.AllowedQuestionFamilies)
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
    }
    [Fact] public void RepeatedFactorsAndZeroExponent()
    {
        const string repeated="uae.g6.u06.l02.repeated_to_power",power="uae.g6.u06.l02.integer_power";
        var p=new Dictionary<string,int>{{"b",6},{"e",4}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(repeated,p,"٦^٤"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(repeated,p,"24"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(power,p,"١٢٩٦"));
        p["e"]=0;
        Assert.True(ExactSkillContractQuestionEngine.Verify(power,p,"1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(repeated,p,"6^0"));
    }
    [Fact] public void DecimalPowersDoNotRound()
    {
        const string family="uae.g6.u06.l02.decimal_power";
        var p=new Dictionary<string,int>{{"b100",150},{"e",3}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,"٣٫٣٧٥"));
        p["b100"]=5;p["e"]=4;
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,"0.00000625"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"0"));
    }
    [Fact] public void RationalPowersAreSimplifiedExactly()
    {
        const string family="uae.g6.u06.l02.fraction_power";
        var p=new Dictionary<string,int>{{"n",1},{"d",2},{"e",3}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,"١/٨"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"1/6"));
        p["n"]=2;p["d"]=4;p["e"]=2;
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,"1/4"));
        p["e"]=5;
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"1/32"));
    }
    [Fact] public void TenPowersMixedOperationsAndSquareClassification()
    {
        var ten=new Dictionary<string,int>{{"e",6}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l02.ten_power",ten,"1000000"));
        var mixed=new Dictionary<string,int>{{"b",3},{"e",2},{"add",8}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l02.power_addition",mixed,"17"));
        var sq=new Dictionary<string,int>{{"b",7},{"offset",0}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l02.perfect_square",sq,"نعم"));
        sq["offset"]=1;
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l02.perfect_square",sq,"لا"));
    }
}

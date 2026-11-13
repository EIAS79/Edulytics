using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

// Grade 5 Arabic Student printed pp55–60. Stage C tests WRITTEN_NOT_RUN.
public sealed class UaeGrade5Unit1Lesson08DecimalOrderingTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L5:GENERAL:01:08";
    private const string Asc="uae.g5.u01.l08.order_decimals_ascending";
    private const string Desc="uae.g5.u01.l08.order_decimals_descending";

    [Fact]
    public void DecimalOrderFamiliesReuseCompareSkill()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal("decimals.compare",contract!.SkillId);
        Assert.Equal(3,contract.AllowedQuestionFamilies.Count);
        foreach(var f in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
            Assert.NotNull(ui);
        }
    }

    [Fact]
    public void AscendingAndDescendingRespectWholeNumbersAndDecimals()
    {
        var p=new Dictionary<string,int>
        { ["a"]=23_890,["b"]=22_650,["c"]=22_000,["d"]=23_250 };
        Assert.True(ExactSkillContractQuestionEngine.Verify(
            Asc,p,"22.000;22.650;23.250;23.890"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(
            Desc,p,"٢٣٫٨٩٠;٢٣٫٢٥٠;٢٢٫٦٥٠;٢٢٫٠٠٠"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            Asc,p,"22;23.89;23.25;22.65"));
        p["d"]=p["b"];
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            Asc,p,"22;22.65;23.25;23.89"));
    }

    [Fact]
    public void GeneratedDecimalOrderingHasExactVerifiedSeededAnswers()
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var f in new[]{Asc,Desc})
        for(int seed=1;seed<=8;seed++)
        foreach(var item in engine.Generate(Lesson,f,[f],
            ExactSkillQuestionDifficulty.Standard,2,seed,[]))
        {
            Assert.True(ExactSkillContractQuestionEngine.Verify(f,item.Parameters,item.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(f,item.Parameters,"0.01;0.01"));
        }
    }
}

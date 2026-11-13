using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

// Grade 5 Arabic Student 01.pdf printed pp49–54. Tests WRITTEN_NOT_RUN.
public sealed class UaeGrade5Unit1Lesson07DecimalComparisonTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L5:GENERAL:01:07";
    private const string Compare="uae.g5.u01.l07.compare_decimal_to_thousandths";
    private const string Greater="uae.g5.u01.l07.select_greater_decimal";

    [Fact]
    public void Grade5ReusesGrade4CompareAndAddsThousandthFamilies()
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
    public void ThousandthsCompareAndTrailingZerosAreExact()
    {
        var p=new Dictionary<string,int>
        { ["firstThousandths"]=98_546,["secondThousandths"]=98_654 };
        Assert.True(ExactSkillContractQuestionEngine.Verify(Compare,p,"<"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Compare,p,">"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Greater,p,"98.654"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Greater,p,"٩٨٫٦٥٤"));
        p["secondThousandths"]=98_546;
        Assert.True(ExactSkillContractQuestionEngine.Verify(Compare,p,"="));
        p["secondThousandths"]=300_001;
        Assert.False(ExactSkillContractQuestionEngine.Verify(Compare,p,"<"));
    }

    [Fact]
    public void GeneratedGrade5DecimalComparisonsVerifyForFixedSeeds()
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var f in new[]{Compare,Greater})
        for(int seed=1;seed<=8;seed++)
        foreach(var item in engine.Generate(Lesson,f,[f],
            ExactSkillQuestionDifficulty.Standard,2,seed,[]))
        {
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                f,item.Parameters,item.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(f,item.Parameters,"BAD"));
        }
    }
}

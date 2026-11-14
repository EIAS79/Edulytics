using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

// UAE Grade5 Local Arabic Student printed pp43-48.
// Stage C authored tests, NOT_RUN.
public sealed class UaeGrade5Unit1Lesson06DecimalExpansionTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L5:GENERAL:01:06";
    private const string Expanded="uae.g5.u01.l06.expanded_decimal_to_standard";

    [Fact]
    public void Grade5SixthLessonUsesThreeExactFamilies()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal("decimals.place_value",contract!.SkillId);
        Assert.Equal(3,contract.AllowedQuestionFamilies.Count);
        foreach(var family in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var interaction));
            Assert.NotNull(interaction);
        }
    }

    [Fact]
    public void ExpandedDecimalUsesThousandthsWithTrailingZeroPreserved()
    {
        var p=new Dictionary<string,int>
        { ["whole"]=5,["tenths"]=6,["hundredths"]=1,["thousandths"]=4 };
        Assert.True(ExactSkillContractQuestionEngine.Verify(Expanded,p,"٥٫٦١٤"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Expanded,p,"5.641"));
        p["whole"]=209;p["tenths"]=1;p["hundredths"]=0;p["thousandths"]=6;
        Assert.True(ExactSkillContractQuestionEngine.Verify(Expanded,p,"209.106"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Expanded,p,"209.16"));
        p["thousandths"]=10;
        Assert.False(ExactSkillContractQuestionEngine.Verify(Expanded,p,"209.110"));
    }

    [Fact]
    public void ExpandedFamilyEmitsVerifiedAnswersAcrossFixedSeeds()
    {
        var engine=new ExactSkillContractQuestionEngine();
        for(int seed=1;seed<=8;seed++)
        foreach(var item in engine.Generate(Lesson,Expanded,[Expanded],
            ExactSkillQuestionDifficulty.Standard,2,seed,[]))
        {
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                Expanded,item.Parameters,item.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(
                Expanded,item.Parameters,"-1"));
            Assert.DoesNotContain("result",item.Parameters.Keys);
        }
    }
}

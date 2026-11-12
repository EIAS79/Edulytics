using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

// Student book 04-01.pdf, printed pages 11-16, independent authored fixtures.
public sealed class UaeGrade4Unit1Lesson01PlaceValueTests
{
    private const string Lesson = "PED:UAE-MOE-MATH:L4:COMMON:01:01";

    [Fact]
    public void G4LearnerContractRoutesThreeFamiliesWithoutAnswerInPayload()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal(3,contract!.AllowedQuestionFamilies.Count);
        Assert.Equal("number.place_value.to_millions",contract.SkillId);
        var engine = new ExactSkillContractQuestionEngine();
        foreach(var family in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
            Assert.NotNull(ui);
            foreach(var item in engine.Generate(Lesson,family,[family],
                     ExactSkillQuestionDifficulty.Standard,1,20261111,[]))
            {
                Assert.DoesNotContain("result",item.Parameters.Keys);
                Assert.DoesNotContain("number",item.Parameters.Keys);
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,item.CorrectAnswer));
            }
        }
    }

    [Fact]
    public void MillionPlaceValueUsesIndependentDigitsAndRejectsBadAnswersAcrossSeeds()
    {
        var engine = new ExactSkillContractQuestionEngine();
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        foreach(var family in contract!.AllowedQuestionFamilies)
        for(int seed=1;seed<=10;seed++)
        foreach(var q in engine.Generate(Lesson,family,[family],
                ExactSkillQuestionDifficulty.Standard,2,seed,[]))
        {
            Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"-999"));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"1.5"));
            string arabic = new(q.CorrectAnswer.Select(c =>
                c is >= '0' and <= '9' ? (char)('٠'+c-'0') : c).ToArray());
            Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,arabic));
            var corrupted=q.Parameters.ToDictionary(x=>x.Key,x=>x.Value);
            corrupted["d0"]=10;
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,corrupted,q.CorrectAnswer));
        }
    }

    [Fact]
    public void ZeroMiddleDigitsAndSevenDigitBoundsAreSafe()
    {
        const string f="uae.g4.u01.l01.digit_value_to_millions";
        var p=new Dictionary<string,int>
        {
            ["pos"]=0,["d0"]=9,["d1"]=0,["d2"]=0,["d3"]=0,
            ["d4"]=0,["d5"]=0,["d6"]=5
        };
        Assert.True(ExactSkillContractQuestionEngine.Verify(f,p,"٩٬٠٠٠٬٠٠٠"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(f,p,"900000"));
        p["pos"]=1;
        Assert.True(ExactSkillContractQuestionEngine.Verify(f,p,"٠"));
        p["d0"]=0;
        Assert.False(ExactSkillContractQuestionEngine.Verify(f,p,"0"));
    }
}

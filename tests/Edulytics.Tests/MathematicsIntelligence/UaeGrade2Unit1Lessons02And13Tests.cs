using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

// Stage A-B-C WRITTEN_NOT_RUN; these are intentionally deferred to Stage D.
public sealed class UaeGrade2Unit1Lessons02And13Tests
{
    [Theory]
    [InlineData("uae.g2.u01.l02.count_on_number_line")]
    [InlineData("uae.g2.u01.l13.two_step_story")]
    public void SourceBoundedQuestions_GenerateVerifyAndRejectBadAnswer(string family)
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        var engine = new ExactSkillContractQuestionEngine();
        var q = engine.Generate("uae-g2-unit1", family, new[] {family},
            ExactSkillQuestionDifficulty.Standard, 4, 220013, Array.Empty<string>());
        Assert.NotEmpty(q);
        foreach(var item in q)
        {
            Assert.True(ExactSkillContractQuestionEngine.Verify(family,
                item.Parameters,item.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,"999"));
        }
    }
    [Fact]
    public void ForgedTotalRejectedAndArabicDigitsAccepted()
    {
        const string fam = "uae.g2.u01.l13.two_step_story";
        var p = new Dictionary<string,int>{
            ["start"]=7, ["add"]=4, ["remove"]=3, ["mode"]=0, ["result"]=8 };
        Assert.True(ExactSkillContractQuestionEngine.Verify(fam,p,"٨"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(fam,p,"7"));
        p["result"]=99;
        Assert.False(ExactSkillContractQuestionEngine.Verify(fam,p,"99"));
    }
}

using System.Globalization;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

// Owner-selected Arabic Student source, Unit 11 lesson 1, pp. 791–796.
// Written for Stage D, NOT RUN during A-B-C.
public sealed class UaeGrade5Unit11Lesson01TallyPracticeTests
{
    [Theory]
    [InlineData("uae.g5.u11.l01.tally_category_frequency")]
    [InlineData("uae.g5.u11.l01.tally_total_frequency")]
    [InlineData("uae.g5.u11.l01.tally_frequency_range")]
    public void TallyFamiliesGenerateSourceAlignedExactQuestions(string family)
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var interaction));
        Assert.NotNull(interaction);
        var engine=new ExactSkillContractQuestionEngine();
        var items=engine.Generate("PED:UAE-MOE-MATH:L5:GENERAL:11:01",
            family,[family],ExactSkillQuestionDifficulty.Standard,8,20261113,[]);
        foreach(var item in items)
        {
            var p=item.Parameters;
            int a=p["a"],b=p["b"],c=p["c"],d=p["d"];
            int oracle=family.EndsWith("tally_category_frequency",StringComparison.Ordinal)
                ?c:family.EndsWith("tally_total_frequency",StringComparison.Ordinal)
                ?a+b+c+d:Math.Max(Math.Max(a,b),Math.Max(c,d))-Math.Min(Math.Min(a,b),Math.Min(c,d));
            Assert.Equal(oracle.ToString(CultureInfo.InvariantCulture),item.CorrectAnswer);
            Assert.DoesNotContain("result",p.Keys);
            Assert.Contains("مسح",item.Prompt);
            Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,item.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"999"));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"not-a-number"));
        }
    }
}

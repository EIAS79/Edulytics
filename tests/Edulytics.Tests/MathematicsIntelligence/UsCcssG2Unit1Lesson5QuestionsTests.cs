using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Stage C authored only; executable Stage D evidence pending.</summary>
public sealed class UsCcssG2Unit1Lesson5QuestionsTests
{
    private static readonly string[] Families =
    [
        "usccss.g2.u01.l05.seven_cards_two_numbers_min_gap",
        "usccss.g2.u01.l05.add_within50_place_value"
    ];
    [Fact]
    public void OriginalSevenCardLessonUsesTwoSourceSpecificDigitalFamilies()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G2:U01:L05", out var contract));
        Assert.NotNull(contract);
        Assert.Equal(Families, contract!.AllowedQuestionFamilies);
        foreach (var family in Families)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family, out var ui));
            Assert.NotNull(ui);
            Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad, ui!.InteractionKind);
        }
    }

    [Fact]
    public void RepeatedDigitsArePermittedButLeadingZeroOrBadCardValuesAreNot()
    {
        // Source's seven physical cards include a duplicate six.
        var example = new Dictionary<string, int>(StringComparer.Ordinal)
        {
            ["card0"] = 3, ["card1"] = 8, ["card2"] = 4, ["card3"] = 6,
            ["card4"] = 6, ["card5"] = 2, ["card6"] = 1
        };
        // An independently enumerated second-best gap is not accepted.
        var all = Enumerable.Range(0, 7).Select(i => example[$"card{i}"]).ToArray();
        var best = int.MaxValue;
        for (var a = 0; a < 7; a++)
        for (var b = 0; b < 7; b++)
        for (var c = 0; c < 7; c++)
        for (var d = 0; d < 7; d++)
        {
            if (new HashSet<int>{a,b,c,d}.Count != 4 ||
                all[a] == 0 || all[c] == 0) continue;
            best = Math.Min(best,
                Math.Abs(10*all[a] + all[b] + 10*all[c] + all[d] - 50));
        }
        Assert.True(ExactSkillContractQuestionEngine.Verify(
            Families[0], example, best.ToString()));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            Families[0], example, (best+1).ToString()));

        example["card6"] = 10;
        Assert.False(ExactSkillContractQuestionEngine.Verify(Families[0], example, "0"));
    }

    [Fact]
    public void ExactGenerationsHaveCorrectWrongAndTamperedControls()
    {
        var engine = new ExactSkillContractQuestionEngine();
        foreach (var family in Families)
        foreach (var seed in new[] {20261021,20261022})
        {
            var items = engine.Generate("im2021-g2-u01-l05",family,[family],
                ExactSkillQuestionDifficulty.Standard,6,seed,[]);
            Assert.Equal(6,items.Count);
            Assert.Equal(6,items.Select(x=>x.ExposureFingerprint)
                .Distinct(StringComparer.Ordinal).Count());
            foreach (var item in items)
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(
                    family,item.Parameters,item.CorrectAnswer));
                var answer=int.Parse(item.CorrectAnswer,
                    System.Globalization.CultureInfo.InvariantCulture);
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family,item.Parameters,(answer+100).ToString()));
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family,item.Parameters,"invalid"));
                var tampered=new Dictionary<string,int>(item.Parameters,
                    StringComparer.Ordinal);
                tampered[family==Families[0]?"card0":"left"]=-3;
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family,tampered,item.CorrectAnswer));
                if (family==Families[1]) Assert.InRange(answer,11,50);
            }
        }
    }
}

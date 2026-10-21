using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Stage C tests written for Stage D execution only.</summary>
public sealed class UsCcssG2Unit1Lesson4QuestionsTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G2:U01:L04";
    private static readonly string[] Families =
    [
        "usccss.g2.u01.l04.five_card_min_distance",
        "usccss.g2.u01.l04.within20_add_or_sub"
    ];

    [Fact]
    public void Lesson4HasOnlyItsOwnTwoDigitGradeableSkills()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson, out var contract));
        Assert.NotNull(contract);
        Assert.Equal(Families, contract!.AllowedQuestionFamilies);
        foreach (var family in Families)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family, out var ui));
            Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad, ui!.InteractionKind);
        }
    }

    [Fact]
    public void SourceFiveCardsHaveAnOptimalDistanceOne()
    {
        var sourceExample = new Dictionary<string, int>(StringComparer.Ordinal)
        {
            ["card0"] = 5, ["card1"] = 8, ["card2"] = 2,
            ["card3"] = 4, ["card4"] = 6
        };
        Assert.True(ExactSkillContractQuestionEngine.Verify(Families[0], sourceExample, "1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Families[0], sourceExample, "2"));

        sourceExample["card4"] = 10;
        Assert.False(ExactSkillContractQuestionEngine.Verify(Families[0], sourceExample, "1"));
        sourceExample["card4"] = 8;
        Assert.False(ExactSkillContractQuestionEngine.Verify(Families[0], sourceExample, "1"));
    }

    [Fact]
    public void SeededFivesAndWithinTwentyRejectWrongAnswersAndTampering()
    {
        var engine = new ExactSkillContractQuestionEngine();
        foreach (var family in Families)
        foreach (var seed in new[] {20261021, 20261022})
        {
            var items = engine.Generate("im2021-g2-u01-l04", family, [family],
                ExactSkillQuestionDifficulty.Standard, 8, seed, []);
            Assert.Equal(8, items.Count);
            Assert.Equal(8, items.Select(x => x.ExposureFingerprint)
                .Distinct(StringComparer.Ordinal).Count());
            foreach (var item in items)
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(
                    family, item.Parameters, item.CorrectAnswer));
                var answer = int.Parse(item.CorrectAnswer,
                    System.Globalization.CultureInfo.InvariantCulture);
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family, item.Parameters, (answer + 100).ToString(
                        System.Globalization.CultureInfo.InvariantCulture)));
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family, item.Parameters, "invalid"));

                var bad = new Dictionary<string, int>(item.Parameters, StringComparer.Ordinal);
                bad[family == Families[0] ? "card0" : "left"] = -10;
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family, bad, item.CorrectAnswer));
                if (family == Families[0])
                {
                    var cards = Enumerable.Range(0, 5)
                        .Select(i => item.Parameters[$"card{i}"]).ToArray();
                    Assert.Equal(5, cards.Distinct().Count());
                    Assert.All(cards, card => Assert.InRange(card, 0, 9));
                }
                else
                    Assert.InRange(answer, 0, 20);
            }
        }
    }
}

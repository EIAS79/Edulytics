using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class Grade4Ordinal107MultiStepTests
{
    private const string Code = "PED:US-CCSS-MATH:G4:U06:L21";
    private const string Family = "usccss.g4.u06.l21.multistep_contexts";
    [Fact]
    public void SourceMathAndLearnerContentMatch2021Tasks()
    {
        Assert.Equal(810, 45 * 18); Assert.Equal(90, 900 - 45 * 18);
        Assert.Equal(490, 14 * 35); Assert.Equal(348, 3132 / 9);
        Assert.Equal(696, 2 * (3132 / 9));
        Assert.Equal(8352, 2 * (3132 / 9) * (7 + 5));
        Assert.Equal(24, 500 - 28 * 17);
        var rich = RichLessonContentV2Registry.Find(Code, "en");
        Assert.NotNull(rich);
        Assert.Equal(5, rich!.WorkedExamples.Count);
        Assert.Equal(3, rich.Visuals.Count);
        Assert.Contains(rich.WorkedExamples, x => x.Question.Contains("3,132"));
        Assert.Contains(rich.WorkedExamples, x => x.Question.Contains("45"));
    }
    [Fact]
    public void ExistingPracticeHostRoutesThisLessonToOnlyTheMatchedFamily()
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(Family));
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Code, out var capability));
        Assert.NotNull(capability);
        Assert.Equal(new[] { Family }, capability!.AllowedQuestionFamilies);
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(Family, out var interaction));
        Assert.NotNull(interaction);
    }
    [Fact]
    public void IndependentVerifierRejectsWrongMalformedAndTamperedAttempts()
    {
        var engine = new ExactSkillContractQuestionEngine();
        var observedModes = new HashSet<int>();
        foreach (var seed in new[] { 20211021, 20261027, 20211022, 1021, 21, 1001 })
        foreach (var item in engine.Generate("im2021-g4-u06-l21", Family, [Family],
            ExactSkillQuestionDifficulty.Standard, 10, seed, []))
        {
            var p = item.Parameters;
            var mode = p["caseType"]; observedModes.Add(mode);
            var people = p["count"]; var price = p["ticketPrice"];
            Assert.InRange(mode, 0, 2);
            Assert.Equal(people * price, p["dailyRevenue"]);
            Assert.Equal(2, p["days"]);
            var expected = mode switch
            {
                0 => p["budget"] - people * price,
                1 => (p["dailyRevenue"] / price) * 2,
                _ => (p["dailyRevenue"] / price) * 2 * (p["drinkPrice"] + p["popcornPrice"])
            };
            Assert.Equal(expected.ToString(CultureInfo.InvariantCulture), item.CorrectAnswer);
            Assert.True(ExactSkillContractQuestionEngine.Verify(Family, p, item.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(Family, p, (expected + 1).ToString()));
            Assert.False(ExactSkillContractQuestionEngine.Verify(Family, p, ""));
            Assert.False(ExactSkillContractQuestionEngine.Verify(Family, p, "wrong"));
            Assert.False(ExactSkillContractQuestionEngine.Verify(Family, p, "90 dollars"));
            var tampered = p.ToDictionary(x => x.Key, x => x.Value, StringComparer.Ordinal);
            tampered["caseType"] = 9;
            Assert.False(ExactSkillContractQuestionEngine.Verify(Family, tampered, item.CorrectAnswer));
            tampered["caseType"] = mode; tampered["ticketPrice"] = 0;
            Assert.False(ExactSkillContractQuestionEngine.Verify(Family, tampered, item.CorrectAnswer));
            tampered["ticketPrice"] = price; tampered["dailyRevenue"]++;
            Assert.False(ExactSkillContractQuestionEngine.Verify(Family, tampered, item.CorrectAnswer));
            tampered["dailyRevenue"]--; tampered["days"] = 3;
            Assert.False(ExactSkillContractQuestionEngine.Verify(Family, tampered, item.CorrectAnswer));
        }
        Assert.Equal(new[] { 0, 1, 2 }, observedModes.OrderBy(x => x));
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Edulytics.Core.Curriculum;
using Edulytics.Services.Mathematics;
using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class Grade4Ordinal109FourOperationsTests
{
    private const string Code = "PED:US-CCSS-MATH:G4:U06:L23";
    private const string Family = "usccss.g4.u06.l23.four_operation_contexts";

    [Fact]
    public void LearnerMathKeepsTheCorrect2021TravelsAndFitnessChallenges()
    {
        Assert.Equal(728, 157 + 134 + 162 + 275);
        Assert.Equal(1456, 2 * 728);
        Assert.Equal(394, 2364 / 6);
        Assert.Equal(9644, 5 * 1456 + 2364);
        Assert.Equal(916, 2 * 5280 - 9644);
        Assert.Equal(4650, 32550 / 7);
        Assert.Equal(4452, 32550 - 28098);
        Assert.Equal(23103, 120000 - (32550 + 28098 + 36249));
        Assert.Equal(5640, 4 * 2 * (230 + 170 + 150) + 2 * 2 * 310);
        var rich = RichLessonContentV2Registry.Find(Code, "en");
        Assert.NotNull(rich);
        Assert.True(rich!.WorkedExamples.Count >= 7);
        Assert.Contains(rich.WorkedExamples, x => x.Question.Contains("2,364") || x.Question.Contains("2364"));
        Assert.Contains(rich.WorkedExamples, x => x.Question.Contains("120000") || x.Question.Contains("120,000"));
        Assert.Contains(rich.WorkedExamples, x => x.Question.Contains("230") && x.Question.Contains("310"));
        Assert.Equal(3, rich.Visuals.Count);
    }

    [Fact]
    public void LessonPracticeRoutesOnlyTheSourceSpecificFamily()
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(Family));
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Code, out var capability));
        Assert.NotNull(capability);
        Assert.Equal(new[] { Family }, capability!.AllowedQuestionFamilies);
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(Family, out var interaction));
        Assert.NotNull(interaction);
    }

    [Fact]
    public void GeneratesSixMultiOperationModesAndRejectsWrongInvalidAndTamperedAnswers()
    {
        var engine = new ExactSkillContractQuestionEngine();
        var modes = new HashSet<int>();
        foreach (var seed in new[] { 20211023, 20261027, 20211024, 23, 109, 512, 781 })
        foreach (var item in engine.Generate("im2021-g4-u06-l23", Family, [Family],
            ExactSkillQuestionDifficulty.Standard, 9, seed, []))
        {
            var p = item.Parameters; var mode = p["caseType"];
            modes.Add(mode);
            var a = p["segmentA"]; var b = p["segmentB"]; var c = p["segmentC"]; var d = p["segmentD"];
            Assert.InRange(mode, 0, 5);
            Assert.Equal(5, p["longTrips"]);
            Assert.Equal(3, p["musicTrips"]);
            Assert.Equal(7, p["days"]);
            Assert.Equal(5280, p["feetPerMile"]);
            Assert.Equal(120000, p["stepTarget"]);
            Assert.Equal(0, p["week1"] % p["days"]);
            Assert.Equal(0, p["musicTotalFeet"] % (2 * p["musicTrips"]));
            var expected = mode switch
            {
                0 => 2 * (a + b + c + d),
                1 => p["musicTotalFeet"] / (2 * p["musicTrips"]),
                2 => 2 * p["feetPerMile"] - (p["longTrips"] * 2 * (a + b + c + d) + p["musicTotalFeet"]),
                3 => p["week1"] / p["days"],
                4 => p["week1"] - p["week2"],
                _ => p["stepTarget"] - p["week1"] - p["week2"] - p["week3"]
            };
            Assert.True(expected > 0);
            Assert.Equal(expected.ToString(CultureInfo.InvariantCulture), item.CorrectAnswer);
            Assert.True(ExactSkillContractQuestionEngine.Verify(Family, p, item.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(Family, p, (expected + 1).ToString(CultureInfo.InvariantCulture)));
            Assert.False(ExactSkillContractQuestionEngine.Verify(Family, p, ""));
            Assert.False(ExactSkillContractQuestionEngine.Verify(Family, p, "wrong"));
            Assert.False(ExactSkillContractQuestionEngine.Verify(Family, p, expected + " feet"));
            var changed = p.ToDictionary(x => x.Key, x => x.Value, StringComparer.Ordinal);
            changed["caseType"] = 10;
            Assert.False(ExactSkillContractQuestionEngine.Verify(Family, changed, item.CorrectAnswer));
            changed["caseType"] = mode;
            changed["longTrips"] = 4;
            Assert.False(ExactSkillContractQuestionEngine.Verify(Family, changed, item.CorrectAnswer));
            changed["longTrips"] = p["longTrips"];
            changed["feetPerMile"] = 0;
            Assert.False(ExactSkillContractQuestionEngine.Verify(Family, changed, item.CorrectAnswer));
            changed["feetPerMile"] = p["feetPerMile"];
            changed["days"] = 6;
            Assert.False(ExactSkillContractQuestionEngine.Verify(Family, changed, item.CorrectAnswer));
        }
        Assert.Equal(new[] { 0, 1, 2, 3, 4, 5 }, modes.OrderBy(x => x));
    }
}

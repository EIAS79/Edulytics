using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class Grade4Ordinal101AreaDivisionExactTests
{
    private static readonly string[] Families =
    [
        "usccss.g4.u06.l15.area_partial_quotients",
        "usccss.g4.u06.l15.mural_complete_columns"
    ];

    [Fact]
    public void GeneratedSourceAlignedQuestionsAreExactAndRejectWrongAndMalformedResponses()
    {
        var engine = new ExactSkillContractQuestionEngine();
        foreach (var family in Families)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            foreach (var seed in new[] { 20211001, 20211002, 20261027 })
            {
                var items = engine.Generate("im2021-g4-unit6-l15", family, [family],
                    ExactSkillQuestionDifficulty.Standard, 6, seed, []);
                Assert.Equal(6, items.Count);
                foreach (var item in items)
                {
                    var p = item.Parameters;
                    var width = p["knownSide"];
                    var area = p["area"];
                    var remainder = p["leftover"];
                    Assert.InRange(width, 2, 9);
                    Assert.InRange(area, 100, 999);
                    Assert.InRange(remainder, 0, width - 1);
                    if (family == Families[0]) Assert.Equal(0, remainder);
                    else Assert.True(remainder > 0);
                    var quotient = area / width;
                    Assert.Equal(quotient.ToString(CultureInfo.InvariantCulture), item.CorrectAnswer);
                    Assert.True(ExactSkillContractQuestionEngine.Verify(family, p, item.CorrectAnswer));
                    Assert.False(ExactSkillContractQuestionEngine.Verify(family, p, (quotient + 1).ToString()));
                    Assert.False(ExactSkillContractQuestionEngine.Verify(family, p, (quotient - 1).ToString()));
                    Assert.False(ExactSkillContractQuestionEngine.Verify(family, p, "not a number"));
                    Assert.False(ExactSkillContractQuestionEngine.Verify(family, p, ""));
                    var changed = p.ToDictionary(x => x.Key, x => x.Value, StringComparer.Ordinal);
                    changed["knownSide"] = 0;
                    Assert.False(ExactSkillContractQuestionEngine.Verify(family, changed, item.CorrectAnswer));
                    changed["knownSide"] = width;
                    changed["leftover"] = family == Families[0] ? 1 : 0;
                    Assert.False(ExactSkillContractQuestionEngine.Verify(family, changed, item.CorrectAnswer));
                }
            }
        }
    }

    [Fact]
    public void StageCMappingRemainsClosedUntilReviewedLearnerInteractionAndSavedAttempt()
    {
        const string lesson = "PED:US-CCSS-MATH:G4:U06:L15";
        Assert.True(LessonPracticeContractRegistry.TryResolve(lesson, out var pending));
        Assert.NotNull(pending);
        Assert.Equal("REVIEW_REQUIRED", pending!.Readiness);
        Assert.Empty(pending.AllowedQuestionFamilies);
        Assert.False(LessonPracticeCapabilityResolver.TryResolve(lesson, out var routed));
        Assert.Null(routed);
        foreach (var family in Families)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.False(LessonPracticeInteractionRegistry.TryResolve(family, out _));
        }
    }

    [Fact]
    public void SourceCasesAndContextualRemainderDoNotAllowAnExtraColumn()
    {
        Assert.Equal(27, 189 / 7);
        Assert.Equal(57, 2280 / 40);
        Assert.Equal(28, 168 / 6);
        var p = new Dictionary<string, int> { ["knownSide"] = 6, ["area"] = 197, ["leftover"] = 5 };
        Assert.True(ExactSkillContractQuestionEngine.Verify(Families[1], p, "32"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Families[1], p, "33"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Families[0], p, "32"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Families[1], p, "32.5"));
    }
}

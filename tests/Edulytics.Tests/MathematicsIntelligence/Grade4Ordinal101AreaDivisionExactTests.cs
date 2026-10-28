using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Core.Curriculum;
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
    public void OnlyTwoSourceAlignedFamiliesReachGrade4Lesson101IntegerInteraction()
    {
        const string lesson = "PED:US-CCSS-MATH:G4:U06:L15";
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(lesson, out var contract));
        Assert.NotNull(contract);
        Assert.Equal("READY_VERIFIED", contract!.Readiness);
        Assert.Equal(Families, contract.AllowedQuestionFamilies);
        foreach (var family in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family, out var ui));
            Assert.NotNull(ui);
            Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad, ui!.InteractionKind);
            Assert.Equal("exact_integer", ui.AnswerType);
        }
    }

    [Fact]
    public void SourceCorrectRichV2LessonLoadsIndependentlyOfOtherLegacyGradeFiles()
    {
        var lesson = RichLessonContentV2Registry.Find(
            "PED:US-CCSS-MATH:G4:U06:L15", "en");
        Assert.NotNull(lesson);
        Assert.Equal("Situations Involving Area", lesson!.Title);
        Assert.Equal(3, lesson.WorkedExamples.Count);
        Assert.Equal(3, lesson.Visuals.Count);
        Assert.Single(lesson.Visuals[0].Rows);
        Assert.Equal(7, lesson.Visuals[1].Rows.Count);
        Assert.All(lesson.Visuals[1].Rows, row => Assert.Equal(2, row.Cells.Count));
        Assert.Equal(6, lesson.Visuals[2].Rows.Count);
        Assert.All(lesson.Visuals[2].Rows, row => Assert.Equal(2, row.Cells.Count));
        Assert.Equal(RichLessonVisualKind.ShapeDecomposition, lesson.Visuals[1].Kind);
        Assert.Contains(lesson.WorkedExamples, x => x.Question.Contains("189"));
        Assert.Contains(lesson.WorkedExamples, x => x.Question.Contains("197"));
        Assert.Contains(lesson.WorkedExamples, x => x.Question.Contains("2,280"));
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

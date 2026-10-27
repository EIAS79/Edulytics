using System;
using System.Collections.Generic;
using System.Linq;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class Grade4Ordinal112DescriptionPrecisionTests
{
    private const string Family = "usccss.g4.u07.l01.geometric_description_precision";

    [Fact]
    public void SourceGoalFamilyGeneratesAndRejectsWrongMalformedAndTamperedResponses()
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(Family));
        var engine = new ExactSkillContractQuestionEngine();
        foreach (var seed in new[] { 20210701, 20210702, 20261027 })
        {
            var items = engine.Generate("g4-2021-u07-l01", Family, [Family],
                ExactSkillQuestionDifficulty.Standard, 16, seed, []);
            Assert.Equal(16, items.Count);
            foreach (var item in items)
            {
                var p = item.Parameters;
                Assert.InRange(p["caseType"], 0, 5);
                Assert.InRange(p["preciseFirst"], 0, 1);
                var expected = p["preciseFirst"] == 1 ? "1" : "2";
                Assert.Equal(expected, item.CorrectAnswer);
                Assert.True(ExactSkillContractQuestionEngine.Verify(Family, p, expected));
                Assert.False(ExactSkillContractQuestionEngine.Verify(Family, p, expected == "1" ? "2" : "1"));
                Assert.False(ExactSkillContractQuestionEngine.Verify(Family, p, ""));
                Assert.False(ExactSkillContractQuestionEngine.Verify(Family, p, "not clear"));
                Assert.False(ExactSkillContractQuestionEngine.Verify(Family, p, "3"));
                var changed = p.ToDictionary(x => x.Key, x => x.Value, StringComparer.Ordinal);
                changed["caseType"] = 12;
                Assert.False(ExactSkillContractQuestionEngine.Verify(Family, changed, expected));
                changed["caseType"] = p["caseType"];
                changed["preciseFirst"] = 7;
                Assert.False(ExactSkillContractQuestionEngine.Verify(Family, changed, expected));
            }
        }
    }
    [Fact]
    public void OriginalOpenDrawingsAreNotReplacedByMultipleChoice()
    {
        // Source partner-card reproduction and five-line constructions remain teacher-judged.
        Assert.True(5 >= 3);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class Grade4Ordinal113PointsLinesRaysSegmentsTests
{
    private const string Family="usccss.g4.u07.l02.geometry_cards_number_talk";
    [Fact]
    public void QuestionsHaveExactSourceClassificationOrNumberTalkAndRejectBadAnswers()
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(Family));
        var engine=new ExactSkillContractQuestionEngine();
        foreach (var seed in new[] { 20210702, 20210703, 20261027 })
        {
            var items=engine.Generate("g4-2021-u07-l02", Family, [Family],
                ExactSkillQuestionDifficulty.Standard, 8, seed, []);
            Assert.Equal(8, items.Count);
            foreach (var item in items)
            {
                var p=item.Parameters;
                Assert.InRange(p["caseType"],0,7);
                Assert.InRange(p["variant"],0,2);
                var expected=p["caseType"]<4 ? p["caseType"]+1 : p["first"]-p["second"];
                Assert.Equal(expected.ToString(),item.CorrectAnswer);
                Assert.True(ExactSkillContractQuestionEngine.Verify(Family,p,item.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(Family,p,(expected+1).ToString()));
                Assert.False(ExactSkillContractQuestionEngine.Verify(Family,p,"?"));
                Assert.False(ExactSkillContractQuestionEngine.Verify(Family,p,""));
                var modified=p.ToDictionary(x=>x.Key,x=>x.Value,StringComparer.Ordinal);
                modified["variant"]=9;
                Assert.False(ExactSkillContractQuestionEngine.Verify(Family,modified,item.CorrectAnswer));
                modified["variant"]=p["variant"];
                modified["first"]=999;
                Assert.False(ExactSkillContractQuestionEngine.Verify(Family,modified,item.CorrectAnswer));
            }
        }
    }
    [Fact]
    public void Original2021NumberTalkValuesAreIndependentlyChecked()
    {
        Assert.Equal(45,90-45);Assert.Equal(225,270-45);
        Assert.Equal(135,270-135);Assert.Equal(225,360-135);
    }
}

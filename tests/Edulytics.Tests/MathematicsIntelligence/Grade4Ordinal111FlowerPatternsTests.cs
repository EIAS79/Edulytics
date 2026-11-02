using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class Grade4Ordinal111FlowerPatternsTests
{
    private const string Code = "PED:US-CCSS-MATH:G4:U06:L25";
    private const string Family = "usccss.g4.u06.l25.flower_garland_patterns";
    [Fact]
    public void SourcePatternAndOptionalPhysicalTasksHaveAccurateLearnerMath()
    {
        Assert.Equal(6, 12 / 4 * 2);
        Assert.Equal(4, 12 / 3);
        Assert.Equal(8, 12 / 3 * 2);
        Assert.Equal(250, 25 * (6 + 4));
        Assert.Equal(350, 25 * (6 + 8));
        Assert.Equal(9, 155 / 16);
        Assert.Equal(11, 155 % 16);
        Assert.Equal(465, 155 * (1 + 2));
        Assert.Equal(234, ((155 + 1) / 2) * 3);
        Assert.Equal(120, 14 * 6 + 9 * 4);
        var rich = RichLessonContentV2Registry.Find(Code,"en");
        Assert.NotNull(rich);
        Assert.True(rich!.WorkedExamples.Count >= 7);
        Assert.Equal(3, rich.Visuals.Count);
        Assert.Contains(rich.WorkedExamples,e=>e.Question.Contains("155"));
        Assert.Contains(rich.WorkedExamples,e=>e.Question.Contains("25"));
        Assert.Contains(rich.WorkedExamples,e=>e.Question.Contains("14") && e.Question.Contains("9"));
    }
    [Fact]
    public void OnlyTheGarlandPracticeFamilyIsRouted()
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(Family));
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Code,out var capability));
        Assert.NotNull(capability);
        Assert.Equal(new[]{Family},capability!.AllowedQuestionFamilies);
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(Family,out var interaction));
        Assert.NotNull(interaction);
    }
    [Fact]
    public void SixPracticeModesAreIndependentlyCheckedAndRejectBadAnswers()
    {
        var engine = new ExactSkillContractQuestionEngine();
        var modes = new HashSet<int>();
        foreach(var seed in new[]{20211025,20261027,110,111,25,774,109})
        foreach(var item in engine.Generate("im2021-g4-u06-l25",Family,[Family],
                 ExactSkillQuestionDifficulty.Standard,10,seed,[]))
        {
            var p=item.Parameters;var mode=p["caseType"];modes.Add(mode);
            Assert.InRange(mode,0,5);
            Assert.Equal(16,p["flowersPerGarland"]);
            Assert.Equal(6,p["priyaBig"]);
            Assert.Equal(6,p["priyaSmall"]);
            Assert.Equal(4,p["jadaBig"]);
            Assert.Equal(8,p["jadaSmall"]);
            Assert.Equal(2,p["workers"]);
            var expected=mode switch
            {
                0 => p["garlandsEach"]*(p["priyaBig"]+p["jadaBig"]),
                1 => p["garlandsEach"]*(p["priyaSmall"]+p["jadaSmall"]),
                2 => p["flowersMade"]/p["flowersPerGarland"],
                3 => p["flowersMade"]%p["flowersPerGarland"],
                4 => p["flowersMade"]*(p["cutMinutes"]+p["finishMinutes"]),
                _ => ((p["flowersMade"]+p["workers"]-1)/p["workers"])*(p["cutMinutes"]+p["finishMinutes"])
            };
            Assert.InRange(expected,0,2000);
            Assert.Equal(expected.ToString(CultureInfo.InvariantCulture),item.CorrectAnswer);
            Assert.True(ExactSkillContractQuestionEngine.Verify(Family,p,item.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(Family,p,(expected+1).ToString(CultureInfo.InvariantCulture)));
            Assert.False(ExactSkillContractQuestionEngine.Verify(Family,p,""));
            Assert.False(ExactSkillContractQuestionEngine.Verify(Family,p,"wrong"));
            Assert.False(ExactSkillContractQuestionEngine.Verify(Family,p,expected+" flowers"));
            var changed=p.ToDictionary(x=>x.Key,x=>x.Value,StringComparer.Ordinal);
            changed["caseType"]=7;
            Assert.False(ExactSkillContractQuestionEngine.Verify(Family,changed,item.CorrectAnswer));
            changed["caseType"]=mode;changed["workers"]=0;
            Assert.False(ExactSkillContractQuestionEngine.Verify(Family,changed,item.CorrectAnswer));
            changed["workers"]=p["workers"];changed["priyaBig"]=5;
            Assert.False(ExactSkillContractQuestionEngine.Verify(Family,changed,item.CorrectAnswer));
        }
        Assert.Equal(new[]{0,1,2,3,4,5},modes.OrderBy(x=>x));
    }
}

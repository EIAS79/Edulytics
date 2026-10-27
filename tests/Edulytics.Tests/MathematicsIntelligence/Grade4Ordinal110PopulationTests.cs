using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class Grade4Ordinal110PopulationTests
{
    private const string Code = "PED:US-CCSS-MATH:G4:U06:L24";
    private const string Family = "usccss.g4.u06.l24.language_population_comparisons";
    [Fact]
    public void Source2021OriginalHistoricalValuesAndFirstPartyCoolDownAreCorrect()
    {
        Assert.Equal(147076, 166826 - 19750);
        Assert.Equal(8, (2 * 166826 + 19750) / (2 * 19750));
        Assert.Equal(191736, 166826 + 13445 + 11465);
        Assert.Equal(111680, 19750 + 17855 + 13445 + 13190 + 11465 + 9735 + 9635 + 9615 + 6990);
        Assert.Equal(55146, 166826 - 111680);
        Assert.Equal(134466, 127352 + 6750 + 364);
        Assert.Equal(1090073, 1224539 - 134466);
        Assert.Equal(314243, (422568 + 25777) - (127352 + 6750));
        Assert.Equal(129840, 178420 - (35680 + 12900));
        var rich = RichLessonContentV2Registry.Find(Code,"en");
        Assert.NotNull(rich);
        Assert.True(rich!.WorkedExamples.Count >= 7);
        Assert.Equal(3, rich.Visuals.Count);
        Assert.Contains(rich.WorkedExamples,x=>x.Question.Contains("166,826"));
        Assert.Contains(rich.WorkedExamples,x=>x.Question.Contains("422,568"));
        Assert.Contains(rich.WorkedExamples,x=>x.Question.Contains("178,420"));
    }
    [Fact]
    public void LessonHasOnlyItsExactSourceFamily()
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(Family));
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Code,out var cap));
        Assert.NotNull(cap);
        Assert.Equal(new[]{Family},cap!.AllowedQuestionFamilies);
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(Family,out var interaction));
        Assert.NotNull(interaction);
    }
    [Fact]
    public void SixModesVerifyCorrectAndRejectWrongInvalidAndTampered()
    {
        var engine = new ExactSkillContractQuestionEngine();
        var modes = new HashSet<int>();
        foreach(var seed in new[]{20211024,20261027,21024,110,240,77,333})
        foreach(var item in engine.Generate("im2021-g4-u06-l24",Family,[Family],
                ExactSkillQuestionDifficulty.Standard,9,seed,[]))
        {
            var p=item.Parameters;var mode=p["caseType"];modes.Add(mode);
            Assert.InRange(mode,0,5);
            var other=p["yupik"]+p["dakota"]+p["apache"]+p["keres"]+
                      p["cherokee"]+p["ojibwa"]+p["choctaw"]+p["zuni"]+p["pima"];
            Assert.True(p["navajo"] > other);
            var expected=mode switch
            {
                0 => p["navajo"]-p["yupik"],
                1 => p["navajo"]+p["apache"]+p["cherokee"],
                2 => p["navajo"]-other,
                3 => p["philadelphiaEnglish"]-p["philadelphiaSpanish"]-p["philadelphiaIndo"]-p["philadelphiaAsian"],
                4 => p["chicagoSpanish"]+p["chicagoIndo"]-p["philadelphiaSpanish"]-p["philadelphiaIndo"],
                _ => (2*p["navajo"]+p["yupik"])/(2*p["yupik"])
            };
            Assert.True(expected > 0);
            Assert.Equal(expected.ToString(CultureInfo.InvariantCulture),item.CorrectAnswer);
            Assert.True(ExactSkillContractQuestionEngine.Verify(Family,p,item.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(Family,p,(expected+1).ToString(CultureInfo.InvariantCulture)));
            Assert.False(ExactSkillContractQuestionEngine.Verify(Family,p,""));
            Assert.False(ExactSkillContractQuestionEngine.Verify(Family,p,"wrong"));
            Assert.False(ExactSkillContractQuestionEngine.Verify(Family,p,expected+" speakers"));
            var changed=p.ToDictionary(x=>x.Key,x=>x.Value,StringComparer.Ordinal);
            changed["caseType"]=9;
            Assert.False(ExactSkillContractQuestionEngine.Verify(Family,changed,item.CorrectAnswer));
            changed["caseType"]=mode;changed["yupik"]=0;
            Assert.False(ExactSkillContractQuestionEngine.Verify(Family,changed,item.CorrectAnswer));
            changed["yupik"]=p["yupik"];changed["navajo"]=0;
            Assert.False(ExactSkillContractQuestionEngine.Verify(Family,changed,item.CorrectAnswer));
        }
        Assert.Equal(new[]{0,1,2,3,4,5},modes.OrderBy(x=>x));
    }
}

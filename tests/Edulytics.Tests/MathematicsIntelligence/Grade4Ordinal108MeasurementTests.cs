using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class Grade4Ordinal108MeasurementTests
{
    private const string Code = "PED:US-CCSS-MATH:G4:U06:L22";
    private const string Family = "usccss.g4.u06.l22.convert_l_shaped_area_perimeter";
    [Fact] public void SourceUnitsAndRichMathHaveExactChecks()
    {
        Assert.Equal(864,36*24); Assert.Equal(768,8*12*8);
        Assert.Equal(96,864-768); Assert.Equal(64,17+12+9+3+8+15);
        Assert.Equal(768,64*12); Assert.Equal(432,1200-768);
        Assert.Equal(228,17*12+8*3); Assert.Equal(228,8*15+9*12);
        var rich=RichLessonContentV2Registry.Find(Code,"en");
        Assert.NotNull(rich); Assert.True(rich!.WorkedExamples.Count>=6);
        Assert.Equal(3,rich.Visuals.Count);
        Assert.Contains(rich.WorkedExamples,x=>x.Question.Contains("204in"));
    }
    [Fact] public void LessonPracticeHasOnlyAnExactSourceFamily()
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(Family));
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Code,out var capability));
        Assert.NotNull(capability);
        Assert.Equal(new[]{Family},capability!.AllowedQuestionFamilies);
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(Family,out var ui));
        Assert.NotNull(ui);
    }
    [Fact] public void GeneratorAndIndependentVerifierRejectBadAndTamperedInputs()
    {
        var engine=new ExactSkillContractQuestionEngine();
        var seen=new HashSet<int>();
        foreach(var seed in new[]{20211022,20261027,222,108,2021,1234})
        foreach(var item in engine.Generate("im2021-g4-u06-l22",Family,[Family],ExactSkillQuestionDifficulty.Standard,10,seed,[]))
        {
            var p=item.Parameters;var mode=p["caseType"];seen.Add(mode);
            var left=p["leftWidthFeet"];var right=3*p["rightWidthYards"];
            var low=3*p["lowerHeightYards"];var rise=p["upperRiseFeet"];
            var perimeter=2*(left+right+low+rise);
            var expected=mode switch
            {
                0=>p["sheetWidthInches"]*p["sheetHeightInches"]-p["bannerLengthFeet"]*12*p["bannerHeightInches"],
                1=>perimeter,
                2=>12*perimeter,
                3=>(left+right)*low+left*rise,
                _=>p["suppliedInches"]-12*perimeter
            };
            Assert.InRange(mode,0,4);
            Assert.Equal(expected.ToString(CultureInfo.InvariantCulture),item.CorrectAnswer);
            Assert.True(ExactSkillContractQuestionEngine.Verify(Family,p,item.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(Family,p,(expected+1).ToString()));
            Assert.False(ExactSkillContractQuestionEngine.Verify(Family,p,""));
            Assert.False(ExactSkillContractQuestionEngine.Verify(Family,p,"not a number"));
            Assert.False(ExactSkillContractQuestionEngine.Verify(Family,p,"64 feet"));
            var bad=p.ToDictionary(x=>x.Key,x=>x.Value,StringComparer.Ordinal);
            bad["caseType"]=9;Assert.False(ExactSkillContractQuestionEngine.Verify(Family,bad,item.CorrectAnswer));
            bad["caseType"]=mode;bad["upperRiseFeet"]=0;
            Assert.False(ExactSkillContractQuestionEngine.Verify(Family,bad,item.CorrectAnswer));
            bad["upperRiseFeet"]=rise;bad["sheetWidthInches"]=0;
            Assert.False(ExactSkillContractQuestionEngine.Verify(Family,bad,item.CorrectAnswer));
        }
        Assert.Equal(new[]{0,1,2,3,4},seen.OrderBy(x=>x));
    }
}

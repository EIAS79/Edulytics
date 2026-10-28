using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class Grade4Ordinal106ContextualRemainderTests
{
    private const string Code = "PED:US-CCSS-MATH:G4:U06:L20";
    private const string Family = "usccss.g4.u06.l20.contextual_remainder_decisions";
    [Fact] public void Original2021ContextRequiresDifferentRemainderInterpretations()
    {
        Assert.Equal(94,378/4);Assert.Equal(2,378%4);Assert.Equal(95,378/4+1);
        Assert.Equal(28,258/9);Assert.Equal(6,258%9);
        Assert.Equal(158,(1270-6)/8);
        Assert.Equal(237,711/3);Assert.Equal(391,3128/8);
        Assert.Equal(35,275/8+1);
        var rich=RichLessonContentV2Registry.Find(Code,"en");
        Assert.NotNull(rich);Assert.True(rich!.WorkedExamples.Count>=5);
        Assert.Equal(3,rich.Visuals.Count);
        Assert.Contains(rich.WorkedExamples,x=>x.Question.Contains("378"));
        Assert.Contains(rich.WorkedExamples,x=>x.Question.Contains("258"));
        Assert.Contains(rich.WorkedExamples,x=>x.Question.Contains("1270"));
    }
    [Fact] public void ScopedPracticeIsRoutedAndUsesNumericEntry()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Code,out var capability));
        Assert.NotNull(capability);
        Assert.Equal(new[]{Family},capability!.AllowedQuestionFamilies);
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(Family,out var ui));
        Assert.NotNull(ui);
    }
    [Fact] public void GeneratedPracticeHandlesFiveContextsAndRejectsWrongMalformedTampering()
    {
        var engine=new ExactSkillContractQuestionEngine();
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(Family));
        var seen=new HashSet<int>();
        foreach(var seed in new[]{20211020,20261027,20211021,20241020,120})
        foreach(var item in engine.Generate("im2021-g4-u06-l20",Family,[Family],ExactSkillQuestionDifficulty.Standard,12,seed,[]))
        {
            var p=item.Parameters;var mode=p["caseType"];seen.Add(mode);
            var n=p["dividend"];var d=p["divisor"];var shortage=p["shortfall"];
            Assert.InRange(d,2,9);Assert.InRange(n,100,9999);
            var q=n/d;var r=n%d;
            var expected=mode switch {0=>q+1,1=>q,2=>r,3=>(n-shortage)/d,_=>q};
            Assert.Equal(expected.ToString(CultureInfo.InvariantCulture),item.CorrectAnswer);
            Assert.True(ExactSkillContractQuestionEngine.Verify(Family,p,item.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(Family,p,(expected+1).ToString(CultureInfo.InvariantCulture)));
            Assert.False(ExactSkillContractQuestionEngine.Verify(Family,p,""));
            Assert.False(ExactSkillContractQuestionEngine.Verify(Family,p,"wrong"));
            Assert.False(ExactSkillContractQuestionEngine.Verify(Family,p,"2.5"));
            var bad=p.ToDictionary(z=>z.Key,z=>z.Value,StringComparer.Ordinal);
            bad["divisor"]=0;
            Assert.False(ExactSkillContractQuestionEngine.Verify(Family,bad,item.CorrectAnswer));
            bad["divisor"]=d;bad["shortfall"]=d;
            Assert.False(ExactSkillContractQuestionEngine.Verify(Family,bad,item.CorrectAnswer));
            bad["shortfall"]=shortage;bad["caseType"]=5;
            Assert.False(ExactSkillContractQuestionEngine.Verify(Family,bad,item.CorrectAnswer));
        }
        Assert.Equal(new[]{0,1,2,3,4},seen.OrderBy(x=>x));
    }
}

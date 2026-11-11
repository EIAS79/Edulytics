using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

// UAE Grade 4 student source 04-01.pdf pp17-22, first-party source equivalents.
public sealed class UaeGrade4Unit1Lesson02ReadWriteTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:01:02";

    [Fact]
    public void ThreeSelectedExactFamiliesAreAvailableToLesson()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal(3,contract!.AllowedQuestionFamilies.Count);
        Assert.Contains("uae.g4.u01.l02.words_to_standard",contract.AllowedQuestionFamilies);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var family in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
            Assert.NotNull(ui);
            foreach(var q in engine.Generate(Lesson,family,[family],
                ExactSkillQuestionDifficulty.Standard,3,20261111,[]))
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"-100"));
                Assert.False(q.Parameters.ContainsKey("result"));
                Assert.False(q.Parameters.ContainsKey("number"));
            }
        }
    }

    [Fact]
    public void ReadingWordsWithPlaceholdingZerosHasCorrectNumericResult()
    {
        const string f="uae.g4.u01.l02.words_to_standard";
        var p=new Dictionary<string,int>{
            ["pos"]=-1,["d0"]=5,["d1"]=0,["d2"]=0,["d3"]=7,
            ["d4"]=0,["d5"]=0,["d6"]=9
        };
        Assert.True(ExactSkillContractQuestionEngine.Verify(f,p,"٥٬٠٠٧٬٠٠٩"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(f,p,"٥٬٧٠٠٬٩٠٠"));
        p["d0"]=0;
        Assert.False(ExactSkillContractQuestionEngine.Verify(f,p,"7009"));
        p["d0"]=5;p["d3"]=10;
        Assert.False(ExactSkillContractQuestionEngine.Verify(f,p,"5007019"));
    }

    [Fact]
    public void ArabicRichV2RetainsFormsAndPlaceValueVisual()
    {
        var lesson=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(lesson);
        Assert.Equal("قراءة وكتابة أعداد متعددة الأرقام",lesson!.Title);
        Assert.Equal(3,lesson.WorkedExamples.Count);
        Assert.Equal(3,lesson.KeyConcepts.Count);
        Assert.Equal(RichLessonVisualKind.PlaceValueChart,Assert.Single(lesson.Visuals).Kind);
        Assert.Contains(lesson.ExplanationParagraphs,x=>x.Contains("التحليلية"));
    }
}

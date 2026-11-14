using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG6U5L01IntegerExplorationTests
{
    const string Lesson="PED:UAE-MOE-MATH:L6:GENERAL:05:01:STUDENT-AR-128";
    const string Position="uae.g6.u05.l01.signed_position";
    const string Opposite="uae.g6.u05.l01.opposite_integer";
    [Fact] public void OriginalArabicLabHasNumericFamiliesAndTeacherReview()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Equal(10,l!.WorkedExamples.Count);
        Assert.Contains(l.Visuals,v=>v.Kind==RichLessonVisualKind.RatioTable);
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal(2,contract!.AllowedQuestionFamilies.Count);
        foreach(var family in contract.AllowedQuestionFamilies)
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
    }
    [Fact] public void PositionUsesSignOfHeightAndAcceptsArabicDigits()
    {
        var under=new Dictionary<string,int>{{"magnitude",5},{"sign",-1}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(Position,under,"-5"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Position,under,"−٥"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Position,under,"5"));
        var above=new Dictionary<string,int>{{"magnitude",25},{"sign",1}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(Position,above,"+25"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Position,above,"٢٥"));
    }
    [Fact] public void OppositeAndZeroAreCorrectAndInvalidParametersRejected()
    {
        var negative=new Dictionary<string,int>{{"magnitude",6},{"sign",-1}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(Opposite,negative,"6"));
        var zero=new Dictionary<string,int>{{"magnitude",0},{"sign",-1}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(Opposite,zero,"٠"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Position,zero,"0"));
        var bad=new Dictionary<string,int>{{"magnitude",-1},{"sign",1}};
        Assert.False(ExactSkillContractQuestionEngine.Verify(Opposite,bad,"1"));
    }
}

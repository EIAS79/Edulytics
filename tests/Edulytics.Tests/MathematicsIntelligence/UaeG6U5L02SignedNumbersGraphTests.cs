using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG6U5L02SignedNumbersGraphTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L6:GENERAL:05:02:STUDENT-AR-128";
    private const string Family="uae.g6.u05.l02.signed_plot_set_extremum";
    [Fact] public void OriginalArabicIntegerGraphLessonReusesExactSignedEngine()
    {
        var content=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(content);
        Assert.Equal("الأعداد الصحيحة وتمثيلها بيانيًا",content!.Title);
        Assert.Equal(10,content.WorkedExamples.Count);
        Assert.Contains(content.Visuals,v=>v.Kind==RichLessonVisualKind.RatioTable);
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var c));
        Assert.Equal(3,c!.AllowedQuestionFamilies.Count);
        foreach(var family in c.AllowedQuestionFamilies)
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
    }
    [Fact] public void NumericGroupExtremaSupportNegativeZeroAndPositiveValues()
    {
        var p=new Dictionary<string,int>{{"a",-4},{"b",0},{"c",3},{"d",-1},{"mode",0}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(Family,p,"-4"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Family,p,"−٤"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,p,"3"));
        p["mode"]=1;
        Assert.True(ExactSkillContractQuestionEngine.Verify(Family,p,"+3"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,p,"0"));
        var invalid=new Dictionary<string,int>{{"a",-50},{"b",0},{"c",3},{"d",-1},{"mode",2}};
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,invalid,"-50"));
    }
    [Fact] public void EarlierSignedNumberPositionAndOppositeFamiliesAreUnchanged()
    {
        var p=new Dictionary<string,int>{{"magnitude",7},{"sign",-1}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u05.l01.signed_position",p,"-7"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u05.l01.opposite_integer",p,"7"));
        var z=new Dictionary<string,int>{{"magnitude",0},{"sign",1}};
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g6.u05.l01.signed_position",z,"0"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u05.l01.opposite_integer",z,"0"));
    }
}

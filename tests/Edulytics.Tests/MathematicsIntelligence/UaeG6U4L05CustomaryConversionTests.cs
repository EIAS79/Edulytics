using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG6U4L05CustomaryConversionTests
{
    const string L="PED:UAE-MOE-MATH:L6:GENERAL:04:05:STUDENT-AR-128", Smaller="uae.g6.u04.l05.to_smaller_exact", Larger="uae.g6.u04.l05.to_larger_exact";
    [Fact] public void ArabicContentAndConversionFamiliesAreAvailable()
    {
        var content=RichLessonContentV2Registry.Find(L,"ar");
        Assert.NotNull(content);
        Assert.Equal(10,content!.WorkedExamples.Count);
        Assert.Contains(content.Visuals,v=>v.Kind==RichLessonVisualKind.RatioTable);
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(L,out var c));
        Assert.Equal(2,c!.AllowedQuestionFamilies.Count);
        foreach(var family in c.AllowedQuestionFamilies)
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
    }
    [Fact] public void ExactForwardReverseAndArabicFractionConversion()
    {
        var y=new Dictionary<string,int>{{"pair",0},{"n",12},{"d",1}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(Smaller,y,"36"));
        var ft=new Dictionary<string,int>{{"pair",0},{"n",18},{"d",1}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(Larger,ft,"6"));
        var mixed=new Dictionary<string,int>{{"pair",0},{"n",3},{"d",2}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(Smaller,mixed,"٩/٢"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Smaller,mixed,"9/3"));
        var qt=new Dictionary<string,int>{{"pair",5},{"n",15},{"d",1}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(Larger,qt,"15/4"));
    }
    [Fact] public void MissingAndInvalidRatioParametersFailClosed()
    {
        var bad=new Dictionary<string,int>{{"pair",99},{"n",2},{"d",1}};
        Assert.False(ExactSkillContractQuestionEngine.Verify(Smaller,bad,"2"));
        var zero=new Dictionary<string,int>{{"pair",0},{"n",2},{"d",0}};
        Assert.False(ExactSkillContractQuestionEngine.Verify(Smaller,zero,"6"));
        var feet=new Dictionary<string,int>{{"pair",1},{"n",20},{"d",1}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(Smaller,feet,"240"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Smaller,feet,"20"));
    }
}

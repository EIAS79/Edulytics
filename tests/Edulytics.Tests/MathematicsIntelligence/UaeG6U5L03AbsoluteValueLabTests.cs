using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG6U5L03AbsoluteValueLabTests
{
    const string Lesson="PED:UAE-MOE-MATH:L6:GENERAL:05:03:STUDENT-AR-128",Family="uae.g6.u05.l03.absolute_distance";
    [Fact] public void ArabicLabReusesTheSignedNumberEngine()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Equal(10,l!.WorkedExamples.Count);
        Assert.Contains(l.Visuals,v=>v.Kind==RichLessonVisualKind.RatioTable);
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var c));
        Assert.Equal(3,c!.AllowedQuestionFamilies.Count);
        foreach(var family in c.AllowedQuestionFamilies)
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
    }
    [Theory]
    [InlineData(-13,"13")]
    [InlineData(13,"١٣")]
    [InlineData(0,"0")]
    [InlineData(-7,"٧")]
    public void AbsoluteDistanceWorksForSignsAndZero(int n,string answer)
    {
        var p=new Dictionary<string,int>{{"n",n}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(Family,p,answer));
    }
    [Fact] public void NegativeDistancesAndInvalidParametersAreRejectedAndPriorFamiliesPreserved()
    {
        var p=new Dictionary<string,int>{{"n",-9}};
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,p,"-9"));
        var bad=new Dictionary<string,int>{{"n",-41}};
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,bad,"41"));
        var earlier=new Dictionary<string,int>{{"magnitude",8},{"sign",-1}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u05.l01.opposite_integer",earlier,"8"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u05.l01.signed_position",earlier,"-8"));
    }
}

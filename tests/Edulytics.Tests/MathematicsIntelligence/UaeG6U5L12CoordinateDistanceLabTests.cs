using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG6U5L12CoordinateDistanceLabTests
{
    const string L="PED:UAE-MOE-MATH:L6:GENERAL:05:12:STUDENT-AR-128",
        H="uae.g6.u05.l12.distance_same_y",
        V="uae.g6.u05.l12.distance_same_x",
        IH="uae.g6.u05.l12.find_horizontal_endpoint",
        IV="uae.g6.u05.l12.find_vertical_endpoint";
    [Fact] public void ArabicLabHasSignedPlotsExamplesAndFourExactContracts()
    {
        var lesson=RichLessonContentV2Registry.Find(L,"ar");
        Assert.NotNull(lesson);
        Assert.Equal(10,lesson!.WorkedExamples.Count);
        Assert.All(lesson.WorkedExamples,e=>Assert.True(e.Steps.Count>=3));
        Assert.Equal(3,lesson.Visuals.Count(v=>v.Kind==RichLessonVisualKind.SignedCartesianPlot));
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(L,out var c));
        Assert.Equal(4,c!.AllowedQuestionFamilies.Count);
        foreach(var family in c.AllowedQuestionFamilies)
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
    }
    [Theory]
    [InlineData(-12,16,8,"7")]
    [InlineData(4,16,12,"3")]
    [InlineData(-14,9,0,"5.75")]
    [InlineData(8,8,-8,"0")]
    public void HorizontalAbsoluteDifferenceIsExact(int x1,int x2,int y,string expected)
    {
        var p=new Dictionary<string,int>{{"a4",x1},{"b4",x2},{"fixed4",y}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(H,p,expected));
    }
    [Theory]
    [InlineData(12,-16,-8,"٧")]
    [InlineData(-4,-16,12,"3")]
    [InlineData(-5,15,10,"5")]
    public void VerticalAbsoluteDifferenceIsExact(int y1,int y2,int x,string expected)
    {
        var p=new Dictionary<string,int>{{"a4",y1},{"b4",y2},{"fixed4",x}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(V,p,expected));
    }
    [Fact] public void EndpointsUseDirectionWithoutRounding()
    {
        var p=new Dictionary<string,int>{{"start4",-8},{"distance4",14},{"direction",1},{"fixed4",12}};
        Assert.False(ExactSkillContractQuestionEngine.Verify(IH,p,"-5.5")); // 14 > bounded lab distance
        p["distance4"]=10;
        Assert.True(ExactSkillContractQuestionEngine.Verify(IH,p,"-4.5"));
        p["direction"]=0;
        Assert.True(ExactSkillContractQuestionEngine.Verify(IV,p,"٠٫٥"));
        p["direction"]=2;
        Assert.False(ExactSkillContractQuestionEngine.Verify(IV,p,"0.5"));
    }
    [Fact] public void InvalidAxisDistanceParametersAreRejected()
    {
        var p=new Dictionary<string,int>{{"a4",-29},{"b4",12},{"fixed4",0}};
        Assert.False(ExactSkillContractQuestionEngine.Verify(H,p,"10.25"));
        p["a4"]=-28;p["unused"]=0;
        Assert.False(ExactSkillContractQuestionEngine.Verify(V,p,"10"));
    }
}

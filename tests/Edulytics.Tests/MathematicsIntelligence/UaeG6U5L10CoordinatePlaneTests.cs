using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG6U5L10CoordinatePlaneTests
{
    const string L="PED:UAE-MOE-MATH:L6:GENERAL:05:10:STUDENT-AR-128",
        Region="uae.g6.u05.l10.coordinate_region",
        X="uae.g6.u05.l10.reflect_across_x",
        Y="uae.g6.u05.l10.reflect_across_y";
    [Fact] public void ArabicContentAndContractsHaveSignedVisualization()
    {
        var rich=RichLessonContentV2Registry.Find(L,"ar");
        Assert.NotNull(rich);
        Assert.Equal(10,rich!.WorkedExamples.Count);
        Assert.All(rich.WorkedExamples,e=>Assert.True(e.Steps.Count>=3));
        Assert.Equal(2,rich.Visuals.Count(v=>v.Kind==RichLessonVisualKind.SignedCartesianPlot));
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(L,out var c));
        Assert.Equal(3,c!.AllowedQuestionFamilies.Count);
        foreach(var family in c.AllowedQuestionFamilies)
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
    }
    [Theory]
    [InlineData(-6,4,"الثاني")]
    [InlineData(6,4,"الأول")]
    [InlineData(-6,-4,"الثالث")]
    [InlineData(6,-4,"الرابع")]
    [InlineData(0,4,"المحور الصادي")]
    [InlineData(4,0,"المحور السيني")]
    [InlineData(0,0,"الأصل")]
    public void QuadrantsAndAxes(int x,int y,string expected)
    {
        var p=new Dictionary<string,int>{{"x2",x},{"y2",y}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(Region,p,expected));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Region,p,"الخامس"));
    }
    [Fact] public void ReflectionsWithArabicAndHalfUnitsAreExact()
    {
        var p=new Dictionary<string,int>{{"x2",-6},{"y2",4}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(X,p,"(−٣،−٢)"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Y,p,"(3,2)"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Y,p,"(-3,-2)"));
        p["x2"]=3;p["y2"]=-1;
        Assert.True(ExactSkillContractQuestionEngine.Verify(X,p,"(١٫٥،٠٫٥)"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Y,p,"(-1.5,-0.5)"));
        p["x2"]=0;p["y2"]=0;
        Assert.True(ExactSkillContractQuestionEngine.Verify(X,p,"(0,0)"));
    }
    [Fact] public void InvalidParametersAreRejected()
    {
        var p=new Dictionary<string,int>{{"x2",11},{"y2",2}};
        Assert.False(ExactSkillContractQuestionEngine.Verify(Region,p,"الأول"));
        p["x2"]=2;p["y2"]=-11;
        Assert.False(ExactSkillContractQuestionEngine.Verify(Y,p,"(-1,-5.5)"));
        p["y2"]=2;p["unused"]=1;
        Assert.False(ExactSkillContractQuestionEngine.Verify(Region,p,"الأول"));
    }
}

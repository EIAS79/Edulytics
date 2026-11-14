using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG6U5L11CoordinatePlotTests
{
    const string L="PED:UAE-MOE-MATH:L6:GENERAL:05:11:STUDENT-AR-128",
        Plot="uae.g6.u05.l11.plot_from_signed_moves",
        Double="uae.g6.u05.l11.reflect_both_axes",
        Distance="uae.g6.u05.l11.mirror_separation",
        Area="uae.g6.u05.l11.axis_aligned_triangle_area";
    [Fact] public void ArabicRichPlotAndContractAreComplete()
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
    [Fact] public void QuarterPlotMovesAreExact()
    {
        var p=new Dictionary<string,int>{{"x4",-13},{"y4",19}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(Plot,p,"(-3.25,4.75)"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Plot,p,"(−٣٫٢٥،٤٫٧٥)"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Plot,p,"(-13,19)"));
        p["x4"]=33;
        Assert.False(ExactSkillContractQuestionEngine.Verify(Plot,p,"(8.25,4.75)"));
    }
    [Fact] public void DoubleReflectionsChangeBothSigns()
    {
        var p=new Dictionary<string,int>{{"x4",-13},{"y4",19}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(Double,p,"(3.25,-4.75)"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Double,p,"(-3.25,-4.75)"));
        p["x4"]=0;p["y4"]=0;
        Assert.True(ExactSkillContractQuestionEngine.Verify(Double,p,"(0,0)"));
    }
    [Fact] public void MirroredDistanceIsTwiceThePerpendicularOffset()
    {
        var p=new Dictionary<string,int>{{"x4",-14},{"y4",7},{"axis",0}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(Distance,p,"٣٫٥"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Distance,p,"1.75"));
        p["axis"]=1;
        Assert.True(ExactSkillContractQuestionEngine.Verify(Distance,p,"7.00"));
        p["axis"]=2;
        Assert.False(ExactSkillContractQuestionEngine.Verify(Distance,p,"7"));
    }
    [Fact] public void RightTriangleAreaUsesSquareUnits()
    {
        var p=new Dictionary<string,int>{{"w4",16},{"h4",12}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(Area,p,"٦"));
        p["w4"]=5;p["h4"]=5;
        Assert.True(ExactSkillContractQuestionEngine.Verify(Area,p,"0.78125"));
        p["w4"]=25;
        Assert.False(ExactSkillContractQuestionEngine.Verify(Area,p,"3.90625"));
    }
}

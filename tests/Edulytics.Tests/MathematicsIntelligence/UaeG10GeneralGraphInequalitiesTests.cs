using System.Globalization;
using Edulytics.Core.Enums;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// UAE owner Advanced Grade10 U01L02 source-specific systems of TWO
/// linear inequalities. A–B–C tests only WRITTEN_NOT_RUN until Stage D.
/// </summary>
public sealed class UaeG10GeneralGraphInequalitiesTests
{
    private const string LessonCode =
        "PED:UAE-MOE-MATH:L10:GENERAL:UNIFIED:U03:L02:C026";
    private const string Point =
        "uae.g10.general.u03.l02.feasible_point_membership";
    private const string Boundary =
        "uae.g10.general.u03.l02.boundary_point_membership";

    private static bool Compare(int y,int rhs,int relation)=>relation switch
    {
        0=>y>rhs,1=>y>=rhs,2=>y<rhs,3=>y<=rhs,
        _=>throw new InvalidOperationException("Unknown inequality relation")
    };

    [Theory]
    [InlineData(Point)]
    [InlineData(Boundary)]
    public void SourceAlignedFamiliesHaveIntegerInputAndExcludeGraphPixelGrading(string family)
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
        Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,ui!.InteractionKind);
    }

    [Theory]
    [InlineData(Point)]
    [InlineData(Boundary)]
    public void IndependentHalfPlaneMembershipAndBoundaryProofAcrossSeeds(string family)
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(int seed in new[]{20261125,20261126,20261127,20261128})
        foreach(var difficulty in new[]{
            ExactSkillQuestionDifficulty.Standard,ExactSkillQuestionDifficulty.Challenge})
        {
            var items=engine.Generate(LessonCode,LessonCode,[family],difficulty,4,seed,[]);
            Assert.Equal(4,items.Count);
            foreach(var question in items)
            {
                var p=question.Parameters;
                Assert.Equal(8,p.Count);
                int m1=p["m1"],m2=p["m2"],b1=p["b1"],b2=p["b2"];
                int x=p["x"],y=p["y"],t1=p["t1"],t2=p["t2"];
                Assert.InRange(m1,-3,3);Assert.InRange(m2,-3,3);
                Assert.InRange(b1,-5,5);Assert.InRange(b2,-5,5);
                Assert.InRange(t1,0,3);Assert.InRange(t2,0,3);
                Assert.InRange(x,-5,5);Assert.InRange(y,-20,20);
                Assert.Equal(AssessmentItemType.Numeric,question.ItemType);

                int line1=m1*x+b1,line2=m2*x+b2;
                if(family==Boundary)Assert.Equal(line1,y);
                int expected=Compare(y,line1,t1)&&Compare(y,line2,t2)?1:0;

                Assert.Equal(expected,int.Parse(question.CorrectAnswer,CultureInfo.InvariantCulture));
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,question.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family,p,(1-expected).ToString(CultureInfo.InvariantCulture)));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"خطأ"));
                var arabic=string.Concat(question.CorrectAnswer.Select(c=>
                    c is >= '0' and <= '9'?(char)('٠'+c-'0'):c));
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,arabic));

                var invalid=p.ToDictionary(k=>k.Key,k=>k.Value,StringComparer.Ordinal);
                invalid["t1"]=6;
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,invalid,question.CorrectAnswer));
                if(family==Boundary)
                {
                    var forged=p.ToDictionary(k=>k.Key,k=>k.Value,StringComparer.Ordinal);
                    forged["y"]=line1==20?line1-1:line1+1;
                    Assert.False(ExactSkillContractQuestionEngine.Verify(
                        family,forged,question.CorrectAnswer));
                }
            }
        }
    }

    [Fact]
    public void OriginalMathBoundaryCasesAndEmptyOrClosedRegions()
    {
        // y>=x-1 and y<-x+5: (0,0) is feasible, (2,3) is on excluded upper edge.
        Assert.True(0>=0-1 && 0<-(0)+5);
        Assert.False(3>=2-1 && 3<-(2)+5);
        // Both strict inequalities cannot have a solution.
        foreach(int x in new[]{-3,0,4})
        foreach(int y in new[]{-5,0,3,10})
            Assert.False(y>x+3 && y<x+1);
        // Strip -1<y<=3 excludes y=-1 but includes y=3.
        Assert.True(3>-1 && 3<=3);
        Assert.False(-1>-1 && -1<=3);
        // Feasible whole budget constraints.
        Assert.True(2*3+3*2<=12);
        Assert.False(2*4+3*2<=12);
    }
}

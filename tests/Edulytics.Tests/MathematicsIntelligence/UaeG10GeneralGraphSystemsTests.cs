using System.Globalization;
using Edulytics.Core.Enums;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Owner Advanced source lesson1-1 on graphing two-line systems.
/// Authored during Grade10 A–B–C; tests are NOT RUN until Stage D.
/// </summary>
public sealed class UaeG10GeneralGraphSystemsTests
{
    private const string LessonCode="PED:UAE-MOE-MATH:L10:GENERAL:UNIFIED:U03:L01:C025";
    private const string X="uae.g10.general.u03.l01.graph_intersection_x";
    private const string Y="uae.g10.general.u03.l01.graph_intersection_y";
    private const string Count="uae.g10.general.u03.l01.graph_solution_count";

    [Theory]
    [InlineData(X)]
    [InlineData(Y)]
    [InlineData(Count)]
    public void AllThreeGraphSystemFamiliesHaveAnExactIntegerRenderer(string family)
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var interaction));
        Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,interaction!.InteractionKind);
    }

    [Theory]
    [InlineData(X)]
    [InlineData(Y)]
    [InlineData(Count)]
    public void SourceExactLinearSystemsHaveIndependentTwoEquationChecks(string family)
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(int seed in new[]{20261125,20261126,20261127})
        foreach(var difficulty in new[]{ExactSkillQuestionDifficulty.Standard,
            ExactSkillQuestionDifficulty.Challenge})
        {
            var qs=engine.Generate(LessonCode,LessonCode,[family],difficulty,4,seed,[]);
            Assert.Equal(4,qs.Count);
            foreach(var q in qs)
            {
                var p=q.Parameters;Assert.Equal(4,p.Count);
                int m1=p["m1"],m2=p["m2"],b1=p["b1"],b2=p["b2"];
                Assert.InRange(m1,-4,4);Assert.InRange(m2,-4,4);
                Assert.InRange(b1,-30,30);Assert.InRange(b2,-30,30);
                int expected;
                if(m1!=m2)
                {
                    Assert.Equal(0,(b2-b1)%(m1-m2));
                    int x=(b2-b1)/(m1-m2),y=m1*x+b1;
                    Assert.InRange(x,-5,5);Assert.InRange(y,-5,5);
                    Assert.Equal(y,m2*x+b2);
                    expected=family==X?x:family==Y?y:1;
                }
                else
                {
                    Assert.Equal(Count,family);
                    expected=b1==b2?2:0;
                }
                Assert.Equal(expected,int.Parse(q.CorrectAnswer,CultureInfo.InvariantCulture));
                Assert.Equal(AssessmentItemType.Numeric,q.ItemType);
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,
                    (expected+1).ToString(CultureInfo.InvariantCulture)));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"خطأ"));
                var arabic=string.Concat(q.CorrectAnswer.Select(c=>
                    c is >= '0' and <= '9'?(char)('٠'+c-'0'):c));
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,arabic));
                var invalid=p.ToDictionary(x=>x.Key,x=>x.Value,StringComparer.Ordinal);
                invalid["m1"]=20;
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,invalid,q.CorrectAnswer));
            }
        }
    }

    [Fact]
    public void OriginalGraphIntersectionAndParallelOrCoincidentCasesMatchGoldenMathematics()
    {
        // y=2x+1 and y=-x+7 meet at (2,5).
        Assert.Equal(5,2*2+1);
        Assert.Equal(5,-2+7);
        // y=x-3 and y=-2x+6 meet at (3,0).
        Assert.Equal(0,3-3);
        Assert.Equal(0,-2*3+6);
        // y=3x-2 and y=3x+4 never meet.
        foreach(int x in new[]{-5,-1,0,2,8})
            Assert.NotEqual(3*x-2,3*x+4);
        // y=2x+1 and 2y=4x+2 are identical.
        foreach(int x in new[]{-4,0,3,7})
            Assert.Equal(2*(2*x+1),4*x+2);
        // Two original AED cost plans: 10+4x and 2+6x at x=4 both cost 26.
        Assert.Equal(26,10+4*4);
        Assert.Equal(26,2+6*4);
    }
}

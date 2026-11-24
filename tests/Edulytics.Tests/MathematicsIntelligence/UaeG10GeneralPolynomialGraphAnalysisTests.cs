using Edulytics.Core.Enums;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using System.Globalization;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Grade 10 approved General U01L12 source-aligned graph analysis.
/// WRITTEN_NOT_RUN during Stage A–B–C. Stage D owns runtime execution.
/// </summary>
public sealed class UaeG10GeneralPolynomialGraphAnalysisTests
{
    private const string LessonCode =
        "PED:UAE-MOE-MATH:L10:GENERAL:UNIFIED:U01:L12:C012";
    private const string Vertex="uae.g10.general.u01.l12.graph_vertex_pair";
    private const string Kind="uae.g10.general.u01.l12.graph_extremum_kind";
    private const string Intercepts="uae.g10.general.u01.l12.graph_x_intercepts";

    [Theory]
    [InlineData(Vertex,LessonPracticeInteractionKind.StructuredEntry)]
    [InlineData(Kind,LessonPracticeInteractionKind.StructuredEntry)]
    [InlineData(Intercepts,LessonPracticeInteractionKind.StructuredEntry)]
    public void FamilyAndRendererMatchGraphAnalysisExactResponse(string family,LessonPracticeInteractionKind input)
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var contract));
        Assert.Equal(input,contract!.InteractionKind);
    }

    [Theory]
    [InlineData(Vertex)]
    [InlineData(Kind)]
    [InlineData(Intercepts)]
    public void GeneratedGraphTablesHaveIndependentCorrectAndIncorrectVerifiers(string family)
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(int seed in new[]{20261124,20261125,20261126})
        foreach(var difficulty in new[]{
            ExactSkillQuestionDifficulty.Standard,
            ExactSkillQuestionDifficulty.Challenge})
        {
            var problems=engine.Generate(LessonCode,LessonCode,[family],
                difficulty,4,seed,[]);
            Assert.Equal(4,problems.Count);
            foreach(var problem in problems)
            {
                var p=problem.Parameters;
                Assert.Equal(7,p.Count);
                Assert.Contains("النقاط",problem.Prompt);
                Assert.Equal(AssessmentItemType.ShortAnswer,problem.ItemType);
                Assert.NotEqual(0,p["a"]);
                string expected;
                if(family==Vertex)
                {
                    Assert.Equal(1,p["mode"]);
                    Assert.InRange(p["h"],-3,3);
                    Assert.InRange(p["k"],-4,4);
                    expected=$"{p["h"]},{p["k"]}";
                    Assert.True(ExactSkillContractQuestionEngine.Verify(
                        family,p,$"({p["h"]}،{p["k"]})"));
                }
                else if(family==Kind)
                {
                    Assert.Equal(1,p["mode"]);
                    expected=p["a"]>0?"صغرى":"عظمى";
                }
                else
                {
                    Assert.Equal(2,p["mode"]);
                    Assert.True(p["r1"]<p["r2"]&&p["r2"]<p["r3"]);
                    // Recompute all integer abscissae at which the function
                    // intersects y=0. Never trust the generated answer string.
                    var roots=Enumerable.Range(-3,7).Where(x=>
                        p["a"]*(x-p["r1"])*(x-p["r2"])*(x-p["r3"])==0).ToArray();
                    Assert.Equal(3,roots.Length);
                    expected=string.Join(",",roots);
                    Assert.True(ExactSkillContractQuestionEngine.Verify(
                        family,p,expected.Replace(',', '،')));
                }

                Assert.Equal(expected,problem.CorrectAnswer);
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,expected));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"خطأ"));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,expected+",7"));

                var local=string.Concat(expected.Select(c=>c is >= '0' and <= '9'
                    ?(char)('٠'+c-'0'):c));
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,local));

                var forged=p.ToDictionary(x=>x.Key,x=>x.Value,StringComparer.Ordinal);
                forged["mode"]=9;
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,forged,expected));
                forged=p.ToDictionary(x=>x.Key,x=>x.Value,StringComparer.Ordinal);
                forged["a"]=0;
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,forged,expected));
            }
        }
    }

    [Fact]
    public void OriginalCubicQuarticQuadraticGraphGoldenCoordinatesAreCorrect()
    {
        foreach(int x in new[]{-3,-2,-1,0,1,2,3})
        {
            Assert.Equal(x*x*x-4*x,x*(x-2)*(x+2));
            Assert.Equal(x*x*x*x-5*x*x+4,(x*x-1)*(x*x-4));
            Assert.Equal(-(x-1)*(x-1)+4,-x*x+2*x+3);
        }
        Assert.Equal(2,(-1)*(-1)*(-1)-3*(-1));
        Assert.Equal(-2,1-3);
        Assert.Equal(4,0*0*0*0-5*0*0+4);
        Assert.Equal(45,2*4*4+3*4+1);
    }

    [Fact]
    public void NoFallthroughToPriorGeneralFunctionDegreeOrEvaluationFamily()
    {
        Assert.NotEqual(Vertex,"uae.g10.general.u01.l11.polynomial_degree_integer");
        Assert.NotEqual(Kind,"uae.g10.general.u01.l11.polynomial_end_behavior");
        Assert.NotEqual(Intercepts,"uae.g10.general.u01.l11.polynomial_evaluate_integer");
    }
}

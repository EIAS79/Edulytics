using System.Text.Json;
using Edulytics.Services.Mathematics;
using Edulytics.Web.Presentation;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Visual scaffolds are generated from the actual question parameters;
/// these tests do not certify a freehand graph or an essay grader.</summary>
public sealed class UsCommonCoreGraphVectorVisualTests
{
    private static readonly string[] Families =
    [
        "usccss.hs.rational_hole_or_pole",
        "usccss.hs.rational_branch_limit_sign",
        "usccss.vector.head_tail_endpoint_y",
        "usccss.vector.scalar_direction_class"
    ];

    [Fact]
    public void AllFourScopedFamiliesRenderSafeDeterministicDiagramsAcrossSeededQuestions()
    {
        var engine = new ExactSkillContractQuestionEngine();
        var checkedCount = 0;
        foreach (var family in Families)
        {
            foreach (var difficulty in new[]
            {
                ExactSkillQuestionDifficulty.Standard,
                ExactSkillQuestionDifficulty.Stretch,
                ExactSkillQuestionDifficulty.Challenge
            })
            {
                for (var seed=0;seed<12;seed++)
                {
                    var question=Assert.Single(engine.Generate(
                        "visual-ccss-release", family, [family], difficulty, 1,
                        8700000+checkedCount, []));
                    var json=JsonSerializer.Serialize(new { parameters=question.Parameters });
                    var first=PracticeMathVisualRenderer.RenderSvg(family,json);
                    var second=PracticeMathVisualRenderer.RenderSvg(family,json);
                    Assert.NotNull(first);
                    Assert.Equal(first,second);
                    Assert.StartsWith("<svg", first!, StringComparison.Ordinal);
                    Assert.Contains("</svg>",first!,StringComparison.Ordinal);
                    Assert.DoesNotContain("<script",first!,StringComparison.OrdinalIgnoreCase);
                    Assert.True(ExactSkillContractQuestionEngine.Verify(
                        question.Family,question.Parameters,question.CorrectAnswer));
                    if (family.Contains(".vector.", StringComparison.Ordinal))
                        Assert.Contains("class='vector",first!,StringComparison.Ordinal);
                    else
                        Assert.Contains("<polyline",first!,StringComparison.Ordinal);
                    checkedCount++;
                }
            }
        }
        Assert.Equal(4*3*12,checkedCount);
    }

    [Fact]
    public void HeadToTailDiagramContainsGenuineTwoStepVectorDisplacements()
    {
        var svg=PracticeMathVisualRenderer.RenderSvg(
            "usccss.vector.head_tail_endpoint_y",
            JsonSerializer.Serialize(new { parameters=new {
                startX=2,startY=-1,ax=3,ay=4,bx=-2,by=5
            }}));
        Assert.NotNull(svg);
        Assert.Contains("Head-to-tail",svg!,StringComparison.Ordinal);
        Assert.Contains(">Start</text>",svg!,StringComparison.Ordinal);
        Assert.Contains(">End</text>",svg!,StringComparison.Ordinal);
        Assert.DoesNotContain("(3, 8)",svg!,StringComparison.Ordinal);
    }

    [Fact]
    public void HoleAndPoleShowDifferentGraphicMarksWithFiniteSvgCoordinates()
    {
        var svg=PracticeMathVisualRenderer.RenderSvg(
            "usccss.hs.rational_hole_or_pole",
            JsonSerializer.Serialize(new { parameters=new {
                removed=-2,pole=3,root=4,ask=-2
            }}));
        Assert.NotNull(svg);
        Assert.Contains("<circle",svg!,StringComparison.Ordinal);
        Assert.Contains("dash",svg!,StringComparison.Ordinal);
        Assert.DoesNotContain("NaN",svg!,StringComparison.Ordinal);
        Assert.DoesNotContain("Infinity",svg!,StringComparison.Ordinal);
    }

    [Fact]
    public void MalformedOrAbsentParametersFailClosed()
    {
        Assert.Null(PracticeMathVisualRenderer.RenderSvg("usccss.vector.head_tail_endpoint_y","{}"));
        Assert.Null(PracticeMathVisualRenderer.RenderSvg("usccss.hs.rational_hole_or_pole","not json"));
    }
}
using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.LessonContent;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Official IM 2021 G1 U01 L02 source Activity 1 and Activity 2 contract.
/// This is a source/activity regression inside the 500-lesson B01-wide
/// tests; it is not individual academic L1-L6 certification.
/// </summary>
public sealed class UsCcssB01SourceDotMatchingActivitiesTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G1:U01:L02";
    private static readonly string[] SourceFamilies =
    [
        "usccss.b01.g1.match_dot_cubes_expression",
        "usccss.b01.g1.complete_missing_dots",
        "usccss.b01.g1.check_off_add_within10"
    ];

    [Fact]
    public void OfficialSourceLessonRequiresAllThreeDistinctActivities()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            Lesson, out var contract));
        Assert.NotNull(contract);
        Assert.Equal(SourceFamilies, contract!.AllowedQuestionFamilies);
        Assert.StartsWith("IM2021_G1_U01_L02_", contract.SourceType,
            StringComparison.Ordinal);
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G1:U01:L03", out var adjacent));
        Assert.DoesNotContain(adjacent!.AllowedQuestionFamilies,
            f => f.StartsWith("usccss.b01.g1.",StringComparison.Ordinal));
    }

    [Fact]
    public void EveryDotTaskHasTheSameVerifiableGroupsExpressionAndPicture()
    {
        var engine = new ExactSkillContractQuestionEngine();
        var cases = 0;
        foreach (var family in SourceFamilies)
        {
            foreach (var difficulty in new[]
            {
                ExactSkillQuestionDifficulty.Standard,
                ExactSkillQuestionDifficulty.Stretch,
                ExactSkillQuestionDifficulty.Challenge
            })
            {
                for (var i = 0; i < 12; i++)
                {
                    var q = Assert.Single(engine.Generate(
                        "b01-official-g1-u01-l02-dot-cube-activities",
                        Lesson, [family], difficulty, 1,
                        925500 + cases * 17, []));
                    int first = q.Parameters["first"];
                    int second = q.Parameters["second"];
                    Assert.InRange(first + second, 0, 10);
                    Assert.True(ExactSkillContractQuestionEngine.Verify(
                        family, q.Parameters, q.CorrectAnswer));
                    var independentlyExpected = family.EndsWith(
                            ".match_dot_cubes_expression",StringComparison.Ordinal)
                        ? $"{first}+{second}"
                        : family.EndsWith(".complete_missing_dots",
                            StringComparison.Ordinal)
                            ? second.ToString()
                            : (first+second).ToString();
                    Assert.Equal(independentlyExpected,q.CorrectAnswer);
                    Assert.False(ExactSkillContractQuestionEngine.Verify(
                        family,q.Parameters,"999"));
                    if (family.EndsWith("check_off_add_within10",
                        StringComparison.Ordinal))
                    {
                        Assert.InRange(first,0,5);
                        Assert.InRange(second,0,5);
                    }

                    var diagram = Assert.Single(
                        UsCcssB01SourceTaskVisuals.Build(Lesson,[q]));
                    Assert.Equal(RichLessonVisualKind.PlaceValueChart,
                        diagram.Kind);
                    Assert.Equal(2,diagram.Rows.Count);
                    Assert.Equal(first,diagram.Rows[0].Cells.Count(x=>x=="●"));
                    Assert.Equal(second,diagram.Rows[1].Cells.Count(x=>x=="●"));
                    Assert.Contains($"{first} + {second} = {first+second}",
                        diagram.Description);
                    cases++;
                }
            }
        }
        Assert.Equal(108,cases);
    }
}

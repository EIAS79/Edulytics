using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Grade 4 U01 L01 original IM 2021 arithmetic and Rich V2 source-only guards.
/// Authoring Stage A-B-C: WRITTEN_NOT_RUN. Execute in subsequent Stage D.
/// </summary>
public sealed class Grade4Unit1Lesson1Original2021SourcePracticeTests
{
    private const string Code = "PED:US-CCSS-MATH:G4:U01:L01";

    [Fact]
    public void ExactSourceSkillContractOverridesGenericFactorsPrimeOnlyRouting()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Code, out var contract));
        Assert.NotNull(contract);
        Assert.Equal("geometry.rectangle.area", contract.SkillId);
        Assert.Equal("RECTANGLE_WIDTH_AREA_MULTIPLES", contract.Mechanic);
        Assert.Equal(4, contract.AllowedQuestionFamilies.Count);
        Assert.All(contract.AllowedQuestionFamilies,
            family => Assert.StartsWith("usccss.g4.u01.l01.", family));
        Assert.All(contract.AllowedQuestionFamilies,
            family => Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family)));
        Assert.DoesNotContain("number.lcm.two_numbers",
            contract.AllowedQuestionFamilies);
    }

    [Fact]
    public void EverySourceSpecificNumericFamilyRecomputesExactMathAndRejectsBadAnswers()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Code, out var contract));
        Assert.NotNull(contract);
        var engine = new ExactSkillContractQuestionEngine();
        foreach (var family in contract.AllowedQuestionFamilies)
        {
            var generated = engine.Generate(
                fingerprintNamespace:"g4-original-2021-source-regression",
                scopeKey:Code,
                allowedQuestionFamilies:[family],
                difficulty:ExactSkillQuestionDifficulty.Standard,
                questionCount:8,
                seed:417,
                excludedExposureFingerprints:[]);
            Assert.Equal(8, generated.Count);
            foreach (var q in generated)
            {
                Assert.Equal(family, q.Family);
                Assert.True(ExactSkillContractQuestionEngine.Verify(
                    q.Family,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    q.Family,q.Parameters,"-999"));

                var p=q.Parameters;
                var width=p["width"];
                var expected=family switch
                {
                    "usccss.g4.u01.l01.fixed_width_area" =>
                        width * p["length"],
                    "usccss.g4.u01.l01.missing_whole_length" =>
                        p["area"] / width,
                    _ => p["area"] % width == 0 ? 1 : 0
                };
                Assert.Equal(expected.ToString(System.Globalization.CultureInfo.InvariantCulture),
                    q.CorrectAnswer);
                if (family.EndsWith(".fixed_width_area",StringComparison.Ordinal))
                {
                    Assert.InRange(width,2,4);
                    Assert.InRange(p["length"],1,10);
                }
                else
                {
                    Assert.Equal(3,width);
                    Assert.True(family.EndsWith(".possible_area_above30",StringComparison.Ordinal)
                        ? p["area"] > 30 : p["area"] <= 30);
                }
            }
        }
    }

    [Fact]
    public void InvalidDimensionsCannotBeMarkedCorrect()
    {
        const string areaFamily="usccss.g4.u01.l01.fixed_width_area";
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            areaFamily,new Dictionary<string,int>{{"width",0},{"length",4}},"0"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            areaFamily,new Dictionary<string,int>{{"width",3},{"length",0}},"0"));
        const string multiple="usccss.g4.u01.l01.area_is_multiple";
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            multiple,new Dictionary<string,int>{{"width",3},{"area",31}},"0"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            multiple,new Dictionary<string,int>{{"width",3},{"area",28}},"1"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(
            multiple,new Dictionary<string,int>{{"width",3},{"area",28}},"0"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(
            multiple,new Dictionary<string,int>{{"width",3},{"area",30}},"1"));
    }

    [Fact]
    public void RichV2Preserves2021WarmupFiguresAndOpenTeacherTaskBoundaries()
    {
        var lesson=RichLessonContentV2Registry.Find(Code,"en");
        Assert.NotNull(lesson);
        Assert.Equal("Multiples of a Number",lesson.Title);
        Assert.Contains(lesson.Visuals,
            x=>x.Kind==RichLessonVisualKind.Grade4Unit1AreaFigures);
        Assert.Contains(lesson.ExplanationParagraphs,
            x=>x.Contains("24 square units",StringComparison.Ordinal) &&
               x.Contains("teacher-reviewed",StringComparison.Ordinal));
        Assert.Contains(lesson.ExplanationParagraphs,
            x=>x.Contains("FIVE distinct",StringComparison.Ordinal) &&
               x.Contains("width",StringComparison.Ordinal));
        Assert.Contains(lesson.ExplanationParagraphs,
            x=>x.Contains("Cool-down",StringComparison.Ordinal) &&
               x.Contains("not represented as verified",StringComparison.Ordinal));
    }
}

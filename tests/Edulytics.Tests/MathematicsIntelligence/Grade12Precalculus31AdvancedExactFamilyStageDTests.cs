using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Independent Grade12 Stage D runtime acceptance for 31 source-skills.
/// These certify numerical subsets, NOT ungraded drawings or source parity.
/// </summary>
public sealed class Grade12Precalculus31AdvancedExactFamilyStageDTests
{
    private const string LessonPrefix =
        "PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:";

    [Fact]
    public void All31AdvancedSourceSectionsHaveExplicitIndependentExactPractice()
    {
        var families=Grade12PrecalculusAdvancedNumericQuestions.All;
        Assert.Equal(31,families.Count);
        Assert.Equal(31,families.Values.Distinct(StringComparer.Ordinal).Count());

        var engine=new ExactSkillContractQuestionEngine();
        var index=0;
        foreach(var (suffix,section) in families)
        {
            var family=Grade12PrecalculusAdvancedNumericQuestions.Prefix+suffix;
            var lesson=LessonPrefix+section;
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family),family);
            Assert.True(LessonPracticeCapabilityResolver.TryResolve(lesson,out var capability),
                $"Missing scoped runtime Practice capability: {lesson}");
            Assert.NotNull(capability);
            Assert.Equal("READY_VERIFIED",capability.Readiness);
            Assert.Single(capability.AllowedQuestionFamilies);
            Assert.Equal(family,capability.AllowedQuestionFamilies[0]);

            foreach(var difficulty in new[]
            {
                ExactSkillQuestionDifficulty.Standard,
                ExactSkillQuestionDifficulty.Stretch,
                ExactSkillQuestionDifficulty.Challenge
            })
            {
                var question=Assert.Single(engine.Generate(
                    "g12-31-original-numerical-source-skill",
                    lesson,[family],difficulty,1,33000+index,[]));
                Assert.Equal(family,question.Family);
                Assert.True(ExactSkillContractQuestionEngine.Verify(
                    family,question.Parameters,question.CorrectAnswer),
                    $"Answer rejected: {section} {difficulty}");
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family,question.Parameters,"not-an-integer"),
                    $"Malformed answer accepted: {section}");
                Assert.False(string.IsNullOrWhiteSpace(question.Prompt));
                Assert.False(string.IsNullOrWhiteSpace(question.Solution));
            }
            index++;
        }
    }

    [Fact]
    public void EveryAdvancedNumericalFamilySupportsTenDistinctVerifiedPracticeItems()
    {
        var engine=new ExactSkillContractQuestionEngine();
        var index=0;
        foreach(var (suffix,section) in Grade12PrecalculusAdvancedNumericQuestions.All)
        {
            var family=Grade12PrecalculusAdvancedNumericQuestions.Prefix+suffix;
            var questions=engine.Generate("g12-advanced-10-item-capacity",
                LessonPrefix+section,
                [family],
                ExactSkillQuestionDifficulty.Standard,
                10,600000+index,[]);
            Assert.Equal(10,questions.Count);
            Assert.Equal(10,questions.Select(q=>q.ExposureFingerprint)
                .Distinct(StringComparer.Ordinal).Count());
            foreach(var q in questions)
            {
                Assert.Equal(family,q.Family);
                Assert.True(ExactSkillContractQuestionEngine.Verify(
                    family,q.Parameters,q.CorrectAnswer),
                    $"Invalid exact mathematics item in {section}");
            }
            index++;
        }
    }
}

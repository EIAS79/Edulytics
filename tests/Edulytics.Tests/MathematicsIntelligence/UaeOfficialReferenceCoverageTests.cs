


using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UaeOfficialReferenceCoverageTests
{
    [Fact]
    public void UaeBlueprints_KeepOfficialOutcomesSeparateFromTextbookReferences()
    {
        var lessons = PedagogicalLessonBlueprintRegistry
            .LoadEmbeddedDocuments()
            .Where(x => x.PackCode == MathematicsCurriculumPackRegistry.UaeCode)
            .SelectMany(x => x.Lessons)
            .ToArray();

        Assert.NotEmpty(lessons);

        var referenceCodes = lessons
            .Where(x => !string.IsNullOrWhiteSpace(x.OfficialReferenceCode))
            .Select(x => x.OfficialReferenceCode!)
            .ToArray();

        Assert.Equal(
            referenceCodes.Length,
            referenceCodes.Distinct(StringComparer.Ordinal).Count());

        foreach (var lesson in lessons)
        {
            Assert.All(
                lesson.OutcomeCodes,
                code => Assert.StartsWith(
                    "UAE:STD:MAT.",
                    code,
                    StringComparison.Ordinal));

            if (string.IsNullOrWhiteSpace(lesson.OfficialReferenceCode))
                continue;

            Assert.StartsWith(
                "UAE:REF:TEXTBOOK:",
                lesson.OfficialReferenceCode,
                StringComparison.Ordinal);

            Assert.Contains(
                lesson.Alignments,
                x =>
                    x.Role == "Addressing" &&
                    x.ReferenceKind == "OfficialReference" &&
                    x.ResolutionKind == "ExactAcceptedReference" &&
                    x.ReferenceCode == lesson.OfficialReferenceCode &&
                    string.IsNullOrWhiteSpace(x.OutcomeCode));
        }
    }

    [Fact]
    public void UaeCurriculum_HasZeroSupportingLessons_AndNoGrade5Or6AdvancedPathway()
    {
        var blueprints = PedagogicalLessonBlueprintRegistry
            .LoadEmbeddedDocuments()
            .Where(x => x.PackCode == MathematicsCurriculumPackRegistry.UaeCode)
            .ToArray();

        Assert.DoesNotContain(
            blueprints,
            x => (x.LogicalLevel == 5 || x.LogicalLevel == 6) &&
                 string.Equals(x.Pathway, "Advanced", StringComparison.OrdinalIgnoreCase));

        var grade4 = Assert.Single(blueprints, x =>
            x.LogicalLevel == 4 &&
            string.Equals(x.Pathway, "Common",
                StringComparison.OrdinalIgnoreCase));
        Assert.Equal(132, grade4.Lessons.Count);
        Assert.Equal(49, grade4.Lessons.Count(x =>
            x.OutcomeCodes.Count == 0 &&
            string.IsNullOrWhiteSpace(x.OfficialReferenceCode)));
        Assert.Equal(83, grade4.Lessons.Count(x =>
            !string.IsNullOrWhiteSpace(x.OfficialReferenceCode)));

        foreach (var blueprint in blueprints)
        {
            Assert.All(
                blueprint.Lessons,
                lesson =>
                {
                    Assert.True(
                        CanonicalLessonRoleRegistry.TryGetIsSupporting(
                            lesson.LessonCode,
                            out var isSupporting));

                    Assert.False(isSupporting);

                    // The G4 owner-authored source audit explicitly records
                    // 49 lessons without an accepted official reference node.
                    // Do not invent official outcomes or imply source parity:
                    // the independent authority/release gate stays mandatory.
                    if (blueprint.LogicalLevel == 4 &&
                        string.Equals(blueprint.Pathway, "Common",
                            StringComparison.OrdinalIgnoreCase) &&
                        string.IsNullOrWhiteSpace(lesson.OfficialReferenceCode) &&
                        lesson.OutcomeCodes.Count == 0)
                    {
                        Assert.StartsWith(
                            "PED:UAE-MOE-MATH:L4:COMMON:",
                            lesson.LessonCode, StringComparison.Ordinal);
                        Assert.Empty(lesson.Alignments);
                    }
                    else
                    {
                        Assert.True(
                            lesson.OutcomeCodes.Count > 0 ||
                            !string.IsNullOrWhiteSpace(lesson.OfficialReferenceCode));
                    }
                });
        }
    }


    [Fact]
    public void UaeAdvancedUnit5_UsesReviewedTopicSpecificPracticeContracts()
    {
        var expected = new Dictionary<string, (string Mechanic, string Family)>
        {
            ["PED:UAE-MOE-MATH:L11:ADVANCED:05:01:MULTIVARIABLE-LINEAR-SYSTEMS-AND-ELEMENTARY-ROW-OPERATIONS"] =
                ("SIMULTANEOUS", "supporting.algebra.simultaneous"),
            ["PED:UAE-MOE-MATH:L11:ADVANCED:05:03:SOLVING-LINEAR-SYSTEMS-USING-INVERSES-AND-CRAMER-S-RULE"] =
                ("SIMULTANEOUS", "supporting.algebra.simultaneous"),
            ["PED:UAE-MOE-MATH:L11:ADVANCED:05:04:PARTIAL-FRACTIONS"] =
                ("ALGEBRAIC_FRACTION", "supporting.algebra.algebraic_fraction"),
            ["PED:UAE-MOE-MATH:L11:ADVANCED:05:05:LINEAR-PROGRAMMING"] =
                ("LINEAR_PROGRAMMING", "supporting.algebra.linear_programming.vertex_optimum")
        };

        foreach (var (lessonCode, target) in expected)
        {
            Assert.True(LessonPracticeContractRegistry.TryResolve(lessonCode, out var contract));
            Assert.NotNull(contract);
            Assert.Equal(target.Mechanic, contract!.Mechanic);
            Assert.Contains(target.Family, contract.AllowedQuestionFamilies);
            Assert.DoesNotContain(
                "supporting.algebra.expressions.mixed",
                contract.AllowedQuestionFamilies);
        }
    }

}

using Edulytics.Core.Curriculum;

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

        Assert.Equal(716, lessons.Length);
        Assert.Equal(75, lessons.Count(x => x.OutcomeCodes.Count > 0));
        Assert.Equal(265, lessons.Count(x => !string.IsNullOrWhiteSpace(x.OfficialReferenceCode)));
        Assert.Equal(
            20,
            lessons.Count(x =>
                x.OutcomeCodes.Count > 0 &&
                !string.IsNullOrWhiteSpace(x.OfficialReferenceCode)));
        Assert.Equal(
            396,
            lessons.Count(x =>
                x.OutcomeCodes.Count == 0 &&
                string.IsNullOrWhiteSpace(x.OfficialReferenceCode)));

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
}

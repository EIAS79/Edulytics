using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
using Edulytics.Data.Seeding;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class CommonCoreG2HundredsPlaceValueSourceCorrectionsTests
{
    private static readonly string[] ExpectedCodes =
    [
        "PED:US-CCSS-MATH:G2:U05:L01",
        "PED:US-CCSS-MATH:G2:U05:L02",
        "PED:US-CCSS-MATH:G2:U05:L03"
    ];

    private static IReadOnlyList<(CanonicalLessonContentPackDocument Document,
        CanonicalLessonContentPackLesson Lesson,
        CanonicalLessonContentPackTranslation English)> GetTargets() =>
        MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(d => d.Lessons.Where(l =>
                CommonCoreG2HundredsPlaceValueSourceCorrections.IsTarget(d, l))
                .Select(l => (Document:d, Lesson:l,
                    English:l.Translations.Single(t => t.CultureCode == "en"))))
            .ToArray();

    [Fact]
    public void ThreeSourceLockedLessonsHaveTargetSpecificMathematicalExplanations()
    {
        var targets = GetTargets();
        Assert.Equal(ExpectedCodes, targets.Select(x => x.Lesson.LessonCode).ToArray());
        Assert.All(targets, row =>
        {
            Assert.Equal(CommonCoreG2Unit5OriginalRichContentCorrections.VersionFor(
                    68 + Array.IndexOf(ExpectedCodes,row.Lesson.LessonCode)),
                CanonicalLessonContentMaterializer.GetEffectiveContentVersion(
                    row.Document, row.Lesson));
            Assert.DoesNotContain("Measurement units must be equal in size",
                row.English.KeyConceptsAndRules);
            Assert.DoesNotContain("Identify what needs to be measured",
                row.English.StepByStepSolutions);
            Assert.DoesNotContain("Apply the key idea from the lesson",
                row.English.StepByStepSolutions);
            Assert.Contains("ORIGINAL", row.English.StepByStepSolutions);
            Assert.True(row.English.StepByStepSolutions.Length > 180);
            Assert.Contains("hundred", row.English.StepByStepSolutions,
                StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("...", row.English.WorkedExamples);
            Assert.True(row.English.WorkedExamples.Length > 200);
            Assert.Equal("CC BY 4.0", row.Lesson.SourceRights);
        });

        var first = targets[0];
        Assert.Contains("10 tens = 100 ones = 1 hundred", first.English.StepByStepSolutions);
        Assert.Contains("90 + 10 = 100", first.English.WorkedExamples);
        Assert.Equal(new[] { "CCSS:2.NBT.A.1", "CCSS:2.NBT.A.2" }, first.Lesson.OutcomeCodes);
        Assert.Equal(100, 10 * 10);

        var second = targets[1];
        Assert.Contains("20 tens = 2 hundreds = 200", second.English.WorkedExamples);
        Assert.Contains("80 × 10 = 800", second.English.WorkedExamples);
        Assert.Equal(800, 80 * 10);
        Assert.Equal(110, 11 * 10);

        var third = targets[2];
        Assert.Contains("200 + 280 + 15 = 495", third.English.WorkedExamples);
        Assert.Contains("400 + 90 + 5 = 495", third.English.StepByStepSolutions);
        Assert.Equal(new[] {"CCSS:2.NBT.A.1", "CCSS:2.NBT.A.2", "CCSS:2.NBT.B.5"},
            third.Lesson.OutcomeCodes);
        Assert.Equal(495, 2*100 + 28*10 + 15);
        Assert.Equal(495, 4*100 + 9*10 + 5);
        Assert.Equal(342, 2*100 + 14*10 + 2);
    }

    [Fact]
    public void SourceCorrectionIsIdempotentAndDoesNotTouchOtherLessons()
    {
        var targets = GetTargets();
        foreach (var row in targets)
        {
            var before = CanonicalLessonContentMaterializer.ComputeLessonFingerprint(
                row.Document, row.Lesson);
            CanonicalLessonContentMaterializer.Materialize(row.Document);
            Assert.Equal(before, CanonicalLessonContentMaterializer.ComputeLessonFingerprint(
                row.Document, row.Lesson));
        }
        var second = targets[1].Document;
        var other = Assert.Single(second.Lessons,
            x => x.LessonCode == "PED:US-CCSS-MATH:G2:U05:L04");
        Assert.False(CommonCoreG2HundredsPlaceValueSourceCorrections
            .IsTarget(second, other));

        var currentVersion = second.ContentVersion;
        try
        {
            second.ContentVersion = "unrelated-fixture-v1";
            Assert.False(CommonCoreG2HundredsPlaceValueSourceCorrections
                .IsTarget(second, targets[1].Lesson));
        }
        finally
        {
            second.ContentVersion = currentVersion;
        }
    }

    [Fact]
    public void SourceIdentityAndPersistedBodyDriftFailClosed()
    {
        var row = GetTargets()[0];
        var old = row.Lesson.CanonicalBodySha256;
        try
        {
            row.Lesson.CanonicalBodySha256 = new string('0', 64);
            Assert.Throws<InvalidOperationException>(() =>
                CommonCoreG2HundredsPlaceValueSourceCorrections.Apply(row.Document));
        }
        finally
        {
            row.Lesson.CanonicalBodySha256 = old;
        }

        Assert.True(CommonCoreG2HundredsPlaceValueSourceCorrections.CanUpgradeExisting(
            row.Document, row.Lesson,
            CommonCoreRichContentCorrections.CorrectionContentVersion));
        Assert.False(CommonCoreG2HundredsPlaceValueSourceCorrections.CanUpgradeExisting(
            row.Document, row.Lesson, "other-version"));

        Assert.Throws<InvalidOperationException>(() =>
            CommonCoreG2HundredsPlaceValueSourceCorrections
                .ValidateExistingBodyBeforeUpgrade(row.Document, row.Lesson,
                    "other-version", []));
        Assert.Throws<InvalidOperationException>(() =>
            CommonCoreG2HundredsPlaceValueSourceCorrections
                .ValidateExistingBodyBeforeUpgrade(row.Document, row.Lesson,
                    CommonCoreRichContentCorrections.CorrectionContentVersion,
                    [new CurriculumLessonContentTranslation {
                        CultureCode = "en", StepByStepSolutions = "Tampered",
                        KeyConceptsAndRules = "Tampered"
                    }]));
    }
}

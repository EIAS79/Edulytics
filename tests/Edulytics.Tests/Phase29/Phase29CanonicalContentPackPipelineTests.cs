using Edulytics.Core.Curriculum;
using Edulytics.Core.Enums;
using Edulytics.Data.Contexts;
using Edulytics.Data.Seeding;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Edulytics.Tests.Phase29;

public sealed class Phase29CanonicalContentPackPipelineTests
{
    [Fact]
    public void PublishedContentRejectsMissingRequiredBodySection()
    {
        var document = ValidDocument();

        document.Lessons[0]
            .Translations[0]
            .Explanation = string.Empty;

        var error =
            Assert.Throws<InvalidOperationException>(
                () =>
                    CanonicalLessonContentPackContract.Validate(
                        document));

        Assert.Contains(
            "Explanation",
            error.Message,
            StringComparison.Ordinal);
    }

    [Fact]
    public void PublishedContentRequiresAcademicLanguageAndReviewEvidence()
    {
        var document = ValidDocument();

        document.Lessons[0].Translations.Clear();

        Assert.Throws<InvalidOperationException>(
            () =>
                CanonicalLessonContentPackContract.Validate(
                    document));

        document = ValidDocument();
        document.ReviewedBy = string.Empty;

        Assert.Throws<InvalidOperationException>(
            () =>
                CanonicalLessonContentPackContract.Validate(
                    document));
    }

    [Fact]
    public void PreviousOfficialFallbackRequiresExplicitReason()
    {
        var document = ValidDocument();

        document.SourceResolution =
            CurriculumSourceResolutionStatus.PreviousOfficialFallback;

        document.SourceCurriculumPeriod = "2025-2026";
        document.TargetCurriculumPeriod = "2026-2027";
        document.FallbackReason = string.Empty;

        var error =
            Assert.Throws<InvalidOperationException>(
                () =>
                    CanonicalLessonContentPackContract.Validate(
                        document));

        Assert.Contains(
            "FallbackReason",
            error.Message,
            StringComparison.Ordinal);
    }

    [Fact]
    public void PreviousOfficialFallbackIsValidWhenFullyTraceable()
    {
        var document = ValidDocument();

        document.SourceResolution =
            CurriculumSourceResolutionStatus.PreviousOfficialFallback;

        document.TargetCurriculumPeriod = "2026-2027";
        document.SourceCurriculumPeriod = "2025-2026";
        document.SourceVersionLabel =
            "Previous official mathematics curriculum";
        document.FallbackReason =
            "The intended newer official source is not yet available or cannot be verified reliably.";

        CanonicalLessonContentPackContract.Validate(
            document);
    }

    [Fact]
    public void CurrentOfficialCannotCarryFallbackReason()
    {
        var document = ValidDocument();

        document.SourceResolution =
            CurriculumSourceResolutionStatus.CurrentOfficial;

        document.FallbackReason =
            "This must not be present.";

        var error =
            Assert.Throws<InvalidOperationException>(
                () =>
                    CanonicalLessonContentPackContract.Validate(
                        document));

        Assert.Contains(
            "FallbackReason",
            error.Message,
            StringComparison.Ordinal);
    }

    [Fact]
    public void PublishedContentRequiresTraceableReviewMethod()
    {
        var document = ValidDocument();

        document.ReviewMethod = string.Empty;

        var error =
            Assert.Throws<InvalidOperationException>(
                () =>
                    CanonicalLessonContentPackContract.Validate(
                        document));

        Assert.Contains(
            "ReviewMethod",
            error.Message,
            StringComparison.Ordinal);
    }

    [Fact]
    public void SourcePolicyV2_RejectsGenericLessonShellTitle()
    {
        var document = ValidDocument();

        document.Lessons[0]
            .Translations
            .Single(x => x.CultureCode == "en")
            .Title =
                "6.EE — Lesson 01";

        var error =
            Assert.Throws<InvalidOperationException>(
                () =>
                    CanonicalLessonContentPackContract.Validate(
                        document));

        Assert.Contains(
            "generic synthetic title",
            error.Message,
            StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void SourcePolicyV2_PublisherFallbackRequiresSelectionEvidence()
    {
        var document = ValidDocument();

        document.PedagogicalSourceType =
            PedagogicalSourceType
                .WidelyUsedPublisherTextbook;

        document.PedagogicalSourceTitle =
            "Publisher mathematics textbook";

        document.PedagogicalSourcePublisher =
            "Publisher";

        document.PedagogicalSourceSelectionReason =
            "No school-adopted or current official textbook is available.";

        document.PedagogicalSourceSelectionEvidence =
            string.Empty;

        var error =
            Assert.Throws<InvalidOperationException>(
                () =>
                    CanonicalLessonContentPackContract.Validate(
                        document));

        Assert.Contains(
            "PedagogicalSourceSelectionEvidence",
            error.Message,
            StringComparison.Ordinal);
    }

    [Fact]
    public void SourcePolicyV2_SchoolTextbookRequiresAdoptionEvidence()
    {
        var document = ValidDocument();

        document.PedagogicalSourceType =
            PedagogicalSourceType
                .SchoolAdoptedTextbook;

        document.PedagogicalSourceSelectionReason =
            "The school selected this textbook for the target year.";

        document.PedagogicalSourceSelectionEvidence =
            string.Empty;

        var error =
            Assert.Throws<InvalidOperationException>(
                () =>
                    CanonicalLessonContentPackContract.Validate(
                        document));

        Assert.Contains(
            "PedagogicalSourceSelectionEvidence",
            error.Message,
            StringComparison.Ordinal);
    }

    [Fact]
    public void SourcePolicyV2_RequiresTraceableLessonTitleOrigin()
    {
        var document = ValidDocument();

        document.Lessons[0]
            .TitleProvenance =
                LessonTitleProvenance.LegacyUnspecified;

        var error =
            Assert.Throws<InvalidOperationException>(
                () =>
                    CanonicalLessonContentPackContract.Validate(
                        document));

        Assert.Contains(
            "TitleProvenance",
            error.Message,
            StringComparison.Ordinal);
    }

    [Fact]
    public async Task SeederUsesExactPedagogicalLessonAndOutcomeAndIsIdempotent()
    {
        await using var db = CreateDb();

        await new MathematicsCurriculumPackSeeder(db)
            .SeedAsync();

        await new MathematicsPedagogicalLessonSeeder(db)
            .SeedAsync();

        var state =
            await db.CurriculumPackImportStates
                .SingleAsync(
                    x =>
                        x.FrameworkCode ==
                        MathematicsCurriculumPackRegistry.CommonCoreCode);

        var fixture =
            await SelectSingleMappedLessonFixtureAsync(
                db,
                state.FrameworkVersionId);

        var document =
            ValidDocument(
                state.VersionCode,
                fixture.LessonCode,
                fixture.OutcomeCode);

        var seeder =
            new MathematicsCanonicalLessonContentSeeder(
                db);

        await seeder.SeedDocumentsAsync([document]);
        await seeder.SeedDocumentsAsync([document]);

        var content =
            Assert.Single(
                await db.CurriculumLessonContents
                    .ToArrayAsync());

        Assert.Equal(
            fixture.LessonId,
            content.PedagogicalLessonId);

        Assert.Equal(
            CanonicalLessonContentStatus.Published,
            content.Status);

        Assert.NotNull(content.VerifiedAtUtc);
        Assert.NotNull(content.PublishedAtUtc);

        var translations =
            await db.CurriculumLessonContentTranslations
                .Where(
                    x =>
                        x.CurriculumLessonContentId ==
                        content.Id)
                .OrderBy(x => x.CultureCode)
                .ToArrayAsync();

        Assert.Equal(
            2,
            translations.Length);

        Assert.Equal(
            new[] { "en", "pl" },
            translations.Select(
                x =>
                    x.CultureCode));
    }

    [Fact]
    public async Task SeederRejectsOutcomeCodeThatDoesNotMatchOfficialAlignment()
    {
        await using var db = CreateDb();

        await new MathematicsCurriculumPackSeeder(db)
            .SeedAsync();

        await new MathematicsPedagogicalLessonSeeder(db)
            .SeedAsync();

        var state =
            await db.CurriculumPackImportStates
                .SingleAsync(
                    x =>
                        x.FrameworkCode ==
                        MathematicsCurriculumPackRegistry.CommonCoreCode);

        var fixture =
            await SelectSingleMappedLessonFixtureAsync(
                db,
                state.FrameworkVersionId);

        var document =
            ValidDocument(
                state.VersionCode,
                fixture.LessonCode,
                "NOT-A-REAL-OFFICIAL-OUTCOME");

        var seeder =
            new MathematicsCanonicalLessonContentSeeder(
                db);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () =>
                seeder.SeedDocumentsAsync(
                    [document]));

        Assert.Empty(
            await db.CurriculumLessonContents
                .ToArrayAsync());
    }

    [Fact]
    public async Task SameVersionLearnerBodyDriftOutsideUaeRebuildRemainsFailClosed()
    {
        await using var db = CreateDb();

        await new MathematicsCurriculumPackSeeder(db)
            .SeedAsync();

        await new MathematicsPedagogicalLessonSeeder(db)
            .SeedAsync();

        var state =
            await db.CurriculumPackImportStates
                .SingleAsync(x =>
                    x.FrameworkCode ==
                        MathematicsCurriculumPackRegistry.CommonCoreCode);

        var fixture =
            await SelectSingleMappedLessonFixtureAsync(
                db,
                state.FrameworkVersionId);

        var document =
            ValidDocument(
                state.VersionCode,
                fixture.LessonCode,
                fixture.OutcomeCode);

        var seeder =
            new MathematicsCanonicalLessonContentSeeder(db);

        await seeder.SeedDocumentsAsync([document]);

        var content =
            await db.CurriculumLessonContents
                .SingleAsync(x =>
                    x.PedagogicalLessonId ==
                        fixture.LessonId);

        var english =
            await db.CurriculumLessonContentTranslations
                .SingleAsync(x =>
                    x.CurriculumLessonContentId == content.Id &&
                    x.CultureCode == "en");

        english.Explanation =
            "unauthorized same-version learner body drift";

        await db.SaveChangesAsync();

        var error =
            await Assert.ThrowsAsync<InvalidOperationException>(
                () =>
                    seeder.SeedDocumentsAsync([document]));

        Assert.Contains(
            "Canonical lesson body drift",
            error.Message,
            StringComparison.Ordinal);
    }

    [Fact]
    public async Task ReviewedProductionCorrectionsRepairStalePersistedLearnerBodyAndCloseParity()
    {
        await using var db = CreateDb();

        await new MathematicsCurriculumPackSeeder(db)
            .SeedAsync();

        await new MathematicsPedagogicalLessonSeeder(db)
            .SeedAsync();

        var seeder =
            new MathematicsCanonicalLessonContentSeeder(db);

        await seeder.SeedAsync();

        const string lessonCode =
            "PED:CAMBRIDGE-INTL-MATH:S6:6AS-MD-1:BUILD";

        var lesson = await db.CurriculumPedagogicalLessons
            .SingleAsync(x => x.Code == lessonCode);

        var content = await db.CurriculumLessonContents
            .SingleAsync(x => x.PedagogicalLessonId == lesson.Id);

        var english = await db.CurriculumLessonContentTranslations
            .SingleAsync(x =>
                x.CurriculumLessonContentId == content.Id &&
                x.CultureCode == "en");

        content.ContentVersion =
            CambridgePrimaryStage6LessonContentCorrections
                .BaseContentVersion;
        english.Explanation =
            "stale learner body";
        english.KeyConceptsAndRules =
            "stale generic rules";

        await db.SaveChangesAsync();

        var mismatchesBefore =
            await seeder
                .FindReviewedProductionParityMismatchesAsync();

        Assert.Contains(
            mismatchesBefore,
            x => x.StartsWith(
                lessonCode + ":",
                StringComparison.Ordinal));

        await seeder
            .SeedApprovedProductionCorrectionsAsync();

        var repairedContent =
            await db.CurriculumLessonContents
                .SingleAsync(x =>
                    x.PedagogicalLessonId == lesson.Id);

        var repairedEnglish =
            await db.CurriculumLessonContentTranslations
                .SingleAsync(x =>
                    x.CurriculumLessonContentId ==
                        repairedContent.Id &&
                    x.CultureCode == "en");

        Assert.Equal(
            "supporting-practice-remediation-v2",
            repairedContent.ContentVersion);

        Assert.Contains(
            "Quantifying a relationship means expressing how quantities are connected by exact operations",
            repairedEnglish.Explanation,
            StringComparison.OrdinalIgnoreCase);

        Assert.Empty(
            await seeder
                .FindReviewedProductionParityMismatchesAsync());
    }

    [Fact]
    public async Task UaeOfficialRebuildCanReplaceSameVersionLegacyLearnerBody()
    {
        await using var db = CreateDb();

        await new MathematicsCurriculumPackSeeder(db)
            .SeedAsync();

        await new MathematicsPedagogicalLessonSeeder(db)
            .SeedAsync();

        var document =
            MathematicsCanonicalLessonContentSeeder
                .LoadEmbeddedDocuments()
                .Single(x =>
                    x.PackCode ==
                        MathematicsCurriculumPackRegistry.UaeCode &&
                    x.Lessons.Any(lesson =>
                        lesson.LessonCode ==
                            "PED:UAE-MOE-MATH:L1:COMMON:01:01"));

        document.Lessons = document.Lessons
            .Where(x =>
                x.LessonCode ==
                    "PED:UAE-MOE-MATH:L1:COMMON:01:01")
            .ToList();

        // This test specifically validates the legacy approved English
        // rebuild policy. The new Arabic-canonical UAE G1 pack intentionally
        // must NOT be treated as freely replaceable same-version English.
        // Simulate only the former English-only official rebuild envelope;
        // never change the real embedded Arabic-canonical document.
        document.AcademicLanguage = "en";
        document.CurriculumTranslationRequired = false;
        foreach (var row in document.Lessons)
            row.Translations = row.Translations
                .Where(x => x.CultureCode == "en").ToList();

        var seeder =
            new MathematicsCanonicalLessonContentSeeder(db);

        await seeder.SeedDocumentsAsync([document]);

        var lesson =
            await db.CurriculumPedagogicalLessons
                .SingleAsync(x =>
                    x.Code ==
                        "PED:UAE-MOE-MATH:L1:COMMON:01:01");

        var content =
            await db.CurriculumLessonContents
                .SingleAsync(x =>
                    x.PedagogicalLessonId == lesson.Id);

        var english =
            await db.CurriculumLessonContentTranslations
                .SingleAsync(x =>
                    x.CurriculumLessonContentId == content.Id &&
                    x.CultureCode == "en");

        var expectedExplanation =
            document.Lessons.Single()
                .Translations.Single(x => x.CultureCode == "en")
                .Explanation;

        english.Explanation =
            "legacy pre-rebuild learner body";

        await db.SaveChangesAsync();

        await seeder.SeedDocumentsAsync([document]);

        var repaired =
            await db.CurriculumLessonContentTranslations
                .SingleAsync(x =>
                    x.CurriculumLessonContentId == content.Id &&
                    x.CultureCode == "en");

        Assert.Equal(
            expectedExplanation,
            repaired.Explanation);
    }

    [Fact]
    public void OwnerRetiredUaeGrade9CannotBeLoadedFromEmbeddedLessonPacks()
    {
        var lessons = MathematicsCanonicalLessonContentSeeder
            .LoadEmbeddedDocuments()
            .Where(x => x.PackCode == MathematicsCurriculumPackRegistry.UaeCode)
            .SelectMany(x => x.Lessons)
            .ToArray();

        Assert.DoesNotContain(lessons, x =>
            x.LessonCode.StartsWith("PED:UAE:G9:", StringComparison.Ordinal) ||
            x.LessonCode.StartsWith("PED:UAE-MOE-MATH:L9:", StringComparison.Ordinal));

        Assert.Contains(lessons, x =>
            x.LessonCode.StartsWith("PED:UAE-MOE-MATH:L8:GENERAL:", StringComparison.Ordinal));
    }

    [Fact]
    public async Task OwnerRetiredUaeGrade9CannotBeReseededByCurriculumPipeline()
    {
        await using var db = CreateDb();
        await new MathematicsCurriculumPackSeeder(db).SeedAsync();
        await new MathematicsPedagogicalLessonSeeder(db).SeedAsync();
        await new MathematicsCanonicalLessonContentSeeder(db).SeedAsync();

        var state = await db.CurriculumPackImportStates
            .SingleAsync(x => x.FrameworkCode == MathematicsCurriculumPackRegistry.UaeCode);

        Assert.Equal(1234, state.NodeCount);
        Assert.Equal(1180, state.OfficialNodeCount);
        Assert.Equal(0, state.LessonCount);
        Assert.Equal(0, state.LinkCount);
        Assert.DoesNotContain(await db.CurriculumPedagogicalLessons
            .Where(x => x.FrameworkVersionId == state.FrameworkVersionId)
            .ToArrayAsync(), x =>
                x.LogicalLevelFrom == 9 && x.LogicalLevelTo == 9);
        Assert.DoesNotContain(await db.CurriculumPackContentNodes
            .Where(x => x.FrameworkVersionId == state.FrameworkVersionId)
            .ToArrayAsync(), x =>
                x.LogicalLevelFrom == 9 && x.LogicalLevelTo == 9 &&
                x.NodeKind is "Lesson" or "Reference" or "Unit" or "SourceCatalog" or "TermScope");
    }

    [Fact]
    public async Task OwnerRetiredUaeGrade9ReconcilesExactProductionImportStateWithoutReseeding()
    {
        await using var db = CreateDb();
        var seeder = new MathematicsCurriculumPackSeeder(db);
        await seeder.SeedAsync();

        var state = await db.CurriculumPackImportStates
            .SingleAsync(x => x.FrameworkCode == MathematicsCurriculumPackRegistry.UaeCode);

        // Production metadata before direct no-backup retirement. The in-memory
        // curriculum nodes have already been reduced to the accepted 1234
        // non-Grade9 set, exactly like the production post-deletion inventory.
        state.NodeCount = 1532;
        state.OfficialNodeCount = 1420;
        state.UnitCount = 6;
        state.LessonCount = 42;
        state.LinkCount = 48;
        state.SourceDigest =
            "470e9bd35d26931e3c4a2e4666b97a35481c3212b72024060c49dd6160bf776f";
        state.ContentDigest =
            "ff6200142b1eea8f12a7d5ff0fd172259582044359c642aeb3c045e5e8886372";
        await db.SaveChangesAsync();

        await seeder.SeedAsync();
        await seeder.SeedAsync();

        Assert.Equal(1234, state.NodeCount);
        Assert.Equal(1180, state.OfficialNodeCount);
        Assert.Equal(0, state.UnitCount);
        Assert.Equal(0, state.LessonCount);
        Assert.Equal(0, state.LinkCount);
        Assert.Equal(1234, await db.CurriculumPackContentNodes
            .CountAsync(x => x.FrameworkVersionId == state.FrameworkVersionId));
        Assert.Empty(await db.CurriculumPackNodeLinks
            .Where(x => x.FrameworkVersionId == state.FrameworkVersionId)
            .ToArrayAsync());
    }

    private static async Task<(
        Guid LessonId,
        string LessonCode,
        string OutcomeCode)>
        SelectSingleMappedLessonFixtureAsync(
            EdulyticsDbContext db,
            Guid frameworkVersionId)
    {
        var mappings =
            await db.CurriculumPedagogicalLessonOutcomes
                .Where(
                    x =>
                        x.FrameworkVersionId ==
                        frameworkVersionId)
                .ToArrayAsync();

        var mappedOnce =
            mappings
                .GroupBy(
                    x =>
                        x.PedagogicalLessonId)
                .Where(
                    group =>
                        group.Count() == 1)
                .Select(
                    group =>
                        group.Key)
                .ToHashSet();

        if (mappedOnce.Count == 0)
        {
            throw new InvalidOperationException(
                "Fixture requires a lesson with exactly one formal outcome mapping.");
        }

        var lessons =
            await db.CurriculumPedagogicalLessons
                .Where(
                    x =>
                        x.FrameworkVersionId ==
                        frameworkVersionId)
                .OrderBy(
                    x =>
                        x.SortOrder)
                .ThenBy(
                    x =>
                        x.Code)
                .ToArrayAsync();

        var lesson =
            lessons.First(
                x =>
                    mappedOnce.Contains(
                        x.Id));

        var mapping =
            mappings.Single(
                x =>
                    x.PedagogicalLessonId ==
                    lesson.Id);

        var outcomeCode =
            await db.CurriculumPackContentNodes
                .Where(
                    x =>
                        x.Id ==
                        mapping.OutcomeNodeId)
                .Select(
                    x =>
                        x.Code)
                .SingleAsync();

        return (
            lesson.Id,
            lesson.Code,
            outcomeCode);
    }

    private static CanonicalLessonContentPackDocument ValidDocument(
        string versionCode = "TEST-VERSION",
        string lessonCode = "PED:TEST:LESSON",
        string outcomeCode = "TEST:OUTCOME") =>
        new()
        {
            PackCode =
                MathematicsCurriculumPackRegistry.CommonCoreCode,

            VersionCode = versionCode,
            ContentVersion = "reviewed-v1",
            AcademicLanguage = "en",
            CurriculumTranslationRequired = false,
            SourcePolicyVersion = 2,

            TargetCurriculumPeriod = "2026-2027",
            SourceCurriculumPeriod = "2026-2027",
            SourceVersionLabel = "Official source fixture",
            SourceAuthority = "Official curriculum authority",
            SourceUrl = "https://example.gov/official-curriculum",
            SourceCheckedAtUtc = "2026-08-29T00:00:00Z",
            SourceResolution =
                CurriculumSourceResolutionStatus.CurrentOfficial,
            FallbackReason = string.Empty,

            PedagogicalSourceType =
                PedagogicalSourceType.CurrentOfficialTextbook,

            PedagogicalSourceTitle =
                "Current official mathematics textbook",

            PedagogicalSourcePublisher =
                "Official curriculum authority",

            PedagogicalSourceEdition =
                "2026-2027",

            PedagogicalSourceUrl =
                "https://example.gov/official-mathematics-textbook",

            PedagogicalSourceCheckedAtUtc =
                "2026-08-29T00:00:00Z",

            PedagogicalSourceSelectionReason =
                "Current official target-year pedagogical material is available.",

            PedagogicalSourceSelectionEvidence =
                string.Empty,

            PedagogicalSourceRightsNote =
                "Reference-only fixture; Edulytics content is independently authored.",

            ReviewMethod =
                "Official-source alignment plus mathematical and pedagogical verification.",

            Status =
                CanonicalLessonContentStatus.Published,

            ReviewedBy =
                "Edulytics curriculum review",

            ReviewEvidence =
                "Product-owner approved reviewed content fixture.",

            Lessons =
            [
                new CanonicalLessonContentPackLesson
                {
                    LessonCode = lessonCode,

                    TitleProvenance =
                        LessonTitleProvenance
                            .EdulyticsDerivedFromOfficialOutcome,

                    TitleSourceReference =
                        outcomeCode,

                    OutcomeCodes =
                    [
                        outcomeCode
                    ],

                    Translations =
                    [
                        Translation(
                            "en",
                            "Reviewed English lesson"),

                        Translation(
                            "pl",
                            "Zweryfikowana lekcja")
                    ]
                }
            ]
        };

    private static CanonicalLessonContentPackTranslation Translation(
        string culture,
        string title) =>
        new()
        {
            CultureCode = culture,
            Title = title,
            Explanation =
                $"Explanation {culture}",
            KeyConceptsAndRules =
                $"Rules {culture}",
            WorkedExamples =
                $"Worked examples {culture}",
            StepByStepSolutions =
                $"Step by step {culture}",
            CommonMistakes =
                $"Common mistakes {culture}",
            QuickSummary =
                $"Summary {culture}"
        };

    private static EdulyticsDbContext CreateDb()
    {
        var options =
            new DbContextOptionsBuilder<
                    EdulyticsDbContext>()
                .UseInMemoryDatabase(
                    "p29-canonical-content-" +
                    Guid.NewGuid())
                .Options;

        return new EdulyticsDbContext(options);
    }
}

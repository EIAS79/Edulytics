using System.Reflection;
using System.Xml.Linq;
using Edulytics.Core.Entities;
using Edulytics.Data.Contexts;
using Edulytics.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Edulytics.Tests.Phase11;

public sealed class ImportModelAndUiTests
{
    [Fact]
    public void ImportBatch_HasConcurrencyAndIdempotencyIndex()
    {
        using var db =
            CreateDb();

        var entity =
            db.Model.FindEntityType(
                typeof(ImportBatch));

        Assert.NotNull(entity);

        var rowVersion =
            entity!.FindProperty(
                nameof(
                    ImportBatch.RowVersion));

        Assert.NotNull(rowVersion);

        Assert.True(
            rowVersion!.IsConcurrencyToken);

        Assert.Contains(
            entity.GetIndexes(),
            x =>
                x.IsUnique &&
                x.Properties
                    .Select(p => p.Name)
                    .SequenceEqual(
                        new[]
                        {
                            nameof(
                                ImportBatch.SchoolId),
                            nameof(
                                ImportBatch.UploadedByUserId),
                            nameof(
                                ImportBatch.ImportType),
                            nameof(
                                ImportBatch.FileHash)
                        }));
    }

    [Fact]
    public void ValidationError_IsMapped()
    {
        using var db =
            CreateDb();

        Assert.NotNull(
            db.Model.FindEntityType(
                typeof(
                    ImportValidationError)));
    }

    [Fact]
    public void Controller_RequiresDataImportPolicy()
    {
        var attribute =
            typeof(ImportsController)
                .GetCustomAttributes<
                    AuthorizeAttribute>()
                .Single();

        Assert.Equal(
            "DataImport",
            attribute.Policy);
    }

    [Fact]
    public void StateChangingImportActions_UseAntiForgery()
    {
        var actions =
            typeof(ImportsController)
                .GetMethods(
                    BindingFlags.Public |
                    BindingFlags.Instance)
                .Where(x =>
                    x.GetCustomAttributes<
                        HttpPostAttribute>()
                        .Any())
                .ToArray();

        Assert.Equal(
            2,
            actions.Length);

        Assert.All(
            actions,
            action =>
                Assert.True(
                    action
                        .GetCustomAttributes<
                            ValidateAntiForgeryTokenAttribute>()
                        .Any()));
    }

    [Fact]
    public void ImportController_HasNoDbContext()
    {
        var root =
            Root();

        var source =
            File.ReadAllText(
                Path.Combine(
                    root,
                    "src/Edulytics.Web/"
                    + "Controllers/"
                    + "ImportsController.cs"));

        Assert.DoesNotContain(
            "EdulyticsDbContext",
            source);

        Assert.DoesNotContain(
            "DbContext",
            source);
    }

    [Fact]
    public void ImportResources_HaveExactEnPlParity()
    {
        var root =
            Root();

        var en =
            Keys(
                Path.Combine(
                    root,
                    "src/Edulytics.Web/"
                    + "Resources/"
                    + "ImportResource.resx"));

        var pl =
            Keys(
                Path.Combine(
                    root,
                    "src/Edulytics.Web/"
                    + "Resources/"
                    + "ImportResource.pl.resx"));

        Assert.Equal(
            en,
            pl);

        Assert.NotEmpty(en);
    }

    [Fact]
    public void ImportUi_HasResponsiveContracts()
    {
        var root =
            Root();

        var css =
            File.ReadAllText(
                Path.Combine(
                    root,
                    "src/Edulytics.Web/"
                    + "wwwroot/css/site.css"));

        Assert.Contains(
            ".import-page",
            css);

        Assert.Contains(
            ".import-table",
            css);

        Assert.Contains(
            "@media (max-width: 767px)",
            css);

        Assert.Contains(
            "@media (max-width: 420px)",
            css);
    }

    [Fact]
    public void PublishedOfflineAssessment_DetailsExposeBothPdfDownloads()
    {
        var root = Root();

        var source = File.ReadAllText(
            Path.Combine(
                root,
                "src/Edulytics.Web/Views/Assessments/Details.cshtml"));

        Assert.Contains(
            "assessment.Status != AssessmentStatus.Draft",
            source);
        Assert.Contains(
            "assessment.DeliveryMode == AssessmentDeliveryMode.Offline",
            source);
        Assert.Contains(
            "asp-action=\"StudentPaperPdf\"",
            source);
        Assert.Contains(
            "asp-action=\"AnswerKeyPdf\"",
            source);
    }

    [Fact]
    public void AssessmentResultsPreview_IsStudentCentricAndKeepsGenericImportsSeparate()
    {
        var root = Root();

        var view = File.ReadAllText(
            Path.Combine(
                root,
                "src/Edulytics.Web/Views/Imports/Details.cshtml"));

        Assert.Contains(
            "var isAssessmentResults = Model.Batch.Type == ImportType.AssessmentResults",
            view);
        Assert.Contains(
            "AssessmentId",
            view);
        Assert.Contains(
            "StudentNumber",
            view);
        Assert.Contains(
            "import-student-details",
            view);
        Assert.Contains(
            "Final score",
            view);
        Assert.Contains(
            "Percentage",
            view);
        Assert.Contains(
            "else",
            view);
        Assert.Contains(
            "data-import-remove-form",
            view);
    }

    [Fact]
    public void AssessmentResultsPreview_MetadataIsHydratedFromCurrentSnapshot()
    {
        var root = Root();

        var source = File.ReadAllText(
            Path.Combine(
                root,
                "src/Edulytics.Services/Imports/DataImportService.cs"));

        Assert.Contains(
            "EnrichAssessmentResultPreview",
            source);
        Assert.Contains(
            "AssessmentMaxScore",
            source);
        Assert.Contains(
            "QuestionPrompt",
            source);
        Assert.Contains(
            "QuestionMaxScore",
            source);
    }

    [Fact]
    public void AssessmentResultsImport_SupportsResultsPagePreselection()
    {
        var root = Root();

        var controller = File.ReadAllText(
            Path.Combine(
                root,
                "src/Edulytics.Web/Controllers/ImportsController.cs"));
        var view = File.ReadAllText(
            Path.Combine(
                root,
                "src/Edulytics.Web/Views/Imports/Index.cshtml"));
        var script = File.ReadAllText(
            Path.Combine(
                root,
                "src/Edulytics.Web/wwwroot/js/import-index.js"));

        Assert.Contains(
            "Guid? assessmentId",
            controller);
        Assert.Contains(
            "selectedAssessmentId",
            controller);
        Assert.Contains(
            "data-selected-assessment-id",
            view);
        Assert.Contains(
            "selectedAssessmentId",
            script);
        Assert.Contains(
            "assessmentSelect.value = preselected.value",
            script);
        Assert.Contains(
            "syncAssessment();",
            script);
    }

    [Fact]
    public void AssessmentResultsPreview_LoadsAuthoritativePaperAnswers()
    {
        var root = Root();

        var controller = File.ReadAllText(
            Path.Combine(
                root,
                "src/Edulytics.Web/Controllers/ImportsController.cs"));
        var view = File.ReadAllText(
            Path.Combine(
                root,
                "src/Edulytics.Web/Views/Imports/Details.cshtml"));

        Assert.Contains(
            "_assessments.GetResultsAsync",
            controller);
        Assert.Contains(
            "assessmentWorkspaces",
            controller);
        Assert.Contains(
            "authoritativeQuestion?.CorrectAnswer",
            view);
        Assert.Contains(
            "new AssessmentPaperViewModel",
            view);
        Assert.Contains(
            "Html.PartialAsync(\"_AssessmentPaper\"",
            view);
    }

    private static string[] Keys(
        string path) =>
        XDocument.Load(path)
            .Root!
            .Elements("data")
            .Select(x =>
                (string?)x.Attribute(
                    "name")
                ?? string.Empty)
            .OrderBy(x => x)
            .ToArray();

    private static EdulyticsDbContext CreateDb()
    {
        var options =
            new DbContextOptionsBuilder<
                EdulyticsDbContext>()
                .UseInMemoryDatabase(
                    $"p11-{Guid.NewGuid():N}")
                .Options;

        return new EdulyticsDbContext(
            options);
    }

    private static string Root()
    {
        var directory =
            new DirectoryInfo(
                AppContext.BaseDirectory);

        while (directory is not null)
        {
            if (File.Exists(
                    Path.Combine(
                        directory.FullName,
                        "Edulytics.sln")))
            {
                return directory.FullName;
            }

            directory =
                directory.Parent;
        }

        throw new DirectoryNotFoundException();
    }
}

using Edulytics.Core.Constants;
using Edulytics.Core.Enums;
using Edulytics.Services.Imports;
using Edulytics.Web.Imports;

namespace Edulytics.Tests.Acceptance;

public sealed class AssessmentResultsWorkflowConvergenceTests
{
    [Fact]
    public void Generic_bulk_import_does_not_offer_assessment_results()
    {
        Assert.False(MathOnlyImportAdapter.IsSupported(ImportType.AssessmentResults));
        Assert.False(DataImportService.CanImportType(RoleNames.Teacher, ImportType.AssessmentResults));
    }

    [Fact]
    public void Generic_template_headers_do_not_reintroduce_assessment_result_csv()
    {
        var options = MathOnlyImportAdapter.FilterOptions(
            [new ImportTypeOption(
                ImportType.AssessmentResults,
                ["AssessmentTitle", "AssessmentDate", "ClassName", "StudentNumber", "QuestionOrder", "Score"])]);

        Assert.Empty(options);
    }
}

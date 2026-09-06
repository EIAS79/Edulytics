namespace Edulytics.Tests.Acceptance;

public sealed class Round3ProductUxAcceptanceTests
{
    [Fact]
    public void Landing_uses_polish_default_and_localized_contact_popover()
    {
        var program = ReadRepositoryFile("src", "Edulytics.Web", "Program.cs");
        var home = ReadRepositoryFile("src", "Edulytics.Web", "Views", "Home", "Index.cshtml");

        Assert.Contains("new RequestCulture(\n                    \"pl\")", program, StringComparison.Ordinal);
        Assert.Contains("isPolish ? \"Kontakt\" : \"Contact us\"", home, StringComparison.Ordinal);
        Assert.Contains("ed-home-contact", home, StringComparison.Ordinal);
        Assert.Contains("ed-home-contact-popover", home, StringComparison.Ordinal);
        Assert.DoesNotContain("public-contact-card", home, StringComparison.Ordinal);
        Assert.Contains("SupportContactOptions.Email", home, StringComparison.Ordinal);
        Assert.Contains("SupportContactOptions.PhoneUri", home, StringComparison.Ordinal);
        Assert.Contains("SupportContactOptions.CompanyWebsiteUrl", home, StringComparison.Ordinal);
        Assert.Contains("SupportContactOptions.ProductWebsiteUrl", home, StringComparison.Ordinal);
    }

    [Fact]
    public void Student_dashboard_subject_tile_opens_learning_without_redundant_open_learning_action()
    {
        var dashboard = ReadRepositoryFile(
            "src", "Edulytics.Web", "Views", "StudentPortal", "Dashboard.cshtml");

        Assert.Contains("student-subject-tile", dashboard, StringComparison.Ordinal);
        Assert.Contains("asp-action=\"Learning\"", dashboard, StringComparison.Ordinal);
        Assert.Contains("asp-route-curriculumAdoptionId", dashboard, StringComparison.Ordinal);
        Assert.Contains("asp-route-classGroupId", dashboard, StringComparison.Ordinal);
        Assert.DoesNotContain("@S[\"OpenLearning\"]", dashboard, StringComparison.Ordinal);
        Assert.DoesNotContain("@S[\"LearningSubjects\"]", dashboard, StringComparison.Ordinal);
    }

    [Fact]
    public void Student_private_practice_limits_are_scope_specific_in_ui_and_backend()
    {
        var view = ReadRepositoryFile(
            "src", "Edulytics.Web", "Views", "StudentPractice", "Index.cshtml");
        var service = ReadRepositoryFile(
            "src", "Edulytics.Services", "Practice", "StudentPrivatePracticeService.cs");

        Assert.Contains("StudentPrivatePracticeScope.Lesson\" data-question-limit=\"10\"", view, StringComparison.Ordinal);
        Assert.Contains("StudentPrivatePracticeScope.Unit\" data-question-limit=\"15\"", view, StringComparison.Ordinal);
        Assert.Contains("StudentPrivatePracticeScope.WeakAreas\" data-question-limit=\"15\"", view, StringComparison.Ordinal);
        Assert.Contains("StudentPrivatePracticeScope.WholeCurriculum\" data-question-limit=\"30\"", view, StringComparison.Ordinal);
        Assert.Contains("count.max = String(limit)", view, StringComparison.Ordinal);

        Assert.Contains("StudentPrivatePracticeScope.Lesson => 10", service, StringComparison.Ordinal);
        Assert.Contains("StudentPrivatePracticeScope.Unit => 15", service, StringComparison.Ordinal);
        Assert.Contains("StudentPrivatePracticeScope.WeakAreas => 15", service, StringComparison.Ordinal);
        Assert.Contains("StudentPrivatePracticeScope.WholeCurriculum => 30", service, StringComparison.Ordinal);
        Assert.Contains("request.QuestionCount > questionLimit", service, StringComparison.Ordinal);
    }

    [Fact]
    public void Teacher_ai_keeps_product_name_caps_generation_and_locks_approved_editing()
    {
        var view = ReadRepositoryFile(
            "src", "Edulytics.Web", "Views", "AssessmentBuilder", "Index.cshtml");
        var controller = ReadRepositoryFile(
            "src", "Edulytics.Web", "Controllers", "AssessmentBuilderController.cs");

        Assert.Contains("@A[\"GenerateQuestionsWithAI\"]", view, StringComparison.Ordinal);
        Assert.Contains("name=\"questionCount\" type=\"number\" min=\"1\" max=\"50\"", view, StringComparison.Ordinal);
        Assert.Contains("MaximumGeneratedQuestionCount = 50", controller, StringComparison.Ordinal);
        Assert.Contains("questionCount is < 1 or > MaximumGeneratedQuestionCount", controller, StringComparison.Ordinal);
        Assert.Contains("if (!isApproved)", view, StringComparison.Ordinal);
        Assert.Contains("RegenerateQuestion", view, StringComparison.Ordinal);
    }

    private static string ReadRepositoryFile(params string[] relativeSegments)
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null &&
               !File.Exists(Path.Combine(directory.FullName, "Edulytics.sln")))
        {
            directory = directory.Parent;
        }

        var root = directory?.FullName
            ?? throw new InvalidOperationException("Repository root not found.");
        return File.ReadAllText(Path.Combine([root, .. relativeSegments]));
    }
}

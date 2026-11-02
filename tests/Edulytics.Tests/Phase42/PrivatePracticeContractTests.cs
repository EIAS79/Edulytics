using Edulytics.Core.Entities;
using Edulytics.Services.Practice;
using Xunit;

namespace Edulytics.Tests.Phase42;

public sealed class PrivatePracticeContractTests
{
    [Fact]
    public void PracticeAttempt_exposes_explicit_private_boundary()
    {
        var property = typeof(PracticeAttempt).GetProperty(nameof(PracticeAttempt.IsPrivate));
        Assert.NotNull(property);
        Assert.Equal(typeof(bool), property!.PropertyType);
    }

    [Fact]
    public void Student_private_practice_supports_required_scopes_and_difficulties()
    {
        Assert.Contains(StudentPrivatePracticeScope.Lesson, Enum.GetValues<StudentPrivatePracticeScope>());
        Assert.Contains(StudentPrivatePracticeScope.Unit, Enum.GetValues<StudentPrivatePracticeScope>());
        Assert.Contains(StudentPrivatePracticeScope.WholeCurriculum, Enum.GetValues<StudentPrivatePracticeScope>());
        Assert.Contains(StudentPrivatePracticeScope.WeakAreas, Enum.GetValues<StudentPrivatePracticeScope>());
        Assert.Contains(StudentPrivatePracticeDifficulty.MyLevel, Enum.GetValues<StudentPrivatePracticeDifficulty>());
    }

    [Fact]
    public void Student_private_generation_has_no_external_ai_dependency_contract()
    {
        var source = File.ReadAllText(FindRepoFile("src/Edulytics.Services/Practice/StudentPrivatePracticeService.cs"));
        Assert.Contains("AssessmentPurpose.StudentPersonalTest", source, StringComparison.Ordinal);
        Assert.Contains("MathematicsQuestionGenerationEngine", source, StringComparison.Ordinal);
        Assert.Contains("IsPrivate = true", source, StringComparison.Ordinal);
        Assert.DoesNotContain("OpenAI", source, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("Gemini", source, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("Claude", source, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("HttpClient", source, StringComparison.Ordinal);
    }

    [Fact]
    public void Official_analytics_filters_out_private_practice_and_evidence()
    {
        var source = File.ReadAllText(FindRepoFile("src/Edulytics.Data/Repositories/AnalyticsRepository.cs"));
        Assert.Contains("!x.IsPrivate", source, StringComparison.Ordinal);
        Assert.Contains("officialPracticeAttemptIds.Contains(x.PracticeAttemptId)", source, StringComparison.Ordinal);
    }

    [Fact]
    public void Student_navigation_exposes_private_practice()
    {
        var source = File.ReadAllText(FindRepoFile("src/Edulytics.Web/Views/Shared/_StudentLayout.cshtml"));
        Assert.Contains("StudentPractice", source, StringComparison.Ordinal);
        Assert.Contains("PrivatePractice", source, StringComparison.Ordinal);
    }

    private static string FindRepoFile(string relativePath)
    {
        var current = new DirectoryInfo(AppContext.BaseDirectory);
        while (current is not null)
        {
            var candidate = Path.Combine(current.FullName, relativePath.Replace('/', Path.DirectorySeparatorChar));
            if (File.Exists(candidate)) return candidate;
            current = current.Parent;
        }
        throw new FileNotFoundException(relativePath);
    }
}

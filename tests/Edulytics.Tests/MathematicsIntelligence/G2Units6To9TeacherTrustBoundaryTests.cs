using System;
using System.IO;
namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class G2Units6To9TeacherTrustBoundaryTests
{
    private static string SourceFile(string path)
    {
        for (var dir = new DirectoryInfo(AppContext.BaseDirectory); dir is not null; dir = dir.Parent)
        {
            var candidate = Path.Combine(dir.FullName, path.Replace('/', Path.DirectorySeparatorChar));
            if (File.Exists(candidate)) return File.ReadAllText(candidate);
        }
        throw new FileNotFoundException("Grade 2 source file missing from test checkout", path);
    }

    [Theory]
    [InlineData(6)]
    [InlineData(7)]
    [InlineData(8)]
    [InlineData(9)]
    public void TeacherApprovalCannotBeProvidedByTheStudentForm(int unit)
    {
        var view = SourceFile($"src/Edulytics.Web/Views/Grade2Unit{unit}Activities/Index.cshtml");
        var controller = SourceFile($"src/Edulytics.Web/Controllers/Grade2Unit{unit}ActivitiesController.cs");
        Assert.DoesNotContain("asp-for=\"Answers.TeacherComments\"", view);
        Assert.Contains("recorded by an authenticated teacher", view);
        Assert.Contains("a.TeacherComments = null;", controller);
    }
}
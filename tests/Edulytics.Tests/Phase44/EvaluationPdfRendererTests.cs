using System.Text;
using Edulytics.Core.Analytics;
using Edulytics.Services.Analytics;
using Edulytics.Web.Printing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace Edulytics.Tests.Phase44;

public sealed class EvaluationPdfRendererTests
{
    [Theory]
    [InlineData(150)]
    [InlineData(300)]
    public void ClassEvaluationReport_PaginatesStudentsBeyondFormerRosterLimit(
        int studentCount)
    {
        var yearId = Guid.NewGuid();
        var classId = Guid.NewGuid();
        var subjectId = Guid.NewGuid();
        var roster = Enumerable.Range(1, studentCount)
            .Select(index => new AnalyticsStudentEvaluationRow(
                Guid.NewGuid(),
                $"ST-{index:000}",
                $"Learner {index:000} Extended family name for wrapping validation",
                70m, 65m, 75m, -10m, 60m, 70m,
                EvaluationConfidenceBand.Strong,
                EvaluationTrendBand.Stable,
                EvaluationTrendBand.Stable,
                3, 2, 1, 0, EvaluationPriority.None))
            .ToArray();
        var page = new AnalyticsStudentsEvaluationPage(
            yearId, "2026/2027", classId, "Large cohort", subjectId,
            "Mathematics",
            new AnalyticsEvaluationDistribution(
                studentCount, 0, studentCount, 0, 0, 0, 0, studentCount, 0),
            roster);
        var topics = new AnalyticsTopicSkillEvaluationPage(
            yearId, page.AcademicYearName, classId, page.ClassName,
            subjectId, page.SubjectName, []);

        var first120 = AnalyticsPdfRenderer.RenderClassEvaluationReport(
            page with { Students = roster.Take(120).ToArray() }, topics);
        var complete = AnalyticsPdfRenderer.RenderClassEvaluationReport(
            page, topics);

        using var first120Stream = new MemoryStream(first120);
        using var completeStream = new MemoryStream(complete);
        using var first120Pdf = PdfReader.Open(
            first120Stream, PdfDocumentOpenMode.Import);
        using var completePdf = PdfReader.Open(
            completeStream, PdfDocumentOpenMode.Import);
        Assert.True(completePdf.PageCount > first120Pdf.PageCount,
            "The complete roster must continue onto additional pages after student 120.");
    }

    [Fact]
    public void StudentEvaluationReport_RendersPdfFromEvaluationModel()
    {
        var evaluation = new StudentSubjectEvaluation(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "ST-001",
            "Student One",
            Guid.NewGuid(),
            "2026/2027",
            Guid.NewGuid(),
            "4A",
            Guid.NewGuid(),
            "Mathematics",
            64m,
            58m,
            73m,
            -15m,
            72m,
            10,
            7,
            80m,
            EvaluationConfidenceBand.Strong,
            EvaluationTrendBand.Improving,
            EvaluationTrendBand.Improving,
            4,
            2,
            1,
            0,
            0,
            [],
            "evaluation-v1");

        var page = new AnalyticsStudentEvaluationPage(
            evaluation,
            [],
            [],
            [],
            new AnalyticsPracticeEvaluationSummary(
                0,
                0,
                0,
                0,
                null,
                EvaluationTrendBand.InsufficientEvidence,
                null),
            [],
            []);

        var bytes =
            AnalyticsPdfRenderer.RenderStudentEvaluationReport(page);

        Assert.True(bytes.Length > 1000);
        Assert.Equal(
            "%PDF",
            Encoding.ASCII.GetString(bytes, 0, 4));
    }

    [Fact]
    public void ClassEvaluationReport_RendersAllStudentsModel()
    {
        var yearId = Guid.NewGuid();
        var classId = Guid.NewGuid();
        var subjectId = Guid.NewGuid();

        var students = new AnalyticsStudentsEvaluationPage(
            yearId,
            "2026/2027",
            classId,
            "4A",
            subjectId,
            "Mathematics",
            new AnalyticsEvaluationDistribution(
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0,
                0),
            []);

        var topics = new AnalyticsTopicSkillEvaluationPage(
            yearId,
            "2026/2027",
            classId,
            "4A",
            subjectId,
            "Mathematics",
            []);

        var bytes =
            AnalyticsPdfRenderer.RenderClassEvaluationReport(
                students,
                topics);

        Assert.True(bytes.Length > 1000);
        Assert.Equal(
            "%PDF",
            Encoding.ASCII.GetString(bytes, 0, 4));
    }
}

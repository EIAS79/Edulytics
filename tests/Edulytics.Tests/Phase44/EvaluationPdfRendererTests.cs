using System.Text;
using Edulytics.Core.Analytics;
using Edulytics.Services.Analytics;
using Edulytics.Web.Printing;

namespace Edulytics.Tests.Phase44;

public sealed class EvaluationPdfRendererTests
{
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

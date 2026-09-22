using System.Globalization;
using Edulytics.Services.Analytics;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;

namespace Edulytics.Web.Printing;

public static class AnalyticsPdfRenderer
{
    public static byte[] RenderClassReport(AnalyticsDashboard dashboard)
    {
        ArgumentNullException.ThrowIfNull(dashboard);

        var document = CreateDocument("Edulytics class analytics report");
        var section = document.AddSection();
        ConfigurePage(section);
        AddTitle(section, "Class analytics report");

        AddContext(
            section,
            dashboard.SelectedAcademicYearId.HasValue
                ? dashboard.AcademicYears.FirstOrDefault(x => x.Id == dashboard.SelectedAcademicYearId)?.Name
                : "All visible academic years",
            dashboard.SelectedClassGroupId.HasValue
                ? dashboard.ClassGroups.FirstOrDefault(x => x.Id == dashboard.SelectedClassGroupId)?.Name
                : "All visible classes",
            dashboard.SelectedSubjectId.HasValue
                ? dashboard.Subjects.FirstOrDefault(x => x.Id == dashboard.SelectedSubjectId)?.Name
                : "All visible subjects");

        var metrics = section.AddParagraph();
        metrics.Format.SpaceAfter = Unit.FromPoint(10);
        metrics.AddFormattedText("Official outcome mastery: ", TextFormat.Bold);
        metrics.AddText(Percent(dashboard.OverallMasteryPercentage));
        metrics.AddText("    ");
        metrics.AddFormattedText("Weak lessons: ", TextFormat.Bold);
        metrics.AddText(dashboard.WeakLessonCount.ToString(CultureInfo.InvariantCulture));
        metrics.AddText("    ");
        metrics.AddFormattedText("Critical outcomes: ", TextFormat.Bold);
        metrics.AddText(dashboard.CriticalOutcomeCount.ToString(CultureInfo.InvariantCulture));

        AddLessonTable(section, dashboard.Lessons);
        AddOutcomeTable(section, dashboard.Outcomes);
        AddRiskTable(section, dashboard.RiskStudents);

        return Render(document);
    }

    public static byte[] RenderStudentReport(AnalyticsStudentReport report)
    {
        ArgumentNullException.ThrowIfNull(report);

        var document = CreateDocument($"Edulytics student analytics report - {report.DisplayName}");
        var section = document.AddSection();
        ConfigurePage(section);
        AddTitle(section, "Student analytics report");

        var student = section.AddParagraph();
        student.Format.SpaceAfter = Unit.FromPoint(4);
        student.AddFormattedText("Student: ", TextFormat.Bold);
        student.AddText($"{report.DisplayName} ({report.StudentNumber})");

        AddContext(
            section,
            report.AcademicYearName,
            report.ClassName,
            report.SubjectName);

        var metrics = section.AddParagraph();
        metrics.Format.SpaceAfter = Unit.FromPoint(10);
        metrics.AddFormattedText("Official outcome mastery: ", TextFormat.Bold);
        metrics.AddText(report.OfficialOutcomeMasteryPercentage.HasValue
            ? Percent(report.OfficialOutcomeMasteryPercentage.Value)
            : "No official outcome evidence");
        metrics.AddText("    ");
        metrics.AddFormattedText("Assessment lesson mastery: ", TextFormat.Bold);
        metrics.AddText(report.AssessmentLessonMasteryPercentage.HasValue
            ? Percent(report.AssessmentLessonMasteryPercentage.Value)
            : "Lesson mastery is unavailable because scored assessment questions are not linked to specific lessons.");

        AddStudentLessonTable(section, report.Lessons);
        AddStudentOutcomeTable(section, report.Outcomes);

        return Render(document);
    }

    private static Document CreateDocument(string title)
    {
        AssessmentPdfFontBootstrap.EnsureConfigured();
        var document = new Document();
        document.Info.Title = title;
        var normal = document.Styles[StyleNames.Normal]!;
        normal.Font.Name = EdulyticsPdfFontResolver.FamilyName;
        normal.Font.Size = Unit.FromPoint(9);
        return document;
    }

    private static void ConfigurePage(Section section)
    {
        section.PageSetup.PageFormat = PageFormat.A4;
        section.PageSetup.Orientation = Orientation.Landscape;
        section.PageSetup.TopMargin = Unit.FromCentimeter(1.2);
        section.PageSetup.BottomMargin = Unit.FromCentimeter(1.2);
        section.PageSetup.LeftMargin = Unit.FromCentimeter(1.2);
        section.PageSetup.RightMargin = Unit.FromCentimeter(1.2);
    }

    private static void AddTitle(Section section, string title)
    {
        var heading = section.AddParagraph();
        heading.Format.Font.Bold = true;
        heading.Format.Font.Size = Unit.FromPoint(16);
        heading.Format.SpaceAfter = Unit.FromPoint(6);
        heading.AddText(title);
    }

    private static void AddContext(
        Section section,
        string? academicYear,
        string? className,
        string? subjectName)
    {
        var context = section.AddParagraph();
        context.Format.SpaceAfter = Unit.FromPoint(8);
        context.AddFormattedText("Academic year: ", TextFormat.Bold);
        context.AddText(academicYear ?? "—");
        context.AddText("    ");
        context.AddFormattedText("Class: ", TextFormat.Bold);
        context.AddText(className ?? "—");
        context.AddText("    ");
        context.AddFormattedText("Subject: ", TextFormat.Bold);
        context.AddText(subjectName ?? "—");
    }

    private static void AddLessonTable(
        Section section,
        IReadOnlyList<AnalyticsLessonItem> lessons)
    {
        AddSectionHeading(section, "Lesson mastery (explicit lesson provenance)");
        if (lessons.Count == 0)
        {
            AddEmpty(section, "Lesson mastery is unavailable because scored assessment questions are not linked to specific lessons.");
            return;
        }

        var table = BaseTable(section, 6);
        Header(table, "Lesson", "Unit", "Class mastery", "Students", "At risk", "Evidence");
        foreach (var item in lessons.Take(100))
        {
            Row(
                table,
                item.LessonTitle,
                item.UnitTitle,
                Percent(item.MasteryPercentage),
                item.StudentCount.ToString(CultureInfo.InvariantCulture),
                item.AtRiskStudentCount.ToString(CultureInfo.InvariantCulture),
                item.EvidenceCount.ToString(CultureInfo.InvariantCulture));
        }
    }

    private static void AddOutcomeTable(
        Section section,
        IReadOnlyList<AnalyticsOutcomeItem> outcomes)
    {
        AddSectionHeading(section, "Official outcome mastery");
        if (outcomes.Count == 0)
        {
            AddEmpty(section, "No official outcome mastery evidence.");
            return;
        }

        var table = BaseTable(section, 5);
        Header(table, "Outcome", "Description", "Mastery", "Students", "Evidence");
        foreach (var item in outcomes.OrderBy(x => x.MasteryPercentage).Take(100))
        {
            Row(
                table,
                item.OutcomeCode,
                item.OutcomeDescription,
                Percent(item.MasteryPercentage),
                item.StudentCount.ToString(CultureInfo.InvariantCulture),
                item.EvidenceCount.ToString(CultureInfo.InvariantCulture));
        }
    }

    private static void AddRiskTable(
        Section section,
        IReadOnlyList<AnalyticsRiskStudentItem> students)
    {
        AddSectionHeading(section, "Students currently at risk");
        if (students.Count == 0)
        {
            AddEmpty(section, "No at-risk students in the selected analytics scope.");
            return;
        }

        var table = BaseTable(section, 4);
        Header(table, "Student", "Class", "Mastery", "Critical outcomes");
        foreach (var item in students.Take(100))
        {
            Row(
                table,
                $"{item.DisplayName} ({item.StudentNumber})",
                item.ClassName,
                Percent(item.MasteryPercentage),
                item.CriticalOutcomeCount.ToString(CultureInfo.InvariantCulture));
        }
    }

    private static void AddStudentLessonTable(
        Section section,
        IReadOnlyList<AnalyticsStudentLessonItem> lessons)
    {
        AddSectionHeading(section, "Lesson assessment mastery");
        if (lessons.Count == 0)
        {
            AddEmpty(section, "Lesson mastery is unavailable because scored assessment questions are not linked to specific lessons for this student.");
            return;
        }

        var table = BaseTable(section, 5);
        Header(table, "Lesson", "Unit", "Mastery", "Evidence", "Assessments");
        foreach (var item in lessons)
        {
            Row(
                table,
                item.LessonTitle,
                item.UnitTitle,
                Percent(item.MasteryPercentage),
                item.EvidenceCount.ToString(CultureInfo.InvariantCulture),
                item.AssessmentCount.ToString(CultureInfo.InvariantCulture));
        }
    }

    private static void AddStudentOutcomeTable(
        Section section,
        IReadOnlyList<AnalyticsStudentOutcomeItem> outcomes)
    {
        AddSectionHeading(section, "Official outcome mastery");
        if (outcomes.Count == 0)
        {
            AddEmpty(section, "No official outcome mastery evidence for this student.");
            return;
        }

        var table = BaseTable(section, 4);
        Header(table, "Outcome", "Description", "Mastery", "Evidence");
        foreach (var item in outcomes)
        {
            Row(
                table,
                item.OutcomeCode,
                item.OutcomeDescription,
                Percent(item.MasteryPercentage),
                item.EvidenceCount.ToString(CultureInfo.InvariantCulture));
        }
    }

    private static void AddSectionHeading(Section section, string title)
    {
        var heading = section.AddParagraph();
        heading.Format.Font.Bold = true;
        heading.Format.Font.Size = Unit.FromPoint(11);
        heading.Format.SpaceBefore = Unit.FromPoint(10);
        heading.Format.SpaceAfter = Unit.FromPoint(4);
        heading.AddText(title);
    }

    private static void AddEmpty(Section section, string text)
    {
        var paragraph = section.AddParagraph(text);
        paragraph.Format.SpaceAfter = Unit.FromPoint(5);
    }

    private static Table BaseTable(Section section, int columns)
    {
        var table = section.AddTable();
        table.Borders.Width = Unit.FromPoint(0.5);
        table.Format.Font.Size = Unit.FromPoint(8);
        var width = Unit.FromCentimeter(25.0 / columns);
        for (var i = 0; i < columns; i++)
            table.AddColumn(width);
        return table;
    }

    private static void Header(Table table, params string[] values)
    {
        var row = table.AddRow();
        row.Format.Font.Bold = true;
        for (var i = 0; i < values.Length; i++)
            row.Cells[i].AddParagraph(values[i]);
    }

    private static void Row(Table table, params string[] values)
    {
        var row = table.AddRow();
        for (var i = 0; i < values.Length; i++)
            row.Cells[i].AddParagraph(values[i] ?? string.Empty);
    }

    private static string Percent(decimal value) =>
        value.ToString("0.##", CultureInfo.InvariantCulture) + "%";

    private static byte[] Render(Document document)
    {
        var renderer = new PdfDocumentRenderer { Document = document };
        renderer.RenderDocument();
        using var stream = new MemoryStream();
        renderer.PdfDocument.Save(stream, false);
        return stream.ToArray();
    }
}

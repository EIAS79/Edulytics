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

    public static byte[] RenderStudentEvaluationReport(
        AnalyticsStudentEvaluationPage page)
    {
        ArgumentNullException.ThrowIfNull(page);

        var report = page.Evaluation;
        var document = CreateDocument(
            $"Edulytics student evaluation report - {report.DisplayName}");
        var section = document.AddSection();
        ConfigurePage(section);
        AddTitle(section, "Student evaluation report");

        var student = section.AddParagraph();
        student.Format.SpaceAfter = Unit.FromPoint(4);
        student.AddFormattedText("Student: ", TextFormat.Bold);
        student.AddText(
            $"{report.DisplayName} ({report.StudentNumber})");

        AddContext(
            section,
            report.AcademicYearName,
            report.ClassName,
            report.SubjectName);

        AddSectionHeading(section, "Evaluation summary");
        var summary = BaseTable(section, 2);
        Header(summary, "Metric", "Value");
        Row(summary, "Current mastery", NullablePercent(report.CurrentMasteryPercentage));
        Row(summary, "Assessment mastery", NullablePercent(report.AssessmentMasteryPercentage));
        Row(summary, "Practice mastery", NullablePercent(report.PracticeMasteryPercentage));
        Row(
            summary,
            "Practice -> Assessment gap",
            Points(report.PracticeToAssessmentGapPercentagePoints));
        Row(
            summary,
            "Curriculum coverage",
            Percent(report.CurriculumCoveragePercentage));
        Row(
            summary,
            "Evidence confidence",
            $"{Percent(report.ConfidencePercentage)} ({report.ConfidenceBand})");
        Row(summary, "Recent trend", Trend(report.ShortTermTrend));
        Row(summary, "Long-term trend", Trend(report.LongTermTrend));
        Row(summary, "Secure skills", report.SecureSkillCount.ToString(CultureInfo.InvariantCulture));
        Row(summary, "Needs-focus skills", report.NeedsFocusSkillCount.ToString(CultureInfo.InvariantCulture));
        Row(summary, "Critical skills", report.CriticalSkillCount.ToString(CultureInfo.InvariantCulture));
        Row(summary, "Retention concerns", report.RetentionConcernCount.ToString(CultureInfo.InvariantCulture));

        var note = section.AddParagraph();
        note.Format.SpaceBefore = Unit.FromPoint(5);
        note.Format.SpaceAfter = Unit.FromPoint(5);
        note.AddText(
            "No evidence is never treated as 0% mastery. Assessment and Practice are shown separately. "
            + "Private student Practice is not included in this staff-facing report.");

        AddSectionHeading(section, "Skill evaluation");
        if (report.Skills.Count == 0)
        {
            AddEmpty(section, "No skill evaluation is available.");
        }
        else
        {
            var skills = BaseTable(section, 8);
            Header(
                skills,
                "Skill",
                "Current",
                "Assessment",
                "Practice",
                "Status",
                "Trend",
                "Evidence",
                "Confidence");

            foreach (var item in report.Skills
                         .OrderByDescending(x => x.InterventionPriority)
                         .ThenBy(x => x.CurrentMasteryPercentage ?? 101m)
                         .Take(80))
            {
                Row(
                    skills,
                    item.SkillName,
                    NullablePercent(item.CurrentMasteryPercentage),
                    NullablePercent(item.AssessmentMasteryPercentage),
                    NullablePercent(item.PracticeMasteryPercentage),
                    item.Status.ToString(),
                    Trend(item.ShortTermTrend),
                    item.Evidence.TotalEvidence.ToString(CultureInfo.InvariantCulture),
                    $"{Percent(item.ConfidencePercentage)} {item.ConfidenceBand}");
            }
        }

        AddSectionHeading(section, "Assessment progress");
        if (page.Assessments.Count == 0)
        {
            AddEmpty(section, "No assessment history is available.");
        }
        else
        {
            var assessments = BaseTable(section, 6);
            Header(
                assessments,
                "Date",
                "Assessment",
                "Term",
                "Score",
                "Overall change",
                "Comparable-skill change");

            foreach (var item in page.Assessments)
            {
                Row(
                    assessments,
                    item.AssessmentDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    item.Title,
                    item.TermName ?? "—",
                    Percent(item.Percentage),
                    Points(item.OverallChangePercentagePoints),
                    item.ComparableSkillChangePercentagePoints.HasValue
                        ? $"{Points(item.ComparableSkillChangePercentagePoints)} ({item.ComparableSkillCount} skills)"
                        : "—");
            }
        }

        AddSectionHeading(section, "Term progress");
        if (page.Terms.Count == 0)
        {
            AddEmpty(section, "No term comparison is available.");
        }
        else
        {
            var terms = BaseTable(section, 6);
            Header(
                terms,
                "Term",
                "Assessment",
                "Practice",
                "Combined",
                "Comparable growth",
                "Evidence");

            foreach (var item in page.Terms)
            {
                Row(
                    terms,
                    item.TermName,
                    NullablePercent(item.AssessmentMasteryPercentage),
                    NullablePercent(item.PracticeMasteryPercentage),
                    NullablePercent(item.CombinedMasteryPercentage),
                    item.ComparableSkillGrowthPercentagePoints.HasValue
                        ? $"{Points(item.ComparableSkillGrowthPercentagePoints)} ({item.ComparableSkillCount} skills)"
                        : "—",
                    item.EvidenceCount.ToString(CultureInfo.InvariantCulture));
            }
        }

        AddSectionHeading(section, "Recommended next steps");
        if (page.Recommendations.Count == 0)
        {
            AddEmpty(
                section,
                "No intervention priority is currently supported by the available evidence.");
        }
        else
        {
            var recommendations = BaseTable(section, 4);
            Header(
                recommendations,
                "Priority",
                "Skill",
                "Current mastery",
                "Reason");

            foreach (var item in page.Recommendations.Take(10))
            {
                var path = item.RecommendedPrerequisitePath.Count == 0
                    ? string.Empty
                    : " Prerequisite path: "
                      + string.Join(
                          " -> ",
                          item.RecommendedPrerequisitePath)
                      + " -> "
                      + item.SkillKey;

                Row(
                    recommendations,
                    item.Priority.ToString(),
                    item.SkillName,
                    NullablePercent(item.CurrentMasteryPercentage),
                    item.Reason + path);
            }
        }

        AddSectionHeading(section, "Evidence sample");
        if (page.Evidence.Count == 0)
        {
            AddEmpty(section, "No evidence records are available.");
        }
        else
        {
            var evidence = BaseTable(section, 6);
            Header(
                evidence,
                "When",
                "Source",
                "Skill",
                "Difficulty",
                "Result",
                "Outcome");

            foreach (var item in page.Evidence.Take(40))
            {
                Row(
                    evidence,
                    item.OccurredAtUtc.ToString(
                        "yyyy-MM-dd",
                        CultureInfo.InvariantCulture),
                    item.Source.ToString(),
                    item.SkillName,
                    item.Difficulty?.ToString() ?? "—",
                    Percent(item.Percentage),
                    item.OutcomeCode);
            }
        }

        return Render(document);
    }

    public static byte[] RenderClassEvaluationReport(
        AnalyticsStudentsEvaluationPage studentsPage,
        AnalyticsTopicSkillEvaluationPage topicPage)
    {
        ArgumentNullException.ThrowIfNull(studentsPage);
        ArgumentNullException.ThrowIfNull(topicPage);

        var document = CreateDocument("Edulytics class evaluation report");
        var section = document.AddSection();
        ConfigurePage(section);
        AddTitle(section, "Class evaluation report");

        AddContext(
            section,
            studentsPage.AcademicYearName,
            studentsPage.ClassName,
            studentsPage.SubjectName);

        var distribution = studentsPage.Distribution;

        AddSectionHeading(section, "Class evaluation summary");
        var summary = BaseTable(section, 2);
        Header(summary, "Metric", "Value");
        Row(summary, "Students", distribution.TotalStudents.ToString(CultureInfo.InvariantCulture));
        Row(summary, "Strong / secure", distribution.StrongOrSecure.ToString(CultureInfo.InvariantCulture));
        Row(summary, "Developing", distribution.Developing.ToString(CultureInfo.InvariantCulture));
        Row(summary, "Needs focus", distribution.NeedsFocus.ToString(CultureInfo.InvariantCulture));
        Row(summary, "Critical", distribution.Critical.ToString(CultureInfo.InvariantCulture));
        Row(summary, "Insufficient evidence", distribution.InsufficientEvidence.ToString(CultureInfo.InvariantCulture));
        Row(summary, "Improving", distribution.Improving.ToString(CultureInfo.InvariantCulture));
        Row(summary, "Stable", distribution.Stable.ToString(CultureInfo.InvariantCulture));
        Row(summary, "Declining", distribution.Declining.ToString(CultureInfo.InvariantCulture));

        AddSectionHeading(section, "All students");
        if (studentsPage.Students.Count == 0)
        {
            AddEmpty(section, "No students are available in this class.");
        }
        else
        {
            var students = BaseTable(section, 8);
            Header(
                students,
                "Student",
                "Current",
                "Assessment",
                "Practice",
                "Coverage",
                "Trend",
                "Critical gaps",
                "Confidence");

            foreach (var item in studentsPage.Students.Take(120))
            {
                Row(
                    students,
                    $"{item.DisplayName} ({item.StudentNumber})",
                    NullablePercent(item.CurrentMasteryPercentage),
                    NullablePercent(item.AssessmentMasteryPercentage),
                    NullablePercent(item.PracticeMasteryPercentage),
                    Percent(item.CoveragePercentage),
                    Trend(item.ShortTermTrend),
                    item.CriticalSkillCount.ToString(CultureInfo.InvariantCulture),
                    $"{Percent(item.ConfidencePercentage)} {item.ConfidenceBand}");
            }
        }

        AddSectionHeading(section, "Priority topic and skill gaps");
        var skillRows = topicPage.Topics
            .SelectMany(topic =>
                topic.Skills.Select(skill =>
                    (
                        Topic: topic.TopicName,
                        Skill: skill
                    )))
            .Where(x =>
                x.Skill.CriticalStudentCount > 0 ||
                x.Skill.NeedsFocusStudentCount > 0)
            .OrderByDescending(x => x.Skill.HighestPriority)
            .ThenByDescending(x =>
                x.Skill.CriticalStudentCount +
                x.Skill.NeedsFocusStudentCount)
            .ThenBy(x => x.Skill.ClassMasteryPercentage ?? 101m)
            .Take(60)
            .ToArray();

        if (skillRows.Length == 0)
        {
            AddEmpty(
                section,
                "No priority skill gaps are currently supported by the available evidence.");
        }
        else
        {
            var gaps = BaseTable(section, 7);
            Header(
                gaps,
                "Topic",
                "Skill",
                "Class mastery",
                "Evaluated",
                "Critical",
                "Needs focus",
                "Priority");

            foreach (var item in skillRows)
            {
                Row(
                    gaps,
                    item.Topic,
                    item.Skill.SkillName,
                    NullablePercent(item.Skill.ClassMasteryPercentage),
                    item.Skill.EvaluatedStudentCount.ToString(CultureInfo.InvariantCulture),
                    item.Skill.CriticalStudentCount.ToString(CultureInfo.InvariantCulture),
                    item.Skill.NeedsFocusStudentCount.ToString(CultureInfo.InvariantCulture),
                    item.Skill.HighestPriority.ToString());
            }
        }

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

    private static string NullablePercent(decimal? value) =>
        value.HasValue
            ? Percent(value.Value)
            : "—";

    private static string Points(decimal? value) =>
        value.HasValue
            ? value.Value.ToString(
                "+0.##;-0.##;0",
                CultureInfo.InvariantCulture) + " pts"
            : "—";

    private static string Trend(
        Edulytics.Core.Analytics.EvaluationTrendBand value) =>
        value switch
        {
            Edulytics.Core.Analytics.EvaluationTrendBand.RapidlyImproving =>
                "Rapidly improving",
            Edulytics.Core.Analytics.EvaluationTrendBand.Improving =>
                "Improving",
            Edulytics.Core.Analytics.EvaluationTrendBand.Stable =>
                "Stable",
            Edulytics.Core.Analytics.EvaluationTrendBand.Declining =>
                "Declining",
            Edulytics.Core.Analytics.EvaluationTrendBand.RapidlyDeclining =>
                "Rapidly declining",
            _ =>
                "Insufficient evidence"
        };

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

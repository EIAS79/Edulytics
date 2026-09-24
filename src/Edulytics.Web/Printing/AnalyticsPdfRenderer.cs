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

    public static byte[] RenderStudentEvaluationReport(
        AnalyticsStudentEvaluationReportPage page)
    {
        ArgumentNullException.ThrowIfNull(page);

        var source = page.Source;
        var report = source.Evaluation;
        var document = CreateDocument(
            $"Edulytics student evaluation report - {report.DisplayName}");
        var section = document.AddSection();

        ConfigureStudentReportPage(section);
        AddReportBrandHeader(
            section,
            "Student evaluation report",
            $"{report.DisplayName} · {report.StudentNumber}");
        AddReportContext(
            section,
            report.AcademicYearName,
            report.ClassName,
            report.SubjectName,
            page.SelectedTermName ?? "Whole academic year",
            page.GeneratedAtUtc);

        if (page.SelectedTerm is not null)
        {
            AddReportSectionHeading(section, $"Term focus · {page.SelectedTerm.TermName}");
            var termTable = ReportTable(section, 4);
            ReportHeader(
                termTable,
                "Assessment",
                "Practice",
                "Comparable growth",
                "Evidence");
            ReportRow(
                termTable,
                NullablePercent(page.SelectedTerm.AssessmentMasteryPercentage),
                NullablePercent(page.SelectedTerm.PracticeMasteryPercentage),
                Points(page.SelectedTerm.ComparableSkillGrowthPercentagePoints),
                page.SelectedTerm.EvidenceCount.ToString(CultureInfo.InvariantCulture));
        }

        AddReportSectionHeading(section, "Performance overview");
        AddReportMetricBar(
            section,
            "Current mastery",
            report.CurrentMasteryPercentage,
            Color.FromRgb(82, 98, 227));
        AddReportMetricBar(
            section,
            "Assessment mastery",
            report.AssessmentMasteryPercentage,
            Color.FromRgb(62, 96, 210));
        AddReportMetricBar(
            section,
            "Practice mastery",
            report.PracticeMasteryPercentage,
            Color.FromRgb(42, 169, 119));
        AddReportMetricBar(
            section,
            "Curriculum coverage",
            report.CurriculumCoveragePercentage,
            Color.FromRgb(233, 162, 61));

        var kpis = ReportTable(section, 4);
        ReportHeader(
            kpis,
            "Evidence confidence",
            "Recent trend",
            "Critical gaps",
            "Retention concerns");
        ReportRow(
            kpis,
            $"{Percent(report.ConfidencePercentage)} · {report.ConfidenceBand}",
            Trend(report.ShortTermTrend),
            report.CriticalSkillCount.ToString(CultureInfo.InvariantCulture),
            report.RetentionConcernCount.ToString(CultureInfo.InvariantCulture));

        var note = section.AddParagraph();
        note.Format.SpaceBefore = Unit.FromPoint(5);
        note.Format.SpaceAfter = Unit.FromPoint(8);
        note.Format.Font.Color = Color.FromRgb(93, 105, 130);
        note.AddText(
            "Assessment and Practice remain separate. Private student Practice is not included in this staff-facing report. "
            + "Missing evidence is never converted to 0% mastery. "
            + (page.SelectedTermId.HasValue
                ? "The selected term filters assessment/evidence history and highlights term metrics; current mastery remains the latest year-level evaluation."
                : string.Empty));

        AddReportSectionHeading(section, "Strengths and areas to strengthen");
        var split = ReportTable(section, 2);
        var splitHeader = split.AddRow();
        splitHeader.Format.Font.Bold = true;
        splitHeader.Cells[0].Shading.Color = Color.FromRgb(232, 248, 240);
        splitHeader.Cells[1].Shading.Color = Color.FromRgb(255, 244, 225);
        splitHeader.Cells[0].AddParagraph("Strengths");
        splitHeader.Cells[1].AddParagraph("Needs attention");

        var splitRow = split.AddRow();
        var strengths = report.Skills
            .Where(x =>
                x.Status is
                    Edulytics.Core.Analytics.EvaluationSkillStatus.Strong or
                    Edulytics.Core.Analytics.EvaluationSkillStatus.Secure)
            .OrderByDescending(x => x.CurrentMasteryPercentage ?? 0m)
            .Take(8)
            .ToArray();
        var focus = report.Skills
            .Where(x =>
                x.Status is
                    Edulytics.Core.Analytics.EvaluationSkillStatus.Critical or
                    Edulytics.Core.Analytics.EvaluationSkillStatus.NeedsFocus)
            .OrderByDescending(x => x.InterventionPriority)
            .ThenBy(x => x.CurrentMasteryPercentage ?? 101m)
            .Take(8)
            .ToArray();

        splitRow.Cells[0].AddParagraph(
            strengths.Length == 0
                ? "No secure skill is supported by enough evidence yet."
                : string.Join(
                    "\n",
                    strengths.Select(x =>
                        $"• {x.SkillName} · {NullablePercent(x.CurrentMasteryPercentage)}")));
        splitRow.Cells[1].AddParagraph(
            focus.Length == 0
                ? "No priority gap is currently supported by the available evidence."
                : string.Join(
                    "\n",
                    focus.Select(x =>
                        $"• {x.SkillName} · {NullablePercent(x.CurrentMasteryPercentage)}")));

        AddReportSectionHeading(section, "Topic mastery");
        if (source.Topics.Count == 0)
        {
            AddEmpty(section, "No topic-level evaluation is available.");
        }
        else
        {
            foreach (var topic in source.Topics
                         .OrderBy(x => x.CurrentMasteryPercentage ?? 101m)
                         .Take(12))
            {
                AddReportMetricBar(
                    section,
                    topic.TopicName,
                    topic.CurrentMasteryPercentage,
                    topic.CurrentMasteryPercentage switch
                    {
                        < 40m => Color.FromRgb(214, 90, 84),
                        < 60m => Color.FromRgb(233, 162, 61),
                        _ => Color.FromRgb(82, 98, 227)
                    });
            }
        }

        AddReportSectionHeading(section, "Exact skill record");
        if (report.Skills.Count == 0)
        {
            AddEmpty(section, "No exact-skill evaluation is available.");
        }
        else
        {
            var skills = ReportTable(section, 5);
            ReportHeader(
                skills,
                "Skill",
                "Current",
                "Assessment",
                "Practice",
                "Status");

            foreach (var item in report.Skills
                         .OrderByDescending(x => x.InterventionPriority)
                         .ThenBy(x => x.CurrentMasteryPercentage ?? 101m)
                         .Take(80))
            {
                ReportRow(
                    skills,
                    item.SkillName,
                    NullablePercent(item.CurrentMasteryPercentage),
                    NullablePercent(item.AssessmentMasteryPercentage),
                    NullablePercent(item.PracticeMasteryPercentage),
                    item.Status.ToString());
            }
        }

        AddReportSectionHeading(section, "Assessment development");
        foreach (var item in page.Assessments
                     .OrderBy(x => x.AssessmentDate)
                     .TakeLast(8))
        {
            AddReportMetricBar(
                section,
                item.Title,
                item.Percentage,
                Color.FromRgb(84, 97, 225));
        }

        if (page.Assessments.Count == 0)
        {
            AddEmpty(section, "No assessment history is available in this report scope.");
        }
        else
        {
            var assessments = ReportTable(section, 5);
            ReportHeader(
                assessments,
                "Date",
                "Assessment",
                "Score",
                "Overall change",
                "Comparable-skill growth");

            foreach (var item in page.Assessments.Take(40))
            {
                ReportRow(
                    assessments,
                    item.AssessmentDate.ToString(
                        "yyyy-MM-dd",
                        CultureInfo.InvariantCulture),
                    item.Title,
                    Percent(item.Percentage),
                    Points(item.OverallChangePercentagePoints),
                    item.ComparableSkillChangePercentagePoints.HasValue
                        ? $"{Points(item.ComparableSkillChangePercentagePoints)} · {item.ComparableSkillCount} skills"
                        : "—");
            }
        }

        AddReportSectionHeading(section, "Recommended next steps");
        if (source.Recommendations.Count == 0)
        {
            AddEmpty(
                section,
                "No intervention priority is currently supported by the available evidence.");
        }
        else
        {
            var recommendations = ReportTable(section, 3);
            ReportHeader(
                recommendations,
                "Priority",
                "Skill",
                "Reason");

            foreach (var item in source.Recommendations.Take(10))
            {
                var path = item.RecommendedPrerequisitePath.Count == 0
                    ? string.Empty
                    : " Prerequisite path: "
                      + string.Join(" -> ", item.RecommendedPrerequisitePath)
                      + " -> "
                      + item.SkillKey;

                ReportRow(
                    recommendations,
                    item.Priority.ToString(),
                    item.SkillName,
                    item.Reason + path);
            }
        }

        AddReportSectionHeading(section, "Evidence summary");
        var evidence = ReportTable(section, 4);
        ReportHeader(
            evidence,
            "Evidence records",
            "Confidence",
            "Coverage",
            "Report scope");
        ReportRow(
            evidence,
            page.Evidence.Count.ToString(CultureInfo.InvariantCulture),
            $"{Percent(report.ConfidencePercentage)} · {report.ConfidenceBand}",
            Percent(report.CurriculumCoveragePercentage),
            page.SelectedTermName ?? "Whole academic year");

        AddReportWritingPlaceholders(
            section,
            "Teacher comments",
            "Parent / student review");

        AddReportFooter(
            section,
            "Edulytics · Student evaluation report",
            page.GeneratedAtUtc);

        return Render(document);
    }

    public static byte[] RenderStudentSelfEvaluationReport(
        StudentSelfEvaluationReportPage page)
    {
        ArgumentNullException.ThrowIfNull(page);

        var source = page.Source;
        var report = source.OfficialEvaluation;
        var document = CreateDocument(
            $"Edulytics my progress report - {report.DisplayName}");
        var section = document.AddSection();

        ConfigureStudentReportPage(section);
        AddReportBrandHeader(
            section,
            "My progress report",
            $"{report.DisplayName} · {report.StudentNumber}");
        AddReportContext(
            section,
            report.AcademicYearName,
            report.ClassName,
            report.SubjectName,
            page.SelectedTermName ?? "Whole academic year",
            page.GeneratedAtUtc);

        if (page.SelectedTerm is not null)
        {
            AddReportSectionHeading(section, $"Term focus · {page.SelectedTerm.TermName}");
            var termTable = ReportTable(section, 3);
            ReportHeader(
                termTable,
                "Assessment",
                "Comparable growth",
                "Evidence");
            ReportRow(
                termTable,
                NullablePercent(page.SelectedTerm.AssessmentMasteryPercentage),
                Points(page.SelectedTerm.ComparableSkillGrowthPercentagePoints),
                page.SelectedTerm.EvidenceCount.ToString(CultureInfo.InvariantCulture));
        }

        AddReportSectionHeading(section, "My progress overview");
        AddReportMetricBar(
            section,
            "Current mastery",
            report.CurrentMasteryPercentage,
            Color.FromRgb(82, 98, 227));
        AddReportMetricBar(
            section,
            "Assessment mastery",
            report.AssessmentMasteryPercentage,
            Color.FromRgb(62, 96, 210));
        AddReportMetricBar(
            section,
            "Private Practice mastery",
            source.PrivatePractice.MasteryPercentage,
            Color.FromRgb(42, 169, 119));
        AddReportMetricBar(
            section,
            "Curriculum coverage",
            report.CurriculumCoveragePercentage,
            Color.FromRgb(233, 162, 61));

        var privatePractice = ReportTable(section, 4);
        ReportHeader(
            privatePractice,
            "Practice sessions",
            "Active Practice days",
            "Skills practiced",
            "Practice evidence");
        ReportRow(
            privatePractice,
            source.PrivatePractice.SessionCount.ToString(CultureInfo.InvariantCulture),
            source.PrivatePractice.ActiveDayCount.ToString(CultureInfo.InvariantCulture),
            source.PrivatePractice.SkillCount.ToString(CultureInfo.InvariantCulture),
            source.PrivatePractice.EvidenceCount.ToString(CultureInfo.InvariantCulture));

        var note = section.AddParagraph();
        note.Format.SpaceBefore = Unit.FromPoint(5);
        note.Format.SpaceAfter = Unit.FromPoint(8);
        note.Format.Font.Color = Color.FromRgb(93, 105, 130);
        note.AddText(
            "Official mastery and private Practice are intentionally shown separately. "
            + "Private Practice helps explain progress but is not silently merged into official mastery.");

        AddReportSectionHeading(section, "What is going well / what to strengthen");
        var split = ReportTable(section, 2);
        var header = split.AddRow();
        header.Format.Font.Bold = true;
        header.Cells[0].Shading.Color = Color.FromRgb(232, 248, 240);
        header.Cells[1].Shading.Color = Color.FromRgb(255, 244, 225);
        header.Cells[0].AddParagraph("Going well");
        header.Cells[1].AddParagraph("Skills to strengthen");

        var row = split.AddRow();
        var strengths = source.Skills
            .Where(x =>
                x.OfficialStatus is
                    Edulytics.Core.Analytics.EvaluationSkillStatus.Strong or
                    Edulytics.Core.Analytics.EvaluationSkillStatus.Secure)
            .OrderByDescending(x => x.OfficialMasteryPercentage ?? 0m)
            .Take(8)
            .ToArray();
        var focus = source.Skills
            .Where(x =>
                x.OfficialStatus is
                    Edulytics.Core.Analytics.EvaluationSkillStatus.Critical or
                    Edulytics.Core.Analytics.EvaluationSkillStatus.NeedsFocus)
            .OrderByDescending(x => x.Priority)
            .ThenBy(x => x.OfficialMasteryPercentage ?? 101m)
            .Take(8)
            .ToArray();

        row.Cells[0].AddParagraph(
            strengths.Length == 0
                ? "More evidence is needed before a secure skill can be confirmed."
                : string.Join(
                    "\n",
                    strengths.Select(x =>
                        $"• {x.SkillName} · {NullablePercent(x.OfficialMasteryPercentage)}")));
        row.Cells[1].AddParagraph(
            focus.Length == 0
                ? "No priority focus skill is currently supported by enough evidence."
                : string.Join(
                    "\n",
                    focus.Select(x =>
                        $"• {x.SkillName} · {NullablePercent(x.OfficialMasteryPercentage)}")));

        AddReportSectionHeading(section, "My skill record");
        if (source.Skills.Count == 0)
        {
            AddEmpty(section, "No skill evaluation is available yet.");
        }
        else
        {
            var skills = ReportTable(section, 5);
            ReportHeader(
                skills,
                "Skill",
                "Current",
                "Assessment",
                "Private Practice",
                "Status");

            foreach (var item in source.Skills
                         .OrderByDescending(x => x.Priority)
                         .ThenBy(x => x.OfficialMasteryPercentage ?? 101m)
                         .Take(80))
            {
                ReportRow(
                    skills,
                    item.SkillName,
                    NullablePercent(item.OfficialMasteryPercentage),
                    NullablePercent(item.AssessmentMasteryPercentage),
                    NullablePercent(item.PrivatePracticeMasteryPercentage),
                    item.OfficialStatus.ToString());
            }
        }

        AddReportSectionHeading(section, "Assessment development");
        foreach (var item in page.Assessments
                     .OrderBy(x => x.AssessmentDate)
                     .TakeLast(8))
        {
            AddReportMetricBar(
                section,
                item.Title,
                item.Percentage,
                Color.FromRgb(84, 97, 225));
        }

        if (page.Assessments.Count == 0)
        {
            AddEmpty(section, "No assessment history is available in this report scope.");
        }
        else
        {
            var assessments = ReportTable(section, 4);
            ReportHeader(
                assessments,
                "Date",
                "Assessment",
                "Score",
                "Comparable-skill growth");

            foreach (var item in page.Assessments.Take(40))
            {
                ReportRow(
                    assessments,
                    item.AssessmentDate.ToString(
                        "yyyy-MM-dd",
                        CultureInfo.InvariantCulture),
                    item.Title,
                    Percent(item.Percentage),
                    Points(item.ComparableSkillChangePercentagePoints));
            }
        }

        AddReportSectionHeading(section, "My next steps");
        if (source.NextSteps.Count == 0)
        {
            AddEmpty(section, "More evidence is needed before a next step can be prioritized.");
        }
        else
        {
            var next = ReportTable(section, 3);
            ReportHeader(next, "Priority", "Skill", "Next step");

            foreach (var item in source.NextSteps.Take(8))
            {
                ReportRow(
                    next,
                    item.Priority.ToString(),
                    item.SkillName,
                    item.Message);
            }
        }

        AddReportWritingPlaceholders(
            section,
            "My reflection",
            "Parent review");

        AddReportFooter(
            section,
            "Edulytics · My progress report",
            page.GeneratedAtUtc);

        return Render(document);
    }

    public static byte[] RenderClassEvaluationReport(
        AnalyticsStudentsEvaluationPage studentsPage,
        AnalyticsTopicSkillEvaluationPage topicPage)
    {
        ArgumentNullException.ThrowIfNull(studentsPage);
        ArgumentNullException.ThrowIfNull(topicPage);

        var generatedAtUtc = DateTime.UtcNow;
        var document = CreateDocument("Edulytics class evaluation report");
        var section = document.AddSection();

        ConfigureClassEvaluationReportPage(section);
        AddClassReportBrandHeader(
            section,
            "Class evaluation report",
            $"{studentsPage.ClassName} · {studentsPage.SubjectName}");

        var context = ClassReportTable(
            section,
            6.4,
            7.0,
            6.0,
            6.2);
        ReportHeader(
            context,
            "Academic year",
            "Class",
            "Subject",
            "Report scope");
        ReportRow(
            context,
            studentsPage.AcademicYearName,
            studentsPage.ClassName,
            studentsPage.SubjectName,
            "Whole class evaluation");

        var generated = section.AddParagraph();
        generated.Format.SpaceBefore = Unit.FromPoint(3);
        generated.Format.SpaceAfter = Unit.FromPoint(7);
        generated.Format.Font.Size = Unit.FromPoint(7.5);
        generated.Format.Font.Color = Color.FromRgb(122, 132, 153);
        generated.AddText(
            $"Generated {generatedAtUtc:yyyy-MM-dd HH:mm} UTC");

        var distribution = studentsPage.Distribution;
        var studentsWithMastery = studentsPage.Students
            .Where(x => x.CurrentMasteryPercentage.HasValue)
            .ToArray();
        var averageMastery = studentsWithMastery.Length == 0
            ? (decimal?)null
            : decimal.Round(
                studentsWithMastery.Average(
                    x => x.CurrentMasteryPercentage!.Value),
                1,
                MidpointRounding.AwayFromZero);
        var averageCoverage = studentsPage.Students.Count == 0
            ? 0m
            : decimal.Round(
                studentsPage.Students.Average(x => x.CoveragePercentage),
                1,
                MidpointRounding.AwayFromZero);
        var averageConfidence = studentsPage.Students.Count == 0
            ? 0m
            : decimal.Round(
                studentsPage.Students.Average(x => x.ConfidencePercentage),
                1,
                MidpointRounding.AwayFromZero);
        var needsAttention =
            distribution.NeedsFocus +
            distribution.Critical;

        AddReportSectionHeading(section, "Class performance overview");
        var kpis = ClassReportTable(
            section,
            5.1,
            5.1,
            5.1,
            5.1,
            5.1);
        ReportHeader(
            kpis,
            "Students",
            "Average mastery",
            "Needs attention",
            "Average coverage",
            "Evidence confidence");
        ReportRow(
            kpis,
            distribution.TotalStudents.ToString(CultureInfo.InvariantCulture),
            NullablePercent(averageMastery),
            needsAttention.ToString(CultureInfo.InvariantCulture),
            Percent(averageCoverage),
            Percent(averageConfidence));

        AddClassMetricBar(
            section,
            "Average current mastery",
            averageMastery,
            Color.FromRgb(82, 98, 227));
        AddClassMetricBar(
            section,
            "Average curriculum coverage",
            averageCoverage,
            Color.FromRgb(233, 162, 61));
        AddClassMetricBar(
            section,
            "Evidence confidence",
            averageConfidence,
            Color.FromRgb(92, 79, 216));

        var confidenceNote = section.AddParagraph();
        confidenceNote.Format.SpaceBefore = Unit.FromPoint(4);
        confidenceNote.Format.SpaceAfter = Unit.FromPoint(7);
        confidenceNote.Format.Font.Size = Unit.FromPoint(8);
        confidenceNote.Format.Font.Color = Color.FromRgb(93, 105, 130);
        confidenceNote.AddText(
            "Mastery grade and evidence confidence are different. "
            + "Evidence confidence is coverage-adjusted and describes how strongly the available evidence supports the subject-level evaluation; it is not the student's grade.");

        AddReportSectionHeading(section, "Mastery distribution");
        AddClassDistributionBar(section, distribution);
        var masteryBands = ClassReportTable(
            section,
            5.1,
            5.1,
            5.1,
            5.1,
            5.1);
        var masteryHeader = masteryBands.AddRow();
        masteryHeader.Format.Font.Bold = true;
        masteryHeader.Format.Font.Color = Color.FromRgb(53, 67, 103);
        masteryHeader.Cells[0].Shading.Color = Color.FromRgb(232, 248, 240);
        masteryHeader.Cells[1].Shading.Color = Color.FromRgb(238, 240, 255);
        masteryHeader.Cells[2].Shading.Color = Color.FromRgb(255, 244, 225);
        masteryHeader.Cells[3].Shading.Color = Color.FromRgb(253, 235, 234);
        masteryHeader.Cells[4].Shading.Color = Color.FromRgb(241, 243, 247);
        masteryHeader.Cells[0].AddParagraph("Secure / strong");
        masteryHeader.Cells[1].AddParagraph("Developing");
        masteryHeader.Cells[2].AddParagraph("Needs focus");
        masteryHeader.Cells[3].AddParagraph("Critical");
        masteryHeader.Cells[4].AddParagraph("Insufficient");

        var masteryRow = masteryBands.AddRow();
        masteryRow.Format.Font.Size = Unit.FromPoint(9);
        masteryRow.Format.Font.Bold = true;
        masteryRow.Cells[0].AddParagraph(
            $"{distribution.StrongOrSecure} · {ClassShare(distribution.StrongOrSecure, distribution.TotalStudents)}");
        masteryRow.Cells[1].AddParagraph(
            $"{distribution.Developing} · {ClassShare(distribution.Developing, distribution.TotalStudents)}");
        masteryRow.Cells[2].AddParagraph(
            $"{distribution.NeedsFocus} · {ClassShare(distribution.NeedsFocus, distribution.TotalStudents)}");
        masteryRow.Cells[3].AddParagraph(
            $"{distribution.Critical} · {ClassShare(distribution.Critical, distribution.TotalStudents)}");
        masteryRow.Cells[4].AddParagraph(
            $"{distribution.InsufficientEvidence} · {ClassShare(distribution.InsufficientEvidence, distribution.TotalStudents)}");

        AddReportSectionHeading(section, "Student movement");
        var movement = ClassReportTable(
            section,
            8.5,
            8.5,
            8.5);
        var movementHeader = movement.AddRow();
        movementHeader.Format.Font.Bold = true;
        movementHeader.Cells[0].Shading.Color = Color.FromRgb(232, 248, 240);
        movementHeader.Cells[1].Shading.Color = Color.FromRgb(238, 243, 255);
        movementHeader.Cells[2].Shading.Color = Color.FromRgb(253, 235, 234);
        movementHeader.Cells[0].AddParagraph("Improving");
        movementHeader.Cells[1].AddParagraph("Stable");
        movementHeader.Cells[2].AddParagraph("Declining");
        ReportRow(
            movement,
            $"{distribution.Improving} · {ClassShare(distribution.Improving, distribution.TotalStudents)}",
            $"{distribution.Stable} · {ClassShare(distribution.Stable, distribution.TotalStudents)}",
            $"{distribution.Declining} · {ClassShare(distribution.Declining, distribution.TotalStudents)}");

        AddReportSectionHeading(section, "Mastery grading");
        var grading = ClassReportTable(
            section,
            5.1,
            5.1,
            5.1,
            5.1,
            5.1);
        var gradingRow = grading.AddRow();
        gradingRow.Cells[0].Shading.Color = Color.FromRgb(253, 235, 234);
        gradingRow.Cells[1].Shading.Color = Color.FromRgb(255, 244, 225);
        gradingRow.Cells[2].Shading.Color = Color.FromRgb(238, 240, 255);
        gradingRow.Cells[3].Shading.Color = Color.FromRgb(232, 248, 240);
        gradingRow.Cells[4].Shading.Color = Color.FromRgb(233, 245, 255);
        gradingRow.Cells[0].AddParagraph("Critical < 40%");
        gradingRow.Cells[1].AddParagraph("Needs focus 40-59%");
        gradingRow.Cells[2].AddParagraph("Developing 60-74%");
        gradingRow.Cells[3].AddParagraph("Secure 75-89%");
        gradingRow.Cells[4].AddParagraph("Strong 90-100%");

        AddReportSectionHeading(section, "All students");
        if (studentsPage.Students.Count == 0)
        {
            AddEmpty(section, "No students are available in this class.");
        }
        else
        {
            var students = ClassReportTable(
                section,
                4.2,
                2.0,
                2.7,
                2.5,
                2.5,
                2.1,
                2.5,
                1.6,
                3.4);
            ReportHeader(
                students,
                "Student",
                "Current",
                "Grade",
                "Assessment",
                "Practice",
                "Coverage",
                "Trend",
                "Gaps",
                "Evidence confidence");

            foreach (var item in studentsPage.Students.Take(120))
            {
                var row = students.AddRow();
                row.VerticalAlignment = VerticalAlignment.Center;
                var grade = ClassMasteryGrade(item.CurrentMasteryPercentage);

                row.Cells[0].AddParagraph(
                    $"{item.DisplayName}\n{item.StudentNumber}");
                row.Cells[1].AddParagraph(
                    NullablePercent(item.CurrentMasteryPercentage));
                row.Cells[2].AddParagraph(grade.Label);
                row.Cells[2].Shading.Color = grade.Color;
                row.Cells[3].AddParagraph(
                    NullablePercent(item.AssessmentMasteryPercentage));
                row.Cells[4].AddParagraph(
                    NullablePercent(item.PracticeMasteryPercentage));
                row.Cells[5].AddParagraph(
                    Percent(item.CoveragePercentage));
                row.Cells[6].AddParagraph(
                    Trend(item.ShortTermTrend));
                row.Cells[7].AddParagraph(
                    item.CriticalSkillCount.ToString(CultureInfo.InvariantCulture));
                row.Cells[8].AddParagraph(
                    $"{Percent(item.ConfidencePercentage)}\n{ConfidenceBand(item.ConfidenceBand)}");
            }
        }

        AddReportSectionHeading(section, "Priority topic and skill gaps");
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
            var gaps = ClassReportTable(
                section,
                5.0,
                6.3,
                3.0,
                2.6,
                2.5,
                3.0,
                3.0);
            ReportHeader(
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
                ReportRow(
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

        AddReportFooter(
            section,
            "Edulytics · Class evaluation report",
            generatedAtUtc);

        return Render(document);
    }

    private static void ConfigureClassEvaluationReportPage(
        Section section)
    {
        section.PageSetup.PageFormat = PageFormat.A4;
        section.PageSetup.Orientation = Orientation.Landscape;
        section.PageSetup.TopMargin = Unit.FromCentimeter(.9);
        section.PageSetup.BottomMargin = Unit.FromCentimeter(1.1);
        section.PageSetup.LeftMargin = Unit.FromCentimeter(1.0);
        section.PageSetup.RightMargin = Unit.FromCentimeter(1.0);
    }

    private static void AddClassReportBrandHeader(
        Section section,
        string title,
        string subtitle)
    {
        var table = section.AddTable();
        table.Borders.Width = Unit.FromPoint(0);
        table.AddColumn(Unit.FromCentimeter(6.0));
        table.AddColumn(Unit.FromCentimeter(19.5));

        var row = table.AddRow();
        row.Cells[0].VerticalAlignment = VerticalAlignment.Center;
        row.Cells[1].VerticalAlignment = VerticalAlignment.Center;

        var logoPath = ResolveBrandLogoPath();
        if (logoPath is not null)
        {
            try
            {
                var logo = row.Cells[0].AddImage(logoPath);
                logo.LockAspectRatio = true;
                logo.Width = Unit.FromCentimeter(4.7);
            }
            catch
            {
                row.Cells[0].AddParagraph("EDULYTICS");
            }
        }
        else
        {
            var brand = row.Cells[0].AddParagraph("EDULYTICS");
            brand.Format.Font.Bold = true;
            brand.Format.Font.Size = Unit.FromPoint(16);
            brand.Format.Font.Color = Color.FromRgb(55, 77, 172);
        }

        var heading = row.Cells[1].AddParagraph();
        heading.Format.Alignment = ParagraphAlignment.Right;
        heading.Format.Font.Bold = true;
        heading.Format.Font.Size = Unit.FromPoint(18);
        heading.Format.Font.Color = Color.FromRgb(28, 42, 81);
        heading.AddText(title);

        var sub = row.Cells[1].AddParagraph();
        sub.Format.Alignment = ParagraphAlignment.Right;
        sub.Format.Font.Size = Unit.FromPoint(9);
        sub.Format.Font.Color = Color.FromRgb(111, 123, 150);
        sub.AddText(subtitle);

        var rule = section.AddParagraph();
        rule.Format.SpaceAfter = Unit.FromPoint(7);
        rule.Format.Borders.Bottom.Width = Unit.FromPoint(1.2);
        rule.Format.Borders.Bottom.Color = Color.FromRgb(82, 98, 227);
    }

    private static Table ClassReportTable(
        Section section,
        params double[] widthsCm)
    {
        var table = section.AddTable();
        table.Borders.Width = Unit.FromPoint(.45);
        table.Borders.Color = Color.FromRgb(224, 229, 239);
        table.Format.Font.Size = Unit.FromPoint(7.6);
        table.Rows.LeftIndent = Unit.Zero;
        table.LeftPadding = Unit.FromPoint(4);
        table.RightPadding = Unit.FromPoint(4);

        foreach (var width in widthsCm)
            table.AddColumn(Unit.FromCentimeter(width));

        return table;
    }

    private static void AddClassMetricBar(
        Section section,
        string label,
        decimal? value,
        Color color)
    {
        var percentage = value.HasValue
            ? Math.Clamp(value.Value, 0m, 100m)
            : 0m;
        const double trackWidthCm = 17.2;
        var fillWidth = value.HasValue
            ? Math.Max(
                .12,
                Math.Min(
                    trackWidthCm - .12,
                    trackWidthCm *
                    (double)(percentage / 100m)))
            : .12;
        var remainderWidth =
            Math.Max(.12, trackWidthCm - fillWidth);

        var table = section.AddTable();
        table.Borders.Width = Unit.FromPoint(0);
        table.Format.Font.Size = Unit.FromPoint(8);
        table.AddColumn(Unit.FromCentimeter(4.5));
        table.AddColumn(Unit.FromCentimeter(fillWidth));
        table.AddColumn(Unit.FromCentimeter(remainderWidth));
        table.AddColumn(Unit.FromCentimeter(3.0));

        var row = table.AddRow();
        row.Height = Unit.FromPoint(12);
        row.VerticalAlignment = VerticalAlignment.Center;
        row.Cells[0].AddParagraph(label);
        row.Cells[1].Shading.Color = value.HasValue
            ? color
            : Color.FromRgb(210, 215, 226);
        row.Cells[2].Shading.Color = Color.FromRgb(238, 241, 246);
        row.Cells[3].AddParagraph(
            value.HasValue
                ? Percent(value.Value)
                : "—");
        row.Cells[3].Format.Font.Bold = true;
        row.Cells[3].Format.Alignment = ParagraphAlignment.Right;

        var spacer = section.AddParagraph();
        spacer.Format.SpaceAfter = Unit.FromPoint(1);
    }

    private static void AddClassDistributionBar(
        Section section,
        AnalyticsEvaluationDistribution distribution)
    {
        var total = Math.Max(1, distribution.TotalStudents);
        const double chartWidth = 25.4;

        var values = new[]
        {
            distribution.StrongOrSecure,
            distribution.Developing,
            distribution.NeedsFocus,
            distribution.Critical,
            distribution.InsufficientEvidence
        };
        var colors = new[]
        {
            Color.FromRgb(44, 169, 119),
            Color.FromRgb(92, 114, 222),
            Color.FromRgb(233, 162, 61),
            Color.FromRgb(214, 90, 84),
            Color.FromRgb(171, 179, 193)
        };

        var widths = values
            .Select(value =>
                Math.Max(
                    .12,
                    chartWidth *
                    (double)(value / (decimal)total)))
            .ToArray();

        var table = section.AddTable();
        table.Borders.Width = Unit.FromPoint(0);
        table.Rows.LeftIndent = Unit.Zero;

        foreach (var width in widths)
            table.AddColumn(Unit.FromCentimeter(width));

        var row = table.AddRow();
        row.Height = Unit.FromPoint(12);

        for (var i = 0; i < colors.Length; i++)
            row.Cells[i].Shading.Color = colors[i];

        var spacer = section.AddParagraph();
        spacer.Format.SpaceAfter = Unit.FromPoint(3);
    }

    private static string ClassShare(
        int count,
        int total)
    {
        if (total <= 0)
            return "0%";

        var percentage =
            decimal.Round(
                count * 100m / total,
                1,
                MidpointRounding.AwayFromZero);

        return Percent(percentage);
    }

    private static (string Label, Color Color) ClassMasteryGrade(
        decimal? mastery)
    {
        if (!mastery.HasValue)
        {
            return (
                "Insufficient",
                Color.FromRgb(241, 243, 247));
        }

        return mastery.Value switch
        {
            < 40m => (
                "Critical",
                Color.FromRgb(253, 235, 234)),
            < 60m => (
                "Needs focus",
                Color.FromRgb(255, 244, 225)),
            < 75m => (
                "Developing",
                Color.FromRgb(238, 240, 255)),
            < 90m => (
                "Secure",
                Color.FromRgb(232, 248, 240)),
            _ => (
                "Strong",
                Color.FromRgb(233, 245, 255))
        };
    }

    private static void ConfigureStudentReportPage(
        Section section)
    {
        section.PageSetup.PageFormat = PageFormat.A4;
        section.PageSetup.Orientation = Orientation.Portrait;
        section.PageSetup.TopMargin = Unit.FromCentimeter(1.1);
        section.PageSetup.BottomMargin = Unit.FromCentimeter(1.25);
        section.PageSetup.LeftMargin = Unit.FromCentimeter(1.15);
        section.PageSetup.RightMargin = Unit.FromCentimeter(1.15);
    }

    private static void AddReportBrandHeader(
        Section section,
        string title,
        string subtitle)
    {
        var table = section.AddTable();
        table.Borders.Width = Unit.FromPoint(0);
        table.AddColumn(Unit.FromCentimeter(5));
        table.AddColumn(Unit.FromCentimeter(12.5));

        var row = table.AddRow();
        row.Cells[0].VerticalAlignment = VerticalAlignment.Center;
        row.Cells[1].VerticalAlignment = VerticalAlignment.Center;

        var logoPath = ResolveBrandLogoPath();

        if (logoPath is not null)
        {
            try
            {
                var logo = row.Cells[0].AddImage(logoPath);
                logo.LockAspectRatio = true;
                logo.Width = Unit.FromCentimeter(4.2);
            }
            catch
            {
                row.Cells[0].AddParagraph("EDULYTICS");
            }
        }
        else
        {
            var brand = row.Cells[0].AddParagraph("EDULYTICS");
            brand.Format.Font.Bold = true;
            brand.Format.Font.Size = Unit.FromPoint(15);
            brand.Format.Font.Color = Color.FromRgb(55, 77, 172);
        }

        var heading = row.Cells[1].AddParagraph();
        heading.Format.Alignment = ParagraphAlignment.Right;
        heading.Format.Font.Bold = true;
        heading.Format.Font.Size = Unit.FromPoint(17);
        heading.Format.Font.Color = Color.FromRgb(28, 42, 81);
        heading.AddText(title);

        var sub = row.Cells[1].AddParagraph();
        sub.Format.Alignment = ParagraphAlignment.Right;
        sub.Format.Font.Size = Unit.FromPoint(9);
        sub.Format.Font.Color = Color.FromRgb(111, 123, 150);
        sub.AddText(subtitle);

        var rule = section.AddParagraph();
        rule.Format.SpaceAfter = Unit.FromPoint(7);
        rule.Format.Borders.Bottom.Width = Unit.FromPoint(1.2);
        rule.Format.Borders.Bottom.Color = Color.FromRgb(82, 98, 227);
    }

    private static void AddReportContext(
        Section section,
        string academicYear,
        string className,
        string subjectName,
        string scope,
        DateTime generatedAtUtc)
    {
        var table = ReportTable(section, 4);
        ReportHeader(
            table,
            "Academic year",
            "Class",
            "Subject",
            "Report scope");
        ReportRow(
            table,
            academicYear,
            className,
            subjectName,
            scope);

        var generated = section.AddParagraph();
        generated.Format.SpaceBefore = Unit.FromPoint(3);
        generated.Format.SpaceAfter = Unit.FromPoint(6);
        generated.Format.Font.Size = Unit.FromPoint(7.5);
        generated.Format.Font.Color = Color.FromRgb(122, 132, 153);
        generated.AddText(
            $"Generated {generatedAtUtc:yyyy-MM-dd HH:mm} UTC");
    }

    private static void AddReportSectionHeading(
        Section section,
        string title)
    {
        var heading = section.AddParagraph();
        heading.Format.Font.Bold = true;
        heading.Format.Font.Size = Unit.FromPoint(11.5);
        heading.Format.Font.Color = Color.FromRgb(31, 45, 82);
        heading.Format.SpaceBefore = Unit.FromPoint(9);
        heading.Format.SpaceAfter = Unit.FromPoint(4);
        heading.AddText(title);
    }

    private static Table ReportTable(
        Section section,
        int columns)
    {
        var table = section.AddTable();
        table.Borders.Width = Unit.FromPoint(.45);
        table.Borders.Color = Color.FromRgb(224, 229, 239);
        table.Format.Font.Size = Unit.FromPoint(7.8);
        table.Rows.LeftIndent = Unit.Zero;
        table.LeftPadding = Unit.FromPoint(4);
        table.RightPadding = Unit.FromPoint(4);

        var total = 17.5;
        var width = total / columns;

        for (var i = 0; i < columns; i++)
            table.AddColumn(Unit.FromCentimeter(width));

        return table;
    }

    private static void ReportHeader(
        Table table,
        params string[] values)
    {
        var row = table.AddRow();
        row.Format.Font.Bold = true;
        row.Format.Font.Color = Color.FromRgb(53, 67, 103);
        row.Shading.Color = Color.FromRgb(244, 246, 251);
        row.VerticalAlignment = VerticalAlignment.Center;

        for (var i = 0; i < values.Length; i++)
        {
            row.Cells[i].AddParagraph(values[i]);
        }
    }

    private static void ReportRow(
        Table table,
        params string[] values)
    {
        var row = table.AddRow();
        row.VerticalAlignment = VerticalAlignment.Center;

        for (var i = 0; i < values.Length; i++)
        {
            row.Cells[i].AddParagraph(values[i] ?? string.Empty);
        }
    }

    private static void AddReportMetricBar(
        Section section,
        string label,
        decimal? value,
        Color color)
    {
        var percentage = value.HasValue
            ? Math.Clamp(value.Value, 0m, 100m)
            : 0m;
        const double trackWidthCm = 10.2;
        var fillWidth = value.HasValue
            ? Math.Max(
                .12,
                Math.Min(
                    trackWidthCm - .12,
                    trackWidthCm *
                    (double)(percentage / 100m)))
            : .12;
        var remainderWidth =
            Math.Max(.12, trackWidthCm - fillWidth);

        var table = section.AddTable();
        table.Borders.Width = Unit.FromPoint(0);
        table.Format.Font.Size = Unit.FromPoint(8);
        table.AddColumn(Unit.FromCentimeter(4.3));
        table.AddColumn(Unit.FromCentimeter(fillWidth));
        table.AddColumn(Unit.FromCentimeter(remainderWidth));
        table.AddColumn(Unit.FromCentimeter(2.5));

        var row = table.AddRow();
        row.Height = Unit.FromPoint(12);
        row.VerticalAlignment = VerticalAlignment.Center;
        row.Cells[0].AddParagraph(label);
        row.Cells[1].Shading.Color = value.HasValue
            ? color
            : Color.FromRgb(210, 215, 226);
        row.Cells[2].Shading.Color = Color.FromRgb(238, 241, 246);
        row.Cells[3].AddParagraph(
            value.HasValue
                ? Percent(value.Value)
                : "—");
        row.Cells[3].Format.Font.Bold = true;
        row.Cells[3].Format.Alignment = ParagraphAlignment.Right;

        var spacer = section.AddParagraph();
        spacer.Format.SpaceAfter = Unit.FromPoint(2);
    }

    private static void AddReportWritingPlaceholders(
        Section section,
        string firstTitle,
        string secondTitle)
    {
        AddReportSectionHeading(section, "Review notes");
        var table = ReportTable(section, 2);
        var header = table.AddRow();
        header.Format.Font.Bold = true;
        header.Shading.Color = Color.FromRgb(247, 248, 252);
        header.Cells[0].AddParagraph(firstTitle);
        header.Cells[1].AddParagraph(secondTitle);

        var row = table.AddRow();
        row.Height = Unit.FromCentimeter(2.2);
        row.Cells[0].AddParagraph(" ");
        row.Cells[1].AddParagraph(" ");
    }

    private static void AddReportFooter(
        Section section,
        string label,
        DateTime generatedAtUtc)
    {
        var footer = section.Footers.Primary.AddParagraph();
        footer.Format.Font.Size = Unit.FromPoint(7);
        footer.Format.Font.Color = Color.FromRgb(128, 137, 155);
        footer.Format.Alignment = ParagraphAlignment.Center;
        footer.AddText(
            $"{label} · Generated {generatedAtUtc:yyyy-MM-dd} · Page ");
        footer.AddPageField();
        footer.AddText(" of ");
        footer.AddNumPagesField();
    }

    private static string? ResolveBrandLogoPath()
    {
        var fileName =
            CultureInfo.CurrentUICulture.TwoLetterISOLanguageName == "pl"
                ? "edulityks-pl.png"
                : "edulytics-en.png";

        var candidates = new[]
        {
            Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "images",
                "brand",
                fileName),
            Path.Combine(
                Directory.GetCurrentDirectory(),
                "src",
                "Edulytics.Web",
                "wwwroot",
                "images",
                "brand",
                fileName),
            Path.Combine(
                AppContext.BaseDirectory,
                "wwwroot",
                "images",
                "brand",
                fileName)
        };

        return candidates.FirstOrDefault(File.Exists);
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

    private static string ConfidenceBand(
        Edulytics.Core.Analytics.EvaluationConfidenceBand value) =>
        value switch
        {
            Edulytics.Core.Analytics.EvaluationConfidenceBand.VeryStrong =>
                "Very strong",
            Edulytics.Core.Analytics.EvaluationConfidenceBand.Strong =>
                "Strong",
            Edulytics.Core.Analytics.EvaluationConfidenceBand.Moderate =>
                "Moderate",
            Edulytics.Core.Analytics.EvaluationConfidenceBand.Limited =>
                "Limited",
            _ =>
                "Insufficient"
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

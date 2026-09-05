using System.Globalization;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using PdfSharp.Fonts;

namespace Edulytics.Web.Printing;

public static class AssessmentPdfRenderer
{
    public static byte[] RenderStudentPaper(
        StudentAssessmentPaper paper,
        AssessmentPdfLabels labels)
    {
        ArgumentNullException.ThrowIfNull(paper);
        ArgumentNullException.ThrowIfNull(labels);

        var document = CreateDocument($"{labels.StudentPaperTitle}: {paper.Title}");
        var section = document.AddSection();
        ConfigurePage(section);
        AddHeading(section, labels.StudentPaperTitle, paper.Title);
        AddAssessmentMeta(section, labels, paper.AssessmentDate, paper.MaxScore);

        var student = section.AddParagraph();
        student.Format.SpaceAfter = Unit.FromPoint(10);
        student.AddFormattedText($"{labels.StudentName}: ", TextFormat.Bold);
        student.AddText("____________________________________________");

        foreach (var question in paper.Questions)
        {
            var paragraph = section.AddParagraph();
            paragraph.Format.SpaceBefore = Unit.FromPoint(10);
            paragraph.AddFormattedText($"{question.Order}. {question.Prompt}", TextFormat.Bold);
            paragraph.AddLineBreak();
            paragraph.AddText($"{labels.Marks}: {FormatScore(question.MaxScore)}");

            var answerSpace = section.AddParagraph();
            answerSpace.Format.SpaceAfter = Unit.FromPoint(8);
            answerSpace.AddText("________________________________________________________________________________");
            answerSpace.AddLineBreak();
            answerSpace.AddText("________________________________________________________________________________");
        }

        return Render(document);
    }

    public static byte[] RenderTeacherAnswerKey(
        TeacherAssessmentAnswerKey answerKey,
        AssessmentPdfLabels labels)
    {
        ArgumentNullException.ThrowIfNull(answerKey);
        ArgumentNullException.ThrowIfNull(labels);

        var document = CreateDocument($"{labels.TeacherAnswerKeyTitle}: {answerKey.Title}");
        var section = document.AddSection();
        ConfigurePage(section);
        AddHeading(section, labels.TeacherAnswerKeyTitle, answerKey.Title);
        AddAssessmentMeta(section, labels, answerKey.AssessmentDate, answerKey.MaxScore);

        foreach (var question in answerKey.Questions)
        {
            var paragraph = section.AddParagraph();
            paragraph.Format.SpaceBefore = Unit.FromPoint(10);
            paragraph.AddFormattedText($"{question.Order}. {question.Prompt}", TextFormat.Bold);
            paragraph.AddLineBreak();
            paragraph.AddText($"{labels.Marks}: {FormatScore(question.MaxScore)}");

            var answer = section.AddParagraph();
            answer.Format.SpaceAfter = Unit.FromPoint(3);
            answer.AddFormattedText($"{labels.CorrectAnswer}: ", TextFormat.Bold);
            answer.AddText(question.CorrectAnswer);

            if (!string.IsNullOrWhiteSpace(question.Solution))
            {
                var solution = section.AddParagraph();
                solution.Format.SpaceAfter = Unit.FromPoint(8);
                solution.AddFormattedText($"{labels.Solution}: ", TextFormat.Bold);
                solution.AddText(question.Solution);
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
        normal.Font.Size = Unit.FromPoint(10);
        return document;
    }

    private static void ConfigurePage(Section section)
    {
        section.PageSetup.PageFormat = PageFormat.A4;
        section.PageSetup.TopMargin = Unit.FromCentimeter(1.5);
        section.PageSetup.BottomMargin = Unit.FromCentimeter(1.5);
        section.PageSetup.LeftMargin = Unit.FromCentimeter(1.7);
        section.PageSetup.RightMargin = Unit.FromCentimeter(1.7);
    }

    private static void AddHeading(
        Section section,
        string documentType,
        string assessmentTitle)
    {
        var heading = section.AddParagraph();
        heading.Format.Font.Bold = true;
        heading.Format.Font.Size = Unit.FromPoint(16);
        heading.Format.SpaceAfter = Unit.FromPoint(4);
        heading.AddText(documentType);

        var title = section.AddParagraph();
        title.Format.Font.Bold = true;
        title.Format.Font.Size = Unit.FromPoint(13);
        title.Format.SpaceAfter = Unit.FromPoint(8);
        title.AddText(assessmentTitle);
    }

    private static void AddAssessmentMeta(
        Section section,
        AssessmentPdfLabels labels,
        DateOnly assessmentDate,
        decimal maxScore)
    {
        var meta = section.AddParagraph();
        meta.Format.SpaceAfter = Unit.FromPoint(8);
        meta.AddFormattedText($"{labels.Date}: ", TextFormat.Bold);
        meta.AddText(assessmentDate.ToString("d", CultureInfo.CurrentCulture));
        meta.AddText("    ");
        meta.AddFormattedText($"{labels.AssessmentMaxScore}: ", TextFormat.Bold);
        meta.AddText(FormatScore(maxScore));
    }

    private static byte[] Render(Document document)
    {
        var renderer = new PdfDocumentRenderer { Document = document };
        renderer.RenderDocument();
        using var stream = new MemoryStream();
        renderer.PdfDocument.Save(stream, false);
        return stream.ToArray();
    }

    private static string FormatScore(decimal score) =>
        score.ToString("0.##", CultureInfo.CurrentCulture);
}

internal static class AssessmentPdfFontBootstrap
{
    private static readonly object Gate = new();
    private static bool configured;

    public static void EnsureConfigured()
    {
        if (configured)
            return;

        lock (Gate)
        {
            if (configured)
                return;

            if (GlobalFontSettings.FontResolver is null)
                GlobalFontSettings.FontResolver = new EdulyticsPdfFontResolver();

            configured = true;
        }
    }
}

internal sealed class EdulyticsPdfFontResolver : IFontResolver
{
    public const string FamilyName = "EdulyticsPdfSans";
    private const string RegularFace = "EdulyticsPdfSans-Regular";
    private const string BoldFace = "EdulyticsPdfSans-Bold";

    private readonly byte[] regular = LoadFont(false);
    private readonly byte[] bold = LoadFont(true);

    public FontResolverInfo ResolveTypeface(
        string familyName,
        bool isBold,
        bool isItalic) =>
        new(isBold ? BoldFace : RegularFace);

    public byte[] GetFont(string faceName) => faceName switch
    {
        RegularFace => regular,
        BoldFace => bold,
        _ => throw new ArgumentOutOfRangeException(
            nameof(faceName),
            faceName,
            "Unknown PDF font face.")
    };

    private static byte[] LoadFont(bool bold)
    {
        foreach (var path in CandidatePaths(bold))
        {
            if (File.Exists(path))
                return File.ReadAllBytes(path);
        }

        throw new InvalidOperationException(
            "No supported PDF font was found. Install fonts-dejavu-core on Linux or provide Arial on Windows.");
    }

    private static IEnumerable<string> CandidatePaths(bool bold)
    {
        var dejavu = bold ? "DejaVuSans-Bold.ttf" : "DejaVuSans.ttf";
        yield return Path.Combine("/usr/share/fonts/truetype/dejavu", dejavu);
        yield return Path.Combine("/usr/share/fonts/dejavu", dejavu);

        var freeSans = bold ? "FreeSansBold.ttf" : "FreeSans.ttf";
        yield return Path.Combine("/usr/share/fonts/truetype/freefont", freeSans);

        var windows = Environment.GetEnvironmentVariable("WINDIR");
        if (!string.IsNullOrWhiteSpace(windows))
            yield return Path.Combine(windows, "Fonts", bold ? "arialbd.ttf" : "arial.ttf");

        yield return Path.Combine(
            "/System/Library/Fonts/Supplemental",
            bold ? "Arial Bold.ttf" : "Arial.ttf");
    }
}

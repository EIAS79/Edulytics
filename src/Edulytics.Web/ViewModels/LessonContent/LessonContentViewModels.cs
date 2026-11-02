using Edulytics.Core.Curriculum;
using Edulytics.Services.LessonContent;
namespace Edulytics.Web.ViewModels.LessonContent;
public sealed record LessonContentIndexViewModel(LessonContentDashboard Dashboard);
public sealed record LessonContentDetailViewModel(CanonicalLessonDetail Lesson);

public sealed record LessonYouTubeStudioViewModel(
    Guid Id,
    string LessonCode,
    string Title,
    string GradeName,
    string FrameworkName,
    RichLessonContentV2Lesson? RichContent,
    string Endpoint);

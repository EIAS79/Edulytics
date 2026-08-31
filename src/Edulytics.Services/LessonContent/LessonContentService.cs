using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
using Edulytics.Core.Enums;
using Edulytics.Core.Interfaces;
using Edulytics.Core.Lessons;
using Edulytics.Core.Users;

namespace Edulytics.Services.LessonContent;

public sealed class LessonContentService : ILessonContentService
{
    private readonly ILessonContentRepository _lessons;
    private readonly ISchoolUserRepository _users;
    private readonly ISchoolRepository _schools;

    public LessonContentService(
        ILessonContentRepository lessons,
        ISchoolUserRepository users,
        ISchoolRepository schools)
    {
        _lessons = lessons;
        _users = users;
        _schools = schools;
    }

    public async Task<LessonContentQueryResult<LessonContentDashboard>> GetDashboardAsync(
        Guid actorUserId,
        CancellationToken cancellationToken = default)
    {
        var scope = await ResolveScopeAsync(actorUserId, cancellationToken);
        if (!scope.Succeeded)
            return LessonContentQueryResult<LessonContentDashboard>.Failure(scope.Error!.Value);
        if (!LessonContentPolicy.CanReadStaff(scope.Actor!.Roles))
            return LessonContentQueryResult<LessonContentDashboard>.Failure(
                LessonContentErrorCode.AccessDenied);

        var contexts = await _lessons.ListStaffAdoptionsAsync(
            scope.School!.Id,
            cancellationToken);
        var resolvableContexts = contexts
            .Where(CanResolveContext)
            .ToArray();

        var lessons = await _lessons.ListPedagogicalLessonsAsync(
            resolvableContexts
                .Select(x => x.FrameworkVersionId)
                .Distinct()
                .ToArray(),
            cancellationToken);
        var contents = await _lessons.ListCanonicalContentsAsync(
            lessons.Select(x => x.Id).ToArray(),
            cancellationToken);
        var contentByLesson = contents.ToDictionary(x => x.PedagogicalLessonId);

        var groups = resolvableContexts
            .GroupBy(x => new
            {
                x.CurriculumAdoptionId,
                x.CurriculumLevelKey,
                x.CurriculumLogicalLevel,
                x.CurriculumPathway,
                x.FrameworkVersionId,
                x.FrameworkCode,
                x.FrameworkName,
                x.FrameworkVersionName,
                x.SubjectName,
                x.SubjectCode,
                x.GradeName,
                x.GradeOrder
            })
            .Select(group =>
            {
                var context = group.First();
                var items = lessons
                    .Where(x => MatchesContext(x, context))
                    .OrderBy(x => x.SortOrder)
                    .Select(lesson =>
                    {
                        contentByLesson.TryGetValue(lesson.Id, out var content);
                        return new CanonicalLessonLibraryItem(
                            lesson.Id,
                            lesson.Code,
                            lesson.Title,
                            lesson.UnitTitle,
                            lesson.SortOrder,
                            content?.Status,
                            content?.PublishedAtUtc,
                            LessonContentPolicy.IsStandaloneCanonicalTarget(
                                lesson.OfficialOutcomeCount));
                    })
                    .ToArray();

                return new CanonicalCurriculumLibraryGroup(
                    context.FrameworkVersionId,
                    context.FrameworkName,
                    context.FrameworkVersionName,
                    context.SubjectName,
                    context.SubjectCode,
                    DisplayLevel(context),
                    items.Length,
                    items.Count(x => LessonContentPolicy.IsProductionReady(
                        x.Status,
                        x.HasOfficialAlignment)),
                    items);
            })
            .OrderBy(x => x.SubjectCode)
            .ThenBy(x => x.GradeName)
            .ThenBy(x => x.FrameworkName)
            .ToArray();

        return LessonContentQueryResult<LessonContentDashboard>.Success(
            new LessonContentDashboard(scope.School.Id, groups));
    }

    public async Task<LessonContentQueryResult<CanonicalLessonDetail>> GetStaffLessonAsync(
        Guid actorUserId,
        Guid lessonId,
        string cultureCode,
        CancellationToken cancellationToken = default)
    {
        var scope = await ResolveScopeAsync(actorUserId, cancellationToken);
        if (!scope.Succeeded)
            return LessonContentQueryResult<CanonicalLessonDetail>.Failure(scope.Error!.Value);
        if (!LessonContentPolicy.CanReadStaff(scope.Actor!.Roles))
            return LessonContentQueryResult<CanonicalLessonDetail>.Failure(
                LessonContentErrorCode.AccessDenied);

        var contexts = await _lessons.ListStaffAdoptionsAsync(
            scope.School!.Id,
            cancellationToken);
        return await BuildStaffDetailAsync(
            contexts,
            lessonId,
            cultureCode,
            cancellationToken);
    }

    public async Task<LessonContentQueryResult<IReadOnlyList<StudentLessonSummary>>>
        ListPublishedForStudentAsync(
            Guid actorUserId,
            string cultureCode,
            CancellationToken cancellationToken = default)
    {
        var scope = await ResolveScopeAsync(actorUserId, cancellationToken);
        if (!scope.Succeeded)
        {
            return LessonContentQueryResult<IReadOnlyList<StudentLessonSummary>>
                .Failure(scope.Error!.Value);
        }

        if (!LessonContentPolicy.IsStudent(scope.Actor!.Roles))
        {
            return LessonContentQueryResult<IReadOnlyList<StudentLessonSummary>>
                .Failure(LessonContentErrorCode.AccessDenied);
        }

        var contexts = await _lessons.ListStudentAdoptionsAsync(
            actorUserId,
            scope.School!.Id,
            cancellationToken);
        var resolvableContexts = contexts
            .Where(CanResolveContext)
            .ToArray();

        var lessons = await _lessons.ListPedagogicalLessonsAsync(
            resolvableContexts
                .Select(x => x.FrameworkVersionId)
                .Distinct()
                .ToArray(),
            cancellationToken);
        var contents = await _lessons.ListCanonicalContentsAsync(
            lessons.Select(x => x.Id).ToArray(),
            cancellationToken);
        var contentByLesson = contents
            .Where(x => x.Status == CanonicalLessonContentStatus.Published)
            .ToDictionary(x => x.PedagogicalLessonId);

        var result = new Dictionary<Guid, StudentLessonSummary>();
        foreach (var context in resolvableContexts)
        {
            foreach (var lesson in lessons.Where(x =>
                         MatchesContext(x, context) &&
                         LessonContentPolicy.IsCanonicalTarget(
                             x.OfficialOutcomeCount)))
            {
                if (!contentByLesson.TryGetValue(lesson.Id, out var content))
                    continue;

                var translation = SelectAcademicContent(
                    content.Translations,
                    context.FrameworkCode);
                if (translation is null)
                    continue;

                result.TryAdd(
                    lesson.Id,
                    new StudentLessonSummary(
                        lesson.Id,
                        translation.Title,
                        lesson.UnitTitle,
                        context.SubjectName,
                        context.SubjectCode,
                        DisplayLevel(context),
                        context.FrameworkName,
                        lesson.SortOrder,
                        LessonContentPolicy.IsSupporting(
                            lesson.OfficialOutcomeCount)));
            }
        }

        return LessonContentQueryResult<IReadOnlyList<StudentLessonSummary>>.Success(
            result.Values
                .OrderBy(x => x.SubjectCode)
                .ThenBy(x => x.Order)
                .ToArray());
    }

    public async Task<LessonContentQueryResult<StudentLessonDetail>> GetPublishedForStudentAsync(
        Guid actorUserId,
        Guid lessonId,
        string cultureCode,
        CancellationToken cancellationToken = default)
    {
        var scope = await ResolveScopeAsync(actorUserId, cancellationToken);
        if (!scope.Succeeded)
            return LessonContentQueryResult<StudentLessonDetail>.Failure(scope.Error!.Value);
        if (!LessonContentPolicy.IsStudent(scope.Actor!.Roles))
            return LessonContentQueryResult<StudentLessonDetail>.Failure(
                LessonContentErrorCode.AccessDenied);

        var contexts = await _lessons.ListStudentAdoptionsAsync(
            actorUserId,
            scope.School!.Id,
            cancellationToken);
        var resolvableContexts = contexts
            .Where(CanResolveContext)
            .ToArray();
        var lessons = await _lessons.ListPedagogicalLessonsAsync(
            resolvableContexts
                .Select(x => x.FrameworkVersionId)
                .Distinct()
                .ToArray(),
            cancellationToken);
        var lesson = lessons.SingleOrDefault(x => x.Id == lessonId);
        if (lesson is null ||
            !LessonContentPolicy.IsCanonicalTarget(lesson.OfficialOutcomeCount))
        {
            return LessonContentQueryResult<StudentLessonDetail>.Failure(
                LessonContentErrorCode.LessonNotFound);
        }

        var context = resolvableContexts.FirstOrDefault(x =>
            MatchesContext(lesson, x));
        if (context is null)
        {
            return LessonContentQueryResult<StudentLessonDetail>.Failure(
                LessonContentErrorCode.LessonNotFound);
        }

        var content = (await _lessons.ListCanonicalContentsAsync(
                [lessonId],
                cancellationToken))
            .SingleOrDefault();
        if (content is null ||
            !LessonContentPolicy.CanExposeCanonicalBody(content.Status))
        {
            return LessonContentQueryResult<StudentLessonDetail>.Failure(
                LessonContentErrorCode.LessonNotFound);
        }

        var translation = SelectAcademicContent(
            content.Translations,
            context.FrameworkCode);
        if (translation is null)
        {
            return LessonContentQueryResult<StudentLessonDetail>.Failure(
                LessonContentErrorCode.LessonNotFound);
        }

        var outcomes = await _lessons.ListOfficialOutcomesAsync(
            lesson.FrameworkVersionId,
            lesson.Id,
            cancellationToken);

        return LessonContentQueryResult<StudentLessonDetail>.Success(
            new StudentLessonDetail(
                lesson.Id,
                translation.Title,
                lesson.UnitTitle,
                context.SubjectName,
                context.SubjectCode,
                DisplayLevel(context),
                context.FrameworkName,
                translation.Explanation,
                translation.KeyConceptsAndRules,
                translation.WorkedExamples,
                translation.StepByStepSolutions,
                translation.CommonMistakes,
                translation.QuickSummary,
                outcomes,
                content.PublishedAtUtc ?? content.UpdatedAtUtc,
                LessonContentPolicy.IsSupporting(lesson.OfficialOutcomeCount)));
    }

    private async Task<LessonContentQueryResult<CanonicalLessonDetail>> BuildStaffDetailAsync(
        IReadOnlyList<CanonicalCurriculumContextRecord> contexts,
        Guid lessonId,
        string cultureCode,
        CancellationToken cancellationToken)
    {
        var resolvableContexts = contexts
            .Where(CanResolveContext)
            .ToArray();
        var lessons = await _lessons.ListPedagogicalLessonsAsync(
            resolvableContexts
                .Select(x => x.FrameworkVersionId)
                .Distinct()
                .ToArray(),
            cancellationToken);
        var lesson = lessons.SingleOrDefault(x => x.Id == lessonId);
        if (lesson is null)
        {
            return LessonContentQueryResult<CanonicalLessonDetail>.Failure(
                LessonContentErrorCode.LessonNotFound);
        }

        var context = resolvableContexts.FirstOrDefault(x =>
            MatchesContext(lesson, x));
        if (context is null)
        {
            return LessonContentQueryResult<CanonicalLessonDetail>.Failure(
                LessonContentErrorCode.LessonNotFound);
        }

        var content = (await _lessons.ListCanonicalContentsAsync(
                [lessonId],
                cancellationToken))
            .SingleOrDefault();
        CanonicalLessonTranslationRecord? body = null;
        if (content is not null &&
            LessonContentPolicy.CanExposeCanonicalBody(content.Status))
        {
            body = SelectAcademicContent(
                content.Translations,
                context.FrameworkCode);
        }

        var outcomes = await _lessons.ListOfficialOutcomesAsync(
            lesson.FrameworkVersionId,
            lesson.Id,
            cancellationToken);

        return LessonContentQueryResult<CanonicalLessonDetail>.Success(
            new CanonicalLessonDetail(
                lesson.Id,
                lesson.Code,
                lesson.Title,
                lesson.UnitTitle,
                context.FrameworkName,
                context.FrameworkVersionName,
                context.SubjectName,
                context.SubjectCode,
                DisplayLevel(context),
                content?.Status,
                content?.PublishedAtUtc,
                body,
                outcomes));
    }

    private async Task<ScopeResult> ResolveScopeAsync(
        Guid actorUserId,
        CancellationToken cancellationToken)
    {
        var actor = await _users.GetActorAsync(actorUserId, cancellationToken);
        if (actor is null ||
            !actor.IsActive ||
            actor.IsLocked ||
            !actor.SchoolId.HasValue)
        {
            return ScopeResult.Fail(LessonContentErrorCode.AccessDenied);
        }

        var school = await _schools.GetByIdAsync(
            actor.SchoolId.Value,
            cancellationToken);
        if (school is null || school.Status != SchoolStatus.Active)
            return ScopeResult.Fail(LessonContentErrorCode.SchoolNotActive);

        return ScopeResult.Success(actor, school);
    }

    private static bool CanResolveContext(
        CanonicalCurriculumContextRecord context) =>
        TryResolveContextIdentity(context, out _, out _);

    private static bool MatchesContext(
        PedagogicalLessonRecord lesson,
        CanonicalCurriculumContextRecord context)
    {
        if (lesson.FrameworkVersionId != context.FrameworkVersionId)
            return false;

        if (!TryResolveContextIdentity(
                context,
                out var logicalLevel,
                out var pathway))
        {
            return false;
        }

        return InLogicalLevel(lesson, logicalLevel) &&
               PathwayMatches(lesson.Pathway, pathway);
    }

    private static bool InLogicalLevel(
        PedagogicalLessonRecord lesson,
        int logicalLevel) =>
        lesson.LogicalLevelFrom <= logicalLevel &&
        logicalLevel <= lesson.LogicalLevelTo;

    private static bool PathwayMatches(
        string? lessonPathway,
        string? contextPathway)
    {
        var lessonIsShared = string.IsNullOrWhiteSpace(lessonPathway);
        var contextIsShared = string.IsNullOrWhiteSpace(contextPathway);

        if (contextIsShared)
            return lessonIsShared;

        return !lessonIsShared &&
               string.Equals(
                   lessonPathway!.Trim(),
                   contextPathway!.Trim(),
                   StringComparison.OrdinalIgnoreCase);
    }

    private static bool TryResolveContextIdentity(
        CanonicalCurriculumContextRecord context,
        out int logicalLevel,
        out string? pathway)
    {
        if (context.CurriculumLogicalLevel.HasValue)
        {
            logicalLevel = context.CurriculumLogicalLevel.Value;
            pathway = context.CurriculumPathway;
            return logicalLevel is >= 1 and <= 13;
        }

        var legacy = CurriculumLevelIdentityRegistry.ResolveLegacy(
            context.FrameworkCode,
            context.GradeName,
            context.GradeOrder);
        if (legacy is null)
        {
            logicalLevel = 0;
            pathway = null;
            return false;
        }

        logicalLevel = legacy.LogicalLevel;
        pathway = legacy.Pathway;
        return true;
    }

    private static string DisplayLevel(
        CanonicalCurriculumContextRecord context) =>
        !string.IsNullOrWhiteSpace(context.CurriculumLevelLabel)
            ? context.CurriculumLevelLabel
            : context.GradeName;

    private static CanonicalLessonTranslationRecord? SelectAcademicContent(
        IReadOnlyList<CanonicalLessonTranslationRecord> translations,
        string frameworkCode)
    {
        var academicLanguage = MathematicsCurriculumPackRegistry.All
            .Single(x => string.Equals(
                x.Code,
                frameworkCode,
                StringComparison.Ordinal))
            .AcademicLanguage;

        return translations.FirstOrDefault(x =>
            NormalizeCulture(x.CultureCode) == NormalizeCulture(academicLanguage));
    }

    private static string NormalizeCulture(string cultureCode)
    {
        if (string.IsNullOrWhiteSpace(cultureCode))
            return "en";

        var value = cultureCode.Trim();
        var separator = value.IndexOf('-');
        return (separator > 0 ? value[..separator] : value).ToLowerInvariant();
    }

    private sealed record ScopeResult(
        SchoolUserRecord? Actor,
        School? School,
        LessonContentErrorCode? Error)
    {
        public bool Succeeded =>
            Actor is not null &&
            School is not null &&
            Error is null;

        public static ScopeResult Success(
            SchoolUserRecord actor,
            School school) =>
            new(actor, school, null);

        public static ScopeResult Fail(LessonContentErrorCode error) =>
            new(null, null, error);
    }
}

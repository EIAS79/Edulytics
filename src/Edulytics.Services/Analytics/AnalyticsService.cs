using Edulytics.Core.Analytics;
using Edulytics.Core.Constants;
using Edulytics.Core.Entities;
using Edulytics.Core.Enums;
using Edulytics.Core.Interfaces;
using Edulytics.Core.Users;

namespace Edulytics.Services.Analytics;

public sealed class AnalyticsService : IAnalyticsService
{
    private readonly IAnalyticsRepository _analytics;
    private readonly ISchoolRepository _schools;
    private readonly ISchoolUserRepository _users;
    private readonly AnalyticsProjectionBuilder _builder;
    private readonly LearningEvaluationEngine _evaluation;
    private readonly ISubjectSupervisorAssignmentRepository?
        _subjectSupervisors;

    public AnalyticsService(
        IAnalyticsRepository analytics,
        ISchoolRepository schools,
        ISchoolUserRepository users,
        AnalyticsProjectionBuilder builder,
        ISubjectSupervisorAssignmentRepository?
            subjectSupervisors = null,
        LearningEvaluationEngine? evaluation = null)
    {
        _analytics = analytics;
        _schools = schools;
        _users = users;
        _builder = builder;
        _subjectSupervisors = subjectSupervisors;
        _evaluation = evaluation ??
            new LearningEvaluationEngine(
                new EvaluationEvidenceNormalizer());
    }

    public async Task<AnalyticsCommandResult> RecalculateAsync(
        Guid actorUserId,
        CancellationToken cancellationToken = default)
    {
        var scope = await ResolveScopeAsync(
            actorUserId,
            cancellationToken);

        if (!scope.Succeeded)
        {
            return AnalyticsCommandResult.Failure(
                scope.Error!.Value);
        }

        if (scope.Role != RoleNames.SubjectSupervisor)
        {
            return AnalyticsCommandResult.Failure(
                AnalyticsErrorCode
                    .RecalculationRequiresSchoolAdmin);
        }

        try
        {
            var source =
                await _analytics.GetSourceSnapshotAsync(
                    scope.School!.Id,
                    cancellationToken);

            var projections = _builder.Build(
                source,
                DateTime.UtcNow);

            var saved =
                await _analytics.ReplaceProjectionsAsync(
                    scope.School.Id,
                    projections,
                    cancellationToken);

            return saved.Succeeded
                ? AnalyticsCommandResult.Success()
                : AnalyticsCommandResult.Failure(
                    AnalyticsErrorCode.PersistenceError);
        }
        catch (InvalidOperationException)
        {
            return AnalyticsCommandResult.Failure(
                AnalyticsErrorCode.InvalidSourceData);
        }
    }

    public async Task<AnalyticsQueryResult<AnalyticsDashboard>>
        GetDashboardAsync(
            Guid actorUserId,
            Guid? academicYearId = null,
            Guid? classGroupId = null,
            Guid? subjectId = null,
            CancellationToken cancellationToken = default)
    {
        var scope = await ResolveScopeAsync(
            actorUserId,
            cancellationToken);

        if (!scope.Succeeded)
        {
            return AnalyticsQueryResult<AnalyticsDashboard>
                .Failure(scope.Error!.Value);
        }

        var schoolId = scope.School!.Id;

        var projection =
            await _analytics.GetProjectionSnapshotAsync(
                schoolId,
                cancellationToken);

        var supervisorSubjectIds =
            scope.SupervisedSubjectIds;

        var pairSet = scope.Role == RoleNames.Teacher
            ? projection.TeacherAssignments
                .Where(
                    x =>
                        x.TeacherUserId ==
                        actorUserId)
                .Select(
                    x =>
                        (
                            x.AcademicYearId,
                            x.ClassGroupId,
                            x.SubjectId
                        ))
                .ToHashSet()
            : [];

        bool PairAllowed(
            Guid yearId,
            Guid classId,
            Guid subject)
        {
            if (scope.Role == RoleNames.SchoolAdmin)
                return true;

            if (scope.Role ==
                RoleNames.SubjectSupervisor)
            {
                return supervisorSubjectIds.Contains(
                    subject);
            }

            return pairSet.Contains(
                (
                    yearId,
                    classId,
                    subject
                ));
        }

        var visibleClasses = projection.ClassGroups
            .Where(x => x.Status == AcademicStructureStatus.Active)
            .Where(
                x =>
                    scope.Role == RoleNames.SchoolAdmin ||
                    scope.Role == RoleNames.SubjectSupervisor ||
                    pairSet.Any(
                        pair =>
                            pair.ClassGroupId == x.Id &&
                            pair.AcademicYearId ==
                            x.AcademicYearId))
            .OrderBy(x => x.Name)
            .ToArray();

        var visibleSubjects = projection.Subjects
            .Where(x => x.Status == AcademicStructureStatus.Active)
            .Where(
                x =>
                    scope.Role == RoleNames.SchoolAdmin ||
                    (scope.Role ==
                        RoleNames.SubjectSupervisor &&
                     supervisorSubjectIds.Contains(x.Id)) ||
                    (scope.Role == RoleNames.Teacher &&
                     pairSet.Any(
                        pair =>
                            pair.SubjectId == x.Id)))
            .OrderBy(x => x.Name)
            .ToArray();

        var visibleYearIds =
            scope.Role == RoleNames.SchoolAdmin ||
            scope.Role == RoleNames.SubjectSupervisor
                ? projection.AcademicYears
                    .Select(x => x.Id)
                    .ToHashSet()
                : pairSet
                    .Select(x => x.AcademicYearId)
                    .ToHashSet();

        var visibleYears = projection.AcademicYears
            .Where(x => visibleYearIds.Contains(x.Id))
            .OrderByDescending(x => x.StartsOn)
            .ToArray();

        if (academicYearId.HasValue &&
            !visibleYearIds.Contains(
                academicYearId.Value))
        {
            return AnalyticsQueryResult<AnalyticsDashboard>
                .Failure(
                    AnalyticsErrorCode.AccessDenied);
        }

        if (classGroupId.HasValue &&
            !visibleClasses.Any(
                x => x.Id == classGroupId.Value))
        {
            return AnalyticsQueryResult<AnalyticsDashboard>
                .Failure(
                    AnalyticsErrorCode.AccessDenied);
        }

        if (subjectId.HasValue &&
            !visibleSubjects.Any(
                x => x.Id == subjectId.Value))
        {
            return AnalyticsQueryResult<AnalyticsDashboard>
                .Failure(
                    AnalyticsErrorCode.AccessDenied);
        }

        if (scope.Role == RoleNames.Teacher &&
            classGroupId.HasValue &&
            subjectId.HasValue)
        {
            var selectedClass =
                visibleClasses.First(
                    x =>
                        x.Id ==
                        classGroupId.Value);

            if (!pairSet.Contains(
                    (
                        selectedClass.AcademicYearId,
                        classGroupId.Value,
                        subjectId.Value
                    )))
            {
                return AnalyticsQueryResult<AnalyticsDashboard>
                    .Failure(
                        AnalyticsErrorCode.AccessDenied);
            }
        }

        bool Matches(
            Guid yearId,
            Guid classId,
            Guid subject)
        {
            if (!PairAllowed(
                    yearId,
                    classId,
                    subject))
            {
                return false;
            }

            if (academicYearId.HasValue &&
                yearId != academicYearId.Value)
            {
                return false;
            }

            if (classGroupId.HasValue &&
                classId != classGroupId.Value)
            {
                return false;
            }

            if (subjectId.HasValue &&
                subject != subjectId.Value)
            {
                return false;
            }

            return true;
        }

        var masteries =
            projection.StudentOutcomeMasteries
                .Where(
                    x =>
                        Matches(
                            x.AcademicYearId,
                            x.ClassGroupId,
                            x.SubjectId))
                .ToArray();

        var classOutcomes =
            projection.ClassOutcomeSummaries
                .Where(
                    x =>
                        Matches(
                            x.AcademicYearId,
                            x.ClassGroupId,
                            x.SubjectId))
                .ToArray();

        var classTopics =
            projection.ClassTopicSummaries
                .Where(
                    x =>
                        Matches(
                            x.AcademicYearId,
                            x.ClassGroupId,
                            x.SubjectId))
                .ToArray();

        var trends =
            projection.ClassAssessmentTrends
                .Where(
                    x =>
                        Matches(
                            x.AcademicYearId,
                            x.ClassGroupId,
                            x.SubjectId))
                .ToArray();

        var years = projection.AcademicYears
            .ToDictionary(x => x.Id);

        var classes = projection.ClassGroups
            .ToDictionary(x => x.Id);

        var subjects = projection.Subjects
            .ToDictionary(x => x.Id);

        var students = projection.StudentProfiles
            .ToDictionary(x => x.Id);

        var outcomes = projection.LearningOutcomes
            .ToDictionary(x => x.Id);

        var topics = projection.CurriculumTopics
            .ToDictionary(x => x.Id);

        string YearName(Guid id) =>
            years.TryGetValue(id, out var value)
                ? value.Name
                : string.Empty;

        string ClassName(Guid id) =>
            classes.TryGetValue(id, out var value)
                ? value.Name
                : string.Empty;

        string SubjectName(Guid id) =>
            subjects.TryGetValue(id, out var value)
                ? value.Name
                : string.Empty;

        var outcomeItems = classOutcomes
            .Where(
                x =>
                    outcomes.ContainsKey(
                        x.LearningOutcomeId))
            .Select(x =>
            {
                var outcome =
                    outcomes[x.LearningOutcomeId];

                var displayCode =
                    AnalyticsPresentationFormatter.OutcomeLabel(outcome.Code);
                var displayDescription =
                    AnalyticsPresentationFormatter.OutcomeDescription(
                        outcome.Description,
                        displayCode,
                        SubjectName(x.SubjectId));

                return new AnalyticsOutcomeItem(
                    x.AcademicYearId,
                    YearName(x.AcademicYearId),
                    x.ClassGroupId,
                    ClassName(x.ClassGroupId),
                    x.SubjectId,
                    SubjectName(x.SubjectId),
                    x.LearningOutcomeId,
                    displayCode,
                    displayDescription,
                    x.AverageMasteryPercentage,
                    x.StudentCount,
                    x.AtRiskStudentCount,
                    x.EvidenceCount,
                    AnalyticsProjectionBuilder.BandFor(
                        x.AverageMasteryPercentage));
            })
            .OrderBy(x => x.ClassName)
            .ThenBy(x => x.SubjectName)
            .ThenBy(x => x.OutcomeCode)
            .ToArray();

        var topicItems = classTopics
            .Where(
                x =>
                    topics.ContainsKey(
                        x.CurriculumTopicId))
            .Select(x =>
            {
                var topic =
                    topics[x.CurriculumTopicId];

                return new AnalyticsTopicItem(
                    x.AcademicYearId,
                    YearName(x.AcademicYearId),
                    x.ClassGroupId,
                    ClassName(x.ClassGroupId),
                    x.SubjectId,
                    SubjectName(x.SubjectId),
                    x.CurriculumTopicId,
                    topic.Name,
                    x.MasteryPercentage,
                    x.OutcomeCount,
                    x.WeakOutcomeCount,
                    x.StudentCount,
                    AnalyticsProjectionBuilder.BandFor(
                        x.MasteryPercentage));
            })
            .OrderBy(x => x.MasteryPercentage)
            .ThenBy(x => x.TopicName)
            .ToArray();

        var visibleAssessments = projection.Assessments
            .Where(x =>
                x.Status != AssessmentStatus.Draft &&
                Matches(
                    x.AcademicYearId,
                    x.ClassGroupId,
                    x.SubjectId))
            .ToDictionary(x => x.Id);
        var visibleResults = projection.AssessmentResults
            .Where(x => visibleAssessments.ContainsKey(x.AssessmentId))
            .ToDictionary(x => x.Id);
        var visibleQuestions = projection.AssessmentQuestions
            .Where(x => visibleAssessments.ContainsKey(x.AssessmentId))
            .ToDictionary(x => x.Id);
        var lessonItemsByQuestion = projection.AssessmentItems
            .Where(x => x.CurriculumPedagogicalLessonId.HasValue)
            .Where(x => visibleQuestions.ContainsKey(x.Id))
            .ToDictionary(x => x.Id);
        var pedagogicalLessons = projection.PedagogicalLessons
            .ToDictionary(x => x.Id);

        var lessonEvidenceRows = new List<LessonEvidenceRow>();
        foreach (var answer in projection.StudentAnswers)
        {
            if (!visibleResults.TryGetValue(answer.AssessmentResultId, out var result) ||
                !visibleAssessments.TryGetValue(result.AssessmentId, out var assessment) ||
                !visibleQuestions.TryGetValue(answer.AssessmentQuestionId, out var question) ||
                question.AssessmentId != assessment.Id ||
                !lessonItemsByQuestion.TryGetValue(question.Id, out var item) ||
                !item.CurriculumPedagogicalLessonId.HasValue ||
                !pedagogicalLessons.ContainsKey(item.CurriculumPedagogicalLessonId.Value) ||
                question.MaxScore <= 0m)
            {
                continue;
            }

            lessonEvidenceRows.Add(new LessonEvidenceRow(
                assessment.AcademicYearId,
                assessment.ClassGroupId,
                assessment.SubjectId,
                item.CurriculumPedagogicalLessonId.Value,
                assessment.Id,
                result.StudentProfileId,
                answer.Score,
                question.MaxScore));
        }

        var lessonItems = lessonEvidenceRows
            .GroupBy(x => new
            {
                x.AcademicYearId,
                x.ClassGroupId,
                x.SubjectId,
                x.LessonId
            })
            .Select(group =>
            {
                var lesson = pedagogicalLessons[group.Key.LessonId];
                var earned = group.Sum(x => x.EarnedScore);
                var possible = group.Sum(x => x.PossibleScore);
                var mastery = possible <= 0m
                    ? 0m
                    : decimal.Round(
                        earned / possible * 100m,
                        2,
                        MidpointRounding.AwayFromZero);
                var atRisk = group
                    .GroupBy(x => x.StudentProfileId)
                    .Count(student =>
                    {
                        var studentPossible = student.Sum(x => x.PossibleScore);
                        if (studentPossible <= 0m)
                            return false;
                        var studentMastery = decimal.Round(
                            student.Sum(x => x.EarnedScore) /
                            studentPossible *
                            100m,
                            2,
                            MidpointRounding.AwayFromZero);
                        return studentMastery < 60m;
                    });

                return new AnalyticsLessonItem(
                    group.Key.AcademicYearId,
                    YearName(group.Key.AcademicYearId),
                    group.Key.ClassGroupId,
                    ClassName(group.Key.ClassGroupId),
                    group.Key.SubjectId,
                    SubjectName(group.Key.SubjectId),
                    lesson.Id,
                    lesson.Title,
                    lesson.UnitKey,
                    lesson.UnitTitle,
                    mastery,
                    group.Select(x => x.StudentProfileId).Distinct().Count(),
                    atRisk,
                    group.Count(),
                    group.Select(x => x.AssessmentId).Distinct().Count(),
                    AnalyticsProjectionBuilder.BandFor(mastery));
            })
            .OrderBy(x => x.MasteryPercentage)
            .ThenBy(x => x.UnitTitle)
            .ThenBy(x => x.LessonTitle)
            .ToArray();

        var trendItems = trends
            .Select(
                x =>
                    new AnalyticsTrendItem(
                        x.AcademicYearId,
                        YearName(x.AcademicYearId),
                        x.ClassGroupId,
                        ClassName(x.ClassGroupId),
                        x.SubjectId,
                        SubjectName(x.SubjectId),
                        x.AssessmentId,
                        x.AssessmentTitle,
                        x.AssessmentDate,
                        x.AveragePercentage,
                        x.StudentCount,
                        x.AtRiskStudentCount,
                        AnalyticsProjectionBuilder.BandFor(
                            x.AveragePercentage)))
            .OrderBy(x => x.AssessmentDate)
            .ThenBy(x => x.AssessmentTitle)
            .ToArray();

        var riskItems = masteries
            .GroupBy(
                x => new
                {
                    x.StudentProfileId,
                    x.AcademicYearId,
                    x.ClassGroupId
                })
            .Select(group =>
            {
                var earned =
                    group.Sum(x => x.EarnedScore);

                var possible =
                    group.Sum(x => x.PossibleScore);

                var percentage =
                    possible <= 0m
                        ? 0m
                        : decimal.Round(
                            earned /
                            possible *
                            100m,
                            2,
                            MidpointRounding
                                .AwayFromZero);

                if (!students.TryGetValue(
                        group.Key.StudentProfileId,
                        out var student))
                {
                    return null;
                }

                return new AnalyticsRiskStudentItem(
                    student.Id,
                    student.StudentNumber,
                    student.DisplayName,
                    group.Key.AcademicYearId,
                    YearName(
                        group.Key.AcademicYearId),
                    group.Key.ClassGroupId,
                    ClassName(
                        group.Key.ClassGroupId),
                    percentage,
                    group.Count(
                        x =>
                            x.MasteryPercentage <
                            40m),
                    AnalyticsProjectionBuilder
                        .BandFor(percentage));
            })
            .Where(x => x is not null)
            .Cast<AnalyticsRiskStudentItem>()
            .Where(x => x.MasteryPercentage < 60m)
            .OrderBy(x => x.MasteryPercentage)
            .ThenBy(x => x.DisplayName)
            .ToArray();

        var earnedTotal =
            masteries.Sum(x => x.EarnedScore);

        var possibleTotal =
            masteries.Sum(x => x.PossibleScore);

        var overall =
            possibleTotal <= 0m
                ? 0m
                : decimal.Round(
                    earnedTotal /
                    possibleTotal *
                    100m,
                    2,
                    MidpointRounding
                        .AwayFromZero);

        var latestProjection =
            projection.SchoolSnapshots
                .Select(
                    x =>
                        (DateTime?)
                        x.CalculatedAtUtc)
                .Concat(
                    projection
                        .StudentOutcomeMasteries
                        .Select(
                            x =>
                                (DateTime?)
                                x.CalculatedAtUtc))
                .Where(x => x.HasValue)
                .Select(x => x!.Value)
                .DefaultIfEmpty()
                .Max();

        DateTime? generatedAt =
            latestProjection == default
                ? null
                : latestProjection;

        var latestSource =
            await _analytics
                .GetLatestSourceUpdateAsync(
                    schoolId,
                    cancellationToken);

        var isStale =
            latestSource.HasValue &&
            (!generatedAt.HasValue ||
             latestSource.Value >
             generatedAt.Value);

        var dashboard =
            new AnalyticsDashboard(
                masteries.Length > 0 || lessonItems.Length > 0 || trendItems.Length > 0,
                isStale,
                scope.Role ==
                RoleNames.SubjectSupervisor,
                generatedAt,
                overall,
                masteries
                    .Select(
                        x =>
                            x.StudentProfileId)
                    .Distinct()
                    .Count(),
                riskItems
                    .Select(
                        x =>
                            x.StudentProfileId)
                    .Distinct()
                    .Count(),
                classOutcomes.Count(
                    x =>
                        x.AverageMasteryPercentage <
                        40m),
                classTopics.Count(
                    x =>
                        x.MasteryPercentage <
                        60m),
                academicYearId,
                classGroupId,
                subjectId,
                visibleYears
                    .Select(
                        x =>
                            new AnalyticsFilterItem(
                                x.Id,
                                x.Name))
                    .ToArray(),
                visibleClasses
                    .Select(
                        x =>
                            new AnalyticsFilterItem(
                                x.Id,
                                AnalyticsPresentationFormatter.ClassFilterLabel(x.Name)))
                    .ToArray(),
                visibleSubjects
                    .Select(
                        x =>
                            new AnalyticsFilterItem(
                                x.Id,
                                $"{x.Name} ({x.Code})"))
                    .ToArray(),
                outcomeItems,
                topicItems,
                trendItems,
                riskItems)
            {
                Lessons = lessonItems
            };

        return AnalyticsQueryResult<AnalyticsDashboard>
            .Success(dashboard);
    }

    public async Task<AnalyticsQueryResult<AnalyticsStudentEvaluationPage>>
        GetStudentEvaluationAsync(
            Guid actorUserId,
            Guid studentProfileId,
            Guid academicYearId,
            Guid classGroupId,
            Guid subjectId,
            CancellationToken cancellationToken = default)
    {
        var access = await GetDashboardAsync(
            actorUserId,
            academicYearId,
            classGroupId,
            subjectId,
            cancellationToken);

        if (access.Value is null)
        {
            return AnalyticsQueryResult<AnalyticsStudentEvaluationPage>.Failure(
                access.Error ?? AnalyticsErrorCode.AccessDenied);
        }

        var scope = await ResolveScopeAsync(
            actorUserId,
            cancellationToken);

        if (!scope.Succeeded)
        {
            return AnalyticsQueryResult<AnalyticsStudentEvaluationPage>.Failure(
                scope.Error!.Value);
        }

        var projection = await _analytics.GetProjectionSnapshotAsync(
            scope.School!.Id,
            cancellationToken);

        var student = projection.StudentProfiles.SingleOrDefault(
            x =>
                x.Id == studentProfileId &&
                !x.IsArchived &&
                x.Status == AcademicStructureStatus.Active);

        if (student is null ||
            !projection.StudentEnrollments.Any(
                x =>
                    x.StudentProfileId == studentProfileId &&
                    x.AcademicYearId == academicYearId &&
                    x.ClassGroupId == classGroupId))
        {
            return AnalyticsQueryResult<AnalyticsStudentEvaluationPage>.Failure(
                AnalyticsErrorCode.AccessDenied);
        }

        try
        {
            var now = DateTime.UtcNow;
            var normalized =
                _evaluation.NormalizeOfficialEvidence(projection);
            var evaluation =
                _evaluation.BuildStudentSubject(
                    projection,
                    normalized,
                    studentProfileId,
                    academicYearId,
                    classGroupId,
                    subjectId,
                    now);

            var topics = projection.CurriculumTopics
                .ToDictionary(x => x.Id);
            var topicRows = evaluation.Skills
                .GroupBy(x => x.TopicId)
                .Select(group =>
                {
                    var rows = group.ToArray();
                    var evaluated = rows
                        .Where(x => x.CurrentMasteryPercentage.HasValue)
                        .ToArray();

                    return new AnalyticsStudentEvaluationTopic(
                        group.Key,
                        topics.TryGetValue(group.Key, out var topic)
                            ? topic.Name
                            : string.Empty,
                        AverageNullable(
                            evaluated.Select(
                                x => x.CurrentMasteryPercentage)),
                        AverageNullable(
                            evaluated.Select(
                                x => x.AssessmentMasteryPercentage)),
                        AverageNullable(
                            evaluated.Select(
                                x => x.PracticeMasteryPercentage)),
                        rows.Length == 0
                            ? 0m
                            : Round2(
                                evaluated.Length /
                                (decimal)rows.Length *
                                100m),
                        rows.Length,
                        evaluated.Length,
                        rows.Count(
                            x =>
                                x.Status ==
                                EvaluationSkillStatus.Critical),
                        rows.Count(
                            x =>
                                x.Status ==
                                EvaluationSkillStatus.NeedsFocus),
                        AggregateTrend(
                            rows.Select(
                                x => x.ShortTermTrend)));
                })
                .OrderBy(x => x.CurrentMasteryPercentage ?? 101m)
                .ThenBy(x => x.TopicName)
                .ToArray();

            var termNames = projection.Terms
                .ToDictionary(x => x.Id, x => x.Name);
            var assessments = projection.Assessments
                .Where(x =>
                    x.Status != AssessmentStatus.Draft &&
                    x.AcademicYearId == academicYearId &&
                    x.ClassGroupId == classGroupId &&
                    x.SubjectId == subjectId)
                .ToDictionary(x => x.Id);
            var assessmentRows = projection.AssessmentResults
                .Where(x =>
                    x.StudentProfileId == studentProfileId &&
                    assessments.ContainsKey(x.AssessmentId))
                .Select(x =>
                {
                    var assessment = assessments[x.AssessmentId];

                    return new AnalyticsStudentAssessmentEvaluationItem(
                        assessment.Id,
                        assessment.Title,
                        assessment.AssessmentDate,
                        termNames.GetValueOrDefault(
                            assessment.TermId),
                        x.Percentage);
                })
                .OrderBy(x => x.AssessmentDate)
                .ThenBy(x => x.Title)
                .ToArray();

            var evidence = normalized
                .Where(x =>
                    x.StudentProfileId == studentProfileId &&
                    x.AcademicYearId == academicYearId &&
                    x.ClassGroupId == classGroupId &&
                    x.SubjectId == subjectId)
                .OrderByDescending(x => x.OccurredAtUtc)
                .ThenBy(x => x.SkillName)
                .ToArray();

            return AnalyticsQueryResult<AnalyticsStudentEvaluationPage>.Success(
                new AnalyticsStudentEvaluationPage(
                    evaluation,
                    topicRows,
                    assessmentRows,
                    evidence));
        }
        catch (InvalidOperationException)
        {
            return AnalyticsQueryResult<AnalyticsStudentEvaluationPage>.Failure(
                AnalyticsErrorCode.InvalidSourceData);
        }
    }

    public async Task<AnalyticsQueryResult<AnalyticsStudentsEvaluationPage>>
        GetStudentsEvaluationAsync(
            Guid actorUserId,
            Guid academicYearId,
            Guid classGroupId,
            Guid subjectId,
            CancellationToken cancellationToken = default)
    {
        var access = await GetDashboardAsync(
            actorUserId,
            academicYearId,
            classGroupId,
            subjectId,
            cancellationToken);

        if (access.Value is null)
        {
            return AnalyticsQueryResult<AnalyticsStudentsEvaluationPage>.Failure(
                access.Error ?? AnalyticsErrorCode.AccessDenied);
        }

        var scope = await ResolveScopeAsync(
            actorUserId,
            cancellationToken);

        if (!scope.Succeeded)
        {
            return AnalyticsQueryResult<AnalyticsStudentsEvaluationPage>.Failure(
                scope.Error!.Value);
        }

        var projection = await _analytics.GetProjectionSnapshotAsync(
            scope.School!.Id,
            cancellationToken);
        var year = projection.AcademicYears.SingleOrDefault(
            x => x.Id == academicYearId);
        var classGroup = projection.ClassGroups.SingleOrDefault(
            x =>
                x.Id == classGroupId &&
                x.AcademicYearId == academicYearId);
        var subject = projection.Subjects.SingleOrDefault(
            x => x.Id == subjectId);

        if (year is null ||
            classGroup is null ||
            subject is null)
        {
            return AnalyticsQueryResult<AnalyticsStudentsEvaluationPage>.Failure(
                AnalyticsErrorCode.InvalidSourceData);
        }

        try
        {
            var now = DateTime.UtcNow;
            var normalized =
                _evaluation.NormalizeOfficialEvidence(projection);
            var enrolledIds = projection.StudentEnrollments
                .Where(x =>
                    x.AcademicYearId == academicYearId &&
                    x.ClassGroupId == classGroupId)
                .Select(x => x.StudentProfileId)
                .ToHashSet();
            var students = projection.StudentProfiles
                .Where(x =>
                    enrolledIds.Contains(x.Id) &&
                    !x.IsArchived &&
                    x.Status == AcademicStructureStatus.Active)
                .OrderBy(x => x.DisplayName)
                .ThenBy(x => x.StudentNumber)
                .ToArray();

            var evaluations = students
                .Select(student =>
                    _evaluation.BuildStudentSubject(
                        projection,
                        normalized,
                        student.Id,
                        academicYearId,
                        classGroupId,
                        subjectId,
                        now))
                .ToArray();

            var rows = evaluations
                .Select(evaluation =>
                    new AnalyticsStudentEvaluationRow(
                        evaluation.StudentProfileId,
                        evaluation.StudentNumber,
                        evaluation.DisplayName,
                        evaluation.CurrentMasteryPercentage,
                        evaluation.AssessmentMasteryPercentage,
                        evaluation.PracticeMasteryPercentage,
                        evaluation.PracticeToAssessmentGapPercentagePoints,
                        evaluation.CurriculumCoveragePercentage,
                        evaluation.ConfidencePercentage,
                        evaluation.ConfidenceBand,
                        evaluation.ShortTermTrend,
                        evaluation.LongTermTrend,
                        evaluation.SecureSkillCount,
                        evaluation.NeedsFocusSkillCount,
                        evaluation.CriticalSkillCount,
                        evaluation.RetentionConcernCount,
                        evaluation.Skills.Count == 0
                            ? EvaluationPriority.None
                            : evaluation.Skills.Max(
                                x => x.InterventionPriority)))
                .OrderByDescending(
                    x => x.HighestPriority)
                .ThenBy(
                    x => x.CurrentMasteryPercentage ?? 101m)
                .ThenBy(x => x.DisplayName)
                .ToArray();

            var distribution = new AnalyticsEvaluationDistribution(
                rows.Length,
                rows.Count(x =>
                    x.CurrentMasteryPercentage >= 75m &&
                    x.CoveragePercentage > 0m),
                rows.Count(x =>
                    x.CurrentMasteryPercentage is >= 60m and < 75m),
                rows.Count(x =>
                    x.CurrentMasteryPercentage is >= 40m and < 60m),
                rows.Count(x =>
                    x.CurrentMasteryPercentage.HasValue &&
                    x.CurrentMasteryPercentage < 40m),
                rows.Count(x =>
                    !x.CurrentMasteryPercentage.HasValue ||
                    x.CoveragePercentage <= 0m),
                rows.Count(x =>
                    x.ShortTermTrend is
                        EvaluationTrendBand.Improving or
                        EvaluationTrendBand.RapidlyImproving),
                rows.Count(x =>
                    x.ShortTermTrend ==
                        EvaluationTrendBand.Stable),
                rows.Count(x =>
                    x.ShortTermTrend is
                        EvaluationTrendBand.Declining or
                        EvaluationTrendBand.RapidlyDeclining));

            return AnalyticsQueryResult<AnalyticsStudentsEvaluationPage>.Success(
                new AnalyticsStudentsEvaluationPage(
                    academicYearId,
                    year.Name,
                    classGroupId,
                    classGroup.Name,
                    subjectId,
                    subject.Name,
                    distribution,
                    rows));
        }
        catch (InvalidOperationException)
        {
            return AnalyticsQueryResult<AnalyticsStudentsEvaluationPage>.Failure(
                AnalyticsErrorCode.InvalidSourceData);
        }
    }

    public async Task<AnalyticsQueryResult<AnalyticsTopicSkillEvaluationPage>>
        GetTopicSkillEvaluationAsync(
            Guid actorUserId,
            Guid academicYearId,
            Guid classGroupId,
            Guid subjectId,
            CancellationToken cancellationToken = default)
    {
        var access = await GetDashboardAsync(
            actorUserId,
            academicYearId,
            classGroupId,
            subjectId,
            cancellationToken);

        if (access.Value is null)
        {
            return AnalyticsQueryResult<AnalyticsTopicSkillEvaluationPage>.Failure(
                access.Error ?? AnalyticsErrorCode.AccessDenied);
        }

        var scope = await ResolveScopeAsync(
            actorUserId,
            cancellationToken);

        if (!scope.Succeeded)
        {
            return AnalyticsQueryResult<AnalyticsTopicSkillEvaluationPage>.Failure(
                scope.Error!.Value);
        }

        var projection = await _analytics.GetProjectionSnapshotAsync(
            scope.School!.Id,
            cancellationToken);
        var year = projection.AcademicYears.SingleOrDefault(
            x => x.Id == academicYearId);
        var classGroup = projection.ClassGroups.SingleOrDefault(
            x =>
                x.Id == classGroupId &&
                x.AcademicYearId == academicYearId);
        var subject = projection.Subjects.SingleOrDefault(
            x => x.Id == subjectId);

        if (year is null ||
            classGroup is null ||
            subject is null)
        {
            return AnalyticsQueryResult<AnalyticsTopicSkillEvaluationPage>.Failure(
                AnalyticsErrorCode.InvalidSourceData);
        }

        try
        {
            var now = DateTime.UtcNow;
            var normalized =
                _evaluation.NormalizeOfficialEvidence(projection);
            var enrolledIds = projection.StudentEnrollments
                .Where(x =>
                    x.AcademicYearId == academicYearId &&
                    x.ClassGroupId == classGroupId)
                .Select(x => x.StudentProfileId)
                .ToHashSet();
            var studentMap = projection.StudentProfiles
                .Where(x =>
                    enrolledIds.Contains(x.Id) &&
                    !x.IsArchived &&
                    x.Status == AcademicStructureStatus.Active)
                .ToDictionary(x => x.Id);

            var evaluations = studentMap.Values
                .Select(student =>
                    _evaluation.BuildStudentSubject(
                        projection,
                        normalized,
                        student.Id,
                        academicYearId,
                        classGroupId,
                        subjectId,
                        now))
                .ToArray();

            var topics = projection.CurriculumTopics
                .ToDictionary(x => x.Id);

            var skillRows = evaluations
                .SelectMany(evaluation =>
                    evaluation.Skills.Select(skill =>
                        new
                        {
                            evaluation.StudentProfileId,
                            evaluation.StudentNumber,
                            evaluation.DisplayName,
                            Skill = skill
                        }))
                .GroupBy(x =>
                    new
                    {
                        x.Skill.TopicId,
                        x.Skill.SkillKey,
                        x.Skill.SkillName
                    })
                .Select(group =>
                {
                    var members = group.ToArray();
                    var evaluated = members
                        .Where(x =>
                            x.Skill.CurrentMasteryPercentage.HasValue)
                        .ToArray();
                    var studentRows = members
                        .Select(x =>
                            new AnalyticsSkillCohortStudent(
                                x.StudentProfileId,
                                x.StudentNumber,
                                x.DisplayName,
                                x.Skill.CurrentMasteryPercentage,
                                x.Skill.AssessmentMasteryPercentage,
                                x.Skill.PracticeMasteryPercentage,
                                x.Skill.Status,
                                x.Skill.ShortTermTrend,
                                x.Skill.ConfidenceBand,
                                x.Skill.InterventionPriority))
                        .OrderByDescending(x => x.Priority)
                        .ThenBy(x => x.MasteryPercentage ?? 101m)
                        .ThenBy(x => x.DisplayName)
                        .ToArray();

                    return new AnalyticsSkillCohortEvaluation(
                        group.Key.TopicId,
                        topics.GetValueOrDefault(
                            group.Key.TopicId)?.Name ??
                            string.Empty,
                        group.Key.SkillKey,
                        group.Key.SkillName,
                        AverageNullable(
                            evaluated.Select(
                                x => x.Skill.CurrentMasteryPercentage)),
                        AverageNullable(
                            evaluated.Select(
                                x => x.Skill.AssessmentMasteryPercentage)),
                        AverageNullable(
                            evaluated.Select(
                                x => x.Skill.PracticeMasteryPercentage)),
                        members.Length == 0
                            ? 0m
                            : Round2(
                                evaluated.Length /
                                (decimal)members.Length *
                                100m),
                        members.Length,
                        evaluated.Length,
                        members.Count(x =>
                            x.Skill.Status ==
                            EvaluationSkillStatus.Critical),
                        members.Count(x =>
                            x.Skill.Status ==
                            EvaluationSkillStatus.NeedsFocus),
                        AggregateTrend(
                            members.Select(
                                x => x.Skill.ShortTermTrend)),
                        members.Length == 0
                            ? EvaluationPriority.None
                            : members.Max(
                                x => x.Skill.InterventionPriority),
                        studentRows);
                })
                .OrderByDescending(x => x.HighestPriority)
                .ThenBy(x => x.ClassMasteryPercentage ?? 101m)
                .ThenBy(x => x.SkillName)
                .ToArray();

            var topicRows = skillRows
                .GroupBy(x => x.TopicId)
                .Select(group =>
                {
                    var rows = group.ToArray();
                    var affectedStudentIds = rows
                        .SelectMany(x => x.Students)
                        .Where(x =>
                            x.Status is
                                EvaluationSkillStatus.Critical or
                                EvaluationSkillStatus.NeedsFocus)
                        .Select(x => x.StudentProfileId)
                        .Distinct()
                        .Count();

                    return new AnalyticsTopicEvaluation(
                        group.Key,
                        topics.GetValueOrDefault(group.Key)?.Name ??
                            string.Empty,
                        AverageNullable(
                            rows.Select(
                                x => x.ClassMasteryPercentage)),
                        AverageNullable(
                            rows.Select(
                                x => x.AssessmentMasteryPercentage)),
                        AverageNullable(
                            rows.Select(
                                x => x.PracticeMasteryPercentage)),
                        AverageNullable(
                            rows.Select(
                                x => (decimal?)x.CoveragePercentage)) ??
                            0m,
                        studentMap.Count,
                        affectedStudentIds,
                        rows.Count(x =>
                            x.HighestPriority ==
                            EvaluationPriority.Critical),
                        AggregateTrend(
                            rows.Select(x => x.Trend)),
                        rows);
                })
                .OrderByDescending(x =>
                    x.CriticalSkillCount)
                .ThenBy(x =>
                    x.ClassMasteryPercentage ?? 101m)
                .ThenBy(x => x.TopicName)
                .ToArray();

            return AnalyticsQueryResult<AnalyticsTopicSkillEvaluationPage>.Success(
                new AnalyticsTopicSkillEvaluationPage(
                    academicYearId,
                    year.Name,
                    classGroupId,
                    classGroup.Name,
                    subjectId,
                    subject.Name,
                    topicRows));
        }
        catch (InvalidOperationException)
        {
            return AnalyticsQueryResult<AnalyticsTopicSkillEvaluationPage>.Failure(
                AnalyticsErrorCode.InvalidSourceData);
        }
    }

    public async Task<AnalyticsQueryResult<AnalyticsStudentReport>>
        GetStudentReportAsync(
            Guid actorUserId,
            Guid studentProfileId,
            Guid academicYearId,
            Guid classGroupId,
            Guid subjectId,
            CancellationToken cancellationToken = default)
    {
        var access = await GetDashboardAsync(
            actorUserId,
            academicYearId,
            classGroupId,
            subjectId,
            cancellationToken);
        if (access.Value is null)
            return AnalyticsQueryResult<AnalyticsStudentReport>.Failure(
                access.Error ?? AnalyticsErrorCode.AccessDenied);

        var scope = await ResolveScopeAsync(actorUserId, cancellationToken);
        if (!scope.Succeeded)
            return AnalyticsQueryResult<AnalyticsStudentReport>.Failure(
                scope.Error!.Value);

        var projection = await _analytics.GetProjectionSnapshotAsync(
            scope.School!.Id,
            cancellationToken);

        var student = projection.StudentProfiles.SingleOrDefault(x =>
            x.Id == studentProfileId &&
            !x.IsArchived &&
            x.Status == AcademicStructureStatus.Active);
        var enrolled = projection.StudentEnrollments.Any(x =>
            x.StudentProfileId == studentProfileId &&
            x.AcademicYearId == academicYearId &&
            x.ClassGroupId == classGroupId);
        if (student is null || !enrolled)
            return AnalyticsQueryResult<AnalyticsStudentReport>.Failure(
                AnalyticsErrorCode.AccessDenied);

        var year = projection.AcademicYears.SingleOrDefault(x => x.Id == academicYearId);
        var classGroup = projection.ClassGroups.SingleOrDefault(x => x.Id == classGroupId);
        var subject = projection.Subjects.SingleOrDefault(x => x.Id == subjectId);
        if (year is null || classGroup is null || subject is null)
            return AnalyticsQueryResult<AnalyticsStudentReport>.Failure(
                AnalyticsErrorCode.InvalidSourceData);

        var outcomesById = projection.LearningOutcomes.ToDictionary(x => x.Id);
        var lessonsById = projection.PedagogicalLessons.ToDictionary(x => x.Id);
        var assessmentsById = projection.Assessments
            .Where(x =>
                x.Status != AssessmentStatus.Draft &&
                x.AcademicYearId == academicYearId &&
                x.ClassGroupId == classGroupId &&
                x.SubjectId == subjectId)
            .ToDictionary(x => x.Id);
        var resultsById = projection.AssessmentResults
            .Where(x =>
                x.StudentProfileId == studentProfileId &&
                assessmentsById.ContainsKey(x.AssessmentId))
            .ToDictionary(x => x.Id);
        var questionsById = projection.AssessmentQuestions
            .Where(x => assessmentsById.ContainsKey(x.AssessmentId))
            .ToDictionary(x => x.Id);
        var itemsById = projection.AssessmentItems
            .Where(x =>
                x.CurriculumPedagogicalLessonId.HasValue &&
                questionsById.ContainsKey(x.Id))
            .ToDictionary(x => x.Id);

        var lessonRows = new List<LessonEvidenceRow>();
        foreach (var answer in projection.StudentAnswers)
        {
            if (!resultsById.TryGetValue(answer.AssessmentResultId, out var result) ||
                !assessmentsById.TryGetValue(result.AssessmentId, out var assessment) ||
                !questionsById.TryGetValue(answer.AssessmentQuestionId, out var question) ||
                question.AssessmentId != assessment.Id ||
                !itemsById.TryGetValue(question.Id, out var item) ||
                !item.CurriculumPedagogicalLessonId.HasValue ||
                !lessonsById.ContainsKey(item.CurriculumPedagogicalLessonId.Value) ||
                question.MaxScore <= 0m)
            {
                continue;
            }

            lessonRows.Add(new LessonEvidenceRow(
                academicYearId,
                classGroupId,
                subjectId,
                item.CurriculumPedagogicalLessonId.Value,
                assessment.Id,
                studentProfileId,
                answer.Score,
                question.MaxScore));
        }

        var lessonItems = lessonRows
            .GroupBy(x => x.LessonId)
            .Select(group =>
            {
                var lesson = lessonsById[group.Key];
                var possible = group.Sum(x => x.PossibleScore);
                var mastery = possible <= 0m
                    ? 0m
                    : decimal.Round(
                        group.Sum(x => x.EarnedScore) / possible * 100m,
                        2,
                        MidpointRounding.AwayFromZero);
                return new AnalyticsStudentLessonItem(
                    lesson.Id,
                    lesson.Title,
                    lesson.UnitTitle,
                    mastery,
                    group.Count(),
                    group.Select(x => x.AssessmentId).Distinct().Count(),
                    AnalyticsProjectionBuilder.BandFor(mastery));
            })
            .OrderBy(x => x.MasteryPercentage)
            .ThenBy(x => x.LessonTitle)
            .ToArray();

        var studentOutcomeMasteries = projection.StudentOutcomeMasteries
            .Where(x =>
                x.StudentProfileId == studentProfileId &&
                x.AcademicYearId == academicYearId &&
                x.ClassGroupId == classGroupId &&
                x.SubjectId == subjectId &&
                outcomesById.ContainsKey(x.LearningOutcomeId))
            .ToArray();

        var outcomeItems = studentOutcomeMasteries
            .Select(x =>
            {
                var outcome = outcomesById[x.LearningOutcomeId];
                var displayCode =
                    AnalyticsPresentationFormatter.OutcomeLabel(outcome.Code);
                var displayDescription =
                    AnalyticsPresentationFormatter.OutcomeDescription(
                        outcome.Description,
                        displayCode,
                        subject.Name);

                return new AnalyticsStudentOutcomeItem(
                    outcome.Id,
                    displayCode,
                    displayDescription,
                    x.MasteryPercentage,
                    x.EvidenceCount,
                    AnalyticsProjectionBuilder.BandFor(x.MasteryPercentage));
            })
            .OrderBy(x => x.MasteryPercentage)
            .ThenBy(x => x.OutcomeCode)
            .ToArray();

        decimal? officialMastery = null;
        var officialPossible = studentOutcomeMasteries.Sum(x => x.PossibleScore);
        if (officialPossible > 0m)
        {
            officialMastery = decimal.Round(
                studentOutcomeMasteries.Sum(x => x.EarnedScore) /
                officialPossible *
                100m,
                2,
                MidpointRounding.AwayFromZero);
        }

        decimal? lessonMastery = null;
        var lessonPossible = lessonRows.Sum(x => x.PossibleScore);
        if (lessonPossible > 0m)
        {
            lessonMastery = decimal.Round(
                lessonRows.Sum(x => x.EarnedScore) /
                lessonPossible *
                100m,
                2,
                MidpointRounding.AwayFromZero);
        }

        return AnalyticsQueryResult<AnalyticsStudentReport>.Success(
            new AnalyticsStudentReport(
                student.Id,
                student.StudentNumber,
                student.DisplayName,
                year.Id,
                year.Name,
                classGroup.Id,
                classGroup.Name,
                subject.Id,
                subject.Name,
                officialMastery,
                lessonMastery,
                lessonItems,
                outcomeItems));
    }

    private static decimal? AverageNullable(
        IEnumerable<decimal?> values)
    {
        var present = values
            .Where(x => x.HasValue)
            .Select(x => x!.Value)
            .ToArray();

        return present.Length == 0
            ? null
            : Round2(present.Average());
    }

    private static decimal Round2(decimal value) =>
        decimal.Round(
            value,
            2,
            MidpointRounding.AwayFromZero);

    private static EvaluationTrendBand AggregateTrend(
        IEnumerable<EvaluationTrendBand> values)
    {
        var scored = values
            .Where(x =>
                x !=
                EvaluationTrendBand.InsufficientEvidence)
            .Select(x =>
                x switch
                {
                    EvaluationTrendBand.RapidlyDeclining => -2m,
                    EvaluationTrendBand.Declining => -1m,
                    EvaluationTrendBand.Improving => 1m,
                    EvaluationTrendBand.RapidlyImproving => 2m,
                    _ => 0m
                })
            .ToArray();

        if (scored.Length == 0)
            return EvaluationTrendBand.InsufficientEvidence;

        var average = scored.Average();

        return average switch
        {
            <= -1.2m => EvaluationTrendBand.RapidlyDeclining,
            <= -0.35m => EvaluationTrendBand.Declining,
            >= 1.2m => EvaluationTrendBand.RapidlyImproving,
            >= 0.35m => EvaluationTrendBand.Improving,
            _ => EvaluationTrendBand.Stable
        };
    }

    private async Task<ScopeResult> ResolveScopeAsync(
        Guid actorUserId,
        CancellationToken cancellationToken)
    {
        var actor =
            await _users.GetActorAsync(
                actorUserId,
                cancellationToken);

        if (actor is null ||
            !actor.IsActive ||
            actor.IsLocked ||
            !actor.SchoolId.HasValue)
        {
            return ScopeResult.Fail(
                AnalyticsErrorCode.AccessDenied);
        }

        var role =
            actor.Roles.Count == 1
                ? actor.Roles[0]
                : null;

        if (role != RoleNames.SchoolAdmin &&
            role != RoleNames.SubjectSupervisor &&
            role != RoleNames.Teacher)
        {
            return ScopeResult.Fail(
                AnalyticsErrorCode.AccessDenied);
        }

        var school =
            await _schools.GetByIdAsync(
                actor.SchoolId.Value,
                cancellationToken);

        if (school is null ||
            school.Status != SchoolStatus.Active)
        {
            return ScopeResult.Fail(
                AnalyticsErrorCode.SchoolNotActive);
        }

        IReadOnlySet<Guid> supervisedSubjectIds =
            new HashSet<Guid>();

        if (role == RoleNames.SubjectSupervisor)
        {
            if (_subjectSupervisors is null)
            {
                return ScopeResult.Fail(
                    AnalyticsErrorCode.AccessDenied);
            }

            var assignments =
                await _subjectSupervisors
                    .ListActiveBySupervisorAsync(
                        school.Id,
                        actorUserId,
                        cancellationToken);

            supervisedSubjectIds = assignments
                .Select(x => x.SubjectId)
                .ToHashSet();

            if (supervisedSubjectIds.Count == 0)
            {
                return ScopeResult.Fail(
                    AnalyticsErrorCode.AccessDenied);
            }
        }

        return ScopeResult.Ok(
            actor,
            school,
            role,
            supervisedSubjectIds);
    }

    private sealed record LessonEvidenceRow(
        Guid AcademicYearId,
        Guid ClassGroupId,
        Guid SubjectId,
        Guid LessonId,
        Guid AssessmentId,
        Guid StudentProfileId,
        decimal EarnedScore,
        decimal PossibleScore);

    private sealed record ScopeResult(
        bool Succeeded,
        SchoolUserRecord? Actor,
        School? School,
        string? Role,
        IReadOnlySet<Guid> SupervisedSubjectIds,
        AnalyticsErrorCode? Error)
    {
        public static ScopeResult Ok(
            SchoolUserRecord actor,
            School school,
            string role,
            IReadOnlySet<Guid> supervisedSubjectIds) =>
            new(
                true,
                actor,
                school,
                role,
                supervisedSubjectIds,
                null);

        public static ScopeResult Fail(
            AnalyticsErrorCode error) =>
            new(
                false,
                null,
                null,
                null,
                new HashSet<Guid>(),
                error);
    }
}

using System.Text.Json;
using Edulytics.Core.Assessments;
using Edulytics.Core.Constants;
using Edulytics.Core.Entities;
using Edulytics.Core.Enums;
using Edulytics.Core.Interfaces;
using Edulytics.Core.Realtime;
using Edulytics.Core.Users;
using Edulytics.Services.Auditing;

namespace Edulytics.Services.Assessments;

public sealed partial class AssessmentService : IAssessmentService
{
    private readonly IAssessmentRepository _repo;
    private readonly ISchoolRepository _schools;
    private readonly ISchoolUserRepository _users;
    private readonly IAuditService? _audit;
    private readonly IAssessmentBuilderRepository? _builder;

    public AssessmentService(
        IAssessmentRepository repo,
        ISchoolRepository schools,
        ISchoolUserRepository users,
        IAuditService? audit = null,
        IAssessmentBuilderRepository? builder = null)
    {
        _repo = repo;
        _schools = schools;
        _users = users;
        _audit = audit;
        _builder = builder;
    }

    public async Task<AssessmentQueryResult<AssessmentWorkspace>> GetWorkspaceAsync(
        Guid actorUserId,
        CancellationToken cancellationToken = default)
    {
        var scope = await ResolveScopeAsync(actorUserId, cancellationToken);
        if (!scope.Succeeded)
            return AssessmentQueryResult<AssessmentWorkspace>.Failure(scope.Error!.Value);

        var snapshot = await _repo.GetSnapshotAsync(scope.School!.Id, cancellationToken);
        IEnumerable<Assessment> visible = snapshot.Assessments;

        if (scope.Role == RoleNames.Teacher)
        {
            var pairs = snapshot.TeacherAssignments
                .Where(x => x.TeacherUserId == actorUserId)
                .Select(x => (x.ClassGroupId, x.SubjectId))
                .ToHashSet();

            visible = visible.Where(x => pairs.Contains((x.ClassGroupId, x.SubjectId)));
        }

        return AssessmentQueryResult<AssessmentWorkspace>.Success(
            BuildWorkspace(scope, snapshot, visible));
    }

    public async Task<AssessmentQueryResult<AssessmentDetails>> GetDetailsAsync(
        Guid actorUserId,
        Guid assessmentId,
        CancellationToken cancellationToken = default)
    {
        var scope = await ResolveScopeAsync(actorUserId, cancellationToken);
        if (!scope.Succeeded)
            return AssessmentQueryResult<AssessmentDetails>.Failure(scope.Error!.Value);

        var snapshot = await _repo.GetSnapshotAsync(scope.School!.Id, cancellationToken);
        var assessment = snapshot.Assessments.FirstOrDefault(x => x.Id == assessmentId);

        if (assessment is null)
            return AssessmentQueryResult<AssessmentDetails>.Failure(AssessmentErrorCode.AssessmentNotFound);

        if (!CanManage(scope, snapshot, assessment))
            return AssessmentQueryResult<AssessmentDetails>.Failure(AssessmentErrorCode.AccessDenied);

        var classGroup = snapshot.ClassGroups.FirstOrDefault(x => x.Id == assessment.ClassGroupId);
        if (classGroup is null)
            return AssessmentQueryResult<AssessmentDetails>.Failure(AssessmentErrorCode.ClassGroupNotFound);

        var eligibleFrameworkVersionIds =
            ResolveEligibleFrameworkVersionIds(
                snapshot,
                assessment.AcademicYearId,
                classGroup.GradeLevelId,
                assessment.SubjectId,
                classGroup.AcademicProgramId);

        var outcomes = snapshot.LearningOutcomes
            .Where(x =>
                x.AcademicProgramId == classGroup.AcademicProgramId &&
                x.SubjectId == assessment.SubjectId &&
                x.GradeLevelId == classGroup.GradeLevelId &&
                eligibleFrameworkVersionIds.Contains(x.FrameworkVersionId))
            .OrderBy(x => x.Code)
            .Select(x => new AssessmentOutcomeItem(x.Id, x.Code, x.Description))
            .ToArray();

        var workspace = BuildWorkspace(scope, snapshot, [assessment]);

        return AssessmentQueryResult<AssessmentDetails>.Success(
            new AssessmentDetails(
                MapAssessment(assessment),
                BuildQuestions(snapshot, assessment.Id),
                outcomes,
                workspace.ClassGroups,
                workspace.Subjects,
                workspace.Terms));
    }

    public async Task<AssessmentQueryResult<AssessmentQuestionItem>> GetQuestionAsync(
        Guid actorUserId,
        Guid questionId,
        CancellationToken cancellationToken = default)
    {
        var scope = await ResolveScopeAsync(actorUserId, cancellationToken);
        if (!scope.Succeeded)
            return AssessmentQueryResult<AssessmentQuestionItem>.Failure(scope.Error!.Value);

        var snapshot = await _repo.GetSnapshotAsync(scope.School!.Id, cancellationToken);
        var question = snapshot.Questions.FirstOrDefault(x => x.Id == questionId);

        if (question is null)
            return AssessmentQueryResult<AssessmentQuestionItem>.Failure(AssessmentErrorCode.QuestionNotFound);

        var assessment = snapshot.Assessments.FirstOrDefault(x => x.Id == question.AssessmentId);

        if (assessment is null)
            return AssessmentQueryResult<AssessmentQuestionItem>.Failure(AssessmentErrorCode.AssessmentNotFound);

        if (!CanManage(scope, snapshot, assessment))
            return AssessmentQueryResult<AssessmentQuestionItem>.Failure(AssessmentErrorCode.AccessDenied);

        return AssessmentQueryResult<AssessmentQuestionItem>.Success(MapQuestion(snapshot, question));
    }

    public async Task<AssessmentQueryResult<AssessmentResultsWorkspace>> GetResultsAsync(
        Guid actorUserId,
        Guid assessmentId,
        CancellationToken cancellationToken = default)
    {
        var scope = await ResolveScopeAsync(actorUserId, cancellationToken);
        if (!scope.Succeeded)
            return AssessmentQueryResult<AssessmentResultsWorkspace>.Failure(scope.Error!.Value);

        var snapshot = await _repo.GetSnapshotAsync(scope.School!.Id, cancellationToken);
        var assessment = snapshot.Assessments.FirstOrDefault(x => x.Id == assessmentId);

        if (assessment is null)
            return AssessmentQueryResult<AssessmentResultsWorkspace>.Failure(AssessmentErrorCode.AssessmentNotFound);

        if (!CanManage(scope, snapshot, assessment))
            return AssessmentQueryResult<AssessmentResultsWorkspace>.Failure(AssessmentErrorCode.AccessDenied);

        if (assessment.Status == AssessmentStatus.Draft)
            return AssessmentQueryResult<AssessmentResultsWorkspace>.Failure(AssessmentErrorCode.AssessmentNotOpen);

        IReadOnlyList<AssessmentQuestionItem> questions = BuildQuestions(snapshot, assessment.Id);

        if (_builder is not null)
        {
            var builderContext = await _builder.GetContextAsync(
                scope.School.Id,
                assessment.Id,
                cancellationToken);

            if (builderContext is not null)
            {
                var expectedAnswers = builderContext.Items
                    .Where(x => !string.IsNullOrWhiteSpace(x.CorrectAnswer))
                    .ToDictionary(x => x.Id, x => x.CorrectAnswer.Trim());

                questions = questions
                    .Select(question =>
                        expectedAnswers.TryGetValue(question.Id, out var expected)
                            ? question with { CorrectAnswer = expected }
                            : question)
                    .ToArray();
            }
        }

        var studentIds = snapshot.StudentEnrollments
            .Where(x => x.AcademicYearId == assessment.AcademicYearId &&
                        x.ClassGroupId == assessment.ClassGroupId)
            .Select(x => x.StudentProfileId)
            .ToHashSet();

        if (assessment.TargetType == AssessmentTargetType.Student)
        {
            if (assessment.TargetStudentProfileId.HasValue)
                studentIds.RemoveWhere(x => x != assessment.TargetStudentProfileId.Value);
            else
                studentIds.Clear();
        }

        var students = snapshot.StudentProfiles
            .Where(x => studentIds.Contains(x.Id) &&
                        x.Status == AcademicStructureStatus.Active)
            .OrderBy(x => x.DisplayName)
            .Select(profile =>
            {
                var result = snapshot.Results.FirstOrDefault(
                    x => x.AssessmentId == assessment.Id &&
                         x.StudentProfileId == profile.Id);

                var answerRows = result is null
                    ? Array.Empty<StudentAnswer>()
                    : snapshot.StudentAnswers
                        .Where(x => x.AssessmentResultId == result.Id)
                        .ToArray();

                IReadOnlyDictionary<Guid, decimal> scores = answerRows
                    .ToDictionary(x => x.AssessmentQuestionId, x => x.Score);

                IReadOnlyDictionary<Guid, string> responses = answerRows
                    .Where(x => x.ResponseText is not null)
                    .ToDictionary(
                        x => x.AssessmentQuestionId,
                        x => x.ResponseText ?? string.Empty);

                return new AssessmentStudentResultItem(
                    profile.Id,
                    profile.StudentNumber,
                    profile.DisplayName,
                    result?.Id,
                    result?.Score ?? 0m,
                    result?.Percentage ?? 0m,
                    result?.RowVersion,
                    scores)
                {
                    QuestionResponses = responses
                };
            })
            .ToArray();

        return AssessmentQueryResult<AssessmentResultsWorkspace>.Success(
            new AssessmentResultsWorkspace(
                MapAssessment(assessment),
                questions,
                students));
    }
}

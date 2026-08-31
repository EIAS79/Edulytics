using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
using Edulytics.Core.Enums;
using Edulytics.Core.Interfaces;
using Edulytics.Core.Users;

namespace Edulytics.Services.Curriculum;

public enum ExplicitCurriculumLevelErrorCode
{
    AccessDenied = 1,
    SchoolNotActive = 2,
    Required = 3,
    InvalidName = 4,
    InvalidOrder = 5,
    AcademicYearNotFound = 6,
    AcademicProgramNotFound = 7,
    AcademicProgramNotOffered = 8,
    CurriculumLevelNotFound = 9,
    CurriculumLevelProgramMismatch = 10,
    DuplicateCurriculumAdoption = 11,
    CurriculumAdoptionNotFound = 12,
    ClassGroupNotFound = 13,
    DuplicateClassName = 14,
    InvalidTeacher = 15,
    DuplicateTeacherAssignment = 16,
    DuplicateTopicName = 17,
    DuplicateTopicOrder = 18,
    TopicNotFound = 19,
    OfficialOutcomeNotFound = 20,
    DuplicateOutcomeCode = 21,
    DuplicateOutcomeOrder = 22,
    PersistenceError = 23
}

public sealed record ExplicitCurriculumLevelCommandResult(
    bool Succeeded,
    string Field,
    ExplicitCurriculumLevelErrorCode? Error)
{
    public static ExplicitCurriculumLevelCommandResult Success() =>
        new(true, string.Empty, null);

    public static ExplicitCurriculumLevelCommandResult Failure(
        string field,
        ExplicitCurriculumLevelErrorCode error) =>
        new(false, field, error);
}

public sealed record ExplicitCurriculumLevelQueryResult<T>(
    T? Value,
    ExplicitCurriculumLevelErrorCode? Error)
{
    public static ExplicitCurriculumLevelQueryResult<T> Success(T value) =>
        new(value, null);

    public static ExplicitCurriculumLevelQueryResult<T> Failure(
        ExplicitCurriculumLevelErrorCode error) =>
        new(default, error);
}

public sealed record ExplicitCurriculumLevelOption(
    Guid AcademicProgramId,
    string AcademicProgramName,
    string AcademicProgramCode,
    string PackCode,
    string LevelKey,
    int LogicalLevel,
    string Label,
    string Stage,
    string? Pathway,
    string DisplayLabel);

public sealed record ExplicitCurriculumAdoptionItem(
    Guid Id,
    Guid AcademicYearId,
    string AcademicYearName,
    Guid AcademicProgramId,
    string AcademicProgramName,
    string AcademicProgramCode,
    Guid SubjectId,
    Guid FrameworkVersionId,
    string FrameworkCode,
    string LevelKey,
    int LogicalLevel,
    string Label,
    string Stage,
    string? Pathway);

public sealed record ExplicitCurriculumLevelDashboard(
    IReadOnlyList<ExplicitCurriculumLevelOption> AvailableLevels,
    IReadOnlyList<ExplicitCurriculumAdoptionItem> Adoptions);

public sealed record AdoptExplicitCurriculumLevelRequest(
    Guid AcademicYearId,
    Guid AcademicProgramId,
    string CurriculumLevelKey);

public sealed record CreateClassForCurriculumLevelRequest(
    Guid AcademicYearId,
    Guid CurriculumAdoptionId,
    string Name,
    AcademicStructureStatus Status);

public sealed record AssignTeacherToCurriculumClassRequest(
    Guid TeacherUserId,
    Guid ClassGroupId);

public sealed record CreateTopicForCurriculumLevelRequest(
    Guid CurriculumAdoptionId,
    string Name,
    int Order);

public sealed record CreateOfficialOutcomeForCurriculumLevelRequest(
    Guid TopicId,
    Guid ContentNodeId,
    Guid? LessonNodeId,
    int Order);

public interface IExplicitCurriculumLevelService
{
    Task<ExplicitCurriculumLevelQueryResult<ExplicitCurriculumLevelDashboard>>
        GetDashboardAsync(
            Guid actorUserId,
            CancellationToken cancellationToken = default);

    Task<ExplicitCurriculumLevelCommandResult> AdoptLevelAsync(
        Guid actorUserId,
        AdoptExplicitCurriculumLevelRequest request,
        CancellationToken cancellationToken = default);

    Task<ExplicitCurriculumLevelCommandResult> CreateClassAsync(
        Guid actorUserId,
        CreateClassForCurriculumLevelRequest request,
        CancellationToken cancellationToken = default);

    Task<ExplicitCurriculumLevelCommandResult> AssignTeacherAsync(
        Guid actorUserId,
        AssignTeacherToCurriculumClassRequest request,
        CancellationToken cancellationToken = default);

    Task<ExplicitCurriculumLevelCommandResult> CreateTopicAsync(
        Guid actorUserId,
        CreateTopicForCurriculumLevelRequest request,
        CancellationToken cancellationToken = default);

    Task<ExplicitCurriculumLevelCommandResult> CreateOfficialOutcomeAsync(
        Guid actorUserId,
        CreateOfficialOutcomeForCurriculumLevelRequest request,
        CancellationToken cancellationToken = default);
}

public sealed class ExplicitCurriculumLevelService : IExplicitCurriculumLevelService
{
    private const string MathematicsCode = "MATH";
    private const string MathematicsName = "Mathematics";

    private readonly ICurriculumRepository _curriculum;
    private readonly IAcademicStructureRepository _academic;
    private readonly ISchoolRepository _schools;
    private readonly ISchoolUserRepository _users;

    public ExplicitCurriculumLevelService(
        ICurriculumRepository curriculum,
        IAcademicStructureRepository academic,
        ISchoolRepository schools,
        ISchoolUserRepository users)
    {
        _curriculum = curriculum;
        _academic = academic;
        _schools = schools;
        _users = users;
    }

    public async Task<ExplicitCurriculumLevelQueryResult<ExplicitCurriculumLevelDashboard>>
        GetDashboardAsync(
            Guid actorUserId,
            CancellationToken cancellationToken = default)
    {
        var scope = await ResolveScopeAsync(actorUserId, cancellationToken);
        if (!scope.Succeeded)
            return ExplicitCurriculumLevelQueryResult<ExplicitCurriculumLevelDashboard>
                .Failure(scope.Error!.Value);

        var snapshot = await _curriculum.GetSnapshotAsync(
            scope.School!.Id,
            cancellationToken);
        var contexts = await _curriculum.GetAdoptedCurriculumContextsAsync(
            scope.School.Id,
            cancellationToken);

        var options = snapshot.AcademicPrograms
            .Where(x =>
                !x.IsDefault &&
                x.Status == AcademicStructureStatus.Active)
            .SelectMany(program =>
            {
                var packCode = CurriculumLevelIdentityRegistry
                    .PackCodeForProgramCode(program.Code);
                if (packCode is null)
                    return Array.Empty<ExplicitCurriculumLevelOption>();

                return CurriculumLevelIdentityRegistry
                    .ForPack(packCode)
                    .Select(level => new ExplicitCurriculumLevelOption(
                        program.Id,
                        program.Name,
                        program.Code,
                        level.PackCode,
                        level.Key,
                        level.LogicalLevel,
                        level.Label,
                        level.Stage,
                        level.Pathway,
                        level.DisplayLabel))
                    .ToArray();
            })
            .OrderBy(x => x.AcademicProgramName)
            .ThenBy(x => x.LogicalLevel)
            .ThenBy(x => x.Pathway ?? string.Empty)
            .ToArray();

        var yearNames = snapshot.AcademicYears.ToDictionary(x => x.Id, x => x.Name);
        var adoptions = contexts
            .Where(x =>
                x.AdoptionId != Guid.Empty &&
                x.AcademicYearId.HasValue &&
                !string.IsNullOrWhiteSpace(x.CurriculumLevelKey) &&
                x.CurriculumLogicalLevel.HasValue)
            .Select(x => new ExplicitCurriculumAdoptionItem(
                x.AdoptionId,
                x.AcademicYearId!.Value,
                yearNames.GetValueOrDefault(x.AcademicYearId.Value) ?? string.Empty,
                x.AcademicProgramId,
                x.AcademicProgramName,
                x.AcademicProgramCode,
                x.SubjectId,
                x.FrameworkVersionId,
                x.FrameworkCode,
                x.CurriculumLevelKey!,
                x.CurriculumLogicalLevel!.Value,
                x.CurriculumLevelLabel ?? string.Empty,
                x.CurriculumStage ?? string.Empty,
                x.CurriculumPathway))
            .OrderByDescending(x => x.AcademicYearName)
            .ThenBy(x => x.AcademicProgramName)
            .ThenBy(x => x.LogicalLevel)
            .ThenBy(x => x.Pathway ?? string.Empty)
            .ToArray();

        return ExplicitCurriculumLevelQueryResult<ExplicitCurriculumLevelDashboard>
            .Success(new ExplicitCurriculumLevelDashboard(options, adoptions));
    }

    public async Task<ExplicitCurriculumLevelCommandResult> AdoptLevelAsync(
        Guid actorUserId,
        AdoptExplicitCurriculumLevelRequest request,
        CancellationToken cancellationToken = default)
    {
        var scope = await ResolveSupervisorScopeAsync(actorUserId, cancellationToken);
        if (!scope.Succeeded)
            return Fail(scope.Error!.Value);

        var schoolId = scope.School!.Id;
        var snapshot = await _curriculum.GetSnapshotAsync(schoolId, cancellationToken);
        var year = snapshot.AcademicYears.SingleOrDefault(x => x.Id == request.AcademicYearId);
        if (year is null)
            return Fail(nameof(request.AcademicYearId), ExplicitCurriculumLevelErrorCode.AcademicYearNotFound);

        var program = snapshot.AcademicPrograms.SingleOrDefault(x =>
            x.Id == request.AcademicProgramId &&
            !x.IsDefault &&
            x.Status == AcademicStructureStatus.Active);
        if (program is null)
            return Fail(nameof(request.AcademicProgramId), ExplicitCurriculumLevelErrorCode.AcademicProgramNotFound);

        if (!snapshot.AcademicYearProgramOfferings.Any(x =>
                x.AcademicYearId == year.Id &&
                x.AcademicProgramId == program.Id &&
                x.IsOffered))
        {
            return Fail(nameof(request.AcademicProgramId), ExplicitCurriculumLevelErrorCode.AcademicProgramNotOffered);
        }

        var level = CurriculumLevelIdentityRegistry.Find(request.CurriculumLevelKey);
        if (level is null)
            return Fail(nameof(request.CurriculumLevelKey), ExplicitCurriculumLevelErrorCode.CurriculumLevelNotFound);

        var expectedPackCode = CurriculumLevelIdentityRegistry.PackCodeForProgramCode(program.Code);
        if (!string.Equals(expectedPackCode, level.PackCode, StringComparison.Ordinal))
        {
            return Fail(
                nameof(request.CurriculumLevelKey),
                ExplicitCurriculumLevelErrorCode.CurriculumLevelProgramMismatch);
        }

        var frameworkVersionId = await _curriculum.GetActivePlatformFrameworkVersionIdAsync(
            level.PackCode,
            cancellationToken);
        if (!frameworkVersionId.HasValue)
            return Fail(nameof(request.CurriculumLevelKey), ExplicitCurriculumLevelErrorCode.CurriculumLevelNotFound);

        var subject = snapshot.Subjects.SingleOrDefault(x =>
            string.Equals(x.NormalizedCode, MathematicsCode, StringComparison.Ordinal) ||
            string.Equals(x.Code, MathematicsCode, StringComparison.OrdinalIgnoreCase));

        if (subject is null)
        {
            subject = new Subject
            {
                Id = Guid.NewGuid(),
                SchoolId = schoolId,
                Name = MathematicsName,
                Code = MathematicsCode,
                NormalizedCode = MathematicsCode,
                Status = AcademicStructureStatus.Active,
                RowVersion = []
            };
            await _academic.AddAsync(subject, cancellationToken);
        }

        var grade = ResolveCompatibilityGrade(snapshot.GradeLevels, level);
        if (grade is null)
        {
            grade = new GradeLevel
            {
                Id = Guid.NewGuid(),
                SchoolId = schoolId,
                Name = CompatibilityGradeName(level),
                Order = level.LogicalLevel
            };
            await _academic.AddAsync(grade, cancellationToken);
        }

        if (snapshot.CurriculumAdoptions.Any(x =>
                x.AcademicYearId == year.Id &&
                x.AcademicProgramId == program.Id &&
                x.SubjectId == subject.Id &&
                string.Equals(x.CurriculumLevelKey, level.Key, StringComparison.Ordinal)))
        {
            return Fail(
                nameof(request.CurriculumLevelKey),
                ExplicitCurriculumLevelErrorCode.DuplicateCurriculumAdoption);
        }

        var now = DateTime.UtcNow;
        var adoption = new SchoolCurriculumAdoption
        {
            Id = Guid.NewGuid(),
            SchoolId = schoolId,
            AcademicYearId = year.Id,
            AcademicProgramId = program.Id,
            GradeLevelId = grade.Id,
            SubjectId = subject.Id,
            FrameworkVersionId = frameworkVersionId.Value,
            CurriculumLevelKey = level.Key,
            CurriculumLogicalLevel = level.LogicalLevel,
            CurriculumLevelLabel = level.Label,
            CurriculumStage = level.Stage,
            CurriculumPathway = level.Pathway,
            IsPrimary = true,
            IsActive = true,
            CreatedAtUtc = now,
            UpdatedAtUtc = now,
            RowVersion = []
        };

        await _academic.AddAsync(adoption, cancellationToken);
        return Map(await _academic.SaveAsync(cancellationToken));
    }

    public async Task<ExplicitCurriculumLevelCommandResult> CreateClassAsync(
        Guid actorUserId,
        CreateClassForCurriculumLevelRequest request,
        CancellationToken cancellationToken = default)
    {
        var scope = await ResolveSupervisorScopeAsync(actorUserId, cancellationToken);
        if (!scope.Succeeded)
            return Fail(scope.Error!.Value);

        var name = Clean(request.Name);
        if (name.Length == 0)
            return Fail(nameof(request.Name), ExplicitCurriculumLevelErrorCode.Required);
        if (name.Length > 150)
            return Fail(nameof(request.Name), ExplicitCurriculumLevelErrorCode.InvalidName);

        var schoolId = scope.School!.Id;
        var contexts = await _curriculum.GetAdoptedCurriculumContextsAsync(schoolId, cancellationToken);
        var adoption = contexts.SingleOrDefault(x =>
            x.AdoptionId == request.CurriculumAdoptionId &&
            x.AcademicYearId == request.AcademicYearId &&
            !string.IsNullOrWhiteSpace(x.CurriculumLevelKey));
        if (adoption is null)
            return Fail(nameof(request.CurriculumAdoptionId), ExplicitCurriculumLevelErrorCode.CurriculumAdoptionNotFound);

        var snapshot = await _academic.GetSnapshotAsync(schoolId, cancellationToken);
        if (!snapshot.AcademicYears.Any(x => x.Id == request.AcademicYearId))
            return Fail(nameof(request.AcademicYearId), ExplicitCurriculumLevelErrorCode.AcademicYearNotFound);

        var normalizedName = NormalizeName(name);
        if (snapshot.ClassGroups.Any(x =>
                x.AcademicYearId == request.AcademicYearId &&
                x.CurriculumAdoptionId == adoption.AdoptionId &&
                string.Equals(
                    x.NormalizedName ?? NormalizeName(x.Name),
                    normalizedName,
                    StringComparison.Ordinal)))
        {
            return Fail(nameof(request.Name), ExplicitCurriculumLevelErrorCode.DuplicateClassName);
        }

        var id = Guid.NewGuid();
        var code = $"CLS-{id:N}"[..16].ToUpperInvariant();
        var entity = new ClassGroup
        {
            Id = id,
            SchoolId = schoolId,
            AcademicYearId = request.AcademicYearId,
            AcademicProgramId = adoption.AcademicProgramId,
            GradeLevelId = adoption.GradeLevelId,
            CurriculumAdoptionId = adoption.AdoptionId,
            Name = name,
            NormalizedName = normalizedName,
            Code = code,
            NormalizedCode = code,
            Status = request.Status,
            RowVersion = []
        };

        await _academic.AddAsync(entity, cancellationToken);
        return Map(await _academic.SaveAsync(cancellationToken));
    }

    public async Task<ExplicitCurriculumLevelCommandResult> AssignTeacherAsync(
        Guid actorUserId,
        AssignTeacherToCurriculumClassRequest request,
        CancellationToken cancellationToken = default)
    {
        var scope = await ResolveSupervisorScopeAsync(actorUserId, cancellationToken);
        if (!scope.Succeeded)
            return Fail(scope.Error!.Value);

        var schoolId = scope.School!.Id;
        var teacher = await _users.GetBySchoolAndIdAsync(
            schoolId,
            request.TeacherUserId,
            cancellationToken);
        if (teacher is null ||
            !teacher.IsActive ||
            teacher.IsLocked ||
            SingleRole(teacher.Roles) != RoleNames.Teacher)
        {
            return Fail(nameof(request.TeacherUserId), ExplicitCurriculumLevelErrorCode.InvalidTeacher);
        }

        var classGroup = await _academic.GetClassGroupAsync(
            schoolId,
            request.ClassGroupId,
            cancellationToken);
        if (classGroup is null || !classGroup.CurriculumAdoptionId.HasValue)
            return Fail(nameof(request.ClassGroupId), ExplicitCurriculumLevelErrorCode.ClassGroupNotFound);

        var contexts = await _curriculum.GetAdoptedCurriculumContextsAsync(schoolId, cancellationToken);
        var adoption = contexts.SingleOrDefault(x =>
            x.AdoptionId == classGroup.CurriculumAdoptionId.Value);
        if (adoption is null)
            return Fail(nameof(request.ClassGroupId), ExplicitCurriculumLevelErrorCode.CurriculumAdoptionNotFound);

        if (await _academic.TeacherAssignmentExistsAsync(
                schoolId,
                teacher.Id,
                classGroup.Id,
                adoption.SubjectId,
                cancellationToken))
        {
            return Fail(string.Empty, ExplicitCurriculumLevelErrorCode.DuplicateTeacherAssignment);
        }

        var assignment = new TeacherAssignment
        {
            Id = Guid.NewGuid(),
            SchoolId = schoolId,
            TeacherUserId = teacher.Id,
            ClassGroupId = classGroup.Id,
            SubjectId = adoption.SubjectId,
            AcademicYearId = classGroup.AcademicYearId,
            CreatedAtUtc = DateTime.UtcNow
        };

        await _academic.AddAsync(assignment, cancellationToken);
        return Map(await _academic.SaveAsync(cancellationToken));
    }

    public async Task<ExplicitCurriculumLevelCommandResult> CreateTopicAsync(
        Guid actorUserId,
        CreateTopicForCurriculumLevelRequest request,
        CancellationToken cancellationToken = default)
    {
        var scope = await ResolveSupervisorScopeAsync(actorUserId, cancellationToken);
        if (!scope.Succeeded)
            return Fail(scope.Error!.Value);

        var name = Clean(request.Name);
        if (name.Length == 0)
            return Fail(nameof(request.Name), ExplicitCurriculumLevelErrorCode.Required);
        if (name.Length > 200)
            return Fail(nameof(request.Name), ExplicitCurriculumLevelErrorCode.InvalidName);
        if (request.Order <= 0)
            return Fail(nameof(request.Order), ExplicitCurriculumLevelErrorCode.InvalidOrder);

        var schoolId = scope.School!.Id;
        var contexts = await _curriculum.GetAdoptedCurriculumContextsAsync(schoolId, cancellationToken);
        var adoption = contexts.SingleOrDefault(x =>
            x.AdoptionId == request.CurriculumAdoptionId &&
            !string.IsNullOrWhiteSpace(x.CurriculumLevelKey));
        if (adoption is null)
            return Fail(nameof(request.CurriculumAdoptionId), ExplicitCurriculumLevelErrorCode.CurriculumAdoptionNotFound);

        var snapshot = await _curriculum.GetSnapshotAsync(schoolId, cancellationToken);
        if (snapshot.Topics.Any(x =>
                x.CurriculumAdoptionId == adoption.AdoptionId &&
                string.Equals(x.Name, name, StringComparison.OrdinalIgnoreCase)))
        {
            return Fail(nameof(request.Name), ExplicitCurriculumLevelErrorCode.DuplicateTopicName);
        }
        if (snapshot.Topics.Any(x =>
                x.CurriculumAdoptionId == adoption.AdoptionId &&
                x.Order == request.Order))
        {
            return Fail(nameof(request.Order), ExplicitCurriculumLevelErrorCode.DuplicateTopicOrder);
        }

        var topic = new CurriculumTopic
        {
            Id = Guid.NewGuid(),
            SchoolId = schoolId,
            AcademicProgramId = adoption.AcademicProgramId,
            FrameworkVersionId = adoption.FrameworkVersionId,
            SubjectId = adoption.SubjectId,
            GradeLevelId = adoption.GradeLevelId,
            CurriculumAdoptionId = adoption.AdoptionId,
            Name = name,
            Order = request.Order
        };

        await _curriculum.AddTopicAsync(topic, cancellationToken);
        return Map(await _curriculum.SaveAsync(cancellationToken));
    }

    public async Task<ExplicitCurriculumLevelCommandResult> CreateOfficialOutcomeAsync(
        Guid actorUserId,
        CreateOfficialOutcomeForCurriculumLevelRequest request,
        CancellationToken cancellationToken = default)
    {
        var scope = await ResolveSupervisorScopeAsync(actorUserId, cancellationToken);
        if (!scope.Succeeded)
            return Fail(scope.Error!.Value);
        if (request.Order <= 0)
            return Fail(nameof(request.Order), ExplicitCurriculumLevelErrorCode.InvalidOrder);

        var schoolId = scope.School!.Id;
        var topic = await _curriculum.GetTopicAsync(schoolId, request.TopicId, cancellationToken);
        if (topic is null || !topic.CurriculumAdoptionId.HasValue)
            return Fail(nameof(request.TopicId), ExplicitCurriculumLevelErrorCode.TopicNotFound);

        var contexts = await _curriculum.GetAdoptedCurriculumContextsAsync(schoolId, cancellationToken);
        var adoption = contexts.SingleOrDefault(x =>
            x.AdoptionId == topic.CurriculumAdoptionId.Value &&
            x.CurriculumLogicalLevel.HasValue);
        if (adoption is null)
            return Fail(nameof(request.TopicId), ExplicitCurriculumLevelErrorCode.CurriculumAdoptionNotFound);

        var source = await _curriculum.GetOfficialOutcomeSourceAsync(
            adoption.FrameworkVersionId,
            adoption.CurriculumLogicalLevel!.Value,
            adoption.CurriculumPathway,
            request.ContentNodeId,
            request.LessonNodeId,
            cancellationToken);
        if (source is null)
            return Fail(nameof(request.ContentNodeId), ExplicitCurriculumLevelErrorCode.OfficialOutcomeNotFound);

        var snapshot = await _curriculum.GetSnapshotAsync(schoolId, cancellationToken);
        if (snapshot.Outcomes.Any(x =>
                x.CurriculumAdoptionId == adoption.AdoptionId &&
                string.Equals(x.Code, source.Code, StringComparison.Ordinal)))
        {
            return Fail(nameof(request.ContentNodeId), ExplicitCurriculumLevelErrorCode.DuplicateOutcomeCode);
        }
        if (snapshot.Outcomes.Any(x => x.TopicId == topic.Id && x.Order == request.Order))
            return Fail(nameof(request.Order), ExplicitCurriculumLevelErrorCode.DuplicateOutcomeOrder);

        var outcome = new LearningOutcome
        {
            Id = Guid.NewGuid(),
            SchoolId = schoolId,
            AcademicProgramId = adoption.AcademicProgramId,
            FrameworkVersionId = adoption.FrameworkVersionId,
            SubjectId = adoption.SubjectId,
            GradeLevelId = adoption.GradeLevelId,
            CurriculumAdoptionId = adoption.AdoptionId,
            TopicId = topic.Id,
            OfficialContentNodeId = source.ContentNodeId,
            Code = source.Code,
            Description = source.Description,
            Weight = 1m,
            Order = request.Order
        };

        await _curriculum.AddOutcomeAsync(outcome, cancellationToken);
        return Map(await _curriculum.SaveAsync(cancellationToken));
    }

    private static GradeLevel? ResolveCompatibilityGrade(
        IReadOnlyList<GradeLevel> grades,
        CurriculumLevelIdentity level)
    {
        var exact = grades
            .Where(x => string.Equals(x.Name, level.Label, StringComparison.OrdinalIgnoreCase))
            .ToArray();
        if (exact.Length == 1)
            return exact[0];

        var sameOrder = grades.Where(x => x.Order == level.LogicalLevel).ToArray();
        return sameOrder.Length == 1 ? sameOrder[0] : null;
    }

    private static string CompatibilityGradeName(CurriculumLevelIdentity level)
    {
        var label = level.Label.Trim();
        if (label.Length <= 100)
            return label;

        return $"Curriculum level {level.LogicalLevel}";
    }

    private async Task<ScopeResult> ResolveSupervisorScopeAsync(
        Guid actorUserId,
        CancellationToken cancellationToken)
    {
        var scope = await ResolveScopeAsync(actorUserId, cancellationToken);
        if (!scope.Succeeded)
            return scope;
        return SingleRole(scope.Actor!.Roles) == RoleNames.SubjectSupervisor
            ? scope
            : ScopeResult.Fail(ExplicitCurriculumLevelErrorCode.AccessDenied);
    }

    private async Task<ScopeResult> ResolveScopeAsync(
        Guid actorUserId,
        CancellationToken cancellationToken)
    {
        var actor = await _users.GetActorAsync(actorUserId, cancellationToken);
        if (actor is null || !actor.IsActive || actor.IsLocked || !actor.SchoolId.HasValue)
            return ScopeResult.Fail(ExplicitCurriculumLevelErrorCode.AccessDenied);

        var school = await _schools.GetByIdAsync(actor.SchoolId.Value, cancellationToken);
        if (school is null || school.Status != SchoolStatus.Active)
            return ScopeResult.Fail(ExplicitCurriculumLevelErrorCode.SchoolNotActive);

        return ScopeResult.Success(actor, school);
    }

    private static string Clean(string? value) =>
        string.Join(
            " ",
            (value ?? string.Empty).Split(
                (char[]?)null,
                StringSplitOptions.RemoveEmptyEntries));

    private static string NormalizeName(string value) =>
        Clean(value).ToUpperInvariant();

    private static string? SingleRole(IEnumerable<string> roles)
    {
        var normalized = roles
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => x.Trim())
            .Distinct(StringComparer.Ordinal)
            .ToArray();
        return normalized.Length == 1 ? normalized[0] : null;
    }

    private static ExplicitCurriculumLevelCommandResult Fail(
        ExplicitCurriculumLevelErrorCode error) =>
        ExplicitCurriculumLevelCommandResult.Failure(string.Empty, error);

    private static ExplicitCurriculumLevelCommandResult Fail(
        string field,
        ExplicitCurriculumLevelErrorCode error) =>
        ExplicitCurriculumLevelCommandResult.Failure(field, error);

    private static ExplicitCurriculumLevelCommandResult Map(
        AcademicPersistenceResult result) =>
        result.Succeeded
            ? ExplicitCurriculumLevelCommandResult.Success()
            : Fail(ExplicitCurriculumLevelErrorCode.PersistenceError);

    private static ExplicitCurriculumLevelCommandResult Map(
        CurriculumPersistenceResult result) =>
        result.Succeeded
            ? ExplicitCurriculumLevelCommandResult.Success()
            : Fail(ExplicitCurriculumLevelErrorCode.PersistenceError);

    private sealed record ScopeResult(
        SchoolUserRecord? Actor,
        School? School,
        ExplicitCurriculumLevelErrorCode? Error)
    {
        public bool Succeeded => Actor is not null && School is not null && Error is null;
        public static ScopeResult Success(SchoolUserRecord actor, School school) =>
            new(actor, school, null);
        public static ScopeResult Fail(ExplicitCurriculumLevelErrorCode error) =>
            new(null, null, error);
    }
}

using System.ComponentModel.DataAnnotations;
using Edulytics.Services.Users;
using Edulytics.Services.StudentSetup;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Edulytics.Web.ViewModels.SchoolUsers;

public sealed class SchoolUserListViewModel
{
    public required SchoolUserManagementContext Context
    {
        get;
        init;
    }

    public IReadOnlyList<SchoolUserListItem> Users
    {
        get;
        init;
    } = [];

    public string? Search { get; init; }
    public string? Role { get; init; }
    public bool? IsActive { get; init; }
    public bool? IsLocked { get; init; }
    public string? Name { get; init; }
    public string? UserId { get; init; }
    public string? Email { get; init; }
    public Guid? AcademicYearId { get; init; }
    public Guid? AcademicProgramId { get; init; }
    public Guid? ClassGroupId { get; init; }
    public Edulytics.Core.Users.SchoolUserDirectoryFilterOptions FilterOptions { get; init; } =
        Edulytics.Core.Users.SchoolUserDirectoryFilterOptions.Empty;
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 50;
    public int TotalCount { get; init; }
    public int TotalPages { get; init; } = 1;
}

public sealed class SchoolUserCreateViewModel
{
    public Guid SchoolId { get; set; }

    [Required(ErrorMessage = "UserEmailRequired")]
    [EmailAddress(ErrorMessage = "UserEmailInvalid")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "UserRoleRequired")]
    public string Role { get; set; } = string.Empty;

    // These fields are required only for direct Student creation. They are
    // validated explicitly by DirectStudentCreationFilter so a Teacher create
    // request from a SubjectSupervisor is not rejected by MVC's implicit
    // non-nullable-string validation when the hidden Student fields are empty.
    [ValidateNever]
    public string StudentNumber { get; set; } = string.Empty;

    [ValidateNever]
    public string FirstName { get; set; } = string.Empty;

    [ValidateNever]
    public string LastName { get; set; } = string.Empty;

    public Guid? ClassGroupId { get; set; }

    public IReadOnlyList<StudentRoleClassOption>
        StudentClasses { get; set; } = [];

    public IReadOnlyList<SchoolUserRoleOptionViewModel>
        RoleOptions { get; set; } = [];
}

public sealed record SchoolUserRoleOptionViewModel(
    string Value,
    string ResourceKey);

public sealed class SchoolUserDetailsViewModel
{
    public required SchoolUserDetails User
    {
        get;
        init;
    }

    public IReadOnlyList<SchoolUserRoleOptionViewModel>
        RoleOptions { get; init; } = [];

    public StudentRoleProvisioningContext?
        StudentSetup { get; init; }
}

public sealed class SchoolHomeViewModel
{
    public required string SchoolName { get; init; }
    public required string Role { get; init; }
    public bool CanManageUsers { get; init; }
    public bool CanManageAssessments { get; init; }
    public bool CanViewAnalytics { get; init; }
    public bool CanViewReports { get; init; }
}

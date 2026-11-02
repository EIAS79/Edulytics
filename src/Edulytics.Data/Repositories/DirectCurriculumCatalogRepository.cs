using Edulytics.Core.DirectStudents;
using Edulytics.Core.Interfaces;
using Edulytics.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Edulytics.Data.Repositories;

public sealed class DirectCurriculumCatalogRepository
    : IDirectCurriculumCatalogRepository
{
    private readonly EdulyticsDbContext _db;

    public DirectCurriculumCatalogRepository(
        EdulyticsDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<DirectCurriculumFrameworkRecord>>
        ListActiveFrameworksAsync(
            CancellationToken cancellationToken = default) =>
        await (
            from framework in _db.CurriculumFrameworks.AsNoTracking()
            join version in _db.CurriculumFrameworkVersions.AsNoTracking()
                on framework.Id equals version.FrameworkId
            where framework.IsActive &&
                  framework.OwnerSchoolId == null &&
                  version.IsActive &&
                  _db.CurriculumPackContentNodes.Any(node =>
                      node.FrameworkVersionId == version.Id &&
                      node.IsActive &&
                      node.NodeKind == "Lesson")
            orderby framework.Name, version.Name
            select new DirectCurriculumFrameworkRecord(
                version.Id,
                framework.Code,
                framework.Name,
                version.Name,
                framework.CountryCode ?? string.Empty))
            .ToArrayAsync(cancellationToken);
}

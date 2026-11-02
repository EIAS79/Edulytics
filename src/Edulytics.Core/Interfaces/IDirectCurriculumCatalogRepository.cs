using Edulytics.Core.DirectStudents;

namespace Edulytics.Core.Interfaces;

public interface IDirectCurriculumCatalogRepository
{
    Task<IReadOnlyList<DirectCurriculumFrameworkRecord>>
        ListActiveFrameworksAsync(
            CancellationToken cancellationToken = default);
}
